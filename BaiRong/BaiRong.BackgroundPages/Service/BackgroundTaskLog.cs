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
                base.BreadCrumbForService(AppManager.Platform.LeftMenu.ID_Service, "����������־", AppManager.Platform.Permission.Platform_Service);

                ETriStateUtils.AddListItems(this.ddlIsSuccess, "ȫ��", "�ɹ�", "ʧ��");

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
                        base.SuccessMessage(string.Format("�ɹ�{0}��־��¼", (ConfigManager.Additional.IsLogTask ? "����" : "����")));
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("{0}��־��¼ʧ��", (ConfigManager.Additional.IsLogTask ? "����" : "����")));
                    }
                }

                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetPlatformUrl("background_taskLog.aspx?Delete=True"), "IDCollection", "IDCollection", "��ѡ����Ҫɾ������־��", "�˲�����ɾ����ѡ��־��ȷ����"));
                this.btnDeleteAll.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetPlatformUrl("background_taskLog.aspx?DeleteAll=True"), "�˲�����ɾ��������־��Ϣ��ȷ����"));

                if (ConfigManager.Additional.IsLogTask)
                {
                    this.btnSetting.Text = "���ü�¼��־����";
                    this.btnSetting.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetPlatformUrl("background_taskLog.aspx?Setting=True"), "�˲�������������������־��¼���ܣ�ȷ����"));
                }
                else
                {
                    this.ltlState.Text = " (����������־��ǰ���ڽ���״̬���������¼��ز�����)";
                    this.btnSetting.Text = "���ü�¼��־����";
                    this.btnSetting.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetPlatformUrl("background_taskLog.aspx?Setting=True"), "�˲�������������������־��¼���ܣ�ȷ����"));
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
