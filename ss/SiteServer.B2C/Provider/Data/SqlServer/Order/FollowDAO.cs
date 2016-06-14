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
    public class FollowDAO : DataProviderBase, IFollowDAO
    {
        private const string TABLE_NAME = "b2c_Follow";

        public const string PARM_Follow_ID = "@FollowID";
        public const string PARM_PUBLISHMENTSYSTEM_ID = "@PublishmentSystemID";
        public const string PARM_USER_NAME = "@UserName";
        public const string PARM_CHANNEL_ID = "@ChannelID";
        public const string PARM_CONTENT_ID = "@ContentID";
        public const string PARM_ADD_DATE = "@AddDate";

        public int Insert(FollowInfo followInfo)
        {

            int followID = IsExists(followInfo);
            if (followID > 0)
            {
                return followID;
            }

            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(followInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        followID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return followID;
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public FollowInfo GetFollowInfo(int followID)
        {
            FollowInfo followInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", followID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    followInfo = new FollowInfo(rdr);
                }
                rdr.Close();
            }

            return followInfo;
        }

        public List<FollowInfo> GetFollowInfoList(string userName)
        {
            // update by wujq

            // string SQL_WHERE = string.Format("WHERE {0} = '{1}'", FollowAttribute.UserName, userName);

            List<FollowInfo> followInfoList = new List<FollowInfo>();

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
                    FollowInfo followInfo = new FollowInfo(rdr);
                    followInfoList.Add(followInfo);
                }
                rdr.Close();
            }

            return followInfoList;
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
        private int IsExists(FollowInfo followInfo)
        {

            string sqlString = string.Format("SELECT {9} FROM {0} WHERE {1}={2} and {3}={4} and {5}={6} and {7}={8}", TABLE_NAME,
                FollowAttribute.UserName, PARM_USER_NAME,
                FollowAttribute.PublishmentSystemID, PARM_PUBLISHMENTSYSTEM_ID,
                FollowAttribute.ChannelID, PARM_CHANNEL_ID,
                FollowAttribute.ContentID, PARM_CONTENT_ID,
                FollowAttribute.ID);

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, followInfo.UserName),
                this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, followInfo.PublishmentSystemID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, followInfo.ChannelID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, followInfo.ContentID)
            };

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString, selectParms);

        }

        public List<FollowInfo> GetUserFollowsByPage(string userName, int pageIndex, int prePageNum)
        {
            try
            {
                List<FollowInfo> list = new List<FollowInfo>();
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(" SELECT tmp.* from ( ");
                sbSql.AppendFormat(" SELECT *, ROW_NUMBER() OVER(ORDER BY AddDate DESC) as rowNum FROM b2c_Follow WHERE userName = @UserName");
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
                        FollowInfo info = new FollowInfo(rdr);
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
