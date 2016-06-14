using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundStoreItem : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button btnDelete;
        public Button btnReturn;

        private int storeID;
        private int storeItemID;

        public static string GetRedirectUrl(int publishmentSystemID, int storeID)
        {
            return PageUtils.GetWXUrl(string.Format("background_StoreItem.aspx?publishmentSystemID={0}&storeID={1}", publishmentSystemID, storeID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.storeID = TranslateUtils.ToInt(base.GetQueryString("storeID"));

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.StoreItemDAO.Delete(base.PublishmentSystemID, list);
                        base.SuccessMessage("门店信息删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "门店信息删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.StoreItemDAO.GetSelectString(this.storeID);
            this.spContents.SortField = StoreItemAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            { 
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Store, "微门店信息管理", AppManager.WeiXin.Permission.WebSite.Store);
                this.spContents.DataBind();


                string urlAdd = BackgroundStoreItemAdd.GetRedirectUrl(base.PublishmentSystemID, this.storeItemID,this.storeID);
                this.btnAdd.Attributes.Add("onclick", string.Format("location.href='{0}';return false", urlAdd));
                

                string urlDelete = PageUtils.AddQueryString(BackgroundStoreItem.GetRedirectUrl(base.PublishmentSystemID, this.storeID), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的微门店信息", "此操作将删除所选微门店信息，确认吗？"));

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundStore.GetRedirectUrl(base.PublishmentSystemID)));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                StoreItemInfo storeItemInfo = new StoreItemInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlStoreName = e.Item.FindControl("ltlStoreName") as Literal;
                Literal ltlStoreCategoryName = e.Item.FindControl("ltlStoreCategoryName") as Literal;
                Literal ltlTel = e.Item.FindControl("ltlTel") as Literal;
                Literal ltlMobile = e.Item.FindControl("ltlMobile") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlStoreName.Text = storeItemInfo.StoreName;

                if (storeItemInfo.CategoryID > 0)
                {
                    ltlStoreCategoryName.Text = DataProviderWX.StoreCategoryDAO.GetStoreCategoryInfo(storeItemInfo.CategoryID).CategoryName.ToString();
                }
                else
                {
                    ltlStoreCategoryName.Text = "";
                }
                ltlTel.Text = storeItemInfo.Tel;
                ltlMobile.Text = storeItemInfo.Mobile;
                string urlEdit = BackgroundStoreItemAdd.GetRedirectUrl(base.PublishmentSystemID, storeItemInfo.ID, this.storeID);
                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", urlEdit);
            }
        }
    }
}
