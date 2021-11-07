using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Webdaugia.Models;

namespace Webdaugia.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        dbQLDGDataContext data = new dbQLDGDataContext();

        [HttpGet]

        public ActionResult DangKi()
        {
            return View();
        }
        [ActionName("DangKi")]

        [HttpPost]
        public ActionResult DangKiPost(FormCollection collection)
        {
            User tk = new User();
            dbQLDGDataContext data = new dbQLDGDataContext();
            //gán giá trị từ các ô đến các biến
            var username = collection["username"];
            var name = collection["name"];
            var email = collection["email"];
            var sdt = collection["sdt"];
            var password = collection["password"];
            var confirmpassword = collection["confirmpassword"];

               if (String.IsNullOrEmpty(username))
                  {
                      ViewData["error-1"] = "Vui lòng điền đầy đủ tên tài khoản!";
                  }
                  else if (String.IsNullOrEmpty(name))
                  {
                      ViewData["error-2"] = "Vui lòng không bỏ trống Họ và tên!";
                  }
                  else if (String.IsNullOrEmpty(email))
                  {
                      ViewData["error-3"] = "Vui lòng nhập email!";
                  }
                  else if (String.IsNullOrEmpty(sdt))
                  {
                      ViewData["error-4"] = "Vui lòng nhập số điện thoại!";
                  }
                  else if (String.IsNullOrEmpty(password))
                  {
                      ViewData["error-5"] = "Vui lòng điền mật khẩu!";
                  }
                  else if (String.IsNullOrEmpty(confirmpassword))
                  {
                      ViewData["error-6"] = "Vui lòng nhập xác nhận mật khẩu!";
                  }

                  else

            {
                //gan gia tri ve doi tuong de luu vao database
                tk.Username = username;
                tk.RoleID = 3;
                tk.FullName = name;
                tk.Email = email;
                tk.Phone = sdt;
                tk.Password = encryptorPass(password);
                data.Users.InsertOnSubmit(tk);
                data.SubmitChanges();
                return RedirectToAction("DangNhap", "Login");
            }
            return View(this.DangKi());
        }

        // GET: Login

        [HttpGet]
        public ActionResult DangNhap()
        {

            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var username = collection["username"];
            var password = collection["password"];
            if (String.IsNullOrEmpty(username))
            {
                ViewData["error-1"] = "Vui lòng điền đầy đủ  tên!";
            }
            else if (String.IsNullOrEmpty(password))
            {
                ViewData["error-2"] = "Vui lòng không bỏ trống mật khẩu !";
            }
            else
            {
                User tk = data.Users.SingleOrDefault(n => n.Username == username && n.Password == encryptorPass(password));
                if (tk != null)
                {

                    ViewBag.Thongbao = "chúc mừng đăng nhập thành công";
                    Session["USERNAME"] = tk;
                        return RedirectToAction("DangKi", "Login");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";


            }
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        //mã hóa md5
        public static string encryptorPass(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }
    }
}