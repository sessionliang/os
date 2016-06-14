using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using BaiRong.Core;
using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
    public interface IUserGroupDAO
    {
        int Insert(UserGroupInfo groupInfo);

        void UpdateWithCredits(string groupSN, UserGroupInfo userGroupInfo, int oldCreditsFrom, int oldCreditsTo);

        void UpdateWithoutCredits(string groupSN, UserGroupInfo userGroupInfo);

        void Delete(string groupSN, int groupID);

        UserGroupInfo GetUserGroupInfo(int groupID);

        bool IsExists(string groupSN, string groupName);

        UserGroupInfo GetUserGroupByGroupName(string groupSN, string groupName);

        bool IsCreditsValid(string groupSN, int creditsFrom, int creditsTo);

        DictionaryEntryArrayList GetUserGroupInfoDictionaryEntryArrayList(string groupSN);

        List<int> GetGroupIDList(string groupSN);

        void CreateDefaultUserGroup(string groupSN);
    }
}