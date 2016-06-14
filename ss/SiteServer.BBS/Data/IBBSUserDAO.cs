using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;

using SiteServer.BBS.Model;

namespace SiteServer.BBS
{
    public interface IBBSUserDAO
    {
        void Insert(BBSUserInfo bbsUserInfo);

        void Update(BBSUserInfo bbsUserInfo);

        void UpdateCredit(string groupSN, string userName, int credits, int prestige, int contribution, int currency, int extCredit1, int extCredit2, int extCredit3);

        void AddPostCount(string groupSN, string userName, DateTime dateTime, int creditMultiplierPostCount, IDbTransaction trans);

        void Delete(string groupSN, string userName);

        BBSUserInfo GetBBSUserInfo(string groupSN, string userName);

        BBSUserInfo GetBBSUserInfo(int userID);

        bool IsExists(string groupSN, string userName);

        ArrayList GetUserNameList(string groupSN, ArrayList threadIDArrayList);
    }
}