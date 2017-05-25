﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class Product : BaseEntity
    {
        public string SerialNumber { get; set; }

        public int TradeMarkID { get; set; }

        public int ProductModelID { get; set; }        

        public int SupplierID { get; set; }

        public int ColorID { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal UnitSalePrice { get; set; }

        public int Count { get; set; }

        public string Payment { get; set; }

        public int CategoryID { get; set; }

        public decimal KDV { get; set; }

        public string IMEI { get; set; }

        public string BankName { get; set; }
        public string BankCartName { get; set; }
        public string CartNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CheckNumber { get; set; }
    }
}
