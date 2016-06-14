using System;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Web;

using BaiRong.Model;
using BaiRong.Core.IO;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core
{
    public class LogUtils
    {
        public static void AddErrorLog(ErrorLogInfo logInfo)
        {
            try
            {
                #region 检测日志是否达到阈值
                if (ConfigManager.Additional.IsTimeThreshold)
                    BaiRongDataProvider.ErrorLogDAO.Delete(ConfigManager.Additional.TimeThreshold, 0);
                if (ConfigManager.Additional.IsCounterThreshold)
                    BaiRongDataProvider.ErrorLogDAO.Delete(0, ConfigManager.Additional.CounterThreshold * 10000);
                #endregion

                BaiRongDataProvider.ErrorLogDAO.Insert(logInfo);
            }
            catch { }
        }

        public static void AddErrorLog(Exception ex)
        {
            AddErrorLog(ex, string.Empty);
        }

        public static void AddErrorLog(Exception ex, string summary)
        {
            ErrorLogInfo logInfo = new ErrorLogInfo(0, DateTime.Now, ex.Message, ex.StackTrace, summary);
            AddErrorLog(logInfo);
        }

        public static void AddLog(string userName, string action)
        {
            LogUtils.AddLog(userName, action, string.Empty);
        }

        public static void AddLog(string userName, string action, string summary)
        {
            #region 检测日志是否达到阈值
            if (ConfigManager.Additional.IsTimeThreshold)
                BaiRongDataProvider.LogDAO.Delete(ConfigManager.Additional.TimeThreshold, 0);
            if (ConfigManager.Additional.IsCounterThreshold)
                BaiRongDataProvider.LogDAO.Delete(0, ConfigManager.Additional.CounterThreshold * 10000);
            #endregion

            if (ConfigManager.Additional.IsLog)
            {
                try
                {
                    if (!string.IsNullOrEmpty(action))
                    {
                        action = StringUtils.MaxLengthText(action, 250);
                    }
                    if (!string.IsNullOrEmpty(summary))
                    {
                        summary = StringUtils.MaxLengthText(summary, 250);
                    }
                    LogInfo logInfo = new LogInfo(0, userName, PageUtils.GetIPAddress(), DateTime.Now, action, summary);

                    BaiRongDataProvider.LogDAO.Insert(logInfo);
                }
                catch { }
            }
        }

        /// <summary>
        /// 添加用户日志
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="action"></param>
        public static void AddUserLog(string userName, string action)
        {
            LogUtils.AddUserLog(userName, action, string.Empty);
        }

        public static void AddUserLog(string userName, string action, string summary)
        {
            if (ConfigManager.Additional.IsLog)
            {
                try
                {
                    if (!string.IsNullOrEmpty(action))
                    {
                        action = StringUtils.MaxLengthText(action, 250);
                    }
                    if (!string.IsNullOrEmpty(summary))
                    {
                        summary = StringUtils.MaxLengthText(summary, 250);
                    }
                    UserLogInfo userLogInfo = new UserLogInfo(0, userName, PageUtils.GetIPAddress(), DateTime.Now, action, summary);
                    BaiRongDataProvider.UserLogDAO.Insert(userLogInfo);
                }
                catch { }
            }
        }
    }
}
