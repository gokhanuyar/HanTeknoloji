using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class AdminUserVM : BaseVM
    {
        [Display(Name = "Ad Soyad"), Required(ErrorMessage = "Bu alan boş geçilemez")]
        public string FullName { get; set; }

        [Display(Name = "E-mail"), Required(ErrorMessage = "Bu alan boş geçilemez"),EmailAddress(ErrorMessage ="Lütfen e-mail formatında giriş yapın.")]
        public string Email { get; set; }

        [Display(Name = "Şifre"), Required(ErrorMessage = "Bu alan boş geçilemez")]
        public string Password { get; set; }

        [Display(Name = "Şifre Tekrar"), Required(ErrorMessage = "Bu alan boş geçilemez"), Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}