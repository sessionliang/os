using System;
using System.Collections;
using System.Data;
using BaiRong.Model;
using SiteServer.CMS.Model;
using System.Text;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core
{
    public interface IContentDAO
    {
        int GetTaxisToInsert(string tableName, int nodeID, bool isTop);

        int Insert(string tableName, PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo);

        int Insert(string tableName, PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo, bool isUpdateContentNum, int taxis);

        void Update(string tableName, PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo);

        void UpdateAutoPageContent(string tableName, PublishmentSystemInfo publishmentSystemInfo);

        ContentInfo GetContentInfoNotTrash(ETableStyle tableStyle, string tableName, int contentID);

        ContentInfo GetContentInfo(ETableStyle tableStyle, string tableName, int contentID);

        ContentInfo GetContentInfo(ETableStyle tableStyle, string sqlString);

        int GetCountOfContentAdd(string tableName, int publishmentSystemID, int nodeID, DateTime begin, DateTime end, string userName);

        int GetCountOfContentUpdate(string tableName, int publishmentSystemID, int nodeID, DateTime begin, DateTime end, string userName);

        string GetSelectCommend(ETableStyle tableStyle, string tableName, int publishmentSystemID, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, bool isSearchChildren, ETriState checkedState);

        string GetSelectCommend(ETableStyle tableStyle, string tableName, int publishmentSystemID, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, bool isSearchChildren, ETriState checkedState, bool isNoDup, bool isTrashContent);

        string GetSelectCommend(ETableStyle tableStyle, string tableName, int publishmentSystemID, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, bool isSearchChildren, ETriState checkedState, bool isNoDup, bool isTrashContent, bool isViewContentOnlySelf);

        ArrayList GetContentIDArrayListChecked(string tableName, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, int totalNum, string orderByString, string whereString, EScopeType scopeType);

        ArrayList GetContentIDArrayListChecked(string tableName, int nodeID, int totalNum, string orderByString, string whereString, EScopeType scopeType);

        ArrayList GetContentIDArrayListChecked(string tableName, int nodeID, int totalNum, string orderByFormatString, string whereString);

        ArrayList GetContentIDArrayListChecked(string tableName, int nodeID, string orderByFormatString, string whereString);

        ArrayList GetContentIDArrayListChecked(string tableName, int nodeID, int totalNum, string orderByFormatString);

        ArrayList GetContentIDArrayListChecked(string tableName, int nodeID, string orderByFormatString);

        void TrashContents(int publishmentSystemID, string tableName, ArrayList contentIDArrayList, int nodeID);

        void TrashContents(int publishmentSystemID, string tableName, ArrayList contentIDArrayList);

        void TrashContentsByNodeID(int publishmentSystemID, string tableName, int nodeID);

        void DeleteContents(int publishmentSystemID, string tableName, ArrayList contentIDArrayList, int nodeID);

        void DeleteContentsByPreview(int publishmentSystemID, string tableName, int nodeID);

        void DeleteContentsByNodeID(int publishmentSystemID, string tableName, int nodeID);

        void RestoreContentsByTrash(int publishmentSystemID, string tableName);

        string GetSelectCommendByContentGroup(string tableName, string contentGroupName, int publishmentSystemID);

        IEnumerable GetStlDataSourceChecked(string tableName, int nodeID, int startNum, int totalNum, string orderByString, string whereString, EScopeType scopeType, string groupChannel, string groupChannelNot, bool isNoDup, NameValueCollection otherAttributes);

        string GetStlSqlStringChecked(string tableName, int publishmentSystemID, int nodeID, int startNum, int totalNum, string orderByString, string whereString, EScopeType scopeType, string groupChannel, string groupChannelNot, bool isNoDup);

        //整理
        void TidyUp(string tableName, int nodeID, string attributeName, bool isDESC);

        bool SetWhereStringBySearch(StringBuilder whereBuilder, PublishmentSystemInfo publishmentSystemInfo, int nodeID, ETableStyle tableStyle, string word, StringCollection typeCollection, string channelID, string dateFrom, string dateTo, string date, string dateAttribute, string excludeAttributes, NameValueCollection form, ECharset charset, SearchwordSettingInfo settingInfo);

        ContentInfo GetContentInfoByTitle(ETableStyle tableStyle, string tableName, string title);

        /// <summary>
        /// 投稿内容
        /// </summary>
        /// <param name="tableStyle"></param>
        /// <param name="tableName"></param>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="isSystemAdministrator"></param>
        /// <param name="owningNodeIDArrayList"></param>
        /// <param name="searchType"></param>
        /// <param name="keyword"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="isSearchChildren"></param>
        /// <param name="checkedState"></param>
        /// <param name="isNoDup"></param>
        /// <param name="isTrashContent"></param>
        /// <param name="isViewContentOnlySelf"></param>
        /// <param name="isMLib"></param>
        /// <returns></returns>
        string GetSelectCommend(ETableStyle tableStyle, string tableName, int publishmentSystemID, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, bool isSearchChildren, ETriState checkedState, bool isNoDup, bool isTrashContent, bool isViewContentOnlySelf, bool isMLib);


        string GetSelectCommend(ETableStyle tableStyle, string tableName, int publishmentSystemID, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, bool isSearchChildren, ETriState checkedState, bool isNoDup, bool isTrashContent, bool isViewContentOnlySelf, string memberName, bool isMLib);


        DataSet GetDateSet(string tableName, int publishmentSystemID, string keyword, string dateFrom, string dateTo, bool isChecked, bool isMLib,string userName);

        /// <summary>
        /// 检测栏目下是否存在该标题的内容
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="nodeID"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        ArrayList GetIDListBySameTitleInOneNode(string tableName, int nodeID, string title);

        /// <summary>
        /// 稿件添加量统计
        /// add by sofuny at 20160201
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="tableName"></param>
        /// <param name="dataFrom"></param>
        /// <param name="dataTo"></param>
        /// <param name="xType"></param>
        /// <returns></returns>
        Hashtable GetTrackingHashtable(int publishmentSystemID, string tableName, DateTime dataFrom, DateTime dataTo, string xType);


        void UpdateSettingXML(string tableName, int publishmentSystemID, int nodeID, ArrayList contentInfoArrayList);


        ArrayList GetContentInfoArrayList(string tableName, ETableStyle tableStyle, int publishmentSystemID, int nodeID, ArrayList contentIDs);


        string GetSelectCommendBySelectType(ETableStyle tableStyle, string tableName, int publishmentSystemID, int nodeID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, bool isSearchChildren, ETriState checkedState, bool isNoDup, bool isTrashContent, string selectType,string adminName);
    }
}
