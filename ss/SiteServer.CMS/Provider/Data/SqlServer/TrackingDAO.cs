using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class TrackingDAO : DataProviderBase, ITrackingDAO
    {
        protected const string PARM_TRACKING_ID = "@TrackingID";
        protected const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        protected const string PARM_USER_NAME = "@UserName";
        protected const string PARM_TRACKER_TYPE = "@TrackerType";
        protected const string PARM_LAST_ACCESS_DATE_TIME = "@LastAccessDateTime";
        protected const string PARM_PAGE_URL = "@PageUrl";
        protected const string PARM_PAGE_NODE_ID = "@PageNodeID";
        protected const string PARM_PAGE_CONTENT_ID = "@PageContentID";
        protected const string PARM_REFERRER = "@Referrer";
        protected const string PARM_IP_ADDRESS = "@IPAddress";
        protected const string PARM_OPERATING_SYSTEM = "@OperatingSystem";
        protected const string PARM_BROWSER = "@Browser";
        protected const string PARM_ACCESS_DATE_TIME = "@AccessDateTime";

        //与数据库字段名无关
        protected const string PARM_CONST_TRACKING_CURRENT_MINUTE = "@TrackingCurrentMinute";

        public void Insert(TrackingInfo trackingInfo)
        {
            string sqlString = "INSERT INTO siteserver_Tracking (PublishmentSystemID, UserName, TrackerType, LastAccessDateTime, PageUrl, PageNodeID, PageContentID, Referrer, IPAddress, OperatingSystem, Browser, AccessDateTime) VALUES (@PublishmentSystemID, @UserName, @TrackerType, @LastAccessDateTime, @PageUrl, @PageNodeID, @PageContentID, @Referrer, @IPAddress, @OperatingSystem, @Browser, @AccessDateTime)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_Tracking (TrackingID, PublishmentSystemID, UserName, TrackerType, LastAccessDateTime, PageUrl, PageNodeID, PageContentID, Referrer, IPAddress, OperatingSystem, Browser, AccessDateTime) VALUES (siteserver_Tracking_SEQ.NEXTVAL, @PublishmentSystemID, @UserName, @TrackerType, @LastAccessDateTime, @PageUrl, @PageNodeID, @PageContentID, @Referrer, @IPAddress, @OperatingSystem, @Browser, @AccessDateTime)";
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, trackingInfo.PublishmentSystemID),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, trackingInfo.UserName),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(trackingInfo.TrackerType)),
				this.GetParameter(PARM_LAST_ACCESS_DATE_TIME, EDataType.DateTime, trackingInfo.LastAccessDateTime),
				this.GetParameter(PARM_PAGE_URL, EDataType.VarChar, 200, trackingInfo.PageUrl),
                this.GetParameter(PARM_PAGE_NODE_ID, EDataType.Integer, trackingInfo.PageNodeID),
                this.GetParameter(PARM_PAGE_CONTENT_ID, EDataType.Integer, trackingInfo.PageContentID),
				this.GetParameter(PARM_REFERRER, EDataType.VarChar, 200, trackingInfo.Referrer),
				this.GetParameter(PARM_IP_ADDRESS, EDataType.VarChar, 200, trackingInfo.IPAddress),
				this.GetParameter(PARM_OPERATING_SYSTEM, EDataType.VarChar, 200, trackingInfo.OperatingSystem),
				this.GetParameter(PARM_BROWSER, EDataType.VarChar, 200, trackingInfo.Browser),
				this.GetParameter(PARM_ACCESS_DATE_TIME, EDataType.DateTime, trackingInfo.AccessDateTime)
			};

            this.ExecuteNonQuery(sqlString, insertParms);
        }

        public virtual DataSet GetDataSource(int publishmentSystemID, int trackingCurrentMinute)
        {
            string SQL_SELECT_TRACKER_ANALYSIS = string.Format(@"
SELECT TrackingID, PublishmentSystemID, UserName, TrackerType, LastAccessDateTime, PageUrl, PageNodeID, PageContentID, Referrer, IPAddress, OperatingSystem, Browser, AccessDateTime
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (AccessDateTime BETWEEN DATEADD(mi, - @TrackingCurrentMinute, {0}) AND {0}) ORDER BY AccessDateTime DESC", SqlUtils.GetDefaultDateString(this.DataBaseType));

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter("@TrackingCurrentMinute", EDataType.Integer, trackingCurrentMinute)
			};

            return this.ExecuteDataset(SQL_SELECT_TRACKER_ANALYSIS, parms);
        }

        public virtual int GetCurrentVisitorNum(int publishmentSystemID, int trackingCurrentMinute)
        {
            int currentVisitorNum = 0;

            string SQL_SELECT_CURRENT_VISITOR_NUM = string.Format(@"
SELECT COUNT(*) AS visitorNum
FROM siteserver_Tracking
WHERE (TrackerType = @TrackerType) AND
(PublishmentSystemID = @PublishmentSystemID) AND (DATEDIFF([minute], AccessDateTime, {0}) <= @TrackingCurrentMinute)", SqlUtils.GetDefaultDateString(this.DataBaseType));//当前在线人数

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site)),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_CONST_TRACKING_CURRENT_MINUTE, EDataType.Integer, trackingCurrentMinute)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_CURRENT_VISITOR_NUM, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        currentVisitorNum = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return currentVisitorNum;
        }

        //总访问量
        public virtual int GetTotalAccessNum(int publishmentSystemID, DateTime sinceDate)
        {
            int totalAccessNum = 0;

            string sqlString = null;
            if (sinceDate == DateUtils.SqlMinValue)
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0})", publishmentSystemID);
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (AccessDateTime BETWEEN '{1}' AND '{2}')", publishmentSystemID, sinceDate.ToShortDateString(), DateTime.Now.ToShortDateString());
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        totalAccessNum = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return totalAccessNum;
        }

        //获取特定页面的总访问量
        public virtual int GetTotalUniqueAccessNumByPageInfo(int publishmentSystemID, int nodeID, int contentID, DateTime sinceDate)
        {
            int totalUniqueAccessNum = 0;

            string sqlString = null;
            if (sinceDate == DateUtils.SqlMinValue)
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (TrackerType = '{1}') AND (PageNodeID = {2}) AND (PageContentID = {3})", publishmentSystemID, ETrackerTypeUtils.GetValue(ETrackerType.Site), nodeID, contentID);
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (TrackerType = '{1}') AND (PageNodeID = {2}) AND (PageContentID = {3}) AND (AccessDateTime BETWEEN '{4}' AND '{5}')", publishmentSystemID, ETrackerTypeUtils.GetValue(ETrackerType.Site), nodeID, contentID, sinceDate.ToShortDateString(), DateTime.Now.ToShortDateString());
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        totalUniqueAccessNum = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return totalUniqueAccessNum;
        }

        //获取特定页面的总访问量
        public virtual int GetTotalUniqueAccessNumByPageUrl(int publishmentSystemID, string pageUrl, DateTime sinceDate)
        {
            int totalUniqueAccessNum = 0;

            string sqlString = null;
            if (sinceDate == DateUtils.SqlMinValue)
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (TrackerType = '{1}') AND (PageUrl = '{2}')", publishmentSystemID, ETrackerTypeUtils.GetValue(ETrackerType.Site), pageUrl);
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (TrackerType = '{1}') AND (PageUrl = '{2}') AND (AccessDateTime BETWEEN '{3}' AND '{4}')", publishmentSystemID, ETrackerTypeUtils.GetValue(ETrackerType.Site), pageUrl, sinceDate.ToShortDateString(), DateTime.Now.ToShortDateString());
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        totalUniqueAccessNum = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return totalUniqueAccessNum;
        }

        //总唯一访问量
        public virtual int GetTotalUniqueAccessNum(int publishmentSystemID, DateTime sinceDate)
        {
            int totalUniqueAccessNum = 0;

            string sqlString = null;
            if (sinceDate == DateUtils.SqlMinValue)
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (TrackerType = '{1}')", publishmentSystemID, ETrackerTypeUtils.GetValue(ETrackerType.Site));
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (TrackerType = '{1}') AND (AccessDateTime BETWEEN '{2}' AND '{3}')", publishmentSystemID, ETrackerTypeUtils.GetValue(ETrackerType.Site), sinceDate.ToShortDateString(), DateTime.Now.ToShortDateString());
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        totalUniqueAccessNum = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return totalUniqueAccessNum;
        }

        //获取特定页面的总访问量
        public virtual int GetTotalAccessNumByPageUrl(int publishmentSystemID, string pageUrl, DateTime sinceDate)
        {
            int totalAccessNum = 0;

            string sqlString = null;
            if (sinceDate == DateUtils.SqlMinValue)
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (PageUrl = '{1}')", publishmentSystemID, pageUrl);
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (PageUrl = '{1}') AND (AccessDateTime BETWEEN '{2}' AND '{3}')", publishmentSystemID, pageUrl, sinceDate.ToShortDateString(), DateTime.Now.ToShortDateString());
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        totalAccessNum = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return totalAccessNum;
        }

        //获取特定页面的总访问量
        public virtual int GetTotalAccessNumByPageInfo(int publishmentSystemID, int nodeID, int contentID, DateTime sinceDate)
        {
            int totalAccessNum = 0;

            string sqlString = null;
            if (sinceDate == DateUtils.SqlMinValue)
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE PublishmentSystemID = {0} AND PageNodeID = {1} AND PageContentID = {2} AND TrackerType = '{3}'", publishmentSystemID, nodeID, contentID, ETrackerTypeUtils.GetValue(ETrackerType.Page));
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE PublishmentSystemID = {0} AND PageNodeID = {1} AND PageContentID = {2} AND TrackerType = '{3}' AND (AccessDateTime BETWEEN '{4}' AND '{5}')", publishmentSystemID, nodeID, contentID, ETrackerTypeUtils.GetValue(ETrackerType.Page), sinceDate.ToShortDateString(), DateTime.Now.ToShortDateString());
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        totalAccessNum = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return totalAccessNum;
        }

        public virtual int GetMaxAccessNumOfDay(int publishmentSystemID, out string maxAccessDay)
        {
            int maxAccessNumOfDay = 0;
            maxAccessDay = string.Empty;

            string SQL_SELECT_MAX_ACCESS_NUM_OF_DAY = @"
SELECT MAX(AccessNum) AS MaxAccessNum, AccessYear, AccessMonth, 
      AccessDay
FROM (SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth, AccessDay
        FROM (SELECT DATEPART([day], AccessDateTime) AS AccessDay, 
                      DATEPART([month], AccessDateTime) AS AccessMonth, 
                      DATEPART([year], AccessDateTime) AS AccessYear
                FROM siteserver_Tracking
                WHERE PublishmentSystemID = @PublishmentSystemID) DERIVEDTBL
        GROUP BY AccessYear, AccessMonth, AccessDay) DERIVEDTBL
GROUP BY AccessYear, AccessMonth, AccessDay
";//最大访问量（日）

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_MAX_ACCESS_NUM_OF_DAY, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        maxAccessNumOfDay = Convert.ToInt32(rdr[0]);
                        int accessYear = Convert.ToInt32(rdr[1]);
                        int accessMonth = Convert.ToInt32(rdr[2]);
                        int accessDay = Convert.ToInt32(rdr[3]);
                        maxAccessDay = string.Format("{0}-{1}-{2}", accessYear, accessMonth, accessDay);
                    }
                }
                rdr.Close();
            }
            return maxAccessNumOfDay;
        }

        public virtual int GetMaxAccessNumOfMonth(int publishmentSystemID)
        {
            int maxAccessNumOfMonth = 0;

            string SQL_SELECT_MAX_ACCESS_NUM_OF_MONTH = @"
SELECT MAX(Expr1) AS Expr1
FROM (SELECT COUNT(*) AS Expr1
        FROM (SELECT DATEPART([month], AccessDateTime) AS Expr1, DATEPART([year], 
                      AccessDateTime) AS Expr2
                FROM siteserver_Tracking
                WHERE PublishmentSystemID = @PublishmentSystemID) DERIVEDTBL
        GROUP BY Expr2, Expr1) DERIVEDTBL
";//最大访问量（月）

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_MAX_ACCESS_NUM_OF_MONTH, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        maxAccessNumOfMonth = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return maxAccessNumOfMonth;
        }

        public virtual int GetMaxUniqueAccessNumOfDay(int publishmentSystemID)
        {
            int maxUniqueAccessNumOfDay = 0;

            string SQL_SELECT_MAX_UNIQUE_ACCESS_NUM_OF_DAY = @"
SELECT MAX(Expr1) AS Expr1
FROM (SELECT COUNT(*) AS Expr1
        FROM (SELECT DATEPART([dayofyear], AccessDateTime) AS Expr1, 
                      DATEPART([year], AccessDateTime) AS Expr2
                FROM siteserver_Tracking
                WHERE PublishmentSystemID = @PublishmentSystemID AND TrackerType = @TrackerType) DERIVEDTBL
        GROUP BY Expr2, Expr1) DERIVEDTBL
";//最大唯一访客（日）

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_MAX_UNIQUE_ACCESS_NUM_OF_DAY, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        maxUniqueAccessNumOfDay = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return maxUniqueAccessNumOfDay;
        }

        public virtual int GetMaxUniqueAccessNumOfMonth(int publishmentSystemID)
        {
            int maxUniqueAccessNumOfMonth = 0;

            string SQL_SELECT_MAX_UNIQUE_ACCESS_NUM_OF_MONTH = @"
SELECT MAX(Expr1) AS Expr1
FROM (SELECT COUNT(*) AS Expr1
        FROM (SELECT DATEPART([month], AccessDateTime) AS Expr1, DATEPART([year], 
                      AccessDateTime) AS Expr2
                FROM siteserver_Tracking
                WHERE PublishmentSystemID = @PublishmentSystemID AND TrackerType = @TrackerType) DERIVEDTBL
        GROUP BY Expr2, Expr1) DERIVEDTBL
";//最大唯一访客（月）

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_MAX_UNIQUE_ACCESS_NUM_OF_MONTH, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        maxUniqueAccessNumOfMonth = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return maxUniqueAccessNumOfMonth;
        }

        public virtual ArrayList GetContentIPAddressArrayList(int publishmentSystemID, int nodeID, int contentID, DateTime begin, DateTime end)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Empty;
            if (contentID != 0)
            {
                sqlString = string.Format(@"
SELECT IPAddress FROM siteserver_Tracking
WHERE (PublishmentSystemID = {0} AND PageNodeID = {1} AND PageContentID = {2} AND (AccessDateTime BETWEEN '{3}' AND '{4}'))
", publishmentSystemID, nodeID, contentID, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }
            else
            {
                sqlString = string.Format(@"
SELECT IPAddress FROM siteserver_Tracking
WHERE (PublishmentSystemID = {0} AND PageNodeID = {1} AND (AccessDateTime BETWEEN '{2}' AND '{3}'))
", publishmentSystemID, nodeID, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return arraylist;
        }

        public virtual ArrayList GetTrackingInfoArrayList(int publishmentSystemID, int nodeID, int contentID, DateTime begin, DateTime end)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Empty;
            if (contentID != 0)
            {
                sqlString = string.Format(@"
SELECT TrackingID, PublishmentSystemID, UserName, TrackerType, LastAccessDateTime, PageUrl, PageNodeID, PageContentID, Referrer, IPAddress, OperatingSystem, Browser, AccessDateTime FROM siteserver_Tracking
WHERE (PublishmentSystemID = {0} AND PageNodeID = {1} AND PageContentID = {2} AND (AccessDateTime BETWEEN '{3}' AND '{4}'))
", publishmentSystemID, nodeID, contentID, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }
            else
            {
                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeID, EScopeType.All, string.Empty, string.Empty);
                nodeIDArrayList.Add(nodeID);
                sqlString = string.Format(@"
SELECT TrackingID, PublishmentSystemID, UserName, TrackerType, LastAccessDateTime, PageUrl, PageNodeID, PageContentID, Referrer, IPAddress, OperatingSystem, Browser, AccessDateTime FROM siteserver_Tracking
WHERE (PublishmentSystemID = {0} AND PageNodeID IN ({1}) AND (AccessDateTime BETWEEN '{2}' AND '{3}'))
", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    TrackingInfo trackingInfo = new TrackingInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), ETrackerTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetDateTime(4), rdr.GetValue(5).ToString(), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetDateTime(12));
                    arraylist.Add(trackingInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public virtual Hashtable GetTrackingHourHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_TRACKING_HOUR = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth, AccessDay, 
      AccessHour
FROM (SELECT DATEPART([year], AccessDateTime) AS AccessYear, DATEPART([Month], 
              AccessDateTime) AS AccessMonth, DATEPART([Day], AccessDateTime) 
              AS AccessDay, DATEPART([Hour], AccessDateTime) AS AccessHour
        FROM siteserver_Tracking
        WHERE (DATEDIFF([Hour], AccessDateTime, {0}) < 24) AND PublishmentSystemID = @PublishmentSystemID) 
      DERIVEDTBL
GROUP BY AccessYear, AccessMonth, AccessDay, AccessHour
ORDER BY AccessYear, AccessMonth, AccessDay, AccessHour
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访问量24小时统计

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TRACKING_HOUR, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int accessNum = Convert.ToInt32(rdr[0]);
                        int year = Convert.ToInt32(rdr[1]);
                        int month = Convert.ToInt32(rdr[2]);
                        int day = Convert.ToInt32(rdr[3]);
                        int hour = Convert.ToInt32(rdr[4]);

                        DateTime dateTime = new DateTime(year, month, day, hour, 0, 0);
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetUniqueTrackingHourHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_UNIQUE_TRACKING_HOUR = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth, AccessDay, 
      AccessHour
FROM (SELECT DATEPART([year], AccessDateTime) AS AccessYear, DATEPART([Month], 
              AccessDateTime) AS AccessMonth, DATEPART([Day], AccessDateTime) 
              AS AccessDay, DATEPART([Hour], AccessDateTime) AS AccessHour
        FROM siteserver_Tracking
        WHERE (DATEDIFF([Hour], AccessDateTime, {0}) < 24) AND PublishmentSystemID = @PublishmentSystemID AND TrackerType = @TrackerType) 
      DERIVEDTBL
GROUP BY AccessYear, AccessMonth, AccessDay, AccessHour
ORDER BY AccessYear, AccessMonth, AccessDay, AccessHour
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访客24小时统计

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_UNIQUE_TRACKING_HOUR, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int accessNum = Convert.ToInt32(rdr[0]);
                        int year = Convert.ToInt32(rdr[1]);
                        int month = Convert.ToInt32(rdr[2]);
                        int day = Convert.ToInt32(rdr[3]);
                        int hour = Convert.ToInt32(rdr[4]);

                        DateTime dateTime = new DateTime(year, month, day, hour, 0, 0);
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetTrackingDayHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth, AccessDay
FROM (SELECT DATEPART([year], AccessDateTime) AS AccessYear, DATEPART([Month], 
              AccessDateTime) AS AccessMonth, DATEPART([Day], AccessDateTime) 
              AS AccessDay
        FROM siteserver_Tracking
        WHERE (DATEDIFF([Hour], AccessDateTime, {0}) < 720) AND PublishmentSystemID = @PublishmentSystemID) 
      DERIVEDTBL
GROUP BY AccessYear, AccessMonth, AccessDay
ORDER BY AccessYear, AccessMonth, AccessDay
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访问量日统计

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TRACKING_DAY, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int accessNum = Convert.ToInt32(rdr[0]);
                        int year = Convert.ToInt32(rdr[1]);
                        int month = Convert.ToInt32(rdr[2]);
                        int day = Convert.ToInt32(rdr[3]);

                        DateTime dateTime = new DateTime(year, month, day, 0, 0, 0);
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetUniqueTrackingDayHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_UNIQUE_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth, AccessDay
FROM (SELECT DATEPART([year], AccessDateTime) AS AccessYear, DATEPART([Month], 
              AccessDateTime) AS AccessMonth, DATEPART([Day], AccessDateTime) 
              AS AccessDay
        FROM siteserver_Tracking
        WHERE (DATEDIFF([Hour], AccessDateTime, {0}) < 720) AND 
              PublishmentSystemID = @PublishmentSystemID AND TrackerType = @TrackerType) DERIVEDTBL
GROUP BY AccessYear, AccessMonth, AccessDay
ORDER BY AccessYear, AccessMonth, AccessDay
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访客日统计

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_UNIQUE_TRACKING_DAY, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int accessNum = Convert.ToInt32(rdr[0]);
                        int year = Convert.ToInt32(rdr[1]);
                        int month = Convert.ToInt32(rdr[2]);
                        int day = Convert.ToInt32(rdr[3]);

                        DateTime dateTime = new DateTime(year, month, day, 0, 0, 0);
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetTrackingMonthHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_TRACKING_MONTH = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth
FROM (SELECT DATEPART([year], AccessDateTime) AS AccessYear, DATEPART([Month], 
              AccessDateTime) AS AccessMonth
        FROM siteserver_Tracking
        WHERE (DATEDIFF([Month], AccessDateTime, {0}) < 12) AND PublishmentSystemID = @PublishmentSystemID) 
      DERIVEDTBL
GROUP BY AccessYear, AccessMonth
ORDER BY AccessYear, AccessMonth
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访问量月统计

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TRACKING_MONTH, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int accessNum = Convert.ToInt32(rdr[0]);
                        int year = Convert.ToInt32(rdr[1]);
                        int month = Convert.ToInt32(rdr[2]);

                        DateTime dateTime = new DateTime(year, month, 1, 0, 0, 0);
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetUniqueTrackingMonthHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_UNIQUE_TRACKING_MONTH = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth
FROM (SELECT DATEPART([year], AccessDateTime) AS AccessYear, DATEPART([Month], 
              AccessDateTime) AS AccessMonth
        FROM siteserver_Tracking
        WHERE (DATEDIFF([Month], AccessDateTime, {0}) < 12) AND 
              PublishmentSystemID = @PublishmentSystemID AND TrackerType = @TrackerType) DERIVEDTBL
GROUP BY AccessYear, AccessMonth
ORDER BY AccessYear, AccessMonth
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访客月统计

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_UNIQUE_TRACKING_MONTH, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int accessNum = Convert.ToInt32(rdr[0]);
                        int year = Convert.ToInt32(rdr[1]);
                        int month = Convert.ToInt32(rdr[2]);

                        DateTime dateTime = new DateTime(year, month, 1, 0, 0, 0);
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetTrackingYearHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_TRACKING_YEAR = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear
FROM (SELECT DATEPART([year], AccessDateTime) AS AccessYear
        FROM siteserver_Tracking
        WHERE (DATEDIFF([Year], AccessDateTime, {0}) < 10) AND PublishmentSystemID = @PublishmentSystemID) 
      DERIVEDTBL
GROUP BY AccessYear
ORDER BY AccessYear
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访问量年统计

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TRACKING_YEAR, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int accessNum = Convert.ToInt32(rdr[0]);
                        int year = Convert.ToInt32(rdr[1]);

                        DateTime dateTime = new DateTime(year, 1, 1, 0, 0, 0);
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetUniqueTrackingYearHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_UNIQUE_TRACKING_YEAR = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear
FROM (SELECT DATEPART([year], AccessDateTime) AS AccessYear
        FROM siteserver_Tracking
        WHERE (DATEDIFF([Year], AccessDateTime, {0}) < 10) AND 
              PublishmentSystemID = @PublishmentSystemID AND TrackerType = @TrackerType) DERIVEDTBL
GROUP BY AccessYear
ORDER BY AccessYear
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访客年统计

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_UNIQUE_TRACKING_YEAR, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int accessNum = Convert.ToInt32(rdr[0]);
                        int year = Convert.ToInt32(rdr[1]);

                        DateTime dateTime = new DateTime(year, 1, 1, 0, 0, 0);
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual DictionaryEntryArrayList GetPageUrlAccessArrayList(int publishmentSystemID)
        {
            DictionaryEntryArrayList arraylist = new DictionaryEntryArrayList();

            string SQL_SELECT_PAGE_URL_ACCESS_NUM = @"
SELECT PageUrl, COUNT(*) AS AccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID)
GROUP BY PageUrl ORDER BY AccessNum DESC
";//访问页面，总访问量

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PAGE_URL_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string pageUrl = rdr.GetValue(0).ToString();
                        int accessNum = rdr.GetInt32(1);

                        DictionaryEntry entry = new DictionaryEntry(pageUrl, accessNum);

                        arraylist.Add(entry);
                    }
                }
                rdr.Close();
            }

            return arraylist;
        }

        public Hashtable GetPageUrlUniqueAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_PAGE_URL_UNIQUE_ACCESS_NUM = @"
SELECT PageUrl, COUNT(*) AS UniqueAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TrackerType = @TrackerType)
GROUP BY PageUrl
";//访问页面，总唯一访客

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PAGE_URL_UNIQUE_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string pageUrl = rdr.GetValue(0).ToString();
                        int uniqueAccessNum = rdr.GetInt32(1);

                        hashtable.Add(pageUrl, uniqueAccessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetPageUrlTodayAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_PAGE_URL_TODAY_ACCESS_NUM = string.Format(@"
SELECT PageUrl, COUNT(*) AS TodayAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (DATEPART([year], 
      AccessDateTime) = DATEPART([year], {0})) AND (DATEPART([month], 
      AccessDateTime) = DATEPART([month], {0})) AND (DATEPART([day], 
      AccessDateTime) = DATEPART([day], {0}))
GROUP BY PageUrl
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访问页面，当天总访问量

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PAGE_URL_TODAY_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string pageUrl = rdr.GetValue(0).ToString();
                        int accessNum = rdr.GetInt32(1);

                        hashtable.Add(pageUrl, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetPageUrlTodayUniqueAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_PAGE_URL_TODAY_UNIQUE_ACCESS_NUM = string.Format(@"
SELECT PageUrl, COUNT(*) AS TodayUniqueAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TrackerType = @TrackerType) AND (DATEPART([year], 
      AccessDateTime) = DATEPART([year], {0})) AND (DATEPART([month], 
      AccessDateTime) = DATEPART([month], {0})) AND (DATEPART([day], 
      AccessDateTime) = DATEPART([day], {0}))
GROUP BY PageUrl
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访问页面，当天总唯一访客

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PAGE_URL_TODAY_UNIQUE_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string pageUrl = rdr.GetValue(0).ToString();
                        int uniqueAccessNum = rdr.GetInt32(1);

                        hashtable.Add(pageUrl, uniqueAccessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetReferrerAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_REFERRER_ACCESS_NUM = @"
SELECT Referrer, COUNT(*) AS AccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID)
GROUP BY Referrer ORDER BY AccessNum DESC
";//来路页面，总访问量

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_REFERRER_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string referrer = PageUtils.GetUrlWithoutPathInfo(rdr.GetValue(0).ToString());
                        int accessNum = rdr.GetInt32(1);

                        if (hashtable[referrer] != null)
                        {
                            int value = (int)hashtable[referrer];
                            accessNum += value;
                        }
                        hashtable[referrer] = accessNum;
                    }
                }
                rdr.Close();
            }

            return hashtable;
        }

        public Hashtable GetReferrerUniqueAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_REFERRER_UNIQUE_ACCESS_NUM = @"
SELECT Referrer, COUNT(*) AS UniqueAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TrackerType = @TrackerType)
GROUP BY Referrer
";//来路页面，总唯一访客

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_REFERRER_UNIQUE_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string referrer = PageUtils.GetUrlWithoutPathInfo(rdr.GetValue(0).ToString());
                        int uniqueAccessNum = rdr.GetInt32(1);

                        if (hashtable[referrer] != null)
                        {
                            int value = (int)hashtable[referrer];
                            uniqueAccessNum += value;
                        }
                        hashtable[referrer] = uniqueAccessNum;
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetReferrerTodayAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_REFERRER_TODAY_ACCESS_NUM = string.Format(@"
SELECT Referrer, COUNT(*) AS TodayAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (DATEPART([year], 
      AccessDateTime) = DATEPART([year], {0})) AND (DATEPART([month], 
      AccessDateTime) = DATEPART([month], {0})) AND (DATEPART([day], 
      AccessDateTime) = DATEPART([day], {0}))
GROUP BY Referrer
", SqlUtils.GetDefaultDateString(this.DataBaseType));//来路页面，当天总访问量

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_REFERRER_TODAY_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string referrer = PageUtils.GetUrlWithoutPathInfo(rdr.GetValue(0).ToString());
                        int accessNum = rdr.GetInt32(1);

                        if (hashtable[referrer] != null)
                        {
                            int value = (int)hashtable[referrer];
                            accessNum += value;
                        }
                        hashtable[referrer] = accessNum;
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetReferrerTodayUniqueAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_REFERRER_TODAY_UNIQUE_ACCESS_NUM = string.Format(@"
SELECT Referrer, COUNT(*) AS TodayUniqueAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TrackerType = @TrackerType) AND (DATEPART([year], 
      AccessDateTime) = DATEPART([year], {0})) AND (DATEPART([month], 
      AccessDateTime) = DATEPART([month], {0})) AND (DATEPART([day], 
      AccessDateTime) = DATEPART([day], {0}))
GROUP BY Referrer
", SqlUtils.GetDefaultDateString(this.DataBaseType));//来路页面，当天总唯一访客

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_REFERRER_TODAY_UNIQUE_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string referrer = PageUtils.GetUrlWithoutPathInfo(rdr.GetValue(0).ToString());
                        int uniqueAccessNum = rdr.GetInt32(1);

                        if (hashtable[referrer] != null)
                        {
                            int value = (int)hashtable[referrer];
                            uniqueAccessNum += value;
                        }
                        hashtable[referrer] = uniqueAccessNum;
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public Hashtable GetOSAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            //";//访问操作系统，总访问量
            string SQL_SELECT_OS_ACCESS_NUM = @"
SELECT OperatingSystem, COUNT(*) AS AccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID)
GROUP BY OperatingSystem
";//访问操作系统，总访问量

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_OS_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string OS = rdr.GetValue(0).ToString();
                        int accessNum = rdr.GetInt32(1);

                        hashtable.Add(OS, accessNum);
                    }
                }
                rdr.Close();
            }

            return hashtable;
        }

        public Hashtable GetOSUniqueAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_OS_UNIQUE_ACCESS_NUM = @"
