using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Webdaugia.Models
{
    public partial class AuctionDBContext : DbContext
    {
        public AuctionDBContext()
            : base("name=AuctionDBContext")
        {
        }

        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<ATM> ATMs { get; set; }
        public virtual DbSet<Auction> Auctions { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Lot> Lots { get; set; }
        public virtual DbSet<LotAttachment> LotAttachments { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductsAttachment> ProductsAttachments { get; set; }
        public virtual DbSet<ProductsImage> ProductsImages { get; set; }
        public virtual DbSet<RegisterBid> RegisterBids { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersImage> UsersImages { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(e => e.SiteTile)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Lots)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.CateID);

            modelBuilder.Entity<Lot>()
                .Property(e => e.SiteTile)
                .IsUnicode(false);

            modelBuilder.Entity<LotAttachment>()
                .Property(e => e.AttachmentLink)
                .IsFixedLength();

            modelBuilder.Entity<Organization>()
                .Property(e => e.TaxCode)
                .IsFixedLength();

            modelBuilder.Entity<ProductsAttachment>()
                .Property(e => e.Attachment)
                .IsFixedLength();

            modelBuilder.Entity<ProductsImage>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<RegisterBid>()
                .HasMany(e => e.Auctions)
                .WithRequired(e => e.RegisterBid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Username)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.CMND)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasOptional(e => e.Organization)
                .WithRequired(e => e.User);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UsersImages)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.UsersID);

            modelBuilder.Entity<UsersImage>()
                .Property(e => e.Image)
                .IsUnicode(false);
        }
    }
}
