using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class Sale : BaseEntity
    {
        public string PaymentType { get; set; }
        
        public int UserID { get; set; }

        public int CustomerID { get; set; }

        public Nullable<DateTime> InvoiceDate { get; set; }        

        public Nullable<DateTime> ExpiryDate { get; set; }

        public virtual List<SaleDetails> SaleDetails { get; set; }

        public decimal TotalPrice { get; set; }

        public bool IsInvoiced { get; set; }
    }
}
