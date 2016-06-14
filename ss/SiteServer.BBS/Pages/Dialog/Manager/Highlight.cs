using System;
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
    public class Highlight : Page
    {
        private int publishmentSystemID;
        private int forumID;
        private int threadID;
        private string action;
        private string threadIDArray;
        private bool isPostBack;

        public static string GetOpenWindowString(int publishmentSystemID, string action, int forumID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("action", action);
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            return DialogUtility.GetOpenWindowStringWithCheckBoxValue(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "highlight.aspx"), arguments, "threadIDArray", "请选择主题进行操作", 400, 460, "主题管理", string.Empty);
        }

        public static string GetOpenWindowString(int publishmentSystemID, string action, int forumID, int threadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("action", action);
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            return DialogUtility.GetOpenWindowString(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "highlight.aspx"), arguments, 400, 460, "主题管理", string.Empty);
        }

        public string GetChecked(string type)
        {
            if (type == this.action)
            {
                return "checked";
            }
            return string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
                this.action = PageUtils.FilterSqlAndXss(base.Request.QueryString["action"]);
                this.forumID = TranslateUtils.ToInt(base.Request.QueryString["forumID"]);
                this.threadID = TranslateUtils.ToInt(base.Request.QueryString["threadID"]);
                this.threadIDArray = PageUtils.FilterSqlAndXss(base.Request.QueryString["threadIDArray"]);
                this.isPostBack = TranslateUtils.ToBool(base.Request.QueryString["isPostBack"]);
                if (isPostBack)
                {
                    bool isMessage = TranslateUtils.ToBool(base.Request.Form["isMessage"]);
                    string reason = PageUtils.FilterSqlAndXss(base.Request.Form["reason"]);

                    NameValueCollection attributes = new NameValueCollection();

                    string errorMessage = string.Empty;
                    bool success = false;

                    string actions = PageUtils.FilterSqlAndXss(base.Request.Form["actions"]);
                    if (string.IsNullOrEmpty(actions))
                    {
                        errorMessage = "至少需要选择置顶、精华或高亮中的一项进行操作";
                    }
                    else if (string.IsNullOrEmpty(threadIDArray) && this.threadID == 0)
                    {
                        errorMessage = "需要选择操作主题项";
                    }

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        try
                        {
                            bool isTopExists = StringUtils.Contains(actions, "top");
                            int topLevel = TranslateUtils.ToInt(base.Request.Form["topLevel"]);
                            string topLevelDate = PageUtils.FilterSqlAndXss(base.Request.Form["topLevelDate"]);

                            bool isDigestExists = StringUtils.Contains(actions, "digest");
                            int digestLevel = TranslateUtils.ToInt(base.Request.Form["digestLevel"]);
                            string digestDate = PageUtils.FilterSqlAndXss(base.Request.Form["digestDate"]);

                            bool isHighlightExists = StringUtils.Contains(actions, "highlight");
                            string highlight = StringUtilityBBS.GetHighlightFormatString(TranslateUtils.ToBool(base.Request.Form["highlight_B"]), TranslateUtils.ToBool(base.Request.Form["highlight_I"]), TranslateUtils.ToBool(base.Request.Form["highlight_U"]), PageUtils.FilterSqlAndXss(base.Request.Form["highlight_Color"]));
                            string highlightDate = PageUtils.FilterSqlAndXss(base.Request.Form["highlightDate"]);

                            int areaID = ForumManager.GetAreaID(this.publishmentSystemID, this.forumID);

                            if (this.threadID > 0)
                            {
                                DataProvider.ThreadDAO.HighlightThreads(this.publishmentSystemID, this.forumID, areaID, this.threadID.ToString(), isTopExists, topLevel, topLevelDate, isDigestExists, digestLevel, digestDate, isHighlightExists, highlight, highlightDate);
                                if (isMessage)
                                {
                                    string action = string.Empty;
                                    if (isTopExists)
                                    {
                                        action = "/置顶";
                                    }
                                    if (isDigestExists)
                                    {
                                        action += "/精华";
                                    }
                                    if (isHighlightExists)
                                    {
                                        action += "/高亮";
                                    }
                                    ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, this.threadID);
                                    if (threadInfo != null)
                                    {
                                        string content = StringUtilityBBS.GetSystemMessageContent(action.Trim('/'), threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                        UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                                    }
                                }
                            }
                            else if (!string.IsNullOrEmpty(this.threadIDArray))
                            {
                                DataProvider.ThreadDAO.HighlightThreads(this.publishmentSystemID, this.forumID, areaID, this.threadIDArray, isTopExists, topLevel, topLevelDate, isDigestExists, digestLevel, digestDate, isHighlightExists, highlight, highlightDate);
                                if (isMessage)
                                {
                                    string action = string.Empty;
                                    if (isTopExists)
                                    {
                                        action = "/置顶";
                                    }
                                    if (isDigestExists)
                                    {
                                        action += "/精华";
                                    }
                                    if (isHighlightExists)
                                    {
                                        action += "/高亮";
                                    }
                                    foreach (int theThreadID in TranslateUtils.StringCollectionToIntArrayList(this.threadIDArray))
                                    {
                                        ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, theThreadID);
                                        if (threadInfo != null)
                                        {
                                            string content = StringUtilityBBS.GetSystemMessageContent(action.Trim('/'), threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                            UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                                        }
                                    }
                                }
                            }

                            success = true;
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            errorMessage = "管理主题失败，" + ex.Message;
                        }
                    }

                    attributes.Add("success", success.ToString().ToLower());
                    attributes.Add("errorMessage", errorMessage);

                    string json = TranslateUtils.NameValueCollectionToJsonString(attributes);
                    base.Response.Write(json);
                    base.Response.End();
                }
            }
        }
    }
}
