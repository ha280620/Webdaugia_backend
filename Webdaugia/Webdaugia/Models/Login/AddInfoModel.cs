using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webdaugia.Models.Login
{
    public class AddInfoModel
    {
        [Required(ErrorMessage ="Bạn cần nhập cmnd")]
        public long cmnd { set; get; }

        [Required(ErrorMessage = "Bạn cần nhập địa chỉ")]
        public string diachi { set; get; }

        [Required(ErrorMessage = "Bạn cần nhập nơi cấp cmnd")]
        public string noicapcmnd { set; get; }

        [Required(ErrorMessage = "Bạn cần nhập ngày cấp cmnd")]
        public DateTime ngaycapcmnd { set; get; }

        [Required(ErrorMessage = "Bạn cần nhập ngày sinh")]
        public DateTime ns { set; get; }

        [Required]
        public bool gt { set; get; }

        [Required(ErrorMessage = "Bạn up ảnh cmnd/cccd mặt trước")]
        public string cmndfront { set; get; }

        [Required(ErrorMessage = "Bạn up ảnh cmnd/cccd mặt sau")]
        public string cmndback { set; get; }

        [Required(ErrorMessage = "Bạn cần nhập số tài khoản")]
        public long atmcode { set; get; }

        [Required(ErrorMessage = "Bạn cần nhập tên tài khoản")]
        public string atmfullname { set; get; }

        [Required(ErrorMessage = "Bạn vui lòng chọn ngân hàng")]
        public int tennganhang { set; get; }
        public List<Bank> ListCategory = new List<Bank>();
    }
}