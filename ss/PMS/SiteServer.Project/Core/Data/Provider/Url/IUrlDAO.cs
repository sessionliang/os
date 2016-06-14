using System;
using System.Data;
using System.Collections;
using SiteServer.Project.Model;

namespace SiteServer.Project.Core
{
    public interface IUrlDAO
    {
        void Insert(UrlInfo info);

        void Update(string guid, int site, string version, bool isBeta, string databaseType, string dotnet, int id);

        void Update(bool isExpire, DateTime expireDate, string expireReason, int id);

        void Update(string summary, int id);

        UrlInfo GetUrlInfo(int id);

        UrlInfo GetUrlInfo(string domain);

        bool IsExpire(string domain, out DateTime expireDate);

        string GetSelectSqlString();

        string GetSelectSqlString(string domain, bool isLicense);

        string GetSortFieldName();

        int GetUrlID(string domain);

        int GetUrlID(string guid, string domain, out DateTime lastActivityDate);

        int GetTodayActiveCount();

        int GetTodayNewCount();

        int GetTotalCount();

        //Hashtable GetProductIDHashtable();

        ArrayList GetVersionArrayList();

        ArrayList GetDatabaseArrayList();

        Hashtable GetCountOfActivityHashtable(int year, int month);
    }
}
