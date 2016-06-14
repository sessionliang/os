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

        //IIS�汾
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
        /// ������վ
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
                    throw new Exception("����վ�Ѵ���" + Environment.NewLine + siteInfo.BindString);
                }

                //��ȡIIS��Ŀ¼
                DirectoryEntry rootEntry = GetDirectoryEntry(entPath, host);
                string newSiteNum = GetNewWebSiteID(host);

                //������վ��
                DirectoryEntry newSiteEntry = rootEntry.Children.Add(newSiteNum, "IIsWebServer");
                newSiteEntry.CommitChanges();
                newSiteEntry.Properties["ServerBindings"].Value = siteInfo.BindString;
                newSiteEntry.Properties["ServerComment"].Value = siteInfo.CommentOfWebSite;
                newSiteEntry.CommitChanges();

                //վ���Ŀ¼
                string rootPath = System.Configuration.ConfigurationManager.AppSettings["RootPath"];
                string webPath = PathUtils.Combine(rootPath, siteInfo.WebPath);
                //������վ��ĸ���վ
                DirectoryEntry vdEntry = newSiteEntry.Children.Add("root", "IIsWebVirtualDir");
                vdEntry.CommitChanges();
                string ChangWebPath = webPath.TrimEnd("\\".ToCharArray());
                vdEntry.Properties["Path"].Value = ChangWebPath;
                vdEntry.Invoke("AppCreate", true);//����Ӧ�ó���
                vdEntry.Properties["AccessRead"][0] = true; //���ö�ȡȨ��
                vdEntry.Properties["AccessWrite"][0] = true;
                vdEntry.Properties["AccessScript"][0] = true;//ִ��Ȩ��
                vdEntry.Properties["AccessExecute"][0] = false;
                vdEntry.Properties["DefaultDoc"][0] = "default.aspx,index.htm,index.html";//����Ĭ���ĵ�
                vdEntry.Properties["AppFriendlyName"][0] = siteInfo.DescOfWebSite; //Ӧ�ó�������           
                vdEntry.Properties["AuthFlags"][0] = 1;//0��ʾ��������������,1��ʾ�Ϳ���3Ϊ���������֤��7Ϊwindows�̳������֤
                vdEntry.CommitChanges();

                #region ��������MIME

                #endregion

                #region ���IIS7
                DirectoryEntry getEntity = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/INFO", host));
                int Version = int.Parse(IISManager.IISVersion);
                if (Version > 6)
                {
                    #region ����Ӧ�ó����
                    if (string.IsNullOrEmpty(appPoolName))
                        appPoolName = siteInfo.CommentOfWebSite;
                    CreateAppPool(appPoolName, runtimeVersion, pipelineMode);
                    vdEntry.Properties["AppPoolId"].Value = appPoolName;
                    vdEntry.CommitChanges();
                    #endregion

                }
                #endregion

                //����aspnet_regiis.exe���� 
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
        /// �жϳ�����Ƿ����
        /// </summary>
        /// <param name="AppPoolName">���������</param>
        /// <returns>true���� false������</returns>
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
        /// ɾ��ָ�������
        /// </summary>
        /// <param name="AppPoolName">���������</param>
        /// <returns>trueɾ���ɹ� falseɾ��ʧ��</returns>
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

        #region IIS7+ ����Ӧ�ó����
        /// <summary>
        /// ����Ӧ�ó����
        /// ����.net ���л���
        /// �ܵ�ģʽ
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
            sm.ApplicationPools[AppPoolName].ManagedPipelineMode = pipelineMode; //�й�ģʽIntegratedΪ���� ClassicΪ����
            sm.CommitChanges();

            return result;
        }

        #endregion

        /// <summary>
        /// ���MIME
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
        /// �����ļ���Ȩ�� �����EVERONE��������Ȩ��
        /// </summary>
        /// <param name="FileAdd">�ļ���·��</param>
        public void SetFileRole(string FileAdd, string filePath)
        {
            FileAdd = FileAdd.Remove(FileAdd.LastIndexOf('\\'), 1);
            DirectorySecurity fSec = new DirectorySecurity(filePath, AccessControlSections.Access);
            fSec.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            System.IO.Directory.SetAccessControl(FileAdd, fSec);
        }

        /// <summary>
        /// ��ȡվ��
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
        /// ��ȡվ��ID
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
        /// �ж�վ���Ƿ����
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
        /// ����
        /// </summary>
        /// <param name="vdEntry">վ��</param>
        /// <returns></returns>
        public static bool StartAppPool(DirectoryEntry vdEntry, out string errors)
        {
            bool result = false;
            try
            {

                //����aspnet_regiis.exe���� 
                string fileName = Environment.GetEnvironmentVariable("windir") + @"\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe";
                ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
                //����Ŀ¼·�� 
                string path = vdEntry.Path.ToUpper();
                int index = path.IndexOf("W3SVC");
                path = path.Remove(0, index);
                //����ASPnet_iis.exe����,ˢ�½ű�ӳ�� 
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