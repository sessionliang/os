using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IKeywordMatchDAO
	{
        void Insert(KeywordMatchInfo matchInfo);

        void DeleteByKeywordID(int keywordID);

        List<string> GetKeywordList(int publishmentSystemID, EKeywordType keywordType);

        List<string> GetKeywordListEnabled(int publishmentSystemID);

        int GetKeywordIDByMPController(int publishmentSystemID, string keyword);

        string GetSelectString(int publishmentSystemID);

        string GetSelectString(int publishmentSystemID, string keywordType, string keyword);

        string GetSortField();

        bool IsExists(int publishmentSystemID, string keyword);

        List<KeywordMatchInfo> GetKeywordMatchInfoList(int publishmentSystemID,int keyWordID);
	}
}
