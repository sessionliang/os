using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
    public interface IKeywordClassifyDAO : ITreeDAO
    {
        int InsertKeywordClassifyInfo(int publishmentSystemID, int parentID, string itemName, string itemIndexName);

        int InsertKeywordClassifyInfo(KeywordClassifyInfo keywordClassifyInfo);

        void UpdateKeywordClassifyInfo(KeywordClassifyInfo keywordClassifyInfo);

        KeywordClassifyInfo GetKeywordClassifyInfo(int itemID);

        void Delete(int deleteID);

        int SetDefaultKeywordClassifyInfo(int publishmentSystemID);
    }
}
