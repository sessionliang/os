using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IKeywordGroupDAO
	{
        int Insert(KeywordGroupInfo groupInfo);

        void Update(KeywordGroupInfo groupInfo);

        void Delete(int groupID);

        KeywordGroupInfo GetKeywordGroupInfo(int groupID);

        IEnumerable GetDataSource();

        int GetCount(int parentID);

        List<KeywordGroupInfo> GetKeywordGroupInfoList();

        bool UpdateTaxisToUp(int parentID, int groupID);

        bool UpdateTaxisToDown(int parentID, int groupID);
	}
}
