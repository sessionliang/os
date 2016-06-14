using System;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Model.Service;
using System.Collections;
using BaiRong.Core.Service;
using BaiRong.Core.Data.Provider;
using System.Reflection;

namespace BaiRong.Service.Execution
{
    public class TaskManager
    {
        public void ExecuteCreateTask()
        {
            EnvironmentManager.InitializeEnvironment();
            try
            {
                TaskManager.Invoke("SiteServer.STL.dll", "SiteServer.STL.Execution", "CreateQueueExecution", null);
            }
            catch { }
        }

        public void ExecuteBackgroundCreateTask()
        {
            EnvironmentManager.InitializeEnvironment();
            try
            {
                TaskManager.Invoke("SiteServer.STL.dll", "SiteServer.STL.Execution", "CreateBackgroundExecution", null);
            }
            catch { }
        }

        public void Execute()
        {
            EnvironmentManager.InitializeEnvironment();

            try
            {
                CacheManager.RenewServiceStatus();

                ArrayList taskInfoArrayList = CacheTask.Instance.GetTaskInfoArrayList();
                foreach (TaskInfo taskInfo in taskInfoArrayList)
                {
                    if (!this.IsNeedExecute(taskInfo))
                    {
                        continue;
                    }

                    try
                    {
                        string dllName = string.Empty;
                        string nameSpace = string.Empty;
                        string className = string.Empty;
                        if (StringUtils.EqualsIgnoreCase(taskInfo.ProductID, AppManager.WCM.AppID))
                        {
                            dllName = "SiteServer.WCM.dll";
                            nameSpace = "SiteServer.WCM.Execution";
                            className = EServiceTypeUtils.GetClassName(taskInfo.ServiceType);
                        }
                        else if (StringUtils.EqualsIgnoreCase(taskInfo.ProductID, AppManager.CMS.AppID))
                        {
                            dllName = "SiteServer.STL.dll";
                            nameSpace = "SiteServer.STL.Execution";
                            className = EServiceTypeUtils.GetClassName(taskInfo.ServiceType);
                        }

                        bool isSuccess = TaskManager.Invoke(dllName, nameSpace, className, taskInfo);
                        if (isSuccess)
                        {
                            TaskLogInfo logInfo = new TaskLogInfo(0, taskInfo.TaskID, true, string.Empty, string.Empty, string.Empty, DateTime.Now);
                            BaiRongDataProvider.TaskLogDAO.Insert(logInfo);
                        }
                    }
                    catch (Exception ex)
                    {
                        TaskManager.LogError(taskInfo, ex);
                    }

                    BaiRongDataProvider.TaskDAO.UpdateLastExecuteDate(taskInfo.TaskID);
                }
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(string.Empty, "任务执行失败", ex.ToString());
            }
        }

        public static void LogError(TaskInfo taskInfo, Exception ex)
        {
            if (taskInfo != null && ex != null)
            {
                TaskLogInfo logInfo = new TaskLogInfo(0, taskInfo.TaskID, false, ex.Message, ex.StackTrace, string.Empty, DateTime.Now);
                BaiRongDataProvider.TaskLogDAO.Insert(logInfo);
            }
        }

        public bool IsNeedExecute(TaskInfo taskInfo)
        {
            if (taskInfo != null && taskInfo.IsEnabled)
            {
                DateTime now = DateTime.Now;
                TimeSpan dateDiff = now - taskInfo.LastExecuteDate;

                if (taskInfo.FrequencyType == EFrequencyType.Month)
                {
                    if (dateDiff.TotalDays >= 28 && now.Day == taskInfo.StartDay && now.Hour == taskInfo.StartHour)
                    {
                        return true;
                    }
                }
                else if (taskInfo.FrequencyType == EFrequencyType.Week)
                {
                    if (dateDiff.TotalDays >= 7 && DateUtils.GetDayOfWeek(now) == taskInfo.StartWeekday && now.Hour == taskInfo.StartHour)
                    {
                        return true;
                    }
                }
                else if (taskInfo.FrequencyType == EFrequencyType.Day)
                {
                    if (dateDiff.TotalDays >= 1 && now.Hour == taskInfo.StartHour)
                    {
                        return true;
                    }
                }
                else if (taskInfo.FrequencyType == EFrequencyType.Hour)
                {
                    if (dateDiff.TotalHours >= 1)
                    {
                        return true;
                    }
                }
                else if (taskInfo.FrequencyType == EFrequencyType.Period)
                {
                    if (dateDiff.TotalMinutes >= taskInfo.PeriodIntervalMinute)
                    {
                        return true;
                    }
                }
                else if (taskInfo.FrequencyType == EFrequencyType.JustInTime)
                {
                    if (taskInfo.ServiceType == EServiceType.Publish)
                    {
                        return true;
                    }
                }
                else if (taskInfo.FrequencyType == EFrequencyType.OnlyOnce)
                {
                    if (taskInfo.ServiceType == EServiceType.Check && taskInfo.OnlyOnceDate <= DateTime.Now)
                    {
                        return true;
                    }
                    if (taskInfo.ServiceType == EServiceType.UnCheck && taskInfo.OnlyOnceDate <= DateTime.Now)
                    {
                        return true;
                    }
                    if (taskInfo.ServiceType == EServiceType.Create && taskInfo.OnlyOnceDate <= DateTime.Now)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool Invoke(string dllName, string nameSpace, string className, TaskInfo taskInfo)
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            string ddlFilePath = PathUtils.Combine(DirectoryUtils.GetDirectoryPath(ass.Location), dllName);

            System.Reflection.Assembly appAssembly = System.Reflection.Assembly.LoadFrom(ddlFilePath);
            Type type = appAssembly.GetType(nameSpace + "." + className);
            IExecution obj = Activator.CreateInstance(type) as IExecution;
            if (obj != null)
            {
                return obj.Execute(taskInfo);
            }
            return false;
        }
    }
}
