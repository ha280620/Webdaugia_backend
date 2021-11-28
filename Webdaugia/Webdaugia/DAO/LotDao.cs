using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using Webdaugia.Models;
using Webdaugia.Models.Common;

namespace Webdaugia.DAO
{
    
    public class LotDao
    {
        AuctionDBContext db = new AuctionDBContext();
        public LotDao()
        {
            db = new AuctionDBContext();
        }

        public Lot ViewDetail(int id)
        {
            return db.Lots.Find(id);
        }

        public IEnumerable<Lot> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Lot> model = db.Lots;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Category.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.TimeForBidEnd).ToPagedList(page, pageSize);
        }
        public IEnumerable<Lot> ListAllPagingRegister(string searchString, int page, int pageSize)
        {
            IQueryable<Lot> model = db.Lots.Where(x=>x.TimeForRegisterStart < DateTime.Now && x.TimeForBidStart > DateTime.Now);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Category.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.TimeForBidEnd).ToPagedList(page, pageSize);
        }
        public IEnumerable<Lot> ListAllPagingAuction(string searchString, int page, int pageSize)
        {
            IQueryable<Lot> model = db.Lots.Where(x => x.TimeForBidStart < DateTime.Now );
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Category.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        public IEnumerable<RegisterBid> ListAllPagingRegisterOfLot(int id,string searchString, int page, int pageSize)
        {
            IQueryable<RegisterBid> model = db.RegisterBids.Where(x => x.LotID == id);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.User.FullName.Contains(searchString) || x.Lot.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        public IEnumerable<RegisterBid> ListAllPagingRegisterOfLot1(string searchString, int page, int pageSize)
        {
            var dao = new UserDao();
            //UserLogin userid = (UserLogin)Session["USER"];
            var userid = ((UserLogin)Session["USER"]).UserID;
            var user = dao.getUserById(userid);
            IQueryable<RegisterBid> model = db.RegisterBids.Where(x => x.UserID == );
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.User.FullName.Contains(searchString) || x.Lot.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        public IEnumerable<Auction> ListAllPagingAuctionOfLot(int id, string searchString, int page, int pageSize)
        {
    
         
            IQueryable<Auction> model = db.Auctions.Where(x => x.RegisterBid.LotID == id && x.RegisterBid.Status == true);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.RegisterBid.User.FullName.Contains(searchString) || x.RegisterBid.Lot.Name.Contains(searchString));
            }
            return model.OrderByDescending(o => o.BidTime).ToPagedList(page, pageSize);
        }
    }
}