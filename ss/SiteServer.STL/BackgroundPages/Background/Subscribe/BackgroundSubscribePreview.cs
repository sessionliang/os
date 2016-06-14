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
    public class BackgroundSubscribePreview : BackgroundBasePage
    {
        public Literal ltlSearchwordInputCode;
        public Literal ltlForm;

        private SubscribeSetInfo subscribeSetInfo;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.subscribeSetInfo = DataProvider.SubscribeSetDAO.GetSubscribeSetInfo(base.PublishmentSystemID);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Subscribe, "预览信息订阅", AppManager.CMS.Permission.WebSite.Subscribe);

                string stlElement = string.Format(@"<stl:subscribe></stl:subscribe>");

                this.ltlSearchwordInputCode.Text = StringUtils.HtmlEncode(stlElement);

                this.ltlForm.Text = StlParserManager.ParsePreviewContent(base.PublishmentSystemInfo, stlElement);                 
            }
        }
    }
}
