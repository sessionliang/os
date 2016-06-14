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
    public class Translate : Page
    {
        private int publishmentSystemID;
        private int forumID;
        private int threadID;
        private string threadIDArray;
        private int targetForumID;
        private bool isRedirect;
        private bool isPostBack;

        public static string GetOpenWindowString(int publishmentSystemID, int forumID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            return DialogUtility.GetOpenWindowStringWithCheckBoxValue(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "translate.aspx"), arguments, "threadIDArray", "请选择主题进行操作", 360, 300, "移动主题", string.Empty);
        }

        public static string GetOpenWindowString(int publishmentSystemID, int forumID, int threadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            return DialogUtility.GetOpenWindowString(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "translate.aspx"), arguments, 360, 300, "移动主题", string.Empty);
        }

        public string GetOptions()
        {
            ListItemCollection listItemCollection = new ListItemCollection();
            ForumManager.AddListItems(this.publishmentSystemID, listItemCollection, true);
            StringBuilder builder = new StringBuilder();
            foreach (ListItem listItem in listItemCollection)
            {
                ForumInfo forumInfo = ForumManager.GetForumInfo(this.publishmentSystemID, TranslateUtils.ToInt(listItem.Value));
                if (forumInfo.ParentsCount <= 1)
                {
                    if (builder.Length > 0)
                    {
                        builder.AppendFormat(@"</OPTGROUP>");
                    }
                    builder.AppendFormat(@"<OPTGROUP label=""{0}"">", forumInfo.ForumName);
                }
                else
                {
                    if (!string.IsNullOrEmpty(forumInfo.LinkUrl))
                    {
                        builder.AppendFormat(@"<option value=""0"" style=""color:gray;"">{0}</option>", listItem.Text);
                    }
                    else
                    {
                        builder.AppendFormat(@"<option value=""{0}"">{1}</option>", listItem.Value, listItem.Text);
                    }
                }
            }
            return builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
                this.forumID = TranslateUtils.ToInt(base.Request.QueryString["forumID"]);
                this.threadID = TranslateUtils.ToInt(base.Request.QueryString["threadID"]);
                this.threadIDArray = PageUtils.FilterSqlAndXss(base.Request.QueryString["threadIDArray"]);
                
                this.isPostBack = TranslateUtils.ToBool(base.Request.QueryString["isPostBack"]);
                if (isPostBack)
                {
                    this.targetForumID = TranslateUtils.ToInt(base.Request.Form["targetForumID"]);
                    this.isRedirect = TranslateUtils.ToBool(base.Request.Form["isRedirect"]);
                    bool isMessage = TranslateUtils.ToBool(base.Request.Form["isMessage"]);
                    string reason = PageUtils.FilterSqlAndXss(base.Request.Form["reason"]);

                    NameValueCollection attributes = new NameValueCollection();

                    string errorMessage = string.Empty;
                    bool success = false;
                    string url = string.Empty;

                    if (this.forumID == this.targetForumID)
                    {
                        errorMessage = "目标板块不能选择当前板块";
                    }
                    else if (this.targetForumID == 0)
                    {
                        errorMessage = "无法转移到目标板块";
                    }

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        try
                        {
                            if (this.threadID > 0)
                            {
                                if (isMessage)
                                {
                                    ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, this.threadID);
                                    if (threadInfo != null)
                                    {
                                        string content = StringUtilityBBS.GetSystemMessageContent("转移", threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                        UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                                    }
                                }

                                DataProvider.ThreadDAO.TranslateThread(this.publishmentSystemID, this.threadID.ToString(), this.targetForumID);
                            }
                            else if (!string.IsNullOrEmpty(this.threadIDArray))
                            {
                                ArrayList threadIDArrayList = TranslateUtils.StringCollectionToIntArrayList(this.threadIDArray);
                                if (isMessage)
                                {
                                    foreach (int theThreadID in threadIDArrayList)
                                    {
                                        ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, theThreadID);
                                        if (threadInfo != null)
                                        {
                                            string content = StringUtilityBBS.GetSystemMessageContent("转移", threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                            UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                                        }
                                    }
                                }
                                DataProvider.ThreadDAO.TranslateThread(this.publishmentSystemID, this.threadIDArray, this.targetForumID);
                            }

                            url = PageUtilityBBS.GetForumUrl(this.publishmentSystemID, this.forumID);
                            success = true;
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            errorMessage = "移动主题失败，" + ex.Message;
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
