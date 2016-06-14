using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.B2C.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.B2C.Model;
using System.Text;
using SiteServer.CMS.BackgroundPages;
using System.Web.UI;

namespace SiteServer.B2C.BackgroundPages.Modal
{
    public class RequestConfiguration : BackgroundBasePage
    {
        protected TextBox tbRequestType;
        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public static string GetShowPopWinString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityB2C.GetOpenWindowString("设置问题类型", "modal_requestConfiguration.aspx", arguments, 500, 350);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                this.tbRequestType.Text = TranslateUtils.ObjectCollectionToString(B2CConfigurationManager.GetPublishmentSystemAdditional(base.PublishmentSystemID).RequestTypeCollection);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                B2CConfigurationInfo configurationInfo = B2CConfigurationManager.GetInstance(base.PublishmentSystemID);
                configurationInfo.Additional.RequestTypeCollection = TranslateUtils.StringCollectionToStringList(this.tbRequestType.Text);

                DataProviderB2C.B2CConfigurationDAO.Update(configurationInfo);

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
                isChanged = false;
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }

    }
}
