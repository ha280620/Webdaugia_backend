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

        public string Name { get; set; }

        public long? MiniumBid { get; set; }

        public long? StartingPrice { get; set; }
  
        public long? AdvanceDesposit { get; set; }

        public int? ParticipationFee { get; set; }

        public int? HostLot { get; set; }

        public DateTime? TimeForRegisterEnd { get; set; }

        public DateTime? TimeForBidEnd { get; set; }

        public DateTime? TimeForBidStart { get; set; }

        public DateTime? TimeForRegisterStart { get; set; }

        public int? AssetID { get; set; }

        public bool? Status { get; set; }

        public int? CateID { get; set; }


        [StringLength(200)]
     
        public string HostName { get; set; }

        [StringLength(158)]

        public string Location { get; set; }

        [StringLength(158)]
     
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
      /*  [Required(ErrorMessage = "Vui lòng chọn ảnh")]
        public string file1;*/
    }
}
