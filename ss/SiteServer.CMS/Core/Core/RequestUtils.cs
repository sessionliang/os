using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace SiteServer.CMS.Core
{
    public class RequestUtils
    {
        public static bool IsAnonymous
        {
            get
            {
                return string.IsNullOrEmpty(RequestUtils.CurrentUserName);
            }
        }

        public static string CurrentUserName
        {
            get
            {
                string userName = string.Empty;
                if (BaiRongDataProvider.UserDAO.IsAnonymous)
                {
                    userName = HttpContext.Current.Request.QueryString["userName"];
                }
                else
                {
                    userName = BaiRongDataProvider.UserDAO.CurrentUserName;
                }
                if (userName == null)
                {
                    userName = string.Empty;
                }
                return userName;
            }
        }

        public static UserInfo Current
        {
            get
            {
                return UserManager.GetUserInfo("", RequestUtils.CurrentUserName);
            }
        }

        public static int PublishmentSystemID
        {
            get
            {
                return TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["publishmentSystemID"]);
            }
        }

        public static PublishmentSystemInfo PublishmentSystemInfo
        {
            get
            {
                return PublishmentSystemManager.GetPublishmentSystemInfo(RequestUtils.PublishmentSystemID);
            }
        }

        public static NameValueCollection QueryString
        {
            get
            {
                NameValueCollection attributes = new NameValueCollection();
                foreach (string key in HttpContext.Current.Request.QueryString.Keys)
                {
                    attributes.Add(key, GetQueryString(key));
                }
                return attributes;
            }
        }

        public static NameValueCollection QueryStringNoSqlAndXss
        {
            get
            {
                NameValueCollection attributes = new NameValueCollection();
                foreach (string key in HttpContext.Current.Request.QueryString.Keys)
                {
                    attributes.Add(key, GetQueryStringNoSqlAndXss(key));
                }
                return attributes;
            }
        }

        public static string GetQueryString(string name)
        {
            return GetQueryStringNoSql(name);
        }


        public static string GetRequestString(string name)
        {
            return HttpContext.Current.Request[name];
        }

        public static string GetRequestStringNoSqlAndXss(string name)
        {
            string value = HttpContext.Current.Request[name];
            if (value == null) return null;
            return PageUtils.FilterSqlAndXss(value);
        }
        public static int GetIntRequestString(string name)
        {
            return TranslateUtils.ToInt(HttpContext.Current.Request[name]);
        }

        public static int GetIntQueryString(string name)
        {
            return TranslateUtils.ToInt(HttpContext.Current.Request.QueryString[name]);
        }

        public static bool GetBoolQueryString(string name)
        {
            return TranslateUtils.ToBool(HttpContext.Current.Request.QueryString[name]);
        }

        private static string GetQueryStringNoSql(string name)
        {
            string value = HttpContext.Current.Request.QueryString[name];
            if (value == null) return null;
            return PageUtils.FilterSql(value);
        }

        public static string GetQueryStringNoXss(string name)
        {
            string value = HttpContext.Current.Request.QueryString[name];
            if (value == null) return null;
            return PageUtils.FilterXSS(value);
        }

        public static string GetQueryStringNoSqlAndXss(string name)
        {
            string value = HttpContext.Current.Request.QueryString[name];
            if (value == null) return null;
            return PageUtils.FilterSqlAndXss(value);
        }

        public static string GetPostString(string name)
        {
            return GetPostStringNoSql(name);
        }

        private static string GetPostStringNoSql(string name)
        {
            string value = HttpContext.Current.Request.Form[name];
            if (value == null) return null;
            return PageUtils.FilterSql(value);
        }

        public static string GetPostStringNoXss(string name)
        {
            string value = HttpContext.Current.Request.Form[name];
            if (value == null) return null;
            return PageUtils.FilterXSS(value);
        }

        public static string GetPostStringNoSqlAndXss(string name)
        {
            string value = HttpContext.Current.Request.Form[name];
            if (value == null) return null;
            return PageUtils.FilterSqlAndXss(value);
        }
    }
}