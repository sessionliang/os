using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using SiteServer.CMS.Core.Security;
using System.Collections;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Model;
using System.Text;

using System;

namespace SiteServer.CMS.Core
{
	public class GovInteractManager
	{
        public static void Initialize(PublishmentSystemInfo publishmentSystemInfo)
        {
            if (publishmentSystemInfo.Additional.GovInteractNodeID > 0)
            {
                if (!DataProvider.NodeDAO.IsExists(publishmentSystemInfo.Additional.GovInteractNodeID))
                {
                    publishmentSystemInfo.Additional.GovInteractNodeID = 0;
                }
            }
            if (publishmentSystemInfo.Additional.GovInteractNodeID == 0)
            {
                int govInteractNodeID = DataProvider.NodeDAO.GetNodeIDByContentModelType(publishmentSystemInfo.PublishmentSystemID, EContentModelType.GovInteract);
                if (govInteractNodeID == 0)
                {
                    govInteractNodeID = DataProvider.NodeDAO.InsertNodeInfo(publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.PublishmentSystemID, "互动交流", string.Empty, EContentModelTypeUtils.GetValue(EContentModelType.GovInteract));
                }
                publishmentSystemInfo.Additional.GovInteractNodeID = govInteractNodeID;
                DataProvider.PublishmentSystemDAO.Update(publishmentSystemInfo);
            }
        }

        public static ArrayList GetNodeInfoArrayList(PublishmentSystemInfo publishmentSystemInfo)
        {
            ArrayList nodeInfoArrayList = new ArrayList();
            if (publishmentSystemInfo != null && publishmentSystemInfo.Additional.GovInteractNodeID > 0)
            {
                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(publishmentSystemInfo.Additional.GovInteractNodeID);
                foreach (int nodeID in nodeIDArrayList)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                    if (nodeInfo != null && EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.GovInteract))
                    {
                        nodeInfoArrayList.Add(nodeInfo);
                    }
                }
            }
            return nodeInfoArrayList;
        }

        public static void AddDefaultTypeInfos(int publishmentSystemID, int nodeID)
        {
            GovInteractTypeInfo typeInfo = new GovInteractTypeInfo(0, "求决", nodeID, publishmentSystemID, 0);
            DataProvider.GovInteractTypeDAO.Insert(typeInfo);
            typeInfo = new GovInteractTypeInfo(0, "举报", nodeID, publishmentSystemID, 0);
            DataProvider.GovInteractTypeDAO.Insert(typeInfo);
            typeInfo = new GovInteractTypeInfo(0, "投诉", nodeID, publishmentSystemID, 0);
            DataProvider.GovInteractTypeDAO.Insert(typeInfo);
            typeInfo = new GovInteractTypeInfo(0, "咨询", nodeID, publishmentSystemID, 0);
            DataProvider.GovInteractTypeDAO.Insert(typeInfo);
            typeInfo = new GovInteractTypeInfo(0, "建议", nodeID, publishmentSystemID, 0);
            DataProvider.GovInteractTypeDAO.Insert(typeInfo);
            typeInfo = new GovInteractTypeInfo(0, "感谢", nodeID, publishmentSystemID, 0);
            DataProvider.GovInteractTypeDAO.Insert(typeInfo);
            typeInfo = new GovInteractTypeInfo(0, "其他", nodeID, publishmentSystemID, 0);
            DataProvider.GovInteractTypeDAO.Insert(typeInfo);
        }

        public static ArrayList GetFirstDepartmentIDArrayList(GovInteractChannelInfo channelInfo)
        {
            if (channelInfo == null || string.IsNullOrEmpty(channelInfo.DepartmentIDCollection))
            {
                return BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByParentID(0);
            }
            else
            {
                return BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByDepartmentIDCollection(channelInfo.DepartmentIDCollection);
            }
        }

        public static string GetTypeName(int typeID)
        {
            if (typeID > 0)
            {
                return DataProvider.GovInteractTypeDAO.GetTypeName(typeID);
            }
            return string.Empty;
        }

        public static bool IsPermission(int publishmentSystemID, int nodeID, string permission)
        {
            ArrayList govInteractPermissionArrayList = ProductPermissionsManager.Current.GovInteractPermissionSortedList[publishmentSystemID] as ArrayList;
            if (govInteractPermissionArrayList == null || govInteractPermissionArrayList.Count == 0)
            {
                govInteractPermissionArrayList = ProductPermissionsManager.Current.GovInteractPermissionSortedList[nodeID] as ArrayList;
            }
            if (govInteractPermissionArrayList != null)
            {
                return govInteractPermissionArrayList.Contains(permission);
            }
            return false;
        }
	}
}
