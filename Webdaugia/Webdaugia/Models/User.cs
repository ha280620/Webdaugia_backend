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

        public int? RoleID { get; set; }

        //[StringLength(100)]
        [Required]
        [StringLength(100, ErrorMessage = "Họ tên không được quá 50 kí tự!")]
        public string FullName { get; set; }

        public bool? Gender { get; set; }

        [StringLength(100)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail không hợp lệ")]
        public string Email { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Vui lòng nhập Ngày hợp lệ!")]
        public DateTime? Birthday { get; set; }

        [StringLength(100, ErrorMessage = "Địa chỉ không được quá 100 kí tự!")]
        public string Address { get; set; }

        //  public int? Status { get; set; }
        public int? Status { get; set; }

        [StringLength(11, ErrorMessage = "Số điện thoại không được quá 11 kí tự!")]
        public string Phone { get; set; }

        [StringLength(13)]
        public string CMND { get; set; }

        [StringLength(158)]
        public string LocationCMND { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DayCMND { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ATM> ATMs { get; set; }

        public virtual Organization Organization { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisterBid> RegisterBids { get; set; }

        public virtual Role Role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsersImage> UsersImages { get; set; }
        public string ResetPasswordCode { get; internal set; }
    }
}
