using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using System.Collections.Generic;



namespace BaiRong.BackgroundPages
{
    public class BackgroundUserSettings : BackgroundBasePage
    {
        //��������
        public CheckBoxList cblPasswordFind;
        public TextBox emailNotice;
        public TextBox phoneNotice;
        public TextBox emailNoticeTitle;
        //public DropDownList ddlIsSendMsg;
        //public TextBox messageTitle;
        //public TextBox messageContent;

        public PlaceHolder phEmail;
        public PlaceHolder phPhone;
        //public PlaceHolder phMessage;

        //��¼
        public CheckBoxList cblLoginMethod;
        public RadioButtonList rblIsRecordIP;
        public RadioButtonList rblIsRecordSource;
        public RadioButtonList rblIsFailToLock;
        public DropDownList ddlLockType;
        public TextBox loginFailCount;
        public TextBox lockingTime;
        public PlaceHolder phLockingTime;
        public PlaceHolder phFailToLock;

        //ע��
        public RadioButtonList IsRegisterAllowed;
        public TextBox tbRegisterUserNameMinLength;
        public DropDownList ddlRegisterPasswordRestriction;
        //public RadioButtonList IsEmailDuplicated;
        public TextBox ReservedUserNames;

        public DropDownList RegisterAuditType;
        //public DropDownList RegisterVerifyType;
        //public TextBox RegisterWelcome;
        //public PlaceHolder phVerifyMailContent;
        //public TextBox RegisterVerifyMailContent;

        public TextBox tbRegisterMinHoursOfIPAddress;
        //public DropDownList ddlRegisterWelcomeType;
        //public PlaceHolder phRegisterWelcome;
        //public TextBox tbRegisterWelcomeTitle;
        //public TextBox tbRegisterWelcomeContent;

        public int currentTab;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            currentTab = TranslateUtils.ToInt(base.Request.Form["hidCurrentTab"], 1);
            base.AddScript(string.Format("_toggleTab({0},3);setCurrentTab({0});", currentTab));
            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserBasicSetting, "�û�����", AppManager.User.Permission.Usercenter_Setting);

                #region ��������
                cblPasswordFind.Items.Add(new ListItem("����", EForgetPasswordTypeUtils.GetValue(EForgetPasswordType.Email)));
                cblPasswordFind.Items.Add(new ListItem("�ֻ���", EForgetPasswordTypeUtils.GetValue(EForgetPasswordType.Phone)));
                cblPasswordFind.Items.Add(new ListItem("�ܱ�����", EForgetPasswordTypeUtils.GetValue(EForgetPasswordType.SecretQuestion)));


                string[] selectedItems = UserConfigManager.Instance.Additional.FindPasswordMethods.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                ControlUtils.SelectListItemsIgnoreCase(cblPasswordFind, selectedItems);

                phEmail.Visible = false;
                if (Array.IndexOf(selectedItems, EForgetPasswordTypeUtils.GetValue(EForgetPasswordType.Email)) >= 0)
                {
                    phEmail.Visible = true;
                }

                phPhone.Visible = false;
                if (Array.IndexOf(selectedItems, EForgetPasswordTypeUtils.GetValue(EForgetPasswordType.Phone)) >= 0)
                {
                    phPhone.Visible = true;
                }

                //phMessage.Visible = false;
                //if (!EForgetPasswordNoticeTypeUtils.Equals(UserConfigManager.Instance.Additional.IsSendFindPasswordMsg, EForgetPasswordNoticeType.None))
                //{
                //    phMessage.Visible = true;
                //}

                //EForgetPasswordNoticeTypeUtils.AddListItems(ddlIsSendMsg);
                //ControlUtils.SelectListItemsIgnoreCase(ddlIsSendMsg, UserConfigManager.Instance.Additional.IsSendFindPasswordMsg.ToString());

