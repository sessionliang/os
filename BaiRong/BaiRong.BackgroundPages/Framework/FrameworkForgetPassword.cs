using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;



namespace BaiRong.BackgroundPages
{
    public class FrameworkForgetPassword : BackgroundBasePage
	{
        protected PlaceHolder phUserNameTemplate;
        protected TextBox UserName;

        protected PlaceHolder phQuestionTemplate;
        protected Literal ltlQuestion;
        protected TextBox Answer;

        protected PlaceHolder phSuccessTemplate;
        protected Literal ltlUserName;
        protected Literal ltlPassword;

        protected PlaceHolder phFailureTemplate;
        protected Literal ltlErrorMessage;

		protected Literal Version;
        protected Literal NetVersion;
        protected Literal Database;
        protected Literal CompanyName;

        protected override bool IsAccessable
        {
            get { return true; }
        }
	
		public void Page_Load(object sender, System.EventArgs e)
		{
            if (base.IsForbidden) return;

            if (!FileConfigManager.Instance.IsForgetPassword)
            {
                PageUtils.RedirectToErrorPage("基于安全考虑，忘记密码功能已关闭，如需使用请与管理员联系！");
            }

			if (!Page.IsPostBack)
			{
                PageUtils.DetermineRedirectToInstaller();

                this.Version.Text = ProductManager.GetFullVersion();
                this.NetVersion.Text = string.Format("{0}.{1}", System.Environment.Version.Major, System.Environment.Version.Minor);
                this.Database.Text = EDatabaseTypeUtils.GetValue(BaiRongDataProvider.DatabaseType);
                this.CompanyName.Text = StringUtils.Constants.CompanyName;

                this.phUserNameTemplate.Visible = true;
                this.phQuestionTemplate.Visible = this.phSuccessTemplate.Visible = this.phFailureTemplate.Visible = false;
			}
		}

        public void Button_Command(object sender, CommandEventArgs e)
        {
            string commandName = ((CommandEventArgs)e).CommandName;
            if (commandName == "userName")
            {
                AdministratorInfo adminInfo = BaiRongDataProvider.AdministratorDAO.GetAdministratorInfo(this.UserName.Text);
                if (adminInfo == null)
                {
                    this.ltlErrorMessage.Text = "用户名不存在。";
                    this.phFailureTemplate.Visible = true;
                    this.phUserNameTemplate.Visible = this.phQuestionTemplate.Visible = this.phSuccessTemplate.Visible = false;
                }
                else
                {
                    this.ltlQuestion.Text = adminInfo.Question;
                    this.phQuestionTemplate.Visible = true;
                    this.phUserNameTemplate.Visible = this.phSuccessTemplate.Visible = this.phFailureTemplate.Visible = false;
                }
            }
            else if (commandName == "question")
            {
                AdministratorInfo adminInfo = BaiRongDataProvider.AdministratorDAO.GetAdministratorInfo(this.UserName.Text);
                if (adminInfo == null)
                {
                    this.ltlErrorMessage.Text = "用户名不存在。";
                    this.phFailureTemplate.Visible = true;
                    this.phUserNameTemplate.Visible = this.phQuestionTemplate.Visible = this.phSuccessTemplate.Visible = false;
                }
                else
                {
                    bool isSuccess = false;

                    if (!string.IsNullOrEmpty(this.Answer.Text))
                    {
                        if (!string.IsNullOrEmpty(adminInfo.Question) && !string.IsNullOrEmpty(adminInfo.Answer))
                        {
                            if (this.Answer.Text == adminInfo.Answer)
                            {
                                string password = adminInfo.Password;
                                if (adminInfo.PasswordFormat != EPasswordFormat.Hashed)
                                {
                                    password = BaiRongDataProvider.AdministratorDAO.GetPassword(adminInfo.Password, adminInfo.PasswordFormat, adminInfo.PasswordSalt);
                                }
                                else
                                {
                                    password = "123456";
                                    BaiRongDataProvider.AdministratorDAO.ChangePassword(adminInfo.UserName, adminInfo.PasswordFormat, password);
                                }
                                this.ltlUserName.Text = adminInfo.DisplayName;
                                this.ltlPassword.Text = password;
                                this.phSuccessTemplate.Visible = true;
                                this.phUserNameTemplate.Visible = this.phQuestionTemplate.Visible = this.phFailureTemplate.Visible = false;

                                isSuccess = true;
                            }
                        }
                    }

                    if (!isSuccess)
                    {
                        this.ltlErrorMessage.Text = "提示问题答案不正确。";
                        this.phFailureTemplate.Visible = true;
                        this.phUserNameTemplate.Visible = this.phQuestionTemplate.Visible = this.phSuccessTemplate.Visible = false;
                    }
                }
            }
        }

        protected string GetProductName()
        {
            return FileConfigManager.Instance.OEMConfig.ProductName;
        }

        protected string GetProductUrl()
        {
            return FileConfigManager.Instance.OEMConfig.ProductUrl;
        }        
	}
}
