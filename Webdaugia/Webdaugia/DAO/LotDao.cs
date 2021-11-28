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
        public IEnumerable<Lot> ListAllPagingEnd(string searchString, int page, int pageSize)
        {
            IQueryable<Lot> model = db.Lots.Where(x =>x.TimeForBidEnd < DateTime.Now);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Category.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.TimeForBidEnd).ToPagedList(page, pageSize);
        }
        public IEnumerable<Lot> ListAllPagingEnd30(string searchString, int page, int pageSize)
        {
            var date = DateTime.Now.AddDays(-180);
            IQueryable<Lot> model = db.Lots.Where(x => x.TimeForBidEnd < date);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Category.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.TimeForBidEnd).ToPagedList(page, pageSize);
        }
        public IEnumerable<LotAttachment> ListAllPagingLink(string searchString, int page, int pageSize)
        {
 
            IQueryable<LotAttachment> model = db.LotAttachments;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Lot.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        public IEnumerable<User> ListAllPagingUser(string searchString, int page, int pageSize)
        {

            IQueryable<User> model = db.Users.Where(x => x.Status == 1 || x.Status == 2);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.FullName.Contains(searchString) || x.ID.ToString().Contains(searchString));
            }

            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
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
        public IEnumerable<RegisterBid> ListAllPagingEndOfLot(int id, string searchString, int page, int pageSize)
        {
            var modelAuctions = db.Auctions.Where(x => x.Status == 1 && x.RegisterBid.LotID == id).FirstOrDefault();
            IQueryable<RegisterBid> model;
            if (modelAuctions != null)
            {
                 model = db.RegisterBids.Where(x => x.LotID == id && x.Status == true && x.ID != modelAuctions.RegisterBidID);
            }
            else
            {
                model = db.RegisterBids.Where(x => x.LotID == id && x.Status == true);
            }
          
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