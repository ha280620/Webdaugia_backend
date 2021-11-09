namespace Webdaugia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductsAttachment
    {
        public int ID { get; set; }

        [StringLength(10)]
        public string Attachment { get; set; }

        public int? ProductId { get; set; }
    }
}
