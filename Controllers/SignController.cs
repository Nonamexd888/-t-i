using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Đề_tài.Controllers
{
    public class SignController : Controller
    {
        // GET: Sign
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult dangki()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Dangnhap()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}