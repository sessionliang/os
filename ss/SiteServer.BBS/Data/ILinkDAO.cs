using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using SiteServer.BBS.Model;

namespace SiteServer.BBS
{
    public interface ILinkDAO
    {
        void Insert(LinkInfo info);

        void Update(LinkInfo info);

        void Delete(int id);

        LinkInfo GetLinksInfo(int id);

        List<LinkInfo> GetLinksList(int publishmentSystemID);

        string GetSelectCommend(int publishmentSystemID);

        string GetSqlString(int publishmentSystemID, bool isIcon);

        bool UpdateTaxisToUp(int publishmentSystemID, int id);

        bool UpdateTaxisToDown(int publishmentSystemID, int id);
    }
}