using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class SupplierVM : BaseVM
    {
        [Display(Name = "Tedarikçi Adı/Ünvanı"),Required(ErrorMessage ="Bu alan boş geçilemez.")]
        public string CompanyName { get; set; }

        [Display(Name = "Adres"), Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string Address { get; set; }

        public string City { get; set; }
        [Display(Name = "İl"), Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public int CityID { get; set; }

        public string Region { get; set; }
        [Display(Name ="İlçe"), Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public int RegionID { get; set; }

        [Display(Name = "Telefon Numarası"), Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string Phone { get; set; }

        [Display(Name = "E-posta Adresi")]
        public string Email { get; set; }

        [Display(Name = "Web Sitesi")]
        public string WebSite { get; set; }
    }
}