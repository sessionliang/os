using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class ViewsStatisticsDAO : DataProviderBase, IViewsStatisticsDAO
    {
        public string TableName
        {
            get
            {
                return "siteserver_ViewsStatistics";
            }
        }

        public int Insert(ViewsStatisticsInfo info)
        {
            int contentID = 0;

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);
            contentID = this.ExecuteNonQuery(SQL_INSERT, parms);

            return contentID;
        }
        public void Update(ViewsStatisticsInfo info)
        {
            string SQL_UPDATE = string.Format(" UPDATE {0} SET StasCount=@StasCount,AddDate=@AddDate WHERE UserID=@UserID AND NodeID=@NodeID AND StasYear=@StasYear AND StasMonth=@StasMonth AND PublishmentSystemID=@PublishmentSystemID ", TableName);

            IDbDataParameter[] parms = new IDbDataParameter[]
            { 
                this.GetParameter(ViewsStatisticsAttribute.StasCount,EDataType.Integer,info.StasCount),
                this.GetParameter(ViewsStatisticsAttribute.AddDate,EDataType.DateTime,info.AddDate),
                this.GetParameter(ViewsStatisticsAttribute.UserID,EDataType.Integer,info.UserID),
                this.GetParameter(ViewsStatisticsAttribute.NodeID,EDataType.Integer,info.NodeID),
                this.GetParameter(ViewsStatisticsAttribute.StasYear,EDataType.VarChar,10,info.StasYear),
                this.GetParameter(ViewsStatisticsAttribute.StasMonth,EDataType.VarChar,2,info.StasMonth),
                this.GetParameter(ViewsStatisticsAttribute.PublishmentSystemID,EDataType.Integer,info.PublishmentSystemID)
            };

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public string GetAllString(int publishmentSystemID, string whereString)
        {
            string sql = string.Format(" select NodeID,UserID, sum(StasCount) as sumCount from {0}  where PublishmentSystemID={1} {2} group by  NodeID,UserID order by sumCount desc ", TableName, publishmentSystemID, whereString);
            //string orderStr = " order by StasCount desc ";
            //string where = string.Format("WHERE (PublishmentSystemID = {0} {1}) {2}", publishmentSystemID, whereString, orderStr);
            return sql;
        }

        public ViewsStatisticsInfo IsExists(ViewsStatisticsInfo info)
        {
            ViewsStatisticsInfo vinfo = null;
            string sql = string.Format("select * from {5} where PublishmentSystemID={0} and  UserID={1} and NodeID={2}  and StasYear={3} and StasMonth={4} ", info.PublishmentSystemID, info.UserID, info.NodeID, info.StasYear, info.StasMonth, TableName);
            using (IDataReader rdr = this.ExecuteReader(sql))
            {
                if (rdr.Read())
                {
                    vinfo = new ViewsStatisticsInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, vinfo);
                }
                rdr.Close();
            }
            if (vinfo == null)
                return info;
            else
                return vinfo;
        }

        public ArrayList GetMaxNode(int publishmentSystemID, int userID, string whereStr)
        {
            string sql = string.Format(" select  NodeID,sumCount from ( select NodeID, sum(StasCount) as sumCount from dbo.siteserver_ViewsStatistics  where PublishmentSystemID={0} and  UserID={1} {2} group by NodeID )b  group by NodeID ,sumCount having sumCount=( select top 1 sumCount from ( select top 1  NodeID, sum(StasCount) as sumCount from dbo.siteserver_ViewsStatistics  where PublishmentSystemID={0} and  UserID={1} {2} group by NodeID  order by sumCount desc)c order by sumCount desc) ", publishmentSystemID, userID, whereStr);

            ArrayList list = new ArrayList();
            using (IDataReader rdr = this.ExecuteReader(sql))
            {
                while (rdr.Read())
                {
                    list.Add(rdr[0].ToString());
                }
                rdr.Close();
            }
            return list;
        }


        public ArrayList GetMaxNode(int publishmentSystemID, int userID, EIntelligentPushType type, string whereStr)
        {
            string typeStr = string.Empty;

            DateTime dateNow = DateTime.Now;
            int year = dateNow.Year;
            int month = dateNow.Month;
            int day = dateNow.Day;

            string[] dateNows = dateNow.ToString("yyyy-MM-dd").Split('-');
            string yearStr = dateNows[0];
            string monthStr = dateNows[1];

            if (EIntelligentPushTypeUtils.Equals(EIntelligentPushType.ALL, type))
            {
                typeStr = string.Empty;
            }
            else if (EIntelligentPushTypeUtils.Equals(EIntelligentPushType.CurrentYear, type))
            {
                typeStr = string.Format(" AND StasYear='{0}'", yearStr);
            }
            else if (EIntelligentPushTypeUtils.Equals(EIntelligentPushType.OneMonth, type))
            {
                if (day < 10)
                {
                    string[] datalast = (dateNow.Month - 1).ToString("yyyy-MM-dd").Split('-');
                    yearStr = datalast[0];
                    monthStr = datalast[1];
                }

                typeStr = string.Format(" AND StasYear='{0}' and StasMonth='{1}'", yearStr, monthStr);

            }
            else if (EIntelligentPushTypeUtils.Equals(EIntelligentPushType.Trimester, type))
            {
                int mtf = 3;
                int mtt = 0;
                if (day < 10)
                {
                    mtf = 4;
                    mtt = 1;
                }
                else
                {
                    mtf = 2;
                    mtt = 0;
                }
                string[] dateFroms = (dateNow.AddMonths(-mtf)).ToString("yyyy-MM-dd").Split('-');
                string dateFrom = dateFroms[0] + dateFroms[1];
                string[] dateTos = (dateNow.AddMonths(-mtt)).ToString("yyyy-MM-dd").Split('-');
                string dateTo = dateTos[0] + dateTos[1];

                typeStr = string.Format(" AND (tbym.ym >= '{0}' and tbym.ym < '{1}')", dateFrom, dateTo);
        }
            else if (EIntelligentPushTypeUtils.Equals(EIntelligentPushType.HalfYear, type))
            {
                int mtf = 6;
                int mtt = 0; 
                if (day < 10)
                {
                    mtf = 7;
                    mtt = 1;
                }
                else
                {
                    mtf = 5;
                    mtt = 0;
                }
                string[] dateFroms = (dateNow.AddMonths(-mtf)).ToString("yyyy-MM-dd").Split('-');
                string dateFrom = dateFroms[0] + dateFroms[1];
                string[] dateTos = (dateNow.AddMonths(-mtt)).ToString("yyyy-MM-dd").Split('-');
                string dateTo = dateTos[0] + dateTos[1];

                typeStr = string.Format(" AND (tbym.ym  >= '{0}' and tbym.ym < '{1}')", dateFrom, dateTo);
            }
            else if (EIntelligentPushTypeUtils.Equals(EIntelligentPushType.OneYear, type))
            {
                int mtf = 12;
                int mtt = 0; 
                if (day < 10)
                {
                    mtf = 13;
                    mtt = 1;
                }
                else
                {
                    mtf = 12;
                    mtt = 0;
                }
                string[] dateFroms = (dateNow.AddMonths(-mtf)).ToString("yyyy-MM-dd").Split('-');
                string dateFrom = dateFroms[0] + dateFroms[1];
                string[] dateTos = (dateNow.AddMonths(-mtt)).ToString("yyyy-MM-dd").Split('-');
                string dateTo = dateTos[0] + dateTos[1];

                typeStr = string.Format(" AND (tbym.ym  >= '{0}' and tbym.ym < '{1}')", dateFrom, dateTo);
            }
            else
                typeStr = string.Empty;

            string sql = string.Format(" select  NodeID,sumCount from ( select NodeID, sum(StasCount) as sumCount from ( SELECT *, (StasYear+StasMonth) as ym from {3})tbym  where PublishmentSystemID={0} and  UserID={1} {4} {2} group by NodeID )b  group by NodeID ,sumCount having sumCount=( select top 1 sumCount from ( select top 1  NodeID, sum(StasCount) as sumCount from (SELECT *, (StasYear+StasMonth) as ym from {3})tbym  where PublishmentSystemID={0} and  UserID={1} {4} {2} group by NodeID  order by sumCount desc)c order by sumCount desc) ", publishmentSystemID, userID, whereStr, TableName, typeStr);

            ArrayList list = new ArrayList();
            using (IDataReader rdr = this.ExecuteReader(sql))
            {
                while (rdr.Read())
                {
                    list.Add(rdr[0].ToString());
                }
                rdr.Close();
            }
            return list;
        }


        public string GetAllString(int publishmentSystemID, string userName, string dateFrom, string dateTo)
        {
            string dateString = string.Empty;
            if (!string.IsNullOrEmpty(dateFrom))
            {
                string[] date = dateFrom.Split('-');
                string ymF = "" + date[0] + "" + date[1];
                dateString = string.Format(" AND b.ym >= '{0}' ", ymF);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddMonths(1));
                string[] date = dateTo.Split('-');
                string ymT = "" + date[0] + "" + date[1];
                dateString += string.Format(" AND b.ym <= '{0}' ", ymT);
            }


            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select NodeID,UserID, sum(StasCount) as sumCount from (SELECT *, (StasYear+StasMonth) as ym    from {0})b  where PublishmentSystemID={1}  ", TableName, publishmentSystemID);

            sql.Append(dateString);

            if (!string.IsNullOrEmpty(userName))
            {
                sql.AppendFormat("and UserID in  (select UserID from dbo.bairong_Users where UserName like '%{0}%') ", userName);
            }

            sql.Append(" group by  NodeID,UserID order by sumCount desc");

            return sql.ToString();
        }
    }
}
