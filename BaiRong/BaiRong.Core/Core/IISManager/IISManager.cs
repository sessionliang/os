using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using Microsoft.Web.Administration;
using IISOle;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Diagnostics;
using System.Security.AccessControl;
using BaiRong.Core.Core.IISManager;
using ActiveDs;

namespace BaiRong.Core
{
    public class IISManager
    {
        public IISManager() { }

        //IIS版本
        public static string IISVersion
        {
            get
            {
                DirectoryEntry getEntity = new DirectoryEntry("IIS://localhost/W3SVC/INFO");
                string Version = getEntity.Properties["MajorIISVersionNumber"].Value.ToString();
                return Version;
            }
        }

        /// <summary>
        /// 创建网站
        /// </summary>
        /// <param name="siteInfo"></param>
        public static void CreateNewWebSite(WebSiteInfo siteInfo, string runtimeVersion, ManagedPipelineMode pipelineMode, string appPoolName)
        {
            try
            {
                string host = "localhost";
                string entPath = String.Format("IIS://{0}/w3svc", host);

                if (!EnsureNewSiteEnavaible(siteInfo.BindString, host))
                {
                    throw new Exception("该网站已存在" + Environment.NewLine + siteInfo.BindString);
                }

                //获取IIS根目录
                DirectoryEntry rootEntry = GetDirectoryEntry(entPath, host);
                string newSiteNum = GetNewWebSiteID(host);

                //创建新站点
                DirectoryEntry newSiteEntry = rootEntry.Children.Add(newSiteNum, "IIsWebServer");
                newSiteEntry.CommitChanges();
                newSiteEntry.Properties["ServerBindings"].Value = siteInfo.BindString;
                newSiteEntry.Properties["ServerComment"].Value = siteInfo.CommentOfWebSite;
                newSiteEntry.CommitChanges();

                //站点根目录
                string rootPath = System.Configuration.ConfigurationManager.AppSettings["RootPath"];
                string webPath = PathUtils.Combine(rootPath, siteInfo.WebPath);
                //创建新站点的根网站
                DirectoryEntry vdEntry = newSiteEntry.Children.Add("root", "IIsWebVirtualDir");
                vdEntry.CommitChanges();
                string ChangWebPath = webPath.TrimEnd("\\".ToCharArray());
                vdEntry.Properties["Path"].Value = ChangWebPath;
                vdEntry.Invoke("AppCreate", true);//创建应用程序
                vdEntry.Properties["AccessRead"][0] = true; //设置读取权限
                vdEntry.Properties["AccessWrite"][0] = true;
                vdEntry.Properties["AccessScript"][0] = true;//执行权限
                vdEntry.Properties["AccessExecute"][0] = false;
                vdEntry.Properties["DefaultDoc"][0] = "default.aspx,index.htm,index.html";//设置默认文档
                vdEntry.Properties["AppFriendlyName"][0] = siteInfo.DescOfWebSite; //应用程序名称           
                vdEntry.Properties["AuthFlags"][0] = 1;//0表示不允许匿名访问,1表示就可以3为基本身份验证，7为windows继承身份验证
                vdEntry.CommitChanges();

                #region 操作增加MIME

                #endregion

                #region 针对IIS7
                DirectoryEntry getEntity = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/INFO", host));
                int Version = int.Parse(IISManager.IISVersion);
                if (Version > 6)
                {
                    #region 创建应用程序池
                    if (string.IsNullOrEmpty(appPoolName))
                        appPoolName = siteInfo.CommentOfWebSite;
                    CreateAppPool(appPoolName, runtimeVersion, pipelineMode);
                    vdEntry.Properties["AppPoolId"].Value = appPoolName;
                    vdEntry.CommitChanges();
                    #endregion

                }
                #endregion

                //启动aspnet_regiis.exe程序 
                string errors = string.Empty;
                StartAppPool(vdEntry, out errors);
                if (errors != string.Empty)
                {
                    throw new Exception(errors);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 判断程序池是否存在
        /// </summary>
        /// <param name="AppPoolName">程序池名称</param>
        /// <returns>true存在 false不存在</returns>
        public static bool IsAppPoolName(string AppPoolName)
        {
            bool result = false;
            DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
            foreach (DirectoryEntry getdir in appPools.Children)
            {
                if (getdir.Name.Equals(AppPoolName))
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 删除指定程序池
        /// </summary>
        /// <param name="AppPoolName">程序池名称</param>
        /// <returns>true删除成功 false删除失败</returns>
        public static bool DeleteAppPool(string AppPoolName)
        {
            bool result = false;
            DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
            foreach (DirectoryEntry getdir in appPools.Children)
            {
                if (getdir.Name.Equals(AppPoolName))
                {
                    try
                    {
                        getdir.DeleteTree();
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        #region IIS7+ 创建应用程序池
        /// <summary>
        /// 创建应用程序池
        /// 设置.net 运行换件
        /// 管道模式
        /// </summary>
        /// <param name="AppPoolName"></param>
        /// <param name="runtimeVersion"></param>
        /// <param name="pipelineMode"></param>
        /// <returns></returns>
        public static bool CreateAppPool(string AppPoolName, string runtimeVersion, ManagedPipelineMode pipelineMode)
        {
            bool result = false;
            if (!IsAppPoolName(AppPoolName))
            {
                DirectoryEntry newpool;
                DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
                newpool = appPools.Children.Add(AppPoolName, "IIsApplicationPool");
                newpool.CommitChanges();
                result = true;
            }

            ServerManager sm = new ServerManager();
            sm.ApplicationPools[AppPoolName].ManagedRuntimeVersion = runtimeVersion;
            sm.ApplicationPools[AppPoolName].ManagedPipelineMode = pipelineMode; //托管模式Integrated为集成 Classic为经典
            sm.CommitChanges();

            return result;
        }

        #endregion

        /// <summary>
        /// 添加MIME
        /// </summary>
        /// <param name="rootEntry"></param>
        /// <param name="extension"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static bool AddMIME(DirectoryEntry rootEntry, string extension, string mimeType)
        {
            bool result = false;
            try
            {

                MimeMap NewMime = new MimeMap();
                NewMime.Extension = extension;
                NewMime.MimeType = mimeType;
                rootEntry.Properties["MimeMap"].Add(NewMime);
                rootEntry.CommitChanges();
                result = true;
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }

        /// <summary>
        /// 设置文件夹权限 处理给EVERONE赋予所有权限
        /// </summary>
        /// <param name="FileAdd">文件夹路径</param>
        public void SetFileRole(string FileAdd, string filePath)
        {
            FileAdd = FileAdd.Remove(FileAdd.LastIndexOf('\\'), 1);
            DirectorySecurity fSec = new DirectorySecurity(filePath, AccessControlSections.Access);
            fSec.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            System.IO.Directory.SetAccessControl(FileAdd, fSec);
        }

        /// <summary>
        /// 获取站点
        /// </summary>
        /// <param name="entPath"></param>
        /// <returns></returns>
        public static DirectoryEntry GetDirectoryEntry(string entPath, string host)
        {
            entPath = String.Format("IIS://{0}/w3svc", host);
            DirectoryEntry ent = new DirectoryEntry(entPath);
            return ent;
        }

        /// <summary>
        /// 获取站点ID
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string GetNewWebSiteID(string host)
        {
            ArrayList list = new ArrayList();
            string tempStr;
            string entPath = string.Format("IIS://{0}/w3svc", host);
            DirectoryEntry ent = GetDirectoryEntry(entPath, host);
            foreach (DirectoryEntry child in ent.Children)
            {
                if (child.SchemaClassName == "IIsWebServer")
                {
                    tempStr = child.Name.ToString();
                    int siteID = 0;
                    if (int.TryParse(tempStr, out siteID))
                    {
                        list.Add(siteID);
                    }
                }
            }
            list.Sort();
            var newId = Convert.ToInt32(list[list.Count - 1]) + 1;
            return newId.ToString();
        }

        /// <summary>
        /// 判断站点是否存在
        /// </summary>
        /// <param name="bindString"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public static bool EnsureNewSiteEnavaible(string bindString, string host)
        {
            string entPath = string.Format("IIS://{0}/w3svc", host);
            DirectoryEntry ent = GetDirectoryEntry(entPath, host);
            foreach (DirectoryEntry child in ent.Children)
            {
                if (child.SchemaClassName == "IIsWebServer")
                {
                    System.DirectoryServices.PropertyValueCollection collection = child.Properties["ServerBindings"];
                    for (int i = 0; i < collection.Count; i++)
                    {
                        if (collection[i].ToString() == bindString)
                        {
                            return false;
                        }
                    }

                }
            }

            return true;
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="vdEntry">站点</param>
        /// <returns></returns>
        public static bool StartAppPool(DirectoryEntry vdEntry, out string errors)
        {
            bool result = false;
            try
            {

                //启动aspnet_regiis.exe程序 
                string fileName = Environment.GetEnvironmentVariable("windir") + @"\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe";
                ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
                //处理目录路径 
                string path = vdEntry.Path.ToUpper();
                int index = path.IndexOf("W3SVC");
                path = path.Remove(0, index);
                //启动ASPnet_iis.exe程序,刷新脚本映射 
                startInfo.Arguments = "-s " + path;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                errors = process.StandardError.ReadToEnd();
                result = true;
            }
            catch (Exception)
            {
                result = false;
                throw;
            }


            return result;
        }
    }
}