using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class ServiceSaleVM : BaseVM
    {
        public string CustomerName { get; set; }

        public string ProductModel { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public int EmployeeID { get; set; }

        public string IMEINumber { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public string PaymentType { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez")]
        public string Note { get; set; }

        public string RedirectPath { get; set; }
    }
}