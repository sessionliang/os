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
    public class BackgroundRequestAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public Literal ltlChargeUserName;
        public BREditor Content;

        private RequestInfo requestInfo;
        private RequestAnswerInfo answerInfo;
        private string returnUrl;

        public static string GetAddUrl(string returnUrl)
        {
            return string.Format("background_requestAdd.aspx?ReturnUrl={0}", StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetEditUrl(int id, string returnUrl)
        {
            return string.Format("background_requestAdd.aspx?ID={0}&ReturnUrl={1}", id, StringUtils.ValueToUrl(returnUrl));
        }

        public override string GetValue(string attributeName)
        {
            string value = base.GetValue(attributeName);
            string retval = value;

            if (attributeName == "SelectAccount")
            {
                retval = Modal.AccountSelect.GetShowPopWinString("selectAccount", true);
            }
            else if (attributeName == "AccountName")
            {
                int accountID = TranslateUtils.ToInt(base.GetValue(RequestAttribute.AccountID));
                if (accountID > 0)
                {
                    retval = DataProvider.AccountDAO.GetAccountName(accountID);
                }

                retval = string.IsNullOrEmpty(retval) ? "选择客户" : retval;
            }

            return retval;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int id = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundRequest.GetRedirectUrl(true, string.Empty, string.Empty, 1);
            }

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

                if (this.requestInfo != null)
                {
                    this.ltlChargeUserName.Text = string.Format(@"<div class=""btn_pencil"" onclick=""{0}""><span class=""pencil""></span>　选择</div><script language=""javascript"">chargeUserName('{1}', '{2}');</script>", Modal.UserNameSelect.GetShowPopWinString(AdminManager.Current.DepartmentID, "chargeUserName"), AdminManager.GetDisplayName(this.requestInfo.ChargeUserName, true), this.requestInfo.ChargeUserName);
                }
                else
                {
                    this.ltlChargeUserName.Text = string.Format(@"<div class=""btn_pencil"" onclick=""{0}""><span class=""pencil""></span>　选择</div><script language=""javascript"">chargeUserName('{1}', '{2}');</script>", Modal.UserNameSelect.GetShowPopWinString(AdminManager.Current.DepartmentID, "chargeUserName"), AdminManager.GetDisplayName(AdminManager.Current.UserName, true), AdminManager.Current.UserName);
                }
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
            PageUtils.Redirect(this.returnUrl);
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
                    requestInfoToAdd.RequestSN = StringUtils.GetShortGUID(true);
                    requestInfoToAdd.AddUserName = AdminManager.Current.UserName;
                    requestInfoToAdd.AddDate = DateTime.Now;

                    int requestID = DataProvider.RequestDAO.Insert(requestInfoToAdd);

                    RequestAnswerInfo answerInfoToAdd = DataProvider.RequestAnswerDAO.GetAnswerInfo(base.Request.Form);
                    answerInfoToAdd.RequestID = requestID;
                    answerInfoToAdd.AddDate = DateTime.Now;

                    DataProvider.RequestAnswerDAO.Insert(answerInfoToAdd);

                    base.SuccessMessage("工单添加成功");
                }
                
                base.AddWaitAndRedirectScript(this.returnUrl);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
