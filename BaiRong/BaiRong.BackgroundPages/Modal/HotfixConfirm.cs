using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using BaiRong.Core.Net;
using BaiRong.Model;

namespace BaiRong.BackgroundPages.Modal
{
    public class HotfixConfirm : BackgroundBasePage
	{
        private string hotfixID;

        public static string GetOpenWindowString()
        {
            return StringUtils.Constants.GetOpenWindowStringWithHotfix(string.Empty);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            if (base.IsForbidden) return;

            this.hotfixID = base.GetQueryString("hotfixID");

			if (!IsPostBack)
			{

			}
		}

        public void btnHotfix_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(ProgressBar.GetRedirectUrlStringOfHotfix(this.hotfixID));
		}
	}
}
