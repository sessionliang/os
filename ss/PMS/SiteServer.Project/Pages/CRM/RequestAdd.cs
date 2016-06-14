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



namespace SiteServer.Project.BackgroundPages
{
    public class RequestAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public BREditor Content;

        private RequestInfo requestInfo;
        private RequestAnswerInfo answerInfo;

        private string domain = string.Empty;
        private int licenseID = 0;
        private int accountID = 0;

        protected override bool IsAccessable
        {
            get
            {
                return true;
            }
        }

        public static string GetAddUrl(string returnUrl)
        {
            return string.Format("background_requestAdd.aspx?ReturnUrl={0}", StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetEditUrl(int id, string returnUrl)
        {
            return string.Format("background_requestAdd.aspx?ID={0}&ReturnUrl={1}", id, StringUtils.ValueToUrl(returnUrl));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int id = TranslateUtils.ToInt(base.GetQueryString("ID"));
            this.domain = base.GetQueryString("domain");
            this.licenseID = TranslateUtils.ToInt(base.GetQueryString("licenseID"));
            bool isValid = !string.IsNullOrEmpty(this.domain) && this.licenseID > 0;
            if (isValid)
            {
                isValid = this.licenseID == DataProvider.DBLicenseDAO.GetLicenseID(domain);
            }

            if (isValid)
            {
                this.accountID = DataProvider.DBLicenseDAO.GetAccountID(this.licenseID);

                if (id > 0)
                {
                    this.requestInfo = DataProvider.RequestDAO.GetRequestInfo(id);
                    this.answerInfo = DataProvider.RequestAnswerDAO.GetFirstAnswerInfoByRequestID(id);
                    base.AddAttributes(this.requestInfo);
                    base.AddAttributes(this.answerInfo);

                    this.ltlPageTitle.Text = "修改工单";
                }
                else
                {
                    this.ltlPageTitle.Text = "新增工单";
                    NameValueCollection attributes = new NameValueCollection();
                    attributes[RequestAttribute.Website] = this.domain;
                    RequestInfo lastRequestInfo = DataProvider.RequestDAO.GetLastRequestInfo(this.domain);
                    if (lastRequestInfo != null)
                    {
                        attributes[RequestAttribute.Email] = lastRequestInfo.Email;
                        attributes[RequestAttribute.Mobile] = lastRequestInfo.Mobile;
                        attributes[RequestAttribute.QQ] = lastRequestInfo.QQ;
                    }
                    base.AddAttributes(attributes);
                }

                string uploadImageUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "Image");
                string uploadScrawlUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "Scrawl");
                string uploadFileUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "File");
                string imageManagerUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "ImageManager");
                string getMovieUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "GetMovie");
                //this.breContent.SetUrl(uploadImageUrl, uploadScrawlUrl, uploadFileUrl, imageManagerUrl, getMovieUrl);

                if (!IsPostBack)
                {
                    this.Content.Text = this.GetValue(RequestAnswerAttribute.Content);
                }
            }
            else
            {
                Page.Response.Write(string.Empty);
                Page.Response.End();
            }
        }

        public string GetRequestTypeOptions()
        {
            StringBuilder builder = new StringBuilder();

            foreach (string requestType in ConfigurationManager.Additional.CRMRequestTypeCollection)
            {
                builder.AppendFormat(@"<option {0} value=""{1}"">{1}</option>", base.GetSelected("RequestType", requestType), requestType);
            }

            return builder.ToString();
        }

        public void Return_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(string.Format("request.aspx?domain={0}&licenseID={1}", this.domain, this.licenseID));
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                string location = base.Request.Form["location"];

                if (this.requestInfo != null)
                {
                    RequestInfo requestInfoToEdit = DataProvider.RequestDAO.GetRequestInfo(base.Request.Form);
                    foreach (string attributeName in RequestAttribute.BasicAttributes)
                    {
                        this.requestInfo.SetExtendedAttribute(attributeName, requestInfoToEdit.GetExtendedAttribute(attributeName));
                    }

                    DataProvider.RequestDAO.Update(this.requestInfo);

                    if (this.answerInfo != null)
                    {
                        RequestAnswerInfo answerInfoToEdit = DataProvider.RequestAnswerDAO.GetAnswerInfo(base.Request.Form);
                        foreach (string attributeName in RequestAttribute.BasicAttributes)
                        {
                            this.answerInfo.SetExtendedAttribute(attributeName, answerInfoToEdit.GetExtendedAttribute(attributeName));
                        }

                        DataProvider.RequestAnswerDAO.Update(this.answerInfo);
                    }
                }
                else
                {
                    RequestInfo requestInfoToAdd = DataProvider.RequestDAO.GetRequestInfo(base.Request.Form);
                    requestInfoToAdd.LicenseID = this.licenseID;
                    requestInfoToAdd.Domain = this.domain.ToLower();
                    requestInfoToAdd.AccountID = this.accountID;
                    requestInfoToAdd.RequestSN = StringUtils.GetShortGUID(true);
                    requestInfoToAdd.AddDate = DateTime.Now;

                    int requestID = DataProvider.RequestDAO.Insert(requestInfoToAdd);

                    RequestAnswerInfo answerInfoToAdd = DataProvider.RequestAnswerDAO.GetAnswerInfo(base.Request.Form);
                    answerInfoToAdd.RequestID = requestID;
                    answerInfoToAdd.AddDate = DateTime.Now;
                    if (string.IsNullOrEmpty(answerInfoToAdd.Content))
                    {
                        answerInfoToAdd.Content = requestInfoToAdd.Subject;
                    }

                    DataProvider.RequestAnswerDAO.Insert(answerInfoToAdd);

                    base.SuccessMessage("工单添加成功");
                }

                base.AddWaitAndRedirectScript(string.Format("request.aspx?domain={0}&licenseID={1}", this.domain, this.licenseID));
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
