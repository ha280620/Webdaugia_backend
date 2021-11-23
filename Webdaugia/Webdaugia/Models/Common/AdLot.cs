using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webdaugia.Models.Common
{
    public class AdLot
    {

        public int? ID { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập tên phiên đấu giá")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bạn  cần nhập bước giá")]
        public long? MiniumBid { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập giá khởi điểm")]
        public long? StartingPrice { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập tiền cọc")]
        public long? AdvanceDesposit { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập phí tham gia")]
        public int? ParticipationFee { get; set; }

        public int? HostLot { get; set; }

        [Required(ErrorMessage = "Bạn cần nhập ngày kết thúc đăng ký")]
        public DateTime? TimeForRegisterEnd { get; set; }

        [Required(ErrorMessage = "Bạn cần nhập ngày bắt đầu đấu giá")]
        public DateTime? TimeForBidEnd { get; set; }

        [Required(ErrorMessage = "Bạn cần nhập ngày kết thúc đấu giá")]

        public DateTime? TimeForBidStart { get; set; }

        [Required(ErrorMessage = "Bạn cần nhập ngày bắt đầu đăng ký")]
        public DateTime? TimeForRegisterStart { get; set; }


        public bool? Status { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn thể loại đấu giá")]
        public int? CateID { get; set; }


        [StringLength(200)]
        [Required(ErrorMessage = "Vui lòng nhập chủ tài sản")]
        public string HostName { get; set; }

        [StringLength(158)]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Location { get; set; }

        [StringLength(158)]
        [Required(ErrorMessage = "Vui lòng nhập thời gian xem sản phẩm")]
        public string ViewInTime { get; set; }

        public string LotImage;
        [Required(ErrorMessage = "Vui lòng chọn ảnh")]
        public HttpPostedFileBase file1 { get; set; }

        public List<Category> ListCategory = new List<Category>();

    }
}