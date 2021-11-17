using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webdaugia.Models.Login
{
    public class AddInfoModel
    {
        public string cmnd { set; get; }
        public string diachi { set; get; }
        public string noicapcmnd { set; get; }
        public DateTime ngaycapcmnd { set; get; }
        public DateTime ns { set; get; }
        public bool gt { set; get; }
        public string cmndfront { set; get; }
        public string imgcmndback { set; get; }
        public string atmcode { set; get; }
        public string atmfullname { set; get; }
        public string tennganhang { set; get; }
    }
}