using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core.Data.Provider;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core.Data;
using BaiRong.Model;
using BaiRong.Core;
using System;

namespace SiteServer.Project.Provider.Data.SqlServer
{
    public class UrlActivityDAO : DataProviderBase, IUrlActivityDAO
	{
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.OuterConnectionString;
            }
        }

        private const string SQL_INSERT = "INSERT INTO brs_UrlActivity (UrlID, ActivityDate) VALUES (@UrlID, @ActivityDate)";

        public void Insert(UrlActivityInfo info)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@UrlID", EDataType.Integer, info.UrlID),
                this.GetParameter("@ActivityDate", EDataType.DateTime, info.ActivityDate)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
		}
	}
}
