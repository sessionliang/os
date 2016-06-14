using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;


using SiteServer.CMS.Core;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WCM.Core;

namespace SiteServer.WCM.BackgroundPages.Modal
{
	public class GovPublicDepartmentAdd : BackgroundBasePage
	{
        public TextBox DepartmentName;
        public TextBox Code;
        public DropDownList ParentID;
        public TextBox Summary;

        private string returnUrl = string.Empty;
        private bool[] isLastNodeArray;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtilityWCM.GetOpenWindowString("添加部门", "modal_govPublicDepartmentAdd.aspx", arguments, 460, 360);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = PageUtils.GetWCMUrl(string.Format("background_govPublicDepartment.aspx", base.PublishmentSystemID));
            }

			if (!IsPostBack)
			{
                this.ParentID.Items.Add(new ListItem("<无上级部门>", "0"));

                ArrayList departmentIDArrayList = GovPublicManager.GetAllDepartmentIDArrayList(base.PublishmentSystemInfo);
                int count = departmentIDArrayList.Count;
                this.isLastNodeArray = new bool[count];
                foreach (int theDepartmentID in departmentIDArrayList)
                {
                    DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(theDepartmentID);
                    ListItem listitem = new ListItem(this.GetTitle(departmentInfo.DepartmentID, departmentInfo.DepartmentName, departmentInfo.ParentsCount, departmentInfo.IsLastNode), theDepartmentID.ToString());
                    this.ParentID.Items.Add(listitem);
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

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                DepartmentInfo departmentInfo = new DepartmentInfo();
                departmentInfo.DepartmentName = this.DepartmentName.Text;
                departmentInfo.Code = this.Code.Text;
                departmentInfo.ParentID = TranslateUtils.ToInt(this.ParentID.SelectedValue);
                departmentInfo.Summary = this.Summary.Text;

                BaiRongDataProvider.DepartmentDAO.Insert(departmentInfo);

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
