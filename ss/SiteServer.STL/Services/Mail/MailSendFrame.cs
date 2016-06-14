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
using System.Collections.Specialized;
using System.Web;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Services
{
    public class MailSendFrame : BasePage
    {
        public TextBox Receiver;
        public TextBox Mail;
        public TextBox Sender;
        protected PlaceHolder phValidateCode;
        public TextBox ValidateCode;
        public Literal ltlValidateCodeImage;
        public Literal ltlError;

        private VCManager vcManager;
        private int channelID;
        private int contentID;
        private string pageTitle;
        private string pageUrl;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.vcManager = ValidateCodeManager.GetInstance(base.PublishmentSystemID, ValidateCodeManager.CODE_SEND_MAIL, PageUtility.IsCrossDomain(base.PublishmentSystemInfo));
            this.channelID = TranslateUtils.ToInt(base.Request.QueryString["channelID"]);
            this.contentID = TranslateUtils.ToInt(base.Request.QueryString["contentID"]);
            this.pageTitle = PageUtils.FilterXSS(base.Request.QueryString["pageTitle"]);
            this.pageUrl = PageUtils.FilterXSS(base.Request.QueryString["pageUrl"]);

            if (FileConfigManager.Instance.IsValidateCode)
            {
                this.ltlValidateCodeImage.Text = string.Format(@"<img id=""imgVerify"" name=""imgVerify"" src=""{0}"" align=""absmiddle"" />", this.vcManager.GetImageUrl(true));
            }
            else
            {
                this.phValidateCode.Visible = false;
            }
        }

        protected void Submit_OnClick(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Receiver.Text))
            {
                this.ltlError.Text = "alert('请填写好友姓名。');";
                return;
            }
            if (string.IsNullOrEmpty(this.Mail.Text))
            {
                this.ltlError.Text = "alert('请填写好友邮箱地址。');";
                return;
            }
            if (string.IsNullOrEmpty(this.Sender.Text))
            {
                this.ltlError.Text = "alert('请填写您的姓名。');";
                return;
            }
            if (FileConfigManager.Instance.IsValidateCode)
            {
                if (string.IsNullOrEmpty(this.ValidateCode.Text))
                {
                    this.ltlError.Text = "alert('请填写验证码。');";
                    return;
                }

                if (!this.vcManager.IsCodeValid(this.ValidateCode.Text))
                {
                    this.ltlError.Text = "alert('验证码不正确，请重新输入。');";
                    return;
                }
            }

            try
            {
                string errorMessage;

                ContentInfo contentInfo = null;
                if (this.contentID > 0)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.channelID);
                    ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
                    contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, this.contentID);
                }

                string title = MessageManager.GetFormattedSendMailString(base.PublishmentSystemInfo.Additional.MailSendTitle, base.PublishmentSystemInfo, contentInfo, this.Receiver.Text, this.Sender.Text, this.pageTitle, this.pageUrl);
                string content = MessageManager.GetFormattedSendMailString(base.PublishmentSystemInfo.Additional.MailSendContent, base.PublishmentSystemInfo, contentInfo, this.Receiver.Text, this.Sender.Text, this.pageTitle, this.pageUrl);

                bool isSuccess = MessageManager.SendMailByPublishmentSystemID(base.PublishmentSystemID, this.Mail.Text, title, content, out errorMessage);

                if (isSuccess)
                {
                    if (base.PublishmentSystemInfo.Additional.IsLogMailSend)
                    {
                        MailSendLogInfo logInfo = new MailSendLogInfo(0, base.PublishmentSystemID, this.channelID, this.contentID, (contentInfo != null) ? contentInfo.Title : this.pageTitle, this.pageUrl, this.Receiver.Text, this.Mail.Text, this.Sender.Text, PageUtils.GetIPAddress(), DateTime.Now);
                        DataProvider.MailSendLogDAO.Insert(logInfo);
                    }
                    Response.Write("<script>alert('邮件发送成功！');parent.stlCloseWindow();</script>");
                    Response.End();
                    Response.Flush();
                }
                else
                {
                    this.ltlError.Text = string.Format("alert('{0}');", StringUtils.ToJsString(errorMessage));
                }
            }
            catch (Exception ex)
            {
                this.ltlError.Text = string.Format("alert('邮件发送失败：{0}');", StringUtils.ToJsString(ex.Message));
            }
        }
    }
}