                emailNotice.Text = UserConfigManager.Instance.Additional.FindPasswordEmailNotice.ToString();
                emailNoticeTitle.Text = UserConfigManager.Instance.Additional.FindPasswordEmailNoticeTitle.ToString();
                phoneNotice.Text = UserConfigManager.Instance.Additional.FindPasswordPhoneNotice.ToString();
                //messageTitle.Text = UserConfigManager.Instance.Additional.FindPasswordMessageTitle.ToString();
                //messageContent.Text = UserConfigManager.Instance.Additional.FindPasswordMessageContent.ToString(); 
                #endregion

                #region ��¼
                cblLoginMethod.Items.Add(new ListItem("����", ELoginValidateTypeUtils.GetValue(ELoginValidateType.Email)));
                cblLoginMethod.Items.Add(new ListItem("�ֻ���", ELoginValidateTypeUtils.GetValue(ELoginValidateType.Phone)));
                cblLoginMethod.Items.Add(new ListItem("�û���", ELoginValidateTypeUtils.GetValue(ELoginValidateType.UserName)));


                string[] selectedValidateFieldItems = UserConfigManager.Instance.Additional.UserLoginValidateFields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                ControlUtils.SelectListItemsIgnoreCase(cblLoginMethod, selectedValidateFieldItems);

                EBooleanUtils.AddListItems(rblIsRecordIP, "��", "��");
                ControlUtils.SelectListItemsIgnoreCase(rblIsRecordIP, UserConfigManager.Instance.Additional.IsRecordIP.ToString());
                EBooleanUtils.AddListItems(rblIsRecordSource, "��", "��");
                ControlUtils.SelectListItemsIgnoreCase(rblIsRecordSource, UserConfigManager.Instance.Additional.IsRecordSource.ToString());
                EBooleanUtils.AddListItems(rblIsFailToLock, "��", "��");
                ControlUtils.SelectListItemsIgnoreCase(rblIsFailToLock, UserConfigManager.Instance.Additional.IsFailToLock.ToString());

                phFailToLock.Visible = false;
                if (UserConfigManager.Instance.Additional.IsFailToLock)
                {
                    phFailToLock.Visible = true;
                }

                loginFailCount.Text = UserConfigManager.Instance.Additional.LoginFailCount.ToString();

                ddlLockType.Items.Add(new ListItem("����������", EUserLockTypeUtils.GetValue(EUserLockType.Day)));
                ddlLockType.Items.Add(new ListItem("��������", EUserLockTypeUtils.GetValue(EUserLockType.Forever)));
                ControlUtils.SelectListItemsIgnoreCase(ddlLockType, UserConfigManager.Instance.Additional.LockingType);

                phLockingTime.Visible = false;
                if (!EUserLockTypeUtils.Equals(UserConfigManager.Instance.Additional.LockingType, EUserLockType.Forever))
                {
                    phLockingTime.Visible = true;
                    lockingTime.Text = UserConfigManager.Instance.Additional.LockingTime.ToString();
                }
                #endregion

                #region ע��
                EBooleanUtils.AddListItems(this.IsRegisterAllowed);

                this.tbRegisterUserNameMinLength.Text = UserConfigManager.Additional.RegisterUserNameMinLength.ToString();

                EUserPasswordRestrictionUtils.AddListItems(this.ddlRegisterPasswordRestriction);
                ControlUtils.SelectListItemsIgnoreCase(this.ddlRegisterPasswordRestriction, EUserPasswordRestrictionUtils.GetValue(UserConfigManager.Additional.RegisterPasswordRestriction));

                //EBooleanUtils.AddListItems(this.IsEmailDuplicated);

                //EUserVerifyTypeUtils.AddListItems(this.RegisterVerifyType);

                ControlUtils.SelectListItemsIgnoreCase(this.IsRegisterAllowed, UserConfigManager.Additional.IsRegisterAllowed.ToString());
                //ControlUtils.SelectListItemsIgnoreCase(this.IsEmailDuplicated, UserConfigManager.Additional.IsEmailDuplicated.ToString());
                //ControlUtils.SelectListItemsIgnoreCase(this.RegisterVerifyType, EUserVerifyTypeUtils.GetValue(UserConfigManager.Additional.RegisterVerifyType));
                this.ReservedUserNames.Text = UserConfigManager.Additional.ReservedUserNames;
                //this.RegisterWelcome.Text = UserConfigManager.Additional.RegisterWelcome;
                //this.RegisterVerifyMailContent.Text = UserConfigManager.Additional.RegisterVerifyMailContent;

