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
    public class Lock : Page
    {
        private int publishmentSystemID;
        private int forumID;
        private int threadID;
        private string threadIDArray;
        private bool isPostBack;

        public static string GetOpenWindowString(int publishmentSystemID, int forumID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            return DialogUtility.GetOpenWindowStringWithCheckBoxValue(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "lock.aspx"), arguments, "threadIDArray", "请选择主题进行操作", 360, 300, "锁定", string.Empty);
        }

        public static string GetOpenWindowString(int publishmentSystemID, int forumID, int threadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            return DialogUtility.GetOpenWindowString(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "lock.aspx"), arguments, 360, 300, "锁定", string.Empty);
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
                    bool isLocked = TranslateUtils.ToBool(base.Request.Form["isLocked"]);
                    bool isMessage = TranslateUtils.ToBool(base.Request.Form["isMessage"]);
                    string reason = PageUtils.FilterSqlAndXss(base.Request.Form["reason"]);

                    NameValueCollection attributes = new NameValueCollection();

                    string errorMessage = string.Empty;
                    bool success = false;

                    try
                    {
                        if (this.threadID > 0)
                        {
                            DataProvider.ThreadDAO.UpdateIsLocked(this.threadID.ToString(), isLocked);
                            if (isMessage)
                            {
                                ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, this.threadID);
                                if (threadInfo != null)
                                {
                                    string content = StringUtilityBBS.GetSystemMessageContent(isLocked ? "锁定" : "解锁", threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                    UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(this.threadIDArray))
                        {
                            DataProvider.ThreadDAO.UpdateIsLocked(this.threadIDArray, isLocked);
                            if (isMessage)
                            {
                                foreach (int theThreadID in TranslateUtils.StringCollectionToIntArrayList(this.threadIDArray))
                                {
                                    ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, theThreadID);
                                    if (threadInfo != null)
                                    {
                                        string content = StringUtilityBBS.GetSystemMessageContent(isLocked ? "锁定" : "解锁", threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
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
                        errorMessage = "主题操作失败，" + ex.Message;
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
