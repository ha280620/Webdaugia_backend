using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
        string FilePath = "";
        public ActionResult Index()
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            return View();
        }
        // GET: Admin/Product
        AuctionDBContext db = null;
        public ActionResult ListProduct(string searchString, int page = 1, int pageSize = 10)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var dao = new ProductDao();
                var model = dao.ListAllPaging(searchString, page, pageSize);
                ViewBag.searchString = searchString;
                return View(model);
            }
            
        }
        //CREATE PRODUCT ======================================================================
        [HttpGet]
        public ActionResult CreateProduct()
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                db = new AuctionDBContext();
                Product product = new Product();
                product.ListLot = db.Lots.ToList();
                return View(product);
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateProduct([Bind(Include = "Name,Description,LotID")] Product product, HttpPostedFileBase[] files)
        {
            db = new AuctionDBContext();
            product.ListLot = db.Lots.ToList();
            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.Now;
                product.CreatedBy = ((UserLogin)Session["AD"]).UserID;
                db.Products.Add(product);
                db.SaveChanges();
                //Lưu từng ảnh nêu có nhập ảnh
                int ProId = db.Products.Max(x => x.ID);//Lấy Id của sản phẩm mới vừa thêm vào
                if (files != null)
                {
                    foreach(HttpPostedFileBase file in files)
                    {
                        ProductsImage img = new ProductsImage();
                
                                 string fileName = UploadFile(file);
                        img.Image = "\\Content\\Images\\Prouduct\\" + fileName;
                        img.ProductId = ProId;
                        db.ProductsImages.Add(img);
                       
                    }
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
        protected string UploadFile(HttpPostedFileBase file)
        {
            string fileName = null;
            string fileExtension = null;
            string strDate = DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss");

            SetFilePath();

            fileExtension = Path.GetExtension(file.FileName).Replace(".", "");
            fileName = file.FileName.Substring(file.FileName.LastIndexOf("\\\\") + 1);
            fileName = fileName.Substring(0, fileName.LastIndexOf(fileExtension)) + strDate + "." + fileExtension;

            FilePath = FilePath + fileName;
            file.SaveAs(FilePath);
            return fileName;
        }

        private void SetFilePath()
        {
            FilePath = Server.MapPath("~/Content/Images/Prouduct/");
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
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
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                db = new AuctionDBContext();
                var product = db.Products.Find(id);              
                product.ListLot = db.Lots.ToList();
                return View(product);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditProduct(/*[Bind(Include = "Name,Description,LotID")] */Product product, HttpPostedFileBase[] files)
        {
            db = new AuctionDBContext();
            product.ListLot = db.Lots.ToList();
            int proID = product.ID;
            if (ModelState.IsValid)
            {
                var editPro = db.Products.Find(proID);
                if (editPro == null)
                    return HttpNotFound();
                //Xóa hình ảnh cũ trong bảng ProductImage
              

                //Sửa thông tin sản phẩm 
      
                editPro.Name = product.Name;
                editPro.LotID = product.LotID;
                editPro.Description = product.Description;
                editPro.UpdatedAt = DateTime.Now;
                editPro.UpdatedBy = ((UserLogin)Session["AD"]).UserID;
                if (files[0] != null)
                {
                    var delImg = db.ProductsImages.Where(x => x.ProductId == product.ID);
                    foreach (var item in delImg)
                    {
                        string path = Path.Combine(Server.MapPath(item.Image)); ;

                        if (!Directory.Exists(path))
                        {

                            System.IO.File.Delete(path);
                        }
                        db.ProductsImages.Remove(item);
                    }
        
                    foreach (HttpPostedFileBase file in files)
                    {
                        ProductsImage img = new ProductsImage();

                        string fileName = UploadFile(file);
                        img.Image = "\\Content\\Images\\Prouduct\\" + fileName;
                        img.ProductId = editPro.ID;
                        db.ProductsImages.Add(img);

                    }
                    db.SaveChanges();

                }
                db.Products.AddOrUpdate(editPro);
                db.SaveChanges();
                //Lưu từng ảnh nêu có nhập ảnh
             
                ViewBag.success = "Sửa Sản Phẩm thành công!";
                return View(product);
            }
            return View();
        }
        //DELETE PRODUCT==========================================================
     

    }
}