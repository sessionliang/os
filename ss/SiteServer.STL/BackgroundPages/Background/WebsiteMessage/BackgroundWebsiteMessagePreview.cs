using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser;

using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.STL.BackgroundPages
{
    public class BackgroundWebsiteMessagePreview : BackgroundBasePage
    {
        public Literal ltlWebsiteMessageName;
        public Literal ltlWebsiteMessageCode;
        public Literal ltlForm;

        private WebsiteMessageInfo websiteMessageInfo;

        public string GetEditUrl()
        {
            return WebsiteMessageAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID, true);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("WebsiteMessageName");

            string websiteMessageName = base.GetQueryStringNoSqlAndXss("WebsiteMessageName");
            this.websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(websiteMessageName, base.PublishmentSystemID);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_WebsiteMessage, "预览提交表单", AppManager.CMS.Permission.WebSite.WebsiteMessage);

                this.ltlWebsiteMessageName.Text = this.websiteMessageInfo.WebsiteMessageName;

                string stlElement = string.Format(@"<stl:websiteMessage></stl:websiteMessage>");

                this.ltlWebsiteMessageCode.Text = StringUtils.HtmlEncode(stlElement);

                this.ltlForm.Text = StlParserManager.ParsePreviewContent(base.PublishmentSystemInfo, stlElement);

                //if (string.IsNullOrEmpty(this.websiteMessageInfo.Template))
                //{
                //    WebsiteMessageTemplate websiteMessageTemplate = new WebsiteMessageTemplate(base.PublishmentSystemID, this.websiteMessageInfo);
                //    this.ltlForm.Text = websiteMessageTemplate.GetTemplate();
                //}
                //else
                //{
                //    this.ltlForm.Text = this.websiteMessageInfo.Template;
                //}
            }
        }
    }
}
