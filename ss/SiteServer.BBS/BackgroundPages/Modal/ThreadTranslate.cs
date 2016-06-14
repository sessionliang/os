using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Core;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class ThreadTranslate : BackgroundBasePage
    {

        protected DropDownList ddlTargetForumID;
        private bool[] isLastNodeArray;

        string threadIDList = "";
        int sourceForumID = 0;

        /// <summary>
        /// 暂不用
        /// </summary>
        /// <param name="threadIDList"></param>
        /// <param name="sourceForumID"></param>
        /// <param name="chbServerID"></param>
        /// <param name="chbClientID"></param>
        /// <returns></returns>
        public static string GetOpenWindowString(int publishmentSystemID, string threadIDList, int sourceForumID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("threadIDList", threadIDList);
            arguments.Add("sourceForumID", sourceForumID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("主题转移", PageUtils.GetBBSUrl("modal_threadTranslate.aspx"), arguments, 450, 200);
        }

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("主题转移", PageUtils.GetBBSUrl("modal_threadTranslate.aspx"), arguments, "ThreadIDCollection", "请选择需要转移的主题", 450, 200);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("ThreadIDCollection") == null)
            {
                base.FailMessage("参数出错！");
                return;
            }
            threadIDList = base.GetQueryString("ThreadIDCollection");
            if (base.GetQueryString("sourceForumID") != null)
                sourceForumID = base.GetIntQueryString("sourceForumID");
            if (!IsPostBack)
            {
                ArrayList forumIDArrayList = DataProvider.ForumDAO.GetForumIDArrayList(base.PublishmentSystemID);
                if (forumIDArrayList != null)
                {
                    int forumCount = forumIDArrayList.Count;
                    this.isLastNodeArray = new bool[forumCount + 1];
                    ListItem listItem = null;
                    string value = "";
                    ForumInfo forumInfo = null;
                    foreach (int theForumID in forumIDArrayList)
                    {
                        forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, theForumID);
                        value = (forumInfo.ParentID > 0) ? theForumID.ToString() : "";
                        listItem = new ListItem(ForumManager.GetSelectText(forumInfo, isLastNodeArray, true), value);
                        if (sourceForumID == theForumID)
                            listItem.Selected = true;
                        ddlTargetForumID.Items.Add(listItem);
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            int targetForumID = TranslateUtils.ToInt(this.ddlTargetForumID.SelectedValue);
            if (targetForumID <= 0)
            {
                base.FailMessage("请选择要转移到的目标版块！");
                return;
            }
            try
            {
                DataProvider.ThreadDAO.TranslateThread(base.PublishmentSystemID, threadIDList, targetForumID);
                StringUtilityBBS.AddLog(base.PublishmentSystemID, "主题转移成功", string.Format("主题IDList:{0}转移到版块:{1}", threadIDList, targetForumID));
                isChanged = true;
            }
            catch(Exception ex)
            {
                isChanged = false;
                base.FailMessage(ex, ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundThread.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}