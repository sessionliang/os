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
        /// ͨ��Ӧ��ID����Ŀ������ȡ��ĿID
        /// </summary>
        /// <param name="siteID">Ӧ��ID</param>
        /// <param name="channelIndex">��Ŀ����</param>
        /// <returns>��ĿID</returns>
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
