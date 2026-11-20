using Đề_tài.App_Start;
using Đề_tài.Models;
using Đề_tài.Models.ViewModel.Admin;
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
    public class UsersController : Controller
    {
        private ChildStoreEntities db = new ChildStoreEntities();

        // GET: Admin/Users
        //[Authorize]
        public ActionResult Index()
        {
            // Chỉ lấy những user có quyền Admin
            var admins = db.Users.Where(u => u.UserRole == "A").ToList();
            return View(admins);
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserVM model)
        {
            if (ModelState.IsValid)
            {
                // 1. Kiểm tra xem Username đã tồn tại chưa
                bool isExist = db.Users.Any(u => u.Username == model.Username);
                if (isExist)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã có người sử dụng!");
                    return View(model); // Trả về form kèm lỗi
                }

                // 2. Tạo User mới
                var newAdmin = new User
                {
                    Username = model.Username,
                    Password = model.Password, 
                    UserRole = "A" 
                };

                db.Users.Add(newAdmin);
                db.SaveChanges();

                TempData["Message"] = "Đã tạo tài khoản Admin mới thành công!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(string id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();

            var model = new UserVM
            {
                Username = user.Username,
                Password = user.Password // (Hoặc để trống nếu không muốn hiện mật khẩu cũ)
            };
            return View(model);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserVM model)
        {
            if (ModelState.IsValid)
            {
                // 1. Tìm User cũ trong DB
                var userDb = db.Users.Find(model.Username);
                if (userDb == null) return HttpNotFound();

                // 2. Cập nhật mật khẩu (nếu có nhập mới)
                if (!string.IsNullOrEmpty(model.Password))
                {
                    userDb.Password = model.Password; 
                }

                // Không cập nhật Username vì nó là khóa chính
                db.SaveChanges();

                TempData["Message"] = "Cập nhật tài khoản thành công!";
                return RedirectToAction("Index");
            }

            // Nếu có lỗi (ví dụ mật khẩu xác nhận không khớp), trả về View
            return View(model);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == User.Identity.Name)
            {
                TempData["Error"] = "Bạn không thể tự xóa tài khoản của chính mình!";
                return RedirectToAction("Index");
            }
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
