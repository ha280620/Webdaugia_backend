namespace Webdaugia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Organization
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(158)]
        public string Name { get; set; }

        [StringLength(20)]
        public string TaxCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IssuedOn { get; set; }

        [StringLength(158)]
        public string Location { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        public int? UserID { get; set; }

        public virtual User User { get; set; }
    }
}
