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
	public class BackgroundGovPublicApplyToReplyDetail : BackgroundGovPublicApplyToDetailBasePage
    {
        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicApply, "待办理申请", AppManager.CMS.Permission.WebSite.GovPublicApply);
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int applyID, string listPageUrl)
        {
            return PageUtils.GetWCMUrl(string.Format(@"background_govPublicApplyToReplyDetail.aspx?PublishmentSystemID={0}&ApplyID={1}&ReturnUrl={2}", publishmentSystemID, applyID, StringUtils.ValueToUrl(listPageUrl)));
        }
	}
}
