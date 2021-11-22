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
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            return View();
        }
    }
}