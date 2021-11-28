namespace Webdaugia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LotAttachment
    {
        public int ID { get; set; }

        [StringLength(158)]
        [Required(ErrorMessage = "Bạn cần nhập tên link đính kèm")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bạn cần nhập link đính kèm")]
        public string AttachmentLink { get; set; }

        [Required(ErrorMessage = "Bạn vui lòng chọn phiên đấu giá")]
        public int? LotID { get; set; }

        public virtual Lot Lot { get; set; }
        public List<Lot> ListLot = new List<Lot>();
    }
}
