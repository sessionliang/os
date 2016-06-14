using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.BBS.Model;
using System.Collections.Generic;
using SiteServer.BBS.Pages;

namespace SiteServer.BBS.Core
{
    public sealed class ThreadManager
    {
        private ThreadManager()
        {
        }

        // 加载主题图标信息
        public static string GetThreadLeftIcon(ThreadInfo threadInfo)
        {
            string icon = string.Empty;
            if (threadInfo.TopLevel > 0)
            {
                icon = string.Format("top{0}", threadInfo.TopLevel);
            }
            else if (threadInfo.IsLocked)
            {
                icon = "locked";
            }
            else
            {
                int newMinutes = 600;

                string readThreads = CookieUtils.GetCookie("readThreads") + "T";
                if (newMinutes > 0 && readThreads.IndexOf("T" + threadInfo.ID.ToString() + "T") == -1 && DateTime.Now.AddMinutes(-1 * newMinutes) < threadInfo.LastDate)
                    icon = "new";
                else
                    icon = "old";

                //if (hotReplyNumber > 0 && threadInfo.Replies >= hotReplyNumber)
                //    folder += "hot";
            }

            return icon;
        }

        // 标识主题为已读
        public static void MarkOldThread(ThreadInfo threadInfo)
        {
            string readThreads = CookieUtils.GetCookie("readThreads") + "T";
            if (readThreads.IndexOf("T" + threadInfo.ID.ToString() + "T") == -1 && DateTime.Now.AddMinutes(-1 * 600) < threadInfo.LastDate)
            {
                readThreads = "T" + threadInfo.ID.ToString() + StringUtilityBBS.CutString(readThreads, 0, readThreads.Length - 1);
                if (readThreads.Length > 3000)
                {
                    readThreads = StringUtilityBBS.CutString(readThreads, 0, 3000);
                    readThreads = StringUtilityBBS.CutString(readThreads, 0, readThreads.LastIndexOf("T"));
                }
                CookieUtils.SetCookie("readThreads", readThreads, DateTime.Now.AddHours(1));
            }
        }

        public static string GetThreadRightIcons(int publishmentSystemID, ThreadInfo threadInfo, string urlPrefix)
        {
            string icons = string.Empty;
            if (threadInfo != null)
            {
                if (threadInfo.DigestLevel > 0)
                {
                    icons += @"<span class=""list_ico"">";
                    for (int i = 1; i <= threadInfo.DigestLevel; i++)
                    {
                        icons += string.Format(@"<img title=""精华 {0}"" src=""{1}images/icon_digest{0}.gif"" />", threadInfo.DigestLevel, urlPrefix);
                    }
                    icons += "</span>";
                }
                if (threadInfo.IsImage)
                {
                    icons += string.Format(@"<span class=""list_ico""><img title=""图片贴"" src=""{0}images/icon_image.gif"" /></span>", urlPrefix);
                }
                if (threadInfo.IsAttachment)
                {
                    icons += string.Format(@"<span class=""list_ico""><img title=""附件贴"" src=""{0}images/icon_attachment.gif"" /></span>", urlPrefix);
                }
                if (threadInfo.ThreadType == EThreadType.Poll)
                {
                    icons += string.Format(@"<span class=""list_ico""><img title=""投票"" src=""{0}images/icon_poll.png"" /></span>", urlPrefix);
                }
                if (threadInfo.IdentifyID > 0)
                {
                    IdentifyInfo identifyInfo = IdentifyManager.GetIdentifyInfo(publishmentSystemID, threadInfo.IdentifyID);
                    if (identifyInfo != null && !string.IsNullOrEmpty(identifyInfo.IconUrl))
                    {
                        icons += string.Format(@"<span class=""list_ico""><img title=""{0}"" src=""{1}"" /></span>", identifyInfo.Title, PageUtilityBBS.GetBBSUrl(publishmentSystemID, identifyInfo.IconUrl));
                    }
                }
            }
            return icons;
        }

        // start 缓存

        private static readonly object lockObject = new object();

        private static string GetCacheKey(int publishmentSystemID)
        {
            return string.Format("SiteServer.BBS.Core.ThreadManager.Top.{0}", publishmentSystemID);
        }

        private static Hashtable GetTopLevelCacheHashtable(int publishmentSystemID)
        {
            lock (lockObject)
            {
                string cacheKey = GetCacheKey(publishmentSystemID);
                if (CacheUtils.Get(cacheKey) == null)
                {
                    Hashtable hashtable = new Hashtable();
                    CacheUtils.Max(cacheKey, hashtable);
                    return hashtable;
                }
                return CacheUtils.Get(cacheKey) as Hashtable;
            }
        }

        public static ArrayList GetTopLevelThradInfoArrayList(int publishmentSystemID, int areaID, int forumID)
        {
            ArrayList arraylist = new ArrayList();

            for (int topLevel = 3; topLevel >= 1; topLevel--)
            {
                arraylist.AddRange(GetTopLevelThradInfoArrayList(publishmentSystemID, topLevel, areaID, forumID));
            }
            return arraylist;
        }

        private static ArrayList GetTopLevelThradInfoArrayList(int publishmentSystemID, int topLevel, int areaID, int forumID)
        {
            ArrayList arraylist = new ArrayList();
            Hashtable hashtable = ThreadManager.GetTopLevelCacheHashtable(publishmentSystemID);
            if (hashtable != null)
            {
                string key = GetCacheKeyOfTop(topLevel, areaID, forumID);
                if (hashtable.ContainsKey(key))
                {
                    arraylist = hashtable[key] as ArrayList;
                }
                else
                {
                    arraylist = DataProvider.ThreadDAO.GetTopLevelThreadInfoArrayList(publishmentSystemID, topLevel, areaID, forumID);
                    hashtable[key] = arraylist;
                }
            }
            return arraylist;
        }

