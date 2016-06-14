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
using System.Web.UI.HtmlControls;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class AlbumContentAdd : BackgroundBasePage
    {
        public TextBox tbTitle;
        public Literal ltlImageUrl;
        public HtmlInputHidden imageUrl;

        private int id;
        private int albumID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int albumID, int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("albumID", albumID.ToString());
            arguments.Add("id", id.ToString());
            return PageUtilityWX.GetOpenWindowString("新建相册", "modal_albumContentAdd.aspx", arguments, 400, 450);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int albumID, int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("albumID", albumID.ToString());
            arguments.Add("id", id.ToString());
            return PageUtilityWX.GetOpenWindowString("编辑相册", "modal_albumContentAdd.aspx", arguments, 400, 450);
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.id = TranslateUtils.ToInt(base.GetQueryString("id"));
            this.albumID = TranslateUtils.ToInt(base.GetQueryString("albumID"));
            this.ltlImageUrl.Text = string.Format(@"<div style=""width:220px;""><img id=""preview_imageUrl"" class=""appmsg_thumb"" src=""{0}"" width=""220"" height=""220"" align=""middle"" /></div>", string.Empty);
            
            if (!IsPostBack)
            { 
                if (this.id > 0)
                {
                    AlbumContentInfo albumContentInfo = DataProviderWX.AlbumContentDAO.GetAlbumContentInfo(this.id);

                    this.tbTitle.Text = albumContentInfo.Title;
                    this.imageUrl.Value = albumContentInfo.LargeImageUrl;
                    this.ltlImageUrl.Text = string.Format(@"<div style=""width:220px;""><img id=""preview_imageUrl"" class=""appmsg_thumb"" src=""{0}"" width=""220"" height=""220"" align=""middle"" /></div>", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, albumContentInfo.LargeImageUrl));
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            try
            {
                if (this.id == 0)
                {
                    AlbumContentInfo albumContentInfo = new AlbumContentInfo();
                    albumContentInfo.PublishmentSystemID = base.PublishmentSystemID;
                    albumContentInfo.AlbumID = this.albumID;
                    albumContentInfo.ParentID = 0;
                    albumContentInfo.Title = this.tbTitle.Text;
                    albumContentInfo.LargeImageUrl = this.imageUrl.Value;

                    DataProviderWX.AlbumContentDAO.Insert(albumContentInfo);

                }
                else
                {
                    AlbumContentInfo albumContentInfo = DataProviderWX.AlbumContentDAO.GetAlbumContentInfo(this.id);
                    albumContentInfo.Title = this.tbTitle.Text;
                    albumContentInfo.LargeImageUrl = this.imageUrl.Value;

                    DataProviderWX.AlbumContentDAO.Update(albumContentInfo);

                }

                JsUtils.OpenWindow.CloseModalPage(Page);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "失败：" + ex.Message);
            }
        }
    }
}
