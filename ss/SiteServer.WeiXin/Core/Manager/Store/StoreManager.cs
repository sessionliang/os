using BaiRong.Core;
using Senparc.Weixin.MP.Entities;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Core
{
    public class StoreManager
    {
        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/store/img/start.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetContentImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl, int sequence)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl(string.Format("services/weixin/store/img/pic{0}.jpg", sequence))));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        private static string GetStoreUrl(StoreInfo storeInfo)
        {
            return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/store/index.html")));
        }

        public static string GetStoreUrl(StoreInfo storeInfo, string wxOpenID)
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("publishmentSystemID", storeInfo.PublishmentSystemID.ToString());
            attributes.Add("storeID", storeInfo.ID.ToString());
            attributes.Add("wxOpenID", wxOpenID);
            attributes.Add("parentID", "0");
            return PageUtils.AddQueryString(GetStoreUrl(storeInfo), attributes);
        }

        private static string GetStoreItemUrl(StoreItemInfo storeItemInfo)
        {
            return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/store/content.html")));
        }

        public static string GetStoreItemUrl(StoreItemInfo storeItemInfo, string wxOpenID)
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("publishmentSystemID", storeItemInfo.PublishmentSystemID.ToString());
            attributes.Add("storeID", storeItemInfo.StoreID.ToString());
            attributes.Add("storeItemID", storeItemInfo.ID.ToString());
            attributes.Add("wxOpenID", wxOpenID);
            return PageUtils.AddQueryString(GetStoreItemUrl(storeItemInfo), attributes);
        }

        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, string wxOpenID)
        {
            List<Article> articleList = new List<Article>();

            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<StoreInfo> storeInfoList = DataProviderWX.StoreDAO.GetStoreInfoListByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (StoreInfo storeInfo in storeInfoList)
            {
                Article article = null;

                if (storeInfo != null)
                {
                    string imageUrl = StoreManager.GetImageUrl(publishmentSystemInfo, storeInfo.ImageUrl);
                    string pageUrl = StoreManager.GetStoreUrl(storeInfo, wxOpenID);

                    article = new Article()
                    {
                        Title = storeInfo.Title,
                        Description = storeInfo.Summary,
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

        public static List<Article> TriggerStoreItem(int publishmentSystemID, string location_X, string location_Y, string wxOpenID)
        {
            List<Article> articleList = new List<Article>();

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            List<StoreItemInfo> storeItemInfoList = DataProviderWX.StoreItemDAO.GetAllStoreItemInfoListByLocation(publishmentSystemID, location_X);

            foreach (StoreItemInfo storeItemInfo in storeItemInfoList)
            {
                Article article = null;

                if (storeItemInfo != null)
                {
                    string imageUrl = StoreManager.GetImageUrl(publishmentSystemInfo, storeItemInfo.ImageUrl);
                    string pageUrl = StoreManager.GetStoreItemUrl(storeItemInfo, wxOpenID);

                    Point beginPointStore = new Point(Convert.ToDouble(location_X), Convert.ToDouble(location_Y));
                    Point endPointStore = new Point(Convert.ToDouble(storeItemInfo.Latitude), Convert.ToDouble(storeItemInfo.Longitude));

                    string NewTitile = storeItemInfo.StoreName + "，距离" + Earth.GetDistance(beginPointStore, endPointStore) * 1000 + "米";

                    article = new Article()
                    {
                        Title = NewTitile,
                        Description = storeItemInfo.Summary,
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

    public class Earth
    {
        /// <summary>  
        /// 地球的半径  
        /// </summary>  
        public const double EARTH_RADIUS = 6378.137;

        /// <summary>  
        /// 计算坐标点的距离  
        /// </summary>  
        /// <param name="begin">开始的经度纬度</param>  
        /// <param name="end">结束的经度纬度</param>  
        /// <returns>距离(公里)</returns>  
        public static double GetDistance(Point begin, Point end)
        {
            double lat = begin.RadLat - end.RadLat;
            double lng = begin.RadLng - end.RadLng;

            double dis = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(lat / 2), 2) + Math.Cos(begin.RadLat) * Math.Cos(end.RadLat) * Math.Pow(Math.Sin(lng / 2), 2)));
            dis = dis * EARTH_RADIUS;
            dis = Math.Round(dis * 1e4) / 1e4;

            return dis;
        }
    }

    /// <summary>  
    /// 代表经度, 纬度  
    /// </summary>  
    public class Point
    {
        /// <param name="lat">纬度 X</param>  
        /// <param name="lng">经度 Y</param>  
        public Point(double lat, double lng)
        {
            this.lat = lat;
            this.lng = lng;
        }

        //  纬度 X  
        private double lat;

        // 经度 Y  
        private double lng;

        /// <summary>  
        /// 代表纬度 X轴  
        /// </summary>  
        public double Lat { set; get; }

        /// <summary>  
        /// 代表经度 Y轴  
        /// </summary>  
        public double Lng { get; set; }

        public double RadLat { get { return lat * Math.PI / 180; } }

        public double RadLng { get { return lng * Math.PI / 180; } }
    }
}
