using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Đề_tài.Models;
using Đề_tài.Models.ViewModel;
using PagedList;
using PagedList.Mvc;

namespace Đề_tài.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private ChildStoreEntities db = new ChildStoreEntities();

        // GET: Admin/Products
        //Cập nhật để hiển thị form search và kết quả tìm kiếm theo Tên, tuổi, mô tả và danh mục
        public ActionResult Index(string searchTerm, decimal? minPrice, decimal? maxPrice, int? minAge, int? maxAge, string sortOrder, int? page)
        {
            var model = new SearchProductVM();
            var product = db.Products.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                
                //Tìm kiếm sản phẩm dựa trên từ khóa
                product = product.Where(p =>
                p.ProductName.Contains(searchTerm) ||
                p.ProductDescription.Contains(searchTerm) ||
                p.Category.CategoryName.Contains(searchTerm)
                );
                model.SearchTerm = searchTerm;
            }
            //Tìm kiếm sản phẩm dựa trên giá\
            if (minPrice.HasValue)
            {
                product = product.Where(p => p.ProductPrice >= minPrice.Value);
                model.MinPrice = minPrice;
            }
            if (maxPrice.HasValue)
            {
                product = product.Where(p => p.ProductPrice <= maxPrice.Value);
                model.MaxPrice = maxPrice;
            }
            //Tìm kiếm sản phẩm dựa trên tuổi
            if (minAge.HasValue)
            {
                product = product.Where(p => p.ProductMinAge >= minAge.Value);
                model.MinAge = minAge;
            }
            if (maxAge.HasValue)
            {
                product = product.Where(p => p.ProductMinAge <= maxAge.Value);
                model.MaxAge = maxAge;
            }
            //Sắp xếp sản phẩm theo giá hoặc theo tên
            switch(sortOrder)
            {
                case "name_desc": product = product.OrderByDescending(p => p.ProductName); break;
                case "name_asc": product = product.OrderBy(p => p.ProductName); break;
                case"price_desc": product = product.OrderByDescending(p => p.ProductPrice); break;
                case "price_asc": product = product.OrderBy(p => p.ProductPrice); break;
                //Mặc định sắp xếp tăng dần theo tên sản phẩm
                default: product = product.OrderBy(p => p.ProductName); break;

            }
            model.SortOrder = sortOrder;
            //Code liên quan tới phân trang
            //Lấy số trang hiện tại, mặc định là một
            int pageNumber = page ?? 1;
            //Số sản phẩm hiển thị trên một trang
            int pageSize = 5;
            model.Products = product.ToPagedList(pageNumber, pageSize);
            return View(model);
            //var products = db.Products.Include(p => p.Category);
            //return View(products.ToList());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,ProductName,ProductDescription,ProductPrice,ProductImage,ProductMinAge,ProductStock,UploadImg")] Product product)
        {
            if (ModelState.IsValid)
            {
                //Thêm đoạn code để lấy đường link của ảnh đã upload và lưu ảnh vào thư mục Content/images
                if(product.UploadImg != null)
                {
                    string filename = Path.GetFileName(product.UploadImg.FileName);
                    string savePath = "~/Content/Admin/ProductImage/";
                    product.ProductImage = savePath + filename;
                    product.UploadImg.SaveAs(Path.Combine(Server.MapPath(savePath), filename));
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,ProductName,ProductDescription,ProductPrice,ProductImage,ProductMinAge,ProductStock,UploadImg")] Product product)
        {
            if (ModelState.IsValid)
            {
                //Thêm đoạn code để lấy đường link của ảnh đã upload và lưu ảnh vào thư mục Content/images
                if (product.UploadImg != null)
                {
                    string filename = Path.GetFileName(product.UploadImg.FileName);
                    string savePath = "~/Content/Admin/ProductImage/";
                    product.ProductImage = savePath + filename;
                    product.UploadImg.SaveAs(Path.Combine(Server.MapPath(savePath), filename));
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Product product = db.Products.Find(id);
            //if (product == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(product);
            return Details(id);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
