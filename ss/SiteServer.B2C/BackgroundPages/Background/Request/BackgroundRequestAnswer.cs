using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundRequestAnswer : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        public Literal ltlStatus;
        public Literal ltlWebsite;
        public Literal ltlEmail;

        public Repeater rptContents;
        public SqlPager spContents;

        public PlaceHolder phAnswer;
        public BREditor Content;

        private RequestInfo requestInfo;
        private string returnUrl;

        public static string GetRedirectUrl(int publishmentSystemID, int id, string returnUrl)
        {
            return PageUtils.GetB2CUrl(string.Format("background_requestAnswer.aspx?publishmentSystemID={0}&id={1}&returnUrl={2}", publishmentSystemID, id, StringUtils.ValueToUrl(returnUrl)));
        }

        public string GetValue(string attributeName)
        {
            object val = requestInfo != null ? requestInfo.GetValue(attributeName) : null;
            return val == null ? string.Empty : val.ToString();
        }
        public void Page_Load(object sender, EventArgs E)
        {
            int id = base.GetIntQueryString("ID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundRequest.GetRedirectUrl(base.PublishmentSystemID, true, string.Empty, string.Empty, 1);
            }

            this.requestInfo = DataProviderB2C.RequestDAO.GetRequestInfo(id);
            //base.AddAttributes(this.requestInfo);

            this.ltlPageTitle.Text = this.requestInfo.Subject;

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderB2C.RequestAnswerDAO.GetSelectString(this.requestInfo.ID);
            this.spContents.SortField = DataProviderB2C.RequestAnswerDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                this.ltlStatus.Text = ERequestStatusUtils.GetText(this.requestInfo.Status);                

                if (!string.IsNullOrEmpty(this.requestInfo.Website))
                {
                    this.ltlWebsite.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", PageUtils.AddProtocolToUrl(this.requestInfo.Website));
                }
                if (!string.IsNullOrEmpty(this.requestInfo.Email))
                {
                    this.ltlEmail.Text = string.Format(@"<a href=""mailto:{0}"">{0}</a>", this.requestInfo.Email);
                }

                if (ERequestStatusUtils.Equals(this.requestInfo.Status, ERequestStatus.Closed))
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
<span class=""content_tips right"">{0}</span>
<div class=""q_content right""><p>{1}</p>{2}<div class=""arrow_gray""></div></div>
", DateUtils.GetDateAndTimeString(answerInfo.AddDate), answerInfo.Content, file);
                }
                else
                {
                    ltlContent.Text = string.Format(@"
<span class=""content_tips left_float"">{0}</span>
<div class=""q_content left""><p>{1}</p>{2}<div class=""arrow_blue""></div></div>", DateUtils.GetDateAndTimeString(answerInfo.AddDate), answerInfo.Content, file);
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
                RequestAnswerInfo answerInfoToAdd = new RequestAnswerInfo(base.Request.Form);
                answerInfoToAdd.RequestID = this.requestInfo.ID;
                answerInfoToAdd.AddDate = DateTime.Now;
                answerInfoToAdd.IsAnswer = true;

                DataProviderB2C.RequestAnswerDAO.Insert(answerInfoToAdd);

                base.SuccessMessage("工单回复成功");

                base.AddWaitAndRedirectScript(BackgroundRequestAnswer.GetRedirectUrl(base.PublishmentSystemID, this.requestInfo.ID, this.returnUrl));
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
