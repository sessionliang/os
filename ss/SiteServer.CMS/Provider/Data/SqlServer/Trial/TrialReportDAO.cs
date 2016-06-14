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
using System.Collections.Generic;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class TrialReportDAO : DataProviderBase, ITrialReportDAO
    {
        public string TableName
        {
            get
            {
                return TrialReportInfo.TableName;
            }
        }

        public int Insert(TrialReportInfo info)
        {
            int contentID = 0;
            IDbDataParameter[] parms = null;
            info.BeforeExecuteNonQuery();
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        contentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TableName);

                        //修改试用申请的IsReport为true
                        //TrialApplyInfo applyInfo = DataProvider.TrialApplyDAO.GetInfo(info.TAID);
                        //applyInfo.IsReport = true;
                        //DataProvider.TrialApplyDAO.UpdateWhithTrans(applyInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return contentID;
        }

        public void InsertWhithTrans(TrialReportInfo info, IDbTransaction trans)
        {
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);

            this.ExecuteNonQuery(trans, SQL_INSERT, parms);

        }


        public void Insert(ArrayList infoList, bool deleteAll)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (TrialReportInfo info in infoList)
                            InsertWhithTrans(info, trans);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(int publishmentSystemID, int nodeID, int contentID, ArrayList idList)
        {
            IDbDataParameter[] parms = null;
            string SQL_Delete = string.Format(@"DELETE FROM {0} where ECID in ({1}) and PublishmentSystemID={2} and NodeID={3} {4} ", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idList), publishmentSystemID, nodeID, contentID != 0 ? ("and ContentID= " + contentID) : "");

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_Delete, parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

        }


        public void Update(TrialReportInfo info)
        {
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms); ;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

        }

        public ArrayList GetInfoList(int publishmentSystemID, int nodeID, int contentID, string tableStyle)
        {
            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0} and NodeID={1} and ContentID={2} and tableStyle='{3}'  ", publishmentSystemID, nodeID, contentID, tableStyle);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        TrialReportInfo info = new TrialReportInfo();
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                        list.Add(info);
                    }
                }
                rdr.Close();
            }

            return list;
        }

        public string GetSelectSqlString(int publishmentSystemID, int nodeID, int contentID)
        {
            string whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2} ORDER BY AddDate DESC", publishmentSystemID, nodeID, contentID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString);
        }

        public string GetSelectSqlString(int publishmentSystemID, List<int> channelIDList, string keyword, int searchDate, ETriState checkedState, ETriState channelState)
        {
            string checkString = string.Empty;
            if (checkedState == ETriState.True)
            {
                checkString = "AND ReportStatus = 'True'";
            }
            else if (checkedState == ETriState.False)
            {
                checkString = "AND ReportStatus = 'False'";
            }
            string channelString = string.Empty;
            if (channelState == ETriState.True)
            {
                channelString = "AND ContentID = 0";
            }
            else if (checkedState == ETriState.False)
            {
                channelString = "AND ContentID > 0";
            }
            string dateString = string.Empty;
            if (searchDate > 0)
            {
                DateTime dateTime = DateTime.Now.AddDays(-searchDate);
                dateString = string.Format(" AND (AddDate >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
            }
            return GetSelectSqlString(publishmentSystemID, channelIDList, keyword, checkString, channelString, dateString);
        }

        private string GetSelectSqlString(int publishmentSystemID, List<int> channelIDList, string keyword, string checkString, string channelString, string dateString)
        {
            string whereString;
            if (string.IsNullOrEmpty(keyword))
            {
                if (channelIDList.Count > 1)
                    whereString = whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID IN ({1}) {2} {3} ORDER BY AddDate DESC", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(channelIDList), dateString, checkString);
                else
                    whereString = whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID ={1} {2} {3} ORDER BY AddDate DESC", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(channelIDList), dateString, checkString);
            }
            else
            {
                if (channelIDList.Count > 1)
                    whereString = whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID IN ({1}) AND Description LIKE '%{2}%' {3} {4} ORDER BY AddDate DESC", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(channelIDList), keyword, dateString, checkString);
                else
                    whereString = whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID ={1} AND Description LIKE '%{2}%' {3} {4} ORDER BY AddDate DESC", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(channelIDList), keyword, dateString, checkString);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString);
        }



        public int GetCountChecked(int publishmentSystemID, int nodeID, int contentID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {4} WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2} AND IsChecked = '{3}'", publishmentSystemID, nodeID, contentID, true.ToString(), TableName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }


        public List<TrialReportInfo> GetInfoListChecked(int publishmentSystemID, int nodeID, int contentID)
        {
            List<TrialReportInfo> infoList = new List<TrialReportInfo>();

            string sqlString = string.Format("SELECT COUNT(*) FROM {4} WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2} AND IsChecked = '{3}'", publishmentSystemID, nodeID, contentID, true.ToString(), TableName);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    TrialReportInfo info = new TrialReportInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    infoList.Add(info);
                }
                rdr.Close();
            }

            return infoList;
        }


        public string GetSortFieldName()
        {
            return "AddDate";
        }



        public TrialReportInfo GetInfo(int publishmentSystemID, int nodeID, int contentID, int taid)
        {
            TrialReportInfo info = new TrialReportInfo();
            string sqlString = string.Format("SELECT * FROM {4} WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2} AND TRID = {3}", publishmentSystemID, nodeID, contentID, taid, TableName);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }
            info.AfterExecuteReader();
            return info;
        }


        public void UpdateStatus(int publishmentSystemID, int nodeID, int contentID, int trid, string adminName)
        {
            IDbDataParameter[] parms = null;
            string SQL_INSERT = string.Format("UPDATE {0} SET ReportStatus='True',AdminName='{5}' WHERE PublishmentSystemID = {1} AND NodeID = {2} AND ContentID = {3} AND TRID = {4} ", TableName, publishmentSystemID, nodeID, contentID, trid, adminName);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        
            
        public object GetSelectCommendOfAnalysisByNode(int publishmentSystemID, int nodeID, string dateFrom, string dateTo)
        {
            string dateString = string.Empty;
            if (!string.IsNullOrEmpty(dateFrom))
            {
                dateString = string.Format(" AND AddDate >= '{0}' ", dateFrom);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString = string.Format(" AND to_char(AddDate,'YYYY-MM-DD') >= '{0}' ", dateFrom);
                }
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddDays(1));
                dateString += string.Format(" AND AddDate <= '{0}' ", dateTo);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString += string.Format(" AND to_char(AddDate,'YYYY-MM-DD') <= '{0}' ", dateTo);
                }
            }
            string sqlString = string.Format(" select (SUM(CompositeScore)/ COUNT(1)) as avgCompositeScore from {0} where PublishmentSystemID={1} and NodeID={2} {3} group by PublishmentSystemID,NodeID  ", TableName, publishmentSystemID, nodeID, dateString);

            double avg = Convert.ToDouble(this.ExecuteScalar(sqlString));

            return avg;
        }

        public int GetReportCountByApplyID(int publishmentSystemID, int nodeID, int contentID, int taid)
        {
            int count = 0;
            string sqlString = string.Format("SELECT COUNT(*) FROM {4} WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2} AND TAID = {3}", publishmentSystemID, nodeID, contentID, taid, TableName);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    count = TranslateUtils.ToInt(rdr[0].ToString());
                }
                rdr.Close();
            }
            return count;
        }
    }
}
