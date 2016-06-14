using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;

using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class UserPassword : BackgroundBasePage
	{
		public Label UserName;
		public TextBox Password;

        private int userID;

        public static string GetOpenWindowString(int publishmentSystemID, int userID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("userID", userID.ToString());
            return PageUtility.GetOpenWindowString("��������", "modal_userPassword.aspx", arguments, 400, 300);
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
                    if (this.Password.Text.Length < UserConfigManager.Additional.UserMinPasswordLength)
                    {
                        base.FailMessage(string.Format("���볤�ȱ�����ڵ���{0}��", UserConfigManager.Additional.UserMinPasswordLength));
                        return;
                    }
                    BaiRongDataProvider.UserDAO.ChangePassword(this.userID, this.Password.Text);

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
