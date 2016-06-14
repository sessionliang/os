using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;


using System.Collections.Generic;
using BaiRong.Core.AuxiliaryTable;

namespace BaiRong.BackgroundPages
{
    public class BackgroundUser : BackgroundBasePage
    {
        public DropDownList ddlUserLevel;
        public DropDownList ddlPageNum;
        public DropDownList ddlLoginNum;

        public DropDownList SearchType;
        public TextBox tbKeyword;
        public DropDownList ddlCreationDate;
        public DropDownList ddlLastActivityDate;

        public Literal ltlColumnHeader;
        public Repeater rptContents;
        public SqlCountPager spContents;

        public Button btnAdd;
        public Button AddToGroup;
        public Button Lock;
        public Button UnLock;
        public Button SendMail;
        public Button SendSMS;
        public Button SendMsg;
        public Button Delete;
        public Button Import;
        public Button Export;

        #region by 20160119 增加新的用户组功能
        public Button AddToNewGroup;
        public Button SetMLibValidityDate;
        #endregion

        private string defaultGroupName = string.Empty;

        private ArrayList tableStyleInfoArrayList;

        public static string GetRedirectUrl()
        {
            return PageUtils.GetPlatformUrl(string.Format("background_user.aspx?"));
        }

        public string GetDateTime(DateTime datetime)
        {
            string retval = string.Empty;
            if (datetime > DateUtils.SqlMinValue)
            {
                retval = DateUtils.GetDateString(datetime);
            }
            return retval;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.User, BaiRongDataProvider.UserDAO.TABLE_NAME, null);

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

            if (string.IsNullOrEmpty(base.GetQueryString("UserLevel")))
            {
                if (TranslateUtils.ToInt(this.ddlPageNum.SelectedValue) == 0)
                {
                    this.spContents.ItemsPerPage = 25;
                }
                else
                {
                    this.spContents.ItemsPerPage = TranslateUtils.ToInt(this.ddlPageNum.SelectedValue);
                }

                this.spContents.SelectCommand = BaiRongDataProvider.UserDAO.GetSelectCommand(string.Empty, true);
            }
            else
            {
                if (TranslateUtils.ToInt(base.GetQueryString("PageNum")) == 0)
                {
                    this.spContents.ItemsPerPage = 25;
                }
                else
                {
                    this.spContents.ItemsPerPage = TranslateUtils.ToInt(base.GetQueryString("PageNum"));
                }
                bool isLockedOutSet = !string.IsNullOrEmpty(base.GetQueryString("IsLockedOut"));
                this.spContents.SelectCommand = BaiRongDataProvider.UserDAO.GetSelectCommand(string.Empty, TranslateUtils.ToInt(base.GetQueryString("UserLevel")), base.GetQueryString("Keyword"), TranslateUtils.ToInt(base.GetQueryString("CreationDate")), TranslateUtils.ToInt(base.GetQueryString("LastActivityDate")), true, TranslateUtils.ToInt(this.GetQueryString("LoginNum")), base.GetQueryString("SearchType"));
            }

            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.SortField = BaiRongDataProvider.UserDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;

            UserLevelInfo levelInfo = UserLevelManager.GetLevelInfoByCredits(string.Empty, 0);
            if (levelInfo != null)
            {
                this.defaultGroupName = levelInfo.LevelName;
            }

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_User, "用户管理", AppManager.User.Permission.Usercenter_User);

                ListItem theListItem = new ListItem("全部", "0");
                theListItem.Selected = true;
                this.ddlUserLevel.Items.Add(theListItem);
                //ArrayList userLevelInfoArrayList = UserLevelManager.GetLevelInfoArrayList(string.Empty);
                //foreach (UserLevelInfo userLevelInfo in userLevelInfoArrayList)
                //{
                //    ListItem listitem = new ListItem(userLevelInfo.LevelName, userLevelInfo.ID.ToString());
                //    this.ddlUserLevel.Items.Add(listitem);
                //}


