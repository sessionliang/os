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
    public class UrlDAO : DataProviderBase, IUrlDAO
	{
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.OuterConnectionString;
            }
        }

        private const string SQL_INSERT = "INSERT INTO brs_Url (GUID, Site, Version, IsBeta, DatabaseType, Dotnet, Domain, AddDate, LastActivityDate, CountOfActivity, IsExpire, ExpireDate, ExpireReason, Summary) VALUES (@GUID, @Site, @Version, @IsBeta, @DatabaseType, @Dotnet, @Domain, @AddDate, @LastActivityDate, @CountOfActivity, @IsExpire, @ExpireDate, @ExpireReason, @Summary)";

        public void Insert(UrlInfo info)
		{
            info.Domain = info.Domain.ToLower();
            info.Domain = StringUtils.ReplaceStartsWith(info.Domain, "http://", string.Empty);
            info.Domain = info.Domain.Replace("/siteserver", string.Empty);

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@GUID", EDataType.VarChar, 50, info.GUID),
                this.GetParameter("@Site", EDataType.Integer, info.Site),
				this.GetParameter("@Version", EDataType.VarChar, 50, info.Version),
                this.GetParameter("@IsBeta", EDataType.VarChar, 18, info.IsBeta.ToString()),
                this.GetParameter("@DatabaseType", EDataType.VarChar, 50, info.DatabaseType),
                this.GetParameter("@Dotnet", EDataType.VarChar, 50, info.Dotnet),
                this.GetParameter("@Domain", EDataType.VarChar, 50, info.Domain.ToLower()),
                this.GetParameter("@AddDate", EDataType.DateTime, info.AddDate),
                this.GetParameter("@LastActivityDate", EDataType.DateTime, info.LastActivityDate),
                this.GetParameter("@CountOfActivity", EDataType.Integer, info.CountOfActivity),
                this.GetParameter("@IsExpire", EDataType.VarChar, 18, info.IsExpire.ToString()),
                this.GetParameter("@ExpireDate", EDataType.DateTime, info.ExpireDate),
                this.GetParameter("@ExpireReason", EDataType.NVarChar, 255, info.ExpireReason),
                this.GetParameter("@Summary", EDataType.NText, info.Summary)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
		}

        public UrlInfo GetUrlInfo(int id)
		{
            UrlInfo info = null;

            string sqlString = "SELECT ID, GUID, Site, Version, IsBeta, DatabaseType, Dotnet, Domain, AddDate, LastActivityDate, CountOfActivity, IsExpire, ExpireDate, ExpireReason, Summary FROM brs_Url WHERE ID = " + id;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    info = new UrlInfo(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetString(3), TranslateUtils.ToBool(rdr.GetString(4)), rdr.GetString(5), rdr.GetString(6), rdr.GetString(7), rdr.GetDateTime(8), rdr.GetDateTime(9), rdr.GetInt32(10), TranslateUtils.ToBool(rdr.GetString(11)), rdr.GetDateTime(12), rdr.GetString(13), rdr.GetString(14));
                }
                rdr.Close();
            }

			return info;
		}

        public UrlInfo GetUrlInfo(string domain)
        {
            UrlInfo info = null;

            string sqlString = string.Format("SELECT ID, GUID, Site, Version, IsBeta, DatabaseType, Dotnet, Domain, AddDate, LastActivityDate, CountOfActivity, IsExpire, ExpireDate, ExpireReason, Summary FROM brs_Url WHERE Domain = '{0}'", domain);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    info = new UrlInfo(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetString(3), TranslateUtils.ToBool(rdr.GetString(4)), rdr.GetString(5), rdr.GetString(6), rdr.GetString(7), rdr.GetDateTime(8), rdr.GetDateTime(9), rdr.GetInt32(10), TranslateUtils.ToBool(rdr.GetString(11)), rdr.GetDateTime(12), rdr.GetString(13), rdr.GetString(14));
                }
                rdr.Close();
            }

            return info;
        }

        public bool IsExpire(string domain, out DateTime expireDate)
        {
            expireDate = DateTime.Now;
            bool isExpire = false;

            string sqlString = string.Format("SELECT IsExpire, ExpireDate FROM brs_Url WHERE Domain = '{0}'", domain);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    isExpire = TranslateUtils.ToBool(rdr.GetString(0));
                    expireDate = rdr.GetDateTime(1);

                    //超过三个月，不提示到期
                    if (DateTime.Now.AddMonths(3) < expireDate)
                    {
                        isExpire = false;
                    }
                }
                rdr.Close();
            }

            return isExpire;
        }

        public string GetSelectSqlString()
        {
            return "SELECT brs_Url.*, brs_License.ID AS LicenseID FROM brs_Url LEFT JOIN brs_License ON brs_Url.Domain = brs_License.Domain";
        }

        public string GetSelectSqlString(string domain, bool isLicense)
        {
            string sqlString = string.Empty;

            if (isLicense)
            {
                sqlString = "SELECT brs_Url.*, brs_License.ID AS LicenseID FROM brs_Url INNER JOIN brs_License ON brs_Url.Domain = brs_License.Domain";
            }
            else
            {
                sqlString = "SELECT brs_Url.*, brs_License.ID AS LicenseID FROM brs_Url LEFT JOIN brs_License ON brs_Url.Domain = brs_License.Domain";               
            }

            if (!string.IsNullOrEmpty(domain))
            {
                sqlString += string.Format(" WHERE brs_Url.Domain LIKE '%{0}%'", domain);
            }

            return sqlString;
        }

        public string GetSortFieldName()
        {
            return "LastActivityDate";
        }

        public int GetUrlID(string domain)
        {
            int urlID = 0;

            string sqlString = sqlString = string.Format("SELECT ID FROM brs_Url WHERE Domain = '{0}'", domain.ToLower());

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    urlID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return urlID;
        }

        public int GetUrlID(string guid, string domain, out DateTime lastActivityDate)
        {
            int urlID = 0;
            lastActivityDate = DateTime.MinValue;

            string sqlString = string.Empty;
            if (!string.IsNullOrEmpty(guid))
            {
                sqlString = string.Format("SELECT ID, LastActivityDate FROM brs_Url WHERE GUID = '{0}'", guid);
            }
            else
            {
                sqlString = string.Format("SELECT ID, LastActivityDate FROM brs_Url WHERE Domain = '{0}'", domain.ToLower());
            }            

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    urlID = rdr.GetInt32(0);
                    lastActivityDate = rdr.GetDateTime(1);
                }
                rdr.Close();
            }

            return urlID;
        }

        public void Update(string guid, int site, string version, bool isBeta, string databaseType, string dotnet, int id)
        {
            string sqlString = string.Format("UPDATE brs_Url SET GUID = '{0}', Site = {1}, Version = '{2}', IsBeta = '{3}', DatabaseType = '{4}', Dotnet = '{5}', LastActivityDate = getdate(), CountOfActivity = CountOfActivity + 1 WHERE ID = {6}", guid, site, version, isBeta, databaseType, dotnet, id);

            this.ExecuteNonQuery(sqlString);
        }

        public void Update(bool isExpire, DateTime expireDate, string expireReason, int id)
        {
            string sqlString = string.Format("UPDATE brs_Url SET IsExpire = '{0}', ExpireDate = '{1}', ExpireReason = '{2}' WHERE ID = {3}", isExpire, expireDate, expireReason, id);

            this.ExecuteNonQuery(sqlString);
        }

        public void Update(string summary, int id)
        {
            string sqlString = string.Format("UPDATE brs_Url SET Summary = '{0}' WHERE ID = {1}", summary, id);

            this.ExecuteNonQuery(sqlString);
        }

        public int GetTodayActiveCount()
        {
            string sqlString = string.Format("select count(*) from brs_Url WHERE (DATEDIFF([d], LastActivityDate, getdate()) = 0)");

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public int GetTodayNewCount()
        {
            string sqlString = string.Format("select count(*) from brs_Url WHERE (DATEDIFF([d], AddDate, getdate()) = 0)");

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public int GetTotalCount()
        {
            string sqlString = string.Format("select count(*) from brs_Url");

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

//        public Hashtable GetProductIDHashtable()
//        {
//            Hashtable hashtable = new Hashtable();

//            string sqlString = @"select ProductID, count(*) AS TotalNum from brs_Url
//GROUP BY ProductID
//ORDER BY ProductID";

//            using (IDataReader rdr = this.ExecuteReader(sqlString))
//            {
//                while (rdr.Read())
//                {
//                    hashtable[rdr.GetString(0)] = rdr.GetInt32(1);
//                }
//                rdr.Close();
//            }

//            return hashtable;
//        }

        public ArrayList GetVersionArrayList()
        {
            ArrayList arrayList = new ArrayList();

            string sqlString = @"select Version, IsBeta, count(*) from brs_Url
where Version <> ''
GROUP BY Version, IsBeta
ORDER BY Version, IsBeta";

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    string version = rdr.GetString(0);
                    bool isBeta = TranslateUtils.ToBool(rdr.GetString(1));
                    int count = rdr.GetInt32(2);
                    arrayList.Add(string.Format("{0}&{1}", ProductManager.GetFullVersion(version, isBeta), count));

                }
                rdr.Close();
            }

            return arrayList;
        }

        public ArrayList GetDatabaseArrayList()
        {
            ArrayList arrayList = new ArrayList();

            string sqlString = @"select DatabaseType, count(*) from brs_Url
where DatabaseType <> ''
GROUP BY DatabaseType
ORDER BY DatabaseType";

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arrayList.Add(string.Format("{0}&{1}", rdr.GetString(0), rdr.GetInt32(1)));

                }
                rdr.Close();
            }

            return arrayList;
        }

        public Hashtable GetCountOfActivityHashtable(int year, int month)
        {
            int people1 = 0;
            int people2 = 0;
            int people3 = 0;
            int people4 = 0;
            int people5 = 0;

            string sqlString = string.Format(@"select countOfActivity, COUNT(*) as website from 
(
  select COUNT(brs_UrlActivity.ID) as countOfActivity from brs_Url inner join brs_UrlActivity ON year(brs_Url.AddDate) = {0} AND month(brs_Url.AddDate) = {1} AND year(brs_UrlActivity.ActivityDate) = {0} AND month(brs_UrlActivity.ActivityDate) = {1} AND brs_UrlActivity.UrlID = brs_Url.ID Group By brs_Url.ID
) as t
group by t.countOfActivity", year, month);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int countOfActivity = rdr.GetInt32(0);
                    int website = rdr.GetInt32(1);

                    if (countOfActivity == 1)
                    {
                        people1 += website;
                    }
                    else if (countOfActivity <= 3)
                    {
                        people2 += website;
                    }
                    else if (countOfActivity <= 7)
                    {
                        people3 += website;
                    }
                    else if (countOfActivity > 7)
                    {
                        people4 += website;
                    }
                }
                rdr.Close();
            }

            sqlString = string.Format(@"select COUNT(*) from brs_Url inner join brs_License on year(brs_Url.AddDate) = {0} AND month(brs_Url.AddDate) = {1} AND brs_Url.Domain = brs_License.Domain", year, month);

            int count = BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
            people5 = count;

            Hashtable current = new Hashtable();
            current[0] = people1;
            current[1] = people2;
            current[2] = people3;
            current[3] = people4;
            current[4] = people5;

            return current;
        }
	}
}