SELECT OperatingSystem, COUNT(*) AS UniqueAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TrackerType = @TrackerType)
GROUP BY OperatingSystem
";//访问操作系统，总唯一访客

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_OS_UNIQUE_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string OS = rdr.GetValue(0).ToString();
                        int uniqueAccessNum = rdr.GetInt32(1);

                        hashtable.Add(OS, uniqueAccessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetOSTodayAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_OS_TODAY_ACCESS_NUM = string.Format(@"
SELECT OperatingSystem, COUNT(*) AS TodayAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (DATEPART([year], 
      AccessDateTime) = DATEPART([year], {0})) AND (DATEPART([month], 
      AccessDateTime) = DATEPART([month], {0})) AND (DATEPART([day], 
      AccessDateTime) = DATEPART([day], {0}))
GROUP BY OperatingSystem
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访问操作系统，当天总访问量

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_OS_TODAY_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string OS = rdr.GetValue(0).ToString();
                        int accessNum = rdr.GetInt32(1);

                        hashtable.Add(OS, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetOSTodayUniqueAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_OS_TODAY_UNIQUE_ACCESS_NUM = string.Format(@"
SELECT OperatingSystem, COUNT(*) AS TodayUniqueAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TrackerType = @TrackerType) AND (DATEPART([year], 
      AccessDateTime) = DATEPART([year], {0})) AND (DATEPART([month], 
      AccessDateTime) = DATEPART([month], {0})) AND (DATEPART([day], 
      AccessDateTime) = DATEPART([day], {0}))
GROUP BY OperatingSystem
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访问操作系统，当天总唯一访客

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_OS_TODAY_UNIQUE_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string OS = rdr.GetValue(0).ToString();
                        int uniqueAccessNum = rdr.GetInt32(1);

                        hashtable.Add(OS, uniqueAccessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public Hashtable GetBrowserAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_BROWSER_ACCESS_NUM = @"
SELECT Browser, COUNT(*) AS AccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID)
GROUP BY Browser
";//访问浏览器，总访问量

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BROWSER_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string browser = rdr.GetValue(0).ToString();
                        int accessNum = rdr.GetInt32(1);

                        hashtable.Add(browser, accessNum);
                    }
                }
                rdr.Close();
            }

            return hashtable;
        }

        public Hashtable GetBrowserUniqueAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_BROWSER_UNIQUE_ACCESS_NUM = @"
