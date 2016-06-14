using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.BBS.Model;
using System.Data;
using BaiRong.Model;
namespace SiteServer.BBS.Provider.SqlServer
{
    public class ReportDAO : DataProviderBase, IReportDAO
    {
        private string SQL_INSERT = "INSERT INTO bbs_Report(PublishmentSystemID, ForumID, ThreadID, PostID, UserName, IPAddress, AddDate, Content) VALUES (@PublishmentSystemID, @ForumID, @ThreadID, @PostID, @UserName, @IPAddress, @AddDate, @Content)";

        public void InsertWithReport(ReportInfo reportInfo)
        {
            IDbDataParameter[] insertParms = new IDbDataParameter[]
             {
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, reportInfo.PublishmentSystemID),
                this.GetParameter("@ForumID", EDataType.Integer, reportInfo.ForumID),
				this.GetParameter("@ThreadID", EDataType.Integer, 255, reportInfo.ThreadID),
				this.GetParameter("@PostID", EDataType.Integer, reportInfo.PostID),
				this.GetParameter("@UserName", EDataType.VarChar, 50, reportInfo.UserName),
				this.GetParameter("@IPAddress", EDataType.VarChar,50, reportInfo.IPAddress),
				this.GetParameter("@AddDate", EDataType.DateTime, reportInfo.AddDate),
                this.GetParameter("@Content", EDataType.NVarChar,255, reportInfo.Content),
             };
            this.ExecuteNonQuery(SQL_INSERT, insertParms);
        }

        public string GetSqlString(int publishmentSystemID)
        {
            return string.Format("SELECT ID, PublishmentSystemID, ForumID, ThreadID, PostID, UserName, IPAddress, AddDate, Content FROM bbs_Report WHERE PublishmentSystemID = {0} ORDER BY AddDate", publishmentSystemID);
        }

        public void DeleteReport(int reportID)
        {
            string sqlString = string.Format("DELETE FROM bbs_Report WHERE ID = {0}", reportID);
            this.ExecuteNonQuery(sqlString);
        }
    }
}
