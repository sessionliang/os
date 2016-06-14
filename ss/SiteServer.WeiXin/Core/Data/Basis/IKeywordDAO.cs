using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IKeywordDAO
	{
        int Insert(KeywordInfo keywordInfo);

        void Update(KeywordInfo keywordInfo);

        void Update(int publishmentSystemID, int keywordID, EKeywordType keywordType, EMatchType matchType, string keywords, bool isDisabled);

        void Update(int publishmentSystemID, int keywordID, EKeywordType keywordType, EMatchType matchType, string keywords);

        void Delete(int keywordID);

        void Delete(List<int> keywordIDList);

        string GetKeywords(int keywordID);

        KeywordInfo GetKeywordInfo(int keywordID);

        int GetKeywordID(int publishmentSystemID, bool isExists, string keywords, EKeywordType keywordType, int existKeywordID);

        KeywordInfo GetAvaliableKeywordInfo(int publishmentSystemID, EKeywordType keywordType);

        IEnumerable GetDataSource(int publishmentSystemID, EKeywordType keywordType);

        int GetCount(int publishmentSystemID, EKeywordType keywordType);

        List<KeywordInfo> GetKeywordInfoList(int publishmentSystemID);

        List<KeywordInfo> GetKeywordInfoList(int publishmentSystemID, EKeywordType keywordType);

        bool UpdateTaxisToUp(int publishmentSystemID, EKeywordType keywordType, int keywordID);

        bool UpdateTaxisToDown(int publishmentSystemID, EKeywordType keywordType, int keywordID);

        int GetKeywordsIDbyName(int publishmentSystemID,string keywords);
	}
}
