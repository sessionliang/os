using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.BBS.Model;
namespace SiteServer.BBS
{
    public interface IReportDAO
    {
        void InsertWithReport(ReportInfo reportInfo);

        string GetSqlString(int publishmentSystemID);

        void DeleteReport(int reportID);
    }
    
}
