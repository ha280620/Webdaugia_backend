﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdaugia.Models;

namespace Webdaugia.DAO
{
    public class ProductDao
    {
        AuctionDBContext db = new AuctionDBContext();
        public ProductDao()
        {
            db = new AuctionDBContext();
        }

        public Lot ViewDetail(int id)
        {
            return db.Lots.Find(id);
        }

        public IEnumerable<Product> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Lot.Name.Contains(searchString) || x.Lot.ID.ToString().Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedAt).ToPagedList(page, pageSize);
        }
    }
}