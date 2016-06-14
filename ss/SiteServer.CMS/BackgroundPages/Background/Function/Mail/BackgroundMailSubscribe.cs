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
	public class BackgroundMailSubscribe : BackgroundBasePage
	{
        public TextBox Keyword;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;

        public Repeater rptContents;
        public SqlPager spContents;

        public Button AddButton;
        public Button Export;
		public Button Delete;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = Constants.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            if (base.GetQueryString("Keyword") == null)
            {
                this.spContents.SelectCommand = DataProvider.MailSubscribeDAO.GetSelectCommend();
            }
            else
            {
                this.spContents.SelectCommand = DataProvider.MailSubscribeDAO.GetSelectCommend(this.PublishmentSystemID, base.GetQueryString("Keyword"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"));
            }

            this.spContents.SortField = "ID";
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if(!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Mail, "邮件订阅列表", AppManager.CMS.Permission.WebSite.Mail);

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
                        DataProvider.MailSubscribeDAO.Delete(arraylist);
                        base.SuccessDeleteMessage();
                    }
                    catch (Exception ex)
                    {
                        base.FailDeleteMessage(ex);
                    }
                }

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_mailSubscribe.aspx?PublishmentSystemID={0}&Delete=True", base.PublishmentSystemID)), "IDCollection", "IDCollection", "请选择需要删除的订阅！", "此操作将删除所选订阅，确认吗？"));

                string showPopWinString = PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToMailSubscribe(base.PublishmentSystemID);
                this.Export.Attributes.Add("onclick", showPopWinString);

                showPopWinString = Modal.MailSubscribeAdd.GetOpenWindowString(base.PublishmentSystemID);
                this.AddButton.Attributes.Add("onclick", showPopWinString);

                this.spContents.DataBind();
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlReceiver = (Literal)e.Item.FindControl("ltlReceiver");
                Literal ltlMail = (Literal)e.Item.FindControl("ltlMail");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlIPAddress = (Literal)e.Item.FindControl("ltlIPAddress");

                ltlReceiver.Text = TranslateUtils.EvalString(e.Item.DataItem, "Receiver");
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
                return PageUtils.GetCMSUrl(string.Format("background_mailSubscribe.aspx?PublishmentSystemID={0}&Keyword={1}&DateFrom={2}&DateTo={3}", base.PublishmentSystemID, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text));
            }
        }
	}
}
