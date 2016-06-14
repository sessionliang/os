using System;
using System.Web;
using BaiRong.Core;
using System.Text;
using BaiRong.Model;

namespace BaiRong.Core
{
    public class CookieUtils
    {
        private CookieUtils()
        {
        }

        //public static void SetObject(string name, object obj)
        //{
        //    string xml = Serializer.ConvertToString(obj);
        //    CookieUtils.SetCookie(name, xml, DateTime.MaxValue);
        //}

        //public static object GetObject(string name, Type type)
        //{
        //    string xml = CookieUtils.GetCookie(name);
        //    return Serializer.ConvertToObject(xml, type);
        //}

        public static void SetCookie(string name, string value, DateTime Expires)
        {
            HttpCookie myCookie = new HttpCookie(name);
            DateTime now = DateTime.Now;

            myCookie.Value = RuntimeUtils.EncryptStringByTranslate(value);
            //myCookie.Value = PageUtils.UrlEncode(value, ECharset.utf_8);
            myCookie.Expires = Expires;
            myCookie.HttpOnly = true;//防止通过js获取到cookie
            if (HttpContext.Current.Request.Url.Scheme.Equals("https"))
                myCookie.Secure = true;//通过https传递cookie

            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        public static void SetCookie(HttpCookie cookie)
        {
            cookie.Value = RuntimeUtils.EncryptStringByTranslate(cookie.Value);
            cookie.HttpOnly = true;//防止通过js获取到cookie
            if (HttpContext.Current.Request.Url.Scheme.Equals("https"))
                cookie.Secure = true;//通过https传递cookie
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string GetCookie(string name)
        {
            if (IsExists(name))
            {
                string value = HttpContext.Current.Request.Cookies[name].Value;
                return RuntimeUtils.DecryptStringByTranslate(value);
                //return PageUtils.FilterSqlAndXss(PageUtils.UrlDecode(HttpContext.Current.Request.Cookies[name].Value, ECharset.utf_8));
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool IsExists(string name)
        {
            return (HttpContext.Current.Request.Cookies[name] != null);
        }

        public static void Erase(string name)
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[name];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Values.Clear();
            }
        }
    }
}
