using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Controls;
using System.Text;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;

namespace BaiRong.BackgroundPages
{
    public class BackgroundBasePageEnd : Page
    {
        public Literal ltlBreadCrumb;

        public Message messageCtrl;

        private bool messageImmidiatary = true;
        private MessageUtils.Message.EMessageType messageType;
        private string message = string.Empty;
        private string scripts = string.Empty;

        private void setMessage(MessageUtils.Message.EMessageType messageType, Exception ex, string message)
        {
            this.messageType = messageType;
            if (ex != null)
            {
                this.message = string.Format("{0}<!-- {1} -->", message, ex.ToString());
            }
            else
            {
                this.message = message;
            }
        }

        protected virtual bool IsAccessable
        {
            get { return false; }
        }

        protected virtual bool IsSinglePage
        {
            get { return false; }
        }

        protected virtual bool IsSaasForbidden
        {
            get { return false; }
        }

        private bool isForbidden = false;
        protected bool IsForbidden
        {
            get { return isForbidden; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (!IsAccessable && !BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
            {
                isForbidden = true;
                PageUtils.RedirectToLoginPage();
            }
            if (IsSaasForbidden && FileConfigManager.Instance.IsSaas)
            {
                isForbidden = true;
                PageUtils.RedirectToLoginPage();
            }
            base.OnInit(e);

            //·ÀÖ¹csrf¹¥»÷
            base.Response.AddHeader("X-Frame-Options", "SAMEORIGIN");
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(this.message))
            {
                if (this.messageImmidiatary && this.messageCtrl != null)
                {
                    this.messageCtrl.IsShowImmidiatary = true;
                    this.messageCtrl.MessageType = this.messageType;
                    this.messageCtrl.Content = this.message;
                }
                else
                {
                    MessageUtils.SaveMessage(this.messageType, message);
                }
            }

            //base.Render(writer);
            StringBuilder tBuilder = new StringBuilder();
            System.Web.UI.HtmlTextWriter tWriter = new System.Web.UI.HtmlTextWriter(new System.IO.StringWriter(tBuilder));
            base.RenderChildren(tWriter);
            writer.Write(TranslatePage(tBuilder.ToString()));
            
            if (!this.IsAccessable && !this.IsSinglePage)
            {
                AjaxUrlInfo ajaxUrlInfo = AjaxUrlManager.FetchAjaxUrlInfo();
                if (ajaxUrlInfo != null)
                {
                    this.scripts += string.Format(@"
top.submitAjaxUrl('{0}', '{1}');
", ajaxUrlInfo.AjaxUrl, ajaxUrlInfo.Parameters);
                }

                writer.Write(@"<script type=""text/javascript"">
if (window.top.location.href.toLowerCase().indexOf(""main.aspx"") == -1){{
	var initializationUrl = window.top.location.href.toLowerCase().substring(0, window.top.location.href.toLowerCase().indexOf(""/{0}/"")) + ""/{0}/initialization.aspx"";
	window.top.location.href = initializationUrl;
}}
</script>", FileConfigManager.Instance.AdminDirectoryName);
            }

            if (!string.IsNullOrEmpty(this.scripts))
            {
                writer.Write(@"<script type=""text/javascript"">{0}</script>", this.scripts);
            }
        }

        private string TranslatePage(string content)
        {
            string language = AdminManager.Current.Language;
            if (string.IsNullOrEmpty(language) && base.Request.UserLanguages != null)
            {
                language = base.Request.UserLanguages[0];
            }
            TransManager transManager = TransManager.GetTransManager(language);
            return transManager.GetTransContent(content);
        }

        protected string GetQueryString(string name)
        {
            return GetQueryStringNoSql(name);
        }

        protected int GetIntQueryString(string name)
        {
            return TranslateUtils.ToInt(base.Request.QueryString[name]);
        }

        protected bool GetBoolQueryString(string name)
        {
            return TranslateUtils.ToBool(base.Request.QueryString[name]);
        }

        protected string GetQueryStringNoSql(string name)
        {
            string value = base.Request.QueryString[name];
            if (value == null) return null;
            return PageUtils.FilterSql(value);
        }

        protected string GetQueryStringNoXss(string name)
        {
            string value = base.Request.QueryString[name];
            if (value == null) return null;
            return PageUtils.FilterXSS(value);
        }

        protected string GetQueryStringNoSqlAndXss(string name)
        {
            string value = base.Request.QueryString[name];
            if (value == null) return null;
            return PageUtils.FilterSqlAndXss(value);
        }

        public void AddScript(string script)
        {
            this.scripts += script;
        }

        public virtual void AddWaitAndRedirectScript(string redirectUrl)
        {
            this.scripts += string.Format(@"
setTimeout(""_goto('{0}')"", 1500);
$('.operation-area').hide();
", redirectUrl);
        }

        public void FailMessage(Exception ex, string message)
        {
            this.setMessage(MessageUtils.Message.EMessageType.Error, ex, message);
        }

        public void FailMessage(string message)
        {
            this.setMessage(MessageUtils.Message.EMessageType.Error, null, message);
        }

        public void SuccessMessage(string message)
        {
            this.setMessage(MessageUtils.Message.EMessageType.Success, null, message);
        }

        public void SuccessMessage()
        {
            this.SuccessMessage("²Ù×÷³É¹¦£¡");
        }

        public void InfoMessage(string message)
        {
            this.setMessage(MessageUtils.Message.EMessageType.Info, null, message);
        }

        public void SuccessDeleteMessage()
        {
            SuccessMessage(MessageUtils.DeleteSuccess);
        }

        public void SuccessUpdateMessage()
        {
            SuccessMessage(MessageUtils.UpdateSuccess);
        }

        public void SuccessCheckMessage()
        {
            SuccessMessage(MessageUtils.CheckSuccess);
        }

        public void SuccessInsertMessage()
        {
            SuccessMessage(MessageUtils.InsertSuccess);
        }

        public void FailInsertMessage(Exception ex)
        {
            FailMessage(ex, MessageUtils.InsertFail);
        }

        public void FailUpdateMessage(Exception ex)
        {
            FailMessage(ex, MessageUtils.UpdateFail);
        }

        public void FailDeleteMessage(Exception ex)
        {
            FailMessage(ex, MessageUtils.DeleteFail);
        }

        public void FailCheckMessage(Exception ex)
        {
            FailMessage(ex, MessageUtils.CheckFail);
        }

        public string MaxLengthText(string str, int length)
        {
            return StringUtils.MaxLengthText(str, length);
        }

        public Control FindControlBySelfAndChildren(string controlID)
        {
            return ControlUtils.FindControlBySelfAndChildren(controlID, this);
        }

        private string module = null;
        public string Module
        {
            get
            {
                if (module == null)
                {
                    module = this.GetQueryStringNoXss("module");
                }
                return module;
            }
        }

        public virtual void BreadCrumb(string leftMenuID, string pageTitle, string permission)
        {
            
        }

        public virtual void Submit_OnClick(object sender, System.EventArgs e)
        {
            JsUtils.OpenWindow.CloseModalPage(base.Page);
        }
    }
}
