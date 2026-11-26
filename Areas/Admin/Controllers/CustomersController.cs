using Đề_tài.App_Start;
using Đề_tài.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Đề_tài.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class CustomersController : Controller
    {
        private ChildStoreEntities db = new ChildStoreEntities();

        // GET: Admin/Customers
        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.User);
            return View(customers.ToList());
        }

        // GET: Admin/Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Admin/Customers/Edit/5
        // GET: Admin/Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            // Sửa lại: "Username" là giá trị, "Username" là text hiển thị (không dùng Password)
            ViewBag.Username = new SelectList(db.Users, "Username", "Username", customer.Username);
            return View(customer);
        }

        // POST: Admin/Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Admin/Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer model, string Password)
        {
            if (ModelState.IsValid)
            {
                // 1. Cập nhật thông tin Customer
                db.Entry(model).State = EntityState.Modified;

                // 2. Xử lý cập nhật mật khẩu (nếu có nhập)
                if (!string.IsNullOrEmpty(Password))
                {
                    // Tìm User tương ứng với Username của Customer này
                    var user = db.Users.FirstOrDefault(u => u.Username == model.Username);
                    if (user != null)
                    {
                        user.Password = Password;
                        db.Entry(user).State = EntityState.Modified;
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, phải load lại DropdownList trước khi trả về View
            ViewBag.Username = new SelectList(db.Users, "Username", "Username", model.Username);
            return View(model);
        }

        // GET: Admin/Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Admin/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            var user = db.Users.SingleOrDefault(u => u.Username == customer.Username);
            db.Customers.Remove(customer); //xóa customer trước vì nó là bảng con
            if (user != null) // xóa user sau vì nó là bảng cha
            {
                db.Users.Remove(user);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
