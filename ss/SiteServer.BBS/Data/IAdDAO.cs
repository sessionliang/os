using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using System.Collections;

namespace SiteServer.BBS
{
    public interface IAdDAO
    {
        void Insert(int publishmentSystemID, AdInfo adInfo);

        void Update(int publishmentSystemID, AdInfo adInfo);

        void Delete(int publishmentSystemID, int id);

        AdInfo GetAdInfo(int id);

        ArrayList GetAdInfoArrayList(int publishmentSystemID);

        bool IsExists(int publishmentSystemID, string adName);

        int GetCount(int publishmentSystemID, EAdLocation adLocation);

        IEnumerable GetDataSource(int publishmentSystemID);

        IEnumerable GetDataSource(int publishmentSystemID, EAdLocation adLocation);

        ArrayList GetAdNameArrayList(int publishmentSystemID);
    }
}