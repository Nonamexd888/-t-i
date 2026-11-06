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
            if (Session["Username"] != null)
            {
                return RedirectToAction("UserProfile", "Account");
            }
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

        //GET:Account/Login
        public ActionResult Login()
        {
            if (Session["Username"] != null)
            {
                return RedirectToAction("UserProfile", "Account");
            }
            return View();
        }

        //POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password && u.UserRole == "C");
                if (user != null)
                {
                    //Lưu trạng thái đăng nhập vào session
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.UserRole;

                    //Lưu thông tin xác thực người dùng vào cookie
                    FormsAuthentication.SetAuthCookie(user.Username, false);

                    return RedirectToAction("Home_page", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng!");
                }
            }
            return View(model);
        }

        //GET: Account/Profile
        public ActionResult UserProfile()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string currentUser = Session["Username"].ToString();

            var user = db.Users.SingleOrDefault(u => u.Username == currentUser);
            var customer = db.Customers.SingleOrDefault(c => c.Username == currentUser);

            if (user == null || customer == null)
            {
                // Gửi thông báo lỗi ra View
                ViewBag.ErrorMessage = "Không tìm thấy thông tin tài khoản của bạn. Vui lòng liên hệ Admin!";
                return View(); // Trả về trang Profile nhưng kèm lỗi
            }

            //Nếu tồn tại tài khoản thì đổ vào profileVM để hiển thị thông tin
            var profile = new ProfileVM
            {
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
                CustomerEmail = customer.CustomerEmail,
                CustomerAddress = customer.CustomerAddress,
                Username = user.Username
            };
            return View(profile);

        }
        //POST: Account/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(ProfileVM model)
        {
            if (ModelState.IsValid)
            {
                string currentUser = Session["Username"].ToString();
                var customer = db.Customers.SingleOrDefault(c => c.Username == currentUser);
                if (customer == null)
                {
                    ViewBag.ErrorMessage = "Không tìm thấy thông tin tài khoản của bạn. Vui lòng liên hệ Admin!";
                    return View(model);
                }
                //Cập nhật thông tin khách hàng
                customer.CustomerName = model.CustomerName;
                customer.CustomerPhone = model.CustomerPhone;
                customer.CustomerEmail = model.CustomerEmail;
                customer.CustomerAddress = model.CustomerAddress;
                db.SaveChanges();
                ViewBag.SuccessMessage = "Cập nhật thông tin thành công!";

            }
            return View(model);
        }

        //GET:Account/ChangePassword
        public ActionResult ChangePassword()
        {
            //Kiểm tra xem đã đăng nhập chưa( dựa vào session)
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        //POST: Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                string currentUser = Session["Username"].ToString();
                var user = db.Users.SingleOrDefault(u => u.Username == currentUser);
                if (user != null)
                {
                    if (user.Password.Trim() != model.OldPassword)
                    {
                        ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng!");
                        return View(model);
                    }
                    user.Password = model.NewPassword; //Cập nhật mật khẩu mới
                    db.SaveChanges();
                    ViewBag.SuccessMessage = "Đổi mật khẩu thành công!";
                    return View(model);
                }
            }
            return View(model);
        }

        public ActionResult LogOut()
        {
            // 1. Xóa toàn bộ Session
            Session.Clear();
            Session.Abandon();

            // 2. Xóa Cookie xác thực (nếu bạn có dùng FormsAuthentication)
            FormsAuthentication.SignOut();

            // 3. Chuyển hướng về trang chủ
            return RedirectToAction("Home_page", "Home");
        }

    }
}