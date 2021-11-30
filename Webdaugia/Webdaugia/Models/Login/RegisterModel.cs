using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webdaugia.Models.Login
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "Tài khoản có ít nhất 6 và nhỏ hơn 16 kí tự!")]
        public string username { set; get; }
        [Required(ErrorMessage = "Vui lòng nhập Họ và Tên")]
        public string name { set; get; }
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string email { set; get; }
 
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [StringLength(11, ErrorMessage = "Vui lòng nhập đúng số điện thoại!")]
        public string phone { set; get; }
        [Required(ErrorMessage = "Vui lòng nhận mật khẩu")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 kí tự!")]
        public string password { set; get; }
        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [Compare("password", ErrorMessage = "Mật khẩu không trùng khớp!")]

        public string confirmpassword { set; get; }
    }
}