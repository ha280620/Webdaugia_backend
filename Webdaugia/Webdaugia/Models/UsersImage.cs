namespace Webdaugia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UsersImage
    {
        public int ID { get; set; }

        public int? UsersID { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        public virtual User User { get; set; }
    }
}
