using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using BaiRong.Model;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.Pages
{
    public class RedirectPage : BasePage
    {
        private const string Type_Forum = "forum";
        private const string Type_Thread = "thread";
        private const string Type_Post = "post";
        private const string Type_Floor = "floor";
        private const string Type_Download = "download";

        public static string GetRedirectUrlByForum(int publishmentSystemID, int forumID)
        {
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("redirect.aspx?publishmentSystemID={0}&type=forum&forumID={1}", publishmentSystemID, forumID));
        }

        public static string GetRedirectUrlByThread(int publishmentSystemID, int forumID, int threadID, int page)
        {
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("redirect.aspx?publishmentSystemID={0}&type=thread&forumID={1}&threadID={2}&page={3}", publishmentSystemID, forumID, threadID, page));
        }

        public static string GetRedirectUrlByPost(int publishmentSystemID, int forumID, int threadID, int postID)
        {
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("redirect.aspx?publishmentSystemID={0}&type=post&forumID={1}&threadID={2}&postID={3}", publishmentSystemID, forumID, threadID, postID));
        }

        public static string GetRedirectUrlByFloor(int publishmentSystemID, int forumID, int threadID)
        {
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("redirect.aspx?publishmentSystemID={0}&type=floor&forumID={1}&threadID={2}&floor=", publishmentSystemID, forumID, threadID));
        }

        public static string GetRedirectUrl(int publishmentSystemID, int attachmentID)
        {
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("redirect.aspx?publishmentSystemID={0}&type=download&attachmentID={1}", publishmentSystemID, attachmentID));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            string type = PageUtils.FilterSqlAndXss(base.Request.QueryString["type"]);
            if (StringUtils.EqualsIgnoreCase(type, Type_Forum))
            {
                int forumID = base.GetIntQueryString("forumID");
                ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, forumID);
                PageUtils.Redirect(PageUtilityBBS.GetForumUrl(base.PublishmentSystemID, forumInfo));
            }
            else if (StringUtils.EqualsIgnoreCase(type, Type_Thread))
            {
                int forumID = base.GetIntQueryString("forumID");
                int threadID = base.GetIntQueryString("threadID");
                int page = base.GetIntQueryString("page");
                PageUtils.Redirect(PageUtilityBBS.GetThreadUrl(base.PublishmentSystemID, forumID, threadID, page));
            }
            else if (StringUtils.EqualsIgnoreCase(type, Type_Post))
            {
                int forumID = base.GetIntQueryString("forumID");
                int threadID = base.GetIntQueryString("threadID");
                int postID = base.GetIntQueryString("postID");

                PostInfo postInfo = DataProvider.PostDAO.GetPostInfo(base.PublishmentSystemID, postID);
                int page = ThreadManager.GetPostPage(base.PublishmentSystemID, postInfo.Taxis);
                PageUtils.Redirect(PageUtilityBBS.GetPostUrl(base.PublishmentSystemID, forumID, threadID, page, postID));
            }
            else if (StringUtils.EqualsIgnoreCase(type, Type_Floor))
            {
                int forumID = base.GetIntQueryString("forumID");
                int threadID = base.GetIntQueryString("threadID");
                int floor = base.GetIntQueryString("floor");

                int postID = DataProvider.PostDAO.GetPostIDByTaxis(base.PublishmentSystemID, forumID, threadID, floor);
                if (postID == 0)
                {
                    PageUtils.Redirect(PageUtilityBBS.GetThreadUrl(base.PublishmentSystemID, forumID, threadID));
                }
                else
                {
                    int page = ThreadManager.GetPostPage(base.PublishmentSystemID, floor);
                    PageUtils.Redirect(PageUtilityBBS.GetPostUrl(base.PublishmentSystemID, forumID, threadID, page, postID));
                }
            }
            else if (StringUtils.EqualsIgnoreCase(type, Type_Download))
            {
                bool isSuccess = false;
                int attachmentID = base.GetIntQueryString("attachmentID");
                if (attachmentID > 0)
                {
                    AttachmentInfo attachmentInfo = DataProvider.AttachmentDAO.GetAttachmentInfo(attachmentID);
                    if (attachmentInfo != null)
                    {
                        try
                        {
                            string fileUrl = attachmentInfo.AttachmentUrl;

                            string filePath = PathUtility.GetPublishmentSystemPath(base.PublishmentSystemID, fileUrl);
                            if (FileUtils.IsFileExists(filePath))
                            {
                                isSuccess = true;
                                DataProvider.AttachmentDAO.AddDownloadCount(attachmentID);
                                PageUtils.Download(base.Response, filePath, attachmentInfo.FileName);
                            }
                        }
                        catch { }
                    }
                }
                if (!isSuccess)
                {
                    base.Response.Write("下载失败，不存在此文件！");
                }
            }            
        }
    }
}
