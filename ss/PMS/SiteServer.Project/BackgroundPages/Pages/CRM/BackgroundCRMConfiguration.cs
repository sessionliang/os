using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Collections;
using System.Web.UI.HtmlControls;


namespace SiteServer.Project.BackgroundPages
{
	public class BackgroundCRMConfiguration : BackgroundBasePage
	{
        public ListBox lbDepartmentID;
        public ListBox lbRequestDepartmentID;
        public TextBox tbRequestType;

        private bool[] isLastNodeArrayOfDepartment;

		public void Page_Load(object sender, EventArgs E)
		{
			if (!IsPostBack)
			{
                ArrayList departmentIDArrayList = DepartmentManager.GetDepartmentIDArrayList();
                int count = departmentIDArrayList.Count;
                this.isLastNodeArrayOfDepartment = new bool[count];
                foreach (int departmentID in departmentIDArrayList)
                {
                    DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);
                    ListItem listitem = new ListItem(this.GetDepartment(departmentInfo.DepartmentID, departmentInfo.DepartmentName, departmentInfo.ParentsCount, departmentInfo.IsLastNode), departmentID.ToString());
                    this.lbDepartmentID.Items.Add(listitem);
                }
                foreach (int departmentID in departmentIDArrayList)
                {
                    DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);
                    ListItem listitem = new ListItem(this.GetDepartment(departmentInfo.DepartmentID, departmentInfo.DepartmentName, departmentInfo.ParentsCount, departmentInfo.IsLastNode), departmentID.ToString());
                    this.lbRequestDepartmentID.Items.Add(listitem);
                }

                ControlUtils.SelectListItems(this.lbDepartmentID, TranslateUtils.ObjectCollectionToString(ConfigurationManager.Additional.CRMDepartmentIDCollection).Split(','));
                ControlUtils.SelectListItems(this.lbRequestDepartmentID, TranslateUtils.ObjectCollectionToString(ConfigurationManager.Additional.CRMRequestDepartmentIDCollection).Split(','));

                this.tbRequestType.Text = TranslateUtils.ObjectCollectionToString(ConfigurationManager.Additional.CRMRequestTypeCollection);
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

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{				
				try
				{
                    ConfigurationManager.Additional.CRMDepartmentIDCollection = ControlUtils.GetSelectedListControlValueIntList(this.lbDepartmentID);
                    ConfigurationManager.Additional.CRMRequestDepartmentIDCollection = ControlUtils.GetSelectedListControlValueIntList(this.lbRequestDepartmentID);
                    ConfigurationManager.Additional.CRMRequestTypeCollection = TranslateUtils.StringCollectionToStringList(this.tbRequestType.Text);

                    DataProvider.ConfigurationDAO.Update(ConfigurationManager.Instance);

                    base.SuccessMessage("客户设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "客户设置修改失败！");
				}
			}
		}
	}
}
