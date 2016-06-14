using System.Web.UI.WebControls;
using BaiRong.Core;
using System;
 
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using System.Text;
using BaiRong.Core.WebService;
using BaiRong.Core.Net;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundNavTransaction : BackgroundBasePage
    {
        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_navTransaction.aspx?PublishmentSystemID=" + publishmentSystemID));
        }
        public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "会员交易", AppManager.Platform.Permission.Platform_Site);
            }
        }
    }
}
