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
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Log, "管理员日志", AppManager.Platform.Permission.Platform_Log);

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
                        base.SuccessMessage(string.Format("成功{0}日志记录", (ConfigManager.Additional.IsLog ? "启用" : "禁用")));
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("{0}日志记录失败", (ConfigManager.Additional.IsLog ? "启用" : "禁用")));
                    }
                }

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetPlatformUrl("background_log.aspx?Delete=True"), "IDCollection", "IDCollection", "请选择需要删除的日志！", "此操作将删除所选日志，确认吗？"));
                this.DeleteAll.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetPlatformUrl("background_log.aspx?DeleteAll=True"), "此操作将删除所有日志信息，确定吗？"));

                if (ConfigManager.Additional.IsLog)
                {
                    this.Setting.Text = "禁用记录日志功能";
                    this.Setting.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetPlatformUrl("background_log.aspx?Setting=True"), "此操作将禁用管理员日志记录功能，确定吗？"));
                }
                else
                {
                    this.ltlState.Text = " (管理员日志当前处于禁用状态，将不会记录相关操作！)";
                    this.Setting.Text = "启用记录日志功能";
                    this.Setting.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetPlatformUrl("background_log.aspx?Setting=True"), "此操作将启用管理员日志记录功能，确定吗？"));
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
