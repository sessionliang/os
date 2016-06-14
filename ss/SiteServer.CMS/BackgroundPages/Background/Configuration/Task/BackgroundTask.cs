using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;


using BaiRong.Model.Service;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundTask : BackgroundBasePage
	{
        public DataGrid dgContents;
        public Button AddTask;

        private EServiceType serviceType;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("ServiceType");

            this.serviceType = EServiceTypeUtils.GetEnumType(base.GetQueryString("ServiceType"));

            if (base.GetQueryString("Delete") != null)
            {
                int taskID = TranslateUtils.ToInt(base.GetQueryString("TaskID"));
                try
                {
                    BaiRongDataProvider.TaskDAO.Delete(taskID);
                    //StringUtility.AddLog(base.PublishmentSystemID, "删除定时任务", string.Format("任务名称:{0}", taskName));
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            if (base.GetQueryString("Enabled") != null)
            {
                int taskID = TranslateUtils.ToInt(base.GetQueryString("TaskID"));
                bool isEnabled = TranslateUtils.ToBool(base.GetQueryString("IsEnabled"));
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
                string taskName = EServiceTypeUtils.GetText(this.serviceType);

                if (base.PublishmentSystemID > 0)
                {
                    base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, AppManager.CMS.LeftMenu.Configuration.ID_ConfigurationTask, taskName + "任务", AppManager.CMS.Permission.WebSite.Configration);
                }
                else
                {
                    base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_SiteSettings, taskName + "任务", AppManager.Platform.Permission.Platform_SiteSettings);
                }

                this.AddTask.Text = string.Format("添加{0}任务", taskName);
                this.AddTask.Attributes.Add("onclick", Modal.TaskAdd.GetOpenWindowStringToAdd(this.serviceType, base.PublishmentSystemID));
                BindGrid();
			}
		}

        public void BindGrid()
        {
            try
            {
                if (base.PublishmentSystemID != 0)
                {
                    this.dgContents.DataSource = BaiRongDataProvider.TaskDAO.GetTaskInfoArrayList(AppManager.CMS.AppID, this.serviceType, base.PublishmentSystemID);
                    this.dgContents.Columns.RemoveAt(0);
                }
                else
                {
                    this.dgContents.DataSource = BaiRongDataProvider.TaskDAO.GetTaskInfoArrayList(AppManager.CMS.AppID, this.serviceType);
                }
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TaskInfo taskInfo = (TaskInfo)e.Item.DataItem;

                Literal ltlSite = e.Item.FindControl("ltlSite") as Literal;
                Literal ltlTaskName = e.Item.FindControl("ltlTaskName") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlFrequencyType = e.Item.FindControl("ltlFrequencyType") as Literal;
                Literal ltlLastExecuteDate = e.Item.FindControl("ltlLastExecuteDate") as Literal;
                Literal ltlEditHtml = e.Item.FindControl("ltlEditHtml") as Literal;
                Literal ltlEnabledHtml = e.Item.FindControl("ltlEnabledHtml") as Literal;
                Literal ltlDeleteHtml = e.Item.FindControl("ltlDeleteHtml") as Literal;

                if (ltlSite != null)
                {
                    ltlSite.Text = this.GetSiteHtml(taskInfo.PublishmentSystemID);
                }
                ltlTaskName.Text = taskInfo.TaskName;
                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(taskInfo.IsEnabled.ToString());
                ltlFrequencyType.Text = EFrequencyTypeUtils.GetText(taskInfo.FrequencyType);
                if (taskInfo.LastExecuteDate > DateUtils.SqlMinValue)
                {
                    ltlLastExecuteDate.Text = DateUtils.GetDateAndTimeString(taskInfo.LastExecuteDate);
                }
                if (!taskInfo.IsSystemTask)
                {
                    ltlEditHtml.Text = this.GetEditHtml(taskInfo.TaskID, taskInfo.PublishmentSystemID);
                    ltlDeleteHtml.Text = this.GetDeleteHtml(taskInfo.TaskID, taskInfo.TaskName, taskInfo.PublishmentSystemID);
                }
                string urlTask = PageUtils.GetCMSUrl(string.Format("background_task.aspx?Enabled=True&TaskID={0}&IsEnabled={1}&ServiceType={2}&PublishmentSystemID={3}", taskInfo.TaskID, !taskInfo.IsEnabled, EServiceTypeUtils.GetValue(taskInfo.ServiceType), taskInfo.PublishmentSystemID));
                ltlEnabledHtml.Text = string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('此操作将{1}任务“{2}”，确认吗？');\">{1}</a>", urlTask, taskInfo.IsEnabled ? "禁用" : "启用", taskInfo.TaskName);
                
            }
        }

        private string GetEditHtml(int taskID, int publishmentSystemID)
        {
            if (base.PublishmentSystemID != 0 && publishmentSystemID != base.PublishmentSystemID) return string.Empty;
            return string.Format("<a href=\"javascript:;\" onClick=\"{0}\">修改</a>", Modal.TaskAdd.GetOpenWindowStringToEdit(taskID, this.serviceType, base.PublishmentSystemID));
        }

        private string GetSiteHtml(int publishmentSystemID)
        {
            if (publishmentSystemID == 0) return "全部应用";
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (publishmentSystemInfo != null)
            {
                return string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", publishmentSystemInfo.PublishmentSystemUrl, publishmentSystemInfo.PublishmentSystemName);
            }
            return string.Empty;
        }

        private string GetDeleteHtml(int taskID, string taskName, int publishmentSystemID)
        {
            if (base.PublishmentSystemID != 0 && publishmentSystemID != base.PublishmentSystemID) return string.Empty;

            string urlDelete = PageUtils.GetCMSUrl(string.Format("background_task.aspx?Delete=True&TaskID={0}&ServiceType={1}&PublishmentSystemID={2}", taskID, EServiceTypeUtils.GetValue(this.serviceType), base.PublishmentSystemID));
            return string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('此操作将删除{1}任务“{2}”，确认吗？');\">删除</a>", urlDelete, EServiceTypeUtils.GetText(this.serviceType), taskName);
        }
	}
}
