using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;


namespace BaiRong.BackgroundPages.Modal
{
	public class UserPassword : BackgroundBasePage
	{
		public Label UserName;
		public TextBox Password;

        private int userID;

        public static string GetOpenWindowString(int userID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("userID", userID.ToString());
            return PageUtilityPF.GetOpenWindowString("重设密码", "modal_userPassword.aspx", arguments, 400, 300);
        }
        
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.userID = TranslateUtils.ToInt(base.GetQueryString("userID"));

			if (!IsPostBack)
			{
                string userName = BaiRongDataProvider.UserDAO.GetUserName(this.userID);

                if (!string.IsNullOrEmpty(userName))
                {
                    UserName.Text = userName;
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
                    if (this.Password.Text.Length < UserConfigManager.Additional.UserMinPasswordLength)
                    {
                        base.FailMessage(string.Format("密码长度必须大于等于{0}！", UserConfigManager.Additional.UserMinPasswordLength));
                        return;
                    }
                    BaiRongDataProvider.UserDAO.ChangePassword(this.userID, this.Password.Text);

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
