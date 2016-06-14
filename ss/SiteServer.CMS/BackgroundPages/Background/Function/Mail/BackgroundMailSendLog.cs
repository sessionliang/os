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
	public class BackgroundMailSendLog : BackgroundBasePage
	{
        public TextBox Keyword;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;

        public Repeater rptContents;
        public SqlPager spContents;

		public Button Delete;
		public Button DeleteAll;
        public Literal ltlState;
        public Button Setting;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = Constants.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            if (base.GetQueryString("Keyword") == null)
            {
                this.spContents.SelectCommand = DataProvider.MailSendLogDAO.GetSelectCommend();
            }
            else
            {
                this.spContents.SelectCommand = DataProvider.MailSendLogDAO.GetSelectCommend(this.PublishmentSystemID, base.GetQueryString("Keyword"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"));
            }

            this.spContents.SortField = "ID";
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if(!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Mail, "推荐好友记录查询", AppManager.CMS.Permission.WebSite.Mail);

                if (base.GetQueryString("Keyword") != null)
                {
                    this.Keyword.Text = base.GetQueryString("Keyword");
                    this.DateFrom.Text = base.GetQueryString("DateFrom");
                    this.DateTo.Text = base.GetQueryString("DateTo");
                }

                if (base.GetQueryString("Delete") != null)
                {
                    ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("IDCollection"));
                    try
                    {
                        DataProvider.MailSendLogDAO.Delete(arraylist);
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
                        DataProvider.MailSendLogDAO.DeleteAll();
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
                        base.PublishmentSystemInfo.Additional.IsLogMailSend = !base.PublishmentSystemInfo.Additional.IsLogMailSend;
                        DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                        base.SuccessMessage(string.Format("成功{0}推荐好友日志记录", (base.PublishmentSystemInfo.Additional.IsLogMailSend ? "启用" : "禁用")));
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("{0}推荐好友日志记录失败", (base.PublishmentSystemInfo.Additional.IsLogMailSend ? "启用" : "禁用")));
                    }
                }

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_mailSendLog.aspx?PublishmentSystemID={0}&Delete=True", base.PublishmentSystemID)), "IDCollection", "IDCollection", "请选择需要删除的日志！", "此操作将删除所选日志，确认吗？"));
                this.DeleteAll.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetCMSUrl(string.Format("background_mailSendLog.aspx?PublishmentSystemID={0}&DeleteAll=True", base.PublishmentSystemID)), "此操作将删除所有日志信息，确定吗？"));

                if (base.PublishmentSystemInfo.Additional.IsLogMailSend)
                {
                    this.Setting.Text = "禁用记录日志功能";
                    this.Setting.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetCMSUrl(string.Format("background_mailSendLog.aspx?PublishmentSystemID={0}&Setting=True", base.PublishmentSystemID)), "此操作将禁用推荐好友日志记录功能，确定吗？"));
                }
                else
                {
                    this.ltlState.Text = " (推荐好友日志当前处于禁用状态，将不会记录相关操作！)";
                    this.Setting.Text = "启用记录日志功能";
                    this.Setting.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetCMSUrl(string.Format("background_mailSendLog.aspx?PublishmentSystemID={0}&Setting=True", base.PublishmentSystemID)), "此操作将启用推荐好友日志记录功能，确定吗？"));
                }

                this.spContents.DataBind();
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlTitle = (Literal)e.Item.FindControl("ltlTitle");
                Literal ltlMail = (Literal)e.Item.FindControl("ltlMail");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlIPAddress = (Literal)e.Item.FindControl("ltlIPAddress");

                ltlTitle.Text = string.Format("<a href='{0}' target='_blank'>{1}</a>", TranslateUtils.EvalString(e.Item.DataItem, "PageUrl"), TranslateUtils.EvalString(e.Item.DataItem, "Title"));
                ltlMail.Text = TranslateUtils.EvalString(e.Item.DataItem, "Mail");
                ltlAddDate.Text = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate").ToString();
                ltlIPAddress.Text = TranslateUtils.EvalString(e.Item.DataItem, "IPAddress");
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
                return PageUtils.GetCMSUrl(string.Format("background_mailSendLog.aspx?PublishmentSystemID={0}&Keyword={1}&DateFrom={2}&DateTo={3}", base.PublishmentSystemID, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text));
            }
        }
	}
}
