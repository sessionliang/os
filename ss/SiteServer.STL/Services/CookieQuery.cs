using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Web;

namespace SiteServer.CMS.Services
{
    public class CookieQuery : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("p3p", "CP=\"CAO PSA OUR\"");

        }

        public string GetCookie()
        {
            string cookies = string.Empty;

            foreach (string key in base.Request.Cookies.Keys)
            {
                cookies += string.Format("{0}={1}; ", key, base.Request.Cookies[key].Value);
            }

            cookies = cookies.Trim();
            if (cookies.EndsWith(";"))
            {
                cookies = cookies.Substring(0, cookies.Length - 1);
            }
            return cookies;
        }
    }
}
