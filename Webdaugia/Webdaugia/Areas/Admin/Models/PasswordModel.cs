﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webdaugia.Areas.Admin.Models
{
    public class PasswordModel
    {
        public string password { set; get; }
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 kí tự!")]
        //[Compare("password", ErrorMessage = "Mật khẩu mới không được giống với mật khẩu cũ!")]
        public string newpassword { set; get; }
        [Compare("newpassword", ErrorMessage = "Mật khẩu không trùng khớp!")]
        public string confirmpassword { set; get; }
    }
}