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
    public class TrialApplyDAO : DataProviderBase, ITrialApplyDAO
    {
        public string TableName
        {
            get
            {
                return TrialApplyInfo.TableName;
            }
        }

        public int Insert(TrialApplyInfo info)
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

        public void InsertWhithTrans(TrialApplyInfo info, IDbTransaction trans)
        {
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);

            this.ExecuteNonQuery(trans, SQL_INSERT, parms);

        }


        public void Inserts(ArrayList infoList)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (TrialApplyInfo info in infoList)
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


        public void Update(TrialApplyInfo info)
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

        public ArrayList GetInfoList(int publishmentSystemID, int nodeID, int contentID, ArrayList infoList)
        {
            string contentstr = string.Empty;
            if (contentID != 0)
            {
                contentstr = string.Format(" and ContentID={2}", contentID);
            }
            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0} and NodeID={1} {2} and TAID in ({3})  ", publishmentSystemID, nodeID, contentstr, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(infoList));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        TrialApplyInfo info = new TrialApplyInfo();
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

        public string GetSelectSqlString(int publishmentSystemID, List<int> channelIDList, int searchDate, ETriState checkedState, ETriState channelState)
        {
            string checkString = string.Empty;
            if (checkedState == ETriState.True)
            {
                checkString = "AND IsChecked = 'True'";
            }
            else if (checkedState == ETriState.False)
            {
                checkString = "AND IsChecked = 'False'";
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
            return GetSelectSqlString(publishmentSystemID, channelIDList, checkString, channelString, dateString);
        }

        private string GetSelectSqlString(int publishmentSystemID, List<int> channelIDList, string checkString, string channelString, string dateString)
        {
            string whereString;

            if (channelIDList.Count > 1)
                whereString = whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID IN ({1}) {2} {3} ORDER BY AddDate DESC", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(channelIDList), dateString, checkString);
            else
                whereString = whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID  ={1} {2} {3} ORDER BY AddDate DESC", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(channelIDList), dateString, checkString);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString);
        }



        public int GetCountChecked(int publishmentSystemID, int nodeID, int contentID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {4} WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2} AND IsChecked = '{3}'", publishmentSystemID, nodeID, contentID, true.ToString(), TableName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }


        public List<TrialApplyInfo> GetInfoListChecked(int publishmentSystemID, int nodeID, int contentID)
        {
            List<TrialApplyInfo> infoList = new List<TrialApplyInfo>();

            string sqlString = string.Format("SELECT COUNT(*) FROM {4} WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2} AND IsChecked = '{3}'", publishmentSystemID, nodeID, contentID, true.ToString(), TableName);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    TrialApplyInfo info = new TrialApplyInfo();
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



        public int GetCount(int publishmentSystemID, int nodeID, int contentID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {3} WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2} ", publishmentSystemID, nodeID, contentID, TableName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public void ApplyChecked(int publishmentSystemID, int nodeID, int contentID, ArrayList ids, bool isChecked, bool isReport, bool isMobile, string checkAdmin, DateTime CheckDate)
        {
            IDbDataParameter[] parms = null;
            string SQL_Delete = string.Format(@"UPDATE {0} SET CheckAdmin='{4}',CheckDate='{5}',IsReport='{6}',IsMobile='{7}',IsChecked='{9}' where TAID in ({1}) and PublishmentSystemID={2} and NodeID={3} AND ContentID='{8}' and IsChecked='False' ", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(ids), publishmentSystemID, nodeID, checkAdmin, CheckDate, isReport, isMobile, contentID, isChecked);

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


        public TrialApplyInfo GetInfo(int taid)
        {
            TrialApplyInfo info = new TrialApplyInfo();
            string sqlString = string.Format("SELECT * FROM {1} WHERE TAID = {0}", taid, TableName);
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



        int ITrialApplyDAO.Update(TrialApplyInfo info)
        {
            int contentID = 0;
            IDbDataParameter[] parms = null;
            info.BeforeExecuteNonQuery();
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_UPDATE, parms);

                        contentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TableName);
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

        public int UpdateWhithTrans(TrialApplyInfo info, IDbTransaction trans)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, TableName, out parms);

            return this.ExecuteNonQuery(trans, SQL_UPDATE, parms);

        }

        public bool IsExists(int publishmentSystemID, int nodeID, int contentID, string userName)
        {
            string sqlString = string.Format("SELECT count(*) FROM {4} WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2} AND UserName = '{3}'", publishmentSystemID, nodeID, contentID, userName, TableName);
            return TranslateUtils.ToInt(this.ExecuteScalar(sqlString).ToString()) > 0 ? true : false;
        }



        public void ApplyChecked(int publishmentSystemID, int nodeID, ArrayList ids, bool isChecked,string applystatus, bool isReport, bool isMobile, string checkAdmin, DateTime CheckDate)
        {
            IDbDataParameter[] parms = null;
            string SQL_Delete = string.Format(@"UPDATE {0} SET CheckAdmin='{4}',CheckDate='{5}',IsReport='{6}',IsMobile='{7}',IsChecked='{8}',ApplyStatus='{9}' where TAID in ({1}) and PublishmentSystemID={2} and NodeID={3} and IsChecked='False' ", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(ids), publishmentSystemID, nodeID, checkAdmin, CheckDate, isReport, isMobile, isChecked, applystatus);

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

    }
}
