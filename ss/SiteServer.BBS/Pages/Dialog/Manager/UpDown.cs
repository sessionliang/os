using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;



using SiteServer.BBS.Model;
using BaiRong.Core;
using System.Collections.Specialized;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.Pages.Dialog
{
    public class UpDown : Page
    {
        private int publishmentSystemID;
        private int forumID;
        private int threadID;
        private string threadIDArray;
        private string postIDArray;
        private bool isPostBack;

        public static string GetOpenWindowStringUpDownThread(int publishmentSystemID, int forumID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            return DialogUtility.GetOpenWindowStringWithCheckBoxValue(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "upDown.aspx"), arguments, "threadIDArray", "请选择主题进行操作", 360, 300, "提升下沉", string.Empty);
        }

        public static string GetOpenWindowStringUpDownPost(int publishmentSystemID, int forumID, int threadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            return DialogUtility.GetOpenWindowStringWithCheckBoxValue(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "upDown.aspx"), arguments, "postIDArray", "请选择帖子进行操作", 360, 300, "升降", string.Empty);
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
                
                this.isPostBack = TranslateUtils.ToBool(base.Request.QueryString["isPostBack"]);
                if (isPostBack)
                {
                    string type = PageUtils.FilterSqlAndXss(base.Request.Form["type"]);
                    bool isMessage = TranslateUtils.ToBool(base.Request.Form["isMessage"]);
                    string reason = PageUtils.FilterSqlAndXss(base.Request.Form["reason"]);

                    NameValueCollection attributes = new NameValueCollection();

                    string errorMessage = string.Empty;
                    bool success = false;
                    string url = string.Empty;

                    if (this.threadID == 0)//主题
                    {
                        try
                        {
                            if (isMessage)
                            {
                                foreach (int theThreadID in TranslateUtils.StringCollectionToIntArrayList(this.threadIDArray))
                                {
                                    ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, theThreadID);
                                    if (threadInfo != null)
                                    {
                                        string content = StringUtilityBBS.GetSystemMessageContent("升降", threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                        UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                                    }
                                }
                            }

                            DataProvider.ThreadDAO.UpDownThread(this.threadIDArray, StringUtils.EqualsIgnoreCase(type, "Up"), this.forumID);
                            success = true;
                            url = PageUtilityBBS.GetForumUrl(this.publishmentSystemID, this.forumID);
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            errorMessage = "主题提升下沉失败，" + ex.Message;
                        }
                    }
                    else//帖子
                    {
                        try
                        {
                            if (isMessage)
                            {
                                foreach (int thePostID in TranslateUtils.StringCollectionToIntArrayList(this.postIDArray))
                                {
                                    PostInfo postInfo = DataProvider.PostDAO.GetPostInfo(this.publishmentSystemID, thePostID);
                                    if (postInfo != null)
                                    {
                                        string content = StringUtilityBBS.GetSystemMessageContent("升降", postInfo.Title, postInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, postInfo.ForumID), reason);
                                        UserMessageManager.SendSystemMessage(postInfo.UserName, content);
                                    }
                                }
                            }
                            DataProvider.PostDAO.UpDownPost(this.publishmentSystemID, this.postIDArray, StringUtils.EqualsIgnoreCase(type, "Up"), this.threadID);

                            success = true;
                            url = PageUtilityBBS.GetThreadUrl(this.publishmentSystemID, this.forumID, this.threadID);
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            errorMessage = "帖子提升下沉失败，" + ex.Message;
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
