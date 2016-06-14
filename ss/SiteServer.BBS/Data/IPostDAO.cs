using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using System.Collections;

namespace SiteServer.BBS {

    public interface IPostDAO
    {
        int InsertWithThread(int publishmentSystemID, PostInfo info, IDbTransaction trans);

        int InsertPostOnly(int publishmentSystemID, PostInfo info);

        int ImportPost(int publishmentSystemID, PostInfo info);

        void Update(int publishmentSystemID, PostInfo info);

        void UpdateContent(int publishmentSystemID, int postID, string content);

        void Handle(int publishmentSystemID, List<int> threadIDList);

        void Delete(int publishmentSystemID, int forumID, int threadID, List<int> postIDList);

        void DeleteAll(int publishmentSystemID);

        void DeletePostTrash(int publishmentSystemID, int forumID, int threadID, List<int> postIDList);

        void DeleteByThreadIDList(int publishmentSystemID, List<int> threadIDList);

        void DeleteSingleThreadPostTrash(int publishmentSystemID, int forumID, List<int> threadIDList);

        void Restore(int publishmentSystemID, List<int> postIDList);

        void AllRestore(int publishmentSystemID);

        void Ban(int publishmentSystemID, int forumID, List<int> postIDList, bool isBanned);

        void BanThreadIDList(int publishmentSystemID, int forumID, List<int> threadIDList, bool isBanned);

        PostInfo GetPostInfo(int publishmentSystemID, int postID);

        int GetPostIDByTaxis(int publishmentSystemID, int forumID, int threadID, int taxis);

        int GetPostCount(int publishmentSystemID, int forumID);

        string GetValue(int publishmentSystemID, int postID, string name);

        string GetThreadValue(int publishmentSystemID, int threadID, string name);

        string GetSqlString(int publishmentSystemID, string userName, string title, string dateFrom, string dateTo, string forumID);

        string GetSqlStringTrash(int publishmentSystemID, string userName, string title, string dateFrom, string dateTo);

        bool IsReply(int publishmentSystemID, int threadID, string userName);

        IEnumerable GetParserDataSource(int publishmentSystemID, int threadID, int startNum, int totalNum, bool isThreadExists, bool isThread, string orderByString, string whereString);

        IEnumerable GetDataSourceByIsHandled(int publishmentSystemID, int startNum, int totalNum, int days);

        int GetDataSourceCountByIsHandled(int publishmentSystemID, int days);

        IEnumerable GetDataSourceByIsMy(int publishmentSystemID, int startNum, int totalNum, string userName);

        int GetDataSourceCountByIsMy(int publishmentSystemID, string userName);

        int GetMaxTaxis(int publishmentSystemID, int threadID);

        int GetTaxis(int publishmentSystemID, int selectedID);

        void SetTaxis(int publishmentSystemID, int id, int taxis);

        int GetPostCount(int publishmentSystemID);

        int GetPostCount(int publishmentSystemID, DateTime date);

        void UpDownPost(int publishmentSystemID, string postIDList, bool isUp, int threadID);

        void UpdateTaxisToUp(int publishmentSystemID, int id, int threadID);

        void UpdateTaxisToDown(int publishmentSystemID, int id, int threadID);

        List<PostInfo> GetNotPassedPost(int publishmentSystemID);

        void Delete(int publishmentSystemID, int postID);

        void Pass(int publishmentSystemID, int postID);
    }
}
