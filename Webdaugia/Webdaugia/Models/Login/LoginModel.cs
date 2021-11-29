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
        public string Username { set; get; }
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        public string Password { set; get; }

    }
}