                this.tbRegisterMinHoursOfIPAddress.Text = UserConfigManager.Additional.RegisterMinHoursOfIPAddress.ToString();
                //EUserWelcomeTypeUtils.AddListItems(this.ddlRegisterWelcomeType);
                //ControlUtils.SelectListItemsIgnoreCase(this.ddlRegisterWelcomeType, EUserWelcomeTypeUtils.GetValue(UserConfigManager.Additional.RegisterWelcomeType));
                //this.tbRegisterWelcomeTitle.Text = UserConfigManager.Additional.RegisterWelcomeTitle;
                //this.tbRegisterWelcomeContent.Text = UserConfigManager.Additional.RegisterWelcomeContent;

                this.RegisterType_SelectedIndexChanged(null, EventArgs.Empty);

                EBooleanUtils.AddListItems(this.RegisterAuditType, "����", "�ر�");
                ControlUtils.SelectListItemsIgnoreCase(this.RegisterAuditType, UserConfigManager.Additional.RegisterAuditType.ToString());
                #endregion


            }
        }


        #region ��������

        protected void cblPasswordFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] selectedMethods = ControlUtils.GetSelectedListControlValueArray(cblPasswordFind);
            if (Array.IndexOf(selectedMethods, EForgetPasswordTypeUtils.GetValue(EForgetPasswordType.Email)) >= 0)
            {
                phEmail.Visible = true;
            }
            else
            {
                phEmail.Visible = false;
            }
            if (Array.IndexOf(selectedMethods, EForgetPasswordTypeUtils.GetValue(EForgetPasswordType.Phone)) >= 0)
            {
                phPhone.Visible = true;
            }
            else
            {
                phPhone.Visible = false;
            }
        }

        //protected void ddlIsSendMsg_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string[] selectedMethods = ControlUtils.GetSelectedListControlValueArray(ddlIsSendMsg);
        //    if (EForgetPasswordNoticeTypeUtils.Equals(EForgetPasswordNoticeType.None, ddlIsSendMsg.SelectedValue))
        //    {
        //        phMessage.Visible = false;
        //    }
        //    else
        //    {
        //        phMessage.Visible = true;
        //    
        //    }
        //}
        #endregion

        #region ��¼

        protected void rblIsFailToLock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TranslateUtils.ToBool(rblIsFailToLock.SelectedValue))
            {
                phFailToLock.Visible = true;
            }
            else
            {
                phFailToLock.Visible = false;
            }
        }

        protected void ddlLockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!EUserLockTypeUtils.Equals(EUserLockType.Forever, ddlLockType.SelectedValue))
            {
                phLockingTime.Visible = true;
            }
            else
            {
                phLockingTime.Visible = false;
            }
        }

        #endregion

        #region ע��
        public void RegisterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.phVerifyMailContent.Visible = EUserVerifyTypeUtils.Equals(EUserVerifyType.Email, this.RegisterVerifyType.SelectedValue);
            //this.phRegisterWelcome.Visible = !EUserWelcomeTypeUtils.Equals(EUserWelcomeType.None, this.ddlRegisterWelcomeType.SelectedValue);
        }
        #endregion

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {


                try
                {


                    #region ��������
                    string[] selectedMethods = ControlUtils.GetSelectedListControlValueArray(cblPasswordFind);
                    if (selectedMethods.Length == 0)
                    {
                        base.FailMessage("����ѡ��һ���һ����뷽ʽ��");
                        return;
                    }

                    UserConfigManager.Instance.Additional.FindPasswordMethods = TranslateUtils.ObjectCollectionToString(selectedMethods);

                    //UserConfigManager.Instance.Additional.IsSendFindPasswordMsg = ddlIsSendMsg.SelectedValue;

                    UserConfigManager.Instance.Additional.FindPasswordEmailNotice = emailNotice.Text;

                    UserConfigManager.Instance.Additional.FindPasswordEmailNoticeTitle = emailNoticeTitle.Text;

                    UserConfigManager.Instance.Additional.FindPasswordPhoneNotice = phoneNotice.Text;

                    //UserConfigManager.Instance.Additional.FindPasswordMessageTitle = messageTitle.Text;

                    //UserConfigManager.Instance.Additional.FindPasswordMessageContent = messageContent.Text; 
                    #endregion

                    #region ��¼
                    string[] selectedLoginMethods = ControlUtils.GetSelectedListControlValueArray(cblLoginMethod);
                    if (selectedLoginMethods.Length == 0)
                    {
                        base.FailMessage("����ѡ��һ���¼��ʽ��");
                        return;
                    }

                    UserConfigManager.Instance.Additional.UserLoginValidateFields = TranslateUtils.ObjectCollectionToString(selectedLoginMethods);

                    UserConfigManager.Instance.Additional.IsRecordIP = TranslateUtils.ToBool(rblIsRecordIP.SelectedValue);
                    UserConfigManager.Instance.Additional.IsRecordSource = TranslateUtils.ToBool(rblIsRecordSource.SelectedValue);
                    UserConfigManager.Instance.Additional.IsFailToLock = TranslateUtils.ToBool(rblIsFailToLock.SelectedValue);

                    UserConfigManager.Instance.Additional.LoginFailCount = TranslateUtils.ToInt(loginFailCount.Text, 3);

                    UserConfigManager.Instance.Additional.LockingType = ddlLockType.SelectedValue;

                    UserConfigManager.Instance.Additional.LockingTime = TranslateUtils.ToInt(lockingTime.Text);
                    #endregion

                    #region ע��
                    UserConfigManager.Additional.IsRegisterAllowed = TranslateUtils.ToBool(this.IsRegisterAllowed.SelectedValue);

                    UserConfigManager.Additional.RegisterUserNameMinLength = TranslateUtils.ToInt(this.tbRegisterUserNameMinLength.Text);
                    UserConfigManager.Additional.RegisterPasswordRestriction = EUserPasswordRestrictionUtils.GetEnumType(this.ddlRegisterPasswordRestriction.SelectedValue);

                    //UserConfigManager.Additional.IsEmailDuplicated = TranslateUtils.ToBool(this.IsEmailDuplicated.SelectedValue);
                    UserConfigManager.Additional.IsEmailDuplicated = false;

                    //UserConfigManager.Additional.RegisterVerifyType = EUserVerifyTypeUtils.GetEnumType(this.RegisterVerifyType.SelectedValue);
                    UserConfigManager.Additional.RegisterAuditType = TranslateUtils.ToBool(this.RegisterAuditType.SelectedValue);
                    UserConfigManager.Additional.ReservedUserNames = this.ReservedUserNames.Text;
                    //UserConfigManager.Additional.RegisterWelcome = this.RegisterWelcome.Text;
                    //UserConfigManager.Additional.RegisterVerifyMailContent = this.RegisterVerifyMailContent.Text;

                    UserConfigManager.Additional.RegisterMinHoursOfIPAddress = TranslateUtils.ToInt(this.tbRegisterMinHoursOfIPAddress.Text);
                    //UserConfigManager.Additional.RegisterWelcomeType = EUserWelcomeTypeUtils.GetEnumType(this.ddlRegisterWelcomeType.SelectedValue);
                    //UserConfigManager.Additional.RegisterWelcomeTitle = this.tbRegisterWelcomeTitle.Text;
                    //UserConfigManager.Additional.RegisterWelcomeContent = this.tbRegisterWelcomeContent.Text;
                    #endregion

                    BaiRongDataProvider.UserConfigDAO.Update(UserConfigManager.Instance);
                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "�޸�������������");

                    base.SuccessMessage("�����޸ĳɹ���");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "�����޸�ʧ�ܣ�");
                }
            }
        }
    }
}
