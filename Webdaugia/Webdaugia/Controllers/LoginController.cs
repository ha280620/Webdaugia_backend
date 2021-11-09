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

namespace Webdaugia.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        //AuctionDBContext data = new AuctionDBContext();

        [HttpGet]
        public ActionResult DangKi()
        {
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
                    user.Status = true;
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
            Session["username"] = null;
            return RedirectToAction("DangNhap", "Login");
        }
        [HttpGet]

        public ActionResult Quenmatkhau()
        {
            return View();
        }
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