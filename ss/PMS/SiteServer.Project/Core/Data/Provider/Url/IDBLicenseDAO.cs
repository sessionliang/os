using System;
using System.Data;
using System.Collections;
using SiteServer.Project.Model;

namespace SiteServer.Project.Core
{
    public interface IDBLicenseDAO
    {
        void Insert(DBLicenseInfo info);

        void Update(DBLicenseInfo info);

        void Delete(int id);

        int GetLicenseID(string domain);

        int GetLicenseIDByAccountID(int accountID);

        int GetLicenseIDByOrderID(int orderID);

        int GetAccountID(int licenseID);

        int GetOrderID(int licenseID);

        string GetDomain(int licenseID);

        DBLicenseInfo GetLicenseInfo(int id);

        ArrayList GetLicenseInfoArrayList(string domain);

        string GetSelectSqlString();

        string GetSelectSqlString(string domain, string siteName, string clientName);

        string GetSortFieldName();
    }
}
