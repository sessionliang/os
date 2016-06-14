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
    public class ApplicationSet : BackgroundBasePage
    {
        public TextBox tbHandleSummary;

        private int applicationID;
        private bool isHandled;

        public static string GetShowPopWinString(int applicationID, bool isHandled)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("applicationID", applicationID.ToString());
            arguments.Add("isHandled", isHandled.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("设置处理", "modal_product_applicationSet.aspx", arguments, 350, 300);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.applicationID = TranslateUtils.ToInt(base.Request.QueryString["applicationID"]);
            this.isHandled = TranslateUtils.ToBool(base.Request.QueryString["isHandled"]);

			if (!IsPostBack)
			{

			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            if (this.applicationID > 0)
			{
                DataProvider.ApplicationDAO.Handle(this.applicationID, this.tbHandleSummary.Text);
                base.SuccessMessage("成功将申请设置为已处理状态");

                isChanged = true;
			}

			if (isChanged)
			{
                if (this.isHandled)
                {
                    JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, "product_applicationHandled.aspx");
                }
                else
                {
                    JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, "product_application.aspx");
                }
			}
		}
	}
}
