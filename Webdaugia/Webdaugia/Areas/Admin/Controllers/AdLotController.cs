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

using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace Webdaugia.Areas.Admin.Controllers
{
    [HandleError]
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
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {

                var user = (UserLogin)Session["AD"];
                var dao = new LotDao();
                var model = user.Role == 2 ? dao.ListAllPaging2(user.UserID, searchString, page, pageSize) : dao.ListAllPaging(searchString, page, pageSize);
                ViewBag.searchString = searchString;
                return View(model);
            }

        }
        public ActionResult ListLotAuction(string searchString, int page = 1, int pageSize = 10)
        {
            var user = (UserLogin)Session["AD"];
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var dao = new LotDao();
                var model = user.Role == 2 ? dao.ListAllPagingAuction2(user.UserID,searchString, page, pageSize)  : dao.ListAllPagingAuction(searchString, page, pageSize);
                ViewBag.searchString = searchString;
                return View(model);
            }

        }

        public ActionResult ListAuctionOfLot(int id, string searchString, int page = 1, int pageSize = 10)
        {
            var user = (UserLogin)Session["AD"];
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var dao = new LotDao();
                
                if(user.Role == 2 && dao.ViewDetail(id).HostLot != user.UserID)
                {
                     return RedirectToAction("ListLotAuction");
                }
                var model = dao.ListAllPagingAuctionOfLot(id, searchString, page, pageSize);

                ViewBag.LotId = id;
                ViewBag.searchString = searchString;
                return View(model);
            }

        }
        public ActionResult ListLotRegister(string searchString, int page = 1, int pageSize = 10)
        {
            var user = (UserLogin)Session["AD"];
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var dao = new LotDao();
                var model = user.Role == 2 ? dao.ListAllPagingRegister2(user.UserID, searchString, page, pageSize) : dao.ListAllPagingRegister(searchString, page, pageSize);
                ViewBag.searchString = searchString;
                return View(model);
            }

        }
        public ActionResult ListUserEndOfLot(int id, string searchString, int page = 1, int pageSize = 10)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {


                db = new AuctionDBContext();

                var user = (UserLogin)Session["AD"];
                if (user.Role == 2)
                {
                    var lot = db.Lots.Where(x => x.ID == id && x.HostLot == user.UserID).SingleOrDefault();
                    if (lot == null)
                    {
                        return RedirectToAction("ListLotEnd");
                    }
                }
                var listATM = db.ATMs.ToList();
                List<ATM> listATMEnd = new List<ATM>();
                var dao = new LotDao();
                long? totalMoney = 0;
                long? Money = db.Lots.Where(x => x.ID == id).SingleOrDefault().AdvanceDesposit;
                var model = dao.ListAllPagingEndOfLot(id, searchString, page, pageSize);
                foreach (var item in model)
                {
                    totalMoney += item.Lot.AdvanceDesposit;
                    var atm = listATM.Where(x => x.UserID == item.UserID).FirstOrDefault();
                    if (atm != null)
                    {
                        listATMEnd.Add(atm);
                    }
                }
                ViewBag.searchString = searchString;
                ViewBag.listATM = listATMEnd;
                ViewBag.totalMoney = totalMoney;
                ViewBag.Money = Money;
                return View(model);
            }

        }
        public ActionResult ListRegitsterOfLot(int id, string searchString, int page = 1, int pageSize = 10)
        {
            var user = (UserLogin)Session["AD"];
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                db = new AuctionDBContext();
                var lot = db.Lots.Find(id);
                if (user.Role == 2)
                {
                    var lot3 = db.Lots.Where(x => x.ID == id && x.HostLot == user.UserID).SingleOrDefault();
                    if (lot3 == null)
                    {
                        return RedirectToAction("ListLotRegister");
                    }
                }
                ViewBag.tiencoc = lot.ParticipationFee + lot.AdvanceDesposit;
                var dao = new LotDao();
                var model = dao.ListAllPagingRegisterOfLot(id, searchString, page, pageSize);
                ViewBag.searchString = searchString;
                ViewBag.LotId = id;
                return View(model);
            }

        }
        public ActionResult ListLotEnd(string searchString, int page = 1, int pageSize = 10)
        {
            var user = (UserLogin)Session["AD"];
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var dao = new LotDao();
                var model = user.Role == 2 ? dao.ListAllPagingEnd2(user.UserID, searchString, page, pageSize) : dao.ListAllPagingEnd(searchString, page, pageSize);
                ViewBag.searchString = searchString;
                return View(model);
            }

        }
        public ActionResult ListLotEndFor180(string searchString, int page = 1, int pageSize = 10)
        {
            var user = (UserLogin)Session["AD"];
            if (Session["AD"] == null || user.Role == 2)
            {
                return RedirectToAction("Index", "AdLogin");
            }

            else
            {
                var dao = new LotDao();
                var model = dao.ListAllPagingEnd30(searchString, page, pageSize);
                ViewBag.searchString = searchString;
                return View(model);
            }

        }
        public ActionResult ListLotLink(string searchString, int page = 1, int pageSize = 10)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var dao = new LotDao();
                var model = dao.ListAllPagingLink(searchString, page, pageSize);
                ViewBag.searchString = searchString;
                return View(model);
            }

        }
        public ActionResult DeleteLot(int id)
        {
            var user = (UserLogin)Session["AD"];
            if (Session["AD"] == null || user.Role == 2)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                db = new AuctionDBContext();
                var date = DateTime.Now.AddDays(-180);

                var lot = db.Lots.Where(x => x.ID == id && x.TimeForBidEnd < date).SingleOrDefault();
                if (lot == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var listproduct = db.Products.Where(x => x.LotID == lot.ID);
                    if (listproduct != null)
                    {
                        List<ProductsImage> listImage = new List<ProductsImage>();
                        foreach (var item in listproduct)
                        {
                            var listimg = db.ProductsImages.Where(x => x.ProductId == item.ID);
                            if (listimg != null)
                            {
                                listImage.AddRange(listimg);
                            }
                        }
                        if (listImage != null)
                        {
                            foreach (var item in listImage)
                            {
                                string path = Path.Combine(Server.MapPath(item.Image)); ;

                                if (!Directory.Exists(path))
                                {

                                    System.IO.File.Delete(path);
                                }
                            }
                            db.ProductsImages.RemoveRange(listImage);


                        }
                        db.Products.RemoveRange(listproduct);


                    }
                    var listRegister = db.RegisterBids.Where(x => x.LotID == lot.ID);
                    if (listRegister != null)
                    {
                        foreach (var item in listRegister)
                        {
                            var listAuction = db.Auctions.Where(x => x.RegisterBidID == item.ID);
                            if (listAuction != null)
                            {
                                db.Auctions.RemoveRange(listAuction);

                            }
                        }
                        db.RegisterBids.RemoveRange(listRegister);


                    }
                    var listAttachment = db.LotAttachments.Where(x => x.LotID == lot.ID);
                    if (listAttachment != null)
                    {
                        db.LotAttachments.RemoveRange(listAttachment);
                    }
                    string path2 = Path.Combine(Server.MapPath(lot.LotImage)); ;
                    if (!Directory.Exists(path2))
                    {

                        System.IO.File.Delete(path2);
                    }
                    db.Lots.Remove(lot);
                    db.SaveChanges();
                }

                return RedirectToAction("ListLotEndFor180");
            }

        }
        public ActionResult DeleteLink(int id)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                db = new AuctionDBContext();
                var link = db.LotAttachments.Find(id);
                if (link != null)
                {
                    db.LotAttachments.Remove(link);
                    db.SaveChanges();
                }



                return RedirectToAction("ListLotLink");
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
            db = new AuctionDBContext();
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            if (lot.TimeForRegisterStart > lot.TimeForRegisterEnd)
            {
                lot.ListCategory = db.Categories.ToList();
                ViewBag.fail = "Ngày kết thúc đăng ký phải lớn ngày bắt đầu đăng ký!";
                return View(lot);
            }
            if (lot.TimeForBidStart > lot.TimeForBidEnd)
            {
                lot.ListCategory = db.Categories.ToList();
                ViewBag.fail = "Ngày kết thúc đấu giá phải lớn ngày bắt đầu đấu !";
                return View(lot);
            }
            if (lot.TimeForRegisterEnd > lot.TimeForBidStart)
            {
                lot.ListCategory = db.Categories.ToList();
                ViewBag.fail = "Ngày kết bắt đầu đấu giá phải lớn người kết thúc đăng ký đấu giá !";
                return View(lot);
            }
            if (ModelState.IsValid)
            {
                db = new AuctionDBContext();


                lot.ListCategory = db.Categories.ToList();
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
        [HttpGet]
        public ActionResult CreateLink()
        {
            if (Session["AD"] == null)
            {

                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                db = new AuctionDBContext();
                LotAttachment objLot = new LotAttachment();
                //ViewBag.banks = new SelectList(banks, "Id", "Name");
                objLot.ListLot = db.Lots.ToList();
                return View(objLot);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateLink(LotAttachment lotAttachment)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }

            else
            {
                db = new AuctionDBContext();
                lotAttachment.ListLot = db.Lots.ToList();
                if (ModelState.IsValid)
                {
                    LotAttachment attachment = new LotAttachment();
                    attachment.Name = lotAttachment.Name;
                    attachment.AttachmentLink = lotAttachment.AttachmentLink;
                    attachment.LotID = lotAttachment.LotID;
                    db.LotAttachments.Add(attachment);
                    db.SaveChanges();
                    ViewBag.success = "Thêm link đính kèm thành công";
                    return View(lotAttachment);
                }
                ViewBag.error = "Thêm link đính kèm bị lỗi rồi";
                return View(lotAttachment);
            }
        }
        [HttpGet]
        public ActionResult EditLink(int id)
        {
            if (Session["AD"] == null)
            {

                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                db = new AuctionDBContext();
                var objLot = db.LotAttachments.Find(id);
                if (objLot == null)
                {
                    return RedirectToAction("ListLotLink");
                }
                //ViewBag.banks = new SelectList(banks, "Id", "Name");
                objLot.ListLot = db.Lots.ToList();
                return View(objLot);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLink(LotAttachment lotAttachment)
        {
            if (Session["AD"] == null)
            {

                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                db = new AuctionDBContext();
                lotAttachment.ListLot = db.Lots.ToList();

                var attachment = db.LotAttachments.Find(lotAttachment.ID);
                if (attachment != null)
                {
                    attachment.Name = lotAttachment.Name;
                    attachment.AttachmentLink = lotAttachment.AttachmentLink;
                    attachment.LotID = lotAttachment.LotID;
                    db.LotAttachments.AddOrUpdate(attachment);
                    db.SaveChanges();
                    ViewBag.success = "Sửa link đính kèm thành công";
                    return View(lotAttachment);
                }
                //ViewBag.banks = new SelectList(banks, "Id", "Name");
                ViewBag.error = "Bị lỗi rồi -_-";
                return View(lotAttachment);
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


        //EDIT LOT============================================================
        [HttpGet]
        public ActionResult EditLot(int id)
        {
            ViewBag.success = null;
            var user = (UserLogin)Session["AD"];
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                db = new AuctionDBContext();
                var lot = db.Lots.Find(id);
                if(user.Role == 2 && lot.HostLot != user.UserID)
                {
                    return RedirectToAction("ListLot");
                }

                lot.ListCategory = db.Categories.ToList();

                return View(lot);
            }
        }

        public ActionResult SendMailRegister(int LotID, string Url)
        {

            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var user = (UserLogin)Session["AD"];

                db = new AuctionDBContext();
                var lot = db.Lots.Find(LotID);
                if (user.Role == 2 && lot.HostLot != user.UserID)
                {
                    return RedirectToAction("ListLotRegister");
                }
                    var listRegister = db.RegisterBids.Where(x => x.LotID == LotID && x.Status == 1).ToList();
                if (listRegister != null)
                {
                    foreach (var item in listRegister)
                    {
                        try
                        {
                            string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/dangthanhcong.html"));
                            content = content.Replace("{{TenPhienDauGia}}", lot.Name);
                            content = content.Replace("{{CustomerName}}", item.User.FullName);
                            content = content.Replace("{{Thoigianbatdau}}", lot.TimeForBidStart.ToString());
                            content = content.Replace("{{Thoiketthuc}}", lot.TimeForBidEnd.ToString());
                            string tb = "Đăng ký tham giá phiên đấu giá " + lot.Name + " thành công";
                            new MailHelper().SendMail(item.User.Email, tb, content);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            return Redirect(Url);
                        }
                    }
                }
                return Redirect(Url);
            }
        }
        public ActionResult SendMailLotNew(int LotID, string Url)
        {

            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var user = (UserLogin)Session["AD"];

                db = new AuctionDBContext();
                var lot = db.Lots.Find(LotID);
                if (user.Role == 2 && lot.HostLot != user.UserID)
                {
                    return RedirectToAction("ListLotRegister");
                }
                var listUser = db.Users.Where(x => x.RoleID == 3).ToList();
                if (listUser != null)
                {
                    foreach (var item in listUser)
                    {
                        try
                        {
                            
                            var domain = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority); 
                            var img = domain + lot.LotImage;
                            var link = domain + "/phien-dau/" + lot.SiteTile + "-" + lot.ID;
                            string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/thongbaolotnew.html"));
                            content = content.Replace("{{TenPhienDauGia}}", lot.Name);
                            content = content.Replace("{{CustomerName}}", item.FullName);
                            content = content.Replace("{{hinhanh}}", img.ToString());
                            content = content.Replace("{{link}}", link.ToString());
                            content = content.Replace("{{Thoigianbatdau}}", lot.TimeForRegisterStart.ToString());
                            content = content.Replace("{{Thoiketthuc}}", lot.TimeForRegisterEnd.ToString());
                            string tb = "Có phiên đấu giá " + lot.Category.Name + " mới có thể bạn quan tâm";
                            new MailHelper().SendMail(item.Email, tb, content);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            return Redirect(Url);
                        }
                    }
                }
                return Redirect(Url);
            }
        }
        public ActionResult SendMailAuction(int LotID, string Url)
        {

            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                var user = (UserLogin)Session["AD"];
                var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                db = new AuctionDBContext();
                var lot = db.Lots.Find(LotID);
                if (user.Role == 2 && lot.HostLot != user.UserID)
                {
                    return RedirectToAction("ListLotRegister");
                }
                var AuctionNull = db.Auctions.Where(x => x.Status == 0 && x.RegisterBid.LotID == LotID).ToList();
                if (AuctionNull.Count() > 0)
                {
                    return Redirect(Url);
                }
                var listRegister = db.RegisterBids.Where(x => x.LotID == LotID && x.Status == 1).ToList();
                var AuctionWin = db.Auctions.Where(x => x.Status == 1 && x.RegisterBid.LotID == LotID).FirstOrDefault();
                RegisterBid registerBidWin = null;
                List<RegisterBid> registerBidFail = null;
                if (AuctionWin != null)
                {
                    registerBidWin = db.RegisterBids.Where(x => x.LotID == LotID && x.Status == 1 && x.ID == AuctionWin.RegisterBidID).FirstOrDefault();
                    registerBidFail = db.RegisterBids.Where(x => x.LotID == LotID && x.Status == 1 && x.ID != AuctionWin.RegisterBidID).ToList();
                }


                if (registerBidFail != null)
                {
                    foreach (var item in registerBidFail)
                    {
                        try
                        {

                            string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/daugiafail.html"));
                            content = content.Replace("{{TenPhienDauGia}}", lot.Name);
                            content = content.Replace("{{CustomerName}}", item.User.FullName);
                            content = content.Replace("{{Sotiencoc}}", string.Format(info, "{0:0,0}", lot.AdvanceDesposit));

                            string tb = "Đấu giá thất bại " + lot.Name;
                            new MailHelper().SendMail(item.User.Email, tb, content);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            return Redirect(Url);
                        }
                    }
                }
                if (registerBidWin != null)
                {

                    try
                    {

                        string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/daugiasuccess.html"));
                        content = content.Replace("{{TenPhienDauGia}}", lot.Name);
                        content = content.Replace("{{CustomerName}}", registerBidWin.User.FullName);
                        content = content.Replace("{{tienthangcuoc}}", string.Format(info, "{0:0,0}", AuctionWin.PriceBid));

                        string tb = "Đấu giá thành công " + lot.Name;
                        new MailHelper().SendMail(registerBidWin.User.Email, tb, content);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return Redirect(Url);
                    }

                }
                return Redirect(Url);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*        [ValidateInput(false)]*/
        public ActionResult EditLot(Lot lot, HttpPostedFileBase file1)
        {
            db = new AuctionDBContext();
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            if (lot.TimeForRegisterStart > lot.TimeForRegisterEnd)
            {
                lot.ListCategory = db.Categories.ToList();
                ViewBag.fail = "Ngày kết thúc đăng ký phải lớn ngày bắt đầu đăng ký!";
                return View(lot);
            }
            if (lot.TimeForBidStart > lot.TimeForBidEnd)
            {
                lot.ListCategory = db.Categories.ToList();
                ViewBag.fail = "Ngày kết thúc đấu giá phải lớn ngày bắt đầu đấu !";
                return View(lot);
            }
            if (lot.TimeForRegisterEnd > lot.TimeForBidStart)
            {
                lot.ListCategory = db.Categories.ToList();
                ViewBag.fail = "Ngày kết bắt đầu đấu giá phải lớn người kết thúc đăng ký đấu giá !";
                return View(lot);
            }
            db = new AuctionDBContext();
            lot.ListCategory = db.Categories.ToList();
            ModelState.Remove("file1");
            ModelState.Remove("CateID");
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

                editLot.TimeForRegisterStart = lot.TimeForRegisterStart;
                editLot.TimeForRegisterEnd = lot.TimeForRegisterEnd;
                editLot.TimeForBidStart = lot.TimeForBidStart;
                editLot.TimeForBidEnd = lot.TimeForBidEnd;
                editLot.SiteTile = FriendlyURL.URLFriendly(lot.Name);
                editLot.HostName = lot.HostName;
                if (file1 != null)
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

            return View(lot);
        }
        public ActionResult LockOrUnlockLot(int id)
        {
            var user = (UserLogin)Session["AD"];
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            if (user.Role == 2)
            {
                return RedirectToAction("ListLot");
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
        }
        public ActionResult ConfirmRegitsterOfLot(int LotId, int UserId, string Url)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            db = new AuctionDBContext();
            var id = LotId;
            var user = (UserLogin)Session["AD"];
            var lot = db.RegisterBids.Where(x => x.LotID == id && x.UserID == UserId).FirstOrDefault();
            if(user.Role == 2 && lot.Lot.HostLot != user.UserID)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            if (lot.Status == 0)
            {
                lot.Status = 1;
            }
            else
            {
                lot.Status = 0;
            }
            db.RegisterBids.AddOrUpdate(lot);
            db.SaveChanges();
            return Redirect(Url);
        }
        public ActionResult ConfirmEndOfLot(int LotId, int UserId, string Url)
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            db = new AuctionDBContext();
            var id = LotId;
            var user = (UserLogin)Session["AD"];
            var RegisterBids = db.RegisterBids.Where(x => x.LotID == id && x.UserID == UserId).FirstOrDefault();
            if (RegisterBids.Status == 0)
            {
                return RedirectToAction(Url);
            }
            if (user.Role == 2 && RegisterBids.Lot.HostLot != user.UserID)
            {
                return RedirectToAction(Url);
            }
            if (RegisterBids.Status == 1)
            {
                RegisterBids.Status = 2;
                db.RegisterBids.AddOrUpdate(RegisterBids);
                db.SaveChanges();
                return Redirect(Url);
         
            }
            if (RegisterBids.Status == 1)
            {
                RegisterBids.Status = 2;
                db.RegisterBids.AddOrUpdate(RegisterBids);
                db.SaveChanges();
                return Redirect(Url);

            }
            return Redirect(Url);
        }
        public ActionResult ConfirmAuctionOfLot(int RegisterBidID, long PriceBid, string Url)
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
                foreach (var item in auctions)
                {
                    item.Status = 2;
                    db.Auctions.AddOrUpdate(auction);
                }
                auction.Status = 1;
            }
            else if (auction.Status == 2)
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