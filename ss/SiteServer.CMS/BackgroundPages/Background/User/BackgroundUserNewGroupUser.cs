using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using BaiRong.BackgroundPages.Modal;

using System.Collections.Generic;
using BaiRong.Core.AuxiliaryTable;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundUserNewGroupUser : BackgroundBasePage
    {
        public TextBox tbKeyword; 

        public Literal ltlColumnHeader;
        public Repeater rptContents;
        public SqlCountPager spContents;

        public Button Lock;
        public Button UnLock;
        public Button SendMail;
        public Button Delete;
        public Button SetMLibValidityDate;

        private int newGroupID;
        private string newGroupName = string.Empty; 

        public string GetDateTime(DateTime datetime)
        {
            string retval = string.Empty;
            if (datetime > DateUtils.SqlMinValue)
            {
                retval = DateUtils.GetDateString(datetime);
            }
            return retval;
        }

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_userNewGroupUser.aspx?publishmentSystemID={0}", publishmentSystemID));
        }
        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.newGroupID = TranslateUtils.ToInt(base.GetQueryString("NewGroupID"), 0);
            this.newGroupName = base.GetQueryString("NewGroupName");
            if (this.newGroupID == 0)
            {
                base.FailMessage(null, "用户组不存在！");
                return;
            } 
            if (base.GetQueryString("Delete") != null)
            {
                List<int> userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));
                try
                {
                    foreach (int userID in userIDList)
                    {
                        BaiRongDataProvider.UserDAO.Delete(userID);
                    }

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除用户", string.Empty);

                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }
            else if (base.GetQueryString("Lock") != null)
            {
                List<int> userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));
                try
                {
                    BaiRongDataProvider.UserDAO.Lock(userIDList, true);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "锁定用户", string.Empty);

                    base.SuccessMessage("成功锁定所选会员！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "锁定所选会员失败！");
                }
            }
            else if (base.GetQueryString("UnLock") != null)
            {
                List<int> userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));
                try
                {
                    BaiRongDataProvider.UserDAO.Lock(userIDList, false);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "解除锁定用户", string.Empty);

                    base.SuccessMessage("成功解除锁定所选会员！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "解除锁定所选会员失败！");
                }
            }
             
            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            if (TranslateUtils.ToInt(base.GetQueryString("PageNum")) == 0)
            {
                this.spContents.ItemsPerPage = 25;
            }
            else
            {
                this.spContents.ItemsPerPage = TranslateUtils.ToInt(base.GetQueryString("PageNum"));
            }
            bool isLockedOutSet = !string.IsNullOrEmpty(base.GetQueryString("IsLockedOut"));
            this.spContents.SelectCommand = BaiRongDataProvider.UserDAO.GetSelectCommandByNewGroup(base.PublishmentSystemInfo.GroupSN, base.GetQueryString("Keyword"), true, this.newGroupID);

            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.SortField = BaiRongDataProvider.UserDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_User, "用户组用户管理", AppManager.BBS.Permission.BBS_User);

                if (ConfigManager.Additional.IsUseMLib)
                {
                    string showPopWinString = Modal.UserSetMLibValidityDate.GetOpenWindowString();
                    this.SetMLibValidityDate.Attributes.Add("onclick", showPopWinString);
                    this.ltlColumnHeader.Text = "<td>投稿有效期</td>";
                    this.ltlColumnHeader.Text = this.ltlColumnHeader.Text + "<td>投稿数量</td>";
                }
                else
                {
                    this.SetMLibValidityDate.Visible = false;
                }

                string backgroundUrl = string.Format("{0}&NewGroupID={1}", BackgroundUserNewGroupUser.GetRedirectUrl(base.PublishmentSystemID), this.newGroupID);

                this.Lock.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&Lock=True", backgroundUrl), "UserIDCollection", "UserIDCollection", "请选择需要锁定的会员！", "此操作将锁定所选会员，确认吗？"));

                this.UnLock.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&UnLock=True", backgroundUrl), "UserIDCollection", "UserIDCollection", "请选择需要解除锁定的会员！", "此操作将解除锁定所选会员，确认吗？"));

                this.SendMail.Attributes.Add("onclick", Modal.UserSendMail.GetOpenWindowString(base.PublishmentSystemID));

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&Delete=True", backgroundUrl), "UserIDCollection", "UserIDCollection", "请选择需要删除的会员！", "此操作将删除所选会员，确认吗？"));


                this.spContents.DataBind();
            }
        }

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserInfo userInfo = new UserInfo(e.Item.DataItem);

                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlDisplayName = (Literal)e.Item.FindControl("ltlDisplayName");
                Literal ltlUserGroupName = (Literal)e.Item.FindControl("ltlUserGroupName");
                Literal ltlLastActivityDate = (Literal)e.Item.FindControl("ltlLastActivityDate");
                Literal ltlCreationDate = (Literal)e.Item.FindControl("ltlCreationDate");
                Literal ltlCredits = (Literal)e.Item.FindControl("ltlCredits");
                Literal ltlSelect = (Literal)e.Item.FindControl("ltlSelect");
                Literal ltlLoginCount = (Literal)e.Item.FindControl("ltlLoginCount");
                Literal ltlColumns = (Literal)e.Item.FindControl("ltlColumns");

                ltlUserName.Text = this.GetUserNameHtml(userInfo.UserID, userInfo.UserName, userInfo.IsLockedOut);
                ltlDisplayName.Text = userInfo.DisplayName;
                ltlUserGroupName.Text = this.newGroupName;
                ltlLoginCount.Text = userInfo.LoginNum.ToString();

                ltlLastActivityDate.Text = DateUtils.GetDateAndTimeString(userInfo.LastActivityDate);
                ltlCreationDate.Text = DateUtils.GetDateAndTimeString(userInfo.CreateDate);
                ltlCredits.Text = userInfo.Credits.ToString();

                ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""UserIDCollection"" value=""{0}"" />", userInfo.UserID);
                if (ConfigManager.Additional.IsUseMLib)
                {
                    UserNewGroupInfo ginfo = BaiRongDataProvider.UserNewGroupDAO.GetInfo(userInfo.NewGroupID);
                    ltlColumns.Text = string.Format(@"<td class=""center"">{0}</td>", GetMLibValidityDateHtml(userInfo, ginfo));
                    ltlColumns.Text = ltlColumns.Text + string.Format(@"<td class=""center"">{0}</td>", userInfo.MLibNum);
                }
            }
        }

        private string GetUserNameHtml(int userID, string userName, bool isLockedOut)
        {
            string showPopWinString = Modal.UserView.GetOpenWindowString(base.PublishmentSystemID, userID);
            string state = string.Empty;
            if (isLockedOut)
            {
                state = @"<span style=""color:red;"">[已被锁定]</span>";
            }
            return string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>{2}", showPopWinString, userName, state);
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(BackgroundUserNewGroup.GetRedirectUrl(base.PublishmentSystemID));
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = string.Format("{0}&NewGroupID={1}&Keyword={2}&NewGroupName={3}", BackgroundUserNewGroupUser.GetRedirectUrl(base.PublishmentSystemID), this.newGroupID, this.tbKeyword.Text, this.newGroupName);
                }
                return this._pageUrl;
            }
        }


        private string GetMLibValidityDateHtml(UserInfo info, UserNewGroupInfo ginfo)
        {
            string validityDate = "无限制";
            if (info.MLibValidityDate != DateUtils.SqlMinValue)
            {
                validityDate = info.MLibValidityDate.ToString("yyyy-MM-dd");
            }
            else
            {
                if (ginfo != null)
                {
                    if (ginfo.Additional.MLibValidityDate == 0)
                    {
                        validityDate = "无限制";
                    }
                    else
                    {
                        if (info.CreateDate > ConfigManager.Additional.MLibStartTime)
                        {
                            validityDate = info.CreateDate.AddMonths(ginfo.Additional.MLibValidityDate).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            validityDate = ConfigManager.Additional.MLibStartTime.AddMonths(ginfo.Additional.MLibValidityDate).ToString("yyyy-MM-dd");
                        }
                    }
                }
                else
                {
                    if (ConfigManager.Additional.UnifiedMLibValidityDate == 0)
                    {
                        validityDate = "无限制";
                    }
                    else
                    {
                        if (info.CreateDate > ConfigManager.Additional.MLibStartTime)
                        {
                            validityDate = info.CreateDate.AddMonths(ConfigManager.Additional.UnifiedMLibValidityDate).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            validityDate = ConfigManager.Additional.MLibStartTime.AddMonths(ConfigManager.Additional.UnifiedMLibValidityDate).ToString("yyyy-MM-dd");
                        }
                    }
                }
            }
            return string.Format(@"{0}", validityDate);
        }
    }
}
