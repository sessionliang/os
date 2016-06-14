using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using BaiRong.Controls;
using System.Text;

namespace BaiRong.BackgroundPages
{
    public class BackgroundUserMessage : BackgroundBasePage
    {
        public RadioButtonList rblSelect;


        public CheckBoxList cblTypeID;

        public PlaceHolder phUser;
        public TextBox tbUserNameList;
        public Literal ltlSelectUser;

        public BREditor breContent;
        public TextBox tbTitle;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserMessage, "系统消息", AppManager.User.Permission.Usercenter_Msg);

                ListItem listItem = new ListItem("所有会员", "All");
                listItem.Selected = true;
                this.rblSelect.Items.Add(listItem);
                listItem = new ListItem("用户", "User");
                this.rblSelect.Items.Add(listItem);

                this.ltlSelectUser.Text = string.Format(@"&nbsp;<a href=""javascript:;"" onclick=""{0}"" style=""vertical-align:bottom"">选择</a>", Modal.UserSelect.GetOpenWindowString(this.tbUserNameList.ClientID));
            }
        }

        public void rblSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.rblSelect.SelectedValue == "User")
            {
                this.phUser.Visible = true;
            }
            else if (this.rblSelect.SelectedValue == "All")
            {
                this.phUser.Visible = false;
            }
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            ArrayList userNameArrayList = new ArrayList();

            if (this.rblSelect.SelectedValue == "User")
            {
                userNameArrayList = TranslateUtils.StringCollectionToArrayList(this.tbUserNameList.Text);
            }
            else if (this.rblSelect.SelectedValue == "All")
            {
                userNameArrayList = BaiRongDataProvider.UserDAO.GetUserNameArrayList(true);
            }

            if (userNameArrayList.Count > 0)
            {
                try
                {
                    string messageForm = AdminManager.DisplayName;
                    DateTime addDate = DateTime.Now;
                    string content = this.breContent.Text;
                    string title = this.tbTitle.Text;
                    foreach (string userName in userNameArrayList)
                    {
                        UserMessageInfo messageInfo = new UserMessageInfo(0, messageForm, userName, EUserMessageType.System, 0, false, addDate, content, addDate, content, title);
                        BaiRongDataProvider.UserMessageDAO.Insert(messageInfo);
                    }

                    base.SuccessMessage(string.Format("系统通知发送成功，共向{0}名用户发送系统通知！", userNameArrayList.Count));
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "系统通知发送失败：" + ex.Message);
                }
            }
        }
    }
}