using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using SiteServer.BBS.Core.TemplateParser;
using System.Collections.Specialized;
using SiteServer.BBS.Core.TemplateParser.Model;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.IO;
using BaiRong.Model;
using BaiRong.Core.Drawing;

namespace SiteServer.BBS.Pages
{
    public class PasswordPage : BasePage
    {
        public Literal UserName;
        public TextBox CurrentPassword;
        public TextBox NewPassword;
        public TextBox ConfirmNewPassword;

        protected override bool IsAccessable
        {
            get
            {
                return false;
            }
        }

        public void Page_Load(object sender, EventArgs e)
        {
            this.UserName.Text = BaiRongDataProvider.UserDAO.CurrentUserName;
        }

        public void Submit_Click(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                string errorMessage = string.Empty;
                if (!BaiRongDataProvider.UserDAO.Validate(base.PublishmentSystemInfo.GroupSN, BaiRongDataProvider.UserDAO.CurrentUserName, this.CurrentPassword.Text, out errorMessage))
                {
                    base.FailureMessage("您输入的当前密码有误，请重新输入！");
                }
                else if (this.NewPassword.Text.Length < UserConfigManager.Additional.UserMinPasswordLength)
                {
                    base.FailureMessage(string.Format("密码长度必须大于等于{0}！", UserConfigManager.Additional.UserMinPasswordLength));
                }
                else
                {
                    try
                    {
                        BaiRongDataProvider.UserDAO.ChangePassword(UserManager.Current.UserID, this.NewPassword.Text);
                        base.SuccessMessage("密码修改成功！", PageUtilityBBS.GetIndexPageUrl(base.PublishmentSystemID));
                    }
                    catch (Exception ex)
                    {
                        base.FailureMessage(string.Format("密码修改失败：{0}！", ex.Message));
                    }
                }
            }
        }
    }
}
