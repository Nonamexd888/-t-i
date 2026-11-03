using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Remoting.Messaging;
using System.Web.Security;
using Đề_tài.Models;
using Đề_tài.Models.ViewModel.Account;
using System.Web.WebPages.Html;

namespace Đề_tài.Controllers
{
    public class AccountController : Controller
    {
        private ChildStoreEntities db = new ChildStoreEntities();
        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                //Kiểm tra tên đăng nhập đã tồn tại chưa
                var existingUser = db.Users.SingleOrDefault(u=>u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username","Tên đăng nhập này đã tồn tại!");
                    return View(model);
                }
                //Nếu chưa tồn tại thì tạo bản ghi thông tin tài khoản trong bảng User
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password,
                    UserRole = "C"
                };
                db.Users.Add(user);
                //Tương tự lưu bản ghi từ RegisterVM vào bảng customer thật
                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username
                };
                db.Customers.Add(customer);
                //Lưu vào db
                db.SaveChanges();
                return RedirectToAction("Home_page", "Home");
            }
            return View(model);
        }
    }
}