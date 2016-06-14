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
using SiteServer.B2C.Model;
using SiteServer.B2C.Core;

using System.Collections.Generic;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class HistoryDAO : DataProviderBase, IHistoryDAO
    {
        private const string TABLE_NAME = "b2c_History";

        public const string PARM_History_ID = "@HistoryID";
        public const string PARM_PUBLISHMENTSYSTEM_ID = "@PublishmentSystemID";
        public const string PARM_USER_NAME = "@UserName";
        public const string PARM_CHANNEL_ID = "@ChannelID";
        public const string PARM_CONTENT_ID = "@ContentID";
        public const string PARM_ADD_DATE = "@AddDate";

        public int Insert(HistoryInfo historyInfo)
        {

            int historyID = IsExists(historyInfo);
            if (historyID > 0)
            {
                UpdateHistoryAddDate(historyID);
                return historyID;
            }

            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(historyInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        historyID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return historyID;
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public HistoryInfo GetHistoryInfo(int historyID)
        {
            HistoryInfo historyInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", historyID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    historyInfo = new HistoryInfo(rdr);
                }
                rdr.Close();
            }

            return historyInfo;
        }

        public List<HistoryInfo> GetHistoryInfoList(string userName)
        {
            List<HistoryInfo> historyInfoList = new List<HistoryInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = @UserName", FollowAttribute.UserName);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, selectParms))
            {
                while (rdr.Read())
                {
                    HistoryInfo historyInfo = new HistoryInfo(rdr);
                    historyInfoList.Add(historyInfo);
                }
                rdr.Close();
            }

            return historyInfoList;
        }

        public int GetCount(string userName)
        {
            // update by wujq

            // string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE {1} = '{2}'", TABLE_NAME, FollowAttribute.UserName, userName);

            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE {1} = @UserName", TABLE_NAME, FollowAttribute.UserName);

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
            };

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString, selectParms);
        }

        public string GetSelectString(string userName)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                whereString = string.Format("WHERE {0} = '{1}'", FollowAttribute.UserName, userName);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
        private int IsExists(HistoryInfo historyInfo)
        {

            string sqlString = string.Format("SELECT {9} FROM {0} WHERE {1}={2} and {3}={4} and {5}={6} and {7}={8}", TABLE_NAME,
                HistoryAttribute.UserName, PARM_USER_NAME,
                HistoryAttribute.PublishmentSystemID, PARM_PUBLISHMENTSYSTEM_ID,
                HistoryAttribute.ChannelID, PARM_CHANNEL_ID,
                HistoryAttribute.ContentID, PARM_CONTENT_ID,
                HistoryAttribute.ID);

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, historyInfo.UserName),
                this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, historyInfo.PublishmentSystemID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, historyInfo.ChannelID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, historyInfo.ContentID)
            };

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString, selectParms);

        }

        private void UpdateHistoryAddDate(int historyID)
        {
            string sqlString = string.Format("UPDATE {0} SET {1}={2} WHERE {3}={4}", TABLE_NAME,
            HistoryAttribute.AddDate, PARM_ADD_DATE,
            HistoryAttribute.ID, PARM_History_ID);


            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, DateTime.Now),
                this.GetParameter(PARM_History_ID, EDataType.Integer, historyID)
            };

            this.ExecuteNonQuery(sqlString, selectParms);

        }

        public List<HistoryInfo> GetUserHistorysByPage(string userName, int pageIndex, int prePageNum)
        {
            try
            {
                List<HistoryInfo> list = new List<HistoryInfo>();
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(" SELECT tmp.* from ( ");
                sbSql.AppendFormat(" SELECT *, ROW_NUMBER() OVER(ORDER BY AddDate DESC) as rowNum FROM b2c_History WHERE userName = @UserName");
                sbSql.AppendFormat(" ) as tmp ");
                sbSql.AppendFormat(" WHERE tmp.rowNum >= {0} and tmp.rowNum <= {1} ", (pageIndex - 1) * prePageNum + 1, pageIndex * prePageNum);
                string SQL_GETUSERLOGINLOG = sbSql.ToString();

                IDataParameter[] parms = new IDataParameter[] {
                    this.GetParameter(PARM_USER_NAME,EDataType.VarChar,50,userName)
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_GETUSERLOGINLOG, parms))
                {
                    while (rdr.Read())
                    {
                        HistoryInfo info = new HistoryInfo(rdr);
                        list.Add(info);
                    }
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
