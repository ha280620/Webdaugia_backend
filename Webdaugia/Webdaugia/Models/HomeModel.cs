using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webdaugia.Models
{
    public class HomeModel
    {
        public List<Lot> listOnGoingLot { set; get; }
        public List<Lot> listEndingLot { set; get; }
        public List<Lot> listReadyLot { set; get; }
        public List<Category> listCategory { set; get; }
        public List<Product> listAttributes { set; get; }
        public List<LotAttachment> listLotAttachment { set; get; }
    }
}