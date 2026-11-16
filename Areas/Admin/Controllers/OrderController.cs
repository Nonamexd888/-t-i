using Đề_tài.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace Đề_tài.Areas.Admin.Controllers
{
    

    public class OrderController : Controller
    {
        private ChildStoreEntities db = new ChildStoreEntities();

        // 1. TRANG DANH SÁCH ĐƠN HÀNG (Index)
        public ActionResult Index(int? page)
        {
            var orders = db.Orders.Include("Customer").OrderByDescending(o => o.OrderDate);
            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        // 2. TRANG CHI TIẾT ĐƠN HÀNG (Detail)
        public ActionResult Detail(int id)
        {
            // Lấy thông tin đơn hàng kèm theo Khách hàng và Chi tiết sản phẩm
            var order = db.Orders
                .Include("Customer")
                .Include("OrderDetails.Product")
                .SingleOrDefault(o => o.OrderID == id);

            if (order == null) return HttpNotFound();

            return View(order);
        }
    }
}