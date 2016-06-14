using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IKeywordResourceDAO
    {
        int Insert(KeywordResourceInfo resourceInfo);

        void Update(KeywordResourceInfo resourceInfo);

        void Delete(int resourceID);

        KeywordResourceInfo GetResourceInfo(int resourceID);

        KeywordResourceInfo GetFirstResourceInfo(int keywordID);

        IEnumerable GetDataSource(int keywordID);

        int GetCount(int keywordID);

        List<KeywordResourceInfo> GetResourceInfoList(int keywordID);

        List<int> GetResourceIDList(int keywordID);

        bool UpdateTaxisToUp(int keywordID, int resourceID);

        bool UpdateTaxisToDown(int keywordID, int resourceID);

        List<KeywordResourceInfo> GetKeywordResourceInfoList(int publishmentSystemID, int keywordID);


    }
}
