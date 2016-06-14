using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Web.UI;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.CMS.Core;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;
using BaiRong.Model;
using System.Collections;
  
namespace SiteServer.WeiXin.BackgroundPages
{
	public class BackgroundAlbumContent : BackgroundBasePageWX
	{
        public Repeater rptParentAlbumContents;
         
        public Button btnAddAlbumContent;

        public int albumID;

        public static string GetRedirectUrl(int publishmentSystemID,int albumID)
        {
            return PageUtils.GetWXUrl(string.Format("background_albumContent.aspx?publishmentSystemID={0}&albumID={1}", publishmentSystemID,albumID));
        }

        public static string GetRedirectUrl(int publishmentSystemID, int albumID,int id)
        {
            return PageUtils.GetWXUrl(string.Format("background_albumContent.aspx?publishmentSystemID={0}&albumID={1}& id={2}", publishmentSystemID, albumID,id));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.albumID = TranslateUtils.ToInt(base.GetQueryString("albumID"));

			if (base.Request.QueryString["delete"] != null)
			{
                int id = TranslateUtils.ToInt(base.Request.QueryString["id"]);
			
				try
				{
                    DataProviderWX.AlbumContentDAO.Delete(base.PublishmentSystemID, id);
                    DataProviderWX.AlbumContentDAO.DeleteByParentID(base.PublishmentSystemID,id);
					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
                    base.FailDeleteMessage(ex);
				}
			}

			if (!IsPostBack)
            { 
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Album, "相册管理", AppManager.WeiXin.Permission.WebSite.Album);
                this.rptParentAlbumContents.DataSource = DataProviderWX.AlbumContentDAO.GetDataSource(base.PublishmentSystemID, this.albumID);
                this.rptParentAlbumContents.ItemDataBound += new RepeaterItemEventHandler(rptParentContents_ItemDataBound);
                this.rptParentAlbumContents.DataBind();

                this.btnAddAlbumContent.Attributes.Add("onclick", Modal.AlbumContentAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID,this.albumID,0));
                
			}
		}

        void rptParentContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int id = TranslateUtils.EvalInt(e.Item.DataItem, "ID");
                int albumID = TranslateUtils.EvalInt(e.Item.DataItem, "AlbumID");
                int parentID = TranslateUtils.EvalInt(e.Item.DataItem, "ParentID");
                string title = TranslateUtils.EvalString(e.Item.DataItem, "Title");
                string imageUrl = TranslateUtils.EvalString(e.Item.DataItem, "ImageUrl");
                string largeImageUrl = TranslateUtils.EvalString(e.Item.DataItem, "LargeImageUrl");

                AlbumInfo albumInfo = DataProviderWX.AlbumDAO.GetAlbumInfo(albumID);
                KeywordInfo keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(albumInfo.KeywordID);
                int count=DataProviderWX.AlbumContentDAO.GetCount(base.PublishmentSystemID,id);

                Literal ltlkeywrods = e.Item.FindControl("ltlkeywrods") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlLargeImageUrl = e.Item.FindControl("ltlLargeImageUrl") as Literal;

                DataList dlAlbumContents = e.Item.FindControl("dlAlbumContents") as DataList;


                Literal ltlAddUrl = e.Item.FindControl("ltlAddUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlkeywrods.Text = string.Format(@" <a href=""javascript:;"" onclick=""{0}"">编辑相册</a>", Modal.AlbumContentAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, this.albumID, id));
                ltlAddDate.Text =DateUtils.GetDateString(keywordInfo.AddDate);

                ltlTitle.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}&nbsp;({2})</a>", "javascript:;", title, count);
                ltlLargeImageUrl.Text = string.Format(@"<img src=""{0}"" class=""appmsg_thumb"">", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, largeImageUrl));


                dlAlbumContents.DataSource = DataProviderWX.AlbumContentDAO.GetDataSource(base.PublishmentSystemID, this.albumID, id);
                dlAlbumContents.ItemDataBound +=dlContents_ItemDataBound;
                dlAlbumContents.DataBind();


                ltlAddUrl.Text = string.Format(@"<a class=""js_edit"" href=""javascript:;"" onclick=""{0}""><i class=""icon18_common edit_gray"">上传照片</i></a>", Modal.AlbumContentPhotoUpload.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.albumID, id));
                
                ltlDeleteUrl.Text = string.Format(@"<a class=""js_delno_extra"" href=""{0}&delete=true&id={1}"" onclick=""javascript:return confirm('此操作将删除相册“{2}”，确认吗？');""><i class=""icon18_common del_gray"">删除</i></a>", BackgroundAlbumContent.GetRedirectUrl(base.PublishmentSystemID, this.albumID),id,title);

            }
        }

        void dlContents_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                string imageUrl = TranslateUtils.EvalString(e.Item.DataItem, "ImageUrl");
                string largeImageUrl = TranslateUtils.EvalString(e.Item.DataItem, "LargeImageUrl");

                TextBox tbContentImageUrl = e.Item.FindControl("tbContentImageUrl") as TextBox;
                Literal ltlImageUrl = e.Item.FindControl("ltlImageUrl") as Literal;

                if (string.IsNullOrEmpty(imageUrl))
                {
                    ltlImageUrl.Text = @"<i class=""appmsg_thumb default"">缩略图</i>";
                }
                else
                {
                    string previewImageClick = SiteServer.CMS.BackgroundPages.Modal.Message.GetOpenWindowStringToPreviewImage(base.PublishmentSystemID, tbContentImageUrl.ClientID);
                    tbContentImageUrl.Text = PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, largeImageUrl);
                    ltlImageUrl.Text = string.Format(@"<a  href=""javascript:;"" onclick=""{0}""> <img class=""img-rounded"" style=""width:80px;"" src=""{1}""> </a>" ,previewImageClick, PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, imageUrl));
                }
            }
        }        
	}
}
