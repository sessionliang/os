using System;
using System.Collections;
using System.Data;
using BaiRong.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public interface ITrackingDAO
    {
        void Insert(TrackingInfo trackerAnalysisInfo);

        DataSet GetDataSource(int publishmentSystemID, int trackingCurrentMinute);

        int GetCurrentVisitorNum(int publishmentSystemID, int trackingCurrentMinute);

        int GetTotalAccessNumByPageUrl(int publishmentSystemID, string pageUrl, DateTime sinceDate);

        int GetTotalAccessNumByPageInfo(int publishmentSystemID, int nodeID, int contentID, DateTime sinceDate);

        int GetTotalAccessNum(int publishmentSystemID, DateTime sinceDate);

        int GetTotalUniqueAccessNumByPageInfo(int publishmentSystemID, int nodeID, int contentID, DateTime sinceDate);

        int GetTotalUniqueAccessNumByPageUrl(int publishmentSystemID, string pageUrl, DateTime sinceDate);

        int GetTotalUniqueAccessNum(int publishmentSystemID, DateTime sinceDate);

        int GetMaxAccessNumOfDay(int publishmentSystemID, out string maxAccessDay);

        int GetMaxAccessNumOfMonth(int publishmentSystemID);

        int GetMaxUniqueAccessNumOfDay(int publishmentSystemID);

        int GetMaxUniqueAccessNumOfMonth(int publishmentSystemID);

        ArrayList GetContentIPAddressArrayList(int publishmentSystemID, int nodeID, int contentID, DateTime begin, DateTime end);

        ArrayList GetTrackingInfoArrayList(int publishmentSystemID, int nodeID, int contentID, DateTime begin, DateTime end);

        Hashtable GetTrackingHourHashtable(int publishmentSystemID);

        Hashtable GetUniqueTrackingHourHashtable(int publishmentSystemID);

        Hashtable GetTrackingDayHashtable(int publishmentSystemID);

        Hashtable GetUniqueTrackingDayHashtable(int publishmentSystemID);

        Hashtable GetTrackingMonthHashtable(int publishmentSystemID);

        Hashtable GetUniqueTrackingMonthHashtable(int publishmentSystemID);

        Hashtable GetTrackingYearHashtable(int publishmentSystemID);

        Hashtable GetUniqueTrackingYearHashtable(int publishmentSystemID);

        DictionaryEntryArrayList GetPageUrlAccessArrayList(int publishmentSystemID);

        Hashtable GetPageUrlUniqueAccessNumHashtable(int publishmentSystemID);

        Hashtable GetPageUrlTodayAccessNumHashtable(int publishmentSystemID);

        Hashtable GetPageUrlTodayUniqueAccessNumHashtable(int publishmentSystemID);

        Hashtable GetReferrerAccessNumHashtable(int publishmentSystemID);

        Hashtable GetReferrerUniqueAccessNumHashtable(int publishmentSystemID);

        Hashtable GetReferrerTodayAccessNumHashtable(int publishmentSystemID);

        Hashtable GetReferrerTodayUniqueAccessNumHashtable(int publishmentSystemID);

        Hashtable GetOSAccessNumHashtable(int publishmentSystemID);

        Hashtable GetOSUniqueAccessNumHashtable(int publishmentSystemID);

        Hashtable GetOSTodayAccessNumHashtable(int publishmentSystemID);

        Hashtable GetOSTodayUniqueAccessNumHashtable(int publishmentSystemID);

        Hashtable GetBrowserAccessNumHashtable(int publishmentSystemID);

        Hashtable GetBrowserUniqueAccessNumHashtable(int publishmentSystemID);

        Hashtable GetBrowserTodayAccessNumHashtable(int publishmentSystemID);

        Hashtable GetBrowserTodayUniqueAccessNumHashtable(int publishmentSystemID);

        Hashtable GetChannelAccessNumHashtable(int publishmentSystemID, DateTime begin, DateTime end);

        Hashtable GetChannelContentAccessNumHashtable(int publishmentSystemID, DateTime begin, DateTime end);

        Hashtable GetContentAccessNumHashtable(int publishmentSystemID, int nodeID, DateTime begin, DateTime end);

        DictionaryEntryArrayList GetContentAccessNumArrayList(int publishmentSystemID);

        Hashtable GetTodayContentAccessNumHashtable(int publishmentSystemID);

        ArrayList GetPageNodeIDArrayListByAccessNum(int publishmentSystemID);

        void DeleteAll(int publishmentSystemID);

        //---------------------------------用户中心------------------------------------//

        string GetUserSqlString(string userName);

        string GetUserSortFieldName();

        /// <summary>
        /// 获取站点的访问量
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dateTime1"></param>
        /// <param name="dateTime2"></param>
        /// <returns></returns>
        int GetHitsCountOfPublishmentSystem(int publishmentSystemId, DateTime dateFrom, DateTime dateTo);
    }
}
