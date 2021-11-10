using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webdaugia.Models
{
    public class HomeModel
    {
        public List<Lot> listOnGoingLot { set; get; }
        public List<Lot> listReadyLot { set; get; }
        public List<Product> listProduct { set; get; }
        public List<Category> listCategory { set; get; }
    }
}