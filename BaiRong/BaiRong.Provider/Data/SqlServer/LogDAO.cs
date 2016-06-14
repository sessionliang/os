using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using System.Text;

namespace BaiRong.Provider.Data.SqlServer
{
    public class LogDAO : DataProviderBase, ILogDAO
    {
        private const string PARM_ID = "@ID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_IP_ADDRESS = "@IPAddress";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_ACTION = "@Action";
        private const string PARM_SUMMARY = "@Summary";

        public void Insert(LogInfo log)
        {
            if (!ConfigManager.IsSysAdministrator(log.UserName))
            {
                string sqlString = "INSERT INTO bairong_Log(UserName, IPAddress, AddDate, Action, Summary) VALUES (@UserName, @IPAddress, @AddDate, @Action, @Summary)";
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    sqlString = "INSERT INTO bairong_Log(ID, UserName, IPAddress, AddDate, Action, Summary) VALUES (bairong_Log_SEQ.NEXTVAL, @UserName, @IPAddress, @AddDate, @Action, @Summary)";
                }

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(LogDAO.PARM_USER_NAME, EDataType.VarChar, 50, log.UserName),
				    this.GetParameter(LogDAO.PARM_IP_ADDRESS, EDataType.VarChar, 50, log.IPAddress),
                    this.GetParameter(LogDAO.PARM_ADD_DATE, EDataType.DateTime, log.AddDate),
				    this.GetParameter(LogDAO.PARM_ACTION, EDataType.NVarChar, 255, log.Action),
				    this.GetParameter(LogDAO.PARM_SUMMARY, EDataType.NVarChar, 255, log.Summary)
			    };

