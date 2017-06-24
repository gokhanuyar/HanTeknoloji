using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.Types.ReturnTypes
{
    public class JResult
    { 
        public string MessageTitle { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string CssClass
        {
            get
            {
                return this.IsSuccess ? "alert alert-success" : "alert alert-danger";
            }
        }
    }
}