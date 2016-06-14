using System.Collections;
using SiteServer.CMS.Model;
using System.Collections.Generic;

namespace SiteServer.CMS.Core
{
    public interface ITeleplayDAO
    {
        void Insert(TeleplayInfo TeleplayInfo);

        void Update(TeleplayInfo TeleplayInfo);

        void Delete(int id);

        void Delete(List<int> idList);

        void Delete(int publishmentSystemID, int contentID);

        TeleplayInfo GetTeleplayInfo(int id);

        TeleplayInfo GetFirstTeleplayInfo(int publishmentSystemID, int contentID);

        int GetCount(int publishmentSystemID, int contentID);

        string GetSortFieldName();

        string GetSelectSqlString(int publishmentSystemID, int contentID);

        IEnumerable GetStlDataSource(int publishmentSystemID, int contentID, int startNum, int totalNum);

        List<int> GetTeleplayContentIDList(int publishmentSystemID, int contentID);

        List<TeleplayInfo> GetTeleplayInfoList(int publishmentSystemID, int contentID);

        bool UpdateTaxisToUp(int publishmentSystemID, int contentID, int id);

        bool UpdateTaxisToDown(int publishmentSystemID, int contentID, int id);
    }
}
