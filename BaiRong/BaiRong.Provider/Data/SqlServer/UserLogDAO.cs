using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using System.Text;
using System.Collections.Generic;

namespace BaiRong.Provider.Data.SqlServer
{
    public class UserLogDAO : DataProviderBase, IUserLogDAO
    {
        private const string PARM_ID = "@ID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_IP_ADDRESS = "@IPAddress";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_ACTION = "@Action";
        private const string PARM_SUMMARY = "@Summary";

        public void Insert(UserLogInfo userLog)
        {
            if (!ConfigManager.IsSysAdministrator(userLog.UserName))
            {
                string sqlString = "INSERT INTO bairong_UserLog(UserName, IPAddress, AddDate, Action, Summary) VALUES (@UserName, @IPAddress, @AddDate, @Action, @Summary)";
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    sqlString = "INSERT INTO bairong_UserLog(ID, UserName, IPAddress, AddDate, Action, Summary) VALUES (bairong_UserLog_SEQ.NEXTVAL, @UserName, @IPAddress, @AddDate, @Action, @Summary)";
                }

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(UserLogDAO.PARM_USER_NAME, EDataType.VarChar, 50, userLog.UserName),
				    this.GetParameter(UserLogDAO.PARM_IP_ADDRESS, EDataType.VarChar, 50, userLog.IPAddress),
                    this.GetParameter(UserLogDAO.PARM_ADD_DATE, EDataType.DateTime, userLog.AddDate),
				    this.GetParameter(UserLogDAO.PARM_ACTION, EDataType.NVarChar, 255, userLog.Action),
				    this.GetParameter(UserLogDAO.PARM_SUMMARY, EDataType.NVarChar, 255, userLog.Summary)
			    };

                this.ExecuteNonQuery(sqlString, parms);
            }
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList != null || idArrayList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM bairong_UserLog WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAll()
        {
            string sqlString = "DELETE FROM bairong_UserLog";

            this.ExecuteNonQuery(sqlString);
        }

        public int GetCount()
        {
            int count = 0;
            string sqlString = "SELECT Count(ID) FROM bairong_UserLog";

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        count = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }

            return count;
        }

        public int GetCount(string where)
        {
            int count = 0;
            string sqlString = "SELECT Count(ID) FROM bairong_UserLog";
            if (!string.IsNullOrEmpty(where))
                sqlString += " WHERE " + where;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        count = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }

            return count;
        }

        public string GetSelectCommend()
        {
            return "SELECT ID, UserName, IPAddress, AddDate, Action, Summary FROM bairong_UserLog";
        }

        public string GetSelectCommend(string userName, string keyword, string dateFrom, string dateTo)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
            {
                return this.GetSelectCommend();
            }

            StringBuilder whereString = new StringBuilder("WHERE ");

            bool isWhere = false;

            if (!string.IsNullOrEmpty(userName))
            {
                isWhere = true;
                whereString.AppendFormat("(UserName = '{0}')", PageUtils.FilterSql(userName));
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                if (isWhere)
                {
                    whereString.Append(" AND ");
                }
                isWhere = true;
                whereString.AppendFormat("(Action LIKE '%{0}%' OR Summary LIKE '%{0}%')", PageUtils.FilterSql(keyword));
            }

            if (!string.IsNullOrEmpty(dateFrom))
            {
                if (isWhere)
                {
                    whereString.Append(" AND ");
                }
                isWhere = true;
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereString.AppendFormat("(to_char(AddDate,'YYYY-MM-DD') >= '{0}')", PageUtils.FilterSql(dateFrom));
                }
                else
                {
                    whereString.AppendFormat("(AddDate >= '{0}')", PageUtils.FilterSql(dateFrom));
                }
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                if (isWhere)
                {
                    whereString.Append(" AND ");
                }
                isWhere = true;
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereString.AppendFormat("(to_char(AddDate,'YYYY-MM-DD') <= '{0}')", PageUtils.FilterSql(dateTo));
                }
                else
                {
                    whereString.AppendFormat("(AddDate <= '{0}')", PageUtils.FilterSql(dateTo));
                }
            }

            return "SELECT ID, UserName, IPAddress, AddDate, Action, Summary FROM bairong_UserLog " + whereString.ToString();
        }

        public DateTime GetLastUserLoginDate(string userName)
        {
            DateTime retval = DateTime.MinValue;
            string sqlString = "SELECT AddDate FROM bairong_UserLog WHERE UserName = @UserName ORDER BY ID DESC";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(UserLogDAO.PARM_USER_NAME, EDataType.VarChar, 50, userName)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                int i = 0;
                while (rdr.Read())
                {
                    i++;
                    if (i == 2)
                    {
                        retval = rdr.GetDateTime(0);
                        break;
                    }
                }
                rdr.Close();
            }
            return retval;
        }

        public DateTime GetLastRemoveUserLogDate(string userName)
        {
            DateTime retval = DateTime.MinValue;
            string sqlString = "SELECT AddDate FROM bairong_UserLog WHERE UserName = @UserName AND Action = '清空数据库日志' ORDER BY ID DESC";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(UserLogDAO.PARM_USER_NAME, EDataType.VarChar, 50, userName)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                while (rdr.Read())
                {

                    retval = rdr.GetDateTime(0);
                    break;
                }
                rdr.Close();
            }
            return retval;
        }

        public List<UserLogInfo> GetUserLoginLog(string userName)
        {
            try
            {
                List<UserLogInfo> list = new List<UserLogInfo>();
                string SQL_GETUSERLOGINLOG = "SELECT * FROM bairong_UserLog WHERE userName = @UserName and action = @Action";

                IDataParameter[] parms = new IDataParameter[] { 
                    this.GetParameter(PARM_USER_NAME,EDataType.VarChar,50,userName),
                    this.GetParameter(PARM_ACTION,EDataType.NVarChar,255,EUserActionTypeUtils.GetValue(EUserActionType.Login))
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_GETUSERLOGINLOG, parms))
                {
                    while (rdr.Read())
                    {
                        UserLogInfo info = new UserLogInfo(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetDateTime(3), rdr.GetString(4), rdr.GetString(5));
                        list.Add(info);
                    }
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UserLogInfo> GetUserLoginLogByPage(string userName, int pageIndex, int prePageNum)
        {
            try
            {
                List<UserLogInfo> list = new List<UserLogInfo>();
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(" SELECT tmp.* from ( ");
                sbSql.AppendFormat(" SELECT *, ROW_NUMBER() OVER(ORDER BY AddDate DESC) as rowNum FROM bairong_UserLog WHERE userName = @UserName and action = @Action ");
                sbSql.AppendFormat(" ) as tmp ");
                sbSql.AppendFormat(" WHERE tmp.rowNum >= {0} and tmp.rowNum <= {1} ", (pageIndex - 1) * prePageNum + 1, pageIndex * prePageNum);
                string SQL_GETUSERLOGINLOG = sbSql.ToString();

                IDataParameter[] parms = new IDataParameter[] { 
                    this.GetParameter(PARM_USER_NAME,EDataType.VarChar,50,userName),
                    this.GetParameter(PARM_ACTION,EDataType.NVarChar,255,EUserActionTypeUtils.GetValue(EUserActionType.Login))
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_GETUSERLOGINLOG, parms))
                {
                    while (rdr.Read())
                    {
                        UserLogInfo info = new UserLogInfo(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetDateTime(3), rdr.GetString(4), rdr.GetString(5));
                        list.Add(info);
                    }
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
