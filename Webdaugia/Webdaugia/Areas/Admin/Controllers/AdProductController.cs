using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdaugia.DAO;
using Webdaugia.Models;
using Webdaugia.Models.Common;

namespace Webdaugia.Areas.Admin.Controllers
{
    public class AdProductController : Controller
    {
        // GET: Admin/Product
        AuctionDBContext db = null;
        public ActionResult ListProduct(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ProductDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.searchString = searchString;
            return View(model);
        }
        //CREATE PRODUCT ======================================================================
        [HttpGet]
        public ActionResult CreateProduct()
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateProduct([Bind(Include = "Name,LotID,Description")] Product product, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
        {
            SetViewBag();
            if (ModelState.IsValid)
            {
                db = new AuctionDBContext();
                product.CreatedAt = DateTime.Now;
                product.CreatedBy = ((UserLogin)Session["AD"]).UserID;
                db.Products.Add(product);
                db.SaveChanges();
                //Lưu từng ảnh nêu có nhập ảnh
                int ProId = db.Products.Max(x => x.ID);//Lấy Id của sản phẩm mới vừa thêm vào
                if (file1 != null)
                {
                    string fileName1 = SaveImage(file1);
                    var img1 = new ProductsImage();
                    img1.ProductId = ProId;
                    img1.Image = fileName1;
                    db.ProductsImages.Add(img1);
                    db.SaveChanges();
                }
                if (file2 != null)
                {
                    string fileName2 = SaveImage(file2);
                    var img2 = new ProductsImage();
                    img2.ProductId = ProId;
                    img2.Image = fileName2;
                    db.ProductsImages.Add(img2);
                    db.SaveChanges();
                }
                if (file3 != null)
                {
                    string fileName3 = SaveImage(file3);
                    var img3 = new ProductsImage();
                    img3.ProductId = ProId;
                    img3.Image = fileName3;
                    db.ProductsImages.Add(img3);
                    db.SaveChanges();
                }

                ViewBag.success = "Tạo mới sản phẩm thành công!";
                return View(product);
            }
            else
            {
                return View(product);
            }
        }
        public void SetViewBag()
        {
            db = new AuctionDBContext();
            var lot01 = db.Lots.Where(x => x.AssetID == null).ToList();
            ViewBag.cate0 = new SelectList(lot01, "Id", "Name");
        }
        public string SaveImage(HttpPostedFileBase fileUpload)
        {
            //Image
            string fileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
            string extension = Path.GetExtension(fileUpload.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string ImagePath = "/Content/Products/" + fileName;
            fileName = Path.Combine(Server.MapPath("/Content/Products/"), fileName);
            try
            {
                fileUpload.SaveAs(fileName);
            }
            catch (Exception ex)
            {

            }
            return ImagePath;
        }
        //public JsonResult GetCate1(int Id)
        //{
        //    db = new AuctionDBContext();
        //    db.Configuration.ProxyCreationEnabled = false;
        //    var cate1 = db.Categories.Where(x => x.ParentId == Id).ToList();
        //    ViewBag.cate1 = new SelectList(cate1, "Id", "Name");
        //    return Json(cate1, JsonRequestBehavior.AllowGet);
        //}
        //EDIT PRODUCT============================================================
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            db = new AuctionDBContext();
            var product = db.Products.Find(id);
            SetViewBag();
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditProduct([Bind(Include = "ID,Name,LotID,Price,Description")] Product product, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
        {
            SetViewBag();
            db = new AuctionDBContext();
            if (ModelState.IsValid)
            {
                var editPro = db.Products.Where(x => x.ID == product.ID).SingleOrDefault();
                if (editPro == null)
                    return HttpNotFound();
                //Xóa hình ảnh cũ trong bảng ProductImage
                var delImg = db.ProductsImages.Where(x => x.ProductId == product.ID);
                foreach (var item in delImg)
                {
                    db.ProductsImages.Remove(item);
                }
                db.SaveChanges();

                //Sửa thông tin sản phẩm 
                editPro.ID = product.ID;
                editPro.Name = product.Name;
                editPro.LotID = product.LotID;
                editPro.Description = product.Description;
                editPro.UpdatedAt = DateTime.Now;
                editPro.UpdatedBy = ((UserLogin)Session["AD"]).UserID;
                db.SaveChanges();
                //Lưu từng ảnh nêu có nhập ảnh
                if (file1 != null)
                {
                    string fileName1 = SaveImage(file1);
                    var img1 = new ProductsImage();
                    img1.ProductId = product.ID;
                    img1.Image = fileName1;
                    db.ProductsImages.Add(img1);
                    db.SaveChanges();
                }
                if (file2 != null)
                {
                    string fileName2 = SaveImage(file2);
                    var img2 = new ProductsImage();
                    img2.ProductId = product.ID;
                    img2.Image = fileName2;
                    db.ProductsImages.Add(img2);
                    db.SaveChanges();
                }
                if (file3 != null)
                {
                    string fileName3 = SaveImage(file3);
                    var img3 = new ProductsImage();
                    img3.ProductId = product.ID;
                    img3.Image = fileName3;
                    db.ProductsImages.Add(img3);
                    db.SaveChanges();
                }
                ViewBag.success = "Sửa Sản Phẩm thành công!";
                return View(product);
            }
            return View();
        }
        //DELETE PRODUCT==========================================================
        public ActionResult DeleteProduct(int id)
        {
            db = new AuctionDBContext();
            //Xóa ảnh SP
            var allimg = db.ProductsImages.Where(x => x.ProductId == id).ToList();
            foreach (var item in allimg)
            {
                db.ProductsImages.Remove(item);
            }
            db.SaveChanges();
            //Xóa SP
            var delPro = db.Products.Where(x => x.ID == id).SingleOrDefault();
            db.Products.Remove(delPro);
            db.SaveChanges();
            return RedirectToAction("ListProduct");
        }

    }
}