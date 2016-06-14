using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.B2C.Model;
using SiteServer.B2C.Core;

using System.Collections.Generic;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class UserSettingDAO : DataProviderBase, IUserSettingDAO
    {
        private const string TABLE_NAME = "b2c_UserSetting";

        public const string PARM_USER_NAME = "@UserName";

        private void UpdateUserNameBySessionID(string userName, string sessionID)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = @UserName WHERE {1} <> @UserName AND {2} = '{3}'", TABLE_NAME, UserSettingAttribute.UserName, UserSettingAttribute.SessionID, sessionID);

            IDbDataParameter[] updateParms = new IDbDataParameter[]
                {                    
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)	                    　
                };

            this.ExecuteNonQuery(sqlString, updateParms);
        }

        public UserSettingInfo GetSettingInfo(string userName, string sessionID)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                UserSettingInfo settingInfo = new UserSettingInfo { UserName = userName };

                string SQL_WHERE = string.Format("WHERE {0} = @UserName", UserSettingAttribute.UserName);
                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);
                IDbDataParameter[] selectParms = new IDbDataParameter[]
                {                    
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)	                    　
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, selectParms))
                {
                    if (rdr.Read())
                    {
                        settingInfo = new UserSettingInfo(rdr);
                    }
                    rdr.Close();
                }

                return settingInfo;
            }
            else
            {
                UserSettingInfo settingInfo = new UserSettingInfo { SessionID = sessionID };

                string SQL_WHERE = string.Format("WHERE {0} = '{1}'", UserSettingAttribute.SessionID, sessionID);
                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                {
                    if (rdr.Read())
                    {
                        settingInfo = new UserSettingInfo(rdr);
                    }
                    rdr.Close();
                }

                return settingInfo;
            }
        }

        public void Update(UserSettingInfo settingInfo)
        {
            if (settingInfo.ID == 0)
            {
                IDbDataParameter[] parms = null;
                string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(settingInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

                this.ExecuteNonQuery(SQL_INSERT, parms);
            }
            else
            {
                IDbDataParameter[] parms = null;
                string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(settingInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

                this.ExecuteNonQuery(SQL_UPDATE, parms);
            }
        }
    }
}