        public static void RemoveCacheOfTop(int publishmentSystemID)
        {
            string cacheKey = GetCacheKey(publishmentSystemID);
            CacheUtils.Remove(cacheKey);
        }

        private static string GetCacheKeyOfTop(int topLevel, int areaID, int forumID)
        {
            string key = string.Empty;
            if (topLevel == 3)//全局置顶
            {
                key = "t:" + topLevel;
            }
            else if (topLevel == 2)//分区置顶
            {
                key = "t:" + topLevel + ",a:" + areaID;
            }
            else//板块置顶
            {
                key = "f:" + forumID;
            }
            return key;
        }

        // end 缓存

        public static int GetPostPage(int publishmentSystemID, int taxis)
        {
            ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);
            int totalCount = taxis;
            int pageCount = totalCount / additional.PostPageNum;
            int mod = totalCount % additional.PostPageNum;
            if (mod > 0)
            {
                pageCount++;
            }
            return pageCount;
        }

        public static int GetPageCount(int publishmentSystemID, int replies)
        {
            return GetPostPage(publishmentSystemID, replies + 1);
        }

        public const string PostReferenceSeparator = "__PostReference__";

        public static string GetPostReferenceString(PostInfo postInfo, bool isPostPage)
        {
            string retval = string.Empty;
            if (postInfo != null)
            {
                string content = StringUtils.StripTagsExcludeBR(postInfo.Content);
                int indexOfSeparator1 = content.IndexOf(ThreadManager.PostReferenceSeparator);
                if (indexOfSeparator1 != -1)
                {
                    content = content.Substring(indexOfSeparator1 + ThreadManager.PostReferenceSeparator.Length);
                }
                if (isPostPage)
                {
                    content = StringUtils.MaxLengthText(content, 40);
                }
                if (postInfo.IsThread)
                {
                    retval = string.Format(@"引用 楼主({0}) 于 {1} 发表的 {2} : <br />{3}", postInfo.UserName, DateUtils.GetDateAndTimeString(postInfo.AddDate), postInfo.Title, content);
                }
                else
                {
                    retval = string.Format(@"引用 {0}({1}) 于 {2} 发表的 {3} : <br />{4}", StringUtilityBBS.GetFloorByTaxis(postInfo.Taxis), postInfo.UserName, DateUtils.GetDateAndTimeString(postInfo.AddDate), GetPostTitle(postInfo.Title), content);
                }
            }
            return retval;
        }

        public static string GetPostTitle(string title)
        {
            if (title != null)
            {
                return (title.StartsWith("Re:") ? string.Empty : title);
            }
            return string.Empty;
        }

        public static string GetPostContent(int publishmentSystemID, PostInfo postInfo, List<AttachmentInfo> attachmentInfoList)
        {
            if (postInfo.Content != null)
            {
                int indexOfSeparator1 = postInfo.Content.IndexOf(ThreadManager.PostReferenceSeparator);
                if (indexOfSeparator1 != -1)
                {
                    string refString = postInfo.Content.Substring(0, indexOfSeparator1);
                    string mainContent = postInfo.Content.Substring(indexOfSeparator1 + ThreadManager.PostReferenceSeparator.Length);
                    mainContent = StringUtilityBBS.TextEditorContentDecode(publishmentSystemID, mainContent);

                    int indexOfSeparator2 = refString.IndexOf("<br />");
                    if (indexOfSeparator2 != -1)
                    {
                        string title = refString.Substring(0, indexOfSeparator2);
                        refString = refString.Substring(indexOfSeparator2 + 6);
                        postInfo.Content = string.Format(@"<div class=""post_ref""><div class=""post_ref_title"">{0}</div>{1}<p class=""post_ref_p""></p></div><br />{2}", title, refString, mainContent);
                    }
                }
                else
                {
                    postInfo.Content = StringUtilityBBS.TextEditorContentDecode(publishmentSystemID, postInfo.Content);
                }

                //postInfo.Content = PageUtils.GetSafeHtmlFragment(postInfo.Content);

                return UBBUtility.Parse(publishmentSystemID, postInfo, attachmentInfoList);
            }
            return string.Empty;
        }

        public static string GetPostContentWithoutReference(int publishmentSystemID, string content)
        {
            if (content != null)
            {
                int indexOfSeparator1 = content.IndexOf(ThreadManager.PostReferenceSeparator);
                if (indexOfSeparator1 != -1)
                {
                    string mainContent = content.Substring(indexOfSeparator1 + ThreadManager.PostReferenceSeparator.Length);
                    mainContent = StringUtilityBBS.TextEditorContentDecode(publishmentSystemID, mainContent);
                    return mainContent;
                }
                else
                {
                    string value = StringUtilityBBS.TextEditorContentDecode(publishmentSystemID, content);
                    return StringUtils.HtmlEncode(value);
                }
            }
            return string.Empty;
        }

        public static string GetPostReference(string content)
        {
            if (content != null)
            {
                int indexOfSeparator1 = content.IndexOf(ThreadManager.PostReferenceSeparator);
                if (indexOfSeparator1 != -1)
                {
                    return content.Substring(0, indexOfSeparator1);
                }
            }
            return string.Empty;
        }

        public static string GetBannedContent()
        {
            return string.Format(@"<div class=""alert"">提示: 该帖被管理员或版主屏蔽</div>");
        }
        public static string GetDeleteContent()
        {
                return string.Format(@"<div class=""alert"">提示: 该帖被管理员或版主删除</div>");
        }
        public static string GetCheckedContent()
        {
            return string.Format(@"<div class=""alert"">提示: 该帖含有敏感词，尚未被管理员审核通过</div>");
        }
    }
}
