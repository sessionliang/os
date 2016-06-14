using System;
using System.Collections;
using System.Text;

using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.Core
{
    public class ChannelManager
    {
        private ChannelManager()
        {

        }


        /// <summary>
        /// 通过应用ID和栏目索引获取栏目ID
        /// </summary>
        /// <param name="siteID">应用ID</param>
        /// <param name="channelIndex">栏目索引</param>
        /// <returns>栏目ID</returns>
        public static int GetChannelIDByChannelIndex(int siteID, string channelIndex)
        {
            return DataProvider.NodeDAO.GetNodeIDByNodeIndexName(siteID, channelIndex);
        }

        public static ArrayList GetChannelIDArrayList(int siteID, int channelID)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(siteID, channelID);
            return DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.All, string.Empty, string.Empty);
        }

        public static NodeInfo GetNodeInfo(int siteID, int channelID)
        {
            return NodeManager.GetNodeInfo(siteID, channelID);
        }
    }
}
