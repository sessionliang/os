using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Controls;
using BaiRong.Core.Data.Provider;



namespace BaiRong.BackgroundPages
{
    public class BackgroundAdministrator : BackgroundBasePage
    {
        public DropDownList RoleName;
        public DropDownList PageNum;
        public DropDownList Order;
        public TextBox Keyword;
        public DropDownList ddlAreaID;
        public DropDownList LastActivityDate;

        public Repeater rptContents;
        public SqlPager spContents;

        public Button AddButton;
        public Button Lock;
        public Button UnLock;
        public Button SendMail;
        public Button Delete;

        private int departmentID;
        private DepartmentInfo departmentInfo;
        private bool[] isLastNodeArrayOfArea;

        public static string GetRedirectUrl(int departmentID)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_administrator.aspx?departmentID={0}", departmentID));
        }

        public string GetRolesHtml(string userName)
        {
            return AdminManager.GetRolesHtml(userName);
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

            this.departmentID = TranslateUtils.ToInt(base.GetQueryString("departmentID"));
            int areaID = TranslateUtils.ToInt(base.GetQueryString("areaID"));
            if (departmentID > 0)
            {
                this.departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);
            }

            if (base.GetQueryString("Delete") != null)
            {
                string userNameCollection = base.GetQueryString("UserNameCollection");
                try
                {
                    ArrayList userNameArrayList = TranslateUtils.StringCollectionToArrayList(userNameCollection);
                    foreach (string userName in userNameArrayList)
                    {
                        BaiRongDataProvider.AdministratorDAO.Delete(userName);
                    }

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除管理员", string.Format("管理员:{0}", userNameCollection));

                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }
            else if (base.GetQueryString("Lock") != null)
            {
                string userNameCollection = base.GetQueryString("UserNameCollection");
                try
                {
                    ArrayList userNameArrayList = TranslateUtils.StringCollectionToArrayList(userNameCollection);
                    BaiRongDataProvider.AdministratorDAO.LockUsers(userNameArrayList, true);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "锁定管理员", string.Format("管理员:{0}", userNameCollection));

                    base.SuccessMessage("成功锁定所选管理员！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "锁定所选管理员失败！");
                }
            }
            else if (base.GetQueryString("UnLock") != null)
            {
                string userNameCollection = base.GetQueryString("UserNameCollection");
                try
                {
                    ArrayList userNameArrayList = TranslateUtils.StringCollectionToArrayList(userNameCollection);
                    BaiRongDataProvider.AdministratorDAO.LockUsers(userNameArrayList, false);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "解除锁定管理员", string.Format("管理员:{0}", userNameCollection));

                    base.SuccessMessage("成功解除锁定所选管理员！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "解除锁定所选管理员失败！");
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.ItemsPerPage = StringUtils.Constants.PageSize;

            if (string.IsNullOrEmpty(base.GetQueryString("PageNum")))
            {
                if (TranslateUtils.ToInt(this.PageNum.SelectedValue) == 0)
                {
                    this.spContents.ItemsPerPage = StringUtils.Constants.PageSize;
                }
                else
                {
                    this.spContents.ItemsPerPage = TranslateUtils.ToInt(this.PageNum.SelectedValue);
                }

                this.spContents.SelectCommand = BaiRongDataProvider.AdministratorDAO.GetSelectCommand(PermissionsManager.Current.IsConsoleAdministrator, AdminManager.Current.UserName, departmentID);
                this.spContents.SortField = BaiRongDataProvider.AdministratorDAO.GetSortFieldName();
                this.spContents.SortMode = SortMode.ASC;
            }
            else
            {
                if (TranslateUtils.ToInt(base.GetQueryString("PageNum")) == 0)
                {
                    this.spContents.ItemsPerPage = StringUtils.Constants.PageSize;
                }
                else
                {
                    this.spContents.ItemsPerPage = TranslateUtils.ToInt(base.GetQueryString("PageNum"));
                }
                this.spContents.SelectCommand = BaiRongDataProvider.AdministratorDAO.GetSelectCommand(base.GetQueryString("Keyword"), base.GetQueryString("RoleName"), TranslateUtils.ToInt(base.GetQueryString("LastActivityDate")), PermissionsManager.Current.IsConsoleAdministrator, AdminManager.Current.UserName, departmentID, TranslateUtils.ToInt(base.GetQueryString("AreaID")));
                this.spContents.SortField = base.GetQueryString("Order");
                if (StringUtils.EqualsIgnoreCase(this.spContents.SortField, "UserName"))
                {
                    this.spContents.SortMode = SortMode.ASC;
                }
                else
                {
                    this.spContents.SortMode = SortMode.DESC;
                }
            }
            //if (base.PublishmentSystemID != 0)
            //{
            //    this.MySqlPager.SelectCommand = BaiRongDataProvider.AdministratorDAO.GetSelectCommand(base.PublishmentSystemID);
            //}
            //else
            //{
            //    this.MySqlPager.SelectCommand = BaiRongDataProvider.AdministratorDAO.GetSelectCommand();
            //}

            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Administrator, "管理员管理", AppManager.Platform.Permission.Platform_Administrator);

                ListItem theListItem = new ListItem("全部", string.Empty);
                theListItem.Selected = true;
                this.RoleName.Items.Add(theListItem);

                string[] allRoles = null;
                if (PermissionsManager.Current.IsConsoleAdministrator)
                {
                    allRoles = RoleManager.GetAllRoles();
                }
                else
                {
                    allRoles = RoleManager.GetAllRolesByCreatorUserName(AdminManager.Current.UserName);
                }

                ArrayList allPredefinedRoles = EPredefinedRoleUtils.GetAllPredefinedRole();
                foreach (string roleName in allRoles)
                {
                    string roleText = roleName;
                    if (allPredefinedRoles.Contains(roleName))
                    {
                        if (EPredefinedRoleUtils.Equals(EPredefinedRole.Administrator, roleName))
                        {
                            roleText = EPredefinedRoleUtils.GetText(EPredefinedRole.Administrator);
                        }
                        else if (EPredefinedRoleUtils.Equals(EPredefinedRole.ConsoleAdministrator, roleName))
                        {
                            roleText = EPredefinedRoleUtils.GetText(EPredefinedRole.ConsoleAdministrator);
                        }
                        else if (EPredefinedRoleUtils.Equals(EPredefinedRole.SystemAdministrator, roleName))
                        {
                            roleText = EPredefinedRoleUtils.GetText(EPredefinedRole.SystemAdministrator);
                        }
                    }
                    ListItem listitem = new ListItem(roleText, roleName);
                    this.RoleName.Items.Add(listitem);
                }

                this.ddlAreaID.Items.Add(new ListItem("<全部区域>", "0"));
                ArrayList areaIDArrayList = AreaManager.GetAreaIDArrayList();
                int count = areaIDArrayList.Count;
                this.isLastNodeArrayOfArea = new bool[count];
                foreach (int theAreaID in areaIDArrayList)
                {
                    AreaInfo areaInfo = AreaManager.GetAreaInfo(theAreaID);
                    ListItem listitem = new ListItem(this.GetArea(areaInfo.AreaID, areaInfo.AreaName, areaInfo.ParentsCount, areaInfo.IsLastNode), theAreaID.ToString());
                    if (areaID == theAreaID)
                    {
                        listitem.Selected = true;
                    }
                    this.ddlAreaID.Items.Add(listitem);
                }

                if (!string.IsNullOrEmpty(base.GetQueryString("PageNum")))
                {
                    ControlUtils.SelectListItems(this.RoleName, base.GetQueryString("RoleName"));
                    ControlUtils.SelectListItems(this.PageNum, base.GetQueryString("PageNum"));
                    this.Keyword.Text = base.GetQueryString("Keyword");
                    ControlUtils.SelectListItems(this.ddlAreaID, base.GetQueryString("AreaID"));
                    ControlUtils.SelectListItems(this.LastActivityDate, base.GetQueryString("LastActivityDate"));
                    ControlUtils.SelectListItems(this.Order, base.GetQueryString("Order"));
                }

                string urlAdd = BackgroundAdministratorAdd.GetRedirectUrlToAdd(departmentID);
                this.AddButton.Attributes.Add("onclick", string.Format(@"location.href='{0}';return false;", urlAdd));

                string urlAdministrator = BackgroundAdministrator.GetRedirectUrl(this.departmentID);

                this.Lock.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlAdministrator + "&Lock=True", "UserNameCollection", "UserNameCollection", "请选择需要锁定的管理员！", "此操作将锁定所选管理员，确认吗？"));

