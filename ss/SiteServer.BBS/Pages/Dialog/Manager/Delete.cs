using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;



using SiteServer.BBS.Model;
using BaiRong.Core;
using System.Collections.Specialized;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.Pages.Dialog
{
    public class Delete : Page
    {
        private int publishmentSystemID;
        private int forumID;
        private int threadID;
        private string threadIDArray;
        private string postIDArray;
        private string action;
        private bool isPostBack;

        public static string GetOpenWindowStringDeleteThread(int publishmentSystemID, int forumID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("action", "DeleteThread");
            return DialogUtility.GetOpenWindowStringWithCheckBoxValue(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "delete.aspx"), arguments, "threadIDArray", "请选择主题进行操作", 300, 260, "删除主题", string.Empty);
        }

        public static string GetOpenWindowStringDeleteThreadSingle(int publishmentSystemID, int forumID, int threadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            arguments.Add("action", "DeleteThreadSingle");
            return DialogUtility.GetOpenWindowString(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "delete.aspx"), arguments, 300, 260, "删除主题", string.Empty);
        }

        public static string GetOpenWindowStringDeletePost(int publishmentSystemID, int forumID, int threadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            arguments.Add("action", "DeletePost");
            return DialogUtility.GetOpenWindowStringWithCheckBoxValue(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "delete.aspx"), arguments, "postIDArray", "请选择帖子进行操作", 300, 260, "删除帖子", string.Empty);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
                this.forumID = TranslateUtils.ToInt(base.Request.QueryString["forumID"]);
                this.threadID = TranslateUtils.ToInt(base.Request.QueryString["threadID"]);
                this.threadIDArray = PageUtils.FilterSqlAndXss(base.Request.QueryString["threadIDArray"]);
                this.postIDArray = PageUtils.FilterSqlAndXss(base.Request.QueryString["postIDArray"]);
                this.action = PageUtils.FilterXSS(base.Request.QueryString["action"]);
                this.isPostBack = TranslateUtils.ToBool(base.Request.QueryString["isPostBack"]);

                if (isPostBack)
                {
                    bool isMessage = TranslateUtils.ToBool(base.Request.Form["isMessage"]);
                    string reason = PageUtils.FilterSqlAndXss(base.Request.Form["reason"]);

                    NameValueCollection attributes = new NameValueCollection();

                    string errorMessage = string.Empty;
                    bool success = false;
                    string url = string.Empty;

                    if (this.action == "DeleteThread")//删除主题
                    {
                        try
                        {
                            if (isMessage)
                            {
                                foreach (int theThreadID in TranslateUtils.StringCollectionToIntList(this.threadIDArray))
                                {
                                    ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, theThreadID);
                                    if (threadInfo != null)
                                    {
                                        string content = StringUtilityBBS.GetSystemMessageContent("删除", threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                        UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                                    }
                                }
                            }

                            DataProvider.ThreadDAO.DeleteThreadTrash(this.publishmentSystemID, this.forumID, TranslateUtils.StringCollectionToIntList(this.threadIDArray));
                            success = true;
                            url = PageUtilityBBS.GetForumUrl(this.publishmentSystemID, this.forumID);
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            errorMessage = "删除主题失败，" + ex.Message;
                        }
                    }
                    else if (this.action == "DeleteThreadSingle")//删除主题
                    {
                        try
                        {
                            if (isMessage)
                            {
                                ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, this.threadID);
                                if (threadInfo != null)
                                {
                                    string content = StringUtilityBBS.GetSystemMessageContent("删除", threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                    UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                                }
                            }

                            DataProvider.PostDAO.DeleteSingleThreadPostTrash(this.publishmentSystemID, this.forumID, TranslateUtils.ToIntList(this.threadID));
                            success = true;
                            url = PageUtilityBBS.GetForumUrl(this.publishmentSystemID, this.forumID);
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            errorMessage = "删除主题失败，" + ex.Message;
                        }
                    }
                    else if (this.action == "DeletePost")//删除帖子
                    {
                        try
                        {
                            List<int> postIDList = TranslateUtils.StringCollectionToIntList(this.postIDArray);
                            if (isMessage)
                            {
                                foreach (int postID in postIDList)
                                {
                                    PostInfo postInfo = DataProvider.PostDAO.GetPostInfo(this.publishmentSystemID, postID);
                                    if (postInfo != null)
                                    {
                                        string content = StringUtilityBBS.GetSystemMessageContent("删除", postInfo.Title, postInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, postInfo.ForumID), reason);
                                        UserMessageManager.SendSystemMessage(postInfo.UserName, content);
                                    }
                                }
                            }

                            DataProvider.PostDAO.DeletePostTrash(this.publishmentSystemID, this.forumID, this.threadID, postIDList);

                            success = true;
                            url = PageUtilityBBS.GetThreadUrl(this.publishmentSystemID, this.forumID, this.threadID);
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            errorMessage = "删除帖子失败，" + ex.Message;
                        }
                    }

                    attributes.Add("success", success.ToString().ToLower());
                    attributes.Add("url", url);
                    attributes.Add("errorMessage", errorMessage);

                    string json = TranslateUtils.NameValueCollectionToJsonString(attributes);
                    base.Response.Write(json);
                    base.Response.End();
                }
            }
        }
    }
}
