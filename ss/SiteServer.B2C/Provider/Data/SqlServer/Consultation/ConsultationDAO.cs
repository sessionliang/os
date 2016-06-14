using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using BaiRong.Model;
using System.Data;
using System.Collections;
using BaiRong.Core;
using BaiRong.Core.Data;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class ConsultationDAO : DataProviderBase, IConsultationDAO
    {
        private const string TABLE_NAME = "b2c_Consultation";

        public int Insert(ConsultationInfo consultationInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(consultationInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            return this.ExecuteNonQuery(SQL_INSERT, parms);
        }

        public void Delete(int consultationID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", TABLE_NAME, consultationID);
            this.ExecuteNonQuery(sqlString);
        }

        public ConsultationInfo GetConsultationInfo(int consultationID)
        {
            ConsultationInfo ConsultationInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", consultationID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    ConsultationInfo = new ConsultationInfo();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        string columnName = rdr.GetName(i);
                        ConsultationInfo.SetValue(columnName, rdr.GetValue(i));
                    }
                }
                rdr.Close();
            }

            return ConsultationInfo;
        }

        public List<ConsultationInfo> GetConsultationInfoList(string userName, string keywords, int pageIndex, int prePageNum)
        {
            List<ConsultationInfo> list = new List<ConsultationInfo>();
            string SQL_WHERE = " WHERE 1=1 ";
            string SQL_SELECT = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                SQL_WHERE = string.Format(" AND {0} = '{1}'", ConsultationAttribute.AddUser, userName);

            }
            if (!string.IsNullOrEmpty(keywords))
            {
                SQL_WHERE += string.Format(" AND {0} like '%{1}%'", ConsultationAttribute.Title, keywords);
            }
            SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID ASC");

            SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlStringByQueryString(this.ConnectionString, SQL_SELECT, (pageIndex - 1) * prePageNum + 1, prePageNum, "ORDER BY ID ASC");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        ConsultationInfo consultationInfo = new ConsultationInfo();
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            string columnName = rdr.GetName(i);
                            consultationInfo.SetValue(columnName, rdr.GetValue(i));
                        }

                        list.Add(consultationInfo);
                    }
                    rdr.Close();
                }
            }

            return list;
        }

        public int GetCountByUser(string userName, string keywords)
        {
            int retval = 0;
            string SQL_SELECT = string.Format("SELECT COUNT(*) FROM {0} WHERE AddUser = '{1}'", TABLE_NAME, PageUtils.FilterSql(userName));
            if (!string.IsNullOrEmpty(keywords))
            {
                SQL_SELECT += string.Format(" AND {0} = '{1}'", ConsultationAttribute.Title, keywords);
            }
            object temp = this.ExecuteScalar(SQL_SELECT);
            retval = TranslateUtils.ToInt(temp != null ? temp.ToString() : string.Empty);
            return retval;
        }

        public void Update(ConsultationInfo consultationInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(consultationInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);
            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public int GetCount(string where)
        {
            int retval = 0;
            string SQL_SELECT = string.Format("SELECT COUNT(*) FROM {0} ", TABLE_NAME);
            if (!string.IsNullOrEmpty(where))
            {
                SQL_SELECT += string.Format(" WHERE {0} ", where);
            }
            object temp = this.ExecuteScalar(SQL_SELECT);
            retval = TranslateUtils.ToInt(temp != null ? temp.ToString() : string.Empty);
            return retval;
        }

        public List<ConsultationInfo> GetConsultationInfoList(string where, int pageIndex, int prePageNum)
        {
            List<ConsultationInfo> list = new List<ConsultationInfo>();
            string SQL_WHERE = " WHERE 1=1 ";
            string SQL_SELECT = string.Empty;
            if (!string.IsNullOrEmpty(where))
            {
                SQL_WHERE = string.Format(" AND {0} ", where);

            }
            SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID ASC");

            SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlStringByQueryString(this.ConnectionString, SQL_SELECT, (pageIndex - 1) * prePageNum + 1, prePageNum, "ORDER BY ID ASC");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        ConsultationInfo consultationInfo = new ConsultationInfo();
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            string columnName = rdr.GetName(i);
                            consultationInfo.SetValue(columnName, rdr.GetValue(i));
                        }

                        list.Add(consultationInfo);
                    }
                    rdr.Close();
                }
            }

            return list;
        }

        public void Delete(ArrayList arraylist)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID in ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(arraylist));
            this.ExecuteNonQuery(sqlString);
        }

        public string GetSelectString()
        {
            string whereString = string.Empty;
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetSelectString(bool isReply, string keyword)
        {
            string whereString = " 1=1 ";
            if (isReply)
                whereString += string.Format(" AND (IsReply = '{0}')", isReply.ToString());
            else
                whereString += string.Format(" AND (IsReply = '' OR IsReply = '{0}')", isReply.ToString());

            if (!string.IsNullOrEmpty(keyword))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" ({1} LIKE '%{0}%' OR {2} LIKE '%{0}%' OR {3} LIKE '%{0}%' OR {4} LIKE '%{0}%') ", keyword, ConsultationAttribute.Question, ConsultationAttribute.Answer, ConsultationAttribute.AddUser, ConsultationAttribute.Title);
            }

            if (!string.IsNullOrEmpty(whereString))
            {
                whereString = "WHERE" + whereString;
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, whereString, null);
        }
    }
}
