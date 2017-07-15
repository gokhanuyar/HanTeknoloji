using HanTeknoloji.Data.Models.Orm.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Context
{
    public class HanTeknolojiContext : DbContext
    {
        public HanTeknolojiContext()
        {
            //Database.Connection.ConnectionString = "Server=DESKTOP-15VL0N4;Database=HanTeknolojiDB;Trusted_Connection=True";
            Database.Connection.ConnectionString = @"Server = mssql13.trwww.com; database = hanteknolojiDB; uid = user_hanteknoloji; pwd = hanteknoloji_DB1";
            //
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductModel> ProductModel { get; set; }
        public DbSet<TradeMark> TradeMark { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<SaleDetails> SaleDetails { get; set; }
        public DbSet<AdminUser> AdminUser { get; set; }        
        public DbSet<ServiceSale> ServiceSale { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerExpiry> CustomerExpiry { get; set; }
        public DbSet<SupplierExpiry> SupplierExpiry { get; set; }
        public DbSet<ExpiryPayment> ExpiryPayment { get; set; }
        public DbSet<PaymentInfo> PaymentInfo { get; set; }
        public DbSet<SaleDetailInfo> SaleDetailInfo { get; set; }
        public DbSet<IMEI> IMEI { get; set; }
        public DbSet<TechnicalServiceCost> TechnicalServiceCost { get; set; }
    }
}
