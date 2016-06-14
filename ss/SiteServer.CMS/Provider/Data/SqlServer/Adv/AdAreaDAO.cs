using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Text;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class AdAreaDAO : DataProviderBase, IAdAreaDAO
    {
        private const string SQL_INSERT_ADAREA = "INSERT INTO siteserver_AdArea ( PublishmentSystemID,AdAreaName, Width, Height, Summary, IsEnabled, AddDate) VALUES (@PublishmentSystemID, @AdAreaName, @Width, @Height, @Summary, @IsEnabled,@AddDate)";

        private const string SQL_UPDATE_ADAREA = "UPDATE siteserver_AdArea SET AdAreaName = @AdAreaName, Width = @Width,Height=@Height, Summary = @Summary, IsEnabled = @IsEnabled, AddDate = @AddDate WHERE AdAreaID = @AdAreaID AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_DELETE_ADAREA = "DELETE FROM siteserver_AdArea WHERE AdAreaName = @AdAreaName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ADAREA_BYNAME = "SELECT AdAreaID, PublishmentSystemID,AdAreaName, Width, Height, Summary, IsEnabled, AddDate FROM siteserver_AdArea WHERE AdAreaName = @AdAreaName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ADAREA_BYID = "SELECT AdAreaID, PublishmentSystemID,AdAreaName, Width, Height, Summary, IsEnabled, AddDate FROM siteserver_AdArea WHERE AdAreaID = @AdAreaID AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ADAREA_NAME = "SELECT AdAreaName FROM siteserver_AdArea WHERE AdAreaName = @AdAreaName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ALL_ADAREA = "SELECT AdAreaID, PublishmentSystemID,AdAreaName, Width, Height, Summary, IsEnabled,AddDate FROM siteserver_AdArea WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY AddDate DESC";

        private const string PARM_ADAREA_ID = "@AdAreaID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_ADAREA_NAME = "@AdAreaName";
        private const string PARM_WIDTH = "@Width";
        private const string PARM_HIGHT = "@Height";
        private const string PARM_SUMMARY = "@Summary";
        private const string PARM_IS_ENABLED = "@IsEnabled";
        private const string PARM_ADD_DATE = "@AddDate";

        public void Insert(AdAreaInfo adAreaInfo)
        {
            IDbDataParameter[] adParms = new IDbDataParameter[]
			{ 
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, adAreaInfo.PublishmentSystemID),
			    this.GetParameter(PARM_ADAREA_NAME, EDataType.NVarChar,255, adAreaInfo.AdAreaName),
                this.GetParameter(PARM_WIDTH, EDataType.Integer, adAreaInfo.Width),
                this.GetParameter(PARM_HIGHT, EDataType.Integer, adAreaInfo.Height),
                this.GetParameter(PARM_SUMMARY, EDataType.NVarChar, 255, adAreaInfo.Summary),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar,18, adAreaInfo.IsEnabled.ToString()),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, adAreaInfo.AddDate)
              
			};

            this.ExecuteNonQuery(SQL_INSERT_ADAREA, adParms);
        }

        public void Update(AdAreaInfo adAreaInfo)
        {
            IDbDataParameter[] adParms = new IDbDataParameter[]
			{
			    this.GetParameter(PARM_ADAREA_NAME, EDataType.NVarChar,255, adAreaInfo.AdAreaName),
                this.GetParameter(PARM_WIDTH, EDataType.Integer, adAreaInfo.Width),
                this.GetParameter(PARM_HIGHT, EDataType.Integer, adAreaInfo.Height),
                this.GetParameter(PARM_SUMMARY, EDataType.Text, 255, adAreaInfo.Summary),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar,18, adAreaInfo.IsEnabled.ToString()),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, adAreaInfo.AddDate),
                this.GetParameter(PARM_ADAREA_ID, EDataType.Integer, adAreaInfo.AdAreaID),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, adAreaInfo.PublishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_ADAREA, adParms);
        }

        public void Delete(string adAreaName, int publishmentSystemID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
			    this.GetParameter(PARM_ADAREA_NAME, EDataType.NVarChar,255, adAreaName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};
            this.ExecuteNonQuery(SQL_DELETE_ADAREA, parms);

        }

        public AdAreaInfo GetAdAreaInfo(string adAreaName, int publishmentSystemID)
        {
            AdAreaInfo adAreaInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ADAREA_NAME, EDataType.NVarChar,255, adAreaName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ADAREA_BYNAME, parms))
            {
                if (rdr.Read())
                {
                    adAreaInfo = new AdAreaInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetValue(5).ToString(), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetDateTime(7));
                }
                rdr.Close();
            }

            return adAreaInfo;
        }

        public AdAreaInfo GetAdAreaInfo(int adAreaID, int publishmentSystemID)
        {
            AdAreaInfo adAreaInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ADAREA_ID, EDataType.Integer, adAreaID),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ADAREA_BYID, parms))
            {
                if (rdr.Read())
                {
                    adAreaInfo = new AdAreaInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetValue(5).ToString(), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetDateTime(7));
                }
                rdr.Close();
            }

            return adAreaInfo;
        }

        public bool IsExists(string adAreaName, int publishmentSystemID)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ADAREA_NAME, EDataType.NVarChar,255, adAreaName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ADAREA_NAME, parms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public IEnumerable GetDataSource(int publishmentSystemID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_ADAREA, parms);
            return enumerable;
        }

        public IEnumerable GetDataSourceByName(string adAreaName, int publishmentSystemID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT AdAreaID, PublishmentSystemID,AdAreaName, Width, Height, Summary, IsEnabled,AddDate FROM siteserver_AdArea WHERE PublishmentSystemID ={0}", publishmentSystemID);
            if (!string.IsNullOrEmpty(adAreaName))
            {
                strSql.AppendFormat(" AND AdAreaName LIKE '%{0}%'", PageUtils.FilterSql(adAreaName));
            }
            strSql.Append("ORDER BY AddDate DESC");

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(strSql.ToString());

            return enumerable;
        }

        public ArrayList GetAdAreaNameArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT AdAreaName FROM siteserver_AdArea WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    string adAreaName = rdr.GetValue(0).ToString();
                    arraylist.Add(adAreaName);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetAdAreaInfoArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_ADAREA, parms))
            {
                while (rdr.Read())
                {
                    AdAreaInfo adAreaInfo = new AdAreaInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetValue(5).ToString(), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetDateTime(7));
                    arraylist.Add(adAreaInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }
    }
}
