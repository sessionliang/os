using System;

using System.Security.Cryptography;
using System.Text;
using System.DirectoryServices;
using System.Web;
using System.Web.Security;
using BaiRong.Model;
using BaiRong.Core;
using BaiRong.Core.Cryptography;

namespace BaiRong.Core
{  
    public class AdminAuthConfig
    {
        // Fields
        private const string _AuthCookieName = "SITESERVER.ADMINISTRATOR.AUTH";
        private const string _UserNameCookieName = "SITESERVER.ADMINISTRATOR.USERNAME";
        private const string _FormsCookiePath = "/";
        private const int _Timeout = 120;

        private AdminAuthConfig() { }

        // Properties
        public static string AuthCookieName
        {
            get
            {
                return _AuthCookieName;
            }
        }

        public static string UserNameCookieName
        {
            get
            {
                return _UserNameCookieName;
            }
        }

        public static string FormsCookiePath
        {
            get
            {
                return _FormsCookiePath;
            }
        }

        public static int Timeout
        {
            get
            {
                return _Timeout;
            }
        }
    }
}
