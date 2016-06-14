using System;
using System.Collections;
using System.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using BaiRong.Core;
using BaiRong.Model.Service;
using BaiRong.Core.Service;

namespace BaiRong.Provider.Data.SqlServer
{
    public class TaskDAO : DataProviderBase, ITaskDAO
    {
        private const string SQL_SELECT_BY_ID = "SELECT TaskID, TaskName, ProductID, IsSystemTask, PublishmentSystemID, ServiceType, ServiceParameters, FrequencyType, PeriodIntervalMinute, StartDay, StartWeekday, StartHour, IsEnabled, AddDate, LastExecuteDate, Description, OnlyOnceDate  FROM bairong_Task WHERE TaskID = @TaskID";

        private const string SQL_SELECT_BY_NAME = "SELECT TaskID, TaskName, ProductID, IsSystemTask, PublishmentSystemID, ServiceType, ServiceParameters, FrequencyType, PeriodIntervalMinute, StartDay, StartWeekday, StartHour, IsEnabled, AddDate, LastExecuteDate, Description, OnlyOnceDate FROM bairong_Task WHERE TaskName = @TaskName";

        private const string SQL_UPDATE_TASK = "UPDATE bairong_Task SET TaskName = @TaskName, ProductID = @ProductID, IsSystemTask = @IsSystemTask, PublishmentSystemID = @PublishmentSystemID, ServiceType = @ServiceType, ServiceParameters = @ServiceParameters, FrequencyType = @FrequencyType, PeriodIntervalMinute = @PeriodIntervalMinute, StartDay = @StartDay, StartWeekday = @StartWeekday, StartHour = @StartHour, IsEnabled = @IsEnabled, AddDate = @AddDate, LastExecuteDate = @LastExecuteDate, Description = @Description, OnlyOnceDate = @OnlyOnceDate WHERE TaskID = @TaskID";

        private const string SQL_UPDATE_TASK_STATE = "UPDATE bairong_Task SET IsEnabled = @IsEnabled WHERE TaskID = @TaskID";

        private const string SQL_UPDATE_TASK_LAST_EXECUTE_DATE = "UPDATE bairong_Task SET LastExecuteDate = @LastExecuteDate WHERE TaskID = @TaskID";

        private const string SQL_DELETE = "DELETE FROM bairong_Task WHERE TaskID = @TaskID";

        private const string PARM_TASK_ID = "@TaskID";
        private const string PARM_TASK_NAME = "@TaskName";
        private const string PARM_PRODUCT_ID = "@ProductID";
        private const string PARM_IS_SYSTEM_TASK = "@IsSystemTask";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_SERVICE_TYPE = "@ServiceType";
        private const string PARM_SERVICE_PARAMETERS = "@ServiceParameters";
        private const string PARM_FREQUENCY_TYPE = "@FrequencyType";
        private const string PARM_PERIOD_INTERVAL_MINUTE = "@PeriodIntervalMinute";
        private const string PARM_START_DAY = "@StartDay";
        private const string PARM_START_WEEKDAY = "@StartWeekday";
        private const string PARM_START_HOUR = "@StartHour";
        private const string PARM_IS_ENABLED = "@IsEnabled";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_LAST_EXECUTE_DATE = "@LastExecuteDate";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_ONLY_ONCE_DATE = "@OnlyOnceDate";

