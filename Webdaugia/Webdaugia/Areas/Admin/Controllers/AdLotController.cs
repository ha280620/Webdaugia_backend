using Webdaugia.Models;
using Webdaugia.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Webdaugia.DAO;
using System.IO;


namespace Webdaugia.Areas.Admin.Controllers
{
    public class AdLotController : Controller
    {
        public ActionResult Index()
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                return View();
            }
            
        }
        // GET: Admin/Lot
        AuctionDBContext db = null;
        public ActionResult ListLot(string searchString, int page = 1, int pageSize = 10)
        {
            if(Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var dao = new LotDao();
                var model = dao.ListAllPaging(searchString, page, pageSize);
                ViewBag.searchString = searchString;
                return View(model);
            }
                
        }
        //CREATE LOT ======================================================================
        [HttpGet]
        public ActionResult CreateLot()
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                SetViewBag();
                return View();
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateLot([Bind(Include = "Name,CateID,StartingPrice,MiniumBid,AdvanceDeposit,ParticipationFee,HostLot,Location,ViewInTime,TimeForBidStart,TimeForBidEnd,TimeForRegisterStart,TimeForRegisterEnd")] Lot lot, HttpPostedFileBase file1)
        {
            SetViewBag();
            if (ModelState.IsValid)
            {
                db = new AuctionDBContext();
                lot.SiteTile = FriendlyURL.URLFriendly(lot.Name);
                lot.Status = true;
                db.Lots.Add(lot);
                db.SaveChanges();
                int LotId = db.Lots.Max(x => x.ID);//Lấy Id của sản phẩm mới vừa thêm vào
                if (file1 != null)
                {
                    string fileName1 = SaveImage(file1);
                    var img1 = new Lot();
                    img1.ID = LotId;
                    img1.LotImage = fileName1;
                    db.Lots.Add(img1);
                    db.SaveChanges();
                }

                ViewBag.success = "Tạo phiên đấu giá mới thành công!";
                return View(lot);
            }
            else
            {
                return View(lot);
            }
        }
        public void SetViewBag()
        {
            db = new AuctionDBContext();
            var cate0 = db.Categories.Where(x => x.ID == null).ToList();
            ViewBag.cate0 = new SelectList(cate0, "Id", "Name");
            
        }
        public string SaveImage(HttpPostedFileBase fileUpload)
        {
            //Image
            string fileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
            string extension = Path.GetExtension(fileUpload.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string ImagePath = "/Content/Product/" + fileName;
            fileName = Path.Combine(Server.MapPath("/Content/Images/Products/"), fileName);
            try
            {
                fileUpload.SaveAs(fileName);
            }
            catch (Exception ex)
            {

            }
            return ImagePath;
        }
        public JsonResult GetCate1(int Id)
        {
            db = new AuctionDBContext();
            db.Configuration.ProxyCreationEnabled = false;
            var cate1 = db.Categories.Where(x => x.ID == Id).ToList();
            ViewBag.cate1 = new SelectList(cate1, "ID", "Name");
            return Json(cate1, JsonRequestBehavior.AllowGet);
        }
        //EDIT LOT============================================================
        [HttpGet]
        public ActionResult EditLot(int id)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                db = new AuctionDBContext();
                var lot = db.Lots.Find(id);
                SetViewBag();
                return View(lot);
            }

            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditLot([Bind(Include = "Name,CateID,StartingPrice,MiniumBid,AdvanceDeposit,ParticipationFee,HostLot,Location,ViewInTime,TimeForBidStart,TimeForBidEnd,TimeForRegisterStart,TimeForRegisterEnd")] Lot lot, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
        {
            SetViewBag();
            db = new AuctionDBContext();
            if (ModelState.IsValid)
            {
                var editLot = db.Lots.Where(x => x.ID == lot.ID).SingleOrDefault();
                if (editLot == null)
                    return HttpNotFound();
                editLot.Name = lot.Name;
                editLot.CateID = lot.CateID;
                editLot.StartingPrice = lot.StartingPrice;
                editLot.MiniumBid = lot.MiniumBid;
                editLot.ParticipationFee = lot.ParticipationFee;
                editLot.AdvanceDesposit = lot.AdvanceDesposit;
                editLot.ViewInTime = lot.ViewInTime;
                editLot.Location = lot.Location;
                editLot.HostLot = lot.HostLot;
                editLot.TimeForRegisterStart = lot.TimeForRegisterStart;
                editLot.TimeForRegisterEnd = lot.TimeForRegisterEnd;
                editLot.TimeForBidStart = lot.TimeForBidStart;
                editLot.TimeForBidEnd = lot.TimeForBidEnd;
                editLot.SiteTile = FriendlyURL.URLFriendly(lot.Name);
                editLot.HostName = ((UserLogin)Session["AD"]).Name;
                db.SaveChanges();
                //Lưu từng ảnh nêu có nhập ảnh
                if (file1 != null)
                {
                    string fileName1 = SaveImage(file1);
                    var img1 = new Lot();
                    img1.ID = lot.ID;
                    img1.LotImage = fileName1;
                    db.Lots.Add(img1);
                    db.SaveChanges();
                }
                ViewBag.success = "Sửa Sản Phẩm thành công!";
                return View(lot);
            }
            return View();
        }
        //DELETE PRODUCT==========================================================
        public ActionResult DeleteLot(int id)
        {
            db = new AuctionDBContext();
            //Xóa ảnh SP
            //var allimg = db.ProductImages.Where(x => x.ProductId == id).ToList();
            //foreach (var item in allimg)
            //{
            //    db.ProductImages.Remove(item);
            //}
            //db.SaveChanges();
            //Xóa SP
            var delLot = db.Lots.Where(x => x.ID == id).SingleOrDefault();
            db.Lots.Remove(delLot);
            db.SaveChanges();
            return RedirectToAction("ListLot");
        }

    }
}