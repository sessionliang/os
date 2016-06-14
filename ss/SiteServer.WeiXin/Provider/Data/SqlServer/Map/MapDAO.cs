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
    public class MapDAO : DataProviderBase, IMapDAO
    {
        private const string TABLE_NAME = "wx_Map";
          
         
        public int Insert(MapInfo mapInfo)
        {
            int mapID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(mapInfo.ToNameValueCollection(), this.ConnectionString, MapDAO.TABLE_NAME, out parms);
             
            
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        mapID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, MapDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return mapID;
        }

        public void Update(MapInfo mapInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(mapInfo.ToNameValueCollection(), this.ConnectionString, MapDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void AddPVCount(int mapID)
        {
            if (mapID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", MapDAO.TABLE_NAME, MapAttribute.PVCount, mapID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int mapID)
        {
            if (mapID > 0)
            {
                List<int> mapIDList = new List<int>();
                mapIDList.Add(mapID);
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(mapIDList));

                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", MapDAO.TABLE_NAME, mapID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(List<int> mapIDList)
        {
            if (mapIDList != null  && mapIDList.Count > 0)
            {
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(mapIDList));

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", MapDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(mapIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        private List<int> GetKeywordIDList(List<int> mapIDList)
        {
            List<int> keywordIDList = new List<int>();

            string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", MapAttribute.KeywordID, MapDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(mapIDList));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    keywordIDList.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return keywordIDList;
        }

        public MapInfo GetMapInfo(int mapID)
        {
            MapInfo mapInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", mapID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, MapDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    mapInfo = new MapInfo(rdr);
                }
                rdr.Close();
            }

            return mapInfo;
        }

        public List<MapInfo> GetMapInfoListByKeywordID(int publishmentSystemID, int keywordID)
        {
            List<MapInfo> mapInfoList = new List<MapInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} <> '{3}'", MapAttribute.PublishmentSystemID, publishmentSystemID, MapAttribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", MapAttribute.KeywordID, keywordID);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, MapDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    MapInfo mapInfo = new MapInfo(rdr);
                    mapInfoList.Add(mapInfo);
                }
                rdr.Close();
            }

            return mapInfoList;
        }

        public List<MapInfo> GetMapInfoList(int publishmentSystemID)
        {
            List<MapInfo> mapInfoList = new List<MapInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", MapAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, MapDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    MapInfo mapInfo = new MapInfo(rdr);
                    mapInfoList.Add(mapInfo);
                }
                rdr.Close();
            }

            return mapInfoList;
        }

        public int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} <> '{4}' AND {5} = {6}", TABLE_NAME, MapAttribute.PublishmentSystemID, publishmentSystemID, MapAttribute.IsDisabled, true, MapAttribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetTitle(int mapID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", mapID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, MapDAO.TABLE_NAME, 0, MapAttribute.Title, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    title = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return title;
        }

        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE {0} = {1}", MapAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(MapDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }
    }
}
