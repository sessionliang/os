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
    public class BackgroundStore : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button btnDelete;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_store.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.StoreDAO.Delete(base.PublishmentSystemID, list);
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
            this.spContents.SelectCommand = DataProviderWX.StoreDAO.GetSelectString(base.PublishmentSystemID);
            this.spContents.SortField = StoreAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            { 
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Store, "门店管理", AppManager.WeiXin.Permission.WebSite.Store);
                this.spContents.DataBind();

                string urlAdd = BackgroundStoreAdd.GetRedirectUrl(base.PublishmentSystemID, 0);
                this.btnAdd.Attributes.Add("onclick", string.Format("location.href='{0}';return false", urlAdd));

                string urlDelete = PageUtils.AddQueryString(BackgroundStore.GetRedirectUrl(base.PublishmentSystemID), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的门店信息", "此操作将删除所选门店信息，确认吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                StoreInfo storeInfo = new StoreInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlKeywords = e.Item.FindControl("ltlKeywords") as Literal;
                Literal ltlPVCount = e.Item.FindControl("ltlPVCount") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlStoreContentUrl = e.Item.FindControl("ltlStoreContentUrl") as Literal;
                Literal ltlPreviewUrl = e.Item.FindControl("ltlPreviewUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlTitle.Text = storeInfo.Title;
                ltlKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(storeInfo.KeywordID);
                ltlPVCount.Text = storeInfo.PVCount.ToString();

                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(!storeInfo.IsDisabled);

                string urlStoreContent = BackgroundStoreItem.GetRedirectUrl(base.PublishmentSystemID, storeInfo.ID);
                 
                ltlStoreContentUrl.Text = string.Format(@"<a href=""{0}"">微门店</a>", urlStoreContent);

                string urlPreview = StoreManager.GetStoreUrl(storeInfo, string.Empty);
                urlPreview = BackgroundPreview.GetRedirectUrlToMobile(urlPreview);
                ltlPreviewUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">预览</a>", urlPreview);

                string urlEdit = BackgroundStoreAdd.GetRedirectUrl(base.PublishmentSystemID, storeInfo.ID);
                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", urlEdit);
            }
        }
         
    }
}
