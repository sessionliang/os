using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;


using BaiRong.Controls;
using System.Text;
using System.Collections.Generic;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundUserSMS : BackgroundBasePage
	{
        public RadioButtonList rblSelect;

        public PlaceHolder phGroup;
        public CheckBoxList cblGroupID;

        public PlaceHolder phUser;
        public TextBox tbUserNameList;
        public Literal ltlSelectUser;

        public PlaceHolder phPhone;
        public TextBox tbPhoneList;

        public TextBox tbContent;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_User, "短信群发", AppManager.CMS.Permission.WebSite.User);

                ListItem listItem = new ListItem("所有会员", "All");
                this.rblSelect.Items.Add(listItem);
                listItem = new ListItem("用户组", "Group");
                listItem.Selected = true;
                this.rblSelect.Items.Add(listItem);
                listItem = new ListItem("用户", "User");
                this.rblSelect.Items.Add(listItem);
                listItem = new ListItem("指定手机", "Phone");
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
            this.phGroup.Visible = this.phUser.Visible = this.phPhone.Visible = false;

            if (this.rblSelect.SelectedValue == "Group")
            {
                this.phGroup.Visible = true;
            }
            else if (this.rblSelect.SelectedValue == "User")
            {
                this.phUser.Visible = true;
            }
            else if (this.rblSelect.SelectedValue == "Phone")
            {
                this.phPhone.Visible = true;
            }
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
		{
            ArrayList mobileArrayList = new ArrayList();

            if (this.rblSelect.SelectedValue == "Group")
            {
                string groupIDCollection = ControlUtils.SelectedItemsValueToStringCollection(this.cblGroupID.Items);
                List<int> userIDList = BaiRongDataProvider.UserDAO.GetUserIDListByGroupIDCollection(groupIDCollection);

                foreach (int userID in userIDList)
                {
                    string mobile = BaiRongDataProvider.UserDAO.GetMobile(userID);
                    if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !mobileArrayList.Contains(mobile))
                    {
                        mobileArrayList.Add(mobile);
                    }
                }
            }
            else if (this.rblSelect.SelectedValue == "User")
            {
                ArrayList userNameArrayList = TranslateUtils.StringCollectionToArrayList(this.tbUserNameList.Text);

                foreach (string userName in userNameArrayList)
                {
                    int userID = BaiRongDataProvider.UserDAO.GetUserID(base.PublishmentSystemInfo.GroupSN, userName);
                    string mobile = BaiRongDataProvider.UserDAO.GetMobile(userID);
                    if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !mobileArrayList.Contains(mobile))
                    {
                        mobileArrayList.Add(mobile);
                    }
                }
            }
            else if (this.rblSelect.SelectedValue == "Phone")
            {
                ArrayList mobiles = TranslateUtils.StringCollectionToArrayList(this.tbPhoneList.Text);
                foreach (string mobile in mobiles)
                {
                    if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !mobileArrayList.Contains(mobile))
                    {
                        mobileArrayList.Add(mobile);
                    }
                }
            }
            else if (this.rblSelect.SelectedValue == "All")
            {
                List<int> userIDList = BaiRongDataProvider.UserDAO.GetUserIDList(true);
                foreach (int userID in userIDList)
                {
                    string mobile = BaiRongDataProvider.UserDAO.GetMobile(userID);
                    if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !mobileArrayList.Contains(mobile))
                    {
                        mobileArrayList.Add(mobile);
                    }
                }
            }

            if (mobileArrayList.Count > 0)
            {
                try
                {
                    string errorMessage = string.Empty;
                    bool isSend = SMSManager.Send(mobileArrayList, this.tbContent.Text, out errorMessage);
                    if (isSend)
                    {
                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "发送短信", string.Format("接收号码:{0},短信内容：{1}", TranslateUtils.ObjectCollectionToString(mobileArrayList), this.tbContent.Text));

                        base.SuccessMessage("短信发送成功！");
                    }
                    else
                    {
                        base.FailMessage(string.Format("短信发送失败：{0}！", errorMessage));
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "短信发送失败：" + ex.Message);
                }
            }
		}
	}
}
