using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Text;
using BaiRong.Model.Service;

namespace BaiRong.Provider.Data.SqlServer
{
    public class TaskLogDAO : DataProviderBase, ITaskLogDAO
    {
        private const string PARM_ID = "@ID";
        private const string PARM_TASK_ID = "@TaskID";
        private const string PARM_IS_SUCCESS = "@IsSuccess";
        private const string PARM_ERROR_MESSAGE = "@ErrorMessage";
        private const string PARM_STACK_TRACE = "@StackTrace";
        private const string PARM_SUBTASK_ERRORS = "@SubtaskErrors";
        private const string PARM_ADD_DATE = "@AddDate";

        public void Insert(TaskLogInfo log)
        {
            if (ConfigManager.Additional.IsLogTask)
            {
                string sqlString = "INSERT INTO bairong_TaskLog(TaskID, IsSuccess, ErrorMessage, StackTrace, SubtaskErrors, AddDate) VALUES (@TaskID, @IsSuccess, @ErrorMessage, @StackTrace, @SubtaskErrors, @AddDate)";
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    sqlString = "INSERT INTO bairong_TaskLog(ID, TaskID, IsSuccess, ErrorMessage, StackTrace, SubtaskErrors, AddDate) VALUES (bairong_TaskLog_SEQ.NEXTVAL, @TaskID, @IsSuccess, @ErrorMessage, @StackTrace, @SubtaskErrors, @AddDate)";
                }

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
                    this.GetParameter(PARM_TASK_ID, EDataType.Integer, log.TaskID),
                    this.GetParameter(PARM_IS_SUCCESS, EDataType.VarChar, 18, log.IsSuccess.ToString()),
                    this.GetParameter(PARM_ERROR_MESSAGE, EDataType.NVarChar, 255, log.ErrorMessage),
                    this.GetParameter(PARM_STACK_TRACE, EDataType.NText, log.StackTrace),
				    this.GetParameter(PARM_SUBTASK_ERRORS, EDataType.NText, log.SubtaskErrors),
				    this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, log.AddDate)
			    };

                this.ExecuteNonQuery(sqlString, parms);
            }
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList != null || idArrayList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM bairong_TaskLog WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAll()
        {
            string sqlString = "DELETE FROM bairong_TaskLog";

            this.ExecuteNonQuery(sqlString);
        }

        public string GetSelectCommend()
        {
            return "SELECT ID, TaskID, IsSuccess, ErrorMessage, StackTrace, SubtaskErrors, AddDate FROM bairong_TaskLog";
        }

        public string GetSelectCommend(ETriState successState, string keyword, string dateFrom, string dateTo)
        {
            StringBuilder whereString = new StringBuilder("WHERE ");

            if (successState != ETriState.All)
            {
                whereString.AppendFormat("IsSuccess = '{0}'", ETriStateUtils.GetValue(successState));
            }
            else
            {
                whereString.AppendFormat("1=1", ETriStateUtils.GetValue(successState));
            }            

            if (!string.IsNullOrEmpty(keyword))
            {
                whereString.Append(" AND ");
                whereString.AppendFormat("(ErrorMessage LIKE '%{0}%' OR StackTrace LIKE '%{0}%' OR SubtaskErrors LIKE '%{0}%')", PageUtils.FilterSql(keyword));
            }

            if (!string.IsNullOrEmpty(dateFrom))
            {
                whereString.Append(" AND ");
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
                whereString.Append(" AND ");
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereString.AppendFormat("(to_char(AddDate,'YYYY-MM-DD') <= '{0}')", dateTo);
                }
                else
                {
                    whereString.AppendFormat("(AddDate <= '{0}')", dateTo);
                }
            }

            return "SELECT ID, TaskID, IsSuccess, ErrorMessage, StackTrace, SubtaskErrors, AddDate FROM bairong_TaskLog " + whereString.ToString();
        }
    }
}
