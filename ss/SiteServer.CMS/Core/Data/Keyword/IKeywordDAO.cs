using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
    public interface IKeywordDAO
    {
        void Insert(KeywordInfo keywordInfo);

        void Update(KeywordInfo keywordInfo);

        KeywordInfo GetKeywordInfo(int keywordID);

        void Delete(int keywordID);

        void Delete(ArrayList idArrayList);

        string GetSelectCommand(int classifyID);

        bool IsExists(string keyword);

        List<KeywordInfo> GetKeywordInfoList(int classifyID);

        List<KeywordInfo> GetKeywordInfoList(ArrayList keywords);

        ArrayList GetKeywordArrayListByContent(string content);

        void DeleteByClassifyID(int itemID);
    }
}
