using System;
using System.IO;
using System.Collections;
using System.Web;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;


using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core
{
    public class PageUtility
    {
        private PageUtility()
        {
        }

        //public static string GetPublishmentSystemUrl(PublishmentSystemInfo publishmentSystemInfo, string requestPath, EVisualType visualType)
        //{
        //    string navigationUrl = GetPublishmentSystemUrl(publishmentSystemInfo, requestPath);
        //    return ParseVisualUrl(navigationUrl, visualType);
        //}

        public static string GetPublishmentSystemUrl(PublishmentSystemInfo publishmentSystemInfo, string requestPath)
        {
            return GetPublishmentSystemUrl(publishmentSystemInfo, requestPath, false);
        }

        public static string GetPublishmentSystemUrl(PublishmentSystemInfo publishmentSystemInfo, string requestPath, bool isFromBackground)
        {
            string url = string.Empty;

            if (isFromBackground)
            {
                if (publishmentSystemInfo.Additional.IsMultiDeployment)
                {
                    url = publishmentSystemInfo.Additional.InnerUrl;
                }
            }
            if (string.IsNullOrEmpty(url))
            {
                url = publishmentSystemInfo.PublishmentSystemUrl;
            }

            if (string.IsNullOrEmpty(url))
            {
                url = "/";
            }
            else
            {
                if (url != "/" && url.EndsWith("/"))
                {
                    url = url.Substring(0, url.Length - 1);
                }
            }

            if (!string.IsNullOrEmpty(requestPath))
            {
                requestPath = PathUtils.RemovePathInvalidChar(requestPath);
                if (requestPath.StartsWith("/"))
                {
                    requestPath = requestPath.Substring(1);
                }

                if (!string.IsNullOrEmpty(publishmentSystemInfo.Additional.IISDefaultPage))
                {
                    if (StringUtils.EqualsIgnoreCase(PageUtils.GetFileNameFromUrl(requestPath), publishmentSystemInfo.Additional.IISDefaultPage))
                    {
                        requestPath = requestPath.Substring(0, requestPath.Length - publishmentSystemInfo.Additional.IISDefaultPage.Length);
                        if (!requestPath.EndsWith("/"))
                        {
                            requestPath += "/";
                        }
                    }
                }

                url = PageUtils.Combine(url, requestPath);
            }
            return url;
        }

        /****获取编辑器中内容，解析@符号，添加了远程路径处理 20151103****/
        public static string GetPublishmentSystemUrlForEditorUploadFilePre(PublishmentSystemInfo publishmentSystemInfo, string requestPath, bool isFromBackground)
        {
            string url = string.Empty;

            if (isFromBackground)
            {
                if (publishmentSystemInfo.Additional.IsMultiDeployment)
                {
                    url = publishmentSystemInfo.Additional.InnerUrl;
                }
            }
            else if (requestPath.StartsWith("@/upload") || requestPath.StartsWith("/upload") || requestPath.StartsWith("@\\upload") || requestPath.StartsWith("\\upload"))
            {
                url = publishmentSystemInfo.Additional.EditorUploadFilePre;
            }
            if (string.IsNullOrEmpty(url))
            {
                url = publishmentSystemInfo.PublishmentSystemUrl;
            }

            if (string.IsNullOrEmpty(url))
            {
                url = "/";
            }
            else
            {
                if (url != "/" && url.EndsWith("/"))
                {
                    url = url.Substring(0, url.Length - 1);
                }
            }

            if (!string.IsNullOrEmpty(requestPath))
            {
                requestPath = PathUtils.RemovePathInvalidChar(requestPath);
                if (requestPath.StartsWith("/"))
                {
                    requestPath = requestPath.Substring(1);
                }

                if (!string.IsNullOrEmpty(publishmentSystemInfo.Additional.IISDefaultPage))
                {
                    if (StringUtils.EqualsIgnoreCase(PageUtils.GetFileNameFromUrl(requestPath), publishmentSystemInfo.Additional.IISDefaultPage))
                    {
                        requestPath = requestPath.Substring(0, requestPath.Length - publishmentSystemInfo.Additional.IISDefaultPage.Length);
                        if (!requestPath.EndsWith("/"))
                        {
                            requestPath += "/";
                        }
                    }
                }

                url = PageUtils.Combine(url, requestPath);
            }
            return url;
        }

        //public static string GetPublishmentSystemUrlByPhysicalPath(PublishmentSystemInfo publishmentSystemInfo, string physicalPath, EVisualType visualType)
        //{
        //    string navigationUrl = GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, physicalPath);
        //    return ParseVisualUrl(navigationUrl, visualType);
        //}

        //返回代码类似/dev/site/images/pic.jpg
        public static string GetPublishmentSystemUrlByPhysicalPath(PublishmentSystemInfo publishmentSystemInfo, string physicalPath)
        {
            if (publishmentSystemInfo == null)
            {
                int publishmentSystemID = PathUtility.GetCurrentPublishmentSystemID();
                publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            }
            if (!string.IsNullOrEmpty(physicalPath))
            {
                string publishmentSystemPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);
                string requestPath = string.Empty;
                if (physicalPath.StartsWith(publishmentSystemPath))
                {
                    requestPath = StringUtils.ReplaceStartsWith(physicalPath, publishmentSystemPath, string.Empty);
                }
                else
                {
                    requestPath = physicalPath.ToLower().Replace(publishmentSystemPath.ToLower(), string.Empty);
                }
                requestPath = requestPath.Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar);
                return GetPublishmentSystemUrl(publishmentSystemInfo, requestPath);
            }
            else
            {
                return publishmentSystemInfo.PublishmentSystemUrl;
            }
        }

        //level=0代表应用根目录，1代表下一级目标。。。返回代码类似../images/pic.jpg
        public static string GetPublishmentSystemUrlOfRelatedByPhysicalPath(PublishmentSystemInfo publishmentSystemInfo, string physicalPath, int level)
        {
            if (publishmentSystemInfo == null)
            {
                int publishmentSystemID = PathUtility.GetCurrentPublishmentSystemID();
                publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            }
            if (!string.IsNullOrEmpty(physicalPath))
            {
                string publishmentSystemPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);
                string requestPath = physicalPath.ToLower().Replace(publishmentSystemPath.ToLower(), string.Empty);
                requestPath = requestPath.Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar);
                requestPath = requestPath.Trim(PageUtils.SeparatorChar);
                if (level > 0)
                {
                    for (int i = 0; i < level; i++)
                    {
                        requestPath = "../" + requestPath;
                    }
                }
                return requestPath;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetPublishmentSystemVirtualUrlByPhysicalPath(PublishmentSystemInfo publishmentSystemInfo, string physicalPath)
        {
            if (publishmentSystemInfo == null)
            {
                int publishmentSystemID = PathUtility.GetCurrentPublishmentSystemID();
                publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            }
            if (!string.IsNullOrEmpty(physicalPath))
            {
                string publishmentSystemPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);
                string requestPath = physicalPath.ToLower().Replace(publishmentSystemPath.ToLower(), string.Empty);
                requestPath = requestPath.Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar);
                return PageUtils.Combine("@", requestPath);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetPublishmentSystemVirtualUrlByAbsoluteUrl(PublishmentSystemInfo publishmentSystemInfo, string absoluteUrl)
        {
            if (publishmentSystemInfo == null)
            {
                int publishmentSystemID = PathUtility.GetCurrentPublishmentSystemID();
                publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            }
            if (!string.IsNullOrEmpty(absoluteUrl))
            {
                if (PageUtils.IsProtocolUrl(absoluteUrl) || absoluteUrl.StartsWith("/"))
                {
                    absoluteUrl = absoluteUrl.ToLower();
                    string publishmentSystemUrl = PageUtility.GetPublishmentSystemUrl(publishmentSystemInfo, string.Empty).ToLower();

                    if (PageUtils.IsProtocolUrl(absoluteUrl))
                    {
                        publishmentSystemUrl = PageUtils.AddProtocolToUrl(publishmentSystemUrl);
                    }

                    absoluteUrl = StringUtils.ReplaceFirst(publishmentSystemUrl, absoluteUrl, string.Empty);
                }
                else if (absoluteUrl.StartsWith("."))
                {
                    absoluteUrl = absoluteUrl.Replace("../", string.Empty);
                    absoluteUrl = absoluteUrl.Replace("./", string.Empty);
                }
                return PageUtils.Combine("@", absoluteUrl);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 从设置信息中的RootUrl中取得地址，并结合发布系统和相对地址，得到能够运行动态文件的地址。
        /// </summary>
        public static string GetRootUrlByPublishmentSystemID(PublishmentSystemInfo psInfo, string requestPath)
        {
            if (psInfo == null)
            {
                int publishmentSystemID = PathUtility.GetCurrentPublishmentSystemID();
                psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            }
            //string url = ConfigUtils.Instance.ApplicationPath;
            string url = PageUtils.GetRootUrl(string.Empty);

            if (url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }
            url += "/" + psInfo.PublishmentSystemDir;

            if (requestPath != null && requestPath.Trim().Length > 0)
            {
                if (requestPath.StartsWith("/"))
                {
                    requestPath = requestPath.Substring(1);
                }
                if (requestPath.EndsWith("/"))
                {
                    requestPath = requestPath.Substring(0, requestPath.Length - 1);
                }
                url = url + "/" + requestPath;
            }
            return url;
        }

        public static string GetSiteFilesUrl(PublishmentSystemInfo publishmentSystemInfo, string relatedUrl)
        {
            //if (publishmentSystemInfo.Additional.FuncFilesType == EFuncFilesType.Direct)
            //{
            //    return PageUtils.GetSiteFilesUrl(relatedUrl);
            //}
            //else if (publishmentSystemInfo.Additional.FuncFilesType == EFuncFilesType.CrossDomain)
            //{
            //    return PageUtils.ParseConfigRootUrl(PageUtils.GetAbsoluteSiteFilesUrl(relatedUrl));
            //}
            //else
            //{
            //    return PageUtility.GetPublishmentSystemUrl(publishmentSystemInfo, PageUtils.Combine(DirectoryUtils.SiteFiles.DirectoryName, relatedUrl));
            //}

            if (PageUtils.IsProtocolUrl(publishmentSystemInfo.PublishmentSystemUrl))
            {
                return PageUtils.AddProtocolToUrl(PageUtils.ParseNavigationUrl(PageUtils.GetAbsoluteSiteFilesUrl(relatedUrl)));
            }
            else
            {
                return PageUtils.GetSiteFilesUrl(relatedUrl);
            }
        }

        /// <summary>
        /// 是否跨域
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        /// <returns></returns>
        public static bool IsCrossDomain(PublishmentSystemInfo publishmentSystemInfo)
        {
            return IsCorsCrossDomain(publishmentSystemInfo) || IsAgentCrossDomain(publishmentSystemInfo);
        }

        /// <summary>
        /// CORS跨域资源共享
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        /// <returns></returns>
        public static bool IsCorsCrossDomain(PublishmentSystemInfo publishmentSystemInfo)
        {
            return publishmentSystemInfo.Additional.FuncFilesType == EFuncFilesType.Cors;
        }
        /// <summary>
        /// 代理跨域
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        /// <returns></returns>
        public static bool IsAgentCrossDomain(PublishmentSystemInfo publishmentSystemInfo)
        {
            return publishmentSystemInfo.Additional.FuncFilesType == EFuncFilesType.CrossDomain;
        }

        public static string GetIconUrl(PublishmentSystemInfo publishmentSystemInfo, string relatedUrl)
        {
            return GetSiteFilesUrl(publishmentSystemInfo, PageUtils.Combine(SiteFiles.Directory.Icons, relatedUrl));
        }

        //public static string GetStlIconUrl(string iconName)
        //{
        //    return PageUtils.ParseConfigRootUrl(SiteFilesAbsolute.Icons.GetIcon(iconName));
        //}

        public static string GetProxyUrl(PublishmentSystemInfo publishmentSystemInfo, string script)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes("PlMqAzuE");
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes("PlMqAzuE");

            System.Security.Cryptography.DESCryptoServiceProvider cryptoProvider = new System.Security.Cryptography.DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Security.Cryptography.CryptoStream cst = new System.Security.Cryptography.CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), System.Security.Cryptography.CryptoStreamMode.Write);

            System.IO.StreamWriter sw = new System.IO.StreamWriter(cst);
            sw.Write(script);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();

            string callback = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            callback = callback.Replace("+", "0add0").Replace("=", "0equals0").Replace("&", "0and0").Replace("?", "0question0").Replace("'", "0quote0").Replace("/", "0slash0");

            return PageUtility.ParseNavigationUrl(publishmentSystemInfo, string.Format("@/utils/proxy.aspx?callback={0}", callback));
        }

        public static string GetSiteTemplatesUrl(string relatedUrl)
        {
            return PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.SiteTemplates, relatedUrl);
        }

        public static string GetSiteTemplateMetadataUrl(string siteTemplateUrl, string relatedUrl)
        {
            return PageUtils.Combine(siteTemplateUrl, DirectoryUtils.SiteTemplates.SiteTemplateMetadata, relatedUrl);
        }

        public static string GetIndependentTemplatesUrl(string relatedUrl)
        {
            return PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.IndependentTemplates, relatedUrl);
        }

        public static string GetIndependentTemplateMetadataUrl(string siteTemplateUrl, string relatedUrl)
        {
            return PageUtils.Combine(siteTemplateUrl, DirectoryUtils.IndependentTemplates.IndependentTemplateMetadata, relatedUrl);
        }

        public static string GetIndexPageUrl(PublishmentSystemInfo publishmentSystemInfo, EVisualType visualType)
        {
            return GetIndexPageUrl(publishmentSystemInfo, false, visualType);
        }

        // 得到发布系统首页地址
        public static string GetIndexPageUrl(PublishmentSystemInfo publishmentSystemInfo, bool isInner, EVisualType visualType)
        {
            if (visualType == EVisualType.Static && !string.IsNullOrEmpty(publishmentSystemInfo.PublishmentSystemUrl) && !isInner)
            {
                return publishmentSystemInfo.PublishmentSystemUrl;
            }

            int indexTemplateID = TemplateManager.GetIndexTempalteID(publishmentSystemInfo.PublishmentSystemID);
            string createdFileFullName = TemplateManager.GetCreatedFileFullName(publishmentSystemInfo.PublishmentSystemID, indexTemplateID);
            if (visualType == EVisualType.Static)
            {
                return PageUtility.ParseNavigationUrl(publishmentSystemInfo, createdFileFullName, isInner);
            }
            else
            {
                return DynamicPage.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, 0, 0, 0, 0);
            }
        }

        public static string GetFileUrl(PublishmentSystemInfo publishmentSystemInfo, int fileTemplateID, EVisualType visualType)
        {
            string createdFileFullName = TemplateManager.GetCreatedFileFullName(publishmentSystemInfo.PublishmentSystemID, fileTemplateID);
            if (visualType == EVisualType.Static)
            {
                return PageUtility.ParseNavigationUrl(publishmentSystemInfo, createdFileFullName);
            }
            else
            {
                return DynamicPage.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, 0, 0, fileTemplateID, 0);
            }
        }

        public static string GetContentUrl(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, int contentID, EVisualType visualType)
        {
            return GetContentUrl(publishmentSystemInfo, nodeInfo, contentID, false, visualType);
        }

        public static string GetContentUrl(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, int contentID, bool isInner, EVisualType visualType)
        {
            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
            return PageUtility.GetContentUrlByID(publishmentSystemInfo, contentInfo, contentInfo.SourceID, contentInfo.ReferenceID, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.LinkUrl), isInner, visualType);
        }

        public static string GetContentUrl(PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo, EVisualType visualType)
        {
            return PageUtility.GetContentUrlByID(publishmentSystemInfo, contentInfo, contentInfo.SourceID, contentInfo.ReferenceID, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.LinkUrl), false, visualType);
        }

        public static string GetContentUrl(PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo, bool isInner, EVisualType visualType)
        {
            return PageUtility.GetContentUrlByID(publishmentSystemInfo, contentInfo, contentInfo.SourceID, contentInfo.ReferenceID, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.LinkUrl), isInner, visualType);
        }

        /// <summary>
        /// 对GetContentUrlByID的优化
        /// 通过传入参数contentInfoCurrent，避免对ContentInfo查询太多
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        /// <param name="contentInfoCurrent"></param>
        /// <param name="sourceID"></param>
        /// <param name="referenceID"></param>
        /// <param name="linkUrl"></param>
        /// <param name="isInner"></param>
        /// <param name="visualType"></param>
        /// <returns></returns>
        private static string GetContentUrlByID(PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfoCurrent, int sourceID, int referenceID, string linkUrl, bool isInner, EVisualType visualType)
        {
            int nodeID = contentInfoCurrent.NodeID;
            int contentID = contentInfoCurrent.ID;
            if (referenceID > 0 && contentInfoCurrent.GetExtendedAttribute(ContentAttribute.TranslateContentType) != ETranslateContentType.ReferenceContent.ToString())
            {
                if (sourceID > 0 && (NodeManager.IsExists(publishmentSystemInfo.PublishmentSystemID, sourceID) || NodeManager.IsExists(sourceID)))
                {
                    int targetNodeID = sourceID;
                    int targetPublishmentSystemID = DataProvider.NodeDAO.GetPublishmentSystemID(targetNodeID);
                    PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);
                    NodeInfo targetNodeInfo = NodeManager.GetNodeInfo(targetPublishmentSystemID, targetNodeID);

                    ETableStyle tableStyle = NodeManager.GetTableStyle(targetPublishmentSystemInfo, targetNodeInfo);
                    string tableName = NodeManager.GetTableName(targetPublishmentSystemInfo, targetNodeInfo);
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, referenceID);
                    if (contentInfo == null || contentInfo.NodeID <= 0)
                    {
                        return PageUtils.UNCLICKED_URL;
                    }
                    else if (contentInfo.PublishmentSystemID == targetPublishmentSystemInfo.PublishmentSystemID)
                    {
                        return PageUtility.GetContentUrlByID(targetPublishmentSystemInfo, contentInfo, contentInfo.SourceID, contentInfo.ReferenceID, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.LinkUrl), isInner, visualType);
                    }
                    else
                    {
                        PublishmentSystemInfo publishmentSystemInfoTmp = PublishmentSystemManager.GetPublishmentSystemInfo(contentInfo.PublishmentSystemID);
                        return PageUtility.GetContentUrlByID(publishmentSystemInfoTmp, contentInfo, contentInfo.SourceID, contentInfo.ReferenceID, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.LinkUrl), isInner, visualType);
                    }
                }
                else
                {
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                    nodeID = BaiRongDataProvider.ContentDAO.GetNodeID(tableName, referenceID);
                    linkUrl = BaiRongDataProvider.ContentDAO.GetValue(tableName, referenceID, BackgroundContentAttribute.LinkUrl);
                    if (NodeManager.IsExists(publishmentSystemInfo.PublishmentSystemID, nodeID))
                    {
                        return GetContentUrlByID(publishmentSystemInfo, nodeID, referenceID, 0, 0, linkUrl, isInner, visualType);
                    }
                    else
                    {
                        int targetPublishmentSystemID = DataProvider.NodeDAO.GetPublishmentSystemID(nodeID);
                        PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);
                        return GetContentUrlByID(targetPublishmentSystemInfo, nodeID, referenceID, 0, 0, linkUrl, isInner, visualType);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(linkUrl))
                {
                    return ParseNavigationUrl(publishmentSystemInfo, linkUrl, isInner);
                }
                else
                {
                    string contentUrl = PathUtility.ContentFilePathRules.Parse(publishmentSystemInfo, nodeID, contentInfoCurrent);
                    if (visualType == EVisualType.Static)
                    {
                        return PageUtility.GetPublishmentSystemUrl(publishmentSystemInfo, contentUrl, isInner);
                    }
                    else
                    {
                        return DynamicPage.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, nodeID, contentID, 0, 0);
                    }
                }
            }
        }


        private static string GetContentUrlByID(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID, int sourceID, int referenceID, string linkUrl, bool isInner, EVisualType visualType)
        {
            ETableStyle tableStyleCurrent = NodeManager.GetTableStyle(publishmentSystemInfo, nodeID);
            string tableNameCurrent = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
            ContentInfo contentInfoCurrent = DataProvider.ContentDAO.GetContentInfo(tableStyleCurrent, tableNameCurrent, contentID);

            if (referenceID > 0 && contentInfoCurrent.GetExtendedAttribute(ContentAttribute.TranslateContentType) != ETranslateContentType.ReferenceContent.ToString())
            {
                if (sourceID > 0 && (NodeManager.IsExists(publishmentSystemInfo.PublishmentSystemID, sourceID) || NodeManager.IsExists(sourceID)))
                {
                    int targetNodeID = sourceID;
                    int targetPublishmentSystemID = DataProvider.NodeDAO.GetPublishmentSystemID(targetNodeID);
                    PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);
                    NodeInfo targetNodeInfo = NodeManager.GetNodeInfo(targetPublishmentSystemID, targetNodeID);

                    ETableStyle tableStyle = NodeManager.GetTableStyle(targetPublishmentSystemInfo, targetNodeInfo);
                    string tableName = NodeManager.GetTableName(targetPublishmentSystemInfo, targetNodeInfo);
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, referenceID);
                    if (contentInfo == null || contentInfo.NodeID <= 0)
                    {
                        return PageUtils.UNCLICKED_URL;
                    }
                    else if (contentInfo.PublishmentSystemID == targetPublishmentSystemInfo.PublishmentSystemID)
                    {
                        return PageUtility.GetContentUrlByID(targetPublishmentSystemInfo, contentInfo.NodeID, contentInfo.ID, contentInfo.SourceID, contentInfo.ReferenceID, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.LinkUrl), isInner, visualType);
                    }
                    else
                    {
                        PublishmentSystemInfo publishmentSystemInfoTmp = PublishmentSystemManager.GetPublishmentSystemInfo(contentInfo.PublishmentSystemID);
                        return PageUtility.GetContentUrlByID(publishmentSystemInfoTmp, contentInfo.NodeID, contentInfo.ID, contentInfo.SourceID, contentInfo.ReferenceID, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.LinkUrl), isInner, visualType);
                    }
                }
                else
                {
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                    nodeID = BaiRongDataProvider.ContentDAO.GetNodeID(tableName, referenceID);
                    linkUrl = BaiRongDataProvider.ContentDAO.GetValue(tableName, referenceID, BackgroundContentAttribute.LinkUrl);
                    return GetContentUrlByID(publishmentSystemInfo, nodeID, referenceID, 0, 0, linkUrl, isInner, visualType);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(linkUrl))
                {
                    return ParseNavigationUrl(publishmentSystemInfo, linkUrl, isInner);
                }
                else
                {
                    string contentUrl = PathUtility.ContentFilePathRules.Parse(publishmentSystemInfo, nodeID, contentID);
                    if (visualType == EVisualType.Static)
                    {
                        return PageUtility.GetPublishmentSystemUrl(publishmentSystemInfo, contentUrl, isInner);
                    }
                    else
                    {
                        return DynamicPage.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, nodeID, contentID, 0, 0);
                    }
                }
            }
        }

        private static string GetChannelUrlNotComputed(PublishmentSystemInfo publishmentSystemInfo, int nodeID, ENodeType nodeType, bool isInner, EVisualType visualType)
        {
            if (nodeType == ENodeType.BackgroundPublishNode)
            {
                return PageUtility.GetIndexPageUrl(publishmentSystemInfo, isInner, visualType);
            }
            string linkUrl = string.Empty;
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
            if (nodeInfo != null)
            {
                linkUrl = nodeInfo.LinkUrl;
            }

            if (string.IsNullOrEmpty(linkUrl))
            {
                if (visualType == EVisualType.Static)
                {
                    string filePath = nodeInfo.FilePath;

                    if (string.IsNullOrEmpty(filePath))
                    {
                        string channelUrl = PathUtility.ChannelFilePathRules.Parse(publishmentSystemInfo, nodeID);
                        return PageUtility.GetPublishmentSystemUrl(publishmentSystemInfo, channelUrl, isInner);
                    }
                    else
                    {
                        return PageUtility.ParseNavigationUrl(publishmentSystemInfo, PathUtility.AddVirtualToPath(filePath), isInner);
                    }
                }
                else
                {
                    return DynamicPage.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, nodeID, 0, 0, 0);
                }
            }
            else
            {
                return ParseNavigationUrl(publishmentSystemInfo, linkUrl, isInner);
            }
        }

        public static string GetChannelUrl(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, EVisualType visualType)
        {
            return GetChannelUrl(publishmentSystemInfo, nodeInfo, false, visualType);
        }

        //得到栏目经过计算后的连接地址
        public static string GetChannelUrl(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, bool isInner, EVisualType visualType)
        {
            string url = string.Empty;
            if (nodeInfo != null)
            {
                if (nodeInfo.NodeType == ENodeType.BackgroundPublishNode)
                {
                    url = PageUtility.GetChannelUrlNotComputed(publishmentSystemInfo, nodeInfo.NodeID, nodeInfo.NodeType, isInner, visualType);
                }
                else
                {
                    if (nodeInfo.LinkType == ELinkType.LinkNoRelatedToChannelAndContent)
                    {
                        url = PageUtility.GetChannelUrlNotComputed(publishmentSystemInfo, nodeInfo.NodeID, nodeInfo.NodeType, isInner, visualType);
                    }
                    else if (nodeInfo.LinkType == ELinkType.NoLink)
                    {
                        url = PageUtils.UNCLICKED_URL;
                    }
                    else
                    {
                        if (nodeInfo.LinkType == ELinkType.NoLinkIfContentNotExists)
                        {
                            if (nodeInfo.ContentNum == 0)
                            {
                                url = PageUtils.UNCLICKED_URL;
                            }
                            else
                            {
                                url = PageUtility.GetChannelUrlNotComputed(publishmentSystemInfo, nodeInfo.NodeID, nodeInfo.NodeType, isInner, visualType);
                            }
                        }
                        else if (nodeInfo.LinkType == ELinkType.LinkToOnlyOneContent)
                        {
                            if (nodeInfo.ContentNum == 1)
                            {
                                int contentID = CreateCacheManager.FirstContentID.GetValue(publishmentSystemInfo, nodeInfo);
                                url = PageUtility.GetContentUrl(publishmentSystemInfo, nodeInfo, contentID, visualType);
                            }
                            else
                            {
                                url = PageUtility.GetChannelUrlNotComputed(publishmentSystemInfo, nodeInfo.NodeID, nodeInfo.NodeType, isInner, visualType);
                            }
                        }
                        else if (nodeInfo.LinkType == ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent)
                        {
                            if (nodeInfo.ContentNum == 0)
                            {
                                url = PageUtils.UNCLICKED_URL;
                            }
                            else if (nodeInfo.ContentNum == 1)
                            {
                                int contentID = CreateCacheManager.FirstContentID.GetValue(publishmentSystemInfo, nodeInfo);
                                url = PageUtility.GetContentUrl(publishmentSystemInfo, nodeInfo, contentID, visualType);
                            }
                            else
                            {
                                url = PageUtility.GetChannelUrlNotComputed(publishmentSystemInfo, nodeInfo.NodeID, nodeInfo.NodeType, isInner, visualType);
                            }
                        }
                        else if (nodeInfo.LinkType == ELinkType.LinkToFirstContent)
                        {
                            if (nodeInfo.ContentNum >= 1)
                            {
                                int contentID = CreateCacheManager.FirstContentID.GetValue(publishmentSystemInfo, nodeInfo);
                                url = PageUtility.GetContentUrl(publishmentSystemInfo, nodeInfo, contentID, visualType);
                            }
                            else
                            {
                                url = PageUtility.GetChannelUrlNotComputed(publishmentSystemInfo, nodeInfo.NodeID, nodeInfo.NodeType, isInner, visualType);
                            }
                        }
                        else if (nodeInfo.LinkType == ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent)
                        {
                            if (nodeInfo.ContentNum >= 1)
                            {
                                int contentID = CreateCacheManager.FirstContentID.GetValue(publishmentSystemInfo, nodeInfo);
                                url = PageUtility.GetContentUrl(publishmentSystemInfo, nodeInfo, contentID, visualType);
                            }
                            else
                            {
                                url = PageUtils.UNCLICKED_URL;
                            }
                        }
                        else if (nodeInfo.LinkType == ELinkType.NoLinkIfChannelNotExists)
                        {
                            if (nodeInfo.ChildrenCount == 0)
                            {
                                url = PageUtils.UNCLICKED_URL;
                            }
                            else
                            {
                                url = PageUtility.GetChannelUrlNotComputed(publishmentSystemInfo, nodeInfo.NodeID, nodeInfo.NodeType, isInner, visualType);
                            }
                        }
                        else if (nodeInfo.LinkType == ELinkType.LinkToLastAddChannel)
                        {
                            NodeInfo lastAddNodeInfo = DataProvider.NodeDAO.GetNodeInfoByLastAddDate(nodeInfo.NodeID);
                            if (lastAddNodeInfo != null)
                            {
                                url = PageUtility.GetChannelUrl(publishmentSystemInfo, lastAddNodeInfo, isInner, visualType);
                            }
                            else
                            {
                                url = PageUtility.GetChannelUrlNotComputed(publishmentSystemInfo, nodeInfo.NodeID, nodeInfo.NodeType, isInner, visualType);
                            }
                        }
                        else if (nodeInfo.LinkType == ELinkType.LinkToFirstChannel)
                        {
                            NodeInfo firstNodeInfo = DataProvider.NodeDAO.GetNodeInfoByTaxis(nodeInfo.NodeID);
                            if (firstNodeInfo != null)
                            {
                                url = PageUtility.GetChannelUrl(publishmentSystemInfo, firstNodeInfo, isInner, visualType);
                            }
                            else
                            {
                                url = PageUtility.GetChannelUrlNotComputed(publishmentSystemInfo, nodeInfo.NodeID, nodeInfo.NodeType, isInner, visualType);
                            }
                        }
                        else if (nodeInfo.LinkType == ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel)
                        {
                            NodeInfo lastAddNodeInfo = DataProvider.NodeDAO.GetNodeInfoByLastAddDate(nodeInfo.NodeID);
                            if (lastAddNodeInfo != null)
                            {
                                url = PageUtility.GetChannelUrl(publishmentSystemInfo, lastAddNodeInfo, isInner, visualType);
                            }
                            else
                            {
                                url = PageUtils.UNCLICKED_URL;
                            }
                        }
                        else if (nodeInfo.LinkType == ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel)
                        {
                            NodeInfo firstNodeInfo = DataProvider.NodeDAO.GetNodeInfoByTaxis(nodeInfo.NodeID);
                            if (firstNodeInfo != null)
                            {
                                url = PageUtility.GetChannelUrl(publishmentSystemInfo, firstNodeInfo, isInner, visualType);
                            }
                            else
                            {
                                url = PageUtils.UNCLICKED_URL;
                            }
                        }
                    }
                }
            }
            return url;
        }

        public static string GetInputChannelUrl(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, EVisualType visualType)
        {
            string channelUrl = GetChannelUrl(publishmentSystemInfo, nodeInfo, visualType);
            if (!string.IsNullOrEmpty(channelUrl))
            {
                channelUrl = StringUtils.ReplaceStartsWith(channelUrl, publishmentSystemInfo.PublishmentSystemUrl, string.Empty);
                channelUrl = channelUrl.Trim('/');
                channelUrl = "/" + channelUrl;
            }
            return channelUrl;
        }

        public static string AddVirtualToUrl(string url)
        {
            string resolvedUrl = url;
            if (!string.IsNullOrEmpty(url) && !PageUtils.IsProtocolUrl(url))
            {
                if (!url.StartsWith("@") && !url.StartsWith("~"))
                {
                    resolvedUrl = PageUtils.Combine("@/", url);
                }
            }
            return resolvedUrl;
        }

        public static string ParseNavigationUrlAddPrefix(PublishmentSystemInfo publishmentSystemInfo, string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (!url.StartsWith("~/") && !url.StartsWith("@/"))
                {
                    url = "@/" + url;
                }
            }
            return ParseNavigationUrl(publishmentSystemInfo, url);
        }

        public static string ParseNavigationUrl(int publishmentSystemID, string url)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            return ParseNavigationUrl(publishmentSystemInfo, url, false);
        }

        public static string ParseNavigationUrl(PublishmentSystemInfo publishmentSystemInfo, string url)
        {
            return ParseNavigationUrl(publishmentSystemInfo, url, false);
        }

        //根据发布系统属性判断是否为相对路径并返回解析后路径
        public static string ParseNavigationUrl(PublishmentSystemInfo publishmentSystemInfo, string url, bool isInner)
        {
            if (publishmentSystemInfo != null)
            {
                if (!string.IsNullOrEmpty(url) && url.StartsWith("@"))
                {
                    string extensionName = PathUtils.GetExtension(url).ToLower();
                    //如果设置编辑器上传文件URL前缀,排除单页html,只允许file,image,video
                    if ((url.StartsWith("@/upload") || url.StartsWith("/upload") || url.StartsWith("@\\upload") || url.StartsWith("\\upload"))
                        && !string.IsNullOrEmpty(publishmentSystemInfo.Additional.EditorUploadFilePre)
                        && (PathUtility.IsImageExtenstionAllowed(publishmentSystemInfo, extensionName)
                        || PathUtility.IsFileExtenstionAllowed(publishmentSystemInfo, extensionName)
                        || PathUtility.IsVideoExtenstionAllowed(publishmentSystemInfo, extensionName)))
                    {
                        /****获取编辑器中内容，解析@符号，添加了远程路径处理 20151103****/
                        return GetPublishmentSystemUrlForEditorUploadFilePre(publishmentSystemInfo, url.Substring(1), isInner);
                    }
                    else
                    {
                        return GetPublishmentSystemUrl(publishmentSystemInfo, url.Substring(1), isInner);
                    }
                }
                else
                {
                    //if (publishmentSystemInfo.IsRelatedUrl)
                    //{
                    //    return PageUtils.ParseNavigationUrl(url);
                    //}
                    //else
                    //{
                    return PageUtils.ParseNavigationUrl(url, ConfigUtils.Instance.ApplicationPath);
                    //}
                }
            }
            else
            {
                return PageUtils.ParseNavigationUrl(url);
            }
        }

        //public static string ParseVisualUrl(string rawUrl, EVisualType visualType)
        //{
        //    if (visualType == EVisualType.Static) return rawUrl;
        //    if (!string.IsNullOrEmpty(rawUrl))
        //    {
        //        string extension = PageUtils.GetExtensionFromUrl(rawUrl);
        //        if (!string.IsNullOrEmpty(extension))
        //        {
        //            if (visualType == EVisualType.Dynamic)
        //            {
        //                return rawUrl.Substring(0, rawUrl.Length - extension.Length) + "_v.aspx";
        //            }
        //        }
        //    }
        //    return rawUrl;
        //}

        //public static string GetFileSystemManagementDirectoryUrl(string currentRootPath, string publishementSystemDir)
        //{
        //    string directoryUrl = string.Empty;
        //    if (string.IsNullOrEmpty(currentRootPath) || !(currentRootPath.StartsWith("~/") || currentRootPath.StartsWith("@/")))
        //    {
        //        currentRootPath = "@/" + currentRootPath;
        //    }
        //    string[] directoryNames = currentRootPath.Split('/');
        //    foreach (string directoryName in directoryNames)
        //    {
        //        if (!string.IsNullOrEmpty(directoryName))
        //        {
        //            if (directoryName.Equals("~"))
        //            {
        //                directoryUrl = ConfigUtils.Instance.ApplicationPath;
        //            }
        //            else if (directoryName.Equals("@"))
        //            {
        //                directoryUrl = PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, publishementSystemDir);
        //            }
        //            else
        //            {
        //                directoryUrl = PageUtils.Combine(directoryUrl, directoryName);
        //            }
        //        }
        //    }
        //    return directoryUrl;
        //}

        public static string GetResumePreviewUrl(int publishmentSystemID, int contentID)
        {
            if (contentID == 0)
            {
                return PageUtility.Services.GetUrl(string.Format("resume/preview.aspx?publishmentSystemID={0}", publishmentSystemID));
            }
            else
            {
                return PageUtility.Services.GetUrl(string.Format("resume/preview.aspx?publishmentSystemID={0}&contentID={1}", publishmentSystemID, contentID));
            }
        }

        public static string GetCMSServiceUrlByPage(string serviceName, string methodName)
        {
            return PageUtils.ParseNavigationUrl(string.Format("~/{0}/cms/services/{1}.aspx?type={2}", FileConfigManager.Instance.AdminDirectoryName, serviceName, methodName));
        }

        public static string GetSTLServiceUrlByPage(string serviceName, string methodName)
        {
            return PageUtils.ParseNavigationUrl(string.Format("~/{0}/stl/services/{1}.aspx?type={2}", FileConfigManager.Instance.AdminDirectoryName, serviceName, methodName));
        }

        public static string GetTextEditorUrl(int publishmentSystemID, ETextEditorType editorType, bool isBackground, out string snapHostUrl, out string uploadImageUrl, out string uploadScrawlUrl, out string uploadFileUrl, out string imageManagerUrl, out string getMovieUrl)
        {
            snapHostUrl = PageUtils.GetHost();
            uploadImageUrl = PageUtils.GetCMSUrl(string.Format("background_textEditorUpload.aspx?PublishmentSystemID={0}&IsBackground={1}&EditorType={2}&FileType={3}", publishmentSystemID, isBackground, ETextEditorTypeUtils.GetValue(editorType), "Image"));
            uploadScrawlUrl = PageUtils.GetCMSUrl(string.Format("background_textEditorUpload.aspx?PublishmentSystemID={0}&IsBackground={1}&EditorType={2}&FileType={3}", publishmentSystemID, isBackground, ETextEditorTypeUtils.GetValue(editorType), "Scrawl"));
            uploadFileUrl = PageUtils.GetCMSUrl(string.Format("background_textEditorUpload.aspx?PublishmentSystemID={0}&IsBackground={1}&EditorType={2}&FileType={3}", publishmentSystemID, isBackground, ETextEditorTypeUtils.GetValue(editorType), "File"));
            imageManagerUrl = PageUtils.GetCMSUrl(string.Format("background_textEditorUpload.aspx?PublishmentSystemID={0}&IsBackground={1}&EditorType={2}&FileType={3}", publishmentSystemID, isBackground, ETextEditorTypeUtils.GetValue(editorType), "ImageManager"));
            getMovieUrl = PageUtils.GetCMSUrl(string.Format("background_textEditorUpload.aspx?PublishmentSystemID={0}&IsBackground={1}&EditorType={2}&FileType={3}", publishmentSystemID, isBackground, ETextEditorTypeUtils.GetValue(editorType), "GetMovie"));

            return PageUtils.GetSiteFilesUrl(PageUtils.Combine("bairong/texteditor", ETextEditorTypeUtils.GetValue(editorType)));
        }

        public static string GetVirtualUrl(PublishmentSystemInfo publishmentSystemInfo, string url)
        {
            string virtualUrl = StringUtils.ReplaceStartsWith(url, publishmentSystemInfo.PublishmentSystemUrl, "@/");
            return StringUtils.ReplaceStartsWith(virtualUrl, "@//", "@/");
        }

        public static bool IsVirtualUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (url.StartsWith("~") || url.StartsWith("@"))
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetContentPreviewUrl(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID)
        {
            //return PageUtility.GetRootUrlByPublishmentSystemID(publishmentSystemInfo, string.Format("contents/{0}/preview_v.aspx?isPreview=true&templateType=ContentTemplate&publishmentSystemID={1}&previewID={2}", channelID, publishmentSystemInfo.PublishmentSystemID, contentID));

            string pageUrl = DynamicPage.GetRedirectUrl(publishmentSystemInfo.PublishmentSystemID, channelID, contentID, 0, 0);
            return DynamicPage.GetPreviewUrl(pageUrl, contentID);
        }

        public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments, int width, int height)
        {
            return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetCMSUrl(pageUrl), arguments, width, height);
        }
        public static string GetOpenWindowStringByHome(string title, string pageUrl, NameValueCollection arguments, int width, int height)
        {
            return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.Combine("/Home/modal", pageUrl), arguments, width, height);
        }

        public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments)
        {
            return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetCMSUrl(pageUrl), arguments);
        }

        public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetCMSUrl(pageUrl), arguments, isCloseOnly);
        }

        public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments, int width, int height, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetCMSUrl(pageUrl), arguments, width, height, isCloseOnly);
        }

        public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTextBoxValue(title, PageUtils.GetCMSUrl(pageUrl), arguments, textBoxID);
        }

        public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID, int width, int height)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTextBoxValue(title, PageUtils.GetCMSUrl(pageUrl), arguments, textBoxID, width, height);
        }

        public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTextBoxValue(title, PageUtils.GetCMSUrl(pageUrl), arguments, textBoxID, isCloseOnly);
        }

        public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID, int width, int height, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTextBoxValue(title, PageUtils.GetCMSUrl(pageUrl), arguments, textBoxID, width, height, isCloseOnly);
        }

        public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, int width, int height)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue(title, PageUtils.GetCMSUrl(pageUrl), arguments, checkBoxID, alertText, width, height);
        }

        public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue(title, PageUtils.GetCMSUrl(pageUrl), arguments, checkBoxID, alertText);
        }

        public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue(title, PageUtils.GetCMSUrl(pageUrl), arguments, checkBoxID, alertText, isCloseOnly);
        }

        public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, int width, int height, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue(title, PageUtils.GetCMSUrl(pageUrl), arguments, checkBoxID, alertText, width, height, isCloseOnly);
        }

        public static string GetOpenWindowStringWithTwoCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID1, string checkBoxID2, string alertText, int width, int height)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTwoCheckBoxValue(title, PageUtils.GetCMSUrl(pageUrl), arguments, checkBoxID1, checkBoxID2, alertText, width, height);
        }

        public static string GetOpenWindowStringWithTwoCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID1, string checkBoxID2, string alertText, int width, int height, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTwoCheckBoxValue(title, PageUtils.GetCMSUrl(pageUrl), arguments, checkBoxID1, checkBoxID2, alertText, width, height, isCloseOnly);
        }

        public class FSOAjaxUrl
        {
            public static string GetUrlCreateContent(int publishmentSystemID, int channelID, int contentID)
            {
                return PageUtils.GetSTLUrl(string.Format("background_serviceSTL.aspx?type={0}&method={1}&publishmentSystemID={2}&channelID={3}&contentID={4}", "AjaxUrlFSO", "CreateContent", publishmentSystemID, channelID, contentID));
            }

            public static string GetUrlCreateChannel(int publishmentSystemID, int channelID)
            {
                return PageUtils.GetSTLUrl(string.Format("background_serviceSTL.aspx?type={0}&method={1}&publishmentSystemID={2}&channelID={3}", "AjaxUrlFSO", "CreateChannel", publishmentSystemID, channelID));
            }

            public static string GetUrlCreateIndex(int publishmentSystemID)
            {
                return PageUtils.GetSTLUrl(string.Format("background_serviceSTL.aspx?type={0}&method={1}&publishmentSystemID={2}", "AjaxUrlFSO", "CreateIndex", publishmentSystemID));
            }

            public static string GetUrlCreateImmediately(int publishmentSystemID, EChangedType changedType, ETemplateType templateType, int channelID, int contentID, int fileTemplateID)
            {
                return PageUtils.GetSTLUrl(string.Format("background_serviceSTL.aspx?type={0}&method={1}&publishmentSystemID={2}&changedType={3}&templateType={4}&channelID={5}&contentID={6}&fileTemplateID={7}", "AjaxUrlFSO", "CreateImmediately", publishmentSystemID, EChangedTypeUtils.GetValue(changedType), ETemplateTypeUtils.GetValue(templateType), channelID, contentID, fileTemplateID));
            }
        }

        public class DynamicPage
        {
            public static string GetRedirectUrl(int publishmentSystemID, int channelID, int contentID, int fileTemplateID, int pageIndex)
            {
                string parms = string.Format("?s={0}", publishmentSystemID);
                if (channelID > 0)
                {
                    parms += string.Format("&n={0}", channelID);
                }
                if (contentID > 0)
                {
                    parms += string.Format("&c={0}", contentID);
                }
                if (fileTemplateID > 0)
                {
                    parms += string.Format("&f={0}", fileTemplateID);
                }
                if (pageIndex > 0)
                {
                    parms += string.Format("&p={0}", pageIndex);
                }
                return PageUtility.Services.GetUrl(string.Format("page.aspx{0}", parms));
            }

            public static string GetPreviewUrl(string pageUrl, int previewID)
            {
                return PageUtils.AddQueryString(pageUrl, string.Format("&isPreview=true&previewID={0}", previewID));
            }

            public static string GetDesignUrl(string pageUrl)
            {
                return GetDesignUrl(pageUrl, string.Empty, EDesignMode.Edit);
            }

            public static string GetDesignUrl(string pageUrl, string includeUrl, EDesignMode designMode)
            {
                NameValueCollection attributes = new NameValueCollection();
                attributes.Add("isDesign", "true");
                attributes.Add("includeUrl", RuntimeUtils.EncryptStringByTranslate(includeUrl));
                attributes.Add("designMode", EDesignModeUtils.GetValue(designMode));
                return PageUtils.AddQueryString(pageUrl, attributes);
            }
        }

        public class ModalSTL
        {

            public static string ChannelImport_GetOpenWindowString(int publishmentSystemID, int nodeID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("NodeID", nodeID.ToString());
                return JsUtils.Layer.GetOpenLayerString("导入栏目", PageUtils.GetSTLUrl("modal_channelImport.aspx"), arguments, 560, 260);
            }

            public static string ContentExport_GetOpenWindowString(int publishmentSystemID, int nodeID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("NodeID", nodeID.ToString());
                return JsUtils.Layer.GetOpenLayerStringWithCheckBoxValue("导出内容", PageUtils.GetSTLUrl("modal_contentExport.aspx"), arguments, "ContentIDCollection", string.Empty);
            }

            public static string ContentImport_GetOpenWindowString(int publishmentSystemID, int nodeID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("NodeID", nodeID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("导入内容", PageUtils.GetSTLUrl("modal_contentImport.aspx"), arguments, 620, 500);
            }

            public static string InputContentImport_GetOpenWindowString(int publishmentSystemID, int inputID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("InputID", inputID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("导入内容", PageUtils.GetSTLUrl("modal_inputContentImport.aspx"), arguments, 570, 320);
            }

            public static string TableStyleImport_GetOpenWindowString(string tableName, ETableStyle tableStyle, int publishmentSystemID, int relatedIdentity)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("TableName", tableName);
                arguments.Add("TableStyle", ETableStyleUtils.GetValue(tableStyle));
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("RelatedIdentity", relatedIdentity.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("导入表样式", PageUtils.GetSTLUrl("modal_tableStyleImport.aspx"), arguments, 560, 200);
            }

            public static string TagStyleGovInteractApplyAdd_GetOpenWindowString(int publishmentSystemID, int styleID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("StyleID", styleID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("互动交流提交设置", PageUtils.GetSTLUrl("modal_tagStyleGovInteractApplyAdd.aspx"), arguments, 360, 240);
            }

            public static string TagStyleCommentInputAdd_GetOpenWindowString(int publishmentSystemID, int styleID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("StyleID", styleID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("评论提交设置", PageUtils.GetSTLUrl("modal_tagStyleCommentInputAdd.aspx"), arguments, 560, 360);
            }

            public static string TagStyleCommentsAdd_GetOpenWindowString(int publishmentSystemID, int styleID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("StyleID", styleID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("评论显示设置", PageUtils.GetSTLUrl("modal_tagStyleCommentsAdd.aspx"), arguments, 420, 460);
            }

            public static string TagStyleContentInputAdd_GetOpenWindowStringToAdd(int publishmentSystemID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("添加内容提交样式", PageUtils.GetSTLUrl("modal_tagStyleContentInputAdd.aspx"), arguments, 560, 420);
            }

            public static string TagStyleContentInputAdd_GetOpenWindowStringToEdit(int publishmentSystemID, int styleID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("StyleID", styleID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("修改内容提交样式", PageUtils.GetSTLUrl("modal_tagStyleContentInputAdd.aspx"), arguments, 560, 420);
            }

            public static string TagStyleDiggAdd_GetOpenWindowStringToAdd(int publishmentSystemID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("添加掘客(digg)样式", PageUtils.GetSTLUrl("modal_tagStyleDiggAdd.aspx"), arguments, 420, 320);
            }

            public static string TagStyleDiggAdd_GetOpenWindowStringToEdit(int publishmentSystemID, int styleID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("StyleID", styleID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("修改掘客(digg)样式", PageUtils.GetSTLUrl("modal_tagStyleDiggAdd.aspx"), arguments, 420, 320);
            }

            public static string TagStyleGovInteractQueryAdd_GetOpenWindowStringToAdd(int publishmentSystemID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("添加依申请公开查询样式", PageUtils.GetSTLUrl("modal_tagStyleGovInteractQueryAdd.aspx"), arguments, 560, 420);
            }

            public static string TagStyleGovInteractQueryAdd_GetOpenWindowStringToEdit(int publishmentSystemID, int styleID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("StyleID", styleID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("修改依申请公开查询样式", PageUtils.GetSTLUrl("modal_tagStyleGovInteractQueryAdd.aspx"), arguments, 560, 420);
            }

            public static string TagStyleGovPublicApplyAdd_GetOpenWindowStringToAdd(int publishmentSystemID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("添加依申请公开提交样式", PageUtils.GetSTLUrl("modal_tagStyleGovPublicApplyAdd.aspx"), arguments, 560, 420);
            }

            public static string TagStyleGovPublicApplyAdd_GetOpenWindowStringToEdit(int publishmentSystemID, int styleID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("StyleID", styleID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("修改依申请公开提交样式", PageUtils.GetSTLUrl("modal_tagStyleGovPublicApplyAdd.aspx"), arguments, 560, 420);
            }

            public static string TagStyleGovPublicQueryAdd_GetOpenWindowStringToAdd(int publishmentSystemID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("添加依申请公开查询样式", PageUtils.GetSTLUrl("modal_tagStyleGovPublicQueryAdd.aspx"), arguments, 500, 220);
            }

            public static string TagStyleGovPublicQueryAdd_GetOpenWindowStringToEdit(int publishmentSystemID, int styleID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("StyleID", styleID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("修改依申请公开查询样式", PageUtils.GetSTLUrl("modal_tagStyleGovPublicQueryAdd.aspx"), arguments, 500, 220);
            }

            public static string TagStyleResumeAdd_GetOpenWindowStringToAdd(int publishmentSystemID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("添加简历提交样式", PageUtils.GetSTLUrl("modal_tagStyleResumeAdd.aspx"), arguments, 520, 520);
            }

            public static string TagStyleResumeAdd_GetOpenWindowStringToEdit(int publishmentSystemID, int styleID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("StyleID", styleID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("修改简历提交样式", PageUtils.GetSTLUrl("modal_tagStyleResumeAdd.aspx"), arguments, 520, 520);
            }

            public static string TagStyleSearchInputAdd_GetOpenWindowStringToAdd(int publishmentSystemID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("添加搜索框样式", PageUtils.GetSTLUrl("modal_tagStyleSearchInputAdd.aspx"), arguments, 460, 530);
            }

            public static string TagStyleSearchInputAdd_GetOpenWindowStringToEdit(int publishmentSystemID, int styleID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("StyleID", styleID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("修改搜索框样式", PageUtils.GetSTLUrl("modal_tagStyleSearchInputAdd.aspx"), arguments, 460, 530);
            }

            public static string TagStyleStarAdd_GetOpenWindowStringToAdd(int publishmentSystemID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("添加登录框样式", PageUtils.GetSTLUrl("modal_tagStyleLoginAdd.aspx"), arguments, 420, 320);
            }

            public static string TagStyleStarAdd_GetOpenWindowStringToEdit(int publishmentSystemID, int styleID)
            {
                NameValueCollection arguments = new NameValueCollection();
                arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                arguments.Add("StyleID", styleID.ToString());
                return JsUtils.OpenWindow.GetOpenWindowString("修改登录框样式", PageUtils.GetSTLUrl("modal_tagStyleLoginAdd.aspx"), arguments, 420, 320);
            }

            public class ExportMessage
            {
                public const int Width = 380;
                public const int Height = 250;
                public const string EXPORT_TYPE_ContentZip = "ContentZip";
                public const string EXPORT_TYPE_ContentAccess = "ContentAccess";
                public const string EXPORT_TYPE_ContentExcel = "ContentExcel";
                public const string EXPORT_TYPE_ContentTxtZip = "ContentTxtZip";
                public const string EXPORT_TYPE_InputContent = "InputContent";
                public const string EXPORT_TYPE_WebsiteMessageContent = "WebsiteMessageContent";
                public const string EXPORT_TYPE_Comment = "Comment";
                public const string EXPORT_TYPE_MailSubscribe = "MailSubscribe";
                public const string EXPORT_TYPE_GatherRule = "GatherRule";
                public const string EXPORT_TYPE_Input = "Input";
                public const string EXPORT_TYPE_RelatedField = "RelatedField";
                public const string EXPORT_TYPE_TagStyle = "TagStyle";
                public const string EXPORT_TYPE_Channel = "Channel";
                public const string EXPORT_TYPE_SingleTableStyle = "SingleTableStyle";
                public const string EXPORT_TYPE_TrackerHour = "TrackerHour";
                public const string EXPORT_TYPE_TrackerDay = "TrackerDay";
                public const string EXPORT_TYPE_TrackerMonth = "TrackerMonth";
                public const string EXPORT_TYPE_TrackerYear = "TrackerYear";
                public const string EXPORT_TYPE_TrackerContent = "TrackerContent";
                public const string EXPORT_TYPE_Survey = "Survey";

                public static string GetRedirectUrlStringToExportContent(int publishmentSystemID, int nodeID, string exportType, string contentIDCollection, string displayAttributes, bool isPeriods, string startDate, string endDate, ETriState checkedState)
                {
                    return PageUtils.GetSTLUrl(string.Format("modal_exportMessage.aspx?PublishmentSystemID={0}&NodeID={1}&ExportType={2}&ContentIDCollection={3}&DisplayAttributes={4}&isPeriods={5}&startDate={6}&endDate={7}&checkedState={8}", publishmentSystemID, nodeID, exportType, contentIDCollection, displayAttributes, isPeriods, startDate, endDate, ETriStateUtils.GetValue(checkedState)));
                }

                public static string GetOpenWindowStringToInputContent(int publishmentSystemID, int inputID)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("InputID", inputID.ToString());
                    arguments.Add("ExportType", EXPORT_TYPE_InputContent);
                    return JsUtils.OpenWindow.GetOpenWindowString("导出数据", PageUtils.GetSTLUrl("modal_exportMessage.aspx"), arguments, Width, Height, true);
                }


                public static string GetOpenWindowStringToWebsiteMessageContent(int publishmentSystemID, int websiteMessageID, int classifyID)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("WebsiteMessageID", websiteMessageID.ToString());
                    arguments.Add("ClassifyID", classifyID.ToString());
                    arguments.Add("ExportType", EXPORT_TYPE_WebsiteMessageContent);
                    return JsUtils.OpenWindow.GetOpenWindowString("导出数据", PageUtils.GetSTLUrl("modal_exportMessage.aspx"), arguments, Width, Height, true);
                }

                public static string GetOpenWindowStringToMailSubscribe(int publishmentSystemID)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("ExportType", EXPORT_TYPE_MailSubscribe);
                    return JsUtils.OpenWindow.GetOpenWindowString("导出数据", PageUtils.GetSTLUrl("modal_exportMessage.aspx"), arguments, Width, Height, true);
                }

                public static string GetOpenWindowStringToComment(int publishmentSystemID, int nodeID, int contentID)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("NodeID", nodeID.ToString());
                    arguments.Add("ContentID", contentID.ToString());
                    arguments.Add("ExportType", EXPORT_TYPE_Comment);
                    return JsUtils.OpenWindow.GetOpenWindowString("导出数据", PageUtils.GetSTLUrl("modal_exportMessage.aspx"), arguments, Width, Height, true);
                }

                public static string GetOpenWindowStringToGatherRule(int publishmentSystemID, string checkBoxID, string alertString)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("ExportType", EXPORT_TYPE_GatherRule);
                    return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("导出数据", PageUtils.GetSTLUrl("modal_exportMessage.aspx"), arguments, checkBoxID, alertString, Width, Height, true);
                }

                public static string GetOpenWindowStringToInput(int publishmentSystemID, int inputID)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("InputID", inputID.ToString());
                    arguments.Add("ExportType", EXPORT_TYPE_Input);
                    return JsUtils.OpenWindow.GetOpenWindowString("导出数据", PageUtils.GetSTLUrl("modal_exportMessage.aspx"), arguments, Width, Height, true);
                }


                public static string GetOpenWindowStringToWebsiteMessage(int publishmentSystemID, int websiteMessageID)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("WebsiteMessageID", websiteMessageID.ToString());
                    arguments.Add("ExportType", EXPORT_TYPE_Input);
                    return JsUtils.OpenWindow.GetOpenWindowString("导出数据", PageUtils.GetSTLUrl("modal_exportMessage.aspx"), arguments, Width, Height, true);
                }

                public static string GetOpenWindowStringToTagStyle(int publishmentSystemID, int styleID)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("StyleID", styleID.ToString());
                    arguments.Add("ExportType", EXPORT_TYPE_TagStyle);
                    return JsUtils.OpenWindow.GetOpenWindowString("导出数据", PageUtils.GetSTLUrl("modal_exportMessage.aspx"), arguments, Width, Height, true);
                }

                public static string GetOpenWindowStringToChannel(int publishmentSystemID, string checkBoxID, string alertString)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("ExportType", EXPORT_TYPE_Channel);
                    return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("导出数据", PageUtils.GetSTLUrl("modal_exportMessage.aspx"), arguments, checkBoxID, alertString, Width, Height, true);
                }

                public static string GetOpenWindowStringToSingleTableStyle(ETableStyle tableStyle, string tableName, int publishmentSystemID, int relatedIdentity)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("TableStyle", ETableStyleUtils.GetValue(tableStyle));
                    arguments.Add("TableName", tableName);
                    arguments.Add("ExportType", EXPORT_TYPE_SingleTableStyle);
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("RelatedIdentity", relatedIdentity.ToString());
                    return JsUtils.OpenWindow.GetOpenWindowString("导出数据", PageUtils.GetSTLUrl("modal_exportMessage.aspx"), arguments, Width, Height, true);
                }

                public static string GetOpenWindowStringToRelatedField(int publishmentSystemID, int relatedFieldID)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("RelatedFieldID", relatedFieldID.ToString());
                    arguments.Add("ExportType", EXPORT_TYPE_RelatedField);
                    return JsUtils.OpenWindow.GetOpenWindowString("导出数据", PageUtils.GetSTLUrl("modal_exportMessage.aspx"), arguments, Width, Height, true);
                }

                public static string GetOpenWindowStringToExport(int publishmentSystemID, string exportType)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("ExportType", exportType);
                    return JsUtils.OpenWindow.GetOpenWindowString("导出数据", PageUtils.GetSTLUrl("modal_exportMessage.aspx"), arguments, Width, Height, true);
                }

                public static string GetRedirectUrlStringToExportTracker(string startDateString, string endDateString, int publishmentSystemID, int nodeID, int contentID, int totalNum, bool isDelete)
                {
                    return PageUtils.GetSTLUrl(string.Format("modal_exportMessage.aspx?ExportType={0}&StartDateString={1}&EndDateString={2}&PublishmentSystemID={3}&NodeID={4}&ContentID={5}&TotalNum={6}&IsDelete={7}", EXPORT_TYPE_TrackerContent, startDateString, endDateString, publishmentSystemID, nodeID, contentID, totalNum, isDelete));
                }

                public static string GetOpenWindowStringToSurvey(int publishmentSystemID, int nodeID, int contentID)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("NodeID", nodeID.ToString());
                    arguments.Add("ContentID", contentID.ToString()); 
                    arguments.Add("ExportType", EXPORT_TYPE_Survey);
                    return JsUtils.OpenWindow.GetOpenWindowString("导出数据", PageUtils.GetSTLUrl("modal_exportMessage.aspx"), arguments, Width, Height, true);
                }
            }

            public class Import
            {
                public const int Width = 560;
                public const int Height = 260;
                public const string TYPE_INPUT = "INPUT";
                public const string TYPE_RELATED_FIELD = "RELATED_FIELD";
                public const string TYPE_TAGSTYLE = "TAGSTYLE";
                public const string TYPE_GATHERRULE = "GATHERRULE";

                public static string GetOpenWindowString(int publishmentSystemID, string type)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("Type", type);
                    string title = string.Empty;
                    if (StringUtils.EqualsIgnoreCase(type, Import.TYPE_GATHERRULE))
                    {
                        title = "导入采集规则";
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, Import.TYPE_INPUT))
                    {
                        title = "导入提交表单";
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, Import.TYPE_RELATED_FIELD))
                    {
                        title = "导入联动字段";
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, Import.TYPE_TAGSTYLE))
                    {
                        title = "导入模板标签样式";
                    }
                    return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetSTLUrl("modal_import.aspx"), arguments, Width, Height);
                }

                /// <summary>
                /// by 20151029 sofuny
                /// </summary>
                /// <param name="publishmentSystemID"></param>
                /// <param name="type"></param>
                /// <param name="itemID"></param>
                /// <returns></returns>
                public static string GetOpenWindowString(int publishmentSystemID, string type, int itemID)
                {
                    NameValueCollection arguments = new NameValueCollection();
                    arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
                    arguments.Add("Type", type);
                    arguments.Add("ItemID", itemID.ToString());
                    string title = string.Empty;
                    if (StringUtils.EqualsIgnoreCase(type, Import.TYPE_GATHERRULE))
                    {
                        title = "导入采集规则";
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, Import.TYPE_INPUT))
                    {
                        title = "导入提交表单";
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, Import.TYPE_RELATED_FIELD))
                    {
                        title = "导入联动字段";
                    }
                    else if (StringUtils.EqualsIgnoreCase(type, Import.TYPE_TAGSTYLE))
                    {
                        title = "导入模板标签样式";
                    }
                    return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetSTLUrl("modal_import.aspx"), arguments, Width, Height);
                }
            }
        }



        public class ServiceSTL
        {
            public class Utils
            {
                public const string Type_StlTrigger = "StlTrigger";
                public const string Type_Redirect = "Redirect";
                public const string Type_Download = "Download";

                public static string GetRedirectUrl(int channelID, bool isInner)
                {
                    string pageUrl = PageUtility.Services.GetUrl(string.Format("utils.aspx?type={0}&channelID={1}", Type_Redirect, channelID));
                    if (isInner)
                    {
                        pageUrl = PageUtils.AddQueryString(pageUrl, "isInner", true.ToString());
                    }
                    return pageUrl;
                }

                public static string GetRedirectUrl(int publishmentSystemID, int channelID, int contentID, bool isInner)
                {
                    string pageUrl = PageUtility.Services.GetUrl(string.Format("utils.aspx?type={0}&publishmentSystemID={1}&channelID={2}&contentID={3}", Type_Redirect, publishmentSystemID, channelID, contentID));
                    if (isInner)
                    {
                        pageUrl = PageUtils.AddQueryString(pageUrl, "isInner", true.ToString());
                    }
                    return pageUrl;
                }

                public static string GetDownloadUrl(int publishmentSystemID, int channelID, int contentID)
                {
                    return PageUtility.Services.GetUrl(string.Format("utils.aspx?type={0}&publishmentSystemID={1}&channelID={2}&contentID={3}", Type_Download, publishmentSystemID, channelID, contentID));
                }

                public static string GetDownloadUrl(int publishmentSystemID, int channelID, int contentID, string fileUrl)
                {
                    return PageUtility.Services.GetUrl(string.Format("utils.aspx?type={0}&publishmentSystemID={1}&channelID={2}&contentID={3}&fileUrl={4}", Type_Download, publishmentSystemID, channelID, contentID, RuntimeUtils.EncryptStringByTranslate(fileUrl)));
                }

                public static string GetDownloadUrl(int publishmentSystemID, string fileUrl)
                {
                    return PageUtility.Services.GetUrl(string.Format("utils.aspx?type={0}&publishmentSystemID={1}&fileUrl={2}", Type_Download, publishmentSystemID, RuntimeUtils.EncryptStringByTranslate(fileUrl)));
                }

                public static string GetDownloadUrlByFilePath(string filePath)
                {
                    return PageUtility.Services.GetUrl(string.Format("utils.aspx?type={0}&filePath={1}", Type_Download, RuntimeUtils.EncryptStringByTranslate(filePath)));
                }

                public static string GetStlTriggerUrl(int publishmentSystemID, int channelID, int contentID)
                {
                    return GetStlTriggerUrl(publishmentSystemID, channelID, contentID, 0, true);
                }

                public static string GetStlTriggerUrl(int publishmentSystemID, int channelID, int contentID, int fileTemplateID, bool isRedirect)
                {
                    string urlParms = string.Format("publishmentSystemID={0}", publishmentSystemID);
                    urlParms += (channelID != 0) ? string.Format("&channelID={0}", channelID) : string.Empty;
                    urlParms += (contentID != 0) ? string.Format("&contentID={0}", contentID) : string.Empty;
                    urlParms += (fileTemplateID != 0) ? string.Format("&fileTemplateID={0}", fileTemplateID) : string.Empty;
                    urlParms += (isRedirect) ? "&isRedirect=true" : string.Empty;
                    return PageUtility.Services.GetUrl(string.Format("utils.aspx?type={0}&{1}", Type_StlTrigger, urlParms));
                }
            }

            public class AjaxUpload
            {
                public static string GetContentPhotoUploadMultipleUrl(int publishmentSystemID)
                {
                    return PageUtility.Services.GetUrl(string.Format("ajaxUpload.aspx?PublishmentSystemID={0}&isContentPhotoSwfUpload=True", publishmentSystemID));
                }

                public static string GetContentPhotoUploadSingleUrl(int publishmentSystemID)
                {
                    return PageUtility.Services.GetUrl(string.Format("ajaxUpload.aspx?PublishmentSystemID={0}&isContentPhoto=True", publishmentSystemID));
                }

                public static string GetContentTeleplayUploadMultipleUrl(int publishmentSystemID)
                {
                    return PageUtility.Services.GetUrl(string.Format("ajaxUpload.aspx?PublishmentSystemID={0}&isContentTeleplaySwfUpload=True", publishmentSystemID));
                }

                public static string GetContentTeleplayUploadSingleUrl(int publishmentSystemID)
                {
                    return PageUtility.Services.GetUrl(string.Format("ajaxUpload.aspx?PublishmentSystemID={0}&isContentTeleplay=True", publishmentSystemID));
                }

                public static string GetUploadWordMultipleUrl(int publishmentSystemID)
                {
                    return PageUtility.Services.GetUrl(string.Format("ajaxUpload.aspx?PublishmentSystemID={0}&isWordSwfUpload=True", publishmentSystemID));
                }
            }
        }

        public class Services
        {
            internal const string DIRECTORY_PATH = "sitefiles/services/cms";

            public const string API_SERVICES_ACTION = "services/cms/action";
            public const string API_SERVICES_PAGE_ACTION = "services/cmspage/action";

            public static string GetServiceUrl(PublishmentSystemInfo publishmentSystemInfo, string relatedUrl)
            {
                return PageUtils.GetRootUrl(string.Format("{0}/{1}", DIRECTORY_PATH, relatedUrl));
            }

            //public static string GetActionUrlOfCommentInput(PublishmentSystemInfo publishmentSystemInfo, int styleID)
            //{
            //    if (ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset) == ECharset.gb2312)
            //    {
            //        return PageUtils.GetRootUrl(string.Format("{0}/gb2312/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_Comment));
            //    }
            //    return PageUtils.GetRootUrl(string.Format("{0}/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_Comment));
            //}

            #region 已修改为API

            public static string GetActionUrlOfContentInput(PublishmentSystemInfo publishmentSystemInfo, int styleID, int channelID)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    //update 20151130 by sofuny, to api
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_Content);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    parms.Add("channelID", channelID.ToString());
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));
                }
                else
                {
                    if (ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset) == ECharset.gb2312)
                    {
                        return PageUtils.GetRootUrl(string.Format(@"{0}/gb2312/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}&channelID={4}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_Content, channelID));
                    }
                    return PageUtils.GetRootUrl(string.Format(@"{0}/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}&channelID={4}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_Content, channelID));
                }
            }

            public static string GetActionUrlOfInput(PublishmentSystemInfo publishmentSystemInfo, int inputID)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    //update 20151201 by sofuny, to api
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_Input);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    parms.Add("InputID", inputID.ToString());
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));
                }
                else
                {
                    if (ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset) == ECharset.gb2312)
                    {
                        return PageUtils.GetRootUrl(string.Format(@"{0}/gb2312/action.aspx?publishmentSystemID={1}&inputID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, inputID, Constants.StlTemplateManagerActionType.ActionType_Input));
                    }
                    return PageUtils.GetRootUrl(string.Format(@"{0}/action.aspx?publishmentSystemID={1}&inputID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, inputID, Constants.StlTemplateManagerActionType.ActionType_Input));
                }
            }

            public static string GetActionUrlOfWebsiteMessage(PublishmentSystemInfo publishmentSystemInfo, int websiteMessageID)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    //update 20151125 by sessionliang, to api
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_WebsiteMessage);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    parms.Add("WebsiteMessageID", websiteMessageID.ToString());
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));
                }
                else
                {
                    if (ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset) == ECharset.gb2312)
                    {
                        return PageUtils.GetRootUrl(string.Format(@"{0}/gb2312/action.aspx?publishmentSystemID={1}&websiteMessageID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, websiteMessageID, Constants.StlTemplateManagerActionType.ActionType_WebsiteMessage));
                    }
                    return PageUtils.GetRootUrl(string.Format(@"{0}/action.aspx?publishmentSystemID={1}&websiteMessageID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, websiteMessageID, Constants.StlTemplateManagerActionType.ActionType_WebsiteMessage));
                }
            }

            public static string GetActionUrlOfVote(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    //update 20151130 by sofuny, to api
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_Vote);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    parms.Add("nodeID", nodeID.ToString());
                    parms.Add("contentID", contentID.ToString());
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));
                }
                else
                {

                    if (ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset) == ECharset.gb2312)
                    {
                        return PageUtils.GetRootUrl(string.Format(@"{0}/gb2312/action.aspx?publishmentSystemID={1}&nodeID={2}&contentID={3}&type={4}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, nodeID, contentID, Constants.StlTemplateManagerActionType.ActionType_Vote));
                    }
                    return PageUtils.GetRootUrl(string.Format(@"{0}/action.aspx?publishmentSystemID={1}&nodeID={2}&contentID={3}&type={4}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, nodeID, contentID, Constants.StlTemplateManagerActionType.ActionType_Vote));
                }
            }

            /// <summary>
            /// 提交会员订阅信息的路径
            /// </summary>
            /// <param name="publishmentSystemInfo"></param>
            /// <returns></returns>
            public static string GetActionUrlOfSubscribe(PublishmentSystemInfo publishmentSystemInfo)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_SubscribeApply);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));
                }
                else
                {

                    if (ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset) == ECharset.gb2312)
                    {
                        return PageUtils.GetRootUrl(string.Format(@"{0}/gb2312/action.aspx?publishmentSystemID={1}&type={2}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, Constants.StlTemplateManagerActionType.ActionType_SubscribeApply));
                    }
                    return PageUtils.GetRootUrl(string.Format(@"{0}/action.aspx?publishmentSystemID={1}&type={2}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, Constants.StlTemplateManagerActionType.ActionType_SubscribeApply));
                }
            }

            /// <summary>
            /// 获取订阅内容的路径
            /// </summary>
            /// <param name="publishmentSystemInfo"></param>
            /// <returns></returns>
            public static string GetUrlOfSubscribe(PublishmentSystemInfo publishmentSystemInfo)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_SubscribeQuery);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));
                }
                else
                {
                    string serviceUrl = string.Format("PageService.aspx?type={0}&publishmentSystemID={1}", Constants.StlTemplateManagerActionType.ActionType_SubscribeQuery, publishmentSystemInfo.PublishmentSystemID);
                    return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
                }
            }

            /// <summary>
            /// 获取广告Html的API路径
            /// </summary>
            /// <param name="publishmentSystemInfo"></param>
            /// <returns></returns>
            public static string GetUrlOfAdHtml(PublishmentSystemInfo publishmentSystemInfo, int unitueID, string adAreaName, int channelID, int fileTemplateID, string templateType)
            {

                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_Ad);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    parms.Add("unitueID", unitueID.ToString());
                    parms.Add("adAreaName", adAreaName);
                    parms.Add("channelID", channelID.ToString());
                    parms.Add("fileTemplateID", fileTemplateID.ToString());
                    parms.Add("templateType", templateType);
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));
                }
                else
                {
                    string serviceUrl = string.Format("adv/js.aspx?publishmentSystemID={0}&uniqueID={1}&adAreaName={2}&channelID={3}&templateType={4}&fileTemplateID={5}",  publishmentSystemInfo.PublishmentSystemID, unitueID, adAreaName, channelID, templateType, fileTemplateID);
                    return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
                }
            }

            #endregion

            #region 未修改为API
            public static string GetActionUrlOfLogin(PublishmentSystemInfo publishmentSystemInfo, int styleID)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    //update 20151130 by sofuny, to api
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_Login);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    parms.Add("styleID", styleID.ToString());
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));
                }
                else
                {
                    if (ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset) == ECharset.gb2312)
                    {
                        return PageUtils.GetRootUrl(string.Format(@"{0}/gb2312/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_Login));
                    }
                    return PageUtils.GetRootUrl(string.Format(@"{0}/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_Login));
                }
            }

            public static string GetActionUrlOfRegister(PublishmentSystemInfo publishmentSystemInfo, int styleID)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    //update 20151130 by sofuny, to api
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_Register);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    parms.Add("styleID", styleID.ToString());
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));

                }
                else
                {
                    if (ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset) == ECharset.gb2312)
                    {
                        return PageUtils.GetRootUrl(string.Format(@"{0}/gb2312/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_Register));
                    }
                    return PageUtils.GetRootUrl(string.Format(@"{0}/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_Register));

                }
            }

            public static string GetActionUrlOfResume(PublishmentSystemInfo publishmentSystemInfo, int styleID)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    //update 20151130 by sofuny, to api
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_Resume);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    parms.Add("styleID", styleID.ToString());
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));
                }
                else
                {
                    if (ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset) == ECharset.gb2312)
                    {
                        return PageUtils.GetRootUrl(string.Format(@"{0}/gb2312/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_Resume));
                    }
                    return PageUtils.GetRootUrl(string.Format(@"{0}/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_Resume));
                }
            }

            public static string GetActionUrlOfGovPublicApply(PublishmentSystemInfo publishmentSystemInfo, int styleID)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    //update 20151130 by sofuny, to api
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_GovPublicApply);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    parms.Add("styleID", styleID.ToString());
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));
                }
                else
                {
                    if (ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset) == ECharset.gb2312)
                    {
                        return PageUtils.GetRootUrl(string.Format(@"{0}/gb2312/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_GovPublicApply));
                    }
                    return PageUtils.GetRootUrl(string.Format(@"{0}/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_GovPublicApply));

                }
            }

            public static string GetActionUrlOfGovPublicQuery(PublishmentSystemInfo publishmentSystemInfo, int styleID)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    //update 20151130 by sofuny, to api
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_GovPublicQuery);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    parms.Add("styleID", styleID.ToString());
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));

                }
                else
                {
                    if (ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset) == ECharset.gb2312)
                    {
                        return PageUtils.GetRootUrl(string.Format(@"{0}/gb2312/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_GovPublicQuery));
                    }
                    return PageUtils.GetRootUrl(string.Format(@"{0}/action.aspx?publishmentSystemID={1}&styleID={2}&type={3}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, styleID, Constants.StlTemplateManagerActionType.ActionType_GovPublicQuery));
                }
            }

            public static string GetActionUrlOfGovInteractApply(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int styleID)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    //update 20151130 by sofuny, to api
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_GovInteractApply);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    parms.Add("nodeID", nodeID.ToString());
                    parms.Add("styleID", styleID.ToString());
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));
                }
                else
                {
                    if (ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset) == ECharset.gb2312)
                    {
                        return PageUtils.GetRootUrl(string.Format(@"{0}/gb2312/action.aspx?publishmentSystemID={1}&nodeID={2}&styleID={3}&type={4}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, nodeID, styleID, Constants.StlTemplateManagerActionType.ActionType_GovInteractApply));
                    }
                    return PageUtils.GetRootUrl(string.Format(@"{0}/action.aspx?publishmentSystemID={1}&nodeID={2}&styleID={3}&type={4}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, nodeID, styleID, Constants.StlTemplateManagerActionType.ActionType_GovInteractApply));

                }
            }

            public static string GetActionUrlOfGovInteractQuery(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int styleID)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    //update 20151130 by sofuny, to api
                    NameValueCollection parms = new NameValueCollection();
                    parms.Add("Type", Constants.StlTemplateManagerActionType.ActionType_GovInteractQuery);
                    parms.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
                    parms.Add("nodeID", nodeID.ToString());
                    parms.Add("styleID", styleID.ToString());
                    return GetUrl(publishmentSystemInfo, PageUtils.AddQueryString(API_SERVICES_ACTION, parms));
                }
                else
                {
                    if (ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset) == ECharset.gb2312)
                    {
                        return PageUtils.GetRootUrl(string.Format(@"{0}/gb2312/action.aspx?publishmentSystemID={1}&nodeID={2}&styleID={3}&type={4}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, nodeID, styleID, Constants.StlTemplateManagerActionType.ActionType_GovInteractQuery));
                    }
                    return PageUtils.GetRootUrl(string.Format(@"{0}/action.aspx?publishmentSystemID={1}&nodeID={2}&styleID={3}&type={4}", DIRECTORY_PATH, publishmentSystemInfo.PublishmentSystemID, nodeID, styleID, Constants.StlTemplateManagerActionType.ActionType_GovInteractQuery));

                }
            }
            #endregion

            public static string GetUrl(string relatedPath)
            {
                if (string.IsNullOrEmpty(relatedPath))
                {
                    return PageUtils.GetRootUrl(DIRECTORY_PATH);
                }
                else
                {
                    return PageUtils.Combine(PageUtils.GetRootUrl(DIRECTORY_PATH), relatedPath);
                }
            }

            public static string GetUrl(PublishmentSystemInfo publishmentSystemInfo, string relatedUrl)
            {
                return GetUrl(publishmentSystemInfo, relatedUrl, ECharset.utf_8, false);
            }

            public static string GetUrl(PublishmentSystemInfo publishmentSystemInfo, string relatedUrl, ECharset charset, bool isXmlContent)
            {
                if (PageUtility.IsAgentCrossDomain(publishmentSystemInfo))//代理跨域
                {
                    string innerPageUrl = string.Format("~/{0}/{1}", PageUtility.Services.DIRECTORY_PATH, relatedUrl);
                    innerPageUrl = PageUtils.ParseConfigRootUrl(innerPageUrl);
                    return GetUrlPrivate(publishmentSystemInfo, innerPageUrl, charset, isXmlContent);
                }
                else if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))//CORS跨域
                {
                    return API.GetAPIUrl(publishmentSystemInfo, relatedUrl);
                }
                else
                {
                    string innerPageUrl = string.Format("~/{0}/{1}", PageUtility.Services.DIRECTORY_PATH, relatedUrl);
                    innerPageUrl = PageUtils.ParseConfigRootUrl(innerPageUrl);
                    return innerPageUrl;
                }
            }

            private static string GetUrlPrivate(PublishmentSystemInfo publishmentSystemInfo, string innerPageUrl, ECharset charset, bool isXmlContent)
            {
                return PageUtility.ParseNavigationUrl(publishmentSystemInfo, string.Format("@/utils/ajaxProxy.aspx?charset={0}&url={1}&isXmlContent={2}", ECharsetUtils.GetValue(charset), PageUtils.UrlEncode(PageUtils.AddProtocolToUrl(innerPageUrl), charset), isXmlContent.ToString().ToLower()));
            }

            public static string GetImageUrl(string relatedPath)
            {
                if (string.IsNullOrEmpty(relatedPath))
                {
                    return PageUtils.GetRootUrl(string.Format("{0}/images", DIRECTORY_PATH));
                }
                else
                {
                    return PageUtils.Combine(PageUtils.GetRootUrl(string.Format("{0}/images", DIRECTORY_PATH)), relatedPath);
                }
            }

            public static string GetPath(string relatedPath)
            {
                return PathUtils.MapPath(PathUtils.Combine(DIRECTORY_PATH, relatedPath));
            }
        }

        //update 20151125 by sessionliang, to api
        public class API
        {
            public static string GetAPIUrl(PublishmentSystemInfo publishmentSystemInfo, string controller, string action)
            {
                string url = string.Empty;

                if (PageUtils.IsProtocolUrl(publishmentSystemInfo.Additional.APIUrl))
                {
                    url = PageUtils.Combine(publishmentSystemInfo.Additional.APIUrl, controller, action);
                }
                else
                {
                    url = PageUtils.AddProtocolToUrl(PageUtils.Combine(PageUtils.GetHost(), PageUtils.ParseNavigationUrl(publishmentSystemInfo.Additional.APIUrl), controller, action));
                }

                return url;
            }

            public static string GetAPIUrl(PublishmentSystemInfo publishmentSystemInfo, string controller, string action, NameValueCollection parms)
            {
                string url = string.Empty;

                if (PageUtils.IsProtocolUrl(publishmentSystemInfo.Additional.APIUrl))
                {
                    url = PageUtils.Combine(publishmentSystemInfo.Additional.APIUrl, controller, action);
                }
                else
                {
                    url = PageUtils.AddProtocolToUrl(PageUtils.Combine(PageUtils.GetHost(), PageUtils.ParseNavigationUrl(publishmentSystemInfo.Additional.APIUrl), controller, action));
                }
                url = PageUtils.AddQueryString(url, parms);
                return url;
            }

            public static string GetAPIUrl(PublishmentSystemInfo publishmentSystemInfo, string relatedUrl)
            {
                string url = string.Empty;

                if (PageUtils.IsProtocolUrl(publishmentSystemInfo.Additional.APIUrl))
                {
                    url = PageUtils.Combine(publishmentSystemInfo.Additional.APIUrl, relatedUrl);
                }
                else
                {
                    url = PageUtils.AddProtocolToUrl(PageUtils.Combine(PageUtils.GetHost(), PageUtils.ParseNavigationUrl(publishmentSystemInfo.Additional.APIUrl), relatedUrl));
                }

                return url;
            }

            public static string GetAPIUrl(PublishmentSystemInfo publishmentSystemInfo, string relatedUrl, NameValueCollection parms)
            {
                string url = string.Empty;

                if (PageUtils.IsProtocolUrl(publishmentSystemInfo.Additional.APIUrl))
                {
                    url = PageUtils.Combine(publishmentSystemInfo.Additional.APIUrl, relatedUrl);
                }
                else
                {
                    url = PageUtils.AddProtocolToUrl(PageUtils.Combine(PageUtils.GetHost(), PageUtils.ParseNavigationUrl(publishmentSystemInfo.Additional.APIUrl), relatedUrl));
                }
                url = PageUtils.AddQueryString(url, parms);
                return url;
            }
        }
    }
}
