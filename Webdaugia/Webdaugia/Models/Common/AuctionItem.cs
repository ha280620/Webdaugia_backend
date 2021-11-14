using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webdaugia.Models.Common
{
    public class AuctionItem
    {
        public int RegisterBidID { get; set; }
        public DateTime BidTime {set; get; }
        public long Quantity { set; get; }
        public long? HighBid { get; set; }
        public long? MiniumBid { get; set; }
        public long? PriceBid {
            get { return HighBid + (MiniumBid * Quantity); }
        }
    }
}