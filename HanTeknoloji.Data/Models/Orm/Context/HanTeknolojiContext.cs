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
            Database.Connection.ConnectionString = "Server=DESKTOP-15VL0N4;Database=HanTeknolojiDB;Trusted_Connection=True";
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
    }
}
