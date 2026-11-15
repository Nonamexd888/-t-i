using Đề_tài.Models;
using Đề_tài.Models.ViewModel.Cart;
using Đề_tài.Models.ViewModel.Order;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Đề_tài.Controllers
{
    public class OrderController : BaseController
    {
        private ChildStoreEntities db = new ChildStoreEntities();

        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        // GET: Order/Checkout
        
        public ActionResult Checkout()
        {
            //Kiểm tra giỏ hàng trong session,
            //Nếu giỏ hàng rỗng hoặc không có sản phẩm thì chuyển hướng về trang chủ
            //var cart = Session["Cart"] as List<CartItem>; // Không được bởi vì Session là lưu dạng cart
            // 1. Lấy CartService từ Session
            var cartService = new CartService(Session);
            var cart = cartService.GetCart();

            // 2. Kiểm tra giỏ hàng (dùng cart.Items thay vì cart)
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Home_page", "Home");
            }

            // 3. Lấy thông tin người dùng
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null) return RedirectToAction("Login", "Account");

            var customer = db.Customers.FirstOrDefault(c => c.Username == user.Username);
            if (customer == null) return RedirectToAction("Login", "Account");

            // 4. Tạo CheckoutVM
            var model = new CheckoutVM
            {
                CartItems = cart.Items.ToList(), // Lấy danh sách sản phẩm từ Cart
                TotalAmount = cart.TotalValue(),
                OrderDate = DateTime.Now,
                ShippingAddress = customer.CustomerAddress,
                CustomerID = customer.CustomerID,
                Username = customer.Username
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Checkout(CheckoutVM model)
        {
            var cartService = new CartService(Session);
            var cart = cartService.GetCart();

            // 1. Lấy thông tin user và customer TRƯỚC
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null) return RedirectToAction("Login", "Account");

            var customer = db.Customers.FirstOrDefault(c => c.Username == user.Username);
            if (customer == null) return RedirectToAction("Login", "Account");

            // 2. XỬ LÝ ĐỊA CHỈ RỖNG (ĐẶT TRƯỚC ModelState.IsValid)
            if (string.IsNullOrWhiteSpace(model.ShippingAddress))
            {
                model.ShippingAddress = customer.CustomerAddress;
                ModelState.Remove("ShippingAddress"); // Xóa lỗi Required để ModelState hợp lệ trở lại
            }

            // 3. Bây giờ mới kiểm tra ModelState
            if (ModelState.IsValid)
            {
                // Kiểm tra giỏ hàng
                if (cart == null || !cart.Items.Any())
                {
                    return RedirectToAction("Home_page", "Home");
                }

                // Thiết lập PaymentStatus
                string paymentStatus = "Chưa thanh toán";
                switch (model.PaymentMethod)
                {
                    case "Tiền mặt": paymentStatus = "Thanh toán tiền mặt"; break;
                    case "Mua trước trả sau": paymentStatus = "Trả góp"; break;
                    default: paymentStatus = "Chưa thanh toán"; break;
                }

                // Tạo đơn hàng
                var order = new Order
                {
                    CustomerID =customer.CustomerID,
                    OrderDate = DateTime.Now,
                    TotalAmount = model.TotalAmount,
                    PaymentStatus = paymentStatus,
                    PaymentMethod = model.PaymentMethod,
                    DeliveryMethod = model.ShippingMethod,
                    AddressDelivery = model.ShippingAddress,
                    OrderDetails = cart.Items.Select(item => new OrderDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    }).ToList()
                };
                try
                {
                    db.Orders.Add(order);
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    // Lấy thông báo lỗi chi tiết từ SQL Server
                    var innerMessage = ex.InnerException?.InnerException?.Message
                                       ?? ex.InnerException?.Message
                                       ?? ex.Message;

                    // Trả thẳng lỗi ra màn hình để xem
                    return Content("LỖI DATABASE: " + innerMessage);
                }

                cartService.ClearCart();
                return RedirectToAction("OrderSuccess", new { id = order.OrderID });
            }

            // Nếu có lỗi khác (ví dụ: chưa chọn phương thức giao hàng), load lại CartItems
            model.CartItems = cart.Items.ToList();
            return View(model);
        }

        public ActionResult OrderSuccess(int id)
        {
            // Sửa Include để tải cả Product
            var order = db.Orders
     .Include("OrderDetails.Product")
     .SingleOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [Authorize]
        public ActionResult MyOrder(string searchTerm, int? page)
        {
            var model = new MyOrderVM();

            // 1. Lấy thông tin khách hàng
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null) return RedirectToAction("Login", "Account");
            var customer = db.Customers.FirstOrDefault(c => c.Username == user.Username);
            if (customer == null) return RedirectToAction("Login", "Account");

            // 2. Lấy danh sách đơn hàng
            var orders = db.Orders
                .Include("OrderDetails.Product")
                .Where(o => o.CustomerID == customer.CustomerID);

            // 3. Tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.SearchTerm = searchTerm;
                orders = orders.Where(o => o.OrderID.ToString().Contains(searchTerm)
                                        || o.OrderDetails.Any(od => od.Product.ProductName.Contains(searchTerm)));
            }

            // 4. Sắp xếp và Phân trang
            int pageNumber = page ?? 1;
            int pageSize = 5; // 5 đơn hàng 1 trang
            model.Orders = orders.OrderByDescending(o => o.OrderDate).ToPagedList(pageNumber, pageSize);

            return View(model);
        }
    }
}