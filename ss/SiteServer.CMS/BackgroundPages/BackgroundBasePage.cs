using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using BaiRong.Controls;
using BaiRong.Model;
using System.Text;
using System.Collections.Specialized;
using BaiRong.BackgroundPages;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundBasePage : BackgroundBasePageEnd
	{
        public bool HasChannelPermissions(int nodeID, params string[] channelPermissionArray)
        {
            return AdminUtility.HasChannelPermissions(this.PublishmentSystemID, nodeID, channelPermissionArray);
        }

        public bool HasChannelPermissionsIgnoreNodeID(params string[] channelPermissionArray)
        {
            return AdminUtility.HasChannelPermissionsIgnoreNodeID(channelPermissionArray);
        }

        public bool HasWebsitePermissions(params string[] websitePermissionArray)
        {
            return AdminUtility.HasWebsitePermissions(this.PublishmentSystemID, websitePermissionArray);
        }

        public bool IsOwningNodeID(int nodeID)
        {
            return AdminUtility.IsOwningNodeID(nodeID);
        }

        public bool IsHasChildOwningNodeID(int nodeID)
        {
            return AdminUtility.IsHasChildOwningNodeID(nodeID);
        }

        private int publishmentSystemID = -1;
        public int PublishmentSystemID
        {
            get
            {
                if (publishmentSystemID == -1)
                {
                    publishmentSystemID = base.GetIntQueryString("PublishmentSystemID");
                }
                return publishmentSystemID;
            }
        }

        private PublishmentSystemInfo publishmentSystemInfo;
        public PublishmentSystemInfo PublishmentSystemInfo
        {
            get
            {
                if (publishmentSystemInfo == null)
                {
                    publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.PublishmentSystemID);
                }
                return publishmentSystemInfo;
            }
        }

        public void BreadCrumbWithItemTitle(string leftMenuID, string pageTitle, string itemTitle, string permission)
        {
            this.BreadCrumbWithItemTitle(leftMenuID, string.Empty, pageTitle, itemTitle, permission);
        }

        public void BreadCrumbWithItemTitle(string leftMenuID, string leftSubMenuID, string pageTitle, string itemTitle, string permission)
        {
            if (base.ltlBreadCrumb != null)
            {
                string topMenuID = AppManager.CMS.TopMenu.ID_SiteManagement;
                string pageUrl = PathUtils.GetFileName(base.Request.FilePath);
                string topTitle = AppManager.CMS.TopMenu.GetText(topMenuID);
                string leftTitle = AppManager.CMS.LeftMenu.GetText(leftMenuID);
                string leftSubTitle = AppManager.CMS.LeftMenu.GetSubText(leftSubMenuID);
                this.ltlBreadCrumb.Text = StringUtils.GetBreadCrumbHtml(AppManager.CMS.AppID, topMenuID, topTitle, leftMenuID, leftTitle, leftSubMenuID, leftSubTitle, pageUrl, pageTitle, itemTitle, false);
            }

            if (!string.IsNullOrEmpty(permission))
            {
                AdminUtility.VerifyWebsitePermissions(this.PublishmentSystemID, permission);
            }
        }

        public override void BreadCrumb(string leftMenuID, string pageTitle, string permission)
        {
            this.BreadCrumbWithItemTitle(leftMenuID, pageTitle, string.Empty, permission);
        }

        public void BreadCrumb(string leftMenuID, string leftSubMenuID, string pageTitle, string permission)
        {
            this.BreadCrumbWithItemTitle(leftMenuID, leftSubMenuID, pageTitle, string.Empty, permission);
        }

        public void BreadCrumbConsole(string leftMenuID, string pageTitle, string permission)
        {
            this.BreadCrumbConsole(leftMenuID, string.Empty, pageTitle, permission);
        }

        public void BreadCrumbConsole(string leftMenuID, string leftSubMenuID, string pageTitle, string permission)
        {
            if (base.ltlBreadCrumb != null)
            {
                string topMenuID = AppManager.CMS.TopMenu.ID_SiteConfiguration;
                string pageUrl = PathUtils.GetFileName(base.Request.FilePath);
                string topTitle = AppManager.CMS.TopMenu.GetText(topMenuID);
                string leftTitle = AppManager.CMS.LeftMenu.GetText(leftMenuID);
                string leftSubTitle = AppManager.CMS.LeftMenu.GetSubText(leftSubMenuID);
                this.ltlBreadCrumb.Text = StringUtils.GetBreadCrumbHtml(AppManager.CMS.AppID, topMenuID, topTitle, leftMenuID, leftTitle, leftSubMenuID, leftSubTitle, pageUrl, pageTitle, string.Empty, false);
            }

            if (!string.IsNullOrEmpty(permission))
            {
                AdminManager.VerifyPermissions(permission);
            }
        }

        #region Input Basic

        private NameValueCollection attributes;
        public NameValueCollection Attributes
        {
            get
            {
                if (this.attributes == null)
                {
                    this.attributes = new NameValueCollection();
                }
                return this.attributes;
            }
        }

        public void AddAttributes(NameValueCollection attributes)
        {
            if (attributes != null)
            {
                foreach (string key in attributes.Keys)
                {
                    this.Attributes[key] = attributes[key];
                }
            }
        }

        public NameValueCollection GetAttributes()
        {
            return this.Attributes;
        }

        public virtual string GetValue(string attributeName)
        {
            if (this.attributes != null)
            {
                return this.attributes[attributeName];
            }
            return string.Empty;
        }

        public void SetValue(string name, string value)
        {
            this.Attributes[name] = value;
        }

        public void RemoveValue(string name)
        {
            this.Attributes.Remove(name);
        }

        public string GetSelected(string attributeName, string value)
        {
            if (this.attributes != null)
            {
                if (this.attributes[attributeName] == value)
                {
                    return @"selected=""selected""";
                }
            }
            return string.Empty;
        }

        public string GetSelected(string attributeName, string value, bool isDefault)
        {
            if (this.attributes != null)
            {
                if (this.attributes[attributeName] == value)
                {
                    return @"selected=""selected""";
                }
            }
            else
            {
                if (isDefault)
                {
                    return @"selected=""selected""";
                }
            }
            return string.Empty;
        }

        public string GetChecked(string attributeName, string value)
        {
            if (this.attributes != null)
            {
                if (this.attributes[attributeName] == value)
                {
                    return @"checked=""checked""";
                }
            }
            return string.Empty;
        }

        public string GetChecked(string attributeName, string value, bool isDefault)
        {
            if (this.attributes != null)
            {
                if (this.attributes[attributeName] == value)
                {
                    return @"checked=""checked""";
                }
            }
            else
            {
                if (isDefault)
                {
                    return @"checked=""checked""";
                }
            }
            return string.Empty;
        }

        #endregion

        #region 用户中心

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
        #endregion
    }
}
