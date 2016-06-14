using System;
using System.Collections.Generic;
using System.Text;

using BaiRong.Core;
using BaiRong.Model;

namespace BaiRong.BackgroundPages
{
    public class BackgroundBasePage : BackgroundBasePageEnd
    {
        public override void BreadCrumb(string leftMenuID, string pageTitle, string permission)
        {
            if (this.ltlBreadCrumb != null)
            {
                string topMenuID = AppManager.Platform.TopMenu.ID_Platform;
                string pageUrl = PathUtils.GetFileName(base.Request.FilePath);
                string topTitle = AppManager.Platform.TopMenu.GetText(topMenuID);
                string leftTitle = AppManager.Platform.LeftMenu.GetText(leftMenuID);
                this.ltlBreadCrumb.Text = StringUtils.GetBreadCrumbHtml(AppManager.Platform.AppID, topMenuID, topTitle, leftMenuID, leftTitle, pageUrl, pageTitle, string.Empty, false);
            }

            if (!string.IsNullOrEmpty(permission))
            {
                AdminManager.VerifyPermissions(permission);
            }
        }

        public void BreadCrumb(string leftMenuID, string leftSubMenuID, string pageTitle, string permission)
        {
            if (this.ltlBreadCrumb != null)
            {
                string topMenuID = AppManager.Platform.TopMenu.ID_Platform;
                string pageUrl = PathUtils.GetFileName(base.Request.FilePath);
                string topTitle = AppManager.Platform.TopMenu.GetText(topMenuID);
                string leftTitle = AppManager.Platform.LeftMenu.GetText(leftMenuID);
                string leftSubTitle = AppManager.Platform.LeftMenu.GetSubText(leftSubMenuID);
                this.ltlBreadCrumb.Text = StringUtils.GetBreadCrumbHtml(AppManager.Platform.AppID, topMenuID, topTitle, leftMenuID, leftTitle, leftSubMenuID, leftSubTitle, pageUrl, pageTitle, string.Empty, false);
            }

            if (!string.IsNullOrEmpty(permission))
            {
                AdminManager.VerifyPermissions(permission);
            }
        }

        public void BreadCrumbForService(string leftMenuID, string pageTitle, string permission)
        {
            if (this.ltlBreadCrumb != null)
            {
                string topMenuID = AppManager.Platform.TopMenu.ID_Service;
                string pageUrl = PathUtils.GetFileName(base.Request.FilePath);
                string topTitle = AppManager.Platform.TopMenu.GetText(topMenuID);
                string leftTitle = AppManager.Platform.LeftMenu.GetText(leftMenuID);
                this.ltlBreadCrumb.Text = StringUtils.GetBreadCrumbHtml(AppManager.Platform.AppID, topMenuID, topTitle, leftMenuID, leftTitle, pageUrl, pageTitle, string.Empty, false);
            }

            if (!string.IsNullOrEmpty(permission))
            {
                AdminManager.VerifyPermissions(permission);
            }
        }


        public void BreadCrumbForUserCenter(string leftMenuID, string pageTitle, string permission)
        {
            if (this.ltlBreadCrumb != null)
            {
                string topMenuID = AppManager.Platform.TopMenu.ID_UserCenter;
                string pageUrl = PathUtils.GetFileName(base.Request.FilePath);
                string topTitle = AppManager.Platform.TopMenu.GetText(topMenuID);
                string leftTitle = AppManager.User.LeftMenu.GetText(leftMenuID);
                this.ltlBreadCrumb.Text = StringUtils.GetBreadCrumbHtml(AppManager.Platform.AppID, topMenuID, topTitle, leftMenuID, leftTitle, pageUrl, pageTitle, string.Empty, false);
            }

            //if (!string.IsNullOrEmpty(permission))
            //{
            //    AdminManager.VerifyPermissions(permission);
            //}
        }

        public UserConfigInfoExtend Additional
        {
            get
            {
                return UserConfigManager.Additional;
            }
        }



    }
}
