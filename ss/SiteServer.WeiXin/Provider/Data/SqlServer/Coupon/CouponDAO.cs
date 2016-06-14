using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;
using ECountType = SiteServer.WeiXin.Model.ECountType;
using ECountTypeUtils = SiteServer.WeiXin.Model.ECountTypeUtils;
using BaiRong.Model;
using System.Text;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class CouponDAO : DataProviderBase, ICouponDAO
    {
        private const string TABLE_NAME = "wx_Coupon";

        public int Insert(CouponInfo couponInfo)
        {
            int couponID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(couponInfo.ToNameValueCollection(), this.ConnectionString, CouponDAO.TABLE_NAME, out parms);


            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        couponID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, CouponDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return couponID;
        }

        public void Update(CouponInfo couponInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(couponInfo.ToNameValueCollection(), this.ConnectionString, CouponDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateTotalNum(int couponID, int totalNum)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE ID = {3}", TABLE_NAME, CouponAttribute.TotalNum, totalNum, couponID);

            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateActID(int couponID, int actID)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE ID = {3}", TABLE_NAME, CouponAttribute.ActID, actID, couponID);

            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int couponID)
        {
            if (couponID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", CouponDAO.TABLE_NAME, couponID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(List<int> couponIDList)
        {
            if (couponIDList != null && couponIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", CouponDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(couponIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public CouponInfo GetCouponInfo(int couponID)
        {
            CouponInfo couponInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", couponID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CouponDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    couponInfo = new CouponInfo(rdr);
                }
                rdr.Close();
            }

            return couponInfo;
        }

        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE {0} = {1}", CouponAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CouponDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public Dictionary<string, int> GetCouponDictionary(int actID)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", CouponAttribute.ActID, actID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, CouponAttribute.Title + "," + CouponAttribute.TotalNum, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    dictionary.Add(rdr.GetValue(0).ToString(), rdr.GetInt32(1));
                }
                rdr.Close();
            }

            return dictionary;
        }

        public List<CouponInfo> GetCouponInfoList(int publishmentSystemID, int actID)
        {
            List<CouponInfo> list = new List<CouponInfo>();

            StringBuilder builder = new StringBuilder(string.Format("WHERE {0} = {1} AND {2} = {3}", CouponAttribute.PublishmentSystemID, publishmentSystemID, CouponAttribute.ActID, actID));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CouponDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY ID");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CouponInfo couponInfo = new CouponInfo(rdr);
                    list.Add(couponInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<CouponInfo> GetAllCouponInfoList(int publishmentSystemID)
        {
            List<CouponInfo> list = new List<CouponInfo>();

            StringBuilder builder = new StringBuilder(string.Format("WHERE {0} = {1} AND {2} > 0", CouponAttribute.PublishmentSystemID, publishmentSystemID, CouponAttribute.TotalNum));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CouponDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY ID");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CouponInfo couponInfo = new CouponInfo(rdr);
                    list.Add(couponInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<CouponInfo> GetCouponInfoList(int publishmentSystemID)
        {
            List<CouponInfo> couponInfoList = new List<CouponInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", CouponAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CouponDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CouponInfo couponInfo = new CouponInfo(rdr);
                    couponInfoList.Add(couponInfo);
                }
                rdr.Close();
            }

            return couponInfoList;
        }
    }
}
