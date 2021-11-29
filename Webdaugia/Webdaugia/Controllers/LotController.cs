using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdaugia.Models;
using Webdaugia.Models.Common;
using PagedList;
using System.Configuration;

namespace Webdaugia.Controllers
{
    [HandleError]
    public class LotController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        // GET: Lot
        AuctionDBContext db = null;
        //Lot Detail
        public ActionResult OnGoingLot(int LotId)
        {
            var user = (Models.Common.UserLogin)Session["USER"];
            db = new AuctionDBContext();
            var Lot = db.Lots.Where(x => x.ID == LotId && x.Status == true).FirstOrDefault();
            if(Lot == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            if(Lot.HighBid != null)
            {
                var total = Lot.HighBid + Lot.MiniumBid;
                ViewBag.highBid = total;
            }
            var listReadyLot = db.Lots.Where(x => x.TimeForRegisterEnd >= DateTime.Now && x.TimeForRegisterStart < DateTime.Now && x.Status == true).Take(3).ToList();
            var listLotAttachment = db.LotAttachments.Where(x => x.LotID == LotId).ToList();
            var listAttributes = db.Products.Where(x => x.LotID == LotId).ToList();
            var listAttachment = db.LotAttachments.Where(x => x.LotID == LotId).ToList();
            ViewBag.bidRegister = null;
            if (user != null)
            {
                RegisterBid UserRegisterBid = db.RegisterBids.Where(x => x.LotID == LotId && x.UserID == user.UserID && x.Status == 1).SingleOrDefault();
                if (UserRegisterBid != null)
                {

                    ViewBag.bidRegister = UserRegisterBid.ID;
                }
            }
            
            var listRegisterBid = db.RegisterBids.Where(x => x.LotID == LotId && x.Status == 1).ToList();
            List<Auction> listAuctioned = new List<Auction>();
            if (listRegisterBid != null)
            {
                foreach(var item in listRegisterBid)
                {
                    List<Auction> listAuction = db.Auctions.Where(x => x.RegisterBidID == item.ID).OrderByDescending(x => x.BidTime).ToList();
                    if(listAuction != null)
                    {
                        listAuctioned.AddRange(listAuction);
                    }
                  
                }
                listAuctioned = listAuctioned.OrderByDescending(o => o.BidTime).ToList();

            }
           if(listAuctioned != null)
            {
                ViewBag.listAuctioned = listAuctioned;
            }
           

            ViewBag.listAttributes = listAttributes;
            ViewBag.listLotAttachment = listLotAttachment;
            ViewBag.listReadyLot = listReadyLot;
            ViewBag.listAttachment = listAttachment;

            ViewBag.listRegisterBid = listRegisterBid.Count();


            return View(Lot);
        }

        public ActionResult IncomingLot(int LotId)

        {
            if (LotId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            db = new AuctionDBContext();
            var user = (Models.Common.UserLogin)Session["USER"];
    
            var Lot = db.Lots.Where(x => x.ID == LotId && x.Status == true).FirstOrDefault();
            if (Lot == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            if (Lot == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var listReadyLot = db.Lots.Where(x => x.TimeForRegisterEnd >= DateTime.Now && x.TimeForRegisterStart < DateTime.Now && x.Status == true).Take(3).ToList();
            //var listLotAttachment = db.LotAttachments.Where(x => x.LotID == LotId).ToList();
            var listAttributes = db.Products.Where(x => x.LotID == LotId).ToList();
            var listAttachment = db.LotAttachments.Where(x => x.LotID == LotId).ToList();
            var totalMoney = Lot.AdvanceDesposit + Lot.ParticipationFee;
            if(user != null)
            {
                var registered = db.RegisterBids.Where(x => x.LotID == LotId && x.UserID == user.UserID).SingleOrDefault();
                if (registered != null)
                {
                    if (registered.Status == 1)
                    {
                        ViewBag.register = true;
                    }
                    else
                    {
                        ViewBag.register = false;
                    }

                }
                else
                {
                    ViewBag.register = null;
                }
            }
           
            ViewBag.total = totalMoney;
            ViewBag.listAttributes = listAttributes;
            ViewBag.JavaScriptFunction = string.Format("countDownTime('{0}');", Lot.TimeForRegisterEnd);
            //ViewBag.listLotAttachment = listLotAttachment;
            ViewBag.listReadyLot = listReadyLot;
            ViewBag.listAttachment = listAttachment;

            return View(Lot);
        }

        //List Lot By Categories
        public ActionResult ListLotByCate(int CateId)
        {
            db = new AuctionDBContext();
            var cate = db.Categories.Where(x => x.ID == CateId).SingleOrDefault();
            if(cate.Status == false)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.CateName = db.Categories.Where(x => x.ID == CateId && x.Status == true).FirstOrDefault().Name;
            ViewBag.listCategory = db.Categories.Where(x=>x.Status == true).ToList();
            var listLotbyCate = db.Lots.Where(x => x.CateID == CateId && x.Status == true).ToList();

            return View(listLotbyCate);
        }

        public ActionResult ListLotByDate1()
        {
            db = new AuctionDBContext();
            var listOnGoingLot = db.Lots.Where(x => x.TimeForBidEnd > DateTime.Now && x.TimeForBidStart < DateTime.Now && x.Status == true).ToList();
            HomeModel homemodel = new HomeModel();
            homemodel.listOnGoingLot = listOnGoingLot;
            
            return View(homemodel);
        }

        public ActionResult ListLotByDate2()
        {
            db = new AuctionDBContext();
            HomeModel homemodel = new HomeModel();
            var listReadyLot = db.Lots.Where(x => x.TimeForBidStart >= DateTime.Now && x.TimeForRegisterStart < DateTime.Now && x.Status == true).ToList();
            homemodel.listReadyLot = listReadyLot;
            
            return View(homemodel);
        }

        public ActionResult ListEndingLot()
        {
            db = new AuctionDBContext();
            var listEndingLot = db.Lots.Where(x => x.TimeForBidEnd < DateTime.Now && x.Status == true).ToList();
            HomeModel homemodel = new HomeModel();
            homemodel.listEndingLot = listEndingLot;

            return View(homemodel);
        }

        public ActionResult RegisterBid(int lotId, int userID, string url)
        {

            if (Session["USER"] == null)
            {
                return Redirect(url);
            }

            db = new AuctionDBContext();
            var product = db.Lots.Where(x => x.Status == true && x.ID == lotId).SingleOrDefault();
            var user = db.Users.Where(x => x.ID == userID).SingleOrDefault();
            var registered = db.RegisterBids.Where(x => x.LotID == lotId && x.UserID == user.ID).SingleOrDefault();
            if (product == null && user == null && registered != null)
            {
                return Redirect(url);
            } else if (product != null && product.TimeForRegisterStart > DateTime.Now || product.TimeForRegisterEnd < DateTime.Now || product.Status != true)
            {
                return Redirect(url);
            }
            else
            {
                RegisterBid register = new RegisterBid();
                register.LotID = lotId;
                register.UserID = userID;
                register.Status = 0;
                db.RegisterBids.Add(register);
                db.SaveChanges();
                try
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/sendangky.html"));

                    content = content.Replace("{{id}}", user.ID.ToString());
                    content = content.Replace("{{name}}", user.FullName);
                    content = content.Replace("{{tenphien}}", product.Name);

                    string tb = "Đăng ký tham gia đấu giá " + user.ID + " - " + user.FullName + " phiên đấu " + product.ID + " - " +product.Name;
                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    new MailHelper().SendMail(toEmail, tb, content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);

                }
            }

            return Redirect(url);
        }

        public ActionResult PlaceBid(int lotId, int registerBidID, long priceBid, string url)
        { 
            if (Session["USER"] == null)
            {
                return Redirect(url);
            }

            db = new AuctionDBContext();
            var lot = db.Lots.Find(lotId);
            var register = db.RegisterBids.Find(registerBidID);
            if (lot == null || register == null)
            {
                return Redirect(url);
            }
            else if (lot != null && lot.TimeForBidStart > DateTime.Now || lot.TimeForBidEnd < DateTime.Now || lot.Status != true || register.Status !=1 || lot.Status != true)
            {
                return Redirect(url);
            }
            else 
            {
                if(lot.HighBid == null)
                {
                    if((priceBid - lot.StartingPrice) == 0)
                    {
                        lot.HighBid = priceBid;
                        db.Lots.AddOrUpdate(lot);
                        Auction item = new Auction();
                        item.RegisterBidID = register.ID;
                        item.PriceBid = priceBid;
                        item.BidTime = DateTime.Now;
                        item.Status = 0;
                        db.Auctions.Add(item);
                        db.SaveChanges();
                        return Redirect(url);
                    }
                    if ((priceBid - lot.StartingPrice)%lot.MiniumBid == 0)
                    {
                        lot.HighBid = priceBid;
                        db.Lots.AddOrUpdate(lot);
                        Auction item = new Auction();
                        item.RegisterBidID = register.ID;
                        item.PriceBid = priceBid;
                        item.BidTime = DateTime.Now;
                        item.Status = 0;
                        db.Auctions.Add(item);
                        db.SaveChanges();
                        return Redirect(url);
                    }

                }
                else
                {
                 
                    if ((priceBid - lot.StartingPrice) % lot.MiniumBid == 0 && priceBid > lot.HighBid)
                    {
                        lot.HighBid = priceBid;
                        db.Lots.AddOrUpdate(lot);
                        Auction item = new Auction();
                        item.RegisterBidID = register.ID;
                        item.PriceBid = priceBid;
                        item.BidTime = DateTime.Now;
                        item.Status = 0;
                        db.Auctions.Add(item);
                        db.SaveChanges();
                        return Redirect(url);
                    }
                }

            }
            return Redirect(url);
        }
    }
}