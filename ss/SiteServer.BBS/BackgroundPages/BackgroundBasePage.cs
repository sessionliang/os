using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using BaiRong.Core;
using SiteServer.BBS.Model;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundBasePage : SiteServer.CMS.BackgroundPages.BackgroundBasePage
    {
        public override void BreadCrumb(string leftMenuID, string pageTitle, string permission)
        {
            if (this.ltlBreadCrumb != null)
            {
                string topMenuID = AppManager.BBS.TopMenu.ID_Forums;
                string pageUrl = PathUtils.GetFileName(base.Request.FilePath);
                string topTitle = AppManager.BBS.TopMenu.GetText(topMenuID);
                string leftTitle = AppManager.BBS.LeftMenu.GetText(leftMenuID);
                this.ltlBreadCrumb.Text = StringUtils.GetBreadCrumbHtml(AppManager.BBS.AppID, topMenuID, topTitle, leftMenuID, leftTitle, pageUrl, pageTitle, string.Empty, true);
            }

            if (!string.IsNullOrEmpty(permission))
            {
                AdminManager.VerifyPermissions(permission);
            }
        }

        public ConfigurationInfoExtend Additional
        {
            get
            {
                return ConfigurationManager.GetAdditional(base.PublishmentSystemID);
            }
        }
    }
}
