using System;
using System.Data;
using System.Collections;
using SiteServer.Project.Model;

namespace SiteServer.Project.Core
{
    public interface IApplicationDAO
    {
        void Insert(ApplicationInfo info);

        void Delete(int id);

        void Handle(int id, string handleSummary);

        ApplicationInfo GetApplicationInfo(int id);

        string GetSelectSqlStringNotHandled();

        string GetSelectSqlStringNotHandled(string applicationType, string applyResource, string keyword);

        string GetSelectSqlStringHandled();

        string GetSelectSqlStringHandled(string applicationType, string applyResource, string keyword);

        string GetSortFieldName();

        string GetSortFieldNameOfHandled();

        int GetTotalCount();
    }
}
