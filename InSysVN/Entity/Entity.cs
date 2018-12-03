namespace Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Entity : DbContext
    {
        public Entity()
            : base("name=Entity")
        {
        }

        public virtual DbSet<CardNumbers> CardNumbers { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<OrderPromotion> OrderPromotion { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<ProductCate> ProductCate { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Return> Return { get; set; }
        public virtual DbSet<ReturnDetail> ReturnDetail { get; set; }
        public virtual DbSet<RoleModule> RoleModule { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Stall> Stall { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Warehouses> Warehouses { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.CustomerCode)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Barcode)
                .IsUnicode(false);

            modelBuilder.Entity<OrderPromotion>()
                .Property(e => e.Barcode)
                .IsUnicode(false);

            modelBuilder.Entity<OrderPromotion>()
                .Property(e => e.ProductCode)
                .IsUnicode(false);

            modelBuilder.Entity<Orders>()
                .Property(e => e.OrderCode)
                .IsUnicode(false);

            modelBuilder.Entity<Stall>()
                .Property(e => e.IpAddress)
                .IsUnicode(false);
        }
    }
}
