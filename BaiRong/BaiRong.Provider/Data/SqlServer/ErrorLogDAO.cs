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
    public class ErrorLogDAO : DataProviderBase, IErrorLogDAO
    {
        private const string PARM_ID = "@ID";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_MESSAGE = "@Message";
        private const string PARM_STACKTRACE = "@Stacktrace";
        private const string PARM_SUMMARY = "@Summary";

        public void Insert(ErrorLogInfo logInfo)
        {
            string sqlString = "INSERT INTO bairong_ErrorLog(AddDate, Message, Stacktrace, Summary) VALUES (@AddDate, @Message, @Stacktrace, @Summary)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_ErrorLog(ID, AddDate, Message, Stacktrace, Summary) VALUES (bairong_ErrorLog_SEQ.NEXTVAL, @AddDate, @Message, @Stacktrace, @Summary)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(ErrorLogDAO.PARM_ADD_DATE, EDataType.DateTime, logInfo.AddDate),
                this.GetParameter(ErrorLogDAO.PARM_MESSAGE, EDataType.NVarChar, 255, logInfo.Message),
                this.GetParameter(ErrorLogDAO.PARM_STACKTRACE, EDataType.NText, logInfo.Stacktrace),
                this.GetParameter(ErrorLogDAO.PARM_SUMMARY, EDataType.NText, logInfo.Summary)
            };

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList != null || idArrayList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM bairong_ErrorLog WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int days, int counter)
        {
            string sqlString = string.Format("DELETE FROM bairong_ErrorLog WHERE 1=1 ");
            if (days > 0)
            {
                string sql1 = sqlString + string.Format(@" AND AddDate < DATEADD(DAY, -{0},GETDATE())", days);
                this.ExecuteNonQuery(sql1);
            }
            if (counter > 0)
            {
                string sql2 = sqlString + string.Format(@" AND ID IN(
SELECT ID from(
SELECT ID, ROW_NUMBER() OVER(ORDER BY AddDate DESC) as rowNum FROM bairong_ErrorLog) as t
WHERE t.rowNum > {0})", counter);
                this.ExecuteNonQuery(sql2);
            }
        }

        public void DeleteAll()
        {
            string sqlString = "DELETE FROM bairong_ErrorLog";

            this.ExecuteNonQuery(sqlString);
        }

        public int GetCount()
        {
            int count = 0;
            string sqlString = "SELECT Count(ID) FROM bairong_ErrorLog";

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
            return "SELECT ID, AddDate, Message, Stacktrace, Summary FROM bairong_ErrorLog";
        }

        public string GetSelectCommend(string keyword, string dateFrom, string dateTo)
        {
            if (string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
            {
                return this.GetSelectCommend();
            }

            StringBuilder whereString = new StringBuilder("WHERE ");

            bool isWhere = false;

            if (!string.IsNullOrEmpty(keyword))
            {
                isWhere = true;
                whereString.AppendFormat("(Message LIKE '%{0}%' OR Stacktrace LIKE '%{0}%' OR Summary LIKE '%{0}%')", PageUtils.FilterSql(keyword));
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
                    whereString.AppendFormat("(to_char(AddDate,'YYYY-MM-DD') >= '{0}')", dateFrom);
                }
                else
                {
                    whereString.AppendFormat("(AddDate >= '{0}')", dateFrom);
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
                    whereString.AppendFormat("(to_char(AddDate,'YYYY-MM-DD') <= '{0}')", dateTo);
                }
                else
                {
                    whereString.AppendFormat("(AddDate <= '{0}')", dateTo);
                }
            }

            return "SELECT ID, AddDate, Message, Stacktrace, Summary FROM bairong_ErrorLog " + whereString.ToString();
        }
    }
}
