using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Controls;


namespace BaiRong.BackgroundPages
{
	public class BackgroundLog : BackgroundBasePage
	{
        public Literal ltlState;
        public TextBox UserName;
        public TextBox Keyword;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;

        public Repeater rptContents;
        public SqlPager spContents;

		public Button Delete;
		public Button DeleteAll;
        public Button Setting;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = StringUtils.Constants.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            if (base.GetQueryString("UserName") == null)
            {
                this.spContents.SelectCommand = BaiRongDataProvider.LogDAO.GetSelectCommend();
            }
            else
            {
                this.spContents.SelectCommand = BaiRongDataProvider.LogDAO.GetSelectCommend(base.GetQueryString("UserName"), base.GetQueryString("Keyword"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"));
            }

            this.spContents.SortField = "ID";
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if(!IsPostBack)
			{
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Log, "����Ա��־", AppManager.Platform.Permission.Platform_Log);

                if (base.GetQueryString("UserName") != null)
                {
                    this.UserName.Text = base.GetQueryString("UserName");
                    this.Keyword.Text = base.GetQueryString("Keyword");
                    this.DateFrom.Text = base.GetQueryString("DateFrom");
                    this.DateTo.Text = base.GetQueryString("DateTo");
                }

                if (base.GetQueryString("Delete") != null)
                {
                    ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("IDCollection"));
                    try
                    {
                        BaiRongDataProvider.LogDAO.Delete(arraylist);
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
                        BaiRongDataProvider.LogDAO.DeleteAll();
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
                        ConfigManager.Additional.IsLog = !ConfigManager.Additional.IsLog;
                        BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);
                        base.SuccessMessage(string.Format("�ɹ�{0}��־��¼", (ConfigManager.Additional.IsLog ? "����" : "����")));
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("{0}��־��¼ʧ��", (ConfigManager.Additional.IsLog ? "����" : "����")));
                    }
                }

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetPlatformUrl("background_log.aspx?Delete=True"), "IDCollection", "IDCollection", "��ѡ����Ҫɾ������־��", "�˲�����ɾ����ѡ��־��ȷ����"));
                this.DeleteAll.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetPlatformUrl("background_log.aspx?DeleteAll=True"), "�˲�����ɾ��������־��Ϣ��ȷ����"));

                if (ConfigManager.Additional.IsLog)
                {
                    this.Setting.Text = "���ü�¼��־����";
                    this.Setting.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetPlatformUrl("background_log.aspx?Setting=True"), "�˲��������ù���Ա��־��¼���ܣ�ȷ����"));
                }
                else
                {
                    this.ltlState.Text = " (����Ա��־��ǰ���ڽ���״̬���������¼��ز�����)";
                    this.Setting.Text = "���ü�¼��־����";
                    this.Setting.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetPlatformUrl("background_log.aspx?Setting=True"), "�˲��������ù���Ա��־��¼���ܣ�ȷ����"));
                }

                this.spContents.DataBind();
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlIPAddress = (Literal)e.Item.FindControl("ltlIPAddress");
                Literal ltlAction = (Literal)e.Item.FindControl("ltlAction");
                Literal ltlSummary = (Literal)e.Item.FindControl("ltlSummary");

                ltlUserName.Text = TranslateUtils.EvalString(e.Item.DataItem, "UserName");
                ltlAddDate.Text = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate").ToString();
                ltlIPAddress.Text = TranslateUtils.EvalString(e.Item.DataItem, "IPAddress");
                ltlAction.Text = TranslateUtils.EvalString(e.Item.DataItem, "Action");
                ltlSummary.Text = TranslateUtils.EvalString(e.Item.DataItem, "Summary");
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
                return PageUtils.GetPlatformUrl(string.Format("background_log.aspx?UserName={0}&Keyword={1}&DateFrom={2}&DateTo={3}", this.UserName.Text, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text));
            }
        }
	}
}
