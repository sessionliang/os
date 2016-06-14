using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using SiteServer.BBS.Model;

namespace SiteServer.BBS
{
    public interface INavigationDAO
    {
        List<NavigationInfo> GetNavigations(int publishmentSystemID, ENavType navType);

        NavigationInfo GetNavigationInfo(int id);

        void Delete(int id);

        void Update(NavigationInfo info);

        void Insert(NavigationInfo info);

        string GetSqlString(int publishmentSystemID, ENavType navType);

        void UpdateTaxisToUp(int publishmentSystemID, int id, ENavType navType);

        void UpdateTaxisToDown(int publishmentSystemID, int id, ENavType navType);

        int GetMaxTaxis(int publishmentSystemID, ENavType navType);

        int GetMinTaxis(int publishmentSystemID, ENavType navType);

        void CreateDefaultNavigation(int publishmentSystemID);
    }
}