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
	public class MapManager
    {
        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/map/img/start.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetMapUrl(string mapWD)
        {
            return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/map/index.html?mapWD=" + System.Web.HttpUtility.UrlEncode(mapWD) + "")));
        }

        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, string wxOpenID)
        {
            List<Article> articleList = new List<Article>();

            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<MapInfo> mapInfoList = DataProviderWX.MapDAO.GetMapInfoListByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (MapInfo mapInfo in mapInfoList)
            {
                Article article = null;

                if (mapInfo != null)
                {
                    string imageUrl = MapManager.GetImageUrl(publishmentSystemInfo, mapInfo.ImageUrl);
                    string pageUrl = MapManager.GetMapUrl(mapInfo.MapWD);

                    article = new Article()
                    {
                        Title = mapInfo.Title,
                        Description = mapInfo.Summary,
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
