using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core.Data.Provider;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core.Data;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.Project.Provider.Data.SqlServer
{
    public class HotfixDownloadDAO : DataProviderBase, IHotfixDownloadDAO
    {
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.OuterConnectionString;
            }
        }

        private const string SQL_INSERT = "INSERT INTO brs_HotfixDownload (HotfixID, Version, IsBeta, Domain, DownloadDate) VALUES (@HotfixID, @Version, @IsBeta, @Domain, @DownloadDate)";

        public void Insert(HotfixDownloadInfo info)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@HotfixID", EDataType.Integer, info.HotfixID),
				this.GetParameter("@Version", EDataType.VarChar, 50, info.Version),
                this.GetParameter("@IsBeta", EDataType.VarChar, 18, info.IsBeta.ToString()),
                this.GetParameter("@Domain", EDataType.VarChar, 50, info.Domain.ToLower()),
                this.GetParameter("@DownloadDate", EDataType.DateTime, info.DownloadDate)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
		}

        public string GetSelectSqlString(int hotfixID)
        {
            return string.Format("SELECT ID, HotfixID, Version, IsBeta, Domain, DownloadDate FROM brs_HotfixDownload WHERE HotfixID = {0}", hotfixID);
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
