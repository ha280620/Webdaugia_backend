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
using Webdaugia.Models.Login;
using System.Web.Helpers;

namespace Webdaugia.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        //AuctionDBContext data = new AuctionDBContext();

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
                if (temp != null)
                {
                    ModelState.AddModelError("username", "Tên đăng nhập đã tồn tại!");
                }
                else
                {
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
                        ViewBag.Success = "Đăng ký thành công mời đăng nhập lại!";
                        model = new RegisterModel();
                        return View("Themthongtin");
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
                var result = dao.Login(model.Username, MD5Encryptor.MD5Hash(model.Password), 3);
                if (result == 1)
                {
                    var user = dao.getByUserName(model.Username);
                    var userSession = new UserLogin();
                    userSession.UserID = user.ID;
                    userSession.UserName = user.Username;
                    userSession.Name = user.FullName;
                    //userSession.ProfileImage = user.ProfileImage;
                    Session.Add("USER", userSession);

                    if (user.CMND == null)
                    {
                        return View("Themthongtin");
                    }

                    if (user.RoleID == 3)
                        return RedirectToAction("Index","Home");
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

        [HttpGet]
        //public ActionResult VerifyAccount(string id)
        //{
        //    int Status = 0;
        //    using (AuctionDBContext dc = new AuctionDBContext())
        //    {
        //        dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
        //                                                        // Confirm password does not match issue on save changes
        //        var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
        //        if (v != null)
        //        {
        //            v.IsEmailVerified = true;
        //            dc.SaveChanges();
        //            Status = 1;
        //        }
        //        else
        //        {
        //            ViewBag.Message = "Invalid Request";
        //        }
        //    }
        //    ViewBag.Status = Status;
        //    return View();
        //}

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (AuctionDBContext dc = new AuctionDBContext())
            {
                var v = dc.Users.Where(a => a.Email == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/User/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("dotnetawesome@gmail.com", "Dotnet Awesome");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "******"; // Replace with actual password

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/>br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        [HttpPost]

        //public ActionResult Quenmatkhau(string EmailID)
        //{
        //    //Verify Email ID
        //    //Generate Reset password link 
        //    //Send Email 
        //    string message = "";
        //    int Status = 0;

        //    using (AuctionDBContext dc = new AuctionDBContext())
        //    {
        //        var account = dc.Users.Where(a => a.Email == EmailID).FirstOrDefault();
        //        if (account != null)
        //        {
        //            //Send email for reset password
        //            string resetCode = Guid.NewGuid().ToString();
        //            SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
        //            account.ResetPasswordCode = resetCode;
        //            //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
        //            //in our model class in part 1
        //            dc.Configuration.ValidateOnSaveEnabled = false;
        //            dc.SaveChanges();
        //            message = "Reset password link has been sent to your email id.";
        //        }
        //        else
        //        {
        //            message = "Account not found";
        //        }
        //    }
        //    ViewBag.Message = message;
        //    return View();
        //}

        //public ActionResult ResetPassword(string id)
        //{
        //    //Verify the reset password link
        //    //Find account associated with this link
        //    //redirect to reset password page
        //    if (string.IsNullOrWhiteSpace(id))
        //    {
        //        return HttpNotFound();
        //    }

        //    using (AuctionDBContext dc = new AuctionDBContext())
        //    {
        //        var user = dc.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
        //        if (user != null)
        //        {
        //            ResetPasswordModel model = new ResetPasswordModel();
        //            model.ResetCode = id;
        //            return View(model);
        //        }
        //        else
        //        {
        //            return HttpNotFound();
        //        }
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ResetPassword(ResetPasswordModel model)
        //{
        //    var message = "";
        //    if (ModelState.IsValid)
        //    {
        //        using (AuctionDBContext dc = new AuctionDBContext())
        //        {
        //            var user = dc.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
        //            if (user != null)
        //            {
        //                user.Password = Crypto.Hash(model.NewPassword);
        //                user.ResetPasswordCode = "";
        //                dc.Configuration.ValidateOnSaveEnabled = false;
        //                dc.SaveChanges();
        //                message = "New password updated successfully";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        message = "Something invalid";
        //    }
        //    ViewBag.Message = message;
        //    return View(model);
        //}


      
        //--------------------------------------------------------------------------------

        [HttpGet]

        public ActionResult Themthongtin()
        {
            return View();
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