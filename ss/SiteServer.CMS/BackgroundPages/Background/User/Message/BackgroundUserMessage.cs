using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;


using BaiRong.Controls;
using System.Text;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundUserMessage : BackgroundBasePage
    {
        public RadioButtonList rblSelect;

        public PlaceHolder phGroup;
        public CheckBoxList cblGroupID;

        public PlaceHolder phUser;
        public TextBox tbUserNameList;
        public Literal ltlSelectUser;

        public BREditor breContent;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_User, "短消息群发", AppManager.CMS.Permission.WebSite.User);

                ListItem listItem = new ListItem("所有会员", "All");
                this.rblSelect.Items.Add(listItem);
                listItem = new ListItem("用户组", "Group");
                listItem.Selected = true;
                this.rblSelect.Items.Add(listItem);
                listItem = new ListItem("用户", "User");
                this.rblSelect.Items.Add(listItem);

                foreach (int groupID in BaiRongDataProvider.UserGroupDAO.GetGroupIDList(base.PublishmentSystemInfo.GroupSN))
                {
                    string groupName = UserGroupManager.GetGroupName(base.PublishmentSystemInfo.GroupSN, groupID);
                    this.cblGroupID.Items.Add(new ListItem(groupName, groupID.ToString()));
                }

                this.ltlSelectUser.Text = string.Format(@"&nbsp;<a href=""javascript:;"" onclick=""{0}"" style=""vertical-align:bottom"">选择</a>", Modal.UserSelect.GetOpenWindowString(base.PublishmentSystemID, this.tbUserNameList.ClientID));
            }
        }

        public void rblSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phGroup.Visible = this.phUser.Visible = false;

            if (this.rblSelect.SelectedValue == "Group")
            {
                this.phGroup.Visible = true;
            }
            else if (this.rblSelect.SelectedValue == "User")
            {
                this.phUser.Visible = true;
            }
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            ArrayList userNameArrayList = new ArrayList();

            if (this.rblSelect.SelectedValue == "Group")
            {
                string groupIDCollection = ControlUtils.SelectedItemsValueToStringCollection(this.cblGroupID.Items);
                userNameArrayList = BaiRongDataProvider.UserDAO.GetUserNameArrayListByGroupIDCollection(groupIDCollection);
            }
            else if (this.rblSelect.SelectedValue == "User")
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
                    foreach (string userName in userNameArrayList)
                    {
                        UserMessageInfo messageInfo = new UserMessageInfo(0, messageForm, userName, EUserMessageType.System, 0, false, addDate, content, addDate, content);
                        BaiRongDataProvider.UserMessageDAO.Insert(messageInfo);
                    }

                    base.SuccessMessage(string.Format("短消息发送成功，共向{0}名用户发送短消息！", userNameArrayList.Count));
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "短消息发送失败：" + ex.Message);
                }
            }
        }
    }
}