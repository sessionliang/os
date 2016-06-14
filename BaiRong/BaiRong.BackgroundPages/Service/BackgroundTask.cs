using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;


using BaiRong.Core.Data.Provider;
using BaiRong.Model.Service;

namespace BaiRong.BackgroundPages
{
	public class BackgroundTask : BackgroundBasePage
	{
        public DataGrid dgContents;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                int taskID = TranslateUtils.ToInt(base.GetQueryString("TaskID"));
                try
                {
                    BaiRongDataProvider.TaskDAO.Delete(taskID);
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            if (base.GetQueryString("Enabled") != null)
            {
                int taskID = base.GetIntQueryString("TaskID");
                bool isEnabled = base.GetBoolQueryString("IsEnabled");
                string func = isEnabled ? "启用" : "禁用";

                try
                {
                    BaiRongDataProvider.TaskDAO.UpdateState(taskID, isEnabled);
                    base.SuccessMessage(string.Format("{0}定时任务成功。", func));
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("{0}定时任务失败。", func));
                }
            }

			if (!IsPostBack)
			{
                base.BreadCrumbForService(AppManager.Platform.LeftMenu.ID_Service, "定时任务管理", AppManager.Platform.Permission.Platform_Service);

                this.dgContents.DataSource = BaiRongDataProvider.TaskDAO.GetTaskInfoArrayList();
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
			}
		}

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TaskInfo taskInfo = (TaskInfo)e.Item.DataItem;

                Literal ltlProduct = e.Item.FindControl("ltlProduct") as Literal;
                Literal ltlPublishmentSystem = e.Item.FindControl("ltlPublishmentSystem") as Literal;
                Literal ltlTaskName = e.Item.FindControl("ltlTaskName") as Literal;
                Literal ltlServiceType = e.Item.FindControl("ltlServiceType") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlFrequencyType = e.Item.FindControl("ltlFrequencyType") as Literal;
                Literal ltlLastExecuteDate = e.Item.FindControl("ltlLastExecuteDate") as Literal;
                Literal ltlEnabledHtml = e.Item.FindControl("ltlEnabledHtml") as Literal;
                Literal ltlDeleteHtml = e.Item.FindControl("ltlDeleteHtml") as Literal;

                ltlProduct.Text = AppManager.GetAppName(taskInfo.ProductID, false);

                ltlPublishmentSystem.Text = BaiRongDataProvider.TaskDAO.GetPublishmentSystemName(taskInfo.PublishmentSystemID);

                ltlTaskName.Text = taskInfo.TaskName;
                ltlServiceType.Text = EServiceTypeUtils.GetText(taskInfo.ServiceType);
                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(taskInfo.IsEnabled.ToString());
                ltlFrequencyType.Text = EFrequencyTypeUtils.GetText(taskInfo.FrequencyType);
                if (taskInfo.LastExecuteDate > DateUtils.SqlMinValue)
                {
                    ltlLastExecuteDate.Text = DateUtils.GetDateAndTimeString(taskInfo.LastExecuteDate);
                }
                string urlTask = PageUtils.GetPlatformUrl(string.Format("background_task.aspx?Enabled=True&TaskID={0}&IsEnabled={1}", taskInfo.TaskID, !taskInfo.IsEnabled));
                ltlEnabledHtml.Text = string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('此操作将{1}任务“{2}”，确认吗？');\">{1}</a>", urlTask, taskInfo.IsEnabled ? "禁用" : "启用", taskInfo.TaskName);
                if (!taskInfo.IsSystemTask)
                {
                    string urlDelete = PageUtils.GetPlatformUrl(string.Format("background_task.aspx?Delete=True&TaskID={0}", taskInfo.TaskID));
                    ltlDeleteHtml.Text = string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('此操作将删除任务“{1}”，确认吗？');\">删除</a>", urlDelete, taskInfo.TaskName);
                }
            }
        }
	}
}
