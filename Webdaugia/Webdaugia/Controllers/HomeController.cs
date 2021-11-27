using Webdaugia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdaugia.DAO;
using Webdaugia.Models.Common;

namespace Webdaugia.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {

        AuctionDBContext db = null;
        public ActionResult Index()
        {
            var dao = new UserDao();
            db = new AuctionDBContext();
            var listOnGoingLot = db.Lots.Where(x => x.TimeForBidEnd > DateTime.Now && x.TimeForBidStart < DateTime.Now && x.Status == true).ToList();
            HomeModel homemodel = new HomeModel();
            homemodel.listOnGoingLot = listOnGoingLot;
            var listReadyLot = db.Lots.Where(x => x.TimeForRegisterEnd >= DateTime.Now && x.TimeForRegisterStart < DateTime.Now && x.Status == true).ToList();
            homemodel.listReadyLot = listReadyLot;
            var listCategory = db.Categories.Where(x=>x.Status == true).ToList();
            homemodel.listCategory = listCategory;
            return View(homemodel);
        }

        public ActionResult Header()
        {
            db = new AuctionDBContext();

            if (Session["USER"] != null)
            {
                int userid = ((UserLogin)Session["USER"]).UserID;
                var imguser = db.UsersImages.Where(x=> x.UsersID == userid).FirstOrDefault();
                ViewBag.imguser = imguser.Image;
                ViewBag.user = true;
            }
            else
            {
                ViewBag.user = false;
            }
        
            return PartialView();
        }
    }
}