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
        public string Name { get; set; }

        [StringLength(50)]
        public string AttachmentLink { get; set; }

        public int? LotID { get; set; }

        public virtual Lot Lot { get; set; }
    }
}
