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
    //public class AccountDAO : DataProviderBase, IAccountDAO
    public class ConfigExtendDAO : DataProviderBase, IConfigExtendDAO
    {
        private const string TABLE_NAME = "wx_ConfigExtend";

        public int Insert(ConfigExtendInfo configExtendInfo)
        {
            int configExtendID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(configExtendInfo.ToNameValueCollection(), this.ConnectionString, ConfigExtendDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        configExtendID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, ConfigExtendDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return configExtendID;
        }

        public void Update(ConfigExtendInfo configExtendInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(configExtendInfo.ToNameValueCollection(), this.ConnectionString, ConfigExtendDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateFuctionID(int publishmentSystemID, int functionID)
        {
            if (functionID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE {1} = 0 AND {3} = {4}", TABLE_NAME, ConfigExtendAttribute.FunctionID, functionID, ConfigExtendAttribute.PublishmentSystemID, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAllNotInIDList(int publishmentSystemID, int functionID, List<int> idList)
        {
            if (functionID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2} AND {3} = {4}", ConfigExtendDAO.TABLE_NAME, ConfigExtendAttribute.PublishmentSystemID, publishmentSystemID, ConfigExtendAttribute.FunctionID, functionID);
                if (idList != null && idList.Count > 0)
                {
                    sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2} AND {3} = {4} AND ID NOT IN ({5})", ConfigExtendDAO.TABLE_NAME, ConfigExtendAttribute.PublishmentSystemID, publishmentSystemID, ConfigExtendAttribute.FunctionID, functionID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idList));
                }
                this.ExecuteNonQuery(sqlString);
            }
        }

        public List<ConfigExtendInfo> GetConfigExtendInfoList(int publishmentSystemID, int functionID,string keywordType)
        {
            List<ConfigExtendInfo> list = new List<ConfigExtendInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3} AND {4}='{5}' ", ConfigExtendAttribute.PublishmentSystemID, publishmentSystemID, ConfigExtendAttribute.FunctionID, functionID,ConfigExtendAttribute.KeywordType,keywordType);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ConfigExtendDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    ConfigExtendInfo configExtendInfo = new ConfigExtendInfo(rdr);
                    list.Add(configExtendInfo);
                }
                rdr.Close();
            }

            return list;
        }
    }
}