        public int Insert(TaskInfo info)
        {
            int id = 0;
            string sqlString = "INSERT INTO bairong_Task (TaskName, ProductID, IsSystemTask, PublishmentSystemID, ServiceType, ServiceParameters, FrequencyType, PeriodIntervalMinute, StartDay, StartWeekday, StartHour, IsEnabled, AddDate, LastExecuteDate, Description, OnlyOnceDate) output @@identity VALUES (@TaskName, @ProductID, @IsSystemTask, @PublishmentSystemID, @ServiceType, @ServiceParameters, @FrequencyType, @PeriodIntervalMinute, @StartDay, @StartWeekday, @StartHour, @IsEnabled, @AddDate, @LastExecuteDate, @Description, @OnlyOnceDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_Task(TaskID, TaskName, ProductID, IsSystemTask, PublishmentSystemID, ServiceType, ServiceParameters, FrequencyType, PeriodIntervalMinute, StartDay, StartWeekday, StartHour, IsEnabled, AddDate, LastExecuteDate, Description, OnlyOnceDate) VALUES (bairong_Task_SEQ.NEXTVAL, @TaskName, @ProductID, @IsSystemTask, @PublishmentSystemID, @ServiceType, @ServiceParameters, @FrequencyType, @PeriodIntervalMinute, @StartDay, @StartWeekday, @StartHour, @IsEnabled, @AddDate, @LastExecuteDate, @Description, @OnlyOnceDate)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_TASK_NAME, EDataType.NVarChar, 50, info.TaskName),
                this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, info.ProductID),
                this.GetParameter(PARM_IS_SYSTEM_TASK, EDataType.VarChar, 18, info.IsSystemTask.ToString()),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter(PARM_SERVICE_TYPE, EDataType.VarChar, 50, EServiceTypeUtils.GetValue(info.ServiceType)),
				this.GetParameter(PARM_SERVICE_PARAMETERS, EDataType.NText, info.ServiceParameters),
				this.GetParameter(PARM_FREQUENCY_TYPE, EDataType.VarChar, 50, EFrequencyTypeUtils.GetValue(info.FrequencyType)),
				this.GetParameter(PARM_PERIOD_INTERVAL_MINUTE, EDataType.Integer, info.PeriodIntervalMinute),
                this.GetParameter(PARM_START_DAY, EDataType.Integer, info.StartDay),
                this.GetParameter(PARM_START_WEEKDAY, EDataType.Integer, info.StartWeekday),
                this.GetParameter(PARM_START_HOUR, EDataType.Integer, info.StartHour),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, info.IsEnabled.ToString()),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, info.AddDate),
                this.GetParameter(PARM_LAST_EXECUTE_DATE, EDataType.DateTime, info.LastExecuteDate),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, info.Description),
                this.GetParameter(PARM_ONLY_ONCE_DATE,EDataType.DateTime,info.OnlyOnceDate)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        id = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bairong_Task");
                        CacheTask.ClearCache();
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }

                }
            }
            return id;
        }

        public void Update(TaskInfo info)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_TASK_NAME, EDataType.NVarChar, 50, info.TaskName),
                this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, info.ProductID),
                this.GetParameter(PARM_IS_SYSTEM_TASK, EDataType.VarChar, 18, info.IsSystemTask.ToString()),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter(PARM_SERVICE_TYPE, EDataType.VarChar, 50, EServiceTypeUtils.GetValue(info.ServiceType)),
				this.GetParameter(PARM_SERVICE_PARAMETERS, EDataType.NText, info.ServiceParameters),
				this.GetParameter(PARM_FREQUENCY_TYPE, EDataType.VarChar, 50, EFrequencyTypeUtils.GetValue(info.FrequencyType)),
				this.GetParameter(PARM_PERIOD_INTERVAL_MINUTE, EDataType.Integer, info.PeriodIntervalMinute),
                this.GetParameter(PARM_START_DAY, EDataType.Integer, info.StartDay),
                this.GetParameter(PARM_START_WEEKDAY, EDataType.Integer, info.StartWeekday),
                this.GetParameter(PARM_START_HOUR, EDataType.Integer, info.StartHour),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, info.IsEnabled.ToString()),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, info.AddDate),
                this.GetParameter(PARM_LAST_EXECUTE_DATE, EDataType.DateTime, info.LastExecuteDate),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, info.Description),
                this.GetParameter(PARM_TASK_ID, EDataType.Integer, info.TaskID),
                this.GetParameter(PARM_ONLY_ONCE_DATE,EDataType.DateTime,info.OnlyOnceDate)
			};

            this.ExecuteNonQuery(SQL_UPDATE_TASK, updateParms);
            CacheTask.ClearCache();
        }

        public void UpdateState(int taskID, bool isEnabled)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, isEnabled.ToString()),
                this.GetParameter(PARM_TASK_ID, EDataType.Integer, taskID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_TASK_STATE, updateParms);
            CacheTask.ClearCache();
        }

        public void UpdateLastExecuteDate(int taskID)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_LAST_EXECUTE_DATE, EDataType.DateTime, DateTime.Now),
                this.GetParameter(PARM_TASK_ID, EDataType.Integer, taskID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_TASK_LAST_EXECUTE_DATE, updateParms);
            CacheTask.ClearCache();
        }

        public void Delete(int taskID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TASK_ID, EDataType.Integer, taskID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
            CacheTask.ClearCache();
        }

        public TaskInfo GetTaskInfo(int taskID)
        {
            TaskInfo info = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TASK_ID, EDataType.Integer, taskID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_ID, parms))
            {
                if (rdr.Read())
                {
                    info = new TaskInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetInt32(4), EServiceTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), EFrequencyTypeUtils.GetEnumType(rdr.GetValue(7).ToString()), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetInt32(10), rdr.GetInt32(11), TranslateUtils.ToBool(rdr.GetValue(12).ToString()), rdr.GetDateTime(13), rdr.GetDateTime(14), rdr.GetValue(15).ToString(), rdr.GetDateTime(16));
                }
                rdr.Close();
            }

            return info;
        }

        public ArrayList GetTaskIDArrayList(string productID, EServiceType serviceType)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = "SELECT TaskID FROM bairong_Task WHERE ProductID = @ProductID AND ServiceType = @ServiceType";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, productID),
                this.GetParameter(PARM_SERVICE_TYPE, EDataType.VarChar, 50, EServiceTypeUtils.GetValue(serviceType))
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetTaskInfoArrayList()
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = "SELECT TaskID, TaskName, ProductID, IsSystemTask, PublishmentSystemID, ServiceType, ServiceParameters, FrequencyType, PeriodIntervalMinute, StartDay, StartWeekday, StartHour, IsEnabled, AddDate, LastExecuteDate, Description, OnlyOnceDate FROM bairong_Task";

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    TaskInfo info = new TaskInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetInt32(4), EServiceTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), EFrequencyTypeUtils.GetEnumType(rdr.GetValue(7).ToString()), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetInt32(10), rdr.GetInt32(11), TranslateUtils.ToBool(rdr.GetValue(12).ToString()), rdr.GetDateTime(13), rdr.GetDateTime(14), rdr.GetValue(15).ToString(), rdr.GetDateTime(16));
                    arraylist.Add(info);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetTaskInfoArrayList(string productID, EServiceType serviceType)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = "SELECT TaskID, TaskName, ProductID, IsSystemTask, PublishmentSystemID, ServiceType, ServiceParameters, FrequencyType, PeriodIntervalMinute, StartDay, StartWeekday, StartHour, IsEnabled, AddDate, LastExecuteDate, Description, OnlyOnceDate FROM bairong_Task WHERE ProductID = @ProductID AND ServiceType = @ServiceType";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, productID),
                this.GetParameter(PARM_SERVICE_TYPE, EDataType.VarChar, 50, EServiceTypeUtils.GetValue(serviceType))
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                while (rdr.Read())
                {
                    TaskInfo info = new TaskInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetInt32(4), EServiceTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), EFrequencyTypeUtils.GetEnumType(rdr.GetValue(7).ToString()), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetInt32(10), rdr.GetInt32(11), TranslateUtils.ToBool(rdr.GetValue(12).ToString()), rdr.GetDateTime(13), rdr.GetDateTime(14), rdr.GetValue(15).ToString(), rdr.GetDateTime(16));
                    arraylist.Add(info);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetTaskInfoArrayList(string productID, EServiceType serviceType, int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = "SELECT TaskID, TaskName, ProductID, IsSystemTask, PublishmentSystemID, ServiceType, ServiceParameters, FrequencyType, PeriodIntervalMinute, StartDay, StartWeekday, StartHour, IsEnabled, AddDate, LastExecuteDate, Description, OnlyOnceDate FROM bairong_Task WHERE ProductID = @ProductID AND ServiceType = @ServiceType AND PublishmentSystemID = @PublishmentSystemID";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, productID),
                this.GetParameter(PARM_SERVICE_TYPE, EDataType.VarChar, 50, EServiceTypeUtils.GetValue(serviceType)),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                while (rdr.Read())
                {
                    TaskInfo info = new TaskInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetInt32(4), EServiceTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), EFrequencyTypeUtils.GetEnumType(rdr.GetValue(7).ToString()), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetInt32(10), rdr.GetInt32(11), TranslateUtils.ToBool(rdr.GetValue(12).ToString()), rdr.GetDateTime(13), rdr.GetDateTime(14), rdr.GetValue(15).ToString(), rdr.GetDateTime(16));
                    arraylist.Add(info);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public SortedList GetTaskInfoSortedList()
        {
            SortedList sortedlist = new SortedList();

            string sqlString = "SELECT TaskID, TaskName, ProductID, IsSystemTask, PublishmentSystemID, ServiceType, ServiceParameters, FrequencyType, PeriodIntervalMinute, StartDay, StartWeekday, StartHour, IsEnabled, AddDate, LastExecuteDate, Description, OnlyOnceDate FROM bairong_Task";

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    TaskInfo info = new TaskInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetInt32(4), EServiceTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), EFrequencyTypeUtils.GetEnumType(rdr.GetValue(7).ToString()), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetInt32(10), rdr.GetInt32(11), TranslateUtils.ToBool(rdr.GetValue(12).ToString()), rdr.GetDateTime(13), rdr.GetDateTime(14), rdr.GetValue(15).ToString(), rdr.GetDateTime(16));
                    sortedlist.Add(info.TaskID, info);
                }
                rdr.Close();
            }
            return sortedlist;
        }

        public IEnumerable GetDataSource(string productID, EServiceType serviceType, int publishmentSystemID)
        {
            string sqlString = "SELECT TaskID, TaskName, ProductID, IsSystemTask, PublishmentSystemID, ServiceType, ServiceParameters, FrequencyType, PeriodIntervalMinute, StartDay, StartWeekday, StartHour, IsEnabled, AddDate, LastExecuteDate, Description, OnlyOnceDate FROM bairong_Task WHERE ProductID = @ProductID AND ServiceType = @ServiceType AND PublishmentSystemID = @PublishmentSystemID";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, productID),
                this.GetParameter(PARM_SERVICE_TYPE, EDataType.VarChar, 50, EServiceTypeUtils.GetValue(serviceType)),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            return (IEnumerable)this.ExecuteReader(sqlString, parms);
        }

        public bool IsExists(string taskName)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TASK_NAME, EDataType.NVarChar, 50, taskName)
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

        public bool IsSystemTaskExists(string productID, int publishmentSystemID, EServiceType serviceType)
        {
            bool exists = false;

            string sqlString = "SELECT TaskID FROM bairong_Task WHERE ProductID = @ProductID AND PublishmentSystemID = @PublishmentSystemID AND IsSystemTask = @IsSystemTask AND ServiceType = @ServiceType";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, productID),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_IS_SYSTEM_TASK, EDataType.VarChar, 18, true.ToString()),
                this.GetParameter(PARM_SERVICE_TYPE, EDataType.VarChar, 50, EServiceTypeUtils.GetValue(serviceType))
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public void DeleteSystemTask(string productID, int publishmentSystemID, EServiceType serviceType)
        {
            string sqlString = "DELETE FROM bairong_Task WHERE ProductID = @ProductID AND PublishmentSystemID = @PublishmentSystemID AND IsSystemTask = @IsSystemTask AND ServiceType = @ServiceType";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, productID),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_IS_SYSTEM_TASK, EDataType.VarChar, 18, true.ToString()),
                this.GetParameter(PARM_SERVICE_TYPE, EDataType.VarChar, 50, EServiceTypeUtils.GetValue(serviceType))
			};

            this.ExecuteNonQuery(sqlString, parms);
            CacheTask.ClearCache();
        }

        public string GetPublishmentSystemName(int publishmentSystemID)
        {
            string publishmentSystemName = string.Empty;

            string sqlString = string.Format("SELECT PublishmentSystemName FROM siteserver_PublishmentSystem WHERE PublishmentSystemID = {0}", publishmentSystemID);

            try
            {
                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read())
                    {
                        publishmentSystemName = rdr.GetValue(0).ToString();
                    }
                    rdr.Close();
                }
            }
            catch { }

            return publishmentSystemName;
        }
    }
}
