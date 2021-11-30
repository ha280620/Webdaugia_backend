using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Webdaugia.Models.Login
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        [StringLength(16, MinimumLength = 5, ErrorMessage = "Tài khoản có ít nhất 5 và nhỏ hơn 16 kí tự!")]
        public string Username { set; get; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { set; get; }

    }
}