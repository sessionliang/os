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
    public class RequestAnswer : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        public Literal ltlStatus;
        public Literal ltlWebsite;

        public Repeater rptContents;
        public SqlPager spContents;

        public PlaceHolder phAnswer;
        public BREditor Content;

        private string domain = string.Empty;
        private int licenseID = 0;
        private RequestInfo requestInfo;
        private string returnUrl;

        protected override bool IsAccessable
        {
            get
            {
                return true;
            }
        }

        public static string GetRedirectUrl(string domain, int licenseID, int id, string returnUrl)
        {
            return string.Format("requestAnswer.aspx?domain={0}&licenseID={1}&ID={2}&ReturnUrl={3}", domain, licenseID, id, StringUtils.ValueToUrl(returnUrl));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.domain = base.GetQueryString("domain");
            this.licenseID = TranslateUtils.ToInt(base.GetQueryString("licenseID"));
            bool isValid = !string.IsNullOrEmpty(this.domain) && this.licenseID > 0;
            if (isValid)
            {
                isValid = this.licenseID == DataProvider.DBLicenseDAO.GetLicenseID(domain);
            }

            if (isValid)
            {
                int id = TranslateUtils.ToInt(base.GetQueryString("ID"));
                this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
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
                    if (!string.IsNullOrEmpty(this.requestInfo.Website))
                    {
                        this.ltlWebsite.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", PageUtils.AddProtocolToUrl(this.requestInfo.Website));
                    }

                    if (this.requestInfo.Status == ERequestStatus.Closed)
                    {
                        this.phAnswer.Visible = false;
                    }

                    this.spContents.DataBind();
                }
            }
            else
            {
                Page.Response.Write(string.Empty);
                Page.Response.End();
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
                answerInfoToAdd.IsAnswer = false;



                DataProvider.RequestAnswerDAO.Insert(answerInfoToAdd);

                base.SuccessMessage("工单提交成功");
                
                base.AddWaitAndRedirectScript(RequestAnswer.GetRedirectUrl(this.domain, this.licenseID, this.requestInfo.ID, this.returnUrl));
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
