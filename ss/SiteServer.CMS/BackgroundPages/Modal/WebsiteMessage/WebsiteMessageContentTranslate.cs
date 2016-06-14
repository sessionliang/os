using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class WebsiteMessageContentTranslate : BackgroundBasePage
    {
        public DropDownList ParentItemID;

        private int websiteMessageID;
        private string returnUrl;
        private ArrayList contentIDArrayList;
        private string classifyID;

        public static string GetRedirectString(int publishmentSystemID, int websiteMessageID, int classifyID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("WebsiteMessageID", websiteMessageID.ToString());
            arguments.Add("ClassifyID", classifyID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowStringWithCheckBoxValue("内容转移", "modal_websiteMessageContentTranslate.aspx", arguments, "ContentIDCollection", "请选择需要转移的内容！", 300, 220);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "WebsiteMessageID", "ReturnUrl", "ContentIDCollection");
            this.websiteMessageID = TranslateUtils.ToInt(base.GetQueryStringNoSqlAndXss("WebsiteMessageID"));
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ContentIDCollection"));
            this.classifyID = base.GetQueryStringNoSqlAndXss("classifyID");

            if (!IsPostBack)
            {
                TreeManager.AddListItems(this.ParentItemID.Items, base.PublishmentSystemID, 0, true, true, "WebsiteMessageClassify", 2);
                ControlUtils.SelectListItems(this.ParentItemID, classifyID);
            }
        }

        public void ParentItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int theItemID = TranslateUtils.ToInt(this.ParentItemID.SelectedValue);
            PageUtils.Redirect(Modal.WebsiteMessageContentTranslate.GetRedirectString(base.PublishmentSystemID, TranslateUtils.ToInt(base.GetQueryString("WebsiteMessageID")), TranslateUtils.ToInt(base.GetQueryString("ClassifyID")), base.GetQueryString("ReturnUrl")));
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            DataProvider.WebsiteMessageContentDAO.TranslateContent(this.contentIDArrayList, TranslateUtils.ToInt(this.ParentItemID.SelectedValue));
            JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
        }

    }
}
