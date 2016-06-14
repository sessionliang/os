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
    public class RequestAnswerDAO : DataProviderBase, IRequestAnswerDAO
	{
        private const string TABLE_NAME = "b2c_RequestAnswer";
        public int Insert(RequestAnswerInfo answerInfo)
        {
            int answerID = 0;

            if (answerInfo.IsAnswer == false)
            {
                answerInfo.Content = StringUtils.ReplaceNewlineToBR(answerInfo.Content);
            }
            else
            {
                answerInfo.Content = StringUtils.ReplaceStartsWith(answerInfo.Content, "<p>", string.Empty);
                answerInfo.Content = StringUtils.ReplaceEndsWith(answerInfo.Content, "</p>", string.Empty);
            }

            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(answerInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        answerID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return answerID;
        }

        public void Update(RequestAnswerInfo answerInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(answerInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public RequestAnswerInfo GetAnswerInfo(int answerID)
        {
            RequestAnswerInfo answerInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", answerID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    answerInfo = new RequestAnswerInfo(rdr);
                }
                rdr.Close();
            }

            return answerInfo;
        }

        public RequestAnswerInfo GetFirstAnswerInfoByRequestID(int requestID)
        {
            RequestAnswerInfo answerInfo = null;

            string SQL_WHERE = string.Format("WHERE RequestID = {0}", requestID);
            string SQL_ORDER = "ORDER BY ID";
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 1, SqlUtils.Asterisk, SQL_WHERE, SQL_ORDER);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    answerInfo = new RequestAnswerInfo(rdr);
                }
                rdr.Close();
            }

            return answerInfo;
        }

        public List<RequestAnswerInfo> GetAnswerInfoList(int requestID)
        {
            List<RequestAnswerInfo> answerInfoList = new List<RequestAnswerInfo>();

            string SQL_WHERE = string.Format("WHERE RequestID = {0}", requestID);
            string SQL_ORDER = "ORDER BY ID";
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, SQL_ORDER);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    RequestAnswerInfo answerInfo = new RequestAnswerInfo(rdr);
                    answerInfoList.Add(answerInfo);
                }
                rdr.Close();
            }

            return answerInfoList;
        }

        public int GetCount(int requestID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE RequestID = {1}", TABLE_NAME, requestID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public string GetSelectString(int requestID)
        {
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, string.Format("WHERE RequestID = {0} ORDER BY ID DESC", requestID), null);
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
