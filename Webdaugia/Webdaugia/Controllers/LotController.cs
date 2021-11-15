using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdaugia.Models;
//using PagedList;

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
        public ActionResult OnGoingLot(int LotId = 1000016)
        {
            db = new AuctionDBContext();
            var Lot = db.Lots.Where(x => x.ID == LotId).FirstOrDefault();
            var listOnGoingLot = db.Lots.Where(x => x.TimeForBidEnd > DateTime.Now && x.TimeForBidStart < DateTime.Now && x.Status == true).ToList();
            var listLotAttachment = db.LotAttachments.Where(x => x.LotID == LotId).ToList();
            var listAttributes = db.Products.Where(x => x.LotID == LotId).ToList();
           


            ViewBag.listAttributes = listAttributes;
            ViewBag.listLotAttachment = listLotAttachment;
            ViewBag.listOnGoingLot = listOnGoingLot;

            return View(Lot);
        }

        public ActionResult IncomingLot(int LotId)

        {
            /*if (LotId == null)
            {
                return RedirectToAction("Index", "Home");
            }*/
            db = new AuctionDBContext();
            var user = (Models.Common.UserLogin)Session["USER"];
    
            var Lot = db.Lots.Where(x => x.ID == LotId).FirstOrDefault();
            if(Lot == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var listReadyLot = db.Lots.Where(x => x.TimeForRegisterEnd >= DateTime.Now && x.TimeForRegisterStart < DateTime.Now && x.Status == true).ToList();
            //var listLotAttachment = db.LotAttachments.Where(x => x.LotID == LotId).ToList();
            var listAttributes = db.Products.Where(x => x.LotID == LotId).ToList();
          
            var totalMoney = Lot.AdvanceDesposit + Lot.ParticipationFee;
            if(user != null)
            {
                var registered = db.RegisterBids.Where(x => x.LotID == LotId && x.UserID == user.UserID).SingleOrDefault();
                if (registered != null)
                {
                    if (registered.Status == true)
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

            return View(Lot);
        }

        //List Lot By Categories
        public ActionResult ListLotByCate(int CateId, int page = 1, int pageSize = 10)
        {
            db = new AuctionDBContext();
            ViewBag.CateName = db.Categories.Where(x => x.ID == CateId).FirstOrDefault().Name;
            var listLotbyCate = db.Lots.Where(x => x.CateID == CateId).ToList();

            return View(listLotbyCate.OrderBy(x => x.TimeForBidEnd)); /*.ToPagedList(page, pageSize));*/
        }

        public ActionResult RegisterBid(int lotId, int userID, string url)
        {

            if (Session["USER"] == null)
            {
                return Redirect(url);
            }

            db = new AuctionDBContext();
            var product = db.Lots.Find(lotId);
            var user = db.Users.Where(x => x.ID == userID).SingleOrDefault();
            var registered = db.RegisterBids.Where(x => x.LotID == lotId && x.UserID == user.ID).SingleOrDefault();
            if (product == null && user == null && registered != null)
            {
                return Redirect(url);
            } else if (product != null && product.TimeForRegisterStart > DateTime.Now || product.TimeForRegisterEnd < DateTime.Now)
            {
                return Redirect(url);
            }
            else
            {
                RegisterBid register = new RegisterBid();
                register.LotID = lotId;
                register.UserID = userID;
                register.Status = false;
                db.RegisterBids.Add(register);
                db.SaveChanges();
            }

            return Redirect(url);
        }
    }
}