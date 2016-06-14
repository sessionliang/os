using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;

using System.Collections;
using BaiRong.Model.Service;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class CheckState : BackgroundBasePage
    {
        public Literal ltlTitle;
        public Literal ltlState;
        public PlaceHolder phCheckReasons;
        public Repeater rpContents;
        public PlaceHolder phCheck;

        public PlaceHolder phCheckTask;
        public Literal ltlCheckTaskName;
        public Literal ltlCheckTaskDate;

        public PlaceHolder phUnCheckTask;
        public Literal ltlUnCheckTaskName;
        public Literal ltlUnCheckTaskDate;

        private int nodeID;
        private ETableStyle tableStyle;
        private string tableName;
        private int contentID;
        private string returnUrl;

        public static string GetOpenWindowString(int publishmentSystemID, ContentInfo contentInfo, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", contentInfo.NodeID.ToString());
            arguments.Add("ContentID", contentInfo.ID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowString("审核状态", "modal_checkState.aspx", arguments, 560, 500);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID", "ContentID", "ReturnUrl");

            this.nodeID = base.GetIntQueryString("NodeID");
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeID);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
            this.contentID = base.GetIntQueryString("ContentID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));

            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, this.contentID);

            int checkedLevel = 0;
            bool isChecked = CheckManager.GetUserCheckLevel(base.PublishmentSystemInfo, base.PublishmentSystemID, out checkedLevel);
            this.phCheck.Visible = LevelManager.IsCheckable(base.PublishmentSystemInfo, this.nodeID, contentInfo.IsChecked, contentInfo.CheckedLevel, isChecked, checkedLevel);

            this.ltlTitle.Text = contentInfo.Title;
            this.ltlState.Text = LevelManager.GetCheckState(base.PublishmentSystemInfo, contentInfo.IsChecked, contentInfo.CheckedLevel);

            ArrayList checkInfoArrayList = BaiRongDataProvider.ContentCheckDAO.GetCheckInfoArrayList(this.tableName, this.contentID);
            if (checkInfoArrayList.Count > 0)
            {
                this.phCheckReasons.Visible = true;
                this.rpContents.DataSource = checkInfoArrayList;
                this.rpContents.ItemDataBound += new RepeaterItemEventHandler(rpContents_ItemDataBound);
                this.rpContents.DataBind();
            }

            //显示定时任务信息
            int checkTaskId = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(ContentAttribute.Check_TaskID));
            int unCheckTaskId = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(ContentAttribute.UnCheck_TaskID));

            if (checkTaskId > 0)
            {
                TaskInfo checkTaskInfo = BaiRongDataProvider.TaskDAO.GetTaskInfo(checkTaskId);
                if (checkTaskInfo != null)
                {
                    this.phCheckTask.Visible = true;
                    this.ltlCheckTaskName.Text = "定时审核";
                    this.ltlCheckTaskDate.Text = checkTaskInfo.OnlyOnceDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            else if (contentInfo.CheckTaskDate != DateUtils.SqlMinValue)
            {
                this.phCheckTask.Visible = true;
                this.ltlCheckTaskName.Text = "定时审核";
                this.ltlCheckTaskDate.Text = contentInfo.CheckTaskDate.ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (unCheckTaskId > 0)
            {
                TaskInfo unCheckTaskInfo = BaiRongDataProvider.TaskDAO.GetTaskInfo(unCheckTaskId);
                if (unCheckTaskInfo != null)
                {
                    this.phUnCheckTask.Visible = true;
                    this.ltlUnCheckTaskName.Text = "定时下架";
                    this.ltlUnCheckTaskDate.Text = unCheckTaskInfo.OnlyOnceDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            else if (contentInfo.UnCheckTaskDate != DateUtils.SqlMinValue)
            {
                this.phUnCheckTask.Visible = true;
                this.ltlUnCheckTaskName.Text = "定时下架";
                this.ltlUnCheckTaskDate.Text = contentInfo.UnCheckTaskDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        void rpContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ContentCheckInfo checkInfo = e.Item.DataItem as ContentCheckInfo;

            Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
            Literal ltlCheckDate = e.Item.FindControl("ltlCheckDate") as Literal;
            Literal ltlReasons = e.Item.FindControl("ltlReasons") as Literal;

            ltlUserName.Text = AdminManager.GetDisplayName(checkInfo.UserName, true);
            ltlCheckDate.Text = DateUtils.GetDateAndTimeString(checkInfo.CheckDate);
            ltlReasons.Text = checkInfo.Reasons;
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            string redirectUrl = ContentCheck.GetLinkUrl(base.PublishmentSystemID, this.nodeID, this.contentID, this.returnUrl);
            PageUtils.Redirect(redirectUrl);
        }

    }
}
