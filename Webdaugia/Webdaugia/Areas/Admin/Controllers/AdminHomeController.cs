using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdaugia.Models;

namespace Webdaugia.Areas.Admin.Controllers
{
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
            return View();
        }
    }
}