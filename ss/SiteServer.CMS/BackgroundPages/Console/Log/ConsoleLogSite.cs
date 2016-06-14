using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Controls;


namespace SiteServer.CMS.BackgroundPages
{
	public class ConsoleLogSite : BackgroundBasePage
	{
        public DropDownList PublishmentSystem;
        public DropDownList LogType;
        public TextBox UserName;
        public TextBox Keyword;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;

        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlPublishmentSystem;

		public Button Delete;
		public Button DeleteAll;

        protected override bool IsSaasForbidden { get { return true; } }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = Constants.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            if (base.GetQueryString("UserName") == null)
            {
                this.spContents.SelectCommand = DataProvider.LogDAO.GetSelectCommend();
            }
            else
            {
                this.spContents.SelectCommand = DataProvider.LogDAO.GetSelectCommend(this.PublishmentSystemID, base.GetQueryString("LogType"), base.GetQueryString("UserName"), base.GetQueryString("Keyword"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"));
            }

            this.spContents.SortField = "ID";
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if(!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Log, "应用管理日志", AppManager.Platform.Permission.Platform_Log);

                if (this.PublishmentSystemID == 0)
                {
                    this.ltlPublishmentSystem.Text = @"<td align=""center"" width=""160"">&nbsp;应用名称</td>";
                }

                this.PublishmentSystem.Items.Add(new ListItem("<<全部应用>>", "0"));

                ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayListOrderByLevel();
                foreach (int psID in publishmentSystemIDArrayList)
                {
                    this.PublishmentSystem.Items.Add(new ListItem(PublishmentSystemManager.GetPublishmentSystemInfo(psID).PublishmentSystemName, psID.ToString())); 
                }

                this.LogType.Items.Add(new ListItem("全部记录", "All"));
                this.LogType.Items.Add(new ListItem("栏目相关记录", "Channel"));
                this.LogType.Items.Add(new ListItem("内容相关记录", "Content"));

                if (base.GetQueryString("UserName") != null)
                {
                    ControlUtils.SelectListItems(this.PublishmentSystem, this.PublishmentSystemID.ToString());
                    ControlUtils.SelectListItems(this.LogType, base.GetQueryString("LogType"));
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
                        DataProvider.LogDAO.Delete(arraylist);
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
                        DataProvider.LogDAO.DeleteAll();
                        base.SuccessDeleteMessage();
                    }
                    catch (Exception ex)
                    {
                        base.FailDeleteMessage(ex);
                    }
                }

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl("console_logSite.aspx?Delete=True"), "IDCollection", "IDCollection", "请选择需要删除的日志！", "此操作将删除所选日志，确认吗？"));
                this.DeleteAll.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetCMSUrl("console_logSite.aspx?DeleteAll=True"), "此操作将删除所有日志信息，确定吗？"));

                this.spContents.DataBind();
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlPublishmentSystem = (Literal)e.Item.FindControl("ltlPublishmentSystem");
                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlIPAddress = (Literal)e.Item.FindControl("ltlIPAddress");
                Literal ltlAction = (Literal)e.Item.FindControl("ltlAction");
                Literal ltlSummary = (Literal)e.Item.FindControl("ltlSummary");

                if (this.PublishmentSystemID == 0)
                {
                    SiteServer.CMS.Model.PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(TranslateUtils.EvalInt(e.Item.DataItem, "PublishmentSystemID"));
                    string publishmentSystemName = string.Empty;
                    if (publishmentSystemInfo != null)
                    {
                        publishmentSystemName = string.Format("<a href='{0}' target='_blank'>{1}</a>", publishmentSystemInfo.PublishmentSystemUrl, publishmentSystemInfo.PublishmentSystemName);
                    }
                    ltlPublishmentSystem.Text = string.Format(@"<td align=""center"" width=""160"">{0}</td>", publishmentSystemName);
                }
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
                return PageUtils.GetCMSUrl(string.Format("console_logSite.aspx?UserName={0}&Keyword={1}&DateFrom={2}&DateTo={3}&PublishmentSystemID={4}&LogType={5}", this.UserName.Text, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text, this.PublishmentSystem.SelectedValue, this.LogType.SelectedValue));
            }
        }
	}
}
