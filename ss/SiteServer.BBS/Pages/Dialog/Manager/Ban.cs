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
    public class Ban : Page
    {
        private int publishmentSystemID;
        private int forumID;
        private int threadID;
        private string threadIDArray;
        private string postIDArray;
        private string action;
        private bool isPostBack;

        public static string GetOpenWindowStringBanThreads(int publishmentSystemID, int forumID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("action", "BanThreads");
            return DialogUtility.GetOpenWindowStringWithCheckBoxValue(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "ban.aspx"), arguments, "threadIDArray", "ÇëÑ¡ÔñÖ÷Ìâ½øÐÐ²Ù×÷", 360, 300, "ÆÁ±ÎÌû×Ó", string.Empty);
        }

        public static string GetOpenWindowStringBanPost(int publishmentSystemID, int forumID, int threadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            arguments.Add("action", "BanPost");
            return DialogUtility.GetOpenWindowStringWithCheckBoxValue(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "ban.aspx"), arguments, "postIDArray", "ÇëÑ¡Ôñ»Ø¸´½øÐÐ²Ù×÷", 300, 280, "ÆÁ±ÎÌû×Ó", string.Empty);
        }

        public static string GetOpenWindowStringBanThread(int publishmentSystemID, int forumID, int threadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            arguments.Add("action", "BanThread");
            return DialogUtility.GetOpenWindowString(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "ban.aspx"), arguments, 300, 280, "ÆÁ±ÎÌû×Ó", string.Empty);
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
                this.action = PageUtils.FilterSqlAndXss(base.Request.QueryString["action"]);
                this.isPostBack = TranslateUtils.ToBool(base.Request.QueryString["isPostBack"]);
                if (isPostBack)
                {
                    bool isBanned = TranslateUtils.ToBool(base.Request.Form["isBanned"]);
                    bool isMessage = TranslateUtils.ToBool(base.Request.Form["isMessage"]);
                    string reason = PageUtils.FilterSqlAndXss(base.Request.Form["reason"]);

                    NameValueCollection attributes = new NameValueCollection();

                    string errorMessage = string.Empty;
                    bool success = false;
                    string url = string.Empty;

                    try
                    {
                        if (this.action == "BanThreads")
                        {
                            List<int> threadIDList = TranslateUtils.StringCollectionToIntList(this.threadIDArray);
                            DataProvider.PostDAO.BanThreadIDList(this.publishmentSystemID, this.forumID, threadIDList, isBanned);

                            foreach (int theThreadID in threadIDList)
                            {
                                if (isMessage)
                                {
                                    ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, theThreadID);
                                    string content = StringUtilityBBS.GetSystemMessageContent((isBanned ? "ÆÁ±Î" : "½â³ýÆÁ±Î"), threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                    UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                                }
                            }

                            success = true;
                            url = PageUtilityBBS.GetForumUrl(this.publishmentSystemID, this.forumID);
                        }
                        else if (this.action == "BanPost")
                        {
                            DataProvider.PostDAO.Ban(this.publishmentSystemID, this.forumID, TranslateUtils.StringCollectionToIntList(this.postIDArray), isBanned);

                            success = true;
                            url = PageUtilityBBS.GetThreadUrl(this.publishmentSystemID, this.forumID, this.threadID);
                        }
                        else if (this.action == "BanThread")
                        {
                            DataProvider.PostDAO.BanThreadIDList(this.publishmentSystemID, this.forumID, TranslateUtils.ToIntList(this.threadID), isBanned);

                            if (isMessage)
                            {
                                ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, this.threadID);
                                string content = StringUtilityBBS.GetSystemMessageContent((isBanned ? "ÆÁ±Î" : "½â³ýÆÁ±Î"), threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                            }

                            success = true;
                            url = PageUtilityBBS.GetThreadUrl(this.publishmentSystemID, this.forumID, this.threadID);
                        }
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        errorMessage = "ÆÁ±Î/½â³ýÌû×ÓÊ§°Ü£¬" + ex.Message;
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
