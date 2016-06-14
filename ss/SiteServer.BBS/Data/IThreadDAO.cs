using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using System.Collections;

namespace SiteServer.BBS {

    public interface IThreadDAO
    {
        int Insert(int publishmentSystemID, int areaID, int forumID, int categoryID, string title, string content, bool isChecked, bool isSignature, bool isAttachment, EThreadType threadType, out int postID);

        int ImportThread(ThreadInfo threadInfo);

        void Update(ThreadInfo info);

        void UpdateIsLocked(string threadIDList, bool isLocked);

        void UpdateIdentifyID(string threadIDList, int identifyID);

        void UpdateThreadByInsertPost(int threadID, int lastPostID, string lastUserName, IDbTransaction trans);

        void Delete(int publishmentSystemID, int forumID, List<int> threadIDList);

        void DeleteAllByTrash(int publishmentSystemID);

        void DeleteThreadTrash(int publishmentSystemID, int forumID, List<int> threadIDList);

        void Restore(int publishmentSystemID, List<int> threadIDList);

        void AllRestore(int publishmentSystemID);

        ThreadInfo GetThreadInfo(int publishmentSystemID, int threadID);

        DateTime GetAddDate(int threadID);

        int GetReplies(int threadID);

        string GetTitle(int threadID);

        int GetCategoryID(int threadID);

        int GetThreadCount(int publishmentSystemID, int forumID);

        int GetForumID(int threadID);

        void AddHits(int threadID);

        int GetThreadID(int publishmentSystemID, int forumID, int taxis, bool isNextThread);

        string GetValue(int threadID, string name);

        ArrayList GetThreadIDArrayListChecked(int publishmentSystemID, int forumID, bool isAllChildren, int totalNum, string whereString);

        DataSet GetDataSource(int publishmentSystemID, string strWhere);

        string GetSqlString(int publishmentSystemID);

        string GetSqlString(int publishmentSystemID, string userName, string title, string dateFrom, string dateTo, string forumID);

        string GetSqlTrashString(int publishmentSystemID);

        void TranslateThreadByForumID(int publishmentSystemID, int sourceForumID, int targetForumID);

        void TranslateThread(int publishmentSystemID, string threadIDList, int targetForumID);

        void CategoryThread(string threadIDList, int categoryID);

        void UpDownThread(string threadIDList, bool isUp, int forumID);

        void TopLevelThread(string threadIDList, DateTime topLevelDate, int topLevel);

        void DigestThread(string threadIDList, DateTime digestDate, int digest);

        void HighlightThreads(int publishmentSystemID, int areaID, int forumID, string threadIDList, bool isTopExists, int topLevel, string topLevelDate, bool isDigestExists, int digestLevel, string digestDate, bool isHighlightExists, string highlight, string highlightDate);

        ArrayList GetTopLevelThreadInfoArrayList(int publishmentSystemID, int topLevel, int areaID, int forumID);

        string GetParserWhereString(int publishmentSystemID, int categoryID, string type, bool isTopExists, bool isTop, string where);

        IEnumerable GetParserDataSourceChecked(int publishmentSystemID, int forumID, int startNum, int totalNum, string orderByString, string whereString, bool isAllChildren);

        int GetParserDataSourceCheckedCount(int publishmentSystemID, int forumID, string whereString, bool isAllChildren);

        int GetMaxTaxis(int forumID, int topLevel);

        int GetTaxis(int selectedID);

        void SetTaxis(int id, int taxis);

        int GetToadyThreadCount(int publishmentSystemID, string userName, DateTime fromTime, DateTime toTime);
    }
}
