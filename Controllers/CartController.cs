using Đề_tài.Models;
using Đề_tài.Models.ViewModel.Cart;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Đề_tài.Models.ViewModel.Cart; // Để dùng CartService

namespace Đề_tài.Controllers
{
    public class CartController : BaseController
    {
        private ChildStoreEntities db = new ChildStoreEntities();

        //Hàm lấy dịch vụ giỏ hàng
        private CartService GetCartService()
        {
            return new CartService(Session);
        }

        //Hiển thị giỏ hàng 
        // GET: Cart
        public ActionResult Index(int?page)
        {
            var cart = GetCartService().GetCart();
            var product = db.Products.ToList();
            var similarProduct = new List<Product>();

            if(cart.Items !=null && cart.Items.Any())
            {
                similarProduct = product.Where(p => cart.Items.Any(ci => ci.Category == p.Category.CategoryName) && !cart.Items.Any(ci => ci.ProductID == p.ProductID)).ToList();
            }

            int pageNumber = page ?? 1;
            int pageSize = cart.PageSize;
            cart.SimilarProducts = similarProduct.OrderBy(p => p.ProductID).ToPagedList(pageNumber, pageSize);

            return View(cart);
        }

        //Thêm sản phẩm vào giỏ
        public ActionResult AddToCart(int id, int quantity = 1)
        {
            var product = db.Products.Find(id);
            if(product != null)
            {
                if (quantity > product.ProductStock)
                {
                    // Dùng TempData thay vì ViewBag để thông báo không bị mất
                    TempData["ErrorMessage"] = "Số lượng bạn chọn vượt quá số lượng tồn kho (còn " + product.ProductStock + " sản phẩm)!";
                    return RedirectToAction("ProductDetail", "Home", new { id = id });
                }
                var cartService = GetCartService();
                cartService.GetCart().AddItem(product.ProductID, product.ProductImage,product.ProductName, product.ProductPrice, quantity,product.ProductStock, product.Category.CategoryName);
            }
            return RedirectToAction("Index", "Cart");
        }

        //Xóa sản phẩm khỏi giỏ
        public ActionResult RemoveFromCart(int id)
        {
            
                var cartService = GetCartService();
                cartService.GetCart().RemoveItem(id);
            
            return RedirectToAction("Index", "Cart");
        }

        //Làm trống giỏ hàng
        public ActionResult ClearCart()
        {
            var cartService = GetCartService();
            cartService.ClearCart();
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            // 1. Tìm sản phẩm trong DB để lấy tồn kho thực tế
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            // 2. Validate: Không cho nhỏ hơn 1 và không vượt quá tồn kho
            if (quantity < 1)
            {
                quantity = 1;
            }
            if (quantity > product.ProductStock)
            {
                quantity = product.ProductStock;
                TempData["ErrorMessage"] = "Chỉ còn " + product.ProductStock + " sản phẩm trong kho!";
            }

            // 3. Cập nhật vào giỏ hàng
            var cartService = GetCartService();
            cartService.GetCart().UpdateQuantity(id, quantity);

            return RedirectToAction("Index", "Cart");
        }
         
    }
}