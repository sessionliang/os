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
    public class DBLicenseDAO : DataProviderBase, IDBLicenseDAO
	{
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.OuterConnectionString;
            }
        }

        private const string SQL_INSERT = "INSERT INTO brs_License (Domain, MacAddress, ProcessorID, ColumnSerialNumber, MaxSiteNumber, LicenseDate, SiteName, ClientName, AccountID, OrderID, ExpireDate, Summary) VALUES (@Domain, @MacAddress, @ProcessorID, @ColumnSerialNumber, @MaxSiteNumber, @LicenseDate, @SiteName, @ClientName, @AccountID, @OrderID, @ExpireDate, @Summary)";

        private const string SQL_UPDATE = "UPDATE brs_License SET Domain = @Domain, MacAddress = @MacAddress, ProcessorID = @ProcessorID, ColumnSerialNumber = @ColumnSerialNumber, MaxSiteNumber = @MaxSiteNumber, LicenseDate = @LicenseDate, SiteName = @SiteName, ClientName = @ClientName, AccountID = @AccountID, OrderID = @OrderID, ExpireDate = @ExpireDate, Summary = @Summary WHERE ID = @ID";

        private const string SQL_DELETE = "DELETE brs_License WHERE ID = @ID";

        private const string SQL_SELECT_ALL = "SELECT ID, Domain, MacAddress, ProcessorID, ColumnSerialNumber, MaxSiteNumber, LicenseDate, SiteName, ClientName, AccountID, OrderID, ExpireDate, Summary FROM brs_License";

        public void Insert(DBLicenseInfo info)
		{
            info.Domain = info.Domain.ToLower();
            info.Domain = StringUtils.ReplaceStartsWith(info.Domain, "http://", string.Empty);

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter("@Domain", EDataType.VarChar, 50, info.Domain),
                this.GetParameter("@MacAddress", EDataType.VarChar, 50, info.MacAddress),
                this.GetParameter("@ProcessorID", EDataType.VarChar, 50, info.ProcessorID),
                this.GetParameter("@ColumnSerialNumber", EDataType.VarChar, 50, info.ColumnSerialNumber),
                this.GetParameter("@MaxSiteNumber", EDataType.Integer, info.MaxSiteNumber),
                this.GetParameter("@LicenseDate", EDataType.DateTime, info.LicenseDate),
                this.GetParameter("@SiteName", EDataType.NVarChar, 50, info.SiteName),
                this.GetParameter("@ClientName", EDataType.NVarChar, 50, info.ClientName),
                this.GetParameter("@AccountID", EDataType.Integer, info.AccountID),
                this.GetParameter("@OrderID", EDataType.Integer, info.OrderID),
                this.GetParameter("@ExpireDate", EDataType.DateTime, info.ExpireDate),
                this.GetParameter("@Summary", EDataType.NVarChar, 255, info.Summary)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
		}

        public void Update(DBLicenseInfo info)
        {
            info.Domain = info.Domain.ToLower();
            info.Domain = StringUtils.ReplaceStartsWith(info.Domain, "http://", string.Empty);

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter("@Domain", EDataType.VarChar, 50, info.Domain),
                this.GetParameter("@MacAddress", EDataType.VarChar, 50, info.MacAddress),
                this.GetParameter("@ProcessorID", EDataType.VarChar, 50, info.ProcessorID),
                this.GetParameter("@ColumnSerialNumber", EDataType.VarChar, 50, info.ColumnSerialNumber),
                this.GetParameter("@MaxSiteNumber", EDataType.Integer, info.MaxSiteNumber),
                this.GetParameter("@LicenseDate", EDataType.DateTime, info.LicenseDate),
                this.GetParameter("@SiteName", EDataType.NVarChar, 50, info.SiteName),
                this.GetParameter("@ClientName", EDataType.NVarChar, 50, info.ClientName),
                this.GetParameter("@AccountID", EDataType.Integer, info.AccountID),
                this.GetParameter("@OrderID", EDataType.Integer, info.OrderID),
                this.GetParameter("@ExpireDate", EDataType.DateTime, info.ExpireDate),
                this.GetParameter("@Summary", EDataType.NVarChar, 255, info.Summary),
                this.GetParameter("@ID", EDataType.Integer, info.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int id)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@ID", EDataType.Integer, id)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
        }

        public string GetDomain(int licenseID)
        {
            string domain = string.Empty;
            if (licenseID > 0)
            {
                string sqlString = string.Format("SELECT Domain FROM brs_License WHERE ID = {0}", licenseID);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read())
                    {
                        domain = rdr.GetValue(0).ToString();
                    }
                    rdr.Close();
                }
            }
            return domain;
        }

        public int GetLicenseID(string domain)
        {
            int licenseID = 0;

            if (!domain.StartsWith("www"))
            {
                domain = PageUtils.GetMainDomain(domain);                
            }

            string sqlString = string.Format("SELECT ID FROM brs_License WHERE Domain = '{0}'", domain);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    licenseID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return licenseID;
        }

        public int GetLicenseIDByAccountID(int accountID)
        {
            int licenseID = 0;

            string sqlString = string.Format("SELECT ID FROM brs_License WHERE AccountID = {0}", accountID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    licenseID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return licenseID;
        }

        public int GetLicenseIDByOrderID(int orderID)
        {
            int licenseID = 0;

            string sqlString = string.Format("SELECT ID FROM brs_License WHERE OrderID = {0}", orderID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    licenseID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return licenseID;
        }

        public int GetAccountID(int licenseID)
        {
            int accountID = 0;

            string sqlString = string.Format("SELECT AccountID FROM brs_License WHERE ID = {0}", licenseID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    accountID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return accountID;
        }

        public int GetOrderID(int licenseID)
        {
            int orderID = 0;

            string sqlString = string.Format("SELECT OrderID FROM brs_License WHERE ID = {0}", licenseID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    orderID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return orderID;
        }

        public DBLicenseInfo GetLicenseInfo(int id)
		{
            DBLicenseInfo info = null;

            string sqlString = "SELECT ID, Domain, MacAddress, ProcessorID, ColumnSerialNumber, MaxSiteNumber, LicenseDate, SiteName, ClientName, AccountID, OrderID, ExpireDate, Summary FROM brs_License WHERE ID = " + id;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    info = new DBLicenseInfo(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetDateTime(6), rdr.GetString(7), rdr.GetString(8), rdr.GetInt32(9), rdr.GetInt32(10), rdr.GetDateTime(11), rdr.GetString(12));
                }
                rdr.Close();
            }

			return info;
		}

        public ArrayList GetLicenseInfoArrayList(string domain)
        {
            ArrayList arraylist = new ArrayList();

            if (!string.IsNullOrEmpty(domain))
            {
                domain = domain.ToLower().Trim().Replace("http://", string.Empty);

                if (domain == "www.siteserver.cn" || domain == "siteserver.cn")
                {
                    arraylist.Add(new DBLicenseInfo(0, "www.siteserver.cn", string.Empty, string.Empty, string.Empty, 0, new DateTime(2004, 1, 1), string.Empty, string.Empty, 0, 0, DateTime.Now, string.Empty));
                }
                else
                {
                    string sqlString = string.Format("SELECT ID, Domain, MacAddress, ProcessorID, ColumnSerialNumber, MaxSiteNumber, LicenseDate, SiteName, ClientName, AccountID, OrderID, ExpireDate, Summary FROM brs_License WHERE Domain = '{0}' OR Domain = 'www.{0}'", domain);
                    using (IDataReader rdr = this.ExecuteReader(sqlString))
                    {
                        while (rdr.Read())
                        {
                            DBLicenseInfo info = new DBLicenseInfo(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetDateTime(6), rdr.GetString(7), rdr.GetString(8), rdr.GetInt32(9), rdr.GetInt32(10), rdr.GetDateTime(11), rdr.GetString(12));
                            arraylist.Add(info);
                        }
                        rdr.Close();
                    }
                }
            }

            return arraylist;
        }

        public string GetSelectSqlString()
        {
            return SQL_SELECT_ALL;
        }

        public string GetSelectSqlString(string domain, string siteName, string clientName)
        {
            string sqlString = "SELECT ID, Domain, MacAddress, ProcessorID, ColumnSerialNumber, MaxSiteNumber, LicenseDate, SiteName, ClientName, AccountID, OrderID, ExpireDate, Summary FROM brs_License";

            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(domain))
            {
                whereString += string.Format(" Domain LIKE '%{0}%' ", domain);
            }
            if (!string.IsNullOrEmpty(siteName))
            {
                if (whereString.Length > 0)
                {
                    whereString += "AND";
                }
                whereString += string.Format(" SiteName LIKE '%{0}%' ", siteName);
            }
            if (!string.IsNullOrEmpty(clientName))
            {
                if (whereString.Length > 0)
                {
                    whereString += "AND";
                }
                whereString += string.Format(" ClientName LIKE '%{0}%' ", clientName);
            }
            if (whereString.Length > 0)
            {
                sqlString += " WHERE " + whereString;
            }

            return sqlString;
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
