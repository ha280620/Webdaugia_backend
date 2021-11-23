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

using Webdaugia.Models.Common;
using System.Data.Entity.Migrations;

namespace Webdaugia.Areas.Admin.Controllers
{
    public class AdLotController : Controller

    {
        string FilePath = "";
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
        public ActionResult ListLotAuction(string searchString, int page = 1, int pageSize = 10)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var dao = new LotDao();
                var model = dao.ListAllPagingAuction(searchString, page, pageSize);
                ViewBag.searchString = searchString;
                return View(model);
            }

        }
        public ActionResult ListRegitsterOfLot(int id,string searchString, int page = 1, int pageSize = 10)
        {
            if(Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var dao = new LotDao();
                var model = dao.ListAllPagingRegisterOfLot(id, searchString, page, pageSize);
                ViewBag.searchString = searchString;
                return View(model);
            }
                
        } 
        public ActionResult ListAuctionOfLot(int id,string searchString, int page = 1, int pageSize = 10)
        {
            if(Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var dao = new LotDao();
                var model = dao.ListAllPagingAuctionOfLot(id, searchString, page, pageSize);
                ViewBag.searchString = searchString;
                return View(model);
            }
                
        }
        public ActionResult ListLotRegister(string searchString, int page = 1, int pageSize = 10)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var dao = new LotDao();
                var model = dao.ListAllPagingRegister(searchString, page, pageSize);
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
                db = new AuctionDBContext();
                AdLot objLot = new AdLot();


                //ViewBag.banks = new SelectList(banks, "Id", "Name");
                objLot.ListCategory = db.Categories.ToList();
                return View(objLot);
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateLot(AdLot lot)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            if (lot.TimeForRegisterStart > lot.TimeForRegisterEnd)
            {
                ViewBag.success = "Ngày kết thúc  đăng ký phải lớn ngày bắt đầu đăng ký!";
                return View(lot);
            }
            if (lot.TimeForBidStart > lot.TimeForBidEnd)
            {
                ViewBag.success = "Ngày kết thúc đấu giá phải lớn ngày bắt đầu đấu !";
                return View(lot);
            }
            if (ModelState.IsValid)
            {
                db = new AuctionDBContext();
                Lot lot1 = new Lot();
                lot1.Name = lot.Name;
                lot1.SiteTile = FriendlyURL.URLFriendly(lot.Name);
                lot1.Status = false;
                lot1.CateID = lot.CateID;
                lot1.StartingPrice = lot.StartingPrice;
                lot1.MiniumBid = lot.MiniumBid;
                lot1.ParticipationFee = lot.ParticipationFee;
                lot1.AdvanceDesposit = lot.AdvanceDesposit;
                lot1.ViewInTime = lot.ViewInTime;
                lot1.Location = lot.Location;
                lot1.HostLot = ((UserLogin)Session["AD"]).UserID;
                lot1.TimeForRegisterStart = lot.TimeForRegisterStart;
                lot1.TimeForRegisterEnd = lot.TimeForRegisterEnd;
                lot1.TimeForBidStart = lot.TimeForBidStart;
                lot1.TimeForBidEnd = lot.TimeForBidEnd;
                lot1.HostName = lot.HostName;
   
                
                    string fileName = UploadFile(lot.file1);
                    lot1.LotImage = "\\Content\\Images\\Lot\\" + fileName;
             
          
                db.Lots.Add(lot1);
                db.SaveChanges();

                ViewBag.success = "Tạo phiên đấu giá mới thành công!";
                return View(lot);
            }
            else
            {
                db = new AuctionDBContext();
                //ViewBag.banks = new SelectList(banks, "Id", "Name");
                lot.ListCategory = db.Categories.ToList();
                return View(lot);
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
            FilePath = Server.MapPath("~/Content/Images/Lot/");
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
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
        
        //EDIT LOT============================================================
        [HttpGet]
        public ActionResult EditLot(int id)
        {
            ViewBag.success = null;
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                db = new AuctionDBContext();
                var lot = db.Lots.Find(id);

                lot.ListCategory = db.Categories.ToList();
                //ViewBag.banks = new SelectList(banks, "Id", "Name");
               /* lotnew.ListCategory = db.Categories.ToList();
                lotnew.Name = lot.Name;      
                lotnew.CateID = lot.CateID;
                lotnew.StartingPrice = lot.StartingPrice;
                lotnew.MiniumBid = lot.MiniumBid;
                lotnew.ParticipationFee = lot.ParticipationFee;
                lotnew.AdvanceDesposit = lot.AdvanceDesposit;
                lotnew.ViewInTime = lot.ViewInTime;
                lotnew.Location = lot.Location;
 
                lotnew.TimeForRegisterStart = lot.TimeForRegisterStart;
                lotnew.TimeForRegisterEnd = lot.TimeForRegisterEnd;
                lotnew.TimeForBidStart = lot.TimeForBidStart;
                lotnew.TimeForBidEnd = lot.TimeForBidEnd;
                lotnew.HostName = lot.HostName;
                lotnew.LotImage = lot.LotImage;*/
                return View(lot);
            }

            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
/*        [ValidateInput(false)]*/
        public ActionResult EditLot(Lot lot, HttpPostedFileBase file1)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            db = new AuctionDBContext();
            lot.ListCategory = db.Categories.ToList();
            ModelState.Remove("file1");
            ModelState.Remove("CateId");
            ModelState.Remove("ViewInTime");
            lot.ListCategory = db.Categories.ToList();
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
                editLot.HostLot = ((UserLogin)Session["AD"]).UserID;
                editLot.TimeForRegisterStart = lot.TimeForRegisterStart;
                editLot.TimeForRegisterEnd = lot.TimeForRegisterEnd;
                editLot.TimeForBidStart = lot.TimeForBidStart;
                editLot.TimeForBidEnd = lot.TimeForBidEnd;
                editLot.SiteTile = FriendlyURL.URLFriendly(lot.Name);
                editLot.HostName = lot.HostName;
                if(file1 != null)
                {
                    string path = Path.Combine(Server.MapPath(editLot.LotImage)); ;
                  
                    if (!Directory.Exists(path))
                    {
                     
                        System.IO.File.Delete(path);
                    }
         
                    string fileName = UploadFile(file1);
                    editLot.LotImage = "\\Content\\Images\\Lot\\" + fileName;
                }
                db.Lots.AddOrUpdate(editLot);
                db.SaveChanges();
                //Lưu từng ảnh nêu có nhập ảnh
                //if (file1 != null)
                //{
                //    string fileName1 = SaveImage(file1);
                //    var img1 = new Lot();
                //    img1.ID = lot.ID;
                //    img1.LotImage = fileName1;
                //    db.Lots(img1);
                //    db.SaveChanges();
                //}
                lot.LotImage = editLot.LotImage;
                ViewBag.success = "Sửa Sản Phẩm thành công!";
                return View(lot);
            }
         
            return View(lot) ;
        }
        public ActionResult LockOrUnlockLot(int id)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            db = new AuctionDBContext();
            var lot = db.Lots.Where(x => x.ID == id).FirstOrDefault();
            if (lot.Status == false)
            {
                lot.Status = true;
            }
            else
            {
                lot.Status = false;
            }
            db.Lots.AddOrUpdate(lot);
            db.SaveChanges();
            return RedirectToAction("ListLot");
        } public ActionResult ConfirmRegitsterOfLot(int LotId, int UserId, string Url)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            db = new AuctionDBContext();
            var id = LotId;
            var user = UserId;
            var lot = db.RegisterBids.Where(x => x.LotID == id && x.UserID == user).FirstOrDefault();
            if (lot.Status == false)
            {
                lot.Status = true;
            }
            else
            {
                lot.Status = false;
            }
            db.RegisterBids.AddOrUpdate(lot);
            db.SaveChanges();
            return Redirect(Url);
        }public ActionResult ConfirmAuctionOfLot(int RegisterBidID, long PriceBid, string Url)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            db = new AuctionDBContext();
    
            var auction = db.Auctions.Where(x => x.RegisterBidID == RegisterBidID && x.PriceBid == PriceBid).FirstOrDefault();
            if (auction.Status == 0)
            {
                var auctions = db.Auctions.Where(x => x.RegisterBid.LotID == auction.RegisterBid.LotID).ToList();
                foreach(var item in auctions)
                {
                    item.Status = 2;
                    db.Auctions.AddOrUpdate(auction);
                }
                auction.Status = 1;
            }
            else if(auction.Status == 2)
            {
                var auctions = db.Auctions.Where(x => x.RegisterBid.LotID == auction.RegisterBid.LotID).ToList();
                foreach (var item in auctions)
                {
                    item.Status = 2;
                    db.Auctions.AddOrUpdate(auction);
                }
                auction.Status = 1;
            }
            db.Auctions.AddOrUpdate(auction);
            db.SaveChanges();
            return Redirect(Url);
        }
    }
}