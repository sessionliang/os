using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using BaiRong.Core;
using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
    public interface IUserLevelDAO
    {
        int Insert(UserLevelInfo LevelInfo);

        void UpdateWithCredits(string LevelSN, UserLevelInfo userLevelInfo, int oldCreditsFrom, int oldCreditsTo);

        void UpdateWithoutCredits(string LevelSN, UserLevelInfo userLevelInfo);

        void Delete(string LevelSN, int LevelID);

        UserLevelInfo GetUserLevelInfo(int LevelID);

        bool IsExists(string LevelSN, string LevelName);

        UserLevelInfo GetUserLevelByLevelName(string LevelSN, string LevelName);

        bool IsCreditsValid(string LevelSN, int creditsFrom, int creditsTo);

        DictionaryEntryArrayList GetUserLevelInfoDictionaryEntryArrayList(string LevelSN);

        List<int> GetLevelIDList(string LevelSN);

        void CreateDefaultUserLevel(string LevelSN);
    }
}