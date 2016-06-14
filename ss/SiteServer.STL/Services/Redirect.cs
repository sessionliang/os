using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using SiteServer.CMS.Model;
using System.Collections.Specialized;

using System.Web;
using BaiRong.Core.AuxiliaryTable;

using BaiRong.Core.Cryptography;
using SiteServer.CMS.Core;
using SiteServer.STL.Parser;

namespace SiteServer.CMS.Services
{
    public class Redirect : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            this.PageRedirect();
        }

        public void PageRedirect()
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(base.Request.QueryString["publishmentSystemID"]) && !string.IsNullOrEmpty(base.Request.QueryString["nodeID"]) && !string.IsNullOrEmpty(base.Request.QueryString["contentID"]))
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
                int nodeID = TranslateUtils.ToInt(base.Request.QueryString["nodeID"]);
                int contentID = TranslateUtils.ToInt(base.Request.QueryString["contentID"]);

                if (contentID != 0)
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                    url = PageUtility.GetContentUrl(publishmentSystemInfo, nodeInfo, contentID, publishmentSystemInfo.Additional.VisualType);
                    if (url == PageUtils.UNCLICKED_URL)
                    {
                        url = ConfigUtils.Instance.ApplicationPath;
                    }
                }
                else
                {
                    url = ConfigUtils.Instance.ApplicationPath;
                }
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["nodeID"]) && !string.IsNullOrEmpty(base.Request.QueryString["contentID"]))
            {
                int nodeID = TranslateUtils.ToInt(base.Request.QueryString["nodeID"]);
                int contentID = TranslateUtils.ToInt(base.Request.QueryString["contentID"]);

                if (contentID != 0)
                {
                    int publishmentSystemID = PathUtility.GetCurrentPublishmentSystemID();
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                    url = PageUtility.GetContentUrl(publishmentSystemInfo, nodeInfo, contentID, publishmentSystemInfo.Additional.VisualType);
                }
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["channelID"]))
            {
                int nodeID = TranslateUtils.ToInt(base.Request.QueryString["channelID"]);

                if (nodeID != 0)
                {
                    int publishmentSystemID = DataProvider.NodeDAO.GetPublishmentSystemID(nodeID);
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                    url = PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, publishmentSystemInfo.Additional.VisualType);
                }
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["channelindex"]))
            {
                string channelIndex = base.Request.QueryString["channelindex"];
                int publishmentSystemID = PathUtility.GetCurrentPublishmentSystemID();
                if (publishmentSystemID == 0)
                {
                    publishmentSystemID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByIsHeadquarters();
                }
                if (publishmentSystemID != 0)
                {
                    int nodeID = DataProvider.NodeDAO.GetNodeIDByNodeIndexName(publishmentSystemID, channelIndex);
                    if (nodeID != 0)
                    {
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                        url = PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, publishmentSystemInfo.Additional.VisualType);
                    }
                }
            }

            if (string.IsNullOrEmpty(url) || url == PageUtils.UNCLICKED_URL)
            {
                url = ConfigUtils.Instance.ApplicationPath;
            }

            PageUtils.Redirect(url);
        }
    }
}
