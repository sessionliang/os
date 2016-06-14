using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;



namespace BaiRong.BackgroundPages.Modal
{
	public class AdminView : BackgroundBasePage
	{
        protected Literal ltlUserName;
        protected Literal ltlDisplayName;
        protected Literal ltlCreationDate;
        protected Literal ltlLastActivityDate;
        protected Literal ltlEmail;
        protected Literal ltlMobile;
        protected Literal ltlRoles;

        public static string GetOpenWindowString(string userName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("UserName", userName);
            return PageUtilityPF.GetOpenWindowString("查看管理员资料", "modal_adminView.aspx", arguments, 400, 450, true);
        }
	
		public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            string userName = base.GetQueryString("UserName");
            AdministratorInfo adminInfo = BaiRongDataProvider.AdministratorDAO.GetAdministratorInfo(userName);
            this.ltlUserName.Text = adminInfo.UserName;
            this.ltlDisplayName.Text = adminInfo.DisplayName;
            this.ltlCreationDate.Text = DateUtils.GetDateAndTimeString(adminInfo.CreationDate);
            this.ltlLastActivityDate.Text = DateUtils.GetDateAndTimeString(adminInfo.LastActivityDate);
            this.ltlEmail.Text = adminInfo.Email;
            this.ltlMobile.Text = adminInfo.Mobile;
            this.ltlRoles.Text = AdminManager.GetRolesHtml(userName);
		}
	}
}