                if (this.tableStyleInfoArrayList != null)
                {
                    foreach (TableStyleInfo styleInfo in this.tableStyleInfoArrayList)
                    {
                        if (styleInfo.IsVisible)
                        {
                            ListItem listitem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);
                            this.SearchType.Items.Add(listitem);
                        }
                    }
                }

                //添加隐藏属性
                this.SearchType.Items.Add(new ListItem("用户ID", UserAttribute.UserID));
                this.SearchType.Items.Add(new ListItem("用户名", UserAttribute.UserName));

                //默认选择用户名
                this.SearchType.SelectedValue = UserAttribute.UserName;

                if (!string.IsNullOrEmpty(base.GetQueryString("SearchType")))
                {
                    ControlUtils.SelectListItems(this.SearchType, base.GetQueryString("SearchType"));
                }

                if (!string.IsNullOrEmpty(base.GetQueryString("UserLevel")))
                {
                    ControlUtils.SelectListItems(this.ddlUserLevel, base.GetQueryString("UserLevel"));
                    ControlUtils.SelectListItems(this.ddlPageNum, base.GetQueryString("PageNum"));
                    ControlUtils.SelectListItems(this.ddlLoginNum, base.GetQueryString("LoginNum"));
                    this.tbKeyword.Text = base.GetQueryString("Keyword");
                    ControlUtils.SelectListItems(this.ddlCreationDate, base.GetQueryString("CreationDate"));
                    ControlUtils.SelectListItems(this.ddlLastActivityDate, base.GetQueryString("LastActivityDate"));
                }

                string showPopWinString = Modal.AddToUserGroup.GetOpenWindowString();
                this.AddToGroup.Attributes.Add("onclick", showPopWinString);


                #region by 20160119 增加新的用户组功能
                showPopWinString = Modal.AddToUserNewGroup.GetOpenWindowString();
                this.AddToNewGroup.Attributes.Add("onclick", showPopWinString);

                if (ConfigManager.Additional.IsUseMLib)
                {
                    showPopWinString = Modal.UserSetMLibValidityDate.GetOpenWindowString();
                    this.SetMLibValidityDate.Attributes.Add("onclick", showPopWinString);

                    this.ltlColumnHeader.Text = "<td>投稿有效期</td>";
                    this.ltlColumnHeader.Text = this.ltlColumnHeader.Text + "<td>投稿数量</td>";
                }
                else
                {
                    this.SetMLibValidityDate.Visible = false;
                }
                #endregion

                string backgroundUrl = BackgroundUser.GetRedirectUrl();

                this.btnAdd.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", BackgroundUserAdd.GetRedirectUrlToAdd(this.PageUrl)));

                this.Lock.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&Lock=True", backgroundUrl), "UserIDCollection", "UserIDCollection", "请选择需要锁定的会员！", "此操作将锁定所选会员，确认吗？"));

                this.UnLock.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&UnLock=True", backgroundUrl), "UserIDCollection", "UserIDCollection", "请选择需要解除锁定的会员！", "此操作将解除锁定所选会员，确认吗？"));

                this.SendMail.Attributes.Add("onclick", Modal.UserSendMail.GetOpenWindowString());
                this.SendSMS.Attributes.Add("onclick", Modal.UserSendSMS.GetOpenWindowString());
                this.SendMsg.Attributes.Add("onclick", Modal.UserSendMsg.GetOpenWindowString());

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&Delete=True", backgroundUrl), "UserIDCollection", "UserIDCollection", "请选择需要删除的会员！", "此操作将删除所选会员，确认吗？"));

                this.Import.Attributes.Add("onclick", Modal.UserImport.GetOpenWindowString());

                this.Export.Attributes.Add("onclick", Modal.UserExport.GetOpenWindowString());

                //if (EPublishmentSystemTypeUtils.IsB2C(base.PublishmentSystemInfo.PublishmentSystemType))
                //{
                //    this.ltlColumnHeader.Text = "<td></td><td></td>";
                //}


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
                //Literal ltlUserLevelName = (Literal)e.Item.FindControl("ltlUserLevelName");
                Literal ltlLastActivityDate = (Literal)e.Item.FindControl("ltlLastActivityDate");
                Literal ltlLoginCount = (Literal)e.Item.FindControl("ltlLoginCount");
                Literal ltlCreationDate = (Literal)e.Item.FindControl("ltlCreationDate");
                Literal ltlCredits = (Literal)e.Item.FindControl("ltlCredits");
                Literal ltlNewGroupName = (Literal)e.Item.FindControl("ltlNewGroupName");
                Literal ltlColumns = (Literal)e.Item.FindControl("ltlColumns");
                Literal ltlSelect = (Literal)e.Item.FindControl("ltlSelect");
                HyperLink hlChangePassword = (HyperLink)e.Item.FindControl("hlChangePassword");
                HyperLink hlEditLink = (HyperLink)e.Item.FindControl("hlEditLink");

                ltlUserName.Text = this.GetUserNameHtml(userInfo.UserID, userInfo.UserName, userInfo.IsLockedOut);
                ltlDisplayName.Text = userInfo.DisplayName;

                //if (userInfo.GroupID != 0)
                //{
                //    ltlUserLevelName.Text = UserLevelManager.GetLevelName(string.Empty, userInfo.GroupID);
                //}
                //if (string.IsNullOrEmpty(ltlUserLevelName.Text))
                //{
                //    ltlUserLevelName.Text = this.defaultGroupName;
                //}

                ltlLastActivityDate.Text = DateUtils.GetDateAndTimeString(userInfo.LastActivityDate);
                ltlLoginCount.Text = userInfo.LoginNum.ToString();
                ltlCreationDate.Text = DateUtils.GetDateAndTimeString(userInfo.CreateDate);
                ltlCredits.Text = userInfo.Credits.ToString();
                UserNewGroupInfo ginfo = BaiRongDataProvider.UserNewGroupDAO.GetInfo(userInfo.NewGroupID);
                ltlNewGroupName.Text = ((ginfo != null) ? ginfo.ItemName : "");

                //if (EPublishmentSystemTypeUtils.IsB2C(base.PublishmentSystemInfo.PublishmentSystemType))
                //{
                //    string urlConsignee = PageUtils.GetB2CUrl(string.Format("background_userConsignee.aspx?publishmentSystemID={0}&userName={1}&returnUrl={2}", base.PublishmentSystemID, userInfo.UserName, StringUtils.ValueToUrl(this.PageUrl)));
                //    string urlInvoice = PageUtils.GetB2CUrl(string.Format("background_userInvoice.aspx?publishmentSystemID={0}&userName={1}&returnUrl={2}", base.PublishmentSystemID, userInfo.UserName, StringUtils.ValueToUrl(this.PageUrl)));
                //    ltlColumns.Text = string.Format(@"<td class=""center""><a href=""{0}"">收件人信息</a></td><td class=""center""><a href=""{1}"">发票信息</a></td>", urlConsignee, urlInvoice);
                //}

                hlEditLink.NavigateUrl = BackgroundUserAdd.GetRedirectUrlToEdit(userInfo.UserID, BackgroundUser.GetRedirectUrl());
                hlChangePassword.Attributes.Add("onclick", Modal.UserPassword.GetOpenWindowString(userInfo.UserID));
                ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""UserIDCollection"" value=""{0}"" />", userInfo.UserID);

                if (ConfigManager.Additional.IsUseMLib)
                {
                    ltlColumns.Text = string.Format(@"<td class=""center"">{0}</td>", GetMLibValidityDateHtml(userInfo, ginfo));
                    ltlColumns.Text = ltlColumns.Text + string.Format(@"<td class=""center"">{0}</td>", userInfo.MLibNum);
                }
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



        private string GetUserNameHtml(int userID, string userName, bool isLockedOut)
        {
            string showPopWinString = Modal.UserView.GetOpenWindowString(userName);
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

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = string.Format("{0}&UserLevel={1}&PageNum={2}&Keyword={3}&CreationDate={4}&LastActivityDate={5}&loginNum={6}&SearchType={7}", BackgroundUser.GetRedirectUrl(), this.ddlUserLevel.SelectedValue, this.ddlPageNum.SelectedValue, this.tbKeyword.Text, this.ddlCreationDate.SelectedValue, this.ddlLastActivityDate.SelectedValue, this.ddlLoginNum.SelectedValue, this.SearchType.SelectedValue);
                }
                return this._pageUrl;
            }
        }
    }
}
