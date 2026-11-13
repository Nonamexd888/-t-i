using Đề_tài.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Đề_tài.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private ChildStoreEntities db = new ChildStoreEntities();

        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName,UploadImg,CategoryImage")] Category category)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = db.Categories.SingleOrDefault(c => c.CategoryName.ToLower().Trim() == category.CategoryName.ToLower().Trim());
                if (existingCategory!= null)
                {
                    ViewBag.ErrorMessage = "Danh mục đã tồn tại. Vui lòng chọn tên khác.";
                    return View(category);
                }
                //Thêm đoạn code để lấy đường link của ảnh đã upload và lưu ảnh vào thư mục Content/images
                if (category.UploadImg != null)
                {
                    string filename = Path.GetFileName(category.UploadImg.FileName);
                    string savePath = "~/Content/Admin/CategoryImage/";
                    category.CategoryImage = savePath + filename;
                    category.UploadImg.SaveAs(Path.Combine(Server.MapPath(savePath), filename));
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    
                        db.Categories.Add(category);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    
                }
               
                
            }

            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Category category = db.Categories.Find(id);
            //if (category == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(category);
            return Details(id);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName,CategoryImage,UploadImg")] Category category)
        {
            if (ModelState.IsValid)
            {
                //Thêm đoạn code để lấy đường link của ảnh đã upload và lưu ảnh vào thư mục Content/images
                if (category.UploadImg != null)
                {
                    string filename = Path.GetFileName(category.UploadImg.FileName);
                    string savePath = "~/Content/Admin/CategoryImage/";
                    category.CategoryImage = savePath + filename;
                    category.UploadImg.SaveAs(Path.Combine(Server.MapPath(savePath), filename));
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
               
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);

        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            //Kiểm tra xem có sản phẩm nào thuộc danh mục này không
            bool HasProduct = db.Products.Any(p=> p.CategoryID == id);
            if (HasProduct)
            {
                ViewBag.ErrorMessage = "Không thể xóa danh mục này vì còn sản phẩm thuộc danh mục này.";
                return View(category);
            }
            db.Categories.Remove(category);
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
