using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;


namespace BaiRong.BackgroundPages.Modal
{
	public class AdminPassword : BackgroundBasePage
	{
		public Label UserName;
		public TextBox Password;

        private string theUserName;

        public static string GetOpenWindowString(string userName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("userName", userName);
            return PageUtilityPF.GetOpenWindowString("重设密码", "modal_adminPassword.aspx", arguments, 400, 300);
        }
        
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.theUserName = base.GetQueryString("userName");

			if (!IsPostBack)
			{
                if (!string.IsNullOrEmpty(this.theUserName) && BaiRongDataProvider.AdministratorDAO.IsUserNameExists(this.theUserName))
                {
                    UserName.Text = this.theUserName;
                }
                else
                {
                    base.FailMessage("此帐户不存在！");
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
			if (IsPostBack && IsValid)
			{
				try
				{
                    if (this.Password.Text.Length < UserConfigManager.Additional.AdminMinPasswordLength)
                    {
                        base.FailMessage(string.Format("密码长度必须大于等于{0}！", UserConfigManager.Additional.AdminMinPasswordLength));
                        return;
                    }
                    BaiRongDataProvider.AdministratorDAO.ChangePassword(this.theUserName, EPasswordFormat.Encrypted, this.Password.Text);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "重设管理员密码", string.Format("管理员:{0}", this.theUserName));

                    base.SuccessMessage("重设密码成功！");

                    JsUtils.OpenWindow.CloseModalPage(Page);
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "重设密码失败！");
				}
			}
		}

	}
}
