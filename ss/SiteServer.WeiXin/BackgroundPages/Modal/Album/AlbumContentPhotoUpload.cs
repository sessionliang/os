using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.Text;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;
using System.Collections.Generic;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class AlbumContentPhotoUpload : BackgroundBasePage
    {
        public Literal ltlScript;

        private int albumID;
        private int parentID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int albumID, int parentID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("albumID", albumID.ToString());
            arguments.Add("parentID", parentID.ToString());
            return PageUtilityWX.GetOpenWindowString("ÉÏ´«ÕÕÆ¬", "modal_albumContentPhotoUpload.aspx", arguments);
        }

        public string GetContentPhotoUploadMultipleUrl()
        {
            return BackgroundAjaxUpload.GetContentPhotoUploadMultipleUrl(base.PublishmentSystemID);
        }

        public string GetContentPhotoUploadSingleUrl()
        {
            return BackgroundAjaxUpload.GetContentPhotoUploadSingleUrl(base.PublishmentSystemID);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.albumID = TranslateUtils.ToInt(base.GetQueryString("albumID"));
            this.parentID = TranslateUtils.ToInt(Request.QueryString["parentID"]);

            if (!IsPostBack)
            {
                List<AlbumContentInfo> list = new List<AlbumContentInfo>();
                if (this.parentID > 0)
                {
                    list = DataProviderWX.AlbumContentDAO.GetAlbumContentInfoList(base.PublishmentSystemID, this.albumID, this.parentID);
                }

                StringBuilder scriptBuilder = new StringBuilder();

                foreach (AlbumContentInfo albumContentInfo in list)
                {
                    scriptBuilder.AppendFormat(@"
add_form({0}, '{1}', '{2}', '{3}', '{4}');
", albumContentInfo.ID, StringUtils.ToJsString(PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, albumContentInfo.ImageUrl)), StringUtils.ToJsString(albumContentInfo.ImageUrl), StringUtils.ToJsString(albumContentInfo.LargeImageUrl), albumContentInfo.Title);
                }

                this.ltlScript.Text = string.Format(@"
$(document).ready(function(){{
	{0}
}});
", scriptBuilder.ToString());
            }
        }

        public string GetPreviewImageSize()
        {
            return string.Format(@"width=""{0}""", 200);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                List<int> albumContentIDList = new List<int>();
                if (this.parentID > 0)
                {
                    albumContentIDList = DataProviderWX.AlbumContentDAO.GetAlbumContentIDList(base.PublishmentSystemID, this.albumID, this.parentID);
                }
                int photo_Count = TranslateUtils.ToInt(base.Request.Form["Photo_Count"]);
                if (photo_Count > 0)
                {
                    for (int index = 1; index <= photo_Count; index++)
                    {
                        int id = TranslateUtils.ToInt(base.Request.Form["ID_" + index]);
                        string smallUrl = base.Request.Form["SmallUrl_" + index];
                        string largeUrl = base.Request.Form["LargeUrl_" + index];
                        string title = base.Request.Form["imgTitle_" + index];

                        if (!string.IsNullOrEmpty(smallUrl) && !string.IsNullOrEmpty(largeUrl))
                        {
                            if (id > 0)
                            {
                                AlbumContentInfo albumContentInfo = DataProviderWX.AlbumContentDAO.GetAlbumContentInfo(id);
                                if (albumContentInfo != null)
                                {
                                    albumContentInfo.ImageUrl = smallUrl;
                                    albumContentInfo.LargeImageUrl = largeUrl;
                                    albumContentInfo.Title = title;

                                    DataProviderWX.AlbumContentDAO.Update(albumContentInfo);
                                }
                                albumContentIDList.Remove(id);
                            }
                            else
                            {
                                AlbumContentInfo albumContentInfo = new AlbumContentInfo();
                                albumContentInfo.PublishmentSystemID = base.PublishmentSystemID;
                                albumContentInfo.AlbumID = this.albumID;
                                albumContentInfo.ParentID = this.parentID;
                                albumContentInfo.ImageUrl = smallUrl;
                                albumContentInfo.LargeImageUrl = largeUrl;
                                albumContentInfo.Title = title;

                                DataProviderWX.AlbumContentDAO.Insert(albumContentInfo);
                            }
                        }
                    }
                }

                if (albumContentIDList.Count > 0)
                {
                    DataProviderWX.AlbumContentDAO.Delete(base.PublishmentSystemID, albumContentIDList);
                }

                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
