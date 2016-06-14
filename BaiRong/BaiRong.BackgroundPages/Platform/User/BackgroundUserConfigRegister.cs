using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;



namespace BaiRong.BackgroundPages
{
    public class BackgroundUserConfigRegister : BackgroundBasePage
    {
        public RadioButtonList IsRegisterAllowed;
        public TextBox tbRegisterUserNameMinLength;
        public DropDownList ddlRegisterPasswordRestriction;
        public RadioButtonList IsEmailDuplicated;
        public TextBox ReservedUserNames;

        public DropDownList RegisterVerifyType;
        public TextBox RegisterWelcome;
        public PlaceHolder phVerifyMailContent;
        public TextBox RegisterVerifyMailContent;

        public TextBox tbRegisterMinHoursOfIPAddress;
        public DropDownList ddlRegisterWelcomeType;
        public PlaceHolder phRegisterWelcome;
        public TextBox tbRegisterWelcomeTitle;
        public TextBox tbRegisterWelcomeContent;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserBasicSetting, "用户注册配置", AppManager.User.Permission.Usercenter_Setting);

                EBooleanUtils.AddListItems(this.IsRegisterAllowed);

                this.tbRegisterUserNameMinLength.Text = UserConfigManager.Additional.RegisterUserNameMinLength.ToString();

                EUserPasswordRestrictionUtils.AddListItems(this.ddlRegisterPasswordRestriction);
                ControlUtils.SelectListItemsIgnoreCase(this.ddlRegisterPasswordRestriction, EUserPasswordRestrictionUtils.GetValue(UserConfigManager.Additional.RegisterPasswordRestriction));

                EBooleanUtils.AddListItems(this.IsEmailDuplicated);
                EUserVerifyTypeUtils.AddListItems(this.RegisterVerifyType);

                ControlUtils.SelectListItemsIgnoreCase(this.IsRegisterAllowed, UserConfigManager.Additional.IsRegisterAllowed.ToString());
                ControlUtils.SelectListItemsIgnoreCase(this.IsEmailDuplicated, UserConfigManager.Additional.IsEmailDuplicated.ToString());
                ControlUtils.SelectListItemsIgnoreCase(this.RegisterVerifyType, EUserVerifyTypeUtils.GetValue(UserConfigManager.Additional.RegisterVerifyType));
                this.ReservedUserNames.Text = UserConfigManager.Additional.ReservedUserNames;
                this.RegisterWelcome.Text = UserConfigManager.Additional.RegisterWelcome;
                this.RegisterVerifyMailContent.Text = UserConfigManager.Additional.RegisterVerifyMailContent;

                this.tbRegisterMinHoursOfIPAddress.Text = UserConfigManager.Additional.RegisterMinHoursOfIPAddress.ToString();
                EUserWelcomeTypeUtils.AddListItems(this.ddlRegisterWelcomeType);
                ControlUtils.SelectListItemsIgnoreCase(this.ddlRegisterWelcomeType, EUserWelcomeTypeUtils.GetValue(UserConfigManager.Additional.RegisterWelcomeType));
                this.tbRegisterWelcomeTitle.Text = UserConfigManager.Additional.RegisterWelcomeTitle;
                this.tbRegisterWelcomeContent.Text = UserConfigManager.Additional.RegisterWelcomeContent;

                this.RegisterType_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        public void RegisterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phVerifyMailContent.Visible = EUserVerifyTypeUtils.Equals(EUserVerifyType.Email, this.RegisterVerifyType.SelectedValue);
            this.phRegisterWelcome.Visible = !EUserWelcomeTypeUtils.Equals(EUserWelcomeType.None, this.ddlRegisterWelcomeType.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                UserConfigManager.Additional.IsRegisterAllowed = TranslateUtils.ToBool(this.IsRegisterAllowed.SelectedValue);

                UserConfigManager.Additional.RegisterUserNameMinLength = TranslateUtils.ToInt(this.tbRegisterUserNameMinLength.Text);
                UserConfigManager.Additional.RegisterPasswordRestriction = EUserPasswordRestrictionUtils.GetEnumType(this.ddlRegisterPasswordRestriction.SelectedValue);

                UserConfigManager.Additional.IsEmailDuplicated = TranslateUtils.ToBool(this.IsEmailDuplicated.SelectedValue);
                UserConfigManager.Additional.RegisterVerifyType = EUserVerifyTypeUtils.GetEnumType(this.RegisterVerifyType.SelectedValue);
                UserConfigManager.Additional.ReservedUserNames = this.ReservedUserNames.Text;
                UserConfigManager.Additional.RegisterWelcome = this.RegisterWelcome.Text;
                UserConfigManager.Additional.RegisterVerifyMailContent = this.RegisterVerifyMailContent.Text;

                UserConfigManager.Additional.RegisterMinHoursOfIPAddress = TranslateUtils.ToInt(this.tbRegisterMinHoursOfIPAddress.Text);
                UserConfigManager.Additional.RegisterWelcomeType = EUserWelcomeTypeUtils.GetEnumType(this.ddlRegisterWelcomeType.SelectedValue);
                UserConfigManager.Additional.RegisterWelcomeTitle = this.tbRegisterWelcomeTitle.Text;
                UserConfigManager.Additional.RegisterWelcomeContent = this.tbRegisterWelcomeContent.Text;

                try
                {
                    BaiRongDataProvider.UserConfigDAO.Update(UserConfigManager.Instance);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改用户注册设置");

                    base.SuccessMessage("设置修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "设置修改失败！");
                }
            }
        }
    }
}
