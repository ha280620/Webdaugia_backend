using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdaugia.DAO;
using Webdaugia.Models.Common;
using Webdaugia.Models.Login;

namespace Webdaugia.Areas.Admin.Controllers
{
    public class AdLoginController : Controller
    {
        public ActionResult Index()
        {
            if (Session["AD"] != null) return Redirect("/admin/account");
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var dao = new UserDao();
            if (ModelState.IsValid)
            {

                var result = dao.Login(model.Username, MD5Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.getByUserName(model.Username);
                    var userSession = new UserLogin();
                    userSession.UserID = user.ID;
                    userSession.Name = user.FullName;
                    userSession.Role = user.RoleID;
                    Session.Add("AD", userSession);
                    return RedirectToAction("Index", "AdminHome");
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
            return View("Index");
        }
        //LOGOUT==============================================================
        public ActionResult Logout()
        {
            Session["AD"] = null;
            return Redirect("/admin/adlogin");
        }
    }
}