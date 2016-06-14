using System.Collections;
using System.Collections.Specialized;
using System.Data;
using BaiRong.Model;
using SiteServer.CMS.Model;
using System.Collections.Generic;

namespace SiteServer.CMS.Core
{
    public interface INodeDAO
    {
        string TableName
        {
            get;
        }

        int InsertNodeInfo(int publishmentSystemID, int parentID, string nodeName, string nodeIndex, string contentModelID);

        int InsertNodeInfo(int publishmentSystemID, int parentID, string nodeName, string nodeIndex, string contentModelID, int chennelTemplateID, int contentTemplateID);

        int InsertNodeInfo(NodeInfo nodeInfo);

        int InsertPublishmentSystemInfo(NodeInfo nodeInfo, PublishmentSystemInfo psInfo);

        void UpdateNodeInfo(NodeInfo nodeInfo);

        void UpdateTaxis(int publishmentSystemID, int selectedNodeID, bool isSubtract);

        void UpdateContentNum(PublishmentSystemInfo publishmentSystemInfo, int nodeID, bool isRemoveCache);

        void UpdateContentNum(PublishmentSystemInfo publishmentSystemInfo);

        void UpdateContentNumToZero(string tableName, EAuxiliaryTableType tableType);

        void UpdateCommentNum(int publishmentSystemID, int nodeID, int comments);

        void AddNodeGroupArrayList(int publishmentSystemID, int nodeID, ArrayList nodeGroupArrayList);

        void Delete(int nodeID);

        NodeInfo GetNodeInfoByLastAddDate(int nodeID);

        NodeInfo GetNodeInfoByTaxis(int nodeID);

        NodeInfo GetNodeInfo(int nodeID);

        NodeInfo GetNodeInfoByParentID(int publishmentSystemID, int parentID, EContentModelType contentModelType);

        int GetPublishmentSystemID(int nodeID);

        int GetNodeID(int publishmentSystemID, string orderString);

        int GetNodeIDByParentIDAndNodeName(int publishmentSystemID, int parentID, string nodeName, bool recursive);

        int GetNodeIDByParentIDAndTaxis(int parentID, int taxis, bool isNextChannel);

        int GetNodeCountByPublishmentSystemID(int publishmentSystemID);

        int GetNodeCount(int nodeID);

        int GetSequence(int publishmentSystemID, int nodeID);

        int GetNodeIDByNodeIndexName(int publishmentSystemID, string nodeIndexName);

        int GetNodeIDByContentModelType(int publishmentSystemID, EContentModelType contentModelType);

        string GetOrderStringInPublishmentSystem(int nodeID);

        string GetSelectCommendByNodeGroupName(int publishmentSystemID, string nodeGroupName);

        bool IsExists(int nodeID);

        void UpdateChannelTemplateID(int nodeID, int channelTemplateID);

        void UpdateContentTemplateID(int nodeID, int contentTemplateID);

        ArrayList GetNodeIndexNameArrayList(int publishmentSystemID);

        ArrayList GetNodeIDArrayList();

        ArrayList GetNodeIDArrayList(int nodeID, int totalNum, string orderByString, string whereString, EScopeType scopeType, string group, string groupNot);

        ArrayList GetNodeIDArrayListByNodeType(params ENodeType[] eNodeTypeArray);

        ArrayList GetNodeIDArrayListByScopeType(int nodeID, EScopeType scopeType, string group, string groupNot);

        ArrayList GetNodeIDArrayListByScopeType(NodeInfo nodeInfo, EScopeType scopeType, string group, string groupNot);

        ArrayList GetNodeIDArrayListByScopeType(NodeInfo nodeInfo, EScopeType scopeType, string group, string groupNot, string contentModelID);

        ArrayList GetNodeIDArrayListForDescendant(int nodeID);

        List<int> GetNodeIDListForDescendant(int nodeID);

        ArrayList GetNodeIDArrayListByPublishmentSystemID(int publishmentSystemID);

        ArrayList GetNodeIDArrayListByParentID(int publishmentSystemID, int parentID);

        ArrayList GetNodeInfoArrayListByParentID(int publishmentSystemID, int parentID);

        Hashtable GetNodeInfoHashtableByPublishmentSystemID(int publishmentSystemID);

        IEnumerable GetDataSource(ArrayList nodeIDArrayList, int totalNum, string whereString, string orderByString);

        DataSet GetDataSet(ArrayList nodeIDArrayList, int totalNum, string whereString, string orderByString);

        IEnumerable GetStlDataSource(NodeInfo nodeInfo, int startNum, int totalNum, string whereString, EScopeType scopeType, string orderByString);

        IEnumerable GetStlDataSourceByPublishmentSystemID(int publishmentSystemID, int startNum, int totalNum, string whereString, string orderByString);

        DataSet GetStlDataSet(NodeInfo nodeInfo, int startNum, int totalNum, string whereString, EScopeType scopeType, string orderByString);

        DataSet GetStlDataSetByPublishmentSystemID(int publishmentSystemID, int startNum, int totalNum, string whereString, string orderByString);

        ArrayList GetNodeInfoArrayListByPublishmentSystemID(int publishmentSystemID, string whereString);

        ArrayList GetNodeInfoArrayList(NodeInfo nodeInfo, int totalNum, string whereString, EScopeType scopeType, string orderByString);

        string GetWhereString(int publishmentSystemID, string group, string groupNot, bool isImageExists, bool isImage, string where);

        ArrayList GetLowerSystemDirArrayList(int parentPublishmentSystemID);

        //辅助属性

        //void InsertNodeAttributes(int publishmentSystemID, int nodeID, string tableName, NameValueCollection nameValueCollection);

        //void UpdateNodeAttributes(int publishmentSystemID, int nodeID, string tableName, NameValueCollection nameValueCollection);

        //void DeleteNodeAttributes(int publishmentSystemID, ArrayList nodeIDArrayList, string tableName);

        int GetContentNumByPublishmentSystemID(int publishmentSystemID);

        //int GetNodeAttributesID(int publishmentSystemID, int nodeID, string tableName);

        //NameValueCollection GetNodeAttributes(int publishmentSystemID, int nodeID);

        //bool IsNodeAttributesExist(int publishmentSystemID, int nodeID);

        ArrayList GetAllFilePathByPublishmentSystemID(int publishmentSystemID);

        ArrayList GetNodeIDArrayListByChildNodeID(int publishmentSystemID, int childNodeID, ArrayList nodeIDArrayList);

        ArrayList GetNodeInfoByNodeIndexOrNodeName(int publishmentSystemID, string filterSearchWords, int count);

        ArrayList GetNodeInfoByBrandName(int publishmentSystemID, string filterSearchWords, int count);

        ArrayList GetNodeInfoByConentName(int publishmentSystemID, string filterSearchWords, int count);

        #region 评价管理 by 20161029 sofuny

        List<int> GetNodeIDListForDescendantUseEvaluation(int publishmentSystemID, int nodeID);

        ArrayList GetNodeIDByFunction(int publishmentSystemID, string functionStr);         


        ArrayList GetNodeIDArrayListByParentID(int publishmentSystemID, int parentID,ArrayList nodeIDList);
        #endregion
    }
}
