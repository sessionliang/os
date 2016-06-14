using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class SiteserverThirdLoginDAO : DataProviderBase, ISiteserverThirdLoginDAO
    {
        private const string SQL_UPDATE_THIRDLOGIN = "UPDATE siteserver_ThirdLogin SET ThirdLoginName = @ThirdLoginName, IsEnabled = @IsEnabled, Description = @Description, SettingsXML = @SettingsXML WHERE ID = @ID";

        private const string SQL_DELETE_THIRDLOGIN = "DELETE FROM siteserver_ThirdLogin WHERE ID = @ID";

        private const string SQL_SELECT_THIRDLOGIN = "SELECT ID, PublishmentSystemID, ThirdLoginType, ThirdLoginName, IsEnabled, Taxis, Description, SettingsXML FROM siteserver_ThirdLogin WHERE ID = @ID";

        private const string SQL_SELECT_BY_NAME = "SELECT ID, PublishmentSystemID, ThirdLoginType, ThirdLoginName, IsEnabled, Taxis, Description, SettingsXML FROM siteserver_ThirdLogin WHERE PublishmentSystemID = @PublishmentSystemID AND ThirdLoginName = @ThirdLoginName";

        private const string SQL_SELECT_BY_TYPE = "SELECT ID, PublishmentSystemID, ThirdLoginType, ThirdLoginName, IsEnabled,  Taxis, Description, SettingsXML FROM siteserver_ThirdLogin WHERE PublishmentSystemID = @PublishmentSystemID AND ThirdLoginType = @ThirdLoginType";

        private const string SQL_SELECT_ALL_THIRDLOGIN = "SELECT ID, PublishmentSystemID, ThirdLoginType, ThirdLoginName, IsEnabled,  Taxis, Description, SettingsXML FROM siteserver_ThirdLogin WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis DESC";

        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_TYPE = "@ThirdLoginType";
        private const string PARM_NAME = "@ThirdLoginName";
        private const string PARM_IS_ENABLED = "@IsEnabled";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_SETTINGS_XML = "@SettingsXML";

        public int Insert(SiteserverThirdLoginInfo siteserverThirdLoginInfo)
        {
            int thirdLoginID = 0;

            string sqlString = "INSERT INTO siteserver_ThirdLogin (PublishmentSystemID, ThirdLoginType, ThirdLoginName, IsEnabled,  Taxis, Description, SettingsXML) VALUES (@PublishmentSystemID, @ThirdLoginType, @ThirdLoginName, @IsEnabled,@Taxis, @Description, @SettingsXML)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_ThirdLogin (ID, PublishmentSystemID, ThirdLoginType, ThirdLoginName, IsEnabled,  Taxis, Description, SettingsXML) VALUES (siteserver_ThirdLogin_SEQ.NEXTVAL, @PublishmentSystemID, @ThirdLoginType, @ThirdLoginName, @IsEnabled, @Taxis, @Description, @SettingsXML)";
            }

            int taxis = this.GetMaxTaxis(siteserverThirdLoginInfo.PublishmentSystemID) + 1;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, siteserverThirdLoginInfo.PublishmentSystemID),
				this.GetParameter(PARM_TYPE, EDataType.VarChar, 50, SiteserverEThirdLoginTypeUtils.GetValue(siteserverThirdLoginInfo.ThirdLoginType)),
                this.GetParameter(PARM_NAME, EDataType.NVarChar, 50, siteserverThirdLoginInfo.ThirdLoginName),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, siteserverThirdLoginInfo.IsEnabled.ToString()),                 
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NText, siteserverThirdLoginInfo.Description),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, siteserverThirdLoginInfo.SettingsXML)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        thirdLoginID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "siteserver_ThirdLogin");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return thirdLoginID;
        }

        public void Update(SiteserverThirdLoginInfo siteserverThirdLoginInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_NAME, EDataType.NVarChar, 50, siteserverThirdLoginInfo.ThirdLoginName),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, siteserverThirdLoginInfo.IsEnabled.ToString()),                
                this.GetParameter(PARM_DESCRIPTION, EDataType.NText, siteserverThirdLoginInfo.Description),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, siteserverThirdLoginInfo.SettingsXML),
				this.GetParameter(PARM_ID, EDataType.Integer, siteserverThirdLoginInfo.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_THIRDLOGIN, parms);
        }

        public void Delete(int thirdLoginID)
        {
            IDbDataParameter[] deleteParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, thirdLoginID)
			};

            this.ExecuteNonQuery(SQL_DELETE_THIRDLOGIN, deleteParms);
        }

        public SiteserverThirdLoginInfo GetSiteserverThirdLoginInfo(int thirdLoginID)
        {
            SiteserverThirdLoginInfo siteserverThirdLoginInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, thirdLoginID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_THIRDLOGIN, parms))
            {
                if (rdr.Read())
                {
                    siteserverThirdLoginInfo = new SiteserverThirdLoginInfo(rdr.GetInt32(0), rdr.GetInt32(1), SiteserverEThirdLoginTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetValue(3).ToString(), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString());
                }
                rdr.Close();
            }

            return siteserverThirdLoginInfo;
        }

        public bool IsExists(int publishmentSystemID, string thirdLoginName)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_NAME, EDataType.NVarChar, 50, thirdLoginName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_NAME, parms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public bool IsExists(int publishmentSystemID, SiteserverEThirdLoginType thirdLoginType)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TYPE, EDataType.VarChar, 50, SiteserverEThirdLoginTypeUtils.GetValue(thirdLoginType))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_TYPE, parms))
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
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_THIRDLOGIN, parms);
            return enumerable;
        }

        public List<SiteserverThirdLoginInfo> GetSiteserverThirdLoginInfoList(int publishmentSystemID)
        {
            List<SiteserverThirdLoginInfo> list = new List<SiteserverThirdLoginInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_THIRDLOGIN, parms))
            {
                while (rdr.Read())
                {
                    SiteserverThirdLoginInfo siteserverThirdLoginInfo = new SiteserverThirdLoginInfo(rdr.GetInt32(0), rdr.GetInt32(1), SiteserverEThirdLoginTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetValue(3).ToString(), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString());
                    list.Add(siteserverThirdLoginInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public bool UpdateTaxisToUp(int publishmentSystemID, int thirdLoginID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM siteserver_ThirdLogin WHERE Taxis > (SELECT Taxis FROM siteserver_ThirdLogin WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis", thirdLoginID, publishmentSystemID);
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = Convert.ToInt32(rdr[0]);
                    higherTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(thirdLoginID);

            if (higherID != 0)
            {
                SetTaxis(thirdLoginID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int publishmentSystemID, int thirdLoginID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM siteserver_ThirdLogin WHERE Taxis < (SELECT Taxis FROM siteserver_ThirdLogin WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis DESC", thirdLoginID, publishmentSystemID);
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = Convert.ToInt32(rdr[0]);
                    lowerTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(thirdLoginID);

            if (lowerID != 0)
            {
                SetTaxis(thirdLoginID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM siteserver_ThirdLogin WHERE PublishmentSystemID = {0}", publishmentSystemID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int thirdLoginID)
        {
            string sqlString = string.Format("SELECT Taxis FROM siteserver_ThirdLogin WHERE (ID = {0})", thirdLoginID);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    taxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int thirdLoginID, int taxis)
        {
            string sqlString = string.Format("UPDATE siteserver_ThirdLogin SET Taxis = {0} WHERE ID = {1}", taxis, thirdLoginID);
            this.ExecuteNonQuery(sqlString);
        }

        public void InsertUserBinding(int userID, string thirdLoginType, string thirdLoginUserID)
        {
            string sqlString = "INSERT INTO bairong_UserBinding (UserID, ThirdLoginType, ThirdLoginUserID) VALUES (@UserID, @ThirdLoginType, @ThirdLoginUserID)";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@UserID", EDataType.Integer,userID),
				this.GetParameter("@ThirdLoginType", EDataType.VarChar, 50, thirdLoginType),
                this.GetParameter("@ThirdLoginUserID", EDataType.NVarChar, 200, thirdLoginUserID),
 
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public int GetUserBindingCount(string ThirdLoginUserID)
        {
            string sqlString = string.Format("SELECT UserID FROM bairong_UserBinding WHERE (ThirdLoginUserID = '{0}')", ThirdLoginUserID);
            int bindingUserID = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    bindingUserID = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return bindingUserID;
        }
    }
}