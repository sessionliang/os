using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;


using System.Collections;
using BaiRong.Core.Data.Provider;

namespace BaiRong.BackgroundPages
{
	public class BackgroundAdministratorAdd : BackgroundBasePage
	{
        public Literal ltlPageTitle;
        public DropDownList ddlDepartmentID;
        public TextBox tbUserName;
        public TextBox tbDisplayName;
        public PlaceHolder phPassword;
        public TextBox tbPassword;
        public TextBox tbPasswordQuestion;
        public TextBox tbPasswordAnswer;

        public DropDownList ddlAreaID;
        public TextBox tbEmail;
        public TextBox tbMobile;
        
        public Button btnReturn;

        private int departmentID;
        private int areaID;
        private string userName;
        private bool[] isLastNodeArrayOfDepartment;
        private bool[] isLastNodeArrayOfArea;

        public static string GetRedirectUrlToAdd(int departmentID)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_administratorAdd.aspx?departmentID={0}", departmentID));
        }

        public static string GetRedirectUrlToEdit(int departmentID, string userName)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_administratorAdd.aspx?departmentID={0}&userName={1}", departmentID, userName));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.departmentID = base.GetIntQueryString("departmentID");
            this.areaID = base.GetIntQueryString("areaID");
            this.userName = base.GetQueryString("userName");

            if (!Page.IsPostBack)
            {
                string pageTitle = string.IsNullOrEmpty(this.userName) ? "添加管理员" : "编辑管理员";
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Administrator, pageTitle, AppManager.Platform.Permission.Platform_Administrator);

                this.ltlPageTitle.Text = pageTitle;

                this.ddlDepartmentID.Items.Add(new ListItem("<无所属部门>", "0"));
                ArrayList departmentIDArrayList = DepartmentManager.GetDepartmentIDArrayList();
                int count = departmentIDArrayList.Count;
                this.isLastNodeArrayOfDepartment = new bool[count];
                foreach (int theDepartmentID in departmentIDArrayList)
                {
                    DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(theDepartmentID);
                    ListItem listitem = new ListItem(this.GetDepartment(departmentInfo.DepartmentID, departmentInfo.DepartmentName, departmentInfo.ParentsCount, departmentInfo.IsLastNode), theDepartmentID.ToString());
                    if (this.departmentID == theDepartmentID)
                    {
                        listitem.Selected = true;
                    }
                    this.ddlDepartmentID.Items.Add(listitem);
                }

                this.ddlAreaID.Items.Add(new ListItem("<无所在区域>", "0"));
                ArrayList areaIDArrayList = AreaManager.GetAreaIDArrayList();
                count = areaIDArrayList.Count;
                this.isLastNodeArrayOfArea = new bool[count];
                foreach (int theAreaID in areaIDArrayList)
                {
                    AreaInfo areaInfo = AreaManager.GetAreaInfo(theAreaID);
                    ListItem listitem = new ListItem(this.GetArea(areaInfo.AreaID, areaInfo.AreaName, areaInfo.ParentsCount, areaInfo.IsLastNode), theAreaID.ToString());
                    if (this.areaID == theAreaID)
                    {
                        listitem.Selected = true;
                    }
                    this.ddlAreaID.Items.Add(listitem);
                }

                if (!string.IsNullOrEmpty(this.userName))
                {
                    AdministratorInfo adminInfo = BaiRongDataProvider.AdministratorDAO.GetAdministratorInfo(this.userName);
                    if (adminInfo != null)
                    {
                        ControlUtils.SelectListItems(this.ddlDepartmentID, adminInfo.DepartmentID.ToString());
                        this.tbUserName.Text = adminInfo.UserName;
                        this.tbUserName.Enabled = false;
                        this.tbDisplayName.Text = adminInfo.DisplayName;
                        this.phPassword.Visible = false;
                        ControlUtils.SelectListItems(this.ddlAreaID, adminInfo.AreaID.ToString());
                        this.tbEmail.Text = adminInfo.Email;
                        this.tbMobile.Text = adminInfo.Mobile;
                    }
                }

                string urlReturn = BackgroundAdministrator.GetRedirectUrl(this.departmentID);
                this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", urlReturn));
            }
		}

        public string GetDepartment(int departmentID, string departmentName, int parentsCount, bool isLastNode)
        {
            string str = "";
            if (isLastNode == false)
            {
                this.isLastNodeArrayOfDepartment[parentsCount] = false;
            }
            else
            {
                this.isLastNodeArrayOfDepartment[parentsCount] = true;
            }
            for (int i = 0; i < parentsCount; i++)
            {
                if (this.isLastNodeArrayOfDepartment[i])
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
            str = String.Concat(str, departmentName);
            return str;
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

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                string errorMessage = string.Empty;

                if (string.IsNullOrEmpty(this.userName))
                {
                    AdministratorInfo adminInfo = new AdministratorInfo();
                    adminInfo.UserName = this.tbUserName.Text.Trim();
                    adminInfo.Password = this.tbPassword.Text;
                    adminInfo.CreatorUserName = AdminManager.Current.UserName;
                    adminInfo.DisplayName = this.tbDisplayName.Text;
                    adminInfo.Question = this.tbPasswordQuestion.Text;
                    adminInfo.Answer = this.tbPasswordAnswer.Text;
                    adminInfo.Email = this.tbEmail.Text;
                    adminInfo.Mobile = this.tbMobile.Text;
                    adminInfo.DepartmentID = TranslateUtils.ToInt(this.ddlDepartmentID.SelectedValue);
                    adminInfo.AreaID = TranslateUtils.ToInt(this.ddlAreaID.SelectedValue);

                    if (ConfigManager.IsSysAdministrator(adminInfo.UserName))
                    {
                        base.FailMessage("管理员添加失败：此账号为系统关键字，请更换");
                        return;
                    }

                    bool isCreated = AdminManager.CreateAdministrator(adminInfo, out errorMessage);

                    if (isCreated)
                    {
                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加管理员", string.Format("管理员:{0}", this.tbUserName.Text.Trim()));

                        base.SuccessMessage("管理员添加成功！");

                        base.AddWaitAndRedirectScript(BackgroundAdministrator.GetRedirectUrl(TranslateUtils.ToInt(this.ddlDepartmentID.SelectedValue)));
                    }
                    else
                    {
                        base.FailMessage(string.Format("管理员添加失败：{0}", errorMessage));
                    }
                }
                else
                {
                    AdministratorInfo adminInfo = BaiRongDataProvider.AdministratorDAO.GetAdministratorInfo(this.userName);

                    adminInfo.DisplayName = this.tbDisplayName.Text;
                    adminInfo.Email = this.tbEmail.Text;
                    adminInfo.Mobile = this.tbMobile.Text;
                    adminInfo.DepartmentID = TranslateUtils.ToInt(this.ddlDepartmentID.SelectedValue);
                    adminInfo.AreaID = TranslateUtils.ToInt(this.ddlAreaID.SelectedValue);

                    BaiRongDataProvider.AdministratorDAO.Update(adminInfo);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改管理员属性", string.Format("管理员:{0}", this.tbUserName.Text.Trim()));

                    base.SuccessMessage("管理员属性设置成功！");
                    
                    base.AddWaitAndRedirectScript(BackgroundAdministrator.GetRedirectUrl(TranslateUtils.ToInt(this.ddlDepartmentID.SelectedValue)));
                }
            }
        }
	}
}
