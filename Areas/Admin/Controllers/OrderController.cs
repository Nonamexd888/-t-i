using Đề_tài.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using Đề_tài.Models.ViewModel.Admin;

namespace Đề_tài.Areas.Admin.Controllers
{
    

    public class OrderController : Controller
    {
        private ChildStoreEntities db = new ChildStoreEntities();

        // 1. TRANG DANH SÁCH ĐƠN HÀNG (Index)
        public ActionResult Index(string searchName, string searchStatus, DateTime? searchDate, int? page)
        {
            var model = new OrderSearch();
            model.SearchName = searchName;
            model.SearchStatus = searchStatus;
            model.SearchDate = searchDate;

            var orders = db.Orders.Include("Customer").AsQueryable();

            // Lọc dữ liệu (nếu có)
            if (!string.IsNullOrEmpty(searchName))
                orders = orders.Where(o => o.Customer.CustomerName.Contains(searchName));
            if (!string.IsNullOrEmpty(searchStatus))
                orders = orders.Where(o => o.PaymentStatus == searchStatus);
            if (searchDate.HasValue)
                orders = orders.Where(o => System.Data.Entity.DbFunctions.TruncateTime(o.OrderDate) == searchDate.Value);

            orders = orders.OrderByDescending(o => o.OrderID);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            model.Orders = orders.ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        // 2. TRANG CHI TIẾT ĐƠN HÀNG (Detail)
        public ActionResult Detail(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            // Lấy đơn hàng kèm theo Khách hàng và Sản phẩm
            var order = db.Orders
                .Include("Customer")
                .Include("OrderDetails.Product")
                .SingleOrDefault(o => o.OrderID == id);

            if (order == null) return HttpNotFound();

            // Trả thẳng order ra View
            return View(order);
        }
        // 1. Hàm GET: Hiển thị form cập nhật trạng thái
        public ActionResult UpdateStatus(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var order = db.Orders.Find(id);
            if (order == null) return HttpNotFound();

            // LOGIC: Xác định xem trạng thái hiện tại thì được phép chuyển sang trạng thái nào
            var allowed = new List<string>();
            string currentStatus = order.OrderStatus;

            switch (currentStatus)
            {
                case "Chờ xử lý":
                    allowed.Add("Đang giao");
                    allowed.Add("Đã giao");
                    allowed.Add("Đã hủy");
                    break;
                case "Đang giao":
                    allowed.Add("Đã giao");
                    allowed.Add("Đã hủy");
                    break;
                case "Đã giao":
                case "Đã hủy":
                    // Trạng thái cuối cùng, không cho chuyển tiếp
                    break;
            }

            // Truyền danh sách trạng thái hợp lệ ra View
            ViewBag.AllowedStatuses = allowed;
            ViewBag.CurrentStatus = currentStatus;

            return View(order); // Trả thẳng Model Order
        }

        // 2. Hàm POST: Xử lý khi bấm nút Lưu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int id, string newStatus)
        {
            var order = db.Orders.Find(id);
            if (order == null) return HttpNotFound();

            // Cập nhật trạng thái mới vào cột OrderStatus
            order.OrderStatus = newStatus;
            db.SaveChanges();

            TempData["Message"] = "Cập nhật trạng thái thành công!";

            // Cập nhật xong thì quay lại trang chi tiết (Detail)
            return RedirectToAction("Detail", new { id = id });
        }
    }
}