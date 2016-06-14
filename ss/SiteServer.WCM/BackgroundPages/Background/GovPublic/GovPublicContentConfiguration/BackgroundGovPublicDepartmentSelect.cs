using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;



namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicDepartmentSelect : BackgroundGovPublicBasePage
	{
		public Literal ltlDepartmentTree;
		
		private string GetDepartmentTreeHtml()
		{
			StringBuilder htmlBuilder = new StringBuilder();
            ArrayList departmentIDArrayList = GovPublicManager.GetFirstDepartmentIDArrayList(base.PublishmentSystemInfo);

            string treeDirectoryUrl = PageUtils.GetIconUrl("tree");

            ArrayList theDepartmentIDArrayList = DepartmentManager.GetDepartmentIDArrayList();
            bool[] IsLastNodeArray = new bool[theDepartmentIDArrayList.Count];
            foreach (int theDepartmentID in theDepartmentIDArrayList)
			{
                DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(theDepartmentID);
                htmlBuilder.Append(GetTitle(departmentInfo, treeDirectoryUrl, IsLastNodeArray, departmentIDArrayList));
				htmlBuilder.Append("<br/>");
			}
			return htmlBuilder.ToString();
		}

        private string GetTitle(DepartmentInfo departmentInfo, string treeDirectoryUrl, bool[] IsLastNodeArray, IList departmentIDArrayList)
		{
            StringBuilder itemBuilder = new StringBuilder();
            if (departmentInfo.IsLastNode == false)
			{
                IsLastNodeArray[departmentInfo.ParentsCount] = false;
			}
			else
			{
                IsLastNodeArray[departmentInfo.ParentsCount] = true;
			}
            for (int i = 0; i < departmentInfo.ParentsCount; i++)
			{
				if (IsLastNodeArray[i])
				{
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_empty.gif\"/>", treeDirectoryUrl);
				}
				else
				{
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_line.gif\"/>", treeDirectoryUrl);
				}
			}
            if (departmentInfo.IsLastNode)
			{
                if (departmentInfo.ChildrenCount > 0)
				{
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_plusbottom.gif\"/>", treeDirectoryUrl);
				}
				else
				{
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_minusbottom.gif\"/>", treeDirectoryUrl);
				}
			}
			else
			{
                if (departmentInfo.ChildrenCount > 0)
				{
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_plusmiddle.gif\"/>", treeDirectoryUrl);
				}
				else
				{
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_minusmiddle.gif\"/>", treeDirectoryUrl);
				}
			}

			string check = "";
            if (departmentIDArrayList.Contains(departmentInfo.DepartmentID))
			{
				check = "checked";
			}

            if (departmentInfo.ParentsCount == 0)
            {
                itemBuilder.AppendFormat(@"<label class=""checkbox inline""><input type=""checkbox"" name=""DepartmentIDCollection"" value=""{0}"" {1} /> {2}</label>", departmentInfo.DepartmentID, check, departmentInfo.DepartmentName);
            }
            else
            {
                itemBuilder.Append(departmentInfo.DepartmentName);
            }

            return itemBuilder.ToString();
		}

		public void Page_Load(object sender, EventArgs E)
		{
			if (!base.IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicContentConfiguration, "可选择部门设置", AppManager.CMS.Permission.WebSite.GovPublicContentConfiguration);

                this.ltlDepartmentTree.Text = GetDepartmentTreeHtml();
			}
		}

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.GovPublicDepartmentIDCollection = Request.Form["DepartmentIDCollection"];
                DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                base.SuccessMessage("可选择部门设置成功");
                base.AddWaitAndRedirectScript(PageUtils.GetWCMUrl(string.Format("background_govPublicDepartmentSelect.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
			}
		}
	}
}
