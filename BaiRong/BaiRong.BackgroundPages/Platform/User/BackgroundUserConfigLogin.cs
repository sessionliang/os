using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using System.Collections.Generic;



namespace BaiRong.BackgroundPages
{
    public class BackgroundUserConfigLogin : BackgroundBasePage
    {
        public CheckBoxList cblLoginMethod;
        public RadioButtonList rblIsRecordIP;
        public RadioButtonList rblIsRecordSource;
        public RadioButtonList rblIsFailToLock;
        public DropDownList ddlLockType;
        public TextBox loginFailCount;
        public TextBox lockingTime;
        public PlaceHolder phLockingTime;
        public PlaceHolder phFailToLock;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserBasicSetting, "�û���¼����", AppManager.User.Permission.Usercenter_Setting);

                cblLoginMethod.Items.Add(new ListItem("����", ELoginValidateTypeUtils.GetValue(ELoginValidateType.Email)));
                cblLoginMethod.Items.Add(new ListItem("�ֻ���", ELoginValidateTypeUtils.GetValue(ELoginValidateType.Phone)));
                cblLoginMethod.Items.Add(new ListItem("�û���", ELoginValidateTypeUtils.GetValue(ELoginValidateType.UserName)));


                string[] selectedItems = UserConfigManager.Instance.Additional.UserLoginValidateFields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                ControlUtils.SelectListItemsIgnoreCase(cblLoginMethod, selectedItems);

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

            }
        }

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

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {


                try
                {


                    string[] selectedMethods = ControlUtils.GetSelectedListControlValueArray(cblLoginMethod);
                    if (selectedMethods.Length == 0)
                    { 
                        base.FailMessage("����ѡ��һ���¼��ʽ��");
                        return;
                    }

                    UserConfigManager.Instance.Additional.UserLoginValidateFields = TranslateUtils.ObjectCollectionToString(selectedMethods);

                    UserConfigManager.Instance.Additional.IsRecordIP = TranslateUtils.ToBool(rblIsRecordIP.SelectedValue);
                    UserConfigManager.Instance.Additional.IsRecordSource = TranslateUtils.ToBool(rblIsRecordSource.SelectedValue);
                    UserConfigManager.Instance.Additional.IsFailToLock = TranslateUtils.ToBool(rblIsFailToLock.SelectedValue);

                    UserConfigManager.Instance.Additional.LoginFailCount = TranslateUtils.ToInt(loginFailCount.Text, 3);

                    UserConfigManager.Instance.Additional.LockingType = ddlLockType.SelectedValue;

                    UserConfigManager.Instance.Additional.LockingTime = TranslateUtils.ToInt(lockingTime.Text);

                    BaiRongDataProvider.UserConfigDAO.Update(UserConfigManager.Instance);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "�޸��û���¼����");

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
