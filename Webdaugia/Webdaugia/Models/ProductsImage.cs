namespace Webdaugia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductsImage
    {
        public int ID { get; set; }

        public int? ProductId { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        public virtual Product Product { get; set; }
    }
}
