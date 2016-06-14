using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using System.Collections;

using SiteServer.B2C.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using System.Text;
using System;

namespace SiteServer.B2C.Core
{
	public class SessionManager
	{
        private const string COOKIE_CART_NEW_TITLE = "SITESERVER.B2C.CARTNEWTITLE";

        public static void CartNew(string title)
        {
            CookieUtils.SetCookie(SessionManager.COOKIE_CART_NEW_TITLE, title, DateTime.Now.AddDays(1));
        }

        public static string GetCartNewTitleAndErase()
        {
            string title = CookieUtils.GetCookie(SessionManager.COOKIE_CART_NEW_TITLE);
            if (!string.IsNullOrEmpty(title))
            {
                CookieUtils.Erase(SessionManager.COOKIE_CART_NEW_TITLE);
            }
            return title;
        }
	}
}