SELECT Browser, COUNT(*) AS UniqueAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TrackerType = @TrackerType)
GROUP BY Browser
";//访问浏览器，总唯一访客

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BROWSER_UNIQUE_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string browser = rdr.GetValue(0).ToString();
                        int uniqueAccessNum = rdr.GetInt32(1);

                        hashtable.Add(browser, uniqueAccessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetBrowserTodayAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_BROWSER_TODAY_ACCESS_NUM = string.Format(@"
SELECT Browser, COUNT(*) AS TodayAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (DATEPART([year], 
      AccessDateTime) = DATEPART([year], {0})) AND (DATEPART([month], 
      AccessDateTime) = DATEPART([month], {0})) AND (DATEPART([day], 
      AccessDateTime) = DATEPART([day], {0}))
GROUP BY Browser
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访问浏览器，当天总访问量

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BROWSER_TODAY_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string browser = rdr.GetValue(0).ToString();
                        int accessNum = rdr.GetInt32(1);

                        hashtable.Add(browser, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual Hashtable GetBrowserTodayUniqueAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_BROWSER_TODAY_UNIQUE_ACCESS_NUM = string.Format(@"
SELECT Browser, COUNT(*) AS TodayUniqueAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TrackerType = @TrackerType) AND (DATEPART([year], 
      AccessDateTime) = DATEPART([year], {0})) AND (DATEPART([month], 
      AccessDateTime) = DATEPART([month], {0})) AND (DATEPART([day], 
      AccessDateTime) = DATEPART([day], {0}))
GROUP BY Browser
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访问浏览器，当天总唯一访客

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Site))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BROWSER_TODAY_UNIQUE_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        string browser = rdr.GetValue(0).ToString();
                        int uniqueAccessNum = rdr.GetInt32(1);

                        hashtable.Add(browser, uniqueAccessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }


        public virtual Hashtable GetChannelAccessNumHashtable(int publishmentSystemID, DateTime begin, DateTime end)
        {
            Hashtable hashtable = new Hashtable();

            //访问栏目，总访问量
            string SQL_SELECT_BROWSER_ACCESS_NUM = string.Format(@"
SELECT PageNodeID, COUNT(*) AS AccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID AND PageNodeID <> 0 AND PageContentID = 0 AND (AccessDateTime BETWEEN '{0}' AND '{1}'))
GROUP BY PageNodeID
", begin.ToShortDateString(), end.AddDays(1).ToShortDateString());

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BROWSER_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int nodeID = rdr.GetInt32(0);
                        int accessNum = rdr.GetInt32(1);

                        hashtable.Add(nodeID, accessNum);
                    }
                }
                rdr.Close();
            }

            return hashtable;
        }

        public virtual Hashtable GetChannelContentAccessNumHashtable(int publishmentSystemID, DateTime begin, DateTime end)
        {
            Hashtable hashtable = new Hashtable();

            //访问栏目，总访问量
            string SQL_SELECT_BROWSER_ACCESS_NUM = string.Empty;

            SQL_SELECT_BROWSER_ACCESS_NUM = string.Format(@"
SELECT PageNodeID, COUNT(*) AS AccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID AND PageNodeID <> 0 AND PageContentID <> 0 AND (AccessDateTime BETWEEN '{0}' AND '{1}'))
GROUP BY PageNodeID
", begin.ToShortDateString(), end.AddDays(1).ToShortDateString());

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BROWSER_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int nodeID = rdr.GetInt32(0);
                        int accessNum = rdr.GetInt32(1);

                        hashtable.Add(nodeID, accessNum);
                    }
                }
                rdr.Close();
            }

            return hashtable;
        }

        public virtual Hashtable GetContentAccessNumHashtable(int publishmentSystemID, int nodeID, DateTime begin, DateTime end)
        {
            Hashtable hashtable = new Hashtable();

            //访问栏目，总访问量
            string SQL_SELECT_BROWSER_ACCESS_NUM = string.Format(@"
SELECT PageContentID, COUNT(*) AS AccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID AND PageNodeID = @PageNodeID AND PageContentID <> 0 AND (AccessDateTime BETWEEN '{0}' AND '{1}'))
GROUP BY PageContentID
", begin.ToShortDateString(), end.AddDays(1).ToShortDateString());

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_PAGE_NODE_ID, EDataType.Integer, nodeID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BROWSER_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int contentID = rdr.GetInt32(0);
                        int accessNum = rdr.GetInt32(1);

                        hashtable.Add(contentID, accessNum);
                    }
                }
                rdr.Close();
            }

            return hashtable;
        }

        public virtual DictionaryEntryArrayList GetContentAccessNumArrayList(int publishmentSystemID)
        {
            DictionaryEntryArrayList arraylist = new DictionaryEntryArrayList();

            string SQL_SELECT_PAGE_URL_ACCESS_NUM = @"
SELECT PageContentID, COUNT(*) AS AccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID AND PageContentID <> 0 AND TrackerType = @TrackerType)
GROUP BY PageContentID ORDER BY AccessNum DESC
";//访问页面，总访问量

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Page))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PAGE_URL_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int pageContentID = rdr.GetInt32(0);
                        int accessNum = rdr.GetInt32(1);

                        DictionaryEntry entry = new DictionaryEntry(pageContentID, accessNum);

                        arraylist.Add(entry);
                    }
                }
                rdr.Close();
            }

            return arraylist;
        }

        public virtual Hashtable GetTodayContentAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_PAGE_URL_TODAY_ACCESS_NUM = string.Format(@"
SELECT PageContentID, COUNT(*) AS TodayAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID AND PageContentID <> 0 AND TrackerType = @TrackerType) AND (DATEPART([year], 
      AccessDateTime) = DATEPART([year], {0})) AND (DATEPART([month], 
      AccessDateTime) = DATEPART([month], {0})) AND (DATEPART([day], 
      AccessDateTime) = DATEPART([day], {0}))
