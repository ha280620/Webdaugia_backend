namespace Webdaugia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Auction")]
    public partial class Auction
    {
        public int ID { get; set; }

        public int RegisterBidID { get; set; }

        public long PriceBid { get; set; }

        public DateTime BidTime { get; set; }

        public bool? Status { get; set; }

        public virtual RegisterBid RegisterBid { get; set; }
    }
}
