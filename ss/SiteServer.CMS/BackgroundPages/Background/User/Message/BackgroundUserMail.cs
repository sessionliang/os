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
    public class BackgroundUserMail : BackgroundBasePage
	{
        public RadioButtonList rblSelect;

        public PlaceHolder phGroup;
        public CheckBoxList cblGroupID;

        public PlaceHolder phUser;
        public TextBox tbUserNameList;
        public Literal ltlSelectUser;

        public PlaceHolder phMail;
        public TextBox tbMailList;

        public TextBox tbTitle;
        public BREditor breContent;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_User, "邮件群发", AppManager.CMS.Permission.WebSite.User);

                ListItem listItem = new ListItem("所有会员", "All");
                this.rblSelect.Items.Add(listItem);
                listItem = new ListItem("用户组", "Group");
                listItem.Selected = true;
                this.rblSelect.Items.Add(listItem);
                listItem = new ListItem("用户", "User");
                this.rblSelect.Items.Add(listItem);
                listItem = new ListItem("指定邮箱", "Mail");
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
            this.phGroup.Visible = this.phUser.Visible = this.phMail.Visible = false;

            if (this.rblSelect.SelectedValue == "Group")
            {
                this.phGroup.Visible = true;
            }
            else if (this.rblSelect.SelectedValue == "User")
            {
                this.phUser.Visible = true;
            }
            else if (this.rblSelect.SelectedValue == "Mail")
            {
                this.phMail.Visible = true;
            }
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
		{
            ArrayList emailArrayList = new ArrayList();

            if (this.rblSelect.SelectedValue == "Group")
            {
                string groupIDCollection = ControlUtils.SelectedItemsValueToStringCollection(this.cblGroupID.Items);
                List<int> userIDList = BaiRongDataProvider.UserDAO.GetUserIDListByGroupIDCollection(groupIDCollection);

                foreach (int userID in userIDList)
                {
                    string email = BaiRongDataProvider.UserDAO.GetEmail(userID);
                    if (!string.IsNullOrEmpty(email) && StringUtils.IsEmail(email) && !emailArrayList.Contains(email))
                    {
                        emailArrayList.Add(email);
                    }
                }
            }
            else if (this.rblSelect.SelectedValue == "User")
            {
                ArrayList userNameArrayList = TranslateUtils.StringCollectionToArrayList(this.tbUserNameList.Text);

                foreach (string userName in userNameArrayList)
                {
                    int userID = BaiRongDataProvider.UserDAO.GetUserID(base.PublishmentSystemInfo.GroupSN, userName);
                    string email = BaiRongDataProvider.UserDAO.GetEmail(userID);
                    if (!string.IsNullOrEmpty(email) && StringUtils.IsEmail(email) && !emailArrayList.Contains(email))
                    {
                        emailArrayList.Add(email);
                    }
                }
            }
            else if (this.rblSelect.SelectedValue == "Mail")
            {
                ArrayList emails = TranslateUtils.StringCollectionToArrayList(this.tbMailList.Text);
                foreach (string email in emails)
                {
                    if (!string.IsNullOrEmpty(email) && StringUtils.IsEmail(email) && !emailArrayList.Contains(email))
                    {
                        emailArrayList.Add(email);
                    }
                }
            }
            else if (this.rblSelect.SelectedValue == "All")
            {
                List<int> userIDList = BaiRongDataProvider.UserDAO.GetUserIDList(true);
                foreach (int userID in userIDList)
                {
                    string email = BaiRongDataProvider.UserDAO.GetEmail(userID);
                    if (!string.IsNullOrEmpty(email) && StringUtils.IsEmail(email) && !emailArrayList.Contains(email))
                    {
                        emailArrayList.Add(email);
                    }
                }
            }

            if (emailArrayList.Count > 0)
            {
                try
                {
                    ISmtpMail smtpMail = MailUtils.GetInstance();
                    bool isSuccess = true;
                    string errorMessage = string.Empty;
                    if (emailArrayList.Count > 0)
                    {
                        smtpMail.AddRecipient(TranslateUtils.ArrayListToStringArray(emailArrayList));

                        smtpMail.MailDomainPort = UserConfigManager.Additional.MailDomainPort;
                        smtpMail.IsHtml = true;
                        smtpMail.Subject = this.tbTitle.Text;

                        smtpMail.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + this.breContent.Text + "</pre>";
                        smtpMail.MailDomain = UserConfigManager.Additional.MailDomain;
                        smtpMail.MailServerUserName = UserConfigManager.Additional.MailServerUserName;
                        smtpMail.MailServerPassword = UserConfigManager.Additional.MailServerPassword;

                        //开始发送
                        isSuccess = smtpMail.Send(out errorMessage);
                    }

                    if (isSuccess)
                    {
                        base.SuccessMessage("邮件发送成功！");
                    }
                    else
                    {
                        base.FailMessage("邮件发送失败：" + errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "邮件发送失败：" + ex.Message);
                }
            }
		}
	}
}