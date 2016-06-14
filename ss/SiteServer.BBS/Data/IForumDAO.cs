using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using System.Collections;

namespace SiteServer.BBS
{
    public interface IForumDAO
    {
        int InsertForumInfo(int publishmentSystemID, int parentID, string forumName, string indexName);

        int InsertForumInfo(int publishmentSystemID, ForumInfo forumInfo);

        void UpdateForumInfo(int publishmentSystemID, ForumInfo forumInfo);

        void UpdateForumByInsertPost(int publishmentSystemID, ForumInfo forumInfo, IDbTransaction trans);

        void UpdateCount(int publishmentSystemID, int forumID);

        void Delete(int publishmentSystemID, int forumID);

        void UpdateTaxis(int publishmentSystemID, int selectedForumID, bool isSubtract);

        ForumInfo GetForumInfo(int publishmentSystemID, int forumID);

        ArrayList GetIndexNameArrayList(int publishmentSystemID);

        int GetForumCount(int publishmentSystemID, int forumID);

        int GetForumIDByIndexName(int publishmentSystemID, string indexName);

        int GetForumIDByParentIDAndForumName(int publishmentSystemID, int parentID, string forumName, bool recursive);

        int GetForumIDByParentIDAndTaxis(int publishmentSystemID, int parentID, int taxis, bool isNextForum);

        bool IsExists(int publishmentSystemID, int forumID);

        ArrayList GetForumIDArrayList(int publishmentSystemID);

        ArrayList GetForumIDArrayListForDescendant(int publishmentSystemID, int forumID);

        ArrayList GetForumIDArrayListByParentID(int publishmentSystemID, int parentID);

        Hashtable GetForumInfoHashtable(int publishmentSystemID);

        ArrayList GetAllFilePath(int publishmentSystemID);

        string GetWhereString(int publishmentSystemID, string group, string groupNot, string where);

        IEnumerable GetParserDataSource(int publishmentSystemID, int forumID, int startNum, int totalNum, string whereString, string orderByString);
    }
}