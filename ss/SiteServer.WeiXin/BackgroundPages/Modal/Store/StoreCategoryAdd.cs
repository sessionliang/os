using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Net;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;

using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using SiteServer.CMS.Core;
using System.Collections.Generic;


namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class StoreCategoryAdd : BackgroundBasePage
    {
        public TextBox CategoryName;
        public PlaceHolder phParentID;
        public DropDownList ParentID;

        private int categoryID = 0;
        private bool[] isLastNodeArray;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityWX.GetOpenWindowString("添加门店属性", "modal_storeCategoryAdd.aspx", arguments, 400, 300);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int storeCategoryID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("storeCategoryID", storeCategoryID.ToString());
            return PageUtilityWX.GetOpenWindowString("编辑门店属性", "modal_storeCategoryAdd.aspx", arguments, 400, 300);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.categoryID = TranslateUtils.ToInt(Request.QueryString["storeCategoryID"]);

            if (!IsPostBack)
            {
                if (this.categoryID == 0)
                {
                    this.ParentID.Items.Add(new ListItem("<无上级区域>", "0"));

                    List<int> categoryIDList = DataProviderWX.StoreCategoryDAO.GetAllCategoryIDList(base.PublishmentSystemID);
                    int count = categoryIDList.Count;
                    if (count > 0)
                    {
                        this.isLastNodeArray = new bool[count];
                        foreach (int theCategoryID in categoryIDList)
                        {
                            StoreCategoryInfo categoryInfo = DataProviderWX.StoreCategoryDAO.GetCategoryInfo(theCategoryID);
                            ListItem listitem = new ListItem(this.GetTitle(categoryInfo.ID, categoryInfo.CategoryName, categoryInfo.ParentsCount, categoryInfo.IsLastNode), theCategoryID.ToString());
                            this.ParentID.Items.Add(listitem);
                        }
                    }
                }
                else
                {
                    this.phParentID.Visible = false;
                }

                if (this.categoryID != 0)
                {
                    StoreCategoryInfo categoryInfo = DataProviderWX.StoreCategoryDAO.GetCategoryInfo(this.categoryID);
                    this.CategoryName.Text = categoryInfo.CategoryName;
                    this.ParentID.SelectedValue = categoryInfo.ParentID.ToString();
                }
            }
        }

        public string GetTitle(int categoryID, string categoryName, int parentsCount, bool isLastNode)
        {
            string str = "";
            if (this.isLastNodeArray.Length > parentsCount)
            {
                if (isLastNode == false)
                {
                    this.isLastNodeArray[parentsCount] = false;
                }
                else
                {
                    this.isLastNodeArray[parentsCount] = true;
                }
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
            str = String.Concat(str, categoryName);
            return str;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {
                if (this.categoryID == 0)
                {
                    StoreCategoryInfo categoryInfo = new StoreCategoryInfo();
                    categoryInfo.PublishmentSystemID = base.PublishmentSystemID;
                    categoryInfo.CategoryName = this.CategoryName.Text;
                    categoryInfo.ParentID = TranslateUtils.ToInt(this.ParentID.SelectedValue);

                    DataProviderWX.StoreCategoryDAO.Insert(base.PublishmentSystemID, categoryInfo);
                }
                else
                {
                    StoreCategoryInfo categoryInfo = DataProviderWX.StoreCategoryDAO.GetCategoryInfo(this.categoryID);
                    categoryInfo.CategoryName = this.CategoryName.Text;
                    categoryInfo.ParentID = TranslateUtils.ToInt(this.ParentID.SelectedValue);

                    DataProviderWX.StoreCategoryDAO.Update(base.PublishmentSystemID, categoryInfo); ;
                }

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "维护门店属性信息");

                base.SuccessMessage("类别设置成功！");
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "类别设置失败！");
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
