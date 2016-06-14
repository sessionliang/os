using BaiRong.Core;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using StoreManager = SiteServer.WeiXin.Core.StoreManager;

using System.Collections.Generic;
using System.Collections;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundStoreItemAdd : BackgroundBasePageWX
    {
        public Literal ltlPageTitle;

        public TextBox tbStoreName;
        public TextBox tbStoreTel;
        public TextBox tbStoreMobile;
        public TextBox tbStoreAddress;
        public Literal ltlImageUrl;
        public TextBox tbSummary;
        public DropDownList ddlStoreCategoryName;

        public Button btnSubmit;
        public Button btnReturn;

        public HtmlInputHidden imageUrl;
        public HtmlInputHidden txtLongitude;
        public HtmlInputHidden txtLatitude;

        private int storeItemID;
        private int storeID;

        private bool[] isLastNodeArray;

        public static string GetRedirectUrl(int publishmentSystemID, int storeItemID, int storeID)
        {
            return PageUtils.GetWXUrl(string.Format("background_storeItemAdd.aspx?publishmentSystemID={0}&storeItemID={1}&storeID={2}", publishmentSystemID, storeItemID, storeID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }


        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.storeID = TranslateUtils.ToInt(base.GetQueryString("storeID"));
            this.storeItemID = TranslateUtils.ToInt(base.GetQueryString("storeItemID"));

            if (!IsPostBack)
            {
                string pageTitle = this.storeItemID > 0 ? "编辑微门店信息" : "添加微门店信息";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Store, pageTitle, AppManager.WeiXin.Permission.WebSite.Store);
                this.ltlPageTitle.Text = pageTitle;
                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""200"" height=""200"" align=""middle"" />", StoreManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));


                this.ddlStoreCategoryName.Items.Add(new ListItem("<请选择>", "0"));

                List<int> categoryIDList = DataProviderWX.StoreCategoryDAO.GetAllCategoryIDList(base.PublishmentSystemID);
                int count = categoryIDList.Count;
                if (count > 0)
                {
                    this.isLastNodeArray = new bool[count];
                    foreach (int theCategoryID in categoryIDList)
                    {
                        StoreCategoryInfo categoryInfo = DataProviderWX.StoreCategoryDAO.GetCategoryInfo(theCategoryID);
                        ListItem listitem = new ListItem(this.GetTitle(categoryInfo.ID, categoryInfo.CategoryName, categoryInfo.ParentsCount, categoryInfo.IsLastNode), theCategoryID.ToString());
                        this.ddlStoreCategoryName.Items.Add(listitem);
                    }
                }

                if (this.storeItemID > 0)
                {
                    StoreItemInfo storeItemInfo = DataProviderWX.StoreItemDAO.GetStoreItemInfo(this.storeItemID);

                    this.tbStoreName.Text = storeItemInfo.StoreName;
                    this.tbStoreTel.Text = storeItemInfo.Tel;
                    this.tbStoreMobile.Text = storeItemInfo.Mobile;
                    this.tbStoreAddress.Text = storeItemInfo.Address;
                    this.txtLatitude.Value = storeItemInfo.Latitude;
                    this.txtLongitude.Value = storeItemInfo.Longitude;
                    this.tbSummary.Text = storeItemInfo.Summary;

                    if (storeItemInfo.CategoryID > 0)
                    {
                        this.ddlStoreCategoryName.Items.FindByValue("" + storeItemInfo.CategoryID + "").Selected = true;
                    }

                    if (!string.IsNullOrEmpty(storeItemInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_storeImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, storeItemInfo.ImageUrl));
                    }

                    this.imageUrl.Value = storeItemInfo.ImageUrl;

                    this.storeID = storeItemInfo.StoreID;
                }

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundStoreItem.GetRedirectUrl(base.PublishmentSystemID, this.storeID)));
            }
        }
        public string GetTitle(int categoryID, string categoryName, int parentsCount, bool isLastNode)
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
            str = String.Concat(str, categoryName);
            return str;
        }
        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                string conflictKeywords = string.Empty;

                StoreItemInfo storeItemInfo = new StoreItemInfo();
                if (this.storeItemID > 0)
                {
                    storeItemInfo = DataProviderWX.StoreItemDAO.GetStoreItemInfo(this.storeItemID);
                }
                storeItemInfo.PublishmentSystemID = base.PublishmentSystemID;
                storeItemInfo.StoreID = this.storeID;
                storeItemInfo.CategoryID = Convert.ToInt32(this.ddlStoreCategoryName.SelectedValue);
                storeItemInfo.StoreName = this.tbStoreName.Text;
                storeItemInfo.Tel = this.tbStoreTel.Text;
                storeItemInfo.Mobile = this.tbStoreMobile.Text;
                storeItemInfo.Address = this.tbStoreAddress.Text;
                storeItemInfo.ImageUrl = this.imageUrl.Value;
                storeItemInfo.Latitude = this.txtLatitude.Value;
                storeItemInfo.Longitude = this.txtLongitude.Value;
                storeItemInfo.Summary = this.tbSummary.Text;

                try
                {
                    if (this.storeItemID > 0)
                    {
                        DataProviderWX.StoreItemDAO.Update(base.PublishmentSystemID, storeItemInfo);

                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改微门店信息", string.Format("微门店:{0}", this.tbStoreName.Text));
                        base.SuccessMessage("修改微门店成功！");
                    }
                    else
                    {
                        this.storeItemID = DataProviderWX.StoreItemDAO.Insert(base.PublishmentSystemID, storeItemInfo);
                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加微门店信息", string.Format("微门店:{0}", this.tbStoreName.Text));
                        base.SuccessMessage("添加微门店成功！");
                    }

                    string redirectUrl = PageUtils.GetWXUrl(string.Format("background_storeItem.aspx?publishmentSystemID={0}&storeID={1}", base.PublishmentSystemID, this.storeID));
                    base.AddWaitAndRedirectScript(redirectUrl);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "微门店设置失败！");
                }

            }
        }
    }
}
