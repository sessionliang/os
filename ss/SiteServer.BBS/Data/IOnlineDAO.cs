using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using System.Collections;

namespace SiteServer.BBS
{
    public interface IOnlineDAO
    {
        void Insert(int publishmentSystemID, OnlineInfo onlineInfo);

        void DeleteByUserName(int publishmentSystemID, string userName);

        void DeleteBySessionID(int publishmentSystemID, string sessionID);

        void RemoveTimeOutUsers(int publishmentSystemID);

        /// <summary>
        ///更新用户活动时间。
        /// </summary>
        void ActiveTime(OnlineInfo onlineInfo);

        Hashtable GetOnlineInfoHashtable(int publishmentSystemID);
    }
}