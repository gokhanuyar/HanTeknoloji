using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.Dto
{
    public class FilterDto
    {
        public string Date { get; set; }
        public string DateSelect { get; set; }
        public string Month { get; set; }
        public string CategoryID { get; set; }
        public string ProductID { get; set; }
        public string Payment { get; set; }
        public bool IsInvoiced { get; set; }
    }
}