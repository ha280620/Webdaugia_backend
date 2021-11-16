namespace Webdaugia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            ATMs = new HashSet<ATM>();
            RegisterBids = new HashSet<RegisterBid>();
            UsersImages = new HashSet<UsersImage>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public int RoleID { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        public bool? Gender { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Birthday { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        public int? Status { get; set; }

        [StringLength(11)]
        public string Phone { get; set; }

        [StringLength(13)]
        public string CMND { get; set; }

        [StringLength(158)]
        public string LocationCMND { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DayCMND { get; set; }

        public string ImageFront { get; set; }

        public string ImageBack { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ATM> ATMs { get; set; }

        public virtual Organization Organization { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisterBid> RegisterBids { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsersImage> UsersImages { get; set; }
       
    }
}
