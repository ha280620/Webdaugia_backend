using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdaugia.Areas.Admin.Models;
using Webdaugia.DAO;
using Webdaugia.Models;
using Webdaugia.Models.Common;

namespace Webdaugia.Areas.Admin.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        // GET: Admin/Account
            AuctionDBContext db = null;
            // GET: Admin/Account
            public ActionResult ListAccount()
            {
                db = new AuctionDBContext();
                List<User> listUser = db.Users.Where(x => x.Status == 1 || x.Status == 2).ToList();
                return View(listUser);
            }
            public ActionResult ConfirmAccount()
            {
            db = new AuctionDBContext();
            List<User> listUser = db.Users.Where(x => x.Status == 0 && x.CMND != null).ToList();
            return View(listUser);
            }

        //[HttpGet]
        //public ActionResult Details(int id)
        //{
        //    ViewBag.success = null;
        //    if (Session["AD"] == null)
        //    {
        //        return RedirectToAction("Index", "AdLogin");
        //    }
        //    else
        //    {
        //        db = new AuctionDBContext();
        //        _ = db.Users.Find(id);

        //        List<User> listUser = db.Users.ToList();

        //        return RedirectToAction("listUser");
        //    }


        //}

        public ActionResult Details(int id)
        {
            ViewBag.success = null;
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            else
            {
                db = new AuctionDBContext();
                var user = from us in db.Users where us.ID == id select us;
                return View(user.SingleOrDefault());
            }


        }
        //View UpdateProfile
        [HttpGet]
        public ActionResult Profile()
        {
            if (Session["AD"] == null)
            {
                return RedirectToAction("Index", "AdLogin");
            }
            int userid = ((UserLogin)Session["AD"]).UserID;
            var dao = new UserDao();
            var user = dao.getUserById(userid);
            return View(user);
        }
            //Update Profile
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Profile(User collection)
            {
                db = new AuctionDBContext();
                int userid = ((UserLogin)Session["AD"]).UserID;
                var dao = new UserDao();
                var user = dao.getUserById(userid);
                try
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    if (!ModelState.IsValid)
                    {
                        //Update Profile
                        user.FullName = collection.FullName;
                        user.Gender = collection.Gender;
                        user.Phone = collection.Phone;
                        user.Birthday = collection.Birthday;
                        user.Email = collection.Email;
                        user.Address = collection.Address;
                        //save change
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
            //Change Password View
            [HttpGet]
            public ActionResult ChangePass()
            {
                return View();
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult ChangePass(PasswordModel model)
            {
                db = new AuctionDBContext();
                int userid = ((UserLogin)Session["AD"]).UserID;
                var dao = new UserDao();
                var user = dao.getUserById(userid);
                try
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    if (ModelState.IsValid)
                    {
                        if (user.Password.Trim() == MD5Encryptor.MD5Hash(model.password.Trim()))
                        {
                        if (model.password != model.newpassword)
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
                            Console.WriteLine(ex);
                                ViewBag.Success = "Đổi mật khẩu thất bại";
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

        //Lock and Unlock Account
            public ActionResult LockOrUnlockAccount(int id)
        {
            db = new AuctionDBContext();
            var user = db.Users.Where(x => x.ID == id).FirstOrDefault();
            if(user.Status == 1)
            {
                user.Status = 2;
            }
            else  if(user.Status == 2)
            {
                user.Status = 1;
            }      
            db.Users.AddOrUpdate(user);
            db.SaveChanges();
            return RedirectToAction("ListAccount");
        }

        public ActionResult ConfirmLockOrUnlockAccount(int id)
        {
            db = new AuctionDBContext();
            var user = db.Users.Where(x => x.ID == id).FirstOrDefault();
            if (user.Status == 1)
            {
                user.Status = 0;
            }
            else if (user.Status == 0)
            {
                user.Status = 1;
            }
            db.Users.AddOrUpdate(user);
            db.SaveChanges();
            return RedirectToAction("ConfirmAccount");
        }


        public ActionResult ConfirmIdetify(int id)
        {
            db = new AuctionDBContext();
            var user = db.Users.Where(x => x.ID == id && x.Status == 0).SingleOrDefault();
            var bank = db.ATMs.Where(x => x.UserID == id).ToList();
            if(user != null)
            {
                if(user.ImageFront != null)
                {
                    string path = Path.Combine(Server.MapPath(user.ImageFront)); ;
                    if (!Directory.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                if (user.ImageBack != null)
                {
                    string path1 = Path.Combine(Server.MapPath(user.ImageBack)); ;
                    if (!Directory.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                }
                user.CMND = null;
                user.DayCMND = null;
                user.Birthday = null;
                user.LocationCMND = null;

                user.ImageFront = null;
                user.ImageBack = null;
            }   
         
            db.Users.AddOrUpdate(user);
            if(bank != null)
            {
                db.ATMs.RemoveRange(bank);
            }
           
            db.SaveChanges();
            return RedirectToAction("ConfirmAccount");
        }
    }
}