using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdaugia.Models;

namespace Webdaugia.Areas.Admin.Controllers
{
    [HandleError]
    public class AdminHomeController : BaseController
    {
        // GET: Admin/AdminHome
        AuctionDBContext db = null;
        public ActionResult Index()
        {
            db = new AuctionDBContext();
            long? Profit = 0;
            int countBidSuccess = db.Auctions.Where(x => x.Status == 1).Count();
            if(countBidSuccess != 0)
            {
                Profit = db.Auctions.Where(x => x.Status == 1).Sum(x => x.PriceBid);
            }
     
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
           
            ViewBag.countLot = db.Lots.Count();
            ViewBag.countProduct = db.Products.Count();
            ViewBag.countAccount = db.Users.Where(x => x.Status == 1 || x.Status == 2).Count();
            ViewBag.countTotal = Profit;

            ViewBag.countLot1 = db.Lots.Where(x => x.CateID == 8).Count();
            ViewBag.countLot2 = db.Lots.Where(x => x.CateID == 10).Count();
            ViewBag.countLot3 = db.Lots.Where(x => x.CateID == 11).Count();
            ViewBag.countLot4 = db.Lots.Where(x => x.CateID == 13).Count();
            ViewBag.countLot5 = db.Lots.Where(x => x.CateID == 17).Count();
            ViewBag.countLot6 = db.Lots.Where(x => x.CateID != 17 && x.CateID != 13 && x.CateID != 11 && x.CateID != 10 && x.CateID != 8).Count();


            ViewBag.countregisterbid0 = db.RegisterBids.Where(x => x.Lot.CateID == 8).Count();
            ViewBag.countregisterbid1 = db.RegisterBids.Where(x => x.Lot.CateID == 10).Count();
            ViewBag.countregisterbid2 = db.RegisterBids.Where(x => x.Lot.CateID == 11).Count();
            ViewBag.countregisterbid3 = db.RegisterBids.Where(x => x.Lot.CateID == 13).Count();
            ViewBag.countregisterbid4 = db.RegisterBids.Where(x => x.Lot.CateID == 17).Count();
            ViewBag.countregisterbid5 = db.RegisterBids.Where(x => x.Lot.CateID != 17 && x.Lot.CateID != 13 && x.Lot.CateID != 11 && x.Lot.CateID != 10 && x.Lot.CateID != 8).Count();
            return View();
        }

    }
}