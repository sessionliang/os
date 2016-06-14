using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Controls;


namespace BaiRong.BackgroundPages
{
	public class BackgroundErrorLog : BackgroundBasePage
	{
        public TextBox Keyword;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;

        public Repeater rptContents;
        public SqlPager spContents;

		public Button Delete;
		public Button DeleteAll;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("IDCollection"));
                try
                {
                    BaiRongDataProvider.ErrorLogDAO.Delete(arraylist);
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
                    BaiRongDataProvider.ErrorLogDAO.DeleteAll();
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = StringUtils.Constants.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            if (base.GetQueryString("UserName") == null)
            {
                this.spContents.SelectCommand = BaiRongDataProvider.ErrorLogDAO.GetSelectCommend();
            }
            else
            {
                this.spContents.SelectCommand = BaiRongDataProvider.ErrorLogDAO.GetSelectCommend(base.GetQueryString("Keyword"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"));
            }

            this.spContents.SortField = "ID";
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if(!IsPostBack)
			{
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Log, "系统错误日志", AppManager.Platform.Permission.Platform_Log);

                if (base.GetQueryString("UserName") != null)
                {
                    this.Keyword.Text = base.GetQueryString("Keyword");
                    this.DateFrom.Text = base.GetQueryString("DateFrom");
                    this.DateTo.Text = base.GetQueryString("DateTo");
                }

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetPlatformUrl("background_errorLog.aspx?Delete=True"), "IDCollection", "IDCollection", "请选择需要删除的日志！", "此操作将删除所选日志，确认吗？"));
                this.DeleteAll.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetPlatformUrl("background_errorLog.aspx?DeleteAll=True"), "此操作将删除所有日志信息，确定吗？"));

                this.spContents.DataBind();
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlMessage = (Literal)e.Item.FindControl("ltlMessage");
                Literal ltlStacktrace = (Literal)e.Item.FindControl("ltlStacktrace");
                Literal ltlSummary = (Literal)e.Item.FindControl("ltlSummary");

                ltlAddDate.Text = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate").ToString();
                ltlMessage.Text = TranslateUtils.EvalString(e.Item.DataItem, "Message");
                ltlStacktrace.Text = TranslateUtils.EvalString(e.Item.DataItem, "Stacktrace");
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
                return PageUtils.GetPlatformUrl(string.Format("background_errorLog.aspx?Keyword={0}&DateFrom={1}&DateTo={2}", this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text));
            }
        }
	}
}
