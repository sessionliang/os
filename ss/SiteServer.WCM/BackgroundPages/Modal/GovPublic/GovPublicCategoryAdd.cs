using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WCM.Core;

namespace SiteServer.WCM.BackgroundPages.Modal
{
	public class GovPublicCategoryAdd : BackgroundBasePage
	{
        public TextBox CategoryName;
        public TextBox CategoryCode;
        public PlaceHolder phParentID;
        public DropDownList ParentID;
        public TextBox Summary;

        private string classCode = string.Empty;
        private int categoryID = 0;
        private string returnUrl = string.Empty;
        private bool[] isLastNodeArray;

        public static string GetOpenWindowStringToAdd(string classCode, int publishmentSystemID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ClassCode", classCode);
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtilityWCM.GetOpenWindowString("添加节点", "modal_govPublicCategoryAdd.aspx", arguments, 500, 340);
        }

        public static string GetOpenWindowStringToEdit(string classCode, int publishmentSystemID, int categoryID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ClassCode", classCode);
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("CategoryID", categoryID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtilityWCM.GetOpenWindowString("修改节点", "modal_govPublicCategoryAdd.aspx", arguments, 520, 320);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.classCode = base.Request.QueryString["ClassCode"];
            this.categoryID = TranslateUtils.ToInt(Request.QueryString["CategoryID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = PageUtils.GetWCMUrl("background_govPublicCategory.aspx");
            }

			if (!IsPostBack)
			{
                if (this.categoryID == 0)
                {
                    this.ParentID.Items.Add(new ListItem("<无上级节点>", "0"));

                    ArrayList categoryIDArrayList = DataProvider.GovPublicCategoryDAO.GetCategoryIDArrayList(this.classCode, base.PublishmentSystemID);
                    int count = categoryIDArrayList.Count;
                    this.isLastNodeArray = new bool[count];
                    foreach (int theCategoryID in categoryIDArrayList)
                    {
                        GovPublicCategoryInfo categoryInfo = DataProvider.GovPublicCategoryDAO.GetCategoryInfo(theCategoryID);
                        ListItem listitem = new ListItem(this.GetTitle(categoryInfo.CategoryID, categoryInfo.CategoryName, categoryInfo.ParentsCount, categoryInfo.IsLastNode), theCategoryID.ToString());
                        this.ParentID.Items.Add(listitem);
                    }
                }
                else
                {
                    this.phParentID.Visible = false;
                }

                if (this.categoryID != 0)
                {
                    GovPublicCategoryInfo categoryInfo = DataProvider.GovPublicCategoryDAO.GetCategoryInfo(this.categoryID);

                    this.CategoryName.Text = categoryInfo.CategoryName;
                    this.CategoryCode.Text = categoryInfo.CategoryCode;
                    this.ParentID.SelectedValue = categoryInfo.ParentID.ToString();
                    this.Summary.Text = categoryInfo.Summary;
                }

				
			}
		}

        public string GetTitle(int categoryID, string departmentName, int parentsCount, bool isLastNode)
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
                if (this.categoryID == 0)
                {
                    GovPublicCategoryInfo categoryInfo = new GovPublicCategoryInfo(0, this.classCode, base.PublishmentSystemID, this.CategoryName.Text, this.CategoryCode.Text, TranslateUtils.ToInt(this.ParentID.SelectedValue), string.Empty, 0, 0, false, 0, DateTime.Now, this.Summary.Text, 0);

                    DataProvider.GovPublicCategoryDAO.Insert(categoryInfo);
                }
                else
                {
                    GovPublicCategoryInfo categoryInfo = DataProvider.GovPublicCategoryDAO.GetCategoryInfo(this.categoryID);

                    categoryInfo.CategoryName = this.CategoryName.Text;
                    categoryInfo.CategoryCode = this.CategoryCode.Text;
                    categoryInfo.Summary = this.Summary.Text;

                    DataProvider.GovPublicCategoryDAO.Update(categoryInfo);
                }

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "维护分类信息");

                base.SuccessMessage("分类设置成功！");
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "分类设置失败！");
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
            }
        }
	}
}
