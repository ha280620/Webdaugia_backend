using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webdaugia.Models.Common
{
    public class AdCategory
    {
  


        [StringLength(128)]
        [Required(ErrorMessage = "Bạn cần nhập tên loại")]
        public string Name { get; set; }
   
        [Required(ErrorMessage = "Bạn cần thêm ảnh loại")]
        public HttpPostedFileBase CateImg { get; set; }
        [Required(ErrorMessage = "Bạn cần thêm icon loại")]
   
        public HttpPostedFileBase CateIcon { get; set; }



       

    }
}