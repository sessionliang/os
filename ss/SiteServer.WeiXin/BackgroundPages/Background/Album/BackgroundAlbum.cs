using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundAlbum : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button btnDelete;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_album.aspx?publishmentSystemID={0}", publishmentSystemID));
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
                        DataProviderWX.AlbumDAO.Delete(base.PublishmentSystemID,list) ;
                        base.SuccessMessage("微相册删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "微相册删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.AlbumDAO.GetSelectString(base.PublishmentSystemID);
            this.spContents.SortField = AlbumAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
 
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Album, "微相册", AppManager.WeiXin.Permission.WebSite.Album);
                this.spContents.DataBind();

                string urlAdd = BackgroundAlbumAdd.GetRedirectUrl(base.PublishmentSystemID, 0);
                this.btnAdd.Attributes.Add("onclick", string.Format("location.href='{0}';return false", urlAdd));

                string urlDelete = PageUtils.AddQueryString(BackgroundAlbum.GetRedirectUrl(base.PublishmentSystemID), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的微相册", "此操作将删除所选微相册，确认吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                AlbumInfo albumInfo = new AlbumInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlKeywords = e.Item.FindControl("ltlKeywords") as Literal;
                Literal ltlPVCount = e.Item.FindControl("ltlPVCount") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlAlbumContentUrl = e.Item.FindControl("ltlAlbumContentUrl") as Literal;
                Literal ltlPreviewUrl = e.Item.FindControl("ltlPreviewUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlTitle.Text = albumInfo.Title;
                ltlKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(albumInfo.KeywordID);
                ltlPVCount.Text = albumInfo.PVCount.ToString();

                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(!albumInfo.IsDisabled);

                string urlAlbumContent = BackgroundAlbumContent.GetRedirectUrl(base.PublishmentSystemID, albumInfo.ID);

                ltlAlbumContentUrl.Text = string.Format(@"<a href=""{0}"">微相册</a>", urlAlbumContent);

                string urlPreview = AlbumManager.GetAlbumUrl(albumInfo, string.Empty);
                urlPreview = BackgroundPreview.GetRedirectUrlToMobile(urlPreview);
                ltlPreviewUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">预览</a>", urlPreview);

                string urlEdit = BackgroundAlbumAdd.GetRedirectUrl(base.PublishmentSystemID, albumInfo.ID);
                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", urlEdit);
            }
        }
    }
}
