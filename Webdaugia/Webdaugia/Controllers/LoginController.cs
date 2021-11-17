using System;
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

namespace Webdaugia.Controllers
{
    [HandleError]
    public class LoginController : Controller
    {
        // GET: Login
        //AuctionDBContext data = new AuctionDBContext();
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
                        model = new RegisterModel();
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
            return View();
        }
        [HttpPost]
        public ActionResult Themthongtin(AddInfoModel model)
        {
            var dao = new UserDao();
            if (Session["USER"] != null)
            {
                
                UserLogin userid = (UserLogin)Session["USER"];
                User user = dao.getUserById(userid.UserID);
                AuctionDBContext db = new AuctionDBContext();
                user.Address = model.diachi;
                user.CMND = model.cmnd;
                user.LocationCMND = model.noicapcmnd;
                user.DayCMND = model.ngaycapcmnd;
                user.Birthday = model.ns;
                //user.Gender = model.gt;
                db.Users.AddOrUpdate(user);
                db.SaveChanges();

            }
            return RedirectToAction("Index", "Home");
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
        //--------------------------------------------------------------------------------


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