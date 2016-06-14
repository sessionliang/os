using System;
using System.Text;
using SiteServer.BBS.Model;
using BaiRong.Core;
using SiteServer.BBS.Pages;


namespace SiteServer.BBS.Core
{
    public class ForumUtils
    {
        private int publishmentSystemID;
        private ForumUtils(int publishmentSystemID)
        {
            this.publishmentSystemID = publishmentSystemID;
        }

        public static ForumUtils GetInstance(int publishmentSystemID)
        {
            return new ForumUtils(publishmentSystemID);
        }

        // 判断今日是否有新主题或者帖子
        public bool IsNewThread(int forumID)
        {
            return GetTodayThreadAndPostCount(forumID) > 0;
        }

        public string GetForumIcon(int forumID)
        {
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (string.IsNullOrEmpty(forumInfo.IconUrl))
            {
                int postCount = GetTodayThreadAndPostCount(forumID);
                if (postCount > 0)
                {
                    return @"<img src=""images/forum_new.gif"" />";
                }
                else
                {
                    return @"<img src=""images/forum.gif"" />";
                }
            }
            else
            {
                return string.Format(@"<img src=""{0}"" />", PageUtilityBBS.ParseNavigationUrl(forumInfo.IconUrl));
            }
        }

        // 获取今日主题及帖子总数
        public int GetTodayThreadAndPostCount(int forumID)
        {
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo.LastPostID > 0)
            {
                if (forumInfo.LastDate.DayOfYear == DateTime.Now.DayOfYear)
                {
                    return forumInfo.TodayThreadCount + forumInfo.TodayPostCount;
                }
            }
            return 0;
        }

        // 获取主题总数
        public int GetThreadCount(int forumID)
        {
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            return forumInfo.ThreadCount;
        }

        // 获取帖子总数
        public int GetPostCount(int forumID)
        {
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            return forumInfo.PostCount;
        }

        // 获取最后发表时间
        public string GetLastDate(int forumID)
        {
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo.LastPostID > 0)
            {
                return string.Format(@"最后发表：<a href=""{0}"">{1}</a>", RedirectPage.GetRedirectUrlByPost(publishmentSystemID, forumID, forumInfo.LastThreadID, forumInfo.LastPostID), DateUtils.ParseThisMoment(forumInfo.LastDate, DateTime.Now));
            }
            return string.Empty;
        }

        public string GetLastTitle(int forumID)
        {
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo.LastPostID > 0)
            {
                return string.Format(@"<a href=""{0}"">{1}</a>", RedirectPage.GetRedirectUrlByPost(publishmentSystemID, forumID, forumInfo.LastThreadID, forumInfo.LastPostID), forumInfo.LastTitle);
            }
            return string.Empty;
        }

        public string GetLastUserNameAndTime(int forumID)
        {
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo.LastPostID > 0)
            {
                return string.Format(@"by <a href=""{0}"" >{1}</a> - <a href=""{2}"">{3}</a>", UserUtils.GetInstance(publishmentSystemID).GetUserUrl(forumInfo.LastUserName), forumInfo.LastUserName, RedirectPage.GetRedirectUrlByPost(publishmentSystemID, forumID, forumInfo.LastThreadID, forumInfo.LastPostID), DateUtils.ParseThisMoment(forumInfo.LastDate, DateTime.Now));
            }
            return string.Empty;
        }

        public string GetHideContent(int forumID, int threadID, int postID, string userName, string content)
        {
            bool isVisible = false;

            if (BBSUserManager.IsModerator(ForumManager.GetForumInfo(publishmentSystemID, forumID)))
            {
                isVisible = true;
            }
            else if (userName == BaiRongDataProvider.UserDAO.CurrentUserName)
            {
                isVisible = true;
            }
            else
            {
                isVisible = DataProvider.PostDAO.IsReply(publishmentSystemID, threadID, BaiRongDataProvider.UserDAO.CurrentUserName);
            }

            if (isVisible)
            {
                return "<div class=\"alert\">本部分内容设置为隐藏，需要回复后才能看到：<br /><br />" + RuntimeUtils.DecryptStringByTranslate(content) + "</div>";
            }
            else
            {
                return "<div class=\"alert\">本部分内容设置为隐藏，需要回复后才能看到</div>";
            }
        }
    }
}
