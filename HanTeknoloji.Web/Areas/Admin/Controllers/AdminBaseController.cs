using HanTeknoloji.Business.Manager;
using HanTeknoloji.Data.Models.Orm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    public class AdminBaseController : Controller
    {
        public GenericRepository<Product> rpproduct;
        public GenericRepository<Category> rpcategory;
        public GenericRepository<ProductModel> rpproductmodel;
        public GenericRepository<TradeMark> rptrademark;
        public GenericRepository<Color> rpcolor;
        public GenericRepository<Sale> rpsale;
        public GenericRepository<AdminUser> rpadminuser;
        public GenericRepository<ServiceSale> rpservicesale;
        public GenericRepository<Supplier> rpsupplier;
        public GenericRepository<City> rpcity;
        public GenericRepository<Region> rpregion;
        public GenericRepository<Customer> rpcustomer;

        public AdminBaseController()
        {
            rpproduct = new GenericRepository<Product>();
            rpcategory = new GenericRepository<Category>();
            rpproductmodel = new GenericRepository<ProductModel>();
            rptrademark = new GenericRepository<TradeMark>();
            rpcolor = new GenericRepository<Color>();
            rpsale = new GenericRepository<Sale>();
            rpadminuser = new GenericRepository<AdminUser>();
            rpservicesale = new GenericRepository<ServiceSale>();
            rpsupplier = new GenericRepository<Supplier>();
            rpcity = new GenericRepository<City>();
            rpregion = new GenericRepository<Region>();
            rpcustomer = new GenericRepository<Customer>();
        }

        protected override void Dispose(bool disposing)
        {
            rpproduct.Dispose();
            rpcategory.Dispose();
            rpproductmodel.Dispose();
            rptrademark.Dispose();
            rpcolor.Dispose();
            rpsale.Dispose();
            rpadminuser.Dispose();
            rpservicesale.Dispose();
            rpsupplier.Dispose();
            rpcity.Dispose();
            rpregion.Dispose();
            rpcustomer.Dispose();
        }

        public string UserEmail()
        {
            IEnumerable<Claim> claims = ((ClaimsIdentity)User.Identity).Claims;
            string UserEmail = claims.First(x => x.Type == ClaimTypes.Name).Value;
            return UserEmail;
        }

        public int UserID()
        {
            IEnumerable<Claim> claims = ((ClaimsIdentity)User.Identity).Claims;
            int UserID = Convert.ToInt32(claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            return UserID;
        }

        public int FirstTrademarkID()
        {
            int id = rptrademark.First().ID;
            return id;
        }
    }
}