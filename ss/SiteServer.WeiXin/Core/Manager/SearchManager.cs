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
	public class SearchManager
    {
        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/search/img/start.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetContentImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/search/img/head_img.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetSearchUrl(SearchInfo searchInfo)
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("publishmentSystemID", searchInfo.PublishmentSystemID.ToString());
            attributes.Add("searchID", searchInfo.ID.ToString());
            string url = APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/search/index.html")));
            return PageUtils.AddQueryString(url, attributes);
        }

        //public static List<ContentInfo> GetContentInfoList(PublishmentSystemInfo publishmentSystemInfo, int nodeID, string keywords)
        //{
        //    List<ContentInfo> contentInfoList = new List<ContentInfo>();
        //    if (nodeID > 0)
        //    {
        //        contentInfoList = DataProvider.BackgroundContentDAO.
        //    }
        //    if (!string.IsNullOrEmpty(keyWords))
        //    {
        //        contentInfoList = DataProvider.ContentDAO.GetContentInfoList(ETableStyle.Site, publishmentSystemInfo.AuxiliaryTableForContent, publishmentSystemID, nodeID, keyWords);
        //    }

        //    return contentInfoList;
        //}

        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, string wxOpenID)
        {
            List<Article> articleList = new List<Article>();

            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<SearchInfo> searchInfoList = DataProviderWX.SearchDAO.GetSearchInfoListByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (SearchInfo searchInfo in searchInfoList)
            {
                Article article = null;

                if (searchInfo != null)
                {
                    string imageUrl = SearchManager.GetImageUrl(publishmentSystemInfo, searchInfo.ImageUrl);
                    string pageUrl = SearchManager.GetSearchUrl(searchInfo);

                    article = new Article()
                    {
                        Title = searchInfo.Title,
                        Description = searchInfo.Summary,
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
