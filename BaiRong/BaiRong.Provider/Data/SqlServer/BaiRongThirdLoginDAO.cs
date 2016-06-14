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

namespace BaiRong.Provider.Data.SqlServer
{
    public class BaiRongThirdLoginDAO : DataProviderBase, IBaiRongThirdLoginDAO
    {
        private const string SQL_UPDATE_THIRDLOGIN = "UPDATE bairong_ThirdLogin SET ThirdLoginName = @ThirdLoginName, IsEnabled = @IsEnabled, Description = @Description, SettingsXML = @SettingsXML WHERE ID = @ID";

        private const string SQL_DELETE_THIRDLOGIN = "DELETE FROM bairong_ThirdLogin WHERE ID = @ID";

        private const string SQL_SELECT_THIRDLOGIN = "SELECT ID, ThirdLoginType, ThirdLoginName, IsEnabled, Taxis, Description, SettingsXML FROM bairong_ThirdLogin WHERE ID = @ID";

        private const string SQL_SELECT_BY_NAME = "SELECT ID,  ThirdLoginType, ThirdLoginName, IsEnabled, Taxis, Description, SettingsXML FROM bairong_ThirdLogin WHERE ThirdLoginName = @ThirdLoginName";

        private const string SQL_SELECT_BY_TYPE = "SELECT ID, ThirdLoginType, ThirdLoginName, IsEnabled,  Taxis, Description, SettingsXML FROM bairong_ThirdLogin WHERE ThirdLoginType = @ThirdLoginType";

        private const string SQL_SELECT_ALL_THIRDLOGIN = "SELECT ID, ThirdLoginType, ThirdLoginName, IsEnabled,  Taxis, Description, SettingsXML FROM bairong_ThirdLogin ORDER BY Taxis DESC";

        private const string PARM_ID = "@ID";
        private const string PARM_TYPE = "@ThirdLoginType";
        private const string PARM_NAME = "@ThirdLoginName";
        private const string PARM_IS_ENABLED = "@IsEnabled";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_SETTINGS_XML = "@SettingsXML";

        public int Insert(BaiRongThirdLoginInfo siteserverThirdLoginInfo)
        {
            int thirdLoginID = 0;

            string sqlString = "INSERT INTO bairong_ThirdLogin (ThirdLoginType, ThirdLoginName, IsEnabled,  Taxis, Description, SettingsXML) VALUES ( @ThirdLoginType, @ThirdLoginName, @IsEnabled,@Taxis, @Description, @SettingsXML)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_ThirdLogin (ID, ThirdLoginType, ThirdLoginName, IsEnabled,  Taxis, Description, SettingsXML) VALUES (bairong_ThirdLogin_SEQ.NEXTVAL, @ThirdLoginType, @ThirdLoginName, @IsEnabled, @Taxis, @Description, @SettingsXML)";
            }

            int taxis = this.GetMaxTaxis() + 1;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TYPE, EDataType.VarChar, 50, ESiteserverThirdLoginTypeUtils.GetValue(siteserverThirdLoginInfo.ThirdLoginType)),
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
                        thirdLoginID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bairong_ThirdLogin");
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

        public void Update(BaiRongThirdLoginInfo siteserverThirdLoginInfo)
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

        public BaiRongThirdLoginInfo GetSiteserverThirdLoginInfo(int thirdLoginID)
        {
            BaiRongThirdLoginInfo siteserverThirdLoginInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, thirdLoginID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_THIRDLOGIN, parms))
            {
                if (rdr.Read())
                {
                    siteserverThirdLoginInfo = new BaiRongThirdLoginInfo(rdr.GetInt32(0), ESiteserverThirdLoginTypeUtils.GetEnumType(rdr.GetValue(1).ToString()), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString());
                }
                rdr.Close();
            }

            return siteserverThirdLoginInfo;
        }

        public bool IsExists(string thirdLoginName)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
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

        public bool IsExists(ESiteserverThirdLoginType thirdLoginType)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TYPE, EDataType.VarChar, 50, ESiteserverThirdLoginTypeUtils.GetValue(thirdLoginType))
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

        public IEnumerable GetDataSource()
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{

			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_THIRDLOGIN, parms);
            return enumerable;
        }

        public List<BaiRongThirdLoginInfo> GetSiteserverThirdLoginInfoList()
        {
            List<BaiRongThirdLoginInfo> list = new List<BaiRongThirdLoginInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_THIRDLOGIN, parms))
            {
                while (rdr.Read())
                {
                    BaiRongThirdLoginInfo siteserverThirdLoginInfo = new BaiRongThirdLoginInfo(rdr.GetInt32(0), ESiteserverThirdLoginTypeUtils.GetEnumType(rdr.GetValue(1).ToString()), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString());
                    list.Add(siteserverThirdLoginInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public bool UpdateTaxisToUp(int thirdLoginID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bairong_ThirdLogin WHERE Taxis > (SELECT Taxis FROM bairong_ThirdLogin WHERE ID = {0}) ORDER BY Taxis", thirdLoginID);
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

        public bool UpdateTaxisToDown(int thirdLoginID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bairong_ThirdLogin WHERE Taxis < (SELECT Taxis FROM bairong_ThirdLogin WHERE ID = {0}) ORDER BY Taxis DESC", thirdLoginID);
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

        private int GetMaxTaxis()
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM bairong_ThirdLogin");
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int thirdLoginID)
        {
            string sqlString = string.Format("SELECT Taxis FROM bairong_ThirdLogin WHERE (ID = {0})", thirdLoginID);
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
            string sqlString = string.Format("UPDATE bairong_ThirdLogin SET Taxis = {0} WHERE ID = {1}", taxis, thirdLoginID);
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

        public void SetDefaultThirdLogin()
        {
            this.ExecuteNonQuery("DELETE FROM bairong_ThirdLogin;");
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("INSERT INTO bairong_ThirdLogin (ThirdLoginType, ThirdLoginName, IsEnabled,  Taxis, Description, SettingsXML) VALUES ( '{0}', '{1}', 'False', 1, '{2}', '');", ESiteserverThirdLoginType.QQ.ToString(), ESiteserverThirdLoginTypeUtils.GetText(ESiteserverThirdLoginType.QQ), ESiteserverThirdLoginTypeUtils.GetDescription(ESiteserverThirdLoginType.QQ));
            sbSql.AppendFormat("INSERT INTO bairong_ThirdLogin (ThirdLoginType, ThirdLoginName, IsEnabled,  Taxis, Description, SettingsXML) VALUES ( '{0}', '{1}', 'False', 2, '{2}', '');", ESiteserverThirdLoginType.Weibo.ToString(), ESiteserverThirdLoginTypeUtils.GetText(ESiteserverThirdLoginType.Weibo), ESiteserverThirdLoginTypeUtils.GetDescription(ESiteserverThirdLoginType.Weibo));
            sbSql.AppendFormat("INSERT INTO bairong_ThirdLogin (ThirdLoginType, ThirdLoginName, IsEnabled,  Taxis, Description, SettingsXML) VALUES ( '{0}', '{1}', 'False', 3, '{2}', '');", ESiteserverThirdLoginType.WeixinPC.ToString(), ESiteserverThirdLoginTypeUtils.GetText(ESiteserverThirdLoginType.WeixinPC), ESiteserverThirdLoginTypeUtils.GetDescription(ESiteserverThirdLoginType.WeixinPC));
            this.ExecuteNonQuery(sbSql.ToString());
        }
    }
}