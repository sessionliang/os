using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using System.Collections;

namespace SiteServer.BBS
{
    public interface IBBSLogDAO
    {
        void Insert(BBSLogInfo log);

        void Delete(ArrayList idArrayList);

        void DeleteAll(int publishmentSystemID);

        string GetSelectCommend(int publishmentSystemID);

        string GetSelectCommend(int publishmentSystemID, int forumID, string logType, string userName, string keyword, string dateFrom, string dateTo);
    }
}