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
            model.FeaturedProducts = products.OrderByDescending(p => p.OrderDetails.Count()).Take(10).ToList();

            //Lấy 20 sản phẩm ế nhất và phân trang
            model.NewProducts = products.OrderBy(p => p.OrderDetails.Count()).ToPagedList(pageNumber, pageSize);

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
    }
}