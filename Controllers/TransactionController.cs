using Đề_tài.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Đề_tài.Controllers
{
    public class TransactionController : Controller
    {
        private ChildStoreEntities db = new ChildStoreEntities();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult lichsugiaodich()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}