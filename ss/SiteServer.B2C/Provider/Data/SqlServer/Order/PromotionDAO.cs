using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Model;
using BaiRong.Model;
using System.Data;
using BaiRong.Core;
using System.Collections;
using SiteServer.B2C.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class PromotionDAO : DataProviderBase, IPromotionDAO
    {
        private const string TABLE_NAME = "b2c_Promotion";
       
        public int Insert(PromotionInfo promotionInfo)
        {
            int promotionID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(promotionInfo.ToNameValueCollection(), this.ConnectionString, PromotionDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        promotionID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, PromotionDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return promotionID;
        }

        public void Update(PromotionInfo promotionInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(promotionInfo.ToNameValueCollection(), this.ConnectionString, PromotionDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(List<int> promotionIDList)
        {
            if (promotionIDList != null && promotionIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", PromotionDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(promotionIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void UpdateIsEnabled(List<int> promotionIDList, bool isEnabled)
        {
            if (promotionIDList != null && promotionIDList.Count > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = '{2}' WHERE ID IN ({3})", PromotionDAO.TABLE_NAME, PromotionAttribute.IsEnabled, isEnabled, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(promotionIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public PromotionInfo GetPromotionInfo(int promotionID)
        {
            PromotionInfo promotionInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", promotionID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, PromotionDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    promotionInfo = new PromotionInfo(rdr);
                }
                rdr.Close();
            }

            return promotionInfo;
        }

        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE {0} = {1}", PromotionAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(PromotionDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<PromotionInfo> GetEnabledPromotionInfoList(int publishmentSystemID)
        {
            List<PromotionInfo> list = new List<PromotionInfo>();

            StringBuilder builder = new StringBuilder(string.Format("WHERE {0} = '{1}' AND {2} = {3}", PromotionAttribute.IsEnabled, true, PromotionAttribute.PublishmentSystemID, publishmentSystemID));
            builder.AppendFormat(" AND ({0} <= getdate())", PromotionAttribute.StartDate);
            builder.AppendFormat(" AND ({0} >= getdate())", PromotionAttribute.EndDate);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, PromotionDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY ID");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    PromotionInfo promotionInfo = new PromotionInfo(rdr);

                    list.Add(promotionInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<PromotionInfo> GetPromotionInfoList(int publishmentSystemID)
        {
            List<PromotionInfo> list = new List<PromotionInfo>();

            StringBuilder builder = new StringBuilder(string.Format("WHERE {0} = {1}", PromotionAttribute.PublishmentSystemID, publishmentSystemID));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, PromotionDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY ID");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    PromotionInfo promotionInfo = new PromotionInfo(rdr);

                    list.Add(promotionInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<int> GetEnabledPromotionIDList(int publishmentSystemID)
        {
            List<int> list = new List<int>();

            StringBuilder builder = new StringBuilder(string.Format("WHERE {0} = '{1}' AND {2} = {3}", PromotionAttribute.IsEnabled, true, PromotionAttribute.PublishmentSystemID, publishmentSystemID));
            builder.AppendFormat(" AND ({0} <= getdate())", PromotionAttribute.StartDate);
            builder.AppendFormat(" AND ({0} >= getdate())", PromotionAttribute.EndDate);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, PromotionDAO.TABLE_NAME, 0, PromotionAttribute.ID, builder.ToString(), "ORDER BY ID");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return list;
        }
    }
}
