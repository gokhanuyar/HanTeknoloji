using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class CustomerVM : BaseVM
    {
        [Display(Name = "TC Kimlik No")]
        public string TCNo { get; set; }

        [Display(Name = "Müşteri Adı / Ünvanı"), Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string Name { get; set; }

        [Display(Name = "Adres"), Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string Address { get; set; }

        [Display(Name = "İl"), Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public int CityID { get; set; }
        public string City { get; set; }

        [Display(Name = "İlçe"), Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public int RegionID { get; set; }
        public string Region { get; set; }

        [Display(Name = "Vergi Dairesi")]
        public string TaxOffice { get; set; }

        [Display(Name = "Vergi Numarası")]
        public string TaxNumber { get; set; }

        [Display(Name = "Telefon Numarası"),
        MaxLength(11, ErrorMessage = " En fazla 11 (onbir) karkater belirtebilirsiniz."),
        MinLength(10, ErrorMessage = " En az 10 (on) karakter belirtmelisiniz.")]
        public string Phone { get; set; }

        [Display(Name = "Müşteri Tipi")]
        public bool IsPerson { get; set; }
    }
}