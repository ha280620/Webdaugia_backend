﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Webdaugia.Models;
using Webdaugia.DAO;
using Webdaugia.Models.Common;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;
using System.Configuration;
using System.Data.Entity.Migrations;
using Webdaugia.Models.Login;
using System.IO;
using System.Web.Services.Description;
using Webdaugia.Areas.Admin.Models;

namespace Webdaugia.Controllers
{
    [HandleError]
    public class LoginController : Controller
    {
        //public static User userid = null;
        // GET: Login
        //AuctionDBContext data = new AuctionDBContext();
        string FilePath = "";
        AuctionDBContext db = null;
        [HttpGet]
        public ActionResult DangKi()
        {
            if (Session["USER"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult DangKi(RegisterModel model)
        {
            var dao = new UserDao();
            if (ModelState.IsValid)
            {   

                var temp = dao.getByUserName(model.username);
                var em = dao.getByUserEmail(model.email);
                var pw = dao.getByUserPhone(model.phone);
                if (pw != null)
                {
                    ModelState.AddModelError("phone", "Số điện thoại đã tồn tại!");
                }
                if (em != null)
                {
                    ModelState.AddModelError("email", "Email đã tồn tại!");
                }
                if (temp != null)
                {
                    ModelState.AddModelError("username", "Tên đăng nhập đã tồn tại!");

                }
                if (pw != null || em != null || temp != null)
                {
                    return View(model);
                }    
                else
                {
                    db = new AuctionDBContext();
                   
                    User user = new User();
                  
                    user.Username = model.username.Trim();
                    user.FullName = model.name;
                    user.Password = MD5Encryptor.MD5Hash(model.password).Trim();
                    user.RoleID = 3;
                    user.Email = model.email;
                    user.Phone = model.phone;
                    
                    user.Status = 0;
                
                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công !";
                        //model = new RegisterModel();
                        var userSession = new UserLogin();
                        userSession.UserID = result;
                        userSession.UserName = user.Username;
                        userSession.Name = user.FullName;
                        userSession.Status = user.Status;

                        var img = "Content\\User\\User00.png";
                        UsersImage usersImage = new UsersImage();
                        usersImage.UsersID = result;
                        usersImage.Image = img;
                        db.UsersImages.Add(usersImage);
                        db.SaveChanges();
                        Session.Add("USER", userSession);
                        return RedirectToAction("Themthongtin", "Login");
                        //return View("Themthongtin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công!");
                    }
                }
            }

            return View(model);
        }

        // GET: Login

        //[HttpGet]
        public ActionResult DangNhap()
        {
            if (Session["USER"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(LoginModel model)
        {
            var dao = new UserDao();
            if (ModelState.IsValid)
            {
                var result = dao.Login(model.Username.Trim(), MD5Encryptor.MD5Hash(model.Password), 3);

                if (result == 1)
                {

                    var user = dao.getByUserName(model.Username);
                    var userSession = new UserLogin();
                    userSession.UserID = user.ID;
                    userSession.UserName = user.Username;
                    userSession.Name = user.FullName;
                    userSession.Status = user.Status;
                    //userSession.ProfileImage = user.ProfileImage;
                    Session.Add("USER", userSession);
                    //userid = (User)Session["USER"];
                    if(user.RoleID == 3)
                    {
                        if (user.CMND == null)
                        {
                     
                            //return View("Themthongtin");
                            return RedirectToAction("Themthongtin", "Login");
                            //return RedirectToAction("DangKi", "Login");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    } 
                        
                    //if (user.CMND == null)
                    //{
                    //    //return View("Themthongtin");
                    //    return RedirectToAction("DangKi", "Login");
                    //}

                    //if (user.RoleID == 3)
                    //    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đã bị khóa!");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng!");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không đúng!");
                }

            }

            return View("DangNhap");
        }
        //----------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Themthongtin()
        {
           db = new AuctionDBContext();
            AddInfoModel objbank = new AddInfoModel();
         
            
            //ViewBag.banks = new SelectList(banks, "Id", "Name");
            objbank.ListCategory = db.Banks.ToList(); 
            return View(objbank);
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
            FilePath = Server.MapPath("~/imgcmnd/");
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Themthongtin(AddInfoModel model, HttpPostedFileBase cmndfront, HttpPostedFileBase cmndback)
        {
            var dao = new UserDao();
            ModelState.Remove("cmndfront");
            ModelState.Remove("cmndback");
            ModelState.Remove("gt");
            if (!ModelState.IsValid)
            {
                db = new AuctionDBContext();
                model.ListCategory = db.Banks.ToList();
                return View(model);
            }
            if (Session["USER"] != null)
            {
                ATM atm = new ATM();
                //Bank bank = new Bank();
                UserLogin userid = (UserLogin)Session["USER"];
                User user = dao.getUserById(userid.UserID);
                AuctionDBContext db = new AuctionDBContext();
                user.Address = model.diachi;
                user.CMND = model.cmnd.ToString();
                user.LocationCMND = model.noicapcmnd;
                user.DayCMND = model.ngaycapcmnd;
                user.Birthday = model.ns;
                user.Gender = model.gt;
                atm.ATMCode = model.atmcode;
                atm.ATMFullName = model.atmfullname;
                atm.UserID = user.ID;
                //var bakid = model.tennganhang;
                atm.BankId = model.tennganhang;
                //var fileName = Path.GetFileName(cmndfront.FileName);
                //var fileName1 = Path.GetFileName(cmndback.FileName);

                //var path = Path.Combine(Server.MapPath("~/images"), fileName);
                //var path1 = Path.Combine(Server.MapPath("~/images"), fileName1);

                string fileName = UploadFile(cmndfront);
                string fileName1 = UploadFile(cmndback);
                //cmndfront.SaveAs(path);
                //cmndback.SaveAs(path1);
                user.ImageFront = "/imgcmnd/" + fileName;
                user.ImageBack = "/imgcmnd/" + fileName1;
                db.ATMs.AddOrUpdate(atm);
                //db.Banks.AddOrUpdate(bank);
                db.Users.AddOrUpdate(user);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
            return RedirectToAction("DangXuat", "Login");
            //return View(model);
        }

        //[HttpPost]
        //public ActionResult ThemthongtinID(FormCollection form)
        //{
        //    var dao = new UserDao();
        //    if (Session["USER"] != null)
        //    {
        //        UserLogin userid = (UserLogin)Session["USER"];
        //        User user = dao.getUserById(userid.UserID);
        //        //ATM atm = dao.getATMById(userid.UserID);
        //        //Bank bank = dao.getBankById(userid.UserID);
        //        AuctionDBContext db = new AuctionDBContext();
        //        user.Address = form["diachi"];
        //        user.CMND = form["cmnd"];
        //        user.LocationCMND = form["noicapcmnd"];
        //        user.ImageFront = form["cmndfront"];
        //        user.ImageBack = form["imgcmndback"];
        //        //MemoryStream stream = new MemoryStream();
        //        //user.DayCMND = form["ngaycapcmnd"];
        //        //user.Gender = form["gt"];
        //        //user.Birthday = form["ns"];
        //        //atm.ATMCode = form["atmcode"];
        //        //atm.ATMFullName = form["atmfullname"];
        //        //bank.Name = form["tennganhang"];
        //        dao.Update(user);
        //        //dao.Update(bank);
        //        //dao.Update(atm);



        //    }
        //    return View("Index","Login");
        //}
        //-----------------------------------------------------------------------------------------------------------
        public ActionResult DangXuat()
        {
            Session["USER"] = null;
            return RedirectToAction("DangNhap", "Login");
        }
        [HttpGet]
        public ActionResult Quenmatkhau()
        {
            //if (Session["USER"] != null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            return View();
        }
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        [HttpPost]
        [ActionName("Quenmatkhau")]
        public ActionResult Quenmatkhau(string email)
        {
            var emailexist = IsEmailExist(email);
            if (emailexist)
            {
                AuctionDBContext db = new AuctionDBContext();
                var User = db.Users.Where(a => a.Email == email).FirstOrDefault();
                var resetPassword = CreatePassword(10);
                User.Password = MD5Encryptor.MD5Hash(resetPassword);
                db.Users.AddOrUpdate(User);
                db.SaveChanges();
                try
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/neworder.html"));
                    content = content.Replace("{{CustomerName}}", resetPassword);
                    content = content.Replace("{{Email}}", email);
                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    new MailHelper().SendMail(email, "Thông báo từ web đấu giá", content);
                    new MailHelper().SendMail(toEmail, "Thông báo từ web đấu giá", content);
                }
                catch (Exception ex)
                {
                    return View("DangNhap");
                }
            }
            else
            {
                return View("DangKi");
            }

            return View("DangNhap");
        }
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (AuctionDBContext dc = new AuctionDBContext())
            {
                var v = dc.Users.Where(a => a.Email == emailID).FirstOrDefault();
                return v != null;
            }
        }
        //------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult ProfileCustomer()
        {
            if (Session["USER"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            //int userid = ((UserLogin)Session["USER"]).UserID;
            UserLogin userid = (UserLogin)Session["USER"];
            var dao = new UserDao();
            User user = dao.getUserById(userid.UserID);
            //var user = dao.getUserById(userid);
            return View(user);
        }
        //Update Profile
        [HttpPost, ActionName("ProfileCustomer")]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileCustomera(User collection)
        {
            db = new AuctionDBContext();
            UserLogin userid = (UserLogin)Session["USER"];
            var dao = new UserDao();
            User user = dao.getUserById(userid.UserID);
            try
            {
                var errors = ModelState.Values.SelectMany(b => b.Errors);
                if (!ModelState.IsValid)
                {
                    user.Email = collection.Email;
                    user.Phone = collection.Phone;
                    var result = dao.Update(user);
                    if (result)
                    {
                        ViewBag.Success = "Cập nhật thông tin thành công!";
                    }
                    else
                    {
                        ViewBag.Fail = "Cập nhật thông tin thất bại!";
                    }
                }
            return View(collection);
             }
            catch
            {
                return View();
            }
        }
        //--------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult ChangePassCustomer()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassCustomer(PasswordModel model)
        {
            if (Session["USER"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            db = new AuctionDBContext();
            UserLogin userid = (UserLogin)Session["USER"];
            var dao = new UserDao();
            User user = dao.getUserById(userid.UserID);
            //int userid = ((UserLogin)Session["USER"]).UserID;
            //var user = dao.getUserById(userid);
      
            try
            {
                var errors = ModelState.Values.SelectMany(b => b.Errors);
                if (ModelState.IsValid)
                {
                    if (user.Password.Trim() == MD5Encryptor.MD5Hash(model.password.Trim()))
                    {
                        if(model.password != model.newpassword)
                        {

                         
                        //Update Password
                            user.Password = MD5Encryptor.MD5Hash(model.newpassword.Trim());

                            db.Users.AddOrUpdate(user);
                            try
                            {
                                db.SaveChanges();
                                ViewBag.Success = "Đổi mật khẩu thành công";
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Fail = "Đổi mật khẩu thất bại";
                            }
                        }
                        else
                        {
                            ViewBag.WrongPass = "Mật khẩu cũ không được giống mật khẩu mới!";
                        }    
                    }
                    else
                    {
                        ViewBag.WrongPass = "Mật khẩu cũ không đúng";
                    }

                    //save change
                }
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        //public ActionResult AccountManagement()
        //{
        //    db = new AuctionDBContext();
        //    List<Lot> listlot = db.Lots.ToList();
        //    return View(listlot);
        //}

        public ActionResult AccountManagement(string searchString, int page = 1, int pageSize = 10)
        {
            if (Session["USER"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                //UserLogin userid = (UserLogin)Session["USER"];
                //var dao1 = new UserDao();
                //User user = dao1.getUserById(userid.UserID);
                //var Order = db.RegisterBids.Where(x => x.UserID == user.ID).ToList();
                //var Order = db.RegisterBids.Where(x => x.UserID == userid.UserID).ToList();

                var dao = new LotDao();
                var model = dao.ListAllPagingRegisterOfLot1(searchString, page, pageSize);
                ViewBag.searchString = searchString;
                return View(model);
            }

        }

        public ActionResult Index()
        {
            if (Session["USER"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}