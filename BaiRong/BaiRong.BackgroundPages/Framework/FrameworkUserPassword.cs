using System;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;



namespace BaiRong.BackgroundPages
{
    public class FrameworkUserPassword : BackgroundBasePage
    {
        public Literal UserName;
        public TextBox CurrentPassword;
        public TextBox NewPassword;
        public TextBox ConfirmNewPassword;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!Page.IsPostBack)
            {
                this.UserName.Text = AdminManager.Current.UserName;
            }
        }

        public void Submit_Click(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                AdministratorInfo adminInfo = AdminManager.Current;

                if (BaiRongDataProvider.AdministratorDAO.CheckPassword(this.CurrentPassword.Text, adminInfo.Password, adminInfo.PasswordFormat, adminInfo.PasswordSalt))
                {
                    bool isChanged = BaiRongDataProvider.AdministratorDAO.ChangePassword(adminInfo.UserName, adminInfo.PasswordFormat, NewPassword.Text);
                    if (isChanged)
                    {
                        base.SuccessMessage("密码更改成功");
                    }
                    else
                    {
                        base.FailMessage("新密码不符合要求");
                    }
                }
                else
                {
                    base.FailMessage("当前帐号密码错误");
                }
            }
        }
    }
}
