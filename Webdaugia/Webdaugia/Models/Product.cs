namespace Webdaugia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            ProductsImages = new HashSet<ProductsImage>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public int? LotID { get; set; }

        public virtual Lot Lot { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductsImage> ProductsImages { get; set; }
        public List<Lot> ListLot= new List<Lot>();
    }
}
