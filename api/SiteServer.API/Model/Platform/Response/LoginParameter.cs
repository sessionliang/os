using SiteServer.B2C.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SiteServer.API.Model
{
    public class LoginParameter
    {
        public bool IsSuccess { get; set; }
        public bool IsRedirectToLogin { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}