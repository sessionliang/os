using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.Oracle
{
    public class TrackingDAO : SiteServer.CMS.Provider.Data.SqlServer.TrackingDAO
	{
		protected override string ADOType
		{
			get
			{
				return SqlUtils.ORACLE;
			}
		}

		protected override EDatabaseType DataBaseType
		{
			get
			{
                return EDatabaseType.Oracle;
			}
		}

        public override DataSet GetDataSource(int publishmentSystemID, int trackingCurrentMinute)
        {
            string SQL_SELECT_TRACKER_ANALYSIS = string.Format(@"
SELECT TrackingID, PublishmentSystemID, UserName, TrackerType, LastAccessDateTime, PageUrl, PageNodeID, PageContentID, Referrer, IPAddress, OperatingSystem, Browser, AccessDateTime
FROM siteserver_Tracking
WHERE (PublishmentSystemID = {0}) AND (AccessDateTime BETWEEN sysdate - {1}/24/60 AND sysdate) ORDER BY AccessDateTime DESC", publishmentSystemID, trackingCurrentMinute);

            return this.ExecuteDataset(SQL_SELECT_TRACKER_ANALYSIS);
        }

        public override int GetCurrentVisitorNum(int publishmentSystemID, int trackingCurrentMinute)
        {
            int currentVisitorNum = 0;

            string sqlString = string.Format(@"
SELECT COUNT(*) AS visitorNum FROM siteserver_Tracking
WHERE (TrackerType = '{0}') AND (PublishmentSystemID = {1}) AND
ROUND(TO_NUMBER(sysdate - TO_DATE(TO_CHAR(AccessDateTime, 'yyyy-mm-dd hh24:mi:ss'), 'yyyy-mm-dd hh24:mi:ss')) * 24 * 60) <= {2}
", ETrackerTypeUtils.GetValue(ETrackerType.Site), publishmentSystemID, trackingCurrentMinute);//当前在线人数

//            string SQL_SELECT_CURRENT_VISITOR_NUM = string.Format(@"
//SELECT COUNT(*) AS visitorNum
//FROM siteserver_Tracking
//WHERE (TrackerType = '{0}') AND
//(PublishmentSystemID = {1}) AND ( (sysdate - AccessDateTime)*60*24 <= {2})", ETrackerTypeUtils.GetValue(ETrackerType.Site), publishmentSystemID, trackingCurrentMinute);//当前在线人数

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    currentVisitorNum = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }
            return currentVisitorNum;
        }

        //总访问量
        public override int GetTotalAccessNum(int publishmentSystemID, DateTime sinceDate)
        {
            int totalAccessNum = 0;

            string sqlString = null;
            if (sinceDate == DateUtils.SqlMinValue)
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0})", publishmentSystemID);
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (AccessDateTime BETWEEN {1} AND sysdate)", publishmentSystemID, SqlUtils.ParseToOracleDateTime(sinceDate));
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
        public override int GetTotalUniqueAccessNumByPageInfo(int publishmentSystemID, int nodeID, int contentID, DateTime sinceDate)
        {
            int totalUniqueAccessNum = 0;

            string sqlString = null;
            if (sinceDate == DateUtils.SqlMinValue)
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (TrackerType = '{1}') AND (PageNodeID = {2}) AND (PageContentID = {3})", publishmentSystemID, ETrackerTypeUtils.GetValue(ETrackerType.Site), nodeID, contentID);
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (TrackerType = '{1}') AND (PageNodeID = {2}) AND (PageContentID = {3}) AND (AccessDateTime BETWEEN {4} AND sysdate)", publishmentSystemID, ETrackerTypeUtils.GetValue(ETrackerType.Site), nodeID, contentID, SqlUtils.ParseToOracleDateTime(sinceDate));
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
        public override int GetTotalUniqueAccessNumByPageUrl(int publishmentSystemID, string pageUrl, DateTime sinceDate)
        {
            int totalUniqueAccessNum = 0;

            string sqlString = null;
            if (sinceDate == DateUtils.SqlMinValue)
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (TrackerType = '{1}') AND (PageUrl = '{2}')", publishmentSystemID, ETrackerTypeUtils.GetValue(ETrackerType.Site), pageUrl);
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (TrackerType = '{1}') AND (PageUrl = '{2}') AND (AccessDateTime BETWEEN {3} AND sysdate)", publishmentSystemID, ETrackerTypeUtils.GetValue(ETrackerType.Site), pageUrl, SqlUtils.ParseToOracleDateTime(sinceDate));
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
        public override int GetTotalUniqueAccessNum(int publishmentSystemID, DateTime sinceDate)
        {
            int totalUniqueAccessNum = 0;

            string sqlString = null;
            if (sinceDate == DateUtils.SqlMinValue)
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (TrackerType = '{1}')", publishmentSystemID, ETrackerTypeUtils.GetValue(ETrackerType.Site));
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (TrackerType = '{1}') AND (AccessDateTime BETWEEN {2} AND sysdate)", publishmentSystemID, ETrackerTypeUtils.GetValue(ETrackerType.Site), SqlUtils.ParseToOracleDateTime(sinceDate));
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
        public override int GetTotalAccessNumByPageUrl(int publishmentSystemID, string pageUrl, DateTime sinceDate)
        {
            int totalAccessNum = 0;

            string sqlString = null;
            if (sinceDate == DateUtils.SqlMinValue)
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (PageUrl = '{1}')", publishmentSystemID, pageUrl);
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE (PublishmentSystemID = {0}) AND (PageUrl = '{1}') AND (AccessDateTime BETWEEN {2} AND sysdate)", publishmentSystemID, pageUrl, SqlUtils.ParseToOracleDateTime(sinceDate));
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
        public override int GetTotalAccessNumByPageInfo(int publishmentSystemID, int nodeID, int contentID, DateTime sinceDate)
        {
            int totalAccessNum = 0;

            string sqlString = null;
            if (sinceDate == DateUtils.SqlMinValue)
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE PublishmentSystemID = {0} AND PageNodeID = {1} AND PageContentID = {2} AND TrackerType = '{3}'", publishmentSystemID, nodeID, contentID, ETrackerTypeUtils.GetValue(ETrackerType.Page));
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS Num FROM siteserver_Tracking WHERE PublishmentSystemID = {0} AND PageNodeID = {1} AND PageContentID = {2} AND TrackerType = '{3}' AND (AccessDateTime BETWEEN {4} AND sysdate)", publishmentSystemID, nodeID, contentID, ETrackerTypeUtils.GetValue(ETrackerType.Page), SqlUtils.ParseToOracleDateTime(sinceDate));
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

        public override int GetMaxAccessNumOfDay(int publishmentSystemID, out string maxAccessDay)
        {
            int maxAccessNumOfDay = 0;
            maxAccessDay = string.Empty;

            string SQL_SELECT_MAX_ACCESS_NUM_OF_DAY = @"
SELECT MAX(AccessNum) AS MaxAccessNum, AccessYear, AccessMonth, 
      AccessDay
FROM (SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth, AccessDay
        FROM (SELECT TO_CHAR(AccessDateTime, 'DD') AS AccessDay,
                     TO_CHAR(AccessDateTime, 'MM') AS AccessMonth,
                     TO_CHAR(AccessDateTime, 'YYYY') AS AccessYear
                FROM siteserver_Tracking
                WHERE PublishmentSystemID = @PublishmentSystemID)
        GROUP BY AccessYear, AccessMonth, AccessDay)
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
                        string accessYear = rdr[1].ToString();
                        string accessMonth = rdr[2].ToString();
                        string accessDay = rdr[3].ToString();
                        maxAccessDay = string.Format("{0}-{1}-{2}", accessYear, accessMonth, accessDay);
                    }
                }
                rdr.Close();
            }
            return maxAccessNumOfDay;
        }

        public override int GetMaxAccessNumOfMonth(int publishmentSystemID)
        {
            int maxAccessNumOfMonth = 0;

            string SQL_SELECT_MAX_ACCESS_NUM_OF_MONTH = @"
SELECT MAX(Expr1) AS Expr1
FROM (SELECT COUNT(*) AS Expr1
        FROM (SELECT TO_CHAR(AccessDateTime, 'MM') AS Expr1, TO_CHAR(AccessDateTime, 'YYYY') AS Expr2
                FROM siteserver_Tracking
                WHERE PublishmentSystemID = @PublishmentSystemID)
        GROUP BY Expr2, Expr1)
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

        public override int GetMaxUniqueAccessNumOfDay(int publishmentSystemID)
        {
            int maxUniqueAccessNumOfDay = 0;

            string SQL_SELECT_MAX_UNIQUE_ACCESS_NUM_OF_DAY = @"
SELECT MAX(Expr1) AS Expr1
FROM (SELECT COUNT(*) AS Expr1
        FROM (SELECT TO_CHAR(AccessDateTime, 'DDD') AS Expr1, 
                      TO_CHAR(AccessDateTime, 'YYY') AS Expr2
                FROM siteserver_Tracking
                WHERE PublishmentSystemID = @PublishmentSystemID AND TrackerType = @TrackerType)
        GROUP BY Expr2, Expr1)
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

        public override int GetMaxUniqueAccessNumOfMonth(int publishmentSystemID)
        {
            int maxUniqueAccessNumOfMonth = 0;

            string SQL_SELECT_MAX_UNIQUE_ACCESS_NUM_OF_MONTH = @"
SELECT MAX(Expr1) AS Expr1
FROM (SELECT COUNT(*) AS Expr1
        FROM (SELECT TO_CHAR(AccessDateTime, 'MM') AS Expr1, TO_CHAR(AccessDateTime, 'YYYY') AS Expr2
                FROM siteserver_Tracking
                WHERE PublishmentSystemID = @PublishmentSystemID AND TrackerType = @TrackerType)
        GROUP BY Expr2, Expr1)
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

        public override ArrayList GetContentIPAddressArrayList(int publishmentSystemID, int nodeID, int contentID, DateTime begin, DateTime end)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Empty;
            if (contentID != 0)
            {
                sqlString = string.Format(@"
SELECT IPAddress FROM siteserver_Tracking
WHERE (PublishmentSystemID = {0} AND PageNodeID = {1} AND PageContentID = {2} AND (AccessDateTime BETWEEN {3} AND {4}))
", publishmentSystemID, nodeID, contentID, SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end));
            }
            else
            {
                sqlString = string.Format(@"
SELECT IPAddress FROM siteserver_Tracking
WHERE (PublishmentSystemID = {0} AND PageNodeID = {1} AND (AccessDateTime BETWEEN {2} AND {3}))
", publishmentSystemID, nodeID, SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end));
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

        public override ArrayList GetTrackingInfoArrayList(int publishmentSystemID, int nodeID, int contentID, DateTime begin, DateTime end)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Empty;
            if (contentID != 0)
            {
                sqlString = string.Format(@"
SELECT TrackingID, PublishmentSystemID, UserName, TrackerType, LastAccessDateTime, PageUrl, PageNodeID, PageContentID, Referrer, IPAddress, OperatingSystem, Browser, AccessDateTime FROM siteserver_Tracking
WHERE (PublishmentSystemID = {0} AND PageNodeID = {1} AND PageContentID = {2} AND (AccessDateTime BETWEEN {3} AND {4}))
", publishmentSystemID, nodeID, contentID, SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end.AddDays(1)));
            }
            else
            {
                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeID, EScopeType.All, string.Empty, string.Empty);
                nodeIDArrayList.Add(nodeID);
                sqlString = string.Format(@"
SELECT TrackingID, PublishmentSystemID, UserName, TrackerType, LastAccessDateTime, PageUrl, PageNodeID, PageContentID, Referrer, IPAddress, OperatingSystem, Browser, AccessDateTime FROM siteserver_Tracking
WHERE (PublishmentSystemID = {0} AND PageNodeID IN ({1}) AND (AccessDateTime BETWEEN {2} AND {3}))
", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end.AddDays(1)));
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

        public override Hashtable GetTrackingHourHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

//            string SQL_SELECT_TRACKING_HOUR = string.Format(@"
//SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth, AccessDay, AccessHour
//FROM (SELECT TO_CHAR(AccessDateTime, 'YYYY') AS AccessYear, TO_CHAR(AccessDateTime, 'MM') AS AccessMonth, TO_CHAR(AccessDateTime, 'DD') AS AccessDay, TO_CHAR(AccessDateTime, 'HH') AS AccessHour
//        FROM siteserver_Tracking
//        WHERE ((sysdate - AccessDateTime) < 1) AND PublishmentSystemID = @PublishmentSystemID) 
//GROUP BY AccessYear, AccessMonth, AccessDay, AccessHour
//ORDER BY AccessYear, AccessMonth, AccessDay, AccessHour
//", SqlUtils.GetDefaultDateString(this.DataBaseType));//访问量24小时统计

            string SQL_SELECT_TRACKING_HOUR = string.Format(@"
            SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth, AccessDay, AccessHour
            FROM (SELECT TO_CHAR(AccessDateTime, 'YYYY') AS AccessYear, TO_CHAR(AccessDateTime, 'MM') AS AccessMonth, TO_CHAR(AccessDateTime, 'DD') AS AccessDay, TO_CHAR(AccessDateTime, 'HH') AS AccessHour
                    FROM siteserver_Tracking
                    WHERE (
ROUND(TO_NUMBER(sysdate - TO_DATE(TO_CHAR(AccessDateTime, 'yyyy-mm-dd hh24:mi:ss'), 'yyyy-mm-dd hh24:mi:ss'))) <= 1
) AND PublishmentSystemID = @PublishmentSystemID) 
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
                        string year = rdr[1].ToString();
                        string month = rdr[2].ToString();
                        string day = rdr[3].ToString();
                        string hour = rdr[4].ToString();

                        DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-{1}-{2} {3}:0:0", year, month, day, hour));
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public override Hashtable GetUniqueTrackingHourHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_UNIQUE_TRACKING_HOUR = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth, AccessDay, 
      AccessHour
FROM (SELECT TO_CHAR(AccessDateTime, 'YYYY') AS AccessYear, TO_CHAR(AccessDateTime, 'MM') AS AccessMonth, TO_CHAR(AccessDateTime, 'DD') AS AccessDay, TO_CHAR(AccessDateTime, 'HH') AS AccessHour
        FROM siteserver_Tracking
        WHERE (
ROUND(TO_NUMBER(sysdate - TO_DATE(TO_CHAR(AccessDateTime, 'yyyy-mm-dd hh24:mi:ss'), 'yyyy-mm-dd hh24:mi:ss'))) <= 1
) AND PublishmentSystemID = @PublishmentSystemID AND TrackerType = @TrackerType) 
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
                        string year = rdr[1].ToString();
                        string month = rdr[2].ToString();
                        string day = rdr[3].ToString();
                        string hour = rdr[4].ToString();

                        DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-{1}-{2} {3}:0:0", year, month, day, hour));
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public override Hashtable GetTrackingDayHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth, AccessDay
FROM (SELECT TO_CHAR(AccessDateTime, 'YYYY') AS AccessYear, TO_CHAR(AccessDateTime, 'MM') AS AccessMonth, TO_CHAR(AccessDateTime, 'DD') AS AccessDay
        FROM siteserver_Tracking
        WHERE (
ROUND(TO_NUMBER(sysdate - TO_DATE(TO_CHAR(AccessDateTime, 'yyyy-mm-dd hh24:mi:ss'), 'yyyy-mm-dd hh24:mi:ss'))) <= 30
) AND PublishmentSystemID = @PublishmentSystemID) 
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
                        string year = rdr[1].ToString();
                        string month = rdr[2].ToString();
                        string day = rdr[3].ToString();

                        DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-{1}-{2}", year, month, day));
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public override Hashtable GetUniqueTrackingDayHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_UNIQUE_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth, AccessDay
FROM (SELECT TO_CHAR(AccessDateTime, 'YYYY') AS AccessYear, TO_CHAR(AccessDateTime, 'MM') AS AccessMonth, TO_CHAR(AccessDateTime, 'DD') AS AccessDay
        FROM siteserver_Tracking
        WHERE (
ROUND(TO_NUMBER(sysdate - TO_DATE(TO_CHAR(AccessDateTime, 'yyyy-mm-dd hh24:mi:ss'), 'yyyy-mm-dd hh24:mi:ss'))) <= 30
) AND 
              PublishmentSystemID = @PublishmentSystemID AND TrackerType = @TrackerType)
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
                        string year = rdr[1].ToString();
                        string month = rdr[2].ToString();
                        string day = rdr[3].ToString();

                        DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-{1}-{2}", year, month, day));
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public override Hashtable GetTrackingMonthHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_TRACKING_MONTH = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth
FROM (SELECT TO_CHAR(AccessDateTime, 'YYYY') AS AccessYear, TO_CHAR(AccessDateTime, 'MM') AS AccessMonth
        FROM siteserver_Tracking
        WHERE (
ROUND(TO_NUMBER(sysdate - TO_DATE(TO_CHAR(AccessDateTime, 'yyyy-mm-dd hh24:mi:ss'), 'yyyy-mm-dd hh24:mi:ss'))) <= 360
) AND PublishmentSystemID = @PublishmentSystemID) 
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
                        string year = rdr[1].ToString();
                        string month = rdr[2].ToString();

                        DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-{1}-1", year, month));
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public override Hashtable GetUniqueTrackingMonthHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_UNIQUE_TRACKING_MONTH = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear, AccessMonth
FROM (SELECT TO_CHAR(AccessDateTime, 'YYYY') AS AccessYear, TO_CHAR(AccessDateTime, 'MM') AS AccessMonth
        FROM siteserver_Tracking
        WHERE (
ROUND(TO_NUMBER(sysdate - TO_DATE(TO_CHAR(AccessDateTime, 'yyyy-mm-dd hh24:mi:ss'), 'yyyy-mm-dd hh24:mi:ss'))) <= 360
) AND 
              PublishmentSystemID = @PublishmentSystemID AND TrackerType = @TrackerType)
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
                        string year = rdr[1].ToString();
                        string month = rdr[2].ToString();

                        DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-{1}-1", year, month));
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public override Hashtable GetTrackingYearHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_TRACKING_YEAR = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear
FROM (SELECT TO_CHAR(AccessDateTime, 'YYYY') AS AccessYear
        FROM siteserver_Tracking
        WHERE (
ROUND(TO_NUMBER(sysdate - TO_DATE(TO_CHAR(AccessDateTime, 'yyyy-mm-dd hh24:mi:ss'), 'yyyy-mm-dd hh24:mi:ss'))) <= 3600
) AND PublishmentSystemID = @PublishmentSystemID) 
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
                        int year = Convert.ToInt32(rdr[1].ToString());

                        DateTime dateTime = new DateTime(year, 1, 1, 0, 0, 0);
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public override Hashtable GetUniqueTrackingYearHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_UNIQUE_TRACKING_YEAR = string.Format(@"
SELECT COUNT(*) AS AccessNum, AccessYear
FROM (SELECT TO_CHAR(AccessDateTime, 'YYYY') AS AccessYear
        FROM siteserver_Tracking
        WHERE (
ROUND(TO_NUMBER(sysdate - TO_DATE(TO_CHAR(AccessDateTime, 'yyyy-mm-dd hh24:mi:ss'), 'yyyy-mm-dd hh24:mi:ss'))) <= 3600
) AND 
              PublishmentSystemID = @PublishmentSystemID AND TrackerType = @TrackerType)
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
                        int year = Convert.ToInt32(rdr[1].ToString());

                        DateTime dateTime = new DateTime(year, 1, 1, 0, 0, 0);
                        hashtable.Add(dateTime, accessNum);
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }

        public override Hashtable GetPageUrlTodayAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_PAGE_URL_TODAY_ACCESS_NUM = string.Format(@"
SELECT PageUrl, COUNT(*) AS TodayAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TO_CHAR(AccessDateTime, 'YYYY') = TO_CHAR(sysdate, 'YYYY')) AND (TO_CHAR(AccessDateTime, 'MM') = TO_CHAR(sysdate, 'MM')) AND (TO_CHAR(AccessDateTime, 'DD') = TO_CHAR(sysdate, 'DD'))
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

        public override Hashtable GetPageUrlTodayUniqueAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_PAGE_URL_TODAY_UNIQUE_ACCESS_NUM = string.Format(@"
SELECT PageUrl, COUNT(*) AS TodayUniqueAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TrackerType = @TrackerType) AND (TO_CHAR(AccessDateTime, 'YYYY') = TO_CHAR(sysdate, 'YYYY')) AND (TO_CHAR(AccessDateTime, 'MM') = TO_CHAR(sysdate, 'MM')) AND (TO_CHAR(AccessDateTime, 'DD') = TO_CHAR(sysdate, 'DD'))
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

        public override Hashtable GetReferrerTodayAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_REFERRER_TODAY_ACCESS_NUM = string.Format(@"
SELECT Referrer, COUNT(*) AS TodayAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TO_CHAR(AccessDateTime, 'YYYY') = TO_CHAR(sysdate, 'YYYY')) AND (TO_CHAR(AccessDateTime, 'MM') = TO_CHAR(sysdate, 'MM')) AND (TO_CHAR(AccessDateTime, 'DD') = TO_CHAR(sysdate, 'DD'))
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

        public override Hashtable GetReferrerTodayUniqueAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_REFERRER_TODAY_UNIQUE_ACCESS_NUM = string.Format(@"
SELECT Referrer, COUNT(*) AS TodayUniqueAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TrackerType = @TrackerType) AND (TO_CHAR(AccessDateTime, 'YYYY') = TO_CHAR(sysdate, 'YYYY')) AND (TO_CHAR(AccessDateTime, 'MM') = TO_CHAR(sysdate, 'MM')) AND (TO_CHAR(AccessDateTime, 'DD') = TO_CHAR(sysdate, 'DD'))
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

        public override Hashtable GetOSTodayAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_OS_TODAY_ACCESS_NUM = string.Format(@"
SELECT OperatingSystem, COUNT(*) AS TodayAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TO_CHAR(AccessDateTime, 'YYYY') = TO_CHAR(sysdate, 'YYYY')) AND (TO_CHAR(AccessDateTime, 'MM') = TO_CHAR(sysdate, 'MM')) AND (TO_CHAR(AccessDateTime, 'DD') = TO_CHAR(sysdate, 'DD'))
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

        public override Hashtable GetOSTodayUniqueAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_OS_TODAY_UNIQUE_ACCESS_NUM = string.Format(@"
SELECT OperatingSystem, COUNT(*) AS TodayUniqueAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TrackerType = @TrackerType) AND (TO_CHAR(AccessDateTime, 'YYYY') = TO_CHAR(sysdate, 'YYYY')) AND (TO_CHAR(AccessDateTime, 'MM') = TO_CHAR(sysdate, 'MM')) AND (TO_CHAR(AccessDateTime, 'DD') = TO_CHAR(sysdate, 'DD'))
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

        public override Hashtable GetBrowserTodayAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_BROWSER_TODAY_ACCESS_NUM = string.Format(@"
SELECT Browser, COUNT(*) AS TodayAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TO_CHAR(AccessDateTime, 'YYYY') = TO_CHAR(sysdate, 'YYYY')) AND (TO_CHAR(AccessDateTime, 'MM') = TO_CHAR(sysdate, 'MM')) AND (TO_CHAR(AccessDateTime, 'DD') = TO_CHAR(sysdate, 'DD'))
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

        public override Hashtable GetBrowserTodayUniqueAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_BROWSER_TODAY_UNIQUE_ACCESS_NUM = string.Format(@"
SELECT Browser, COUNT(*) AS TodayUniqueAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (TrackerType = @TrackerType) AND (TO_CHAR(AccessDateTime, 'YYYY') = TO_CHAR(sysdate, 'YYYY')) AND (TO_CHAR(AccessDateTime, 'MM') = TO_CHAR(sysdate, 'MM')) AND (TO_CHAR(AccessDateTime, 'DD') = TO_CHAR(sysdate, 'DD'))
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


        public override Hashtable GetChannelAccessNumHashtable(int publishmentSystemID, DateTime begin, DateTime end)
        {
            Hashtable hashtable = new Hashtable();

            //访问栏目，总访问量
            string SQL_SELECT_BROWSER_ACCESS_NUM = string.Format(@"
SELECT PageNodeID, COUNT(*) AS AccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID AND PageNodeID <> 0 AND PageContentID = 0 AND (AccessDateTime BETWEEN {0} AND {1}))
GROUP BY PageNodeID
", SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end.AddDays(1)));

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

        public override Hashtable GetChannelContentAccessNumHashtable(int publishmentSystemID, DateTime begin, DateTime end)
        {
            Hashtable hashtable = new Hashtable();

            //访问栏目，总访问量
            string SQL_SELECT_BROWSER_ACCESS_NUM = string.Format(@"
SELECT PageNodeID, COUNT(*) AS AccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID AND PageNodeID <> 0 AND PageContentID <> 0 AND (AccessDateTime BETWEEN {0} AND {1}))
GROUP BY PageNodeID
", SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end.AddDays(1)));

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

        public override Hashtable GetContentAccessNumHashtable(int publishmentSystemID, int nodeID, DateTime begin, DateTime end)
        {
            Hashtable hashtable = new Hashtable();

            //访问栏目，总访问量
            string SQL_SELECT_BROWSER_ACCESS_NUM = string.Format(@"
SELECT PageContentID, COUNT(*) AS AccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID AND PageNodeID = @PageNodeID AND PageContentID <> 0 AND (AccessDateTime BETWEEN {0} AND {1}))
GROUP BY PageContentID
", SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end.AddDays(1)));

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

        public override Hashtable GetTodayContentAccessNumHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string SQL_SELECT_PAGE_URL_TODAY_ACCESS_NUM = string.Format(@"
SELECT PageContentID, COUNT(*) AS TodayAccessNum
FROM siteserver_Tracking
WHERE (PublishmentSystemID = @PublishmentSystemID AND PageContentID <> 0 AND TrackerType = @TrackerType) AND (TO_CHAR(AccessDateTime, 'YYYY') = TO_CHAR(sysdate, 'YYYY')) AND (TO_CHAR(AccessDateTime, 'MM') = TO_CHAR(sysdate, 'MM')) AND (TO_CHAR(AccessDateTime, 'DD') = TO_CHAR(sysdate, 'DD'))
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
	}
}
