using BaiRong.Model;
using System.Collections;
using System;
using System.Data;
using System.Collections.Specialized;

namespace BaiRong.Core.Data.Provider
{
    public interface IContentDAO
    {
        int Insert(string tableName, ContentInfo contentInfo);

        void Update(string tableName, ContentInfo contentInfo);

        bool UpdateTaxisToUp(string tableName, int nodeID, int contentID, bool isTop);

        bool UpdateTaxisToDown(string tableName, int nodeID, int contentID, bool isTop);

        int GetMaxTaxis(string tableName, int nodeID, bool isTop);

        int GetTaxis(int selectedID, string tableName);

        void SetTaxis(int id, int taxis, string tableName);

        void UpdateIsChecked(string tableName, int publishmentSystemID, int nodeID, ArrayList contentIDArrayList, int translateNodeID, bool isAdmin, string userName, bool isChecked, int checkedLevel, string reasons);

        void UpdateIsChecked(string tableName, int publishmentSystemID, int nodeID, ArrayList contentIDArrayList, int translateNodeID, bool isAdmin, string userName, bool isChecked, int checkedLevel, string reasons, bool isCheck);

        void AddHits(string tableName, bool isCountHits, bool isCountHitsByDay, int contentID);

        void UpdateComments(string tableName, int contentID, int comments);

        void UpdatePhotos(string tableName, int contentID, int photos);

        void UpdateTeleplays(string tableName, int contentID, int teleplays);

        int GetReferenceID(ETableStyle tableStyle, string tableName, int contentID, out string linkUrl, out int nodeID);

        int GetReferenceID(ETableStyle tableStyle, string tableName, int contentID, out string linkUrl);

        int GetCountOfContentAdd(string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, DateTime begin, DateTime end, string userName);

        int GetCountOfContentUpdate(string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, DateTime begin, DateTime end, string userName);

        string GetSelectCommendByCondition(ETableStyle tableStyle, string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, ETriState checkedState, bool isNoDup, bool isTrashContent);

        string GetSelectCommendByCondition(ETableStyle tableStyle, string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, ETriState checkedState, bool isNoDup, bool isTrashContent, bool isViewContentOnlySelf);

        string GetSelectCommend(string tableName, int nodeID, ETriState checkedState);

        string GetSelectCommend(string tableName, int nodeID, ETriState checkedState, bool isViewContentOnlySelf);

        string GetSelectCommend(string tableName, ArrayList nodeIDArrayList, ETriState checkedState);

        string GetSelectCommendByWhere(string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, string where, ETriState checkedState);

        string GetValue(string tableName, int contentID, string name);

        void SetValue(string tableName, int contentID, string name, string value);

        void AddContentGroupArrayList(string tableName, int contentID, ArrayList contentGroupArrayList);

        ArrayList GetReferenceIDArrayList(string tableName, ArrayList contentIDArrayList);

        ArrayList GetContentIDArrayList(string tableName, int nodeID);

        ArrayList GetContentIDArrayList(string tableName, int nodeID, bool isPeriods, string dateFrom, string dateTo, ETriState checkedState);

        ArrayList GetContentIDArrayListByPublishmentSystemID(string tableName, int publishmentSystemID);

        ArrayList GetContentIDArrayListChecked(string tableName, ArrayList nodeIDArrayList, int totalNum, string orderByString, string whereString);

        ArrayList GetContentIDArrayListByTrash(int publishmentSystemID, string tableName);

        int TrashContents(int publishmentSystemID, string tableName, ArrayList contentIDArrayList);

        int TrashContentsByNodeID(int publishmentSystemID, string tableName, int nodeID);

        int DeleteContents(string productID, int publishmentSystemID, string tableName, ArrayList contentIDArrayList);

        int DeleteContentsByNodeID(string productID, int publishmentSystemID, string tableName, int nodeID, ArrayList contentIDArrayList);

        void DeleteContentsArchive(int publishmentSystemID, string tableName, ArrayList contentIDArrayList);

        void DeleteContentsByTrash(string productID, int publishmentSystemID, string tableName);

        int RestoreContentsByTrash(int publishmentSystemID, string tableName);

        // 以栏目ID和内容排序号为搜索条件得到内容的ID
        int GetContentID(string tableName, int nodeID, int taxis, bool isNextContent);

        //根据排序规则获得第一条内容的ID
        int GetContentID(string tableName, int nodeID, string orderByString);

        int GetContentID(string tableName, int nodeID, string attributeName, string value);

        ArrayList GetValueArrayList(string tableName, int nodeID, string name);

        ArrayList GetValueArrayListByStartString(string tableName, int nodeID, string name, string startString, int totalNum);

        int GetNodeID(string tableName, int contentID);

        DateTime GetAddDate(string tableName, int contentID);

        DateTime GetLastEditDate(string tableName, int contentID);

        int GetCount(string tableName, int nodeID);

        int GetCountChecked(string tableName, int nodeID, int days);

        int GetCountChecked(string tableName, int nodeID, string whereString);

        int GetSequence(string tableName, int nodeID, int contentID);

        string GetSelectCommendOfAdminExcludeRecycle(string tableName, int publishmentSystemID, DateTime begin, DateTime end);

        ArrayList GetNodeIDArrayListCheckedByLastEditDateHour(string tableName, int publishmentSystemID, int hour);

        string GetSelectedCommendByCheck(string tableName, int publishmentSystemID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, ArrayList checkLevelArrayList);

        IEnumerable GetStlDataSourceChecked(string tableName, ArrayList nodeIDArrayList, int startNum, int totalNum, string orderByString, string whereString, bool isNoDup, NameValueCollection otherAttributes);

        int GetStlCountChecked(string tableName, ArrayList nodeIDArrayList, string whereString);

        string GetStlWhereString(string productID, int publishmentSystemID, string group, string groupNot, string tags, bool isTopExists, bool isTop, string where);

        string GetSortFieldName();

        ArrayList GetContentIDArrayListCheck(int publishmentSystemID, int nodeID, string tableName);

        ArrayList GetContentIDArrayListUnCheck(int publishmentSystemID, int nodeID, string tableName);

        DataSet GetDataSetOfAdminExcludeRecycle(string tableName, int publishmentSystemID, DateTime begin, DateTime end);


        /// <summary>
        /// 投稿内容
        /// </summary>
        /// <param name="tableStyle"></param>
        /// <param name="tableName"></param>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeIDArrayList"></param>
        /// <param name="searchType"></param>
        /// <param name="keyword"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="checkedState"></param>
        /// <param name="isNoDup"></param>
        /// <param name="isTrashContent"></param>
        /// <param name="isViewContentOnlySelf"></param>
        /// <param name="isMLib"></param>
        /// <returns></returns>
        string GetSelectCommendByMLib(ETableStyle tableStyle, string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, ETriState checkedState, bool isNoDup, bool isTrashContent, bool isViewContentOnlySelf,string memberName, bool isMLib);

        ArrayList GetUserContent(string tableName, string whereString, int pageIndex, int prePageNum);


        string GetSelectCommendOfDeptExcludeRecycle(string tableName, int publishmentSystemID, DateTime begin, DateTime end, int detpID, string userName);


        string GetSelectCommendBySelectType(ETableStyle tableStyle, string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, ETriState checkedState, bool isNoDup, bool isTrashContent, string selectType, string adminName);

    }
}
