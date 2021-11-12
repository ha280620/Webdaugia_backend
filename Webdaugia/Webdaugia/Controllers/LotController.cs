using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdaugia.Models;
//using PagedList;

namespace Webdaugia.Controllers
{
    public class LotController : Controller
    {
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

        public ActionResult IncomingLot(int LotId = 1000016)
        {
            db = new AuctionDBContext();
            var Lot = db.Lots.Where(x => x.ID == LotId).FirstOrDefault();
            var listReadyLot = db.Lots.Where(x => x.TimeForRegisterEnd >= DateTime.Now && x.TimeForRegisterStart < DateTime.Now && x.Status == true).ToList();
            //var listLotAttachment = db.LotAttachments.Where(x => x.LotID == LotId).ToList();
            var listAttributes = db.Products.Where(x => x.LotID == LotId).ToList();

            ViewBag.listAttributes = listAttributes;
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

        

        public ActionResult Index()
        {
            return View();
        }
    }
}