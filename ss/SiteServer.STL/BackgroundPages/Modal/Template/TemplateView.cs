using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal
{
	public class TemplateView : BackgroundBasePage
	{
        public TextBox tbContent;

        public static string GetOpenLayerString(int publishmentSystemID, int templateLogID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateLogID", templateLogID.ToString());
            return JsUtils.Layer.GetOpenLayerString("²é¿´ÐÞ¶©ÄÚÈÝ", PageUtils.GetSTLUrl("modal_templateView.aspx"), arguments);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
           
			if (!IsPostBack)
			{
                int templateLogID = base.GetIntQueryString("templateLogID");
                this.tbContent.Text = DataProvider.TemplateLogDAO.GetTemplateContent(templateLogID);
			}
		}
	}
}
