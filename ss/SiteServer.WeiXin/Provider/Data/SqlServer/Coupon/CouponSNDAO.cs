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
    public class CouponSNDAO : DataProviderBase, ICouponSNDAO
    {
        private const string TABLE_NAME = "wx_CouponSN";

        public int Insert(CouponSNInfo couponSNInfo)
        {
            int couponSNID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(couponSNInfo.ToNameValueCollection(), this.ConnectionString, CouponSNDAO.TABLE_NAME, out parms);


            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        couponSNID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, CouponSNDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return couponSNID;
        }

        public void Insert(int publishmentSystemID, int couponID, int totalNum)
        {
            List<string> couponSNList = CouponManager.GetCouponSN(totalNum);
            foreach (string sn in couponSNList)
            {
                string sqlString = string.Format("INSERT INTO {0} (PublishmentSystemID, CouponID, SN, Status) VALUES ({1}, {2}, '{3}', '{4}')", TABLE_NAME, publishmentSystemID, couponID, sn, ECouponStatusUtils.GetValue(ECouponStatus.Unused));

                this.ExecuteNonQuery(sqlString);

            }

            DataProviderWX.CouponDAO.UpdateTotalNum(couponID, DataProviderWX.CouponSNDAO.GetTotalNum(publishmentSystemID, couponID));
        }

        public void Insert(int publishmentSystemID, int couponID, List<string> snList)
        {

            foreach (string sn in snList)
            {
                if (!string.IsNullOrEmpty(sn))
                {
                    string sqlString = string.Format("INSERT INTO {0} (PublishmentSystemID, CouponID, SN, Status) VALUES ({1}, {2}, '{3}', '{4}')", TABLE_NAME, publishmentSystemID, couponID, sn, ECouponStatusUtils.GetValue(ECouponStatus.Unused));
                    this.ExecuteNonQuery(sqlString);

                }
            }

            DataProviderWX.CouponDAO.UpdateTotalNum(couponID, DataProviderWX.CouponSNDAO.GetTotalNum(publishmentSystemID, couponID));
        }

        public void Update(CouponSNInfo couponSNInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(couponSNInfo.ToNameValueCollection(), this.ConnectionString, CouponSNDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);

        }

        public void UpdateStatus(ECouponStatus status, List<int> snIDList)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}' WHERE ID IN ({3})", TABLE_NAME, CouponSNAttribute.Status, ECouponStatusUtils.GetValue(status), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(snIDList));

            if (status == ECouponStatus.Cash)
            {
                sqlString = string.Format("UPDATE {0} SET {1} = '{2}', {3} = getdate(), {4} = getdate() WHERE ID IN ({5})", TABLE_NAME, CouponSNAttribute.Status, ECouponStatusUtils.GetValue(status), CouponSNAttribute.HoldDate, CouponSNAttribute.CashDate, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(snIDList));
            }
            else if (status == ECouponStatus.Hold)
            {
                sqlString = string.Format("UPDATE {0} SET {1} = '{2}', {3} = getdate() WHERE ID IN ({4})", TABLE_NAME, CouponSNAttribute.Status, ECouponStatusUtils.GetValue(status), CouponSNAttribute.HoldDate, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(snIDList));
            }

            this.ExecuteNonQuery(sqlString);
        }

        public int Hold(int publishmentSystemID, int actID, string cookieSN)
        {
            int snID = 0;

            string sqlString = string.Format(@"SELECT ID FROM wx_CouponSN WHERE 
PublishmentSystemID = {0} AND 
CookieSN = '{1}' AND 
Status <> '{2}' AND
CouponID IN (SELECT ID FROM wx_Coupon WHERE ActID = {3})", publishmentSystemID, cookieSN, ECouponStatusUtils.GetValue(ECouponStatus.Unused), actID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    snID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            if (snID == 0)
            {
                sqlString = string.Format(@"SELECT TOP 1 ID FROM wx_CouponSN WHERE 
PublishmentSystemID = {0} AND
Status = '{1}' AND
CouponID IN (SELECT ID FROM wx_Coupon WHERE ActID = {2})
ORDER BY ID", publishmentSystemID, ECouponStatusUtils.GetValue(ECouponStatus.Unused), actID);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read())
                    {
                        snID = rdr.GetInt32(0);
                    }
                    rdr.Close();
                }

            }

            return snID;
        }

        public void Delete(int snID)
        {
            if (snID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", CouponSNDAO.TABLE_NAME, snID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(List<int> snIDList)
        {
            if (snIDList != null && snIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", CouponSNDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(snIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public CouponSNInfo GetSNInfo(int snID)
        {
            CouponSNInfo snInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", snID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CouponSNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    snInfo = new CouponSNInfo(rdr);
                }
                rdr.Close();
            }

            return snInfo;
        }

        public List<CouponSNInfo> GetSNInfoByCookieSN(int publishmentSystemID, string cookieSN, string uniqueID)
        {
            List<CouponSNInfo> list = new List<CouponSNInfo>();
            StringBuilder builder;
            if (string.IsNullOrEmpty(uniqueID))
            {
                builder = new StringBuilder(string.Format("WHERE {0} = {1} AND {2} = '{3}'", CouponSNAttribute.PublishmentSystemID, publishmentSystemID, CouponSNAttribute.CookieSN, PageUtils.FilterSql(cookieSN)));
            }
            else
            {
                builder = new StringBuilder(string.Format("WHERE {0} = {1} AND {2} = '{3}'", CouponSNAttribute.PublishmentSystemID, publishmentSystemID, CouponSNAttribute.WXOpenID, uniqueID));
            }
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CouponSNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY ID");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CouponSNInfo couponSnInfo = new CouponSNInfo(rdr);
                    list.Add(couponSnInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public int GetTotalNum(int publishmentSystemID, int couponID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = {4}", TABLE_NAME, CouponSNAttribute.PublishmentSystemID, publishmentSystemID, CouponSNAttribute.CouponID, couponID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetHoldNum(int publishmentSystemID, int couponID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = {4} AND ({5} = '{6}')", TABLE_NAME, CouponSNAttribute.PublishmentSystemID, publishmentSystemID, CouponSNAttribute.CouponID, couponID, CouponSNAttribute.Status, ECouponStatusUtils.GetValue(ECouponStatus.Hold));
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCashNum(int publishmentSystemID, int couponID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = {4} AND {5} = '{6}'", TABLE_NAME, CouponSNAttribute.PublishmentSystemID, publishmentSystemID, CouponSNAttribute.CouponID, couponID, CouponSNAttribute.Status, ECouponStatusUtils.GetValue(ECouponStatus.Cash));
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectString(int publishmentSystemID, int couponID)
        {
            string whereString = string.Format("WHERE {0} = {1} AND {2} = {3}", CouponSNAttribute.PublishmentSystemID, publishmentSystemID, CouponSNAttribute.CouponID, couponID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CouponSNDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<CouponSNInfo> GetCouponSNInfoList(int publishmentSystemID, int couponID)
        {
            List<CouponSNInfo> couponSNInfoList = new List<CouponSNInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3}", CouponSNAttribute.PublishmentSystemID, publishmentSystemID, CouponSNAttribute.CouponID, couponID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CouponSNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CouponSNInfo couponSNInfo = new CouponSNInfo(rdr);
                    couponSNInfoList.Add(couponSNInfo);
                }
                rdr.Close();
            }

            return couponSNInfoList;
        }

    }
}
