using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.IO;
using BaiRong.Core.Net;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using SiteServer.STL.IO;

namespace SiteServer.CMS.Services
{
    public class Utils : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                string type = PageUtils.FilterSql(base.Request.QueryString["type"]);
                if (string.IsNullOrEmpty(type))
                {
                    type = PageUtility.ServiceSTL.Utils.Type_Redirect;
                }

                if (StringUtils.EqualsIgnoreCase(type, PageUtility.ServiceSTL.Utils.Type_Redirect))
                {
                    Redirect();
                }
                else if (StringUtils.EqualsIgnoreCase(type, PageUtility.ServiceSTL.Utils.Type_Download))
                {
                    Download();
                }
                else if (StringUtils.EqualsIgnoreCase(type, PageUtility.ServiceSTL.Utils.Type_StlTrigger))
                {
                    StlTrigger();
                }
            }
            catch { }
        }

        public void StlTrigger()
        {
            int publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
            int channelID = 0;
            if (base.Request.QueryString["channelID"] != null)
            {
                channelID = TranslateUtils.ToInt(base.Request.QueryString["channelID"]);
            }
            else
            {
                channelID = publishmentSystemID;
            }
            int contentID = 0;
            if (base.Request.QueryString["contentID"] != null)
            {
                contentID = TranslateUtils.ToInt(base.Request.QueryString["contentID"]);
            }
            int fileTemplateID = 0;
            if (base.Request.QueryString["fileTemplateID"] != null)
            {
                fileTemplateID = TranslateUtils.ToInt(base.Request.QueryString["fileTemplateID"]);
            }
            bool isRedirect = false;
            if (base.Request.QueryString["isRedirect"] != null)
            {
                isRedirect = TranslateUtils.ToBool(base.Request.QueryString["isRedirect"]);
            }

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
            if (fileTemplateID != 0)
            {
                FSO.CreateFile(fileTemplateID);
            }
            else if (contentID != 0)
            {
                FSO.CreateContent(tableStyle, tableName, channelID, contentID);
            }
            else if (channelID != 0)
            {
                FSO.CreateChannel(channelID);
            }
            else if (publishmentSystemID != 0)
            {
                FSO.CreateIndex();
            }

            if (isRedirect)
            {
                string redirectUrl = string.Empty;
                if (fileTemplateID != 0)
                {
                    redirectUrl = PageUtility.GetFileUrl(publishmentSystemInfo, fileTemplateID, publishmentSystemInfo.Additional.VisualType);
                }
                else if (contentID != 0)
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                    redirectUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, publishmentSystemInfo.Additional.VisualType);
                }
                else if (channelID != 0)
                {
                    redirectUrl = PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, publishmentSystemInfo.Additional.VisualType);
                }
                else if (publishmentSystemID != 0)
                {
                    redirectUrl = PageUtility.GetIndexPageUrl(publishmentSystemInfo, publishmentSystemInfo.Additional.VisualType);
                }

                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    redirectUrl = PageUtils.AddQueryString(redirectUrl, "__r", StringUtils.GetRandomInt(1, 10000).ToString());
                    base.Response.Redirect(redirectUrl, true);
                    return;
                }
            }

            base.Response.Write(string.Empty);
            base.Response.End();
        }

        public void Redirect()
        {
            string url;
            if (!string.IsNullOrEmpty(base.Request.QueryString["u"]))
            {
                url = base.Request.QueryString["u"];
                if (!url.ToLower().StartsWith("http://") && !url.ToLower().StartsWith("https://"))
                {
                    url = "http://" + url;
                }
                PageUtils.Redirect(url);
            }
            //else if (!string.IsNullOrEmpty(base.Request.QueryString["download"]))
            //{
            //    bool isSuccess = false;
            //    try
            //    {
            //        if (!string.IsNullOrEmpty(base.Request.QueryString["publishmentSystemID"]) && !string.IsNullOrEmpty(base.Request.QueryString["fileUrl"]))
            //        {
            //            int publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
            //            string fileUrl = RuntimeUtils.DecryptStringByTranslate(base.Request.QueryString["fileUrl"]);

            //            if (PageUtils.IsProtocolUrl(fileUrl))
            //            {
            //                isSuccess = true;
            //                PageUtils.Redirect(fileUrl);
            //            }
            //            else
            //            {
            //                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            //                string filePath = PathUtility.MapPath(publishmentSystemInfo, fileUrl);
            //                EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
            //                if (EFileSystemTypeUtils.IsDownload(fileType))
            //                {
            //                    if (FileUtils.IsFileExists(filePath))
            //                    {
            //                        isSuccess = true;
            //                        PageUtils.Download(base.Response, filePath);
            //                    }
            //                }
            //                else
            //                {
            //                    isSuccess = true;
            //                    PageUtils.Redirect(PageUtility.ParseNavigationUrl(publishmentSystemInfo, fileUrl));
            //                }
            //            }
            //        }
            //        else if (!string.IsNullOrEmpty(base.Request.QueryString["filePath"]))
            //        {
            //            string filePath = RuntimeUtils.DecryptStringByTranslate(base.Request.QueryString["filePath"]);
            //            EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
            //            if (EFileSystemTypeUtils.IsDownload(fileType))
            //            {
            //                if (FileUtils.IsFileExists(filePath))
            //                {
            //                    isSuccess = true;
            //                    PageUtils.Download(base.Response, filePath);
            //                }
            //            }
            //            else
            //            {
            //                isSuccess = true;
            //                string fileUrl = PageUtils.GetRootUrlByPhysicalPath(filePath);
            //                PageUtils.Redirect(PageUtils.ParseNavigationUrl(fileUrl));
            //            }
            //        }
            //        else if (!string.IsNullOrEmpty(base.Request.QueryString["publishmentSystemID"]) && !string.IsNullOrEmpty(base.Request.QueryString["channelID"]) && !string.IsNullOrEmpty(base.Request.QueryString["contentID"]))
            //        {
            //            int publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
            //            int channelID = TranslateUtils.ToInt(base.Request.QueryString["channelID"]);
            //            int contentID = TranslateUtils.ToInt(base.Request.QueryString["contentID"]);
            //            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            //            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
            //            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
            //            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
            //            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

            //            if (contentInfo != null && !string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl)))
            //            {
            //                string fileUrl = contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl);
            //                if (publishmentSystemInfo.Additional.IsCountDownload)
            //                {
            //                    CountManager.AddCount(AppManager.CMS.AppID, tableName, contentID.ToString(), ECountType.Download);
            //                }

            //                if (PageUtils.IsProtocolUrl(fileUrl))
            //                {
            //                    isSuccess = true;
            //                    PageUtils.Redirect(fileUrl);
            //                }
            //                else
            //                {
            //                    string filePath = PathUtility.MapPath(publishmentSystemInfo, fileUrl);
            //                    EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
            //                    if (EFileSystemTypeUtils.IsDownload(fileType))
            //                    {
            //                        if (FileUtils.IsFileExists(filePath))
            //                        {
            //                            isSuccess = true;
            //                            PageUtils.Download(base.Response, filePath);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        isSuccess = true;
            //                        PageUtils.Redirect(PageUtility.ParseNavigationUrl(publishmentSystemInfo, fileUrl));
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    catch { }
            //    if (!isSuccess)
            //    {
            //        base.Response.Write("下载失败，不存在此文件！");
            //    }
            //}
            else if (!string.IsNullOrEmpty(base.Request.QueryString["publishmentSystemID"]) && !string.IsNullOrEmpty(base.Request.QueryString["channelID"]) && !string.IsNullOrEmpty(base.Request.QueryString["contentID"]))
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
                int channelID = TranslateUtils.ToInt(base.Request.QueryString["channelID"]);
                int contentID = TranslateUtils.ToInt(base.Request.QueryString["contentID"]);
                bool isInner = TranslateUtils.ToBool(base.Request.QueryString["isInner"]);

                if (contentID != 0)
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                    url = PageUtility.GetContentUrl(publishmentSystemInfo, nodeInfo, contentID, isInner, publishmentSystemInfo.Additional.VisualType);
                    if (!url.Equals(PageUtils.UNCLICKED_URL))
                    {
                        PageUtils.Redirect(url);
                    }
                    else
                    {
                        Redirect_DefaultDirection();
                    }
                }
                else
                {
                    Redirect_DefaultDirection();
                }
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["channelID"]) && !string.IsNullOrEmpty(base.Request.QueryString["contentID"]))
            {
                int channelID = TranslateUtils.ToInt(base.Request.QueryString["channelID"]);
                int contentID = TranslateUtils.ToInt(base.Request.QueryString["contentID"]);
                bool isInner = TranslateUtils.ToBool(base.Request.QueryString["isInner"]);

                if (contentID != 0)
                {
                    int publishmentSystemID = DataProvider.NodeDAO.GetPublishmentSystemID(channelID);
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                    url = PageUtility.GetContentUrl(publishmentSystemInfo, nodeInfo, contentID, isInner, publishmentSystemInfo.Additional.VisualType);
                    if (!url.Equals(PageUtils.UNCLICKED_URL))
                    {
                        PageUtils.Redirect(url);
                    }
                    else
                    {
                        Redirect_DefaultDirection();
                    }
                }
                else
                {
                    Redirect_DefaultDirection();
                }
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["channelID"]))
            {
                int channelID = TranslateUtils.ToInt(base.Request.QueryString["channelID"]);
                bool isInner = TranslateUtils.ToBool(base.Request.QueryString["isInner"]);

                if (channelID != 0)
                {
                    int publishmentSystemID = DataProvider.NodeDAO.GetPublishmentSystemID(channelID);
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                    url = PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, isInner, publishmentSystemInfo.Additional.VisualType);
                    if (!url.Equals(PageUtils.UNCLICKED_URL))
                    {
                        PageUtils.Redirect(url);
                    }
                    else
                    {
                        Redirect_DefaultDirection();
                    }
                }
                else
                {
                    Redirect_DefaultDirection();
                }
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["channelindex"]))
            {
                string channelIndex = base.Request.QueryString["channelindex"];
                int publishmentSystemID = PathUtility.GetCurrentPublishmentSystemID();
                if (publishmentSystemID == 0)
                {
                    publishmentSystemID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByIsHeadquarters();
                }
                if (publishmentSystemID != 0)
                {
                    int channelID = DataProvider.NodeDAO.GetNodeIDByNodeIndexName(publishmentSystemID, channelIndex);
                    if (channelID != 0)
                    {
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                        url = PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, publishmentSystemInfo.Additional.VisualType);
                        if (!url.Equals(PageUtils.UNCLICKED_URL))
                        {
                            PageUtils.Redirect(url);
                        }
                        else
                        {
                            Redirect_DefaultDirection();
                        }
                    }
                    else
                    {
                        Redirect_DefaultDirection();
                    }
                }
                else
                {
                    Redirect_DefaultDirection();
                }
            }
            else
            {
                Redirect_DefaultDirection();
            }
        }

        public void Download()
        {
            bool isSuccess = false;
            try
            {
                if (!string.IsNullOrEmpty(base.Request.QueryString["publishmentSystemID"]) && !string.IsNullOrEmpty(base.Request.QueryString["fileUrl"]) && string.IsNullOrEmpty(base.Request.QueryString["contentID"]))
                {
                    int publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
                    string fileUrl = RuntimeUtils.DecryptStringByTranslate(base.Request.QueryString["fileUrl"]);

                    if (PageUtils.IsProtocolUrl(fileUrl))
                    {
                        isSuccess = true;
                        PageUtils.Redirect(fileUrl);
                    }
                    else
                    {
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                        string filePath = PathUtility.MapPath(publishmentSystemInfo, fileUrl);
                        EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
                        if (EFileSystemTypeUtils.IsDownload(fileType))
                        {
                            if (FileUtils.IsFileExists(filePath))
                            {
                                isSuccess = true;
                                PageUtils.Download(base.Response, filePath);
                            }
                        }
                        else
                        {
                            isSuccess = true;
                            PageUtils.Redirect(PageUtility.ParseNavigationUrl(publishmentSystemInfo, fileUrl));
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(base.Request.QueryString["filePath"]))
                {
                    string filePath = RuntimeUtils.DecryptStringByTranslate(base.Request.QueryString["filePath"]);
                    EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
                    if (EFileSystemTypeUtils.IsDownload(fileType))
                    {
                        if (FileUtils.IsFileExists(filePath))
                        {
                            isSuccess = true;
                            PageUtils.Download(base.Response, filePath);
                        }
                    }
                    else
                    {
                        isSuccess = true;
                        string fileUrl = PageUtils.GetRootUrlByPhysicalPath(filePath);
                        PageUtils.Redirect(PageUtils.ParseNavigationUrl(fileUrl));
                    }
                }
                else if (!string.IsNullOrEmpty(base.Request.QueryString["publishmentSystemID"]) && !string.IsNullOrEmpty(base.Request.QueryString["channelID"]) && !string.IsNullOrEmpty(base.Request.QueryString["contentID"]) && !string.IsNullOrEmpty(base.Request.QueryString["fileUrl"]))
                {
                    int publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
                    int channelID = TranslateUtils.ToInt(base.Request.QueryString["channelID"]);
                    int contentID = TranslateUtils.ToInt(base.Request.QueryString["contentID"]);
                    string fileUrl = RuntimeUtils.DecryptStringByTranslate(base.Request.QueryString["fileUrl"]);
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                    ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                    if (contentInfo != null && !string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl)))
                    {
                        //string fileUrl = contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl);
                        if (publishmentSystemInfo.Additional.IsCountDownload)
                        {
                            CountManager.AddCount(AppManager.CMS.AppID, tableName, contentID.ToString(), ECountType.Download);
                        }

                        if (PageUtils.IsProtocolUrl(fileUrl))
                        {
                            isSuccess = true;
                            PageUtils.Redirect(fileUrl);
                        }
                        else
                        {
                            string filePath = PathUtility.MapPath(publishmentSystemInfo, fileUrl, true);
                            EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
                            if (EFileSystemTypeUtils.IsDownload(fileType))
                            {
                                if (FileUtils.IsFileExists(filePath))
                                {
                                    isSuccess = true;
                                    PageUtils.Download(base.Response, filePath);
                                }
                            }
                            else
                            {
                                isSuccess = true;
                                PageUtils.Redirect(PageUtility.ParseNavigationUrl(publishmentSystemInfo, fileUrl));
                            }
                        }
                    }
                }
            }
            catch { }
            if (!isSuccess)
            {
                base.Response.Write("下载失败，不存在此文件！");
            }
        }

        private void Redirect_DefaultDirection()
        {
            string url;
            if (base.Request.QueryString["ErrorUrl"] != null)
            {
                url = base.Request.QueryString["ErrorUrl"];
                url = PageUtils.ParseNavigationUrl(url);
                PageUtils.Redirect(url);
            }
            int publishmentSystemID = PathUtility.GetCurrentPublishmentSystemID();
            if (publishmentSystemID == 0)
            {
                publishmentSystemID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByIsHeadquarters();
            }
            if (publishmentSystemID != 0)
            {
                url = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID).PublishmentSystemUrl;
                FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                string filePath = System.IO.Path.Combine(FSO.PublishmentSystemPath, "Utility\\RedirectError.aspx");
                if (System.IO.File.Exists(filePath))
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (string key in base.Request.QueryString.Keys)
                    {
                        builder.Append(string.Format("&{0}={1}", key, base.Request.QueryString[key]));
                    }
                    if (builder.Length > 0)
                    {
                        builder.Remove(0, 1);
                    }
                    url = url + string.Format("/Utility/RedirectError.aspx?{0}", builder);
                }
                PageUtils.Redirect(url);
            }
            else
            {
                url = ConfigUtils.Instance.ApplicationPath;
                PageUtils.Redirect(url);
            }
        }
    }
}
