using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Web.UI;
using BaiRong.Core;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Model;
using System.Collections.Specialized;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class Service : Page
    {
        const string CACHE_TOTAL_COUNT = "_TotalCount";
        const string CACHE_CURRENT_COUNT = "_CurrentCount";
        const string CACHE_MESSAGE = "_Message";

        public static string GetRedirectUrl(int publishmentSystemID, string type)
        {
            return PageUtils.GetBBSUrl(string.Format("service.aspx?publishmentSystemID={0}&type={1}", publishmentSystemID, type));
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            int publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
            string type = base.Request.QueryString["type"];
            string userKeyPrefix = base.Request["userKeyPrefix"];
            NameValueCollection retval = new NameValueCollection();

            if (type == "GetCountArray")
            {
                retval = GetCountArray(userKeyPrefix);
            }
            else if (type == "CreateForums")
            {
                retval = CreateForums(publishmentSystemID, userKeyPrefix);
            }
            else if (type == "CreateFiles")
            {
                string directoryName = base.Request.Form["directoryName"];
                retval = CreateFiles(publishmentSystemID, directoryName, userKeyPrefix);
            }
            else if (type == "CreateAll")
            {
                retval = CreateAll(publishmentSystemID, userKeyPrefix);
            }

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(retval);
            Page.Response.Write(jsonString);
            Page.Response.End();
        }

        NameValueCollection GetCountArray(string userKeyPrefix)//进度及显示
        {
            NameValueCollection retval = new NameValueCollection();
            if (CacheUtils.Get(userKeyPrefix + CACHE_TOTAL_COUNT) != null && CacheUtils.Get(userKeyPrefix + CACHE_CURRENT_COUNT) != null && CacheUtils.Get(userKeyPrefix + CACHE_MESSAGE) != null)
            {
                int totalCount = TranslateUtils.ToInt((string)CacheUtils.Get(userKeyPrefix + CACHE_TOTAL_COUNT));
                int currentCount = TranslateUtils.ToInt((string)CacheUtils.Get(userKeyPrefix + CACHE_CURRENT_COUNT));
                string message = (string)CacheUtils.Get(userKeyPrefix + CACHE_MESSAGE);

                retval.Add("totalCount", totalCount.ToString());
                retval.Add("currentCount", currentCount.ToString());
                retval.Add("message", message);
            }
            return retval;
        }

        #region 生成页面

        NameValueCollection CreateForums(int publishmentSystemID, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval = new NameValueCollection();

            ArrayList forumIDArrayList = TranslateUtils.StringCollectionToIntArrayList(DbCacheManager.GetAndRemove(userKeyPrefix + "ForumIDCollection"));

            try
            {
                StringBuilder resultBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();

                int totalCount = forumIDArrayList.Count;
                int currentCount = 0;
                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                if (forumIDArrayList.Count > 0)
                {
                    FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                    foreach (int forumIDToCreate in forumIDArrayList)
                    {
                        try
                        {
                            FSO.CreateForum(forumIDToCreate);
                            currentCount++;
                            CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                        }
                        catch (Exception ex)
                        {
                            errorBuilder.Append("<br />板块“").Append(ForumManager.GetForumName(publishmentSystemID, forumIDToCreate)).Append("”生成失败！");
                            errorBuilder.Append("<br />").Append(ex.Message);
                            LogUtils.AddErrorLog(ex);
                        }
                    }
                }

                resultBuilder.AppendFormat("任务完成，共生成板块<strong> {0} </strong>页。", currentCount);

                retval.Add("resultMessage", resultBuilder.ToString());
                retval.Add("errorMessage", errorBuilder.ToString());
            }
            catch (Exception ex)
            {
                retval.Add("resultMessage", string.Empty);
                retval.Add("errorMessage", ex.Message);
                LogUtils.AddErrorLog(ex);
            }

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息

            return retval;
        }

        NameValueCollection CreateFiles(int publishmentSystemID, string directoryName, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval = new NameValueCollection();

            ArrayList fileNameArrayList = TranslateUtils.StringCollectionToArrayList(DbCacheManager.GetAndRemove(userKeyPrefix + "FileNameCollection"));
            try
            {
                StringBuilder resultBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();

                int totalCount = fileNameArrayList.Count;
                int currentCount = 0;

                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                if (totalCount > 0)
                {
                    FileSystemObject FSO = new FileSystemObject(publishmentSystemID);

                    foreach (string fileName in fileNameArrayList)
                    {
                        try
                        {
                            FSO.CreateFile(directoryName, fileName);

                            currentCount++;
                            CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                        }
                        catch (Exception ex)
                        {
                            errorBuilder.Append("<br />文件“").Append(PathUtility.GetPublishmentSystemPath(publishmentSystemID, directoryName, fileName)).Append("”生成失败！");
                            errorBuilder.Append("<br />").Append(ex.Message);
                            LogUtils.AddErrorLog(ex);
                        }
                    }
                }

                resultBuilder.AppendFormat("任务完成，共生成文件<strong> {0} </strong>页。", currentCount);

                retval.Add("resultMessage", resultBuilder.ToString());
                retval.Add("errorMessage", errorBuilder.ToString());
            }
            catch (Exception ex)
            {
                retval.Add("resultMessage", string.Empty);
                retval.Add("errorMessage", ex.Message);
                LogUtils.AddErrorLog(ex);
            }

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息

            return retval;
        }

        NameValueCollection CreateAll(int publishmentSystemID, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
            CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
            CacheUtils.Max(cacheMessageKey, string.Empty);//存储消息

            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval = new NameValueCollection();
            
            try
            {
                StringBuilder resultBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();

                int totalCount = 0;
                int currentCount = 0;

                string directoryPath = PathUtilityBBS.GetTemplateDirectoryPath(publishmentSystemID);
                Hashtable hashtable = new Hashtable();
                ArrayList directoryNames = new ArrayList();
                directoryNames.Add(string.Empty);
                directoryNames.AddRange(DirectoryUtils.GetDirectoryNames(directoryPath));
                foreach (string directoryName in directoryNames)
                {
                    if (StringUtils.EqualsIgnoreCase(directoryName, "include")) continue;

                    ArrayList fileNameArrayList = TranslateUtils.StringArrayToArrayList(DirectoryUtils.GetFileNames(PathUtils.Combine(directoryPath, directoryName)));

                    if (fileNameArrayList.Count > 0)
                    {
                        totalCount += fileNameArrayList.Count;
                        hashtable[directoryName] = fileNameArrayList;
                    }
                }

                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                if (totalCount > 0)
                {
                    FileSystemObject FSO = new FileSystemObject(publishmentSystemID);

                    foreach (string directoryName in hashtable.Keys)
                    {
                        ArrayList fileNameArrayList = hashtable[directoryName] as ArrayList;
                        foreach (string fileName in fileNameArrayList)
                        {
                            try
                            {
                                currentCount++;
                                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数

                                if (StringUtils.EqualsIgnoreCase(fileName, "template.xml")) continue;
                                FSO.CreateFile(directoryName, fileName);
                            }
                            catch (Exception ex)
                            {
                                errorBuilder.Append("<br />文件“").Append(PathUtility.GetPublishmentSystemPath(publishmentSystemID, directoryName, fileName)).Append("”生成失败！");
                                errorBuilder.Append("<br />").Append(ex.Message);
                                LogUtils.AddErrorLog(ex);
                            }
                        }
                    }
                }

                resultBuilder.AppendFormat("任务完成，共生成文件<strong> {0} </strong>页。", currentCount);

                retval.Add("resultMessage", resultBuilder.ToString());
                retval.Add("errorMessage", errorBuilder.ToString());
            }
            catch (Exception ex)
            {
                retval.Add("resultMessage", string.Empty);
                retval.Add("errorMessage", ex.Message);
                LogUtils.AddErrorLog(ex);
            }

            CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
            CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
            CacheUtils.Remove(cacheMessageKey);//取消存储消息

            return retval;
        }
        
        #endregion
    }
}
