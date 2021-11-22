﻿using Webdaugia.Models;
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
                return View();
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateLot(Lot lot, HttpPostedFileBase file1)
        {
            
            if (ModelState.IsValid)
            {
                db = new AuctionDBContext();
                Lot lot1 = new Lot();
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
                
                return View(lot);
            }

            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditLot(Lot lot, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
        {

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
                editLot.HostLot = ((UserLogin)Session["AD"]).UserID;
                editLot.TimeForRegisterStart = lot.TimeForRegisterStart;
                editLot.TimeForRegisterEnd = lot.TimeForRegisterEnd;
                editLot.TimeForBidStart = lot.TimeForBidStart;
                editLot.TimeForBidEnd = lot.TimeForBidEnd;
                editLot.SiteTile = FriendlyURL.URLFriendly(lot.Name);
                editLot.HostName = lot.HostName;
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
                ViewBag.success = "Sửa Sản Phẩm thành công!";
                return View(lot);
            }
            return View();
        }
    }
}