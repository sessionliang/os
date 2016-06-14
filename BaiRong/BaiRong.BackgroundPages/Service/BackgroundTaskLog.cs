using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Controls;
using BaiRong.Model.Service;

namespace BaiRong.BackgroundPages
{
    public class BackgroundTaskLog : BackgroundBasePage
	{
        public Literal ltlState;
        public DropDownList ddlIsSuccess;
        public TextBox tbKeyword;
        public DateTimeTextBox tbDateFrom;
        public DateTimeTextBox tbDateTo;

        public Repeater rptContents;
        public SqlPager spContents;

		public Button btnDelete;
		public Button btnDeleteAll;
        public Button btnSetting;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = StringUtils.Constants.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            if (base.GetQueryString("Keyword") == null)
            {
                this.spContents.SelectCommand = BaiRongDataProvider.TaskLogDAO.GetSelectCommend();
            }
            else
            {
                this.spContents.SelectCommand = BaiRongDataProvider.TaskLogDAO.GetSelectCommend(ETriStateUtils.GetEnumType(base.GetQueryString("IsSuccess")), base.GetQueryString("Keyword"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"));
            }

            this.spContents.SortField = "ID";
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if(!IsPostBack)
            {
                base.BreadCrumbForService(AppManager.Platform.LeftMenu.ID_Service, "任务运行日志", AppManager.Platform.Permission.Platform_Service);

                ETriStateUtils.AddListItems(this.ddlIsSuccess, "全部", "成功", "失败");

                if (base.GetQueryString("Keyword") != null)
                {
                    ControlUtils.SelectListItems(this.ddlIsSuccess, base.GetQueryString("IsSuccess"));
                    this.tbKeyword.Text = base.GetQueryString("Keyword");
                    this.tbDateFrom.Text = base.GetQueryString("DateFrom");
                    this.tbDateTo.Text = base.GetQueryString("DateTo");
                }

                if (base.GetQueryString("Delete") != null)
                {
                    ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("IDCollection"));
                    try
                    {
                        BaiRongDataProvider.TaskLogDAO.Delete(arraylist);
                        base.SuccessDeleteMessage();
                    }
                    catch (Exception ex)
                    {
                        base.FailDeleteMessage(ex);
                    }
                }
                else if (base.GetQueryString("DeleteAll") != null)
                {
                    try
                    {
                        BaiRongDataProvider.TaskLogDAO.DeleteAll();
                        base.SuccessDeleteMessage();
                    }
                    catch (Exception ex)
                    {
                        base.FailDeleteMessage(ex);
                    }
                }
                else if (base.GetQueryString("Setting") != null)
                {
                    try
                    {
                        ConfigManager.Additional.IsLogTask = !ConfigManager.Additional.IsLogTask;
                        BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);
                        base.SuccessMessage(string.Format("成功{0}日志记录", (ConfigManager.Additional.IsLogTask ? "启用" : "禁用")));
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("{0}日志记录失败", (ConfigManager.Additional.IsLogTask ? "启用" : "禁用")));
                    }
                }

                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetPlatformUrl("background_taskLog.aspx?Delete=True"), "IDCollection", "IDCollection", "请选择需要删除的日志！", "此操作将删除所选日志，确认吗？"));
                this.btnDeleteAll.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetPlatformUrl("background_taskLog.aspx?DeleteAll=True"), "此操作将删除所有日志信息，确定吗？"));

                if (ConfigManager.Additional.IsLogTask)
                {
                    this.btnSetting.Text = "禁用记录日志功能";
                    this.btnSetting.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetPlatformUrl("background_taskLog.aspx?Setting=True"), "此操作将禁用任务运行日志记录功能，确定吗？"));
                }
                else
                {
                    this.ltlState.Text = " (任务运行日志当前处于禁用状态，将不会记录相关操作！)";
                    this.btnSetting.Text = "启用记录日志功能";
                    this.btnSetting.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetPlatformUrl("background_taskLog.aspx?Setting=True"), "此操作将启用任务运行日志记录功能，确定吗？"));
                }

                this.spContents.DataBind();
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int taskID = TranslateUtils.EvalInt(e.Item.DataItem, "TaskID");
                TaskInfo taskInfo = BaiRongDataProvider.TaskDAO.GetTaskInfo(taskID);
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate");
                string isSuccess = TranslateUtils.EvalString(e.Item.DataItem, "IsSuccess");
                string errorMessage = TranslateUtils.EvalString(e.Item.DataItem, "ErrorMessage");

                Literal ltlProduct = (Literal)e.Item.FindControl("ltlProduct");
                Literal ltlPublishmentSystem = (Literal)e.Item.FindControl("ltlPublishmentSystem");
                Literal ltlTaskName = (Literal)e.Item.FindControl("ltlTaskName");
                Literal ltlServiceType = (Literal)e.Item.FindControl("ltlServiceType");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlIsSuccess = (Literal)e.Item.FindControl("ltlIsSuccess");
                Literal ltlErrorMessage = (Literal)e.Item.FindControl("ltlErrorMessage");

                ltlProduct.Text = AppManager.GetAppName(taskInfo.ProductID, false);
                ltlPublishmentSystem.Text = BaiRongDataProvider.TaskDAO.GetPublishmentSystemName(taskInfo.PublishmentSystemID);
                ltlTaskName.Text = taskInfo.TaskName;
                ltlServiceType.Text = EServiceTypeUtils.GetText(taskInfo.ServiceType);
                
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(addDate);
                ltlIsSuccess.Text = StringUtils.GetTrueOrFalseImageHtml(isSuccess);
                ltlErrorMessage.Text = errorMessage;
            }
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            base.Response.Redirect(this.PageUrl, true);
        }

        private string PageUrl
        {
            get
            {
                return PageUtils.GetPlatformUrl(string.Format("background_taskLog.aspx?Keyword={0}&DateFrom={1}&DateTo={2}&IsSuccess={3}", this.tbKeyword.Text, this.tbDateFrom.Text, this.tbDateTo.Text, this.ddlIsSuccess.SelectedValue));
            }
        }
	}
}
