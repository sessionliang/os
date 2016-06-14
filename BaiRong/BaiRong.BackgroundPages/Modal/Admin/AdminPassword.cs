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
            return PageUtilityPF.GetOpenWindowString("��������", "modal_adminPassword.aspx", arguments, 400, 300);
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
                    base.FailMessage("���ʻ������ڣ�");
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
                        base.FailMessage(string.Format("���볤�ȱ�����ڵ���{0}��", UserConfigManager.Additional.AdminMinPasswordLength));
                        return;
                    }
                    BaiRongDataProvider.AdministratorDAO.ChangePassword(this.theUserName, EPasswordFormat.Encrypted, this.Password.Text);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "�������Ա����", string.Format("����Ա:{0}", this.theUserName));

                    base.SuccessMessage("��������ɹ���");

                    JsUtils.OpenWindow.CloseModalPage(Page);
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "��������ʧ�ܣ�");
				}
			}
		}

	}
}
