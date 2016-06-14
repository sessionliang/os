using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class SeoMetaView : BackgroundBasePage
	{
		protected NoTagText MetaName;

		protected TextBox MetaCode;

        public static string GetOpenWindowString(int publishmentSystemID, int seoMetaID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("SeoMetaID", seoMetaID.ToString());
            return PageUtility.GetOpenWindowString("页面元数据源代码查看", "modal_seoMetaView.aspx", arguments, 480, 360, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
				if (base.GetQueryString("SeoMetaID") != null)
				{
                    int seoMetaID = TranslateUtils.ToInt(base.GetQueryString("SeoMetaID"));
					SeoMetaInfo seoMetaInfo = DataProvider.SeoMetaDAO.GetSeoMetaInfo(seoMetaID);
					if (seoMetaInfo != null)
					{
						this.MetaName.Text = seoMetaInfo.SeoMetaName;

						this.MetaCode.Text = SeoManager.GetMetaContent(seoMetaInfo);
					}
				}
			}
		}
	}
}
