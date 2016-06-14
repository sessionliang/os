using System;

using System.Security.Cryptography;
using System.Text;
using System.DirectoryServices;
using System.Web;
using System.Web.Security;
using BaiRong.Model;
using BaiRong.Core.Cryptography;

namespace BaiRong.Core
{
    public class UserAuthConfig
    {
        private UserAuthConfig() { }

        public static string AuthCookieName
        {
            get
            {
                return "SITESERVER.USER.AUTH";
            }
        }

        public static string GroupSNCookieName
        {
            get
            {
                return "SITESERVER.USER.GROUPSN";
            }
        }

        public static string UserNameCookieName
        {
            get
            {
                return "SITESERVER.USER.USERNAME";
            }
        }

        public static string UserCookiePath
        {
            get
            {
                return "/";
            }
        }

        public static double Timeout
        {
            get
            {
                return 120;
            }
        }
    }
}
