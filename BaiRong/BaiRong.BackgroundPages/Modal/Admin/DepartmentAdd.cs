using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;


using BaiRong.Core.Data.Provider;

namespace BaiRong.BackgroundPages.Modal
{
	public class DepartmentAdd : BackgroundBasePage
	{
        public TextBox DepartmentName;
        public TextBox Code;
        public PlaceHolder phParentID;
        public DropDownList ParentID;
        public TextBox Summary;

        private int departmentID = 0;
        private string returnUrl = string.Empty;
        private bool[] isLastNodeArray;

        public static string GetOpenWindowStringToAdd(string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtilityPF.GetOpenWindowString("添加部门", "modal_departmentAdd.aspx", arguments, 460, 380);
        }

        public static string GetOpenWindowStringToEdit(int departmentID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("DepartmentID", departmentID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtilityPF.GetOpenWindowString("修改部门", "modal_departmentAdd.aspx", arguments, 460, 380);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.departmentID = base.GetIntQueryString("DepartmentID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = PageUtils.GetPlatformUrl("background_department.aspx");
            }

			if (!IsPostBack)
			{
                if (this.departmentID == 0)
                {
                    this.ParentID.Items.Add(new ListItem("<无上级部门>", "0"));

                    ArrayList departmentIDArrayList = DepartmentManager.GetDepartmentIDArrayList();
                    int count = departmentIDArrayList.Count;
                    this.isLastNodeArray = new bool[count];
                    foreach (int theDepartmentID in departmentIDArrayList)
                    {
                        DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(theDepartmentID);
                        ListItem listitem = new ListItem(this.GetTitle(departmentInfo.DepartmentID, departmentInfo.DepartmentName, departmentInfo.ParentsCount, departmentInfo.IsLastNode), theDepartmentID.ToString());
                        this.ParentID.Items.Add(listitem);
                    }
                }
                else
                {
                    this.phParentID.Visible = false;
                }

                if (this.departmentID != 0)
                {
                    DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(this.departmentID);

                    this.DepartmentName.Text = departmentInfo.DepartmentName;
                    this.Code.Text = departmentInfo.Code;
                    this.ParentID.SelectedValue = departmentInfo.ParentID.ToString();
                    this.Summary.Text = departmentInfo.Summary;
                }
			}
		}

        public string GetTitle(int departmentID, string departmentName, int parentsCount, bool isLastNode)
        {
            string str = "";
            if (isLastNode == false)
            {
                this.isLastNodeArray[parentsCount] = false;
            }
            else
            {
                this.isLastNodeArray[parentsCount] = true;
            }
            for (int i = 0; i < parentsCount; i++)
            {
                if (this.isLastNodeArray[i])
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

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {
                if (this.departmentID == 0)
                {
                    DepartmentInfo departmentInfo = new DepartmentInfo();
                    departmentInfo.DepartmentName = this.DepartmentName.Text;
                    departmentInfo.Code = this.Code.Text;
                    departmentInfo.ParentID = TranslateUtils.ToInt(this.ParentID.SelectedValue);
                    departmentInfo.Summary = this.Summary.Text;

                    BaiRongDataProvider.DepartmentDAO.Insert(departmentInfo);
                }
                else
                {
                    DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(this.departmentID);

                    departmentInfo.DepartmentName = this.DepartmentName.Text;
                    departmentInfo.Code = this.Code.Text;
                    departmentInfo.ParentID = TranslateUtils.ToInt(this.ParentID.SelectedValue);
                    departmentInfo.Summary = this.Summary.Text;

                    BaiRongDataProvider.DepartmentDAO.Update(departmentInfo);
                }

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "维护部门信息");

                base.SuccessMessage("部门设置成功！");
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "部门设置失败！");
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
            }
        }
	}
}
