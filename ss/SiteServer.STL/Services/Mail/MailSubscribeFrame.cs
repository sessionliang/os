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
    public class MailSubscribeFrame : BasePage
    {
        public TextBox Receiver;
        public TextBox Mail;
        protected PlaceHolder phValidateCode;
        public TextBox ValidateCode;
        public Literal ltlValidateCodeImage;
        public Literal ltlError;

        private VCManager vcManager;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.vcManager = ValidateCodeManager.GetInstance(base.PublishmentSystemID, ValidateCodeManager.CODE_MAIL_SUBSCRIBE, PageUtility.IsCrossDomain(base.PublishmentSystemInfo));

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
                this.ltlError.Text = "alert('请填写您的姓名。');";
                return;
            }
            if (string.IsNullOrEmpty(this.Mail.Text))
            {
                this.ltlError.Text = "alert('请填写您的邮箱地址。');";
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

                if (DataProvider.MailSubscribeDAO.IsExists(base.PublishmentSystemID, this.Mail.Text))
                {
                    this.ltlError.Text = "alert('此邮箱已订阅邮件');";
                }
                else
                {
                    MailSubscribeInfo msInfo = new MailSubscribeInfo(0, base.PublishmentSystemID, this.Receiver.Text, this.Mail.Text, PageUtils.GetIPAddress(), DateTime.Now);
                    DataProvider.MailSubscribeDAO.Insert(msInfo);

                    string title = MessageManager.GetFormattedMailSubscribeString(base.PublishmentSystemInfo.Additional.MailSubscribeTitle, base.PublishmentSystemInfo, this.Receiver.Text);
                    string content = MessageManager.GetFormattedMailSubscribeString(base.PublishmentSystemInfo.Additional.MailSubscribeContent, base.PublishmentSystemInfo, this.Receiver.Text);

                    bool isSuccess = MessageManager.SendMailByPublishmentSystemID(base.PublishmentSystemID, this.Mail.Text, title, content, out errorMessage);

                    if (isSuccess)
                    {
                        Response.Write("<script>alert('邮件订阅成功！');parent.stlCloseWindow();</script>");
                        Response.End();
                        Response.Flush();
                    }
                    else
                    {
                        this.ltlError.Text = string.Format("alert('{0}');", StringUtils.ToJsString(errorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                this.ltlError.Text = string.Format("alert('邮件订阅失败：{0}');", StringUtils.ToJsString(ex.Message));
            }
        }
    }
}
