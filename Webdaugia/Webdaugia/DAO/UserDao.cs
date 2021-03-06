using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Webdaugia.Models;

namespace Webdaugia.DAO
{
    public class UserDao
    {
        AuctionDBContext db = null;

        public UserDao()
        {
            db = new AuctionDBContext();
        }
        //Insert
        public int Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        //getById
        public User getByUserName(string username)
        {
            return db.Users.SingleOrDefault(x => x.Username == username);
        }
        public User getUserById(int ID)
        {
            return db.Users.Find(ID);
        }
        //Update
        public bool Update(User entity)
        {
            try
            {
                User userUpdate = db.Users.SingleOrDefault(x => x.ID == entity.ID);
                if (userUpdate != null)
                {
                    db.Users.AddOrUpdate(entity);
                    db.SaveChanges();
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //
        public int Login(string userName, string passWord, int role)
        {
            var result = db.Users.SingleOrDefault(x => x.Username == userName && x.RoleID== role);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == false)
                    return -1;
                else
                {
                    if (result.Password.Trim() == passWord)
                        return 1;
                    else
                        return -2;
                }
            }
        }
        public int Login(string userName, string passWord)
        {
            var result = db.Users.SingleOrDefault(x => x.Username == userName && x.RoleID != 3);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == false)
                    return -1;
                else
                {
                    if (result.Password == passWord)
                        return 1;
                    else
                        return -2;
                }
            }

        }
    }
}