GROUP BY PageContentID
", SqlUtils.GetDefaultDateString(this.DataBaseType));//访问页面，当天总访问量

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Page))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PAGE_URL_TODAY_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int pageContentID = rdr.GetInt32(0);
                        int accessNum = rdr.GetInt32(1);

                        hashtable.Add(pageContentID, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public virtual ArrayList GetPageNodeIDArrayListByAccessNum(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            string SQL_SELECT_PAGE_URL_ACCESS_NUM = @"
SELECT PageNodeID, COUNT(*) AS AccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID AND PageNodeID <> 0 AND TrackerType = @TrackerType)
GROUP BY PageNodeID ORDER BY AccessNum DESC
";//访问页面，总访问量

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_TRACKER_TYPE, EDataType.VarChar, 50, ETrackerTypeUtils.GetValue(ETrackerType.Page))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PAGE_URL_ACCESS_NUM, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int pageNodeID = rdr.GetInt32(0);
                        arraylist.Add(pageNodeID);
                    }
                }
                rdr.Close();
            }

            return arraylist;
        }

        public void DeleteAll(int publishmentSystemID)
        {
            string sqlString = string.Format("DELETE FROM siteserver_Tracking WHERE PublishmentSystemID = {0}", publishmentSystemID);
            this.ExecuteNonQuery(sqlString);
        }

        //-----------------------------用户中心--------------------------------//

        public string GetUserSqlString(string userName)
        {
            string sqlString = string.Format(@"
SELECT TrackingID, PublishmentSystemID, UserName, TrackerType, LastAccessDateTime, PageUrl, PageNodeID, PageContentID, Referrer, IPAddress, OperatingSystem, Browser, AccessDateTime FROM siteserver_Tracking
WHERE UserName = '{0}' AND PageContentID <> 0
", userName, SqlUtils.GetDefaultDateString(this.DataBaseType));//访问页面，当天总访问量

            return sqlString;
        }

        public string GetUserSortFieldName()
        {
            return "AccessDateTime";
        }

        /// <summary>
        /// 统计站点访问量
        /// add by sessionlliang at 201225
        /// </summary>
        /// <param name="publishmentSystemId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public int GetHitsCountOfPublishmentSystem(int publishmentSystemId, DateTime dateFrom, DateTime dateTo)
        {
            string sqlString = string.Format(@" SELECT COUNT(*) FROM siteserver_Tracking WHERE PublishmentSystemId = {0} ", publishmentSystemId);
            if (dateFrom > DateUtils.SqlMinValue)
            {
                sqlString += string.Format(@" AND AccessDateTime >= '{0}' ", dateFrom.ToString());
            }
            if (dateTo > DateUtils.SqlMinValue)
            {
                sqlString += string.Format(@" AND AccessDateTime <= '{0}' ", dateTo.ToString());
            }
            return TranslateUtils.ToInt(this.ExecuteScalar(sqlString).ToString());
        }
    }
}
