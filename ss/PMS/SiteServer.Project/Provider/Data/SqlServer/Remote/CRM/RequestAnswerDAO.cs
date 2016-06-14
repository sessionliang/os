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
using SiteServer.Project.Model;
using SiteServer.Project.Core;


namespace SiteServer.Project.Provider.Data.SqlServer
{
    public class RequestAnswerDAO : DataProviderBase, IRequestAnswerDAO
	{
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.InnerConnectionString;
            }
        }
        public int Insert(RequestAnswerInfo answerInfo)
        {
            int answerID = 0;

            answerInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(answerInfo.Attributes, this.ConnectionString, RequestAnswerInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        answerID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, RequestAnswerInfo.TableName);

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
            answerInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(answerInfo.Attributes, this.ConnectionString, RequestAnswerInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", RequestAnswerInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public RequestAnswerInfo GetAnswerInfo(NameValueCollection form)
        {
            RequestAnswerInfo answerInfo = new RequestAnswerInfo();

            foreach (string name in form.AllKeys)
            {
                if (RequestAnswerAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        answerInfo.SetExtendedAttribute(name, value.Trim());
                    }
                }
            }

            return answerInfo;
        }

        public RequestAnswerInfo GetAnswerInfo(int answerID)
        {
            RequestAnswerInfo answerInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", answerID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, RequestAnswerInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    answerInfo = new RequestAnswerInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, answerInfo);
                }
                rdr.Close();
            }

            if (answerInfo != null) answerInfo.AfterExecuteReader();
            return answerInfo;
        }

        public RequestAnswerInfo GetFirstAnswerInfoByRequestID(int requestID)
        {
            RequestAnswerInfo answerInfo = null;
            string SQL_WHERE = string.Format("WHERE RequestID = {0}", requestID);
            string SQL_ORDER = "ORDER BY ID";
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, RequestAnswerInfo.TableName, 1, SqlUtils.Asterisk, SQL_WHERE, SQL_ORDER);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    answerInfo = new RequestAnswerInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, answerInfo);
                }
                rdr.Close();
            }

            if (answerInfo != null) answerInfo.AfterExecuteReader();
            return answerInfo;
        }

        public int GetCount(int requestID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE RequestID = {1}", RequestAnswerInfo.TableName, requestID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public string GetSelectString(int requestID)
        {
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, RequestAnswerInfo.TableName, 0, SqlUtils.Asterisk, string.Format("WHERE RequestID = {0} ORDER BY ID DESC", requestID), null);
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
