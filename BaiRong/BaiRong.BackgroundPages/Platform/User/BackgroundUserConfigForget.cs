using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using System.Collections.Generic;



namespace BaiRong.BackgroundPages
{
    public class BackgroundUserConfigForget : BackgroundBasePage
    {
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

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserBasicSetting, "忘记密码配置", AppManager.User.Permission.Usercenter_Setting);

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


            }
        }



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
        //    }
        //}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {


                try
                {


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

                    BaiRongDataProvider.UserConfigDAO.Update(UserConfigManager.Instance);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改忘记密码设置");

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
