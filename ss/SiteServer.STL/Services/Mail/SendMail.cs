using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using SiteServer.CMS.Model;
using BaiRong.Core.Net;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Services
{
    public class SendMail : BasePage
    {
        private string pageUrl;
        private ECharset charset;
        private string pageTitle;

        public PlaceHolder PlaceHolderInput;
        protected TextBox Mail;

        public PlaceHolder PlaceHolderSuccess;
        public Literal MessageSuccess;
        public PlaceHolder PlaceHolderFailure;
        public Literal MessageFailure;

        public void Page_Load(object sender, System.EventArgs e)
        {
            this.pageUrl = PageUtils.FilterXSS(base.Request.QueryString["pageUrl"]);
            this.charset = ECharsetUtils.GetEnumType(base.Request.QueryString["charset"]);
            this.pageTitle = PageUtils.FilterXSS(base.Request.QueryString["pageTitle"]);

            if (!Page.IsPostBack)
            {
                this.PlaceHolderInput.Visible = true;
            }
        }

        public string GetTitle()
        {
            if (string.IsNullOrEmpty(this.pageTitle))
            {
                return "页面转发";
            }
            return this.pageTitle;
        }

        public void Send_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    this.PlaceHolderInput.Visible = false;

                    string errorMessage;

                    bool isSuccess = MessageManager.SendMailByPublishmentSystemID(base.PublishmentSystemID, this.Mail.Text, this.GetTitle(), WebClientUtils.GetRemoteFileSource(this.pageUrl, this.charset), out errorMessage);

                    if (isSuccess)
                    {
                        this.PlaceHolderSuccess.Visible = true;
                        this.MessageSuccess.Text = "邮件发送成功！";
                    }
                    else
                    {
                        this.PlaceHolderFailure.Visible = true;
                        this.MessageFailure.Text = "邮件发送失败！";
                    }
                }
                catch (Exception ex)
                {
                    this.PlaceHolderFailure.Visible = true;
                    this.MessageFailure.Text = "邮件发送失败：" + ex.Message;
                }
            }
        }
    }
}
