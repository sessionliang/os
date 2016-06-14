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
    public class BackgroundSearchwordInputPreview : BackgroundBasePage
    {
        public Literal ltlSearchwordInputCode;
        public Literal ltlForm;

        private SearchwordSettingInfo searchwordSettingInfo;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.searchwordSettingInfo = DataProvider.SearchwordSettingDAO.GetSearchwordSettingInfo(base.PublishmentSystemID);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Searchword, "预览站内搜索", AppManager.CMS.Permission.WebSite.Searchword);

                string stlElement = string.Format(@"<stl:searchwordInput></stl:searchwordInput>");

                this.ltlSearchwordInputCode.Text = StringUtils.HtmlEncode(stlElement);

                this.ltlForm.Text = StlParserManager.ParsePreviewContent(base.PublishmentSystemInfo, stlElement);

                //if (string.IsNullOrEmpty(this.searchwordInputInfo.Template))
                //{
                //    SearchwordInputTemplate searchwordInputTemplate = new SearchwordInputTemplate(base.PublishmentSystemID, this.searchwordInputInfo);
                //    this.ltlForm.Text = searchwordInputTemplate.GetTemplate();
                //}
                //else
                //{
                //    this.ltlForm.Text = this.searchwordInputInfo.Template;
                //}
            }
        }
    }
}
