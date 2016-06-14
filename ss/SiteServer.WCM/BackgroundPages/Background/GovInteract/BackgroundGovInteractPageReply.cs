using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;

using System.Web.UI.HtmlControls;

using System.Text;

namespace SiteServer.WCM.BackgroundPages
{
	public class BackgroundGovInteractPageReply : BackgroundGovInteractPageBasePage
	{
        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovInteract, "待办理办件", AppManager.CMS.Permission.WebSite.GovInteract);
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID, int contentID, string listPageUrl)
        {
            return PageUtils.GetWCMUrl(string.Format(@"background_govInteractPageReply.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&ReturnUrl={3}", publishmentSystemID, nodeID, contentID, StringUtils.ValueToUrl(listPageUrl)));
        }
	}
}
