using System;
using System.Web.UI.WebControls;
using BaiRong.Core.Cryptography;

using BaiRong.Core;

namespace BaiRong.BackgroundPages
{
	public class BackgroundJsMin : BackgroundBasePage
	{
        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Utility, "JS½Å±¾Ñ¹Ëõ", AppManager.Platform.Permission.Platform_Utility);
            }
        }
	}
}
