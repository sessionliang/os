using System;
using System.Data;
using System.Collections;
using SiteServer.Project.Model;

namespace SiteServer.Project.Core
{
    public interface IHotfixDownloadDAO
    {
        void Insert(HotfixDownloadInfo info);

        string GetSelectSqlString(int hotfixID);

        string GetSortFieldName();
    }
}
