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

namespace BaiRong.Provider.Data.SqlServer
{
    public class CreateTaskDAO : DataProviderBase, ICreateTaskDAO
    {
        public int Insert(CreateTaskInfo info)
        {
            int contentID = 0;

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, CreateTaskInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //只保留10条数据
                        this.DeleteLast(trans);
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        contentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, CreateTaskInfo.TableName);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return contentID;
        }

        public void Update(CreateTaskInfo info)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, CreateTaskInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            CreateTaskInfo info = new CreateTaskInfo();
            if (deleteIDArrayList.Count > 0)
            {
                info = this.GetCreateTaskInfo(TranslateUtils.ToInt(deleteIDArrayList[0].ToString()));
            }

            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", CreateTaskInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int id)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", CreateTaskInfo.TableName, id);
            this.ExecuteNonQuery(sqlString);
        }

        public CreateTaskInfo GetCreateTaskInfo(int createTaskID)
        {
            CreateTaskInfo info = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", createTaskID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CreateTaskInfo.TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new CreateTaskInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public string GetSelectSqlString(int startNum, int totalNum, string whereString, string orderByString)
        {
            if (!string.IsNullOrEmpty(whereString) && !StringUtils.StartsWithIgnoreCase(whereString.Trim(), "AND "))
            {
                whereString = "AND " + whereString.Trim();
            }
            string sqlWhereString = string.Format("WHERE 1=1 AND {1}", true.ToString(), whereString);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CreateTaskInfo.TableName, startNum, totalNum, SqlUtils.Asterisk, sqlWhereString, orderByString);
        }

        public void GetQueuingTaskCount(int id, out int totalCount, out int currentCount, out int errorCount, out int queuingCount)
        {
            totalCount = currentCount = errorCount = queuingCount = 0;
            string sql = string.Format(@"select TotalCount,
		(ct.TotalCount -(
		 select  COUNT(*) from bairong_AjaxUrl where CreateTaskID = ct.ID
		)) as CurrentCount,
		ErrorCount,
		(
		 select COUNT(*) from bairong_CreateTask where AddDate < ct.AddDate and state <> '{1}' and state <> '{2}'
		) as QueuingCount
from bairong_CreateTask ct
where ID = {0}", id, ECreateTaskTypeUtils.GetValue(ECreateTaskType.Completed), ECreateTaskTypeUtils.GetValue(ECreateTaskType.Cancel));
            using (IDataReader reader = this.ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    totalCount = TranslateUtils.ToInt(reader[0].ToString());
                    currentCount = TranslateUtils.ToInt(reader[1].ToString());
                    errorCount = TranslateUtils.ToInt(reader[2].ToString());
                    queuingCount = TranslateUtils.ToInt(reader[3].ToString());
                }
                reader.Close();
            }
        }


        public void UpdateState(int createTaskID, ECreateTaskType eCreateTaskType)
        {
            string sql = string.Format("UPDATE bairong_CreateTask SET State = '{0}' WHERE ID={1}", ECreateTaskTypeUtils.GetValue(eCreateTaskType), createTaskID);
            this.ExecuteNonQuery(sql);
        }
        public void UpdateState(int createTaskID)
        {
            string sql = string.Format(@"UPDATE bairong_CreateTask SET State = 'Completed',
EndTime = getdate()
WHERE ID = {0} 
AND NOT EXISTS (SELECT 1 FROM bairong_AjaxUrl WHERE CreateTaskID = {0})
AND DATEDIFF(D,EndTime, '{1}') = 0", createTaskID, DateUtils.SqlMinValue.ToString());
            this.ExecuteNonQuery(sql);
        }
        public void UpdateState()
        {
            string sql = string.Format(@"UPDATE bairong_CreateTask SET State = 'Completed'
WHERE ID NOT IN(
        SELECT CreateTaskID FROM bairong_AjaxUrl GROUP BY CreateTaskID
)
AND State <> 'Completed' AND State <> 'Cancel' ");
            this.ExecuteNonQuery(sql);
        }

        public void UpdateStartTime(int createTaskID)
        {
            string sql = string.Format(@"UPDATE bairong_CreateTask SET StartTime = getdate(), State = '{2}' WHERE ID = {0} AND DATEDIFF(D,StartTime, '{1}') = 0 ", createTaskID, DateUtils.SqlMinValue.ToString(), ECreateTaskTypeUtils.GetValue(ECreateTaskType.Processing));
            this.ExecuteNonQuery(sql);
        }

        public void UpdateEndTime(int createTaskID)
        {
            string sql = string.Format(@"UPDATE bairong_CreateTask SET EndTime = getdate(), State = '{2}' WHERE ID = {0} AND  DATEDIFF(D,EndTime, '{1}') = 0", createTaskID, DateUtils.SqlMinValue.ToString(), ECreateTaskTypeUtils.GetValue(ECreateTaskType.Completed));
            this.ExecuteNonQuery(sql);
        }

        public ArrayList GetCreateTaskInfoArrayList(string where)
        {
            ArrayList arrayList = new ArrayList();
            CreateTaskInfo info = null;
            string SQL_WHERE = string.Format("WHERE 1=1 {0}", string.IsNullOrEmpty(where) ? string.Empty : ("AND " + where));
            string SQL_ORDER = string.Format(" ORDER BY  State DESC, AddDate DESC ");
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CreateTaskInfo.TableName, SqlUtils.Asterisk, SQL_WHERE, SQL_ORDER);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    info = new CreateTaskInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    if (info != null) info.AfterExecuteReader();
                    arrayList.Add(info);
                }
                rdr.Close();
            }
            return arrayList;
        }



        private void DeleteLast(IDbTransaction trans)
        {
            string SQL_SELECT = string.Format(@"delete from bairong_CreateTask
where ID not in
(
select top 10 ID from bairong_CreateTask order by AddDate desc
)
and (State = 'Completed' or State = 'Cancel') ");
            this.ExecuteNonQuery(trans, SQL_SELECT);
        }
    }
}
