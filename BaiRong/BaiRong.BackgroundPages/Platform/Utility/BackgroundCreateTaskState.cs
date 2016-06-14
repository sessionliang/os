using System;
using System.Collections;
using System.Text;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Model;


namespace BaiRong.BackgroundPages
{
    public class BackgroundCreateTaskState : BackgroundBasePage
    {
        public int CreateTaskID;

        protected Repeater rptContents;
        protected DropDownList ddlInteval;

        public static string GetRedirectString(int createTaskID)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_createTaskState.aspx?createTaskID={0}", createTaskID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.GetQueryString("Cancel")))
            {
                //取消任务
                int createTaskID = TranslateUtils.ToInt(base.GetQueryString("CreateTaskID"));
                CreateTaskInfo createTaskInfo = BaiRongDataProvider.CreateTaskDAO.GetCreateTaskInfo(createTaskID);
                if (createTaskInfo != null && createTaskInfo.ID > 0)
                {
                    //删除ajaxurl
                    BaiRongDataProvider.AjaxUrlDAO.DeleteByTaskID(createTaskID);
                    //更改task状态
                    BaiRongDataProvider.CreateTaskDAO.UpdateState(createTaskID, ECreateTaskType.Cancel);
                }
            }
            BaiRongDataProvider.CreateTaskDAO.UpdateState();
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Utility, "生成队列", AppManager.Platform.Permission.Platform_Utility);
                this.CreateTaskID = TranslateUtils.ToInt(base.GetQueryString("CreateTaskID"));
                this.ddlInteval.SelectedValue = base.GetQueryString("inteval");
            }
            this.rptContents.ItemDataBound += rptContents_ItemDataBound;
            if (!PermissionsManager.Current.IsConsoleAdministrator)
                this.rptContents.DataSource = BaiRongDataProvider.CreateTaskDAO.GetCreateTaskInfoArrayList(string.Format(" userName='{0}' ", AdminManager.Current.UserName));
            else
                this.rptContents.DataSource = BaiRongDataProvider.CreateTaskDAO.GetCreateTaskInfoArrayList(string.Format(""));
            this.rptContents.DataBind();
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int createTaskID = TranslateUtils.EvalInt(e.Item.DataItem, "ID");
                CreateTaskInfo createTaskInfo = BaiRongDataProvider.CreateTaskDAO.GetCreateTaskInfo(createTaskID);

                int queuingCount = 0;//排队数量
                int totalCount = 0;//生成总数
                int currentCount = 0;//已经处理数量
                int errorCount = 0;//错误数量
                double pecent = 0;
                BaiRongDataProvider.CreateTaskDAO.GetQueuingTaskCount(createTaskID, out totalCount, out currentCount, out errorCount, out queuingCount);
                if (createTaskInfo.State == ECreateTaskType.Completed)
                {
                    //已完成，那么不排队
                    queuingCount = 0;
                }
                else if (createTaskInfo.State == ECreateTaskType.Queuing || createTaskInfo.State == ECreateTaskType.Cancel)
                {
                    //排队中，没有执行数量
                    currentCount = 0;
                }

                if (totalCount == 0 && currentCount == 0)
                {
                    pecent = 100;
                }
                else
                {
                    pecent = (1.0 * currentCount / totalCount) * 100;
                }
                Literal ltlItemIndex = (Literal)e.Item.FindControl("ltlItemIndex");
                Literal ltlSummary = (Literal)e.Item.FindControl("ltlSummary");
                Literal ltlProcessCount = (Literal)e.Item.FindControl("ltlProcessCount");
                Literal ltlProcess = (Literal)e.Item.FindControl("ltlProcess");
                Literal ltlState = (Literal)e.Item.FindControl("ltlState");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlQueuing = (Literal)e.Item.FindControl("ltlQueuing");
                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlCancel = (Literal)e.Item.FindControl("ltlCancel");
                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlSummary.Text = createTaskInfo.Summary;
                ltlProcessCount.Text = currentCount + "/" + totalCount;
                ltlProcess.Text = string.Format(@"<div style=""width:230px;position: relative;"" class=""progress progress-success progress-striped"">
            <div class=""bar"" style=""width: {0}%;{2}""></div><div style=""color:darkblue;position: absolute;left: 0;top: 0;height: 100%;width: 100%;text-align: center;"">&nbsp;{1}</div>
          </div>", pecent, pecent.ToString("0.00") + "%", createTaskInfo.State == ECreateTaskType.Cancel ? "background-color:gray" : "");

                double totalSeconds = (createTaskInfo.EndTime - createTaskInfo.StartTime).TotalSeconds;
                ltlState.Text = ECreateTaskTypeUtils.GetText(createTaskInfo.State) + (ECreateTaskTypeUtils.Equals(createTaskInfo.State, ECreateTaskType.Completed) ? string.Format("(用时：{0}小时 {1}分钟 {2}秒)",
                    (createTaskInfo.EndTime - createTaskInfo.StartTime).Hours,
                    (createTaskInfo.EndTime - createTaskInfo.StartTime).Minutes,
                    (createTaskInfo.EndTime - createTaskInfo.StartTime).Seconds) : "");
                ltlAddDate.Text = createTaskInfo.AddDate.ToString();
                ltlQueuing.Text = queuingCount.ToString();
                ltlUserName.Text = createTaskInfo.UserName;
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("Cancel", "True");
                nvc.Add("CreateTaskID", createTaskID.ToString());
                bool canCancel = false;
                if (createTaskInfo.State == ECreateTaskType.Processing || createTaskInfo.State == ECreateTaskType.Queuing)
                    canCancel = true;
                if (canCancel)
                    ltlCancel.Text = string.Format("<a href='{0}'>取消</a>", PageUtils.AddQueryString(this.PageUrl, nvc));
                else
                    ltlCancel.Text = string.Empty;
            }
        }

        protected void btnFresh_Click(object sender, EventArgs e)
        {
            JsUtils.GetRedirectString(this.PageUrl);
        }

        protected void ddlInteval_SelectedIndexChanged(object sender, EventArgs e)
        {
            JsUtils.GetRedirectString(this.PageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetPlatformUrl(string.Format("background_createTaskState.aspx?inteval={0}", this.ddlInteval.SelectedValue));
                }
                return _pageUrl;
            }
        }
    }
}