                this.ExecuteNonQuery(sqlString, parms);
            }
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList != null || idArrayList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM bairong_Log WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int days, int counter)
        {
            string sqlString = string.Format("DELETE FROM bairong_Log WHERE 1=1 ");
            if (days > 0)
            {
                string sql1 = sqlString + string.Format(@" AND AddDate < DATEADD(DAY, -{0},GETDATE())", days);
                this.ExecuteNonQuery(sql1);
            }
            if (counter > 0)
            {
                string sql2 = sqlString + string.Format(@" AND ID IN(
SELECT ID from(
SELECT ID, ROW_NUMBER() OVER(ORDER BY AddDate DESC) as rowNum FROM bairong_Log) as t
WHERE t.rowNum > {0})", counter);
                this.ExecuteNonQuery(sql2);
            }
        }

        public void DeleteAll()
        {
            string sqlString = "DELETE FROM bairong_Log";

            this.ExecuteNonQuery(sqlString);
        }

        public int GetCount()
        {
            int count = 0;
            string sqlString = "SELECT Count(ID) FROM bairong_Log";

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
            return "SELECT ID, UserName, IPAddress, AddDate, Action, Summary FROM bairong_Log";
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

            return "SELECT ID, UserName, IPAddress, AddDate, Action, Summary FROM bairong_Log " + whereString.ToString();
        }

        public DateTime GetLastLoginDate(string userName)
        {
            DateTime retval = DateTime.MinValue;
            string sqlString = "SELECT AddDate FROM bairong_Log WHERE UserName = @UserName ORDER BY ID DESC";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(LogDAO.PARM_USER_NAME, EDataType.VarChar, 50, userName)
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

        public DateTime GetLastRemoveLogDate(string userName)
        {
            DateTime retval = DateTime.MinValue;
            string sqlString = "SELECT AddDate FROM bairong_Log WHERE UserName = @UserName AND Action = '清空数据库日志' ORDER BY ID DESC";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(LogDAO.PARM_USER_NAME, EDataType.VarChar, 50, userName)
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

        /// <summary>
        /// 统计管理员actionType的操作次数
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="xType"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public Hashtable GetAdminLoginHashtableByDate(DateTime dateFrom, DateTime dateTo, string xType, string actionType)
        {
            Hashtable hashtable = new Hashtable();
            if (string.IsNullOrEmpty(xType))
            {
                xType = EStatictisXTypeUtils.GetValue(EStatictisXType.Day);
            }

            StringBuilder builder = new StringBuilder();
            if (dateFrom > DateUtils.SqlMinValue)
            {
                builder.AppendFormat(" AND AddDate >= '{0}'", dateFrom.ToString());
            }
            if (dateTo != DateUtils.SqlMinValue)
            {
                builder.AppendFormat(" AND AddDate < '{0}'", dateTo.ToString());
            }

            string SQL_SELECT_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AddNum, AddYear, AddMonth, AddDay
FROM (SELECT DATEPART([year], AddDate) AS AddYear, DATEPART([Month], 
              AddDate) AS AddMonth, DATEPART([Day], AddDate) 
              AS AddDay
        FROM [dbo].[bairong_Log]
        WHERE (DATEDIFF([Day], AddDate, {0}) < 30)  {1}) 
      DERIVEDTBL
GROUP BY AddYear, AddMonth, AddDay
ORDER BY AddNum DESC
", SqlUtils.GetDefaultDateString(this.DataBaseType), builder);//添加日统计

            if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Month))
            {
                SQL_SELECT_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AddNum, AddYear, AddMonth
FROM (SELECT DATEPART([year], AddDate) AS AddYear, DATEPART([Month], 
              AddDate) AS AddMonth
        FROM [dbo].[bairong_Log]
        WHERE (DATEDIFF([Month], AddDate, {0}) < 12) {1}) 
      DERIVEDTBL
GROUP BY AddYear, AddMonth
ORDER BY AddNum DESC
", SqlUtils.GetDefaultDateString(this.DataBaseType), builder);//添加月统计
            }
            else if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Year))
            {
                SQL_SELECT_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AddNum, AddYear
FROM (SELECT DATEPART([year], AddDate) AS AddYear
        FROM [dbo].[bairong_Log]
        WHERE (DATEDIFF([Year], AddDate, {0}) < 10) {1}) 
      DERIVEDTBL
GROUP BY AddYear
ORDER BY AddNum DESC
", SqlUtils.GetDefaultDateString(this.DataBaseType), builder);//添加年统计
            }


            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TRACKING_DAY))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int accessNum = Convert.ToInt32(rdr[0]);
                        if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Day))
                        {
                            string year = rdr[1].ToString();
                            string month = rdr[2].ToString();
                            string day = rdr[3].ToString();
                            DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-{1}-{2}", year, month, day));
                            hashtable.Add(dateTime, accessNum);
                        }
                        else if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Month))
                        {
                            string year = rdr[1].ToString();
                            string month = rdr[2].ToString();

                            DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-{1}-1", year, month));
                            hashtable.Add(dateTime, accessNum);
                        }
                        else if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Year))
                        {
                            string year = rdr[1].ToString();
                            DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-1-1", year));
                            hashtable.Add(dateTime, accessNum);
                        }
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        /// <summary>
        /// 统计管理员actionType的操作次数
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="xType"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public Hashtable GetAdminLoginHashtableByName(DateTime dateFrom, DateTime dateTo, string actionType)
        {
            Hashtable hashtable = new Hashtable();

            StringBuilder builder = new StringBuilder();
            if (dateFrom > DateUtils.SqlMinValue)
            {
                builder.AppendFormat(" AND AddDate >= '{0}'", dateFrom.ToString());
            }
            if (dateTo != DateUtils.SqlMinValue)
            {
                builder.AppendFormat(" AND AddDate < '{0}'", dateTo.ToString());
            }

            string SQL_SELECT_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AddNum, UserName
FROM (SELECT DATEPART([year], AddDate) AS AddYear, DATEPART([Month], 
              AddDate) AS AddMonth, DATEPART([Day], AddDate) 
              AS AddDay, UserName
        FROM [dbo].[bairong_Log]
        WHERE (DATEDIFF([Day], AddDate, {0}) < 30)  {1}) 
      DERIVEDTBL
GROUP BY UserName
ORDER BY AddNum DESC
", SqlUtils.GetDefaultDateString(this.DataBaseType), builder);//添加日统计


            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TRACKING_DAY))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int accessNum = Convert.ToInt32(rdr[0]);
                        string userName = rdr[1].ToString();
                        hashtable.Add(userName, accessNum);

                    }
                }
                rdr.Close();
            }
            return hashtable;
        }
    }
}
