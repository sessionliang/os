using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using SiteServer.CMS.Controls;
using System.Web.UI.HtmlControls;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundConfigurationUserCenter : BackgroundBasePage
    {
        public TextBox PublishmentSystemName;
        public AuxiliaryControl acAttributes;

        public TextBox tbPublishmentSystemUrl;
        public DropDownList ddlIsMultiDeployment;
        public PlaceHolder phIsMultiDeployment;
        public TextBox tbOuterUrl;
        public TextBox tbInnerUrl;
        public DropDownList ddlFuncFilesType;
        public PlaceHolder phCrossDomainFilesCopy;
        public PlaceHolder phFuncFilesCopy;
        public Literal ltOuterUrl;
        public Literal ltInnerUrl;


        //add by liangjian at 20150817, 设置API访问路径，方便API分离部署
        public TextBox tbAPIUrl;
        public PlaceHolder phAPIUrl;

        #region Platform
        //忘记密码
        public CheckBoxList cblPasswordFind;
        public TextBox emailNotice;
        public TextBox phoneNotice;
        public TextBox emailNoticeTitle;

        public PlaceHolder phEmail;
        public PlaceHolder phPhone;

        //登录
        public CheckBoxList cblLoginMethod;
        public RadioButtonList rblIsRecordIP;
        public RadioButtonList rblIsRecordSource;
        public RadioButtonList rblIsFailToLock;
        public DropDownList ddlLockType;
        public TextBox loginFailCount;
        public TextBox lockingTime;
        public PlaceHolder phLockingTime;
        public PlaceHolder phFailToLock;

        //注册
        public RadioButtonList IsRegisterAllowed;
        public TextBox tbRegisterUserNameMinLength;
        public DropDownList ddlRegisterPasswordRestriction;
        public TextBox ReservedUserNames;

        public DropDownList RegisterAuditType;

        public TextBox tbRegisterMinHoursOfIPAddress;

        public int currentTab;
        #endregion

        private ArrayList relatedIdentities;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.Site, base.PublishmentSystemID, base.PublishmentSystemID);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, "基本配置", AppManager.CMS.Permission.WebSite.Configration);

                PublishmentSystemName.Text = base.PublishmentSystemInfo.PublishmentSystemName;

                this.acAttributes.SetParameters(base.PublishmentSystemInfo.Additional.Attributes, base.PublishmentSystemInfo, 0, this.relatedIdentities, ETableStyle.Site, DataProvider.PublishmentSystemDAO.TableName, true, base.IsPostBack);

                #region 访问地址
                this.tbPublishmentSystemUrl.Text = base.PublishmentSystemInfo.PublishmentSystemUrl;
                EBooleanUtils.AddListItems(this.ddlIsMultiDeployment, "内外网分离部署", "同一台服务器");
                ControlUtils.SelectListItems(this.ddlIsMultiDeployment, base.PublishmentSystemInfo.Additional.IsMultiDeployment.ToString());
                this.tbOuterUrl.Text = base.PublishmentSystemInfo.Additional.OuterUrl;
                this.tbInnerUrl.Text = base.PublishmentSystemInfo.Additional.InnerUrl;

                this.tbAPIUrl.Text = base.PublishmentSystemInfo.Additional.APIUrl;

                if (base.PublishmentSystemInfo.Additional.IsMultiDeployment)
                {
                    ltOuterUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", base.PublishmentSystemInfo.Additional.OuterUrl);
                    ltInnerUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", base.PublishmentSystemInfo.Additional.InnerUrl);
                }
                else
                {
                    ltOuterUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", PageUtility.GetPublishmentSystemUrl(base.PublishmentSystemInfo, string.Empty));
                }


                if (base.PublishmentSystemInfo.IsHeadquarters)
                {
                    this.ddlFuncFilesType.Items.Add(EFuncFilesTypeUtils.GetListItem(EFuncFilesType.Direct, false));
                }
                else
                {
                    EFuncFilesTypeUtils.AddListItems(this.ddlFuncFilesType);
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlFuncFilesType, EFuncFilesTypeUtils.GetValue(base.PublishmentSystemInfo.Additional.FuncFilesType));
                }

                this.ddlIsMultiDeployment_SelectedIndexChanged(null, EventArgs.Empty);
                this.ddlFuncFilesType_SelectedIndexChanged(null, EventArgs.Empty);

                if (base.PublishmentSystemInfo.Additional.IsMultiDeployment)
                {
                    //内外网分离部署
                    this.ddlFuncFilesType.Items.Remove(new ListItem("直接访问（非跨域）", "Direct"));
                    this.phCrossDomainFilesCopy.Visible = true;

                    if (!this.ddlFuncFilesType.Items.Contains(new ListItem("通过代理访问（跨域）", "CrossDomain")))
                        this.ddlFuncFilesType.Items.Insert(0, new ListItem("通过代理访问（跨域）", "CrossDomain"));
                    if (!this.ddlFuncFilesType.Items.Contains(new ListItem("复制到站内访问（跨域）", "CopyToSite")))
                        this.ddlFuncFilesType.Items.Insert(1, new ListItem("复制到站内访问（跨域）", "CopyToSite"));
                }
                else
                {
                    this.ddlFuncFilesType.Items.Remove(new ListItem("通过代理访问（跨域）", "CrossDomain"));
                    this.ddlFuncFilesType.Items.Remove(new ListItem("复制到站内访问（跨域）", "CopyToSite"));
                    this.phCrossDomainFilesCopy.Visible = false;

                    if (!this.ddlFuncFilesType.Items.Contains(new ListItem("直接访问（非跨域）", "Direct")))
                        this.ddlFuncFilesType.Items.Insert(0, new ListItem("直接访问（非跨域）", "Direct"));
                }
                this.phCrossDomainFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CrossDomain, this.ddlFuncFilesType.SelectedValue);
                this.phFuncFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CopyToSite, this.ddlFuncFilesType.SelectedValue);
                this.phAPIUrl.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.Cors, this.ddlFuncFilesType.SelectedValue);
                #endregion

                #region 忘记密码
                cblPasswordFind.Items.Add(new ListItem("邮箱", EForgetPasswordTypeUtils.GetValue(EForgetPasswordType.Email)));
                cblPasswordFind.Items.Add(new ListItem("手机号", EForgetPasswordTypeUtils.GetValue(EForgetPasswordType.Phone)));
                cblPasswordFind.Items.Add(new ListItem("密保问题", EForgetPasswordTypeUtils.GetValue(EForgetPasswordType.SecretQuestion)));


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

                #region 登录
                cblLoginMethod.Items.Add(new ListItem("邮箱", ELoginValidateTypeUtils.GetValue(ELoginValidateType.Email)));
                cblLoginMethod.Items.Add(new ListItem("手机号", ELoginValidateTypeUtils.GetValue(ELoginValidateType.Phone)));
                cblLoginMethod.Items.Add(new ListItem("用户名", ELoginValidateTypeUtils.GetValue(ELoginValidateType.UserName)));


                string[] selectedValidateFieldItems = UserConfigManager.Instance.Additional.UserLoginValidateFields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                ControlUtils.SelectListItemsIgnoreCase(cblLoginMethod, selectedValidateFieldItems);

                EBooleanUtils.AddListItems(rblIsRecordIP, "是", "否");
                ControlUtils.SelectListItemsIgnoreCase(rblIsRecordIP, UserConfigManager.Instance.Additional.IsRecordIP.ToString());
                EBooleanUtils.AddListItems(rblIsRecordSource, "是", "否");
                ControlUtils.SelectListItemsIgnoreCase(rblIsRecordSource, UserConfigManager.Instance.Additional.IsRecordSource.ToString());
                EBooleanUtils.AddListItems(rblIsFailToLock, "是", "否");
                ControlUtils.SelectListItemsIgnoreCase(rblIsFailToLock, UserConfigManager.Instance.Additional.IsFailToLock.ToString());

                phFailToLock.Visible = false;
                if (UserConfigManager.Instance.Additional.IsFailToLock)
                {
                    phFailToLock.Visible = true;
                }

                loginFailCount.Text = UserConfigManager.Instance.Additional.LoginFailCount.ToString();

                ddlLockType.Items.Add(new ListItem("按天数锁定", EUserLockTypeUtils.GetValue(EUserLockType.Day)));
                ddlLockType.Items.Add(new ListItem("永久锁定", EUserLockTypeUtils.GetValue(EUserLockType.Forever)));
                ControlUtils.SelectListItemsIgnoreCase(ddlLockType, UserConfigManager.Instance.Additional.LockingType);

                phLockingTime.Visible = false;
                if (!EUserLockTypeUtils.Equals(UserConfigManager.Instance.Additional.LockingType, EUserLockType.Forever))
                {
                    phLockingTime.Visible = true;
                    lockingTime.Text = UserConfigManager.Instance.Additional.LockingTime.ToString();
                }
                #endregion

                #region 注册
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

                EBooleanUtils.AddListItems(this.RegisterAuditType, "开启", "关闭");
                ControlUtils.SelectListItemsIgnoreCase(this.RegisterAuditType, UserConfigManager.Additional.RegisterAuditType.ToString());
                #endregion
            }
            else
            {
                this.acAttributes.SetParameters(base.Request.Form, base.PublishmentSystemInfo, 0, this.relatedIdentities, ETableStyle.Site, DataProvider.PublishmentSystemDAO.TableName, true, base.IsPostBack);
            }
        }

        #region 忘记密码

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

        #region 登录

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

        #region 注册
        public void RegisterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.phVerifyMailContent.Visible = EUserVerifyTypeUtils.Equals(EUserVerifyType.Email, this.RegisterVerifyType.SelectedValue);
            //this.phRegisterWelcome.Visible = !EUserWelcomeTypeUtils.Equals(EUserWelcomeType.None, this.ddlRegisterWelcomeType.SelectedValue);
        }
        #endregion

        public string GetChangeTabFunction()
        {
            if (!string.IsNullOrEmpty(base.Request.Form["index"]))
            {
                return string.Format(@"changeTab({0});", base.Request.Form["index"]);
            }
            return string.Empty;
        }

        public void ddlIsMultiDeployment_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phIsMultiDeployment.Visible = TranslateUtils.ToBool(this.ddlIsMultiDeployment.SelectedValue);
            if (base.IsPostBack)
            {
                if (TranslateUtils.ToBool(ddlIsMultiDeployment.SelectedValue))
                {
                    //内外网分离部署
                    this.ddlFuncFilesType.Items.Remove(new ListItem("直接访问（非跨域）", "Direct"));

                    if (!this.ddlFuncFilesType.Items.Contains(new ListItem("通过代理访问（跨域）", "CrossDomain")))
                        this.ddlFuncFilesType.Items.Insert(0, new ListItem("通过代理访问（跨域）", "CrossDomain"));
                    if (!this.ddlFuncFilesType.Items.Contains(new ListItem("复制到站内访问（跨域）", "CopyToSite")))
                        this.ddlFuncFilesType.Items.Insert(1, new ListItem("复制到站内访问（跨域）", "CopyToSite"));
                }
                else
                {
                    this.ddlFuncFilesType.Items.Remove(new ListItem("通过代理访问（跨域）", "CrossDomain"));
                    this.ddlFuncFilesType.Items.Remove(new ListItem("复制到站内访问（跨域）", "CopyToSite"));

                    if (!this.ddlFuncFilesType.Items.Contains(new ListItem("直接访问（非跨域）", "Direct")))
                        this.ddlFuncFilesType.Items.Insert(0, new ListItem("直接访问（非跨域）", "Direct"));
                }
                this.phCrossDomainFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CrossDomain, this.ddlFuncFilesType.SelectedValue);
                this.phFuncFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CopyToSite, this.ddlFuncFilesType.SelectedValue);
                this.phAPIUrl.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.Cors, this.ddlFuncFilesType.SelectedValue);
            }
        }

        public void ddlFuncFilesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phCrossDomainFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CrossDomain, this.ddlFuncFilesType.SelectedValue);
            this.phFuncFilesCopy.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.CopyToSite, this.ddlFuncFilesType.SelectedValue);
            this.phAPIUrl.Visible = EFuncFilesTypeUtils.Equals(EFuncFilesType.Cors, this.ddlFuncFilesType.SelectedValue);
        }

        public void btnCopyCrossDomainFiles_OnClick(object sender, EventArgs E)
        {
            FileUtility.CopyCrossDomainFilesToSite(base.PublishmentSystemInfo);
            base.SuccessMessage("跨域代理页复制成功！");
        }

        public void btnCopyFuncFiles_OnClick(object sender, EventArgs E)
        {
            FileUtility.CopyFuncFilesToSite(base.PublishmentSystemInfo);
            base.SuccessMessage("功能页复制成功！");
        }

        public string GetSiteName()
        {
            return base.PublishmentSystemInfo.PublishmentSystemName;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                base.PublishmentSystemInfo.PublishmentSystemName = PublishmentSystemName.Text;

                #region 访问设置
                base.PublishmentSystemInfo.PublishmentSystemUrl = this.tbPublishmentSystemUrl.Text;
                base.PublishmentSystemInfo.Additional.IsMultiDeployment = TranslateUtils.ToBool(this.ddlIsMultiDeployment.SelectedValue);
                base.PublishmentSystemInfo.Additional.OuterUrl = this.tbOuterUrl.Text;
                base.PublishmentSystemInfo.Additional.InnerUrl = this.tbInnerUrl.Text;
                base.PublishmentSystemInfo.Additional.FuncFilesType = EFuncFilesTypeUtils.GetEnumType(this.ddlFuncFilesType.SelectedValue);
                base.PublishmentSystemInfo.Additional.APIUrl = this.tbAPIUrl.Text;
                #endregion


                #region 忘记密码
                string[] selectedMethods = ControlUtils.GetSelectedListControlValueArray(cblPasswordFind);
                if (selectedMethods.Length == 0)
                {
                    base.FailMessage("至少选择一项找回密码方式。");
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

                #region 登录
                string[] selectedLoginMethods = ControlUtils.GetSelectedListControlValueArray(cblLoginMethod);
                if (selectedLoginMethods.Length == 0)
                {
                    base.FailMessage("至少选择一项登录方式。");
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

                #region 注册
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

                try
                {

                    InputTypeParser.AddValuesToAttributes(ETableStyle.Site, DataProvider.PublishmentSystemDAO.TableName, base.PublishmentSystemInfo, this.relatedIdentities, this.Page.Request.Form, base.PublishmentSystemInfo.Additional.Attributes);

                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                    PublishmentSystemManager.UpdateUrlRewriteFile();


                    BaiRongDataProvider.UserConfigDAO.Update(UserConfigManager.Instance);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改设置");

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
