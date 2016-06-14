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
    public class Identify : Page
    {
        private int publishmentSystemID;
        private int forumID;
        private int threadID;
        private string threadIDArray;
        private int identifyID;
        private bool isPostBack;

        public static string GetOpenWindowString(int publishmentSystemID, int forumID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            return DialogUtility.GetOpenWindowStringWithCheckBoxValue(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "identify.aspx"), arguments, "threadIDArray", "请选择主题进行操作", 360, 390, "鉴定", string.Empty);
        }

        public static string GetOpenWindowString(int publishmentSystemID, int forumID, int threadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            return DialogUtility.GetOpenWindowString(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "identify.aspx"), arguments, 360, 390, "鉴定", string.Empty);
        }

        public string GetOptions()
        {
            IList<IdentifyInfo> list = DataProvider.IdentifyDAO.GetIdentifyList(this.publishmentSystemID);
            
            StringBuilder builder = new StringBuilder();
            builder.Append(@"<option value=""""><<取消鉴定>></option>");
            foreach (IdentifyInfo info in list)
            {
                string iconUrl = string.Empty;
                if (!string.IsNullOrEmpty(info.IconUrl))
                {
                    iconUrl = PageUtilityBBS.GetBBSUrl(this.publishmentSystemID, info.IconUrl);
                }
                string stampUrl = string.Empty;
                if (!string.IsNullOrEmpty(info.StampUrl))
                {
                    stampUrl = PageUtilityBBS.GetBBSUrl(this.publishmentSystemID, info.StampUrl);
                }
                builder.AppendFormat(@"<option value=""{0}"" iconUrl=""{1}"" stampUrl=""{2}"">{3}</option>", info.ID, iconUrl, stampUrl, info.Title);
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
                    this.identifyID = TranslateUtils.ToInt(base.Request.Form["identifyID"]);
                    bool isMessage = TranslateUtils.ToBool(base.Request.Form["isMessage"]);
                    string reason = PageUtils.FilterSqlAndXss(base.Request.Form["reason"]);

                    NameValueCollection attributes = new NameValueCollection();

                    string errorMessage = string.Empty;
                    bool success = false;
                    string url = string.Empty;

                    try
                    {
                        if (this.threadID > 0)
                        {
                            DataProvider.ThreadDAO.UpdateIdentifyID(this.threadID.ToString(), this.identifyID);

                            if (isMessage)
                            {
                                ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, this.threadID);
                                if (threadInfo != null)
                                {
                                    string content = StringUtilityBBS.GetSystemMessageContent("鉴定", threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                    UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                                }
                            }

                            success = true;
                            url = PageUtilityBBS.GetThreadUrl(this.publishmentSystemID, this.forumID, this.threadID);
                        }
                        else if (!string.IsNullOrEmpty(this.threadIDArray))
                        {
                            DataProvider.ThreadDAO.UpdateIdentifyID(this.threadIDArray, this.identifyID);
                            foreach (int theThreadID in TranslateUtils.StringCollectionToIntArrayList(this.threadIDArray))
                            {
                                if (isMessage)
                                {
                                    ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(this.publishmentSystemID, theThreadID);
                                    if (threadInfo != null)
                                    {
                                        string content = StringUtilityBBS.GetSystemMessageContent("鉴定", threadInfo.Title, threadInfo.AddDate, ForumManager.GetForumName(this.publishmentSystemID, threadInfo.ForumID), reason);
                                        UserMessageManager.SendSystemMessage(threadInfo.UserName, content);
                                    }
                                }
                            }

                            success = true;
                            url = PageUtilityBBS.GetForumUrl(this.publishmentSystemID, this.forumID);
                        }
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        errorMessage = "鉴定主题失败，" + ex.Message;
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
