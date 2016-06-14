using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using SiteServer.BBS.Model;

namespace SiteServer.BBS
{
    public interface IAnnouncementDAO
    {
        IList<AnnouncementInfo> GetAnnouncements(int publishmentSystemID);

        AnnouncementInfo GetAnnouncementInfo(int id);

        void Delete(int id);

        void Update(AnnouncementInfo info);

        void Insert(AnnouncementInfo info);

        string GetSqlString(int publishmentSystemID);

        void UpdateTaxisToUp(int publishmentSystemID, int id);

        void UpdateTaxisToDown(int publishmentSystemID, int id);

        int GetMaxTaxis(int publishmentSystemID);

        int GetMinTaxis(int publishmentSystemID);
    }
}