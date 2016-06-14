using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core;
using SiteServer.BBS.Model;
using SiteServer.BBS.Core.TemplateParser.Model;
using System.Collections;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.Core
{
    public class PageUtilityBBS
    {
        public static string GetBBSUrl(int publishmentSystemID, string relatedUrl)
        {
            if (relatedUrl != null) relatedUrl = relatedUrl.Trim('/');
            return PageUtility.ParseNavigationUrl(publishmentSystemID, string.Format("@/{0}", relatedUrl));
        }

        public static string GetAjaxPageUrl(int publishmentSystemID, string realtedUrl)
        {
            return GetBBSUrl(publishmentSystemID, PageUtils.Combine("ajax", realtedUrl));
        }

        public static string GetDialogPageUrl(int publishmentSystemID, string realtedUrl)
        {
            return GetBBSUrl(publishmentSystemID, PageUtils.Combine("dialog", realtedUrl));
        }

        public static string GetIndexPageUrl(int publishmentSystemID)
        {
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, "default.aspx");
        }

        public static string GetLogoutUrl(int publishmentSystemID, string returnUrl)
        {
            return PageUtils.GetUrlWithReturnUrl(PageUtilityBBS.GetBBSUrl(publishmentSystemID, "logout.aspx"), returnUrl);
        }

        public static string GetResourceUrl(int publishmentSystemID, string relatedUrl)
        {
            return PageUtils.AddProtocolToUrl(PageUtilityBBS.GetBBSUrl(publishmentSystemID, relatedUrl));
        }

        public static string GetForumUrl(int publishmentSystemID, int forumID)
        {
            return GetForumUrl(publishmentSystemID, ForumManager.GetForumInfo(publishmentSystemID, forumID));
        }

        public static string GetForumUrl(int publishmentSystemID, ForumInfo forumInfo)
        {
            if (forumInfo != null)
            {
                if (!string.IsNullOrEmpty(forumInfo.LinkUrl))
                {
                    return PageUtility.ParseNavigationUrl(publishmentSystemID, forumInfo.LinkUrl);
                }

                string filePath = string.Empty;
                filePath = forumInfo.FilePath;
                if (string.IsNullOrEmpty(filePath))
                {
                    string forumUrl = PathUtilityBBS.ParseFilePathRule(publishmentSystemID, forumInfo.ForumID);
                    return PageUtilityBBS.GetBBSUrl(publishmentSystemID, forumUrl);
                }
                else
                {
                    return PageUtilityBBS.GetBBSUrl(publishmentSystemID, forumInfo.FilePath);
                }
            }
            return string.Empty;
        }

        public static string GetThreadUrlFormat(int publishmentSystemID, int forumID, int threadID)
        {
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("thread-{0}-{1}-{2}.aspx", forumID, threadID, "{0}"));
        }

        public static string GetThreadUrl(int publishmentSystemID, int forumID, int threadID)
        {
            return GetThreadUrl(publishmentSystemID, forumID, threadID, 0);
        }

        public static string GetThreadUrl(int publishmentSystemID, int forumID, int threadID, int page)
        {
            if (page > 1)
            {
                return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("thread-{0}-{1}-{2}.aspx", forumID, threadID, page));
            }
            else
            {
                return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("thread-{0}-{1}.aspx", forumID, threadID));
            }
        }

        public static string GetPostUrl(int publishmentSystemID, int forumID, int threadID, int page, int postID)
        {
            return PageUtilityBBS.GetThreadUrl(publishmentSystemID, forumID, threadID, page) + "#" + postID;
        }

        public static string GetInputForumUrl(int publishmentSystemID, ForumInfo forumInfo)
        {
            string forumUrl = GetForumUrl(publishmentSystemID, forumInfo);
            if (!string.IsNullOrEmpty(forumUrl))
            {
                forumUrl = StringUtils.ReplaceStartsWith(forumUrl.ToLower(), GetBBSUrl(publishmentSystemID, string.Empty).ToLower(), string.Empty);
                forumUrl = forumUrl.Trim('/');
                if (!PageUtils.IsProtocolUrl(forumUrl))
                {
                    forumUrl = "/" + forumUrl;
                }
            }
            return forumUrl;
        }

        public static string GetTemplatesUrl(int publishmentSystemID, string relatedUrl)
        {
            return PageUtils.Combine(PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Empty), "templates", relatedUrl);
        }

        public static string GetTemplateUrl(string templateUrl, string relatedUrl)
        {
            return PageUtils.Combine(templateUrl, relatedUrl);
        }

        public static string GetParserCurrentUrl(PageInfo pageInfo, int forumID, int threadID)
        {
            string currentUrl = string.Empty;
            if (pageInfo.TemplateType == ETemplateType.Index || pageInfo.TemplateType == ETemplateType.File)
            {
                currentUrl = PageUtilityBBS.GetBBSUrl(pageInfo.PublishmentSystemID, PathUtils.Combine(pageInfo.DirectoryName, pageInfo.FileName));
            }
            else if (pageInfo.TemplateType == ETemplateType.Forum)
            {
                ForumInfo forumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, forumID);
                currentUrl = PageUtilityBBS.GetForumUrl(pageInfo.PublishmentSystemID, forumInfo);
            }
            else if (pageInfo.TemplateType == ETemplateType.Thread)
            {
                currentUrl = PageUtilityBBS.GetThreadUrl(pageInfo.PublishmentSystemID, forumID, threadID);
            }
            return PageUtils.AddProtocolToUrl(currentUrl);
        }

        public static string GetBBSUrlByPhysicalPath(int publishmentSystemID, string physicalPath)
        {
            if (!string.IsNullOrEmpty(physicalPath))
            {
                string bbsPath = PathUtility.GetPublishmentSystemPath(publishmentSystemID, string.Empty);
                string requestPath = physicalPath.ToLower().Replace(bbsPath.ToLower(), string.Empty);
                requestPath = requestPath.Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar);
                return PageUtilityBBS.GetBBSUrl(publishmentSystemID, requestPath);
            }
            return string.Empty;
        }

        public static string GetRelatedUrlByPhysicalPath(int publishmentSystemID, string physicalPath)
        {
            if (!string.IsNullOrEmpty(physicalPath))
            {
                string bbsPath = PathUtility.GetPublishmentSystemPath(publishmentSystemID, string.Empty);
                string requestPath = physicalPath.ToLower().Replace(bbsPath.ToLower(), string.Empty);
                return requestPath.Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar);
            }
            return string.Empty;
        }
        //新加
        public static string GetVirtualUrl(string url)
        {
            string virtualUrl = StringUtils.ReplaceStartsWith(url, "/BBS", "@/");
            return StringUtils.ReplaceStartsWith(virtualUrl, "@//", "@/");
        }

        //根据发布系统属性判断是否为相对路径并返回解析后路径
        public static string ParseNavigationUrl(string url)
        {
            
                if (!string.IsNullOrEmpty(url) && url.StartsWith("@"))
                {
                    return GetPublishmentSystemUrl(url.Substring(1));
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
        public static string GetPublishmentSystemUrl(string requestPath)
        {
            string url = "/bbs";

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
               
                if (requestPath.StartsWith("/"))
                {
                    requestPath = requestPath.Substring(1);
                }
                if (requestPath.EndsWith("/"))
                {
                    requestPath = requestPath.Substring(0, requestPath.Length - 1);
                }
                //if (!string.IsNullOrEmpty(tend.SubDomainCollection))
                //{
                //    ArrayList subDomainArrayList = TranslateUtils.StringCollectionToArrayList(publishmentSystemInfo.Additional.SubDomainCollection, '|');
                //    string subDomain = requestPath;
                //    string restPath = string.Empty;
                //    if (requestPath.IndexOf("/") != -1)
                //    {
                //        subDomain = requestPath.Substring(0, requestPath.IndexOf("/"));
                //        restPath = requestPath.Substring(requestPath.IndexOf("/") + 1);
                //    }
                //    if (subDomainArrayList.Contains(subDomain))
                //    {
                //        url = url.Replace("http://www.", string.Format("http://{0}.", subDomain));
                //        requestPath = restPath;
                //    }
                //}

                url = PageUtils.Combine(url, requestPath);

            }
            return url;
        }
    }
}
