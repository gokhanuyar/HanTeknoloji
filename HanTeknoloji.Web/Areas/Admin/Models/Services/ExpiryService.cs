using HanTeknoloji.Business.Manager;
using HanTeknoloji.Data.Models.Orm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.Services
{
    public class ExpiryService
    {
        public static void SetSupplierExpiry(SupplierExpiry entity)
        {
            GenericRepository<Supplier> rpsupplier = new GenericRepository<Supplier>();
            var supplier = rpsupplier.Find(entity.SupplierID);
            supplier.TotalExpiryValue += (entity.TotalBuyingPrice - entity.PaidPrice);
            supplier.PaidExpiryValue += entity.PaidPrice;
            rpsupplier.SaveChanges();
        }

        public static void SetCustomerExpiry(CustomerExpiry entity)
        {
            GenericRepository<Customer> rpcustomer = new GenericRepository<Customer>();
            var customer = rpcustomer.Find(entity.CustomerID);
            customer.ExpiryValue += entity.ExpiryValue;
            customer.PaidExpiryValue += (entity.SaleTotalPrice - entity.ExpiryValue);
            rpcustomer.SaveChanges();
        }
    }
}