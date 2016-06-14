using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Controls;


namespace SiteServer.Project.BackgroundPages.Modal
{
	public class UrlSummaryAdd : BackgroundBasePage
	{
        public BREditor brSummary;

        private int urlID;
        private string returnUrl;

        public static string GetShowPopWinString(int urlID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("urlID", urlID.ToString());
            arguments.Add("returnUrl", StringUtils.ValueToUrl(returnUrl));
            return JsUtils.OpenWindow.GetOpenWindowString("ÓòÃû±¸×¢", "modal_urlSummaryAdd.aspx", arguments, 550, 600);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.urlID = TranslateUtils.ToInt(base.Request.QueryString["urlID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["returnUrl"]);

			if (!IsPostBack)
			{
                if (this.urlID > 0)
                {
                    UrlInfo urlInfo = DataProvider.UrlDAO.GetUrlInfo(this.urlID);
                    if (urlInfo != null)
                    {
                        this.brSummary.Text = urlInfo.Summary;
                    }
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
				
			if (this.urlID > 0)
			{
                DataProvider.UrlDAO.Update(this.brSummary.Text, this.urlID);

                isChanged = true;
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
			}
		}
	}
}
