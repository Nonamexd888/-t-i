using Đề_tài.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Đề_tài.Controllers
{
    public class BaseController : Controller
    {
        // Hàm này tự động chạy TRƯỚC MỖI ACTION trong Controller con
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var pmmodel = new PersonalMenuVM();

            // 1. Kiểm tra đăng nhập
            if (User.Identity.IsAuthenticated)
            {
                pmmodel.IsLoggedIn = true;
                pmmodel.Username = User.Identity.Name;
            }
            else
            {
                pmmodel.IsLoggedIn = false;
                pmmodel.Username = "";
            }

            // 2. Lấy số lượng giỏ hàng từ Session
            var cart = Session["Cart"] as Đề_tài.Models.ViewModel.Cart.Cart;
            if (cart != null && cart.Items.Any())
            {
                pmmodel.CartCount = cart.Items.Count();
            }
            else
            {
                pmmodel.CartCount = 0;
            }

            // 3. Gán vào ViewBag cho tất cả các trang
            ViewBag.pmmodel = pmmodel;

            base.OnActionExecuting(filterContext);
        }
    }
}