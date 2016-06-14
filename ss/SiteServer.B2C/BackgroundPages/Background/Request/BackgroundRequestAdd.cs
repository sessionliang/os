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



using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundRequestAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public Literal ltlAdminUserName;
        public BREditor Content;

        private RequestInfo requestInfo;
        private RequestAnswerInfo answerInfo;
        private string returnUrl;

        public static string GetAddUrl(int publishmentSystemID, string returnUrl)
        {
            return PageUtils.GetB2CUrl(string.Format("background_requestAdd.aspx?publishmentSystemID={0}&returnUrl={1}", publishmentSystemID, StringUtils.ValueToUrl(returnUrl)));
        }

        public static string GetEditUrl(int publishmentSystemID, int id, string returnUrl)
        {
            return PageUtils.GetB2CUrl(string.Format("background_requestAdd.aspx?publishmentSystemID={0}&id={1}&returnUrl={2}", publishmentSystemID, id, StringUtils.ValueToUrl(returnUrl)));
        }

        public override string GetValue(string attributeName)
        {
            string value = string.Empty;
            if (this.requestInfo != null)
            {
                value = Convert.ToString(this.requestInfo.GetValue(attributeName));
            }
            string retval = value;

            if (attributeName == "SelectUserNameClick")
            {
                retval = SiteServer.CMS.BackgroundPages.Modal.UserSelect.GetOpenWindowString(base.PublishmentSystemID, "userName");
            }
            else if (attributeName == "SelectUserName")
            {
                retval = this.GetValue(RequestAttribute.UserName);

                retval = string.IsNullOrEmpty(retval) ? "选择用户" : retval;
            }

            return retval;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int id = base.GetIntQueryString("ID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundRequest.GetRedirectUrl(base.PublishmentSystemID, true, string.Empty, string.Empty, 1);
            }

            if (id > 0)
            {
                this.requestInfo = DataProviderB2C.RequestDAO.GetRequestInfo(id);
                this.answerInfo = DataProviderB2C.RequestAnswerDAO.GetFirstAnswerInfoByRequestID(id);
                //base.AddAttributes(this.requestInfo);
                //base.AddAttributes(this.answerInfo);

                this.ltlPageTitle.Text = "修改工单";
            }
            else
            {
                this.ltlPageTitle.Text = "新增工单";
            }

            if (!IsPostBack)
            {
                this.Content.Text = this.GetValue(RequestAnswerAttribute.Content);

                if (this.requestInfo != null)
                {
                    this.ltlAdminUserName.Text = string.Format(@"<div class=""btn_pencil"" onclick=""{0}""><span class=""pencil""></span>　选择</div><script language=""javascript"">chargeUserName('{1}', '{2}');</script>", BaiRong.BackgroundPages.Modal.AdminSelect.GetShowPopWinString(AdminManager.Current.DepartmentID, "chargeUserName"), AdminManager.GetDisplayName(this.requestInfo.AdminUserName, true), this.requestInfo.AdminUserName);
                }
                else
                {
                    this.ltlAdminUserName.Text = string.Format(@"<div class=""btn_pencil"" onclick=""{0}""><span class=""pencil""></span>　选择</div><script language=""javascript"">chargeUserName('{1}', '{2}');</script>", BaiRong.BackgroundPages.Modal.AdminSelect.GetShowPopWinString(AdminManager.Current.DepartmentID, "chargeUserName"), AdminManager.GetDisplayName(AdminManager.Current.UserName, true), AdminManager.Current.UserName);
                }
            }
        }

        public string GetRequestTypeOptions()
        {
            StringBuilder builder = new StringBuilder();

            foreach (string requestType in B2CConfigurationManager.GetPublishmentSystemAdditional(base.PublishmentSystemID).RequestTypeCollection)
            {
                string selected = string.Empty;
                if (this.requestInfo != null && this.requestInfo.RequestType == requestType)
                {
                    selected = "selected";
                }
                builder.AppendFormat(@"<option {0} value=""{1}"">{1}</option>", selected, requestType);
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
                    RequestInfo requestInfoToEdit = new RequestInfo(base.Request.Form);
                    foreach (string attributeName in RequestAttribute.AllAttributes)
                    {
                        this.requestInfo.SetValue(attributeName, requestInfoToEdit.GetValue(attributeName));
                    }

                    DataProviderB2C.RequestDAO.Update(this.requestInfo);

                    if (this.answerInfo != null)
                    {
                        RequestAnswerInfo answerInfoToEdit = new RequestAnswerInfo(base.Request.Form);
                        foreach (string attributeName in RequestAttribute.AllAttributes)
                        {
                            this.answerInfo.SetValue(attributeName, answerInfoToEdit.GetValue(attributeName));
                        }

                        DataProviderB2C.RequestAnswerDAO.Update(this.answerInfo);
                    }
                }
                else
                {
                    RequestInfo requestInfoToAdd = new RequestInfo(base.Request.Form);
                    requestInfoToAdd.SN = StringUtils.GetShortGUID(true);
                    requestInfoToAdd.AdminUserName = AdminManager.Current.UserName;
                    requestInfoToAdd.AddDate = DateTime.Now;

                    int requestID = DataProviderB2C.RequestDAO.Insert(requestInfoToAdd);

                    RequestAnswerInfo answerInfoToAdd = new RequestAnswerInfo(base.Request.Form);
                    answerInfoToAdd.RequestID = requestID;
                    answerInfoToAdd.AddDate = DateTime.Now;

                    DataProviderB2C.RequestAnswerDAO.Insert(answerInfoToAdd);

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
