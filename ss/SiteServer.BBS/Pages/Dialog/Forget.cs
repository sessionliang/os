using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Web.Security;
using System.Web.UI;


using BaiRong.Core;

namespace SiteServer.BBS.Pages.Dialog
{
    public class Forget : BasePage
    {
        public TextBox tbUserName;
        public TextBox tbVerifyCode;

        public Label lblErrorMessage_UserName;
        public Label lblErrorMessage_VerifyCode;
        public Label lblSuccess;

        public PlaceHolder phValidateCode;
        public Literal ltlValidateCode;

        private VCManager vcManager;

        public Button btnforget;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.vcManager = VCManager.GetInstanceOfLogin();
            if (!IsPostBack)
            {
                if (FileConfigManager.Instance.IsValidateCode)
                {
                    this.ltlValidateCode.Text = string.Format(@"<img id=""imgVerify"" name=""imgVerify"" src=""{0}"" align=""absmiddle"" />", this.vcManager.GetImageUrl(true));
                }
                else
                {
                    this.phValidateCode.Visible = false;
                }
            }
        }

        protected void btnforget_Click(object sender, EventArgs e)
        {
            string userName = tbUserName.Text.Trim();
            string verifyCode = tbVerifyCode.Text.Trim();

            if (!BaiRongDataProvider.UserDAO.IsUserExists(base.PublishmentSystemInfo.GroupSN, userName))
            {
                this.lblErrorMessage_UserName.Text = "此用户不存在，请重新输入";
                return;
            }
            else if (!this.vcManager.IsCodeValid(verifyCode))
            {
                this.lblErrorMessage_UserName.Text = string.Empty;
                this.lblErrorMessage_VerifyCode.Text = "验证码不正确，请重新输入";
                return;
            }
            else
            {
                this.lblErrorMessage_UserName.Text = string.Empty;
                this.lblErrorMessage_VerifyCode.Text = string.Empty;

                int userID = BaiRongDataProvider.UserDAO.GetUserID(base.PublishmentSystemInfo.GroupSN, userName);

                string mail = BaiRongDataProvider.UserDAO.GetEmail(userID);
                string password = BaiRongDataProvider.UserDAO.GetPassword(userID);
                StringBuilder mailBody = new StringBuilder();
                mailBody.Append("尊敬的<strong>");
                mailBody.Append(userName);
                mailBody.Append("</strong>：");
                mailBody.Append("<br/>");
                mailBody.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
                mailBody.Append("您好，恭喜您您的密码已找回:<strong>");
                mailBody.Append(password);
                mailBody.Append("</strong>。如果您意外地收到此邮件，很可能是其他用户在尝试重设密码时，误输入了您的电子邮件地址。如果您没有提出此请求，则无需做进一步的操作，可以放心地忽略此电子邮件。<br />");
                mailBody.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
                mailBody.Append("此邮件为确认邮件，我们不会对此邮件的回复进行答复。");

                string errorMessage = string.Empty;
                UserMailManager.SendMail(mail, "网站密码提醒", mailBody.ToString(), out errorMessage);

                Response.Write("<script language='javascript' defer>alert('密码已发至您的邮箱，请注意查收');window.top.location.href = '../login.aspx';</script>");
            }
        }
    }
}
