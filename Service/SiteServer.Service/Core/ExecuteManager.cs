using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SiteServer.Service.Core
{
    public sealed class ExecuteManager : IDisposable
    {
        private BackgroundWorker worker = new BackgroundWorker();
        private DispatcherTimer timer = new DispatcherTimer();

        private static MethodInfo REF_METHOD = null;
        private static object REF_OBJECT = null;

        #region 服务组件Queue生成
        private BackgroundWorker createWorker = new BackgroundWorker();
        private DispatcherTimer createTimer = new DispatcherTimer();
        private static MethodInfo REF_METHOD_CREATE = null;
        private static object REF_OBJECT_CREATE = null;
        #endregion

        #region 服务组件Background生成
        private BackgroundWorker backgroundCreateWorker = new BackgroundWorker();
        private DispatcherTimer backgroundCreateTimer = new DispatcherTimer();
        private static MethodInfo REF_METHOD_BACKGROUND_CREATE = null;
        private static object REF_OBJECT_BACKGROUND_CREATE = null;
        #endregion

        public void Execute()
        {
            worker.DoWork += worker_DoWork;
            //worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 20);   // 20秒
            timer.Start();

            createWorker.DoWork += createWorker_DoWork;
            createTimer.Tick += createTimer_Tick;
            createTimer.Interval = new TimeSpan(0, 0, 3);   // 3秒
            createTimer.Start();

            backgroundCreateWorker.DoWork += backgroundCreateWorker_DoWork;
            backgroundCreateTimer.Tick += backgroundCreateTimer_Tick;
            backgroundCreateTimer.Interval = new TimeSpan(0, 0, 3);   // 3秒
            backgroundCreateTimer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.DirectoryPath) && !worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
        }

        void createTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.DirectoryPath) && !createWorker.IsBusy)
            {
                createWorker.RunWorkerAsync();
            }
        }

        void backgroundCreateTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.DirectoryPath) && !backgroundCreateWorker.IsBusy)
            {
                backgroundCreateWorker.RunWorkerAsync();
            }
        }

        //void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{

        //}

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            string ddlFilePath = ConfigurationManager.GetDDLFilePath(ConfigurationManager.DirectoryPath);
            ExecuteManager.Invoke(ddlFilePath, ConfigurationManager.NAME_SPACE, ConfigurationManager.CLASS_NAME, ConfigurationManager.METHOD_NAME, null);
        }

        void createWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string ddlFilePath = ConfigurationManager.GetDDLFilePath(ConfigurationManager.DirectoryPath);
            ExecuteManager.InvokeCreateTask(ddlFilePath, ConfigurationManager.NAME_SPACE, ConfigurationManager.CLASS_NAME, ConfigurationManager.METHOD_NAME_CREATETASK, null);
        }

        void backgroundCreateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string ddlFilePath = ConfigurationManager.GetDDLFilePath(ConfigurationManager.DirectoryPath);
            ExecuteManager.InvokeBackgroundCreateTask(ddlFilePath, ConfigurationManager.NAME_SPACE, ConfigurationManager.CLASS_NAME, ConfigurationManager.METHOD_NAME_BACKGROUNDCREATETASK, null);
        }

        private static object Invoke(string ddlFilePath, string nameSpace, string className, string methodName, object[] parameters)
        {
            if (REF_METHOD == null || REF_OBJECT == null)
            {
                if (FileSystemUtils.IsFileExists(ddlFilePath))
                {
                    Assembly appAssembly = Assembly.LoadFrom(ddlFilePath);
                    if (appAssembly != null)
                    {
                        Type type = appAssembly.GetType(nameSpace + "." + className);
                        if (type != null)
                        {
                            REF_METHOD = type.GetMethod(methodName);
                            REF_OBJECT = Activator.CreateInstance(type);
                        }
                    }
                }
            }
            if (REF_METHOD != null && REF_OBJECT != null)
            {
                return REF_METHOD.Invoke(REF_OBJECT, parameters);
            }
            return null;
        }


        private static object InvokeCreateTask(string ddlFilePath, string nameSpace, string className, string methodName, object[] parameters)
        {
            if (REF_METHOD_CREATE == null || REF_OBJECT_CREATE == null)
            {
                if (FileSystemUtils.IsFileExists(ddlFilePath))
                {
                    Assembly appAssembly = Assembly.LoadFrom(ddlFilePath);
                    if (appAssembly != null)
                    {
                        Type type = appAssembly.GetType(nameSpace + "." + className);
                        if (type != null)
                        {
                            REF_METHOD_CREATE = type.GetMethod(methodName);
                            REF_OBJECT_CREATE = Activator.CreateInstance(type);
                        }
                    }
                }
            }
            if (REF_METHOD_CREATE != null && REF_OBJECT_CREATE != null)
            {
                return REF_METHOD_CREATE.Invoke(REF_OBJECT_CREATE, parameters);
            }
            return null;
        }

        private static object InvokeBackgroundCreateTask(string ddlFilePath, string nameSpace, string className, string methodName, object[] parameters)
        {
            if (REF_METHOD_CREATE == null || REF_OBJECT_BACKGROUND_CREATE == null)
            {
                if (FileSystemUtils.IsFileExists(ddlFilePath))
                {
                    Assembly appAssembly = Assembly.LoadFrom(ddlFilePath);
                    if (appAssembly != null)
                    {
                        Type type = appAssembly.GetType(nameSpace + "." + className);
                        if (type != null)
                        {
                            REF_METHOD_BACKGROUND_CREATE = type.GetMethod(methodName);
                            REF_OBJECT_BACKGROUND_CREATE = Activator.CreateInstance(type);
                        }
                    }
                }
            }
            if (REF_METHOD_CREATE != null && REF_OBJECT_BACKGROUND_CREATE != null)
            {
                return REF_METHOD_BACKGROUND_CREATE.Invoke(REF_OBJECT_BACKGROUND_CREATE, parameters);
            }
            return null;
        }

        public void Dispose()
        {
            worker.Dispose();
            createWorker.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
