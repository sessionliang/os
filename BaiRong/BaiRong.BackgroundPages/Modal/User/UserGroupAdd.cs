using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;


namespace BaiRong.BackgroundPages.Modal
{
    public class UserGroupAdd : BackgroundBasePage
	{
        protected PlaceHolder phCreditsAddTips;
        protected PlaceHolder phCreditsEditTips;
        protected PlaceHolder phCredits;

        protected TextBox GroupName;
        protected TextBox CreditsFrom;
        protected TextBox CreditsTo;
        protected TextBox Stars;
        protected TextBox Color;

        private int groupID = 0;
        private bool isCredits = false;
        private bool isAdd = false;

        public static string GetOpenWindowStringToEdit( int groupID, bool isCredits)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("GroupID", groupID.ToString());
            arguments.Add("IsCredits", isCredits.ToString());
            arguments.Add("IsAdd", false.ToString());
            return PageUtilityPF.GetOpenWindowString("修改用户组", "modal_userGroupAdd.aspx", arguments, 620, 550);
        }

        public static string GetOpenWindowStringToAdd( int groupID, bool isCredits)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("GroupID", groupID.ToString());
            arguments.Add("IsCredits", isCredits.ToString());
            arguments.Add("IsAdd", true.ToString());
            return PageUtilityPF.GetOpenWindowString("添加用户组", "modal_userGroupAdd.aspx", arguments, 620, 550);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.groupID = TranslateUtils.ToInt(base.GetQueryString("GroupID"));
            this.isCredits = TranslateUtils.ToBool(base.GetQueryString("IsCredits"));
            this.isAdd = TranslateUtils.ToBool(base.GetQueryString("IsAdd"));

            if (!IsPostBack)
            {
                if (this.isCredits)
                {
                    this.phCredits.Visible = true;
                    if (!this.isAdd)
                    {
                        this.phCreditsAddTips.Visible = false;
                        this.phCreditsEditTips.Visible = true;
                    }
                    else
                    {
                        this.phCreditsAddTips.Visible = true;
                        this.phCreditsEditTips.Visible = false;
                    }
                }
                else
                {
                    this.phCreditsAddTips.Visible = this.phCreditsEditTips.Visible = this.phCredits.Visible = false;
                }

                if (!this.isAdd)
                {
                    UserGroupInfo groupInfo = UserGroupManager.GetGroupInfo(string.Empty, this.groupID);
                    if (groupInfo != null)
                    {
                        this.GroupName.Text = groupInfo.GroupName;
                        this.CreditsFrom.Text = groupInfo.CreditsFrom.ToString();
                        this.CreditsTo.Text = groupInfo.CreditsTo.ToString();
                        this.Stars.Text = groupInfo.Stars.ToString();
                        this.Color.Text = groupInfo.Color;
                    }
                }
                else
                {
                    if (this.groupID > 0)
                    {
                        UserGroupInfo groupInfo = UserGroupManager.GetGroupInfo(string.Empty, this.groupID);
                        if (groupInfo != null)
                        {
                            this.CreditsFrom.Text = groupInfo.CreditsFrom.ToString();
                            this.CreditsTo.Text = groupInfo.CreditsTo.ToString();
                            this.Stars.Text = groupInfo.Stars.ToString();
                            this.Color.Text = groupInfo.Color;
                        }
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (!this.isAdd)
                {
                    try
                    {
                        UserGroupInfo groupInfo = UserGroupManager.GetGroupInfo(string.Empty, this.groupID);
                        if (groupInfo.GroupName != this.GroupName.Text && BaiRongDataProvider.UserGroupDAO.IsExists(string.Empty, this.GroupName.Text))
                        {
                            base.FailMessage("用户组修改失败，用户组名称已存在！");
                            return;
                        }

                        int oldCreditsFrom = groupInfo.CreditsFrom;
                        int oldCreditsTo = groupInfo.CreditsTo;
                        if (this.isCredits)
                        {
                            int creditsFrom = TranslateUtils.ToInt(this.CreditsFrom.Text);
                            int creditsTo = TranslateUtils.ToInt(this.CreditsTo.Text);
                            if (creditsFrom < oldCreditsFrom || creditsTo > oldCreditsTo)
                            {
                                base.FailMessage(string.Format("用户组修改失败，积分范围必须在 {0} 和 {1} 之间！", groupInfo.CreditsFrom, groupInfo.CreditsTo));
                                return;
                            }
                            if (creditsFrom == 0 && creditsFrom != 0)
                            {
                                base.FailMessage("用户组修改失败，必须保留一个最低积分为 0 的用户组！");
                                return;
                            }
                            groupInfo.CreditsFrom = creditsFrom;
                            groupInfo.CreditsTo = creditsTo;
                        }

                        groupInfo.GroupName = this.GroupName.Text;
                        groupInfo.Stars = TranslateUtils.ToInt(this.Stars.Text);
                        groupInfo.Color = this.Color.Text;

                        if (this.isCredits)
                        {
                            BaiRongDataProvider.UserGroupDAO.UpdateWithCredits(string.Empty, groupInfo, oldCreditsFrom, oldCreditsTo);
                        }
                        else
                        {
                            BaiRongDataProvider.UserGroupDAO.UpdateWithoutCredits(string.Empty, groupInfo);
                        }

                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改用户组", string.Format("用户组:{0}", this.GroupName.Text));

                        JsUtils.OpenWindow.CloseModalPage(Page);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "用户组修改失败！");
                    }
                }
                else
                {
                    if (BaiRongDataProvider.UserGroupDAO.IsExists(string.Empty, this.GroupName.Text))
                    {
                        base.FailMessage("用户组添加失败，用户组名称已存在！");
                        return;
                    }
                    else
                    {
                        int creditsFrom = TranslateUtils.ToInt(this.CreditsFrom.Text);
                        int creditsTo = TranslateUtils.ToInt(this.CreditsTo.Text);

                        if (this.isCredits && !BaiRongDataProvider.UserGroupDAO.IsCreditsValid(string.Empty, creditsFrom, creditsTo))
                        {
                            base.FailMessage("用户组添加失败，积分范围必须在已有的某个用户组积分范围之内！");
                            return;
                        }

                        UserGroupInfo groupInfo = new UserGroupInfo(0, string.Empty, this.GroupName.Text, this.isCredits ? EUserGroupType.Credits : EUserGroupType.Specials, creditsFrom, creditsTo, TranslateUtils.ToInt(this.Stars.Text), this.Color.Text, string.Empty);

                        try
                        {
                            BaiRongDataProvider.UserGroupDAO.Insert(groupInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加用户组", string.Format("用户组:{0}", this.GroupName.Text));

                            JsUtils.OpenWindow.CloseModalPage(Page);
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, "用户组添加失败！" + ex.ToString());
                        }
                    }
                }

            }
        }

	}
}
