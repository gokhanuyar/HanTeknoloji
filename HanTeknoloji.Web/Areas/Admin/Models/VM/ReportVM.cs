using HanTeknoloji.Data.Models.Orm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class ReportVM : BaseVM
    {
        public ReportVM()
        {
            ImeiList = new List<string>();
        }
        public int CustomerID { get; set; }

        public int SupplierID { get; set; }

        public int ProductID { get; set; }

        public int UserID { get; set; }

        public decimal Price { get; set; }

        public decimal KdvPrice { get; set; }

        public string PaymentType { get; set; }

        public int Quantity { get; set; }

        public string SaleDate { get; set; }

        public string SaleTime { get; set; }

        public string InvoiceDate { get; set; }

        public ProductVM Product { get; set; }

        public string AdminUserName { get; set; }

        public CustomerVM Customer { get; set; }

        public string Note { get; set; }

        public List<SaleDetailsVM> Details { get; set; }

        public List<string> ImeiList { get; set; }

        public decimal UnitBuyPrice { get; set; }
        public decimal UnitSalePrice { get; set; }
    }
}