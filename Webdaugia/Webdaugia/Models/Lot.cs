namespace Webdaugia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Lot")]
    public partial class Lot
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lot()
        {
            LotAttachments = new HashSet<LotAttachment>();
            Products = new HashSet<Product>();
            RegisterBids = new HashSet<RegisterBid>();
        }

        public int ID { get; set; }

        [StringLength(200)]
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

        public int? AssetID { get; set; }

        public bool? Status { get; set; }
        [Required(ErrorMessage = "Vùi lòng chọn thể loại đấu giá")]
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

        [StringLength(128)]
        public string SiteTile { get; set; }

        [StringLength(250)]
        public string LotImage { get; set; }

        public long? HighBid { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LotAttachment> LotAttachments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisterBid> RegisterBids { get; set; }
        public List<Category> ListCategory = new List<Category>();
    }
}
