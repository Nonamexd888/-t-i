using Antlr.Runtime.Tree;
using Đề_tài.Models;
using Đề_tài.Models.ViewModel;
using Đề_tài.Models.ViewModel.Home;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Đề_tài.Controllers
{
    public class HomeController : Controller
    {
        private ChildStoreEntities db = new ChildStoreEntities();
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}

        public ActionResult Home_page(string searchTerm, int?page)
        {
            var model = new HomeProductVM();
            var products = db.Products.AsQueryable();
            //Tìm kiếm sản phẩm dựa trên từ khóa
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.SearchTerm = searchTerm;
                products = products.Where(p => p.ProductName.Contains(searchTerm) || p.ProductDescription.Contains(searchTerm) || p.Category.CategoryName.Contains(searchTerm));
            }

            //Đoạn code liên quan tới phân trang 
            //Lấy số trang hiện tại (mặc định là trang 1 nếu không có giá trị)
            int pageNumber = page ?? 1;
            int pageSize = 6; //Số sản phẩm trong trang

            //Lấy top 10 sản phẩm bán chạy nhất
            model.FeaturedProducts = products.OrderByDescending(p => p.OrderDetails.Sum(od => od.Quantity)).Take(10).ToList();
            // Tính toán "Đã bán" và lưu vào một cái túi tên là SoldCounts
            var soldCounts = new Dictionary<int, int>();
            foreach (var p in model.FeaturedProducts)
            {
                soldCounts[p.ProductID] = p.OrderDetails.Sum(od => od.Quantity);
            }
            ViewBag.SoldCounts = soldCounts; // Bỏ túi vào ViewBag

            //Lấy 20 sản phẩm ế nhất và phân trang
            model.NewProducts = products.OrderBy(p => p.OrderDetails.Sum(q=> q.Quantity)).Take(20).ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        public ActionResult For03(int?page,string searchTerm)
        {
            var model = new SearchProductVM();
            var products = db.Products.AsQueryable();
            //Lọc theo độ tuổi
            products = products.Where(p => p.ProductMinAge >= 0 && p.ProductMinAge <= 3);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.Contains(searchTerm) || p.ProductDescription.Contains(searchTerm) || p.Category.CategoryName.Contains(searchTerm));
                model.SearchTerm = searchTerm;
            }
            int pageNumber = page??1;
            int pageSize = 20;
            model.Products = products.OrderBy(p => p.OrderDetails.Count()).ToPagedList(pageNumber, pageSize);
            return View(model);
        }

        public ActionResult For36(int? page, string searchTerm)
        {
            var model = new SearchProductVM();
            var products = db.Products.AsQueryable();
            //Lọc theo độ tuổi
            products = products.Where(p => p.ProductMinAge >= 3 && p.ProductMinAge <= 6);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.Contains(searchTerm) || p.ProductDescription.Contains(searchTerm) || p.Category.CategoryName.Contains(searchTerm));
                model.SearchTerm = searchTerm;
            }
            int pageNumber = page ?? 1;
            int pageSize = 20;
            model.Products = products.OrderBy(p => p.OrderDetails.Count()).ToPagedList(pageNumber, pageSize);
            return View(model);
        }
        public ActionResult ForOver6(int? page, string searchTerm)
        {
            var model = new SearchProductVM();
            var products = db.Products.AsQueryable();
            //Lọc theo độ tuổi
            products = products.Where(p => p.ProductMinAge >= 6);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.Contains(searchTerm) || p.ProductDescription.Contains(searchTerm) || p.Category.CategoryName.Contains(searchTerm));
                model.SearchTerm = searchTerm;
            }
            int pageNumber = page ?? 1;
            int pageSize = 20;
            model.Products = products.OrderBy(p => p.OrderDetails.Count()).ToPagedList(pageNumber, pageSize);
            return View(model);
        }

        //GET:Home/ProductDetails
        public ActionResult ProductDetail(int?id, int?quantity, int? page)
        {
            if(id == null)
            {
                return RedirectToAction("Home_page", "Home");
            }

            Product pro = db.Products.Find(id);
            if(pro == null)
            {
                return RedirectToAction("Home_page","Home");
            }

            //Lấy tất cả các sản phẩm cùng danh mục Nhưng không phải là sản phẩm đang hiện
            var productSameCate = db.Products.Where(p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID);

            ProductDetailVM model = new ProductDetailVM();

            //Đoạn code liên quan tới phân trang
            int PageNumber = page ?? 1;
            int PageSize = model.PageSize;

            model.Product = pro;
            model.RelatedProducts = productSameCate.OrderBy(p => p.ProductID).Take(8).ToPagedList(PageNumber, PageSize);
            model.TopProducts = productSameCate
    .OrderByDescending(p => p.OrderDetails.Sum(od => od.Quantity)) // Đổi Count() thành Sum()
    .Take(8)
    .ToPagedList(PageNumber, PageSize);
            if (quantity.HasValue)
            {
                model.Quantity = quantity.Value;
            }

            //Tính đã bán cho sản phẩm này rồi truyền vào ViewBag
            //var totalSold = pro.OrderDetails.Sum(q => q.Quantity);
            //ViewBag.totalSold = totalSold;

            return View(model);
        }
    }
}