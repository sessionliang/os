using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using BaiRong.Controls;

namespace BaiRong.BackgroundPages
{
    public class BackgroundUserConfigMessage : BackgroundBasePage
    {
        public RadioButtonList rblIsMessage;
        public PlaceHolder phMessage;
        public TextBox tbMessageTitle;
        public BREditor breMessageContent;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserMessage, "系统通知", AppManager.User.Permission.Usercenter_Msg);

                EBooleanUtils.AddListItems(this.rblIsMessage);
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsMessage, UserConfigManager.Instance.Additional.IsMessage.ToString());
                this.tbMessageTitle.Text = UserConfigManager.Instance.Additional.MessageTitle;
                this.breMessageContent.Text = UserConfigManager.Instance.Additional.MessageContent;

                this.rblIsMessage_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        public void rblIsMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phMessage.Visible = TranslateUtils.ToBool(this.rblIsMessage.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                UserConfigManager.Additional.IsMessage = TranslateUtils.ToBool(this.rblIsMessage.SelectedValue);
                UserConfigManager.Additional.MessageTitle = this.tbMessageTitle.Text;
                UserConfigManager.Additional.MessageContent = this.breMessageContent.Text;

                try
                {
                    BaiRongDataProvider.UserConfigDAO.Update(UserConfigManager.Instance);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改通知设置");

                    base.SuccessMessage("通知设置修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "通知设置修改失败！");
                }
            }
        }
    }
}
