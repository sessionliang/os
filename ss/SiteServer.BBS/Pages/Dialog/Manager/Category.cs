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
    public class Category : Page
    {
        private int publishmentSystemID;
        private int forumID;
        private int threadID;
        private string threadIDArray;
        private int categoryID;
        private bool isPostBack;

        public static string GetOpenWindowString(int publishmentSystemID, int forumID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            if (ThreadCategoryManager.GetThreadCategoryInfoArrayList(publishmentSystemID, forumID).Count == 0)
            {
                return DialogUtility.FailureMessage("该板块没有设置主题分类");
            }
            else
            {
                return DialogUtility.GetOpenWindowStringWithCheckBoxValue(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "category.aspx"), arguments, "threadIDArray", "请选择主题进行操作", 360, 300, "主题分类", string.Empty);
            }
        }

        public static string GetOpenWindowString(int publishmentSystemID, int forumID, int threadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            if (ThreadCategoryManager.GetThreadCategoryInfoArrayList(publishmentSystemID, forumID).Count == 0)
            {
                return DialogUtility.FailureMessage("该板块没有设置主题分类");
            }
            else
            {
                return DialogUtility.GetOpenWindowString(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "category.aspx"), arguments, 360, 300, "主题分类", string.Empty);
            }
        }

        public string GetOptions()
        {
            ArrayList categoryInfoArrayList = DataProvider.ThreadCategoryDAO.GetCategoryInfoArrayList(this.publishmentSystemID, this.forumID);

            StringBuilder builder = new StringBuilder();
            builder.Append(@"<option value=""0""><取消分类></option>");

            foreach (ThreadCategoryInfo categoryInfo in categoryInfoArrayList)
            {
                builder.AppendFormat(@"<option value=""{0}"">{1}</option>", categoryInfo.CategoryID, categoryInfo.CategoryName);
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
                    this.categoryID = TranslateUtils.ToInt(base.Request.Form["categoryID"]);
                    bool isMessage = TranslateUtils.ToBool(base.Request.Form["isMessage"]);
                    string reason = PageUtils.FilterSqlAndXss(base.Request.Form["reason"]);

                    NameValueCollection attributes = new NameValueCollection();

                    string errorMessage = string.Empty;
                    bool success = false;

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        try
                        {
                            if (this.threadID > 0)
                            {
                                DataProvider.ThreadDAO.CategoryThread(this.threadID.ToString(), this.categoryID);

                                if (isMessage)
                                {
                                    ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, this.threadID);
                                    string content = StringUtilityBBS.GetSystemMessageContent("设置分类", threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                    UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                                }
                            }
                            else if (!string.IsNullOrEmpty(this.threadIDArray))
                            {
                                DataProvider.ThreadDAO.CategoryThread(this.threadIDArray, this.categoryID);
                                foreach (int theThreadID in TranslateUtils.StringCollectionToIntArrayList(this.threadIDArray))
                                {
                                    if (isMessage)
                                    {
                                        ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, theThreadID);
                                        string content = StringUtilityBBS.GetSystemMessageContent("设置分类", threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                        UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                                    }
                                }
                            }

                            success = true;
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            errorMessage = "主题分类失败，" + ex.Message;
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
