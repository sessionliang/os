using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.Project.Controls;
using System.Collections.Specialized;


using BaiRong.Core.Data.Provider;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundRequestAnswer : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        public Literal ltlStatus;
        public Literal ltlLicense;
        public Literal ltlDomain;
        public Literal ltlWebsite;
        public Literal ltlEmail;
        public Literal ltlAccount;
        public Literal ltlContact;

        public Repeater rptContents;
        public SqlPager spContents;

        public PlaceHolder phAnswer;
        public BREditor Content;

        private RequestInfo requestInfo;
        private string returnUrl;

        public static string GetRedirectUrl(int id, string returnUrl)
        {
            return string.Format("background_requestAnswer.aspx?ID={0}&ReturnUrl={1}", id, StringUtils.ValueToUrl(returnUrl));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int id = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundRequest.GetRedirectUrl(true, string.Empty, string.Empty, 1);
            }

            this.requestInfo = DataProvider.RequestDAO.GetRequestInfo(id);
            base.AddAttributes(this.requestInfo);

            this.ltlPageTitle.Text = this.requestInfo.Subject;

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.RequestAnswerDAO.GetSelectString(this.requestInfo.ID);
            this.spContents.SortField = DataProvider.RequestAnswerDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                this.ltlStatus.Text = ERequestStatusUtils.GetText(this.requestInfo.Status);

                if (this.requestInfo.LicenseID > 0)
                {
                    this.ltlLicense.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">查看产品许可</a>", Modal.LicenseView.GetShowPopWinString(this.requestInfo.LicenseID));

                    int urlID = DataProvider.UrlDAO.GetUrlID(this.requestInfo.Domain);
                    ltlDomain.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", Modal.UrlSummaryAdd.GetShowPopWinString(urlID, string.Format(@"background_requestAnswer.aspx?ID={0}", this.requestInfo.ID)), this.requestInfo.Domain);
                }                

                if (!string.IsNullOrEmpty(this.requestInfo.Website))
                {
                    this.ltlWebsite.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", PageUtils.AddProtocolToUrl(this.requestInfo.Website));
                }
                if (!string.IsNullOrEmpty(this.requestInfo.Email))
                {
                    this.ltlEmail.Text = string.Format(@"<a href=""mailto:{0}"">{0}</a>", this.requestInfo.Email);
                }
                if (this.requestInfo.AccountID > 0)
                {
                    ltlAccount.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">{1}</a>", Modal.AccountView.GetShowPopWinString(this.requestInfo.AccountID), DataProvider.AccountDAO.GetAccountName(this.requestInfo.AccountID));
                }
                if (this.requestInfo.ContactID > 0)
                {
                    this.ltlContact.Text = this.requestInfo.ContactID.ToString();
                }

                if (this.requestInfo.Status == ERequestStatus.Closed)
                {
                    this.phAnswer.Visible = false;
                }

                this.spContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RequestAnswerInfo answerInfo = new RequestAnswerInfo(e.Item.DataItem);

                Literal ltlContent = e.Item.FindControl("ltlContent") as Literal;

                string file = string.Empty;
                if (!string.IsNullOrEmpty(answerInfo.FileUrl))
                {
                    file = string.Format(@"<p>附件:<a href=""{0}"" target=""_blank"">{0}</a></p>", PageUtils.AddProtocolToUrl(answerInfo.FileUrl));
                }

                if (!answerInfo.IsAnswer)
                {
                    ltlContent.Text = string.Format(@"
                <div class=""q_content right"">{0}{1}<div class=""arrow_gray""></div></div>
                <span class=""content_tips right"">{2}</span>", answerInfo.Content, file, DateUtils.GetDateAndTimeString(answerInfo.AddDate));
                }
                else
                {
                    ltlContent.Text = string.Format(@"
                <div class=""q_content left"">{0}{1}<div class=""arrow_blue""></div></div>
                <span class=""content_tips left_float"">{2}</span>", answerInfo.Content, file, DateUtils.GetDateAndTimeString(answerInfo.AddDate));
                }
            }
        }

        public void Return_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.returnUrl);
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                RequestAnswerInfo answerInfoToAdd = DataProvider.RequestAnswerDAO.GetAnswerInfo(base.Request.Form);
                answerInfoToAdd.RequestID = this.requestInfo.ID;
                answerInfoToAdd.AddDate = DateTime.Now;
                answerInfoToAdd.IsAnswer = true;

                DataProvider.RequestAnswerDAO.Insert(answerInfoToAdd);

                base.SuccessMessage("工单回复成功");
                
                base.AddWaitAndRedirectScript(BackgroundRequestAnswer.GetRedirectUrl(this.requestInfo.ID, this.returnUrl));
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
