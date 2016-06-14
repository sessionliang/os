using System.Web.UI;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using SiteServer.WeiXin.Model;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using Senparc.Weixin.MP.Entities;

namespace SiteServer.WeiXin.Core
{
	public class AlbumManager
    {
        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/album/img/start.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetContentImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
        }

        private static string GetAlbumUrl(AlbumInfo albumInfo)
        {
            return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/album/index.html")));
        }

        public static string GetAlbumUrl(AlbumInfo albumInfo, string wxOpenID)
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("publishmentSystemID", albumInfo.PublishmentSystemID.ToString());
            attributes.Add("albumID", albumInfo.ID.ToString());
            attributes.Add("wxOpenID", wxOpenID);
            return PageUtils.AddQueryString(GetAlbumUrl(albumInfo), attributes);
        }

        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, string wxOpenID)
        {
            List<Article> articleList = new List<Article>();

            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<AlbumInfo> albumInfoList = DataProviderWX.AlbumDAO.GetAlbumInfoListByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (AlbumInfo albumInfo in albumInfoList)
            {
                Article article = null;

                if (albumInfo != null)
                {
                    string imageUrl = AlbumManager.GetImageUrl(publishmentSystemInfo, albumInfo.ImageUrl);
                    string pageUrl = AlbumManager.GetAlbumUrl(albumInfo, wxOpenID);

                    article = new Article()
                    {
                        Title = albumInfo.Title,
                        Description = albumInfo.Summary,
                        PicUrl = imageUrl,
                        Url = pageUrl
                    };
                }

                if (article != null)
                {
                    articleList.Add(article);
                }
            }

            return articleList;
        }
	}
}
