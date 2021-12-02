namespace Webdaugia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ATM")]
    public partial class ATM
    {
        public int ID { get; set; }
        [StringLength(20)]
        public string ATMCode { get; set; }

        [StringLength(50)]
        public string ATMFullName { get; set; }

        public int? BankId { get; set; }

        public int? UserID { get; set; }

        public virtual Bank Bank { get; set; }

        public virtual User User { get; set; }
        public List<Bank> ListBank = new List<Bank>();
    }
}