                this.UnLock.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlAdministrator + "&UnLock=True", "UserNameCollection", "UserNameCollection", "请选择需要解除锁定的管理员！", "此操作将解除锁定所选管理员，确认吗？"));

                this.SendMail.Attributes.Add("onclick", BaiRong.BackgroundPages.Modal.AdminSendMail.GetOpenWindowString());

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlAdministrator + "&Delete=True", "UserNameCollection", "UserNameCollection", "请选择需要删除的管理员！", "此操作将删除所选管理员，确认吗？"));

                this.spContents.DataBind();
            }
        }

        public string GetArea(int areaID, string areaName, int parentsCount, bool isLastNode)
        {
            string str = "";
            if (isLastNode == false)
            {
                this.isLastNodeArrayOfArea[parentsCount] = false;
            }
            else
            {
                this.isLastNodeArrayOfArea[parentsCount] = true;
            }
            for (int i = 0; i < parentsCount; i++)
            {
                if (this.isLastNodeArrayOfArea[i])
                {
                    str = String.Concat(str, "　");
                }
                else
                {
                    str = String.Concat(str, "│");
                }
            }
            if (isLastNode)
            {
                str = String.Concat(str, "└");
            }
            else
            {
                str = String.Concat(str, "├");
            }
            str = String.Concat(str, areaName);
            return str;
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string userName = TranslateUtils.EvalString(e.Item.DataItem, "UserName");
                string displayName = TranslateUtils.EvalString(e.Item.DataItem, "DisplayName");
                int departmentID = TranslateUtils.EvalInt(e.Item.DataItem, "DepartmentID");
                int areaID = TranslateUtils.EvalInt(e.Item.DataItem, "AreaID");
                if (string.IsNullOrEmpty(displayName))
                {
                    displayName = userName;
                }
                string departmentName = string.Empty;
                if (this.departmentInfo != null)
                {
                    departmentName = this.departmentInfo.DepartmentName;
                }
                else if (departmentID > 0)
                {
                    departmentName = DepartmentManager.GetDepartmentName(departmentID);
                }
                string creatorUserName = TranslateUtils.EvalString(e.Item.DataItem, "CreatorUserName");
                bool isLockedOut = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsLockedOut"));

                if (ConfigManager.IsSysAdministrator(userName))
                {
                    e.Item.Visible = false;
                    return;
                }

                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlDisplayName = (Literal)e.Item.FindControl("ltlDisplayName");
                Literal ltlDepartment = (Literal)e.Item.FindControl("ltlDepartment");
                Literal ltlArea = (Literal)e.Item.FindControl("ltlArea");
                Literal ltlEdit = (Literal)e.Item.FindControl("ltlEdit");
                HyperLink hlChangePassword = (HyperLink)e.Item.FindControl("hlChangePassword");
                Literal ltlRole = (Literal)e.Item.FindControl("ltlRole");
                Literal ltlSelect = (Literal)e.Item.FindControl("ltlSelect");

                ltlUserName.Text = this.GetUserNameHtml(userName, isLockedOut);
                ltlDisplayName.Text = displayName;
                ltlDepartment.Text = departmentName;
                ltlArea.Text = AreaManager.GetAreaName(areaID);

                string urlEdit = BackgroundAdministratorAdd.GetRedirectUrlToEdit(departmentID, userName);
                ltlEdit.Text = string.Format(@"<a href=""{0}"">修改属性</a>", urlEdit);
                hlChangePassword.Attributes.Add("onclick", BaiRong.BackgroundPages.Modal.AdminPassword.GetOpenWindowString(userName));

                if (AdminManager.Current.UserName != userName)
                {
                    string openWindowString = PageUtilityPF.GetPermissionsSetOpenWindowString(userName);
                    ltlRole.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">权限设置</a>", openWindowString);
                    ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""UserNameCollection"" value=""{0}"" />", userName);
                }
            }
        }

        private string GetUserNameHtml(string userName, bool isLockedOut)
        {
            string showPopWinString = Modal.AdminView.GetOpenWindowString(userName);
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
                    string url = BackgroundAdministrator.GetRedirectUrl(this.departmentID);
                    this._pageUrl = url + string.Format("&RoleName={0}&PageNum={1}&Keyword={2}&AreaID={3}&LastActivityDate={4}&Order={5}", this.RoleName.SelectedValue, this.PageNum.SelectedValue, this.Keyword.Text, this.ddlAreaID.SelectedValue, this.LastActivityDate.SelectedValue, this.Order.SelectedValue);
                }
                return this._pageUrl;
            }
        }
    }
}
