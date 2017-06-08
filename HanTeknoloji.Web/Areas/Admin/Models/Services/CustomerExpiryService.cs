using HanTeknoloji.Business.Manager;
using HanTeknoloji.Data.Models.Orm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.Services
{
    public class CustomerExpiryService
    {
        public static void SetCustomerExpiry(CustomerExpiry entity)
        {
            GenericRepository<Customer> rpcustomer = new GenericRepository<Customer>();
            var customer = rpcustomer.Find(entity.CustomerID);
            customer.ExpiryValue += entity.ExpiryValue;
            rpcustomer.SaveChanges();
        } 
    }
}