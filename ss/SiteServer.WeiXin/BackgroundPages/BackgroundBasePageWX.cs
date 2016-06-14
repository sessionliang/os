using BaiRong.BackgroundPages;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Model;
using SiteServer.WeiXin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundBasePageWX : BackgroundBasePageEnd
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
                    publishmentSystemID = TranslateUtils.ToInt(Request.QueryString["PublishmentSystemID"]);
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

        public void BreadCrumb(string topMenuID, string leftMenuID, string pageTitle, string permission)
        {
            this.BreadCrumbConsole(topMenuID, leftMenuID, string.Empty, pageTitle, permission);
        }

        public void BreadCrumbConsole(string topMenuID, string leftMenuID, string leftSubMenuID, string pageTitle, string permission)
        {
            if (base.ltlBreadCrumb != null)
            {
                string pageUrl = PathUtils.GetFileName(base.Request.FilePath);
                string topTitle = AppManager.WeiXin.LeftMenu.GetText(topMenuID);
                string leftTitle = AppManager.WeiXin.LeftMenu.GetSubText(leftMenuID);
                string leftSubTitle = AppManager.WeiXin.LeftMenu.GetSubText(leftSubMenuID);
                this.ltlBreadCrumb.Text = StringUtils.GetBreadCrumbHtml(AppManager.WeiXin.AppID, topMenuID, topTitle, leftMenuID, leftTitle, leftSubMenuID, leftSubTitle, pageUrl, pageTitle, string.Empty, false);

                if (FileConfigManager.Instance.IsSaas && FileConfigManager.Instance.SSOConfig.IntegrationType == EIntegrationType.QCloud)
                {
                    this.ltlBreadCrumb.Text += @"<script src=""http://qzonestyle.gtimg.cn/qcloud/app/open/api/v1/wxcloud.js""></script>";
                }
            }

            if (!string.IsNullOrEmpty(permission))
            {
                AdminUtility.VerifyWebsitePermissions(this.PublishmentSystemID, permission);
            }
        }

        public override void AddWaitAndRedirectScript(string redirectUrl)
        {
            bool qCloudJS = false;

            if (FileConfigManager.Instance.IsSaas && FileConfigManager.Instance.SSOConfig.IntegrationType == EIntegrationType.QCloud)
            {
                List<string> keywordList = DataProviderWX.KeywordMatchDAO.GetKeywordListEnabled(this.publishmentSystemID);
                if (keywordList != null && keywordList.Count > 0)
                {
                    base.AddScript(string.Format(@"
function setKeywordsOK(e){{
    location.href = '{1}';
}}
wxCloud.setKeywords([{0}], 'setKeywordsOK');
$('.operation-area').hide();
", TranslateUtils.ObjectCollectionToSqlInStringWithQuote(keywordList), redirectUrl));
                    qCloudJS = true;
                }
            }

            if (!qCloudJS)
            {
                base.AddWaitAndRedirectScript(redirectUrl);
            }
        }
    }
}
