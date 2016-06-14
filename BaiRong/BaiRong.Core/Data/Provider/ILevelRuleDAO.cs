using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
    public interface ILevelRuleDAO
    {
        void Insert(LevelRuleInfo LevelRuleInfo);

        void Update(LevelRuleInfo LevelRuleInfo);

        void Delete(int id);

        LevelRuleInfo GetLevelRuleInfo(int id);

        Hashtable GetLevelRuleInfoHashtable();
    }
}