using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;
using BaiRong.Model;
using System.Text;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class AlbumDAO : DataProviderBase, IAlbumDAO
    {
        private const string TABLE_NAME = "wx_Album";
         
        public int Insert(AlbumInfo albumInfo)
        {
            int albumID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(albumInfo.ToNameValueCollection(), this.ConnectionString, AlbumDAO.TABLE_NAME, out parms);
             
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        albumID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, AlbumDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return albumID;
        }

        public void Update(AlbumInfo albumInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(albumInfo.ToNameValueCollection(), this.ConnectionString, AlbumDAO.TABLE_NAME, out parms);
             
            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }
  
        public void AddPVCount(int albumID)
        {
            if (albumID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", AlbumDAO.TABLE_NAME, AlbumAttribute.PVCount, albumID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, int albumID)
        {
            if (albumID > 0)
            {
                List<int> albumIDList = new List<int>();
                albumIDList.Add(albumID);
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(albumIDList));

                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", AlbumDAO.TABLE_NAME, albumID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> albumIDList)
        {
            if (albumIDList != null && albumIDList.Count > 0)
            {
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(albumIDList));

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", AlbumDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(albumIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        private List<int> GetKeywordIDList(List<int> albumIDList)
        {
            List<int> keywordIDList = new List<int>();

            string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", AlbumAttribute.KeywordID, AlbumDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(albumIDList));

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

        public AlbumInfo GetAlbumInfo(int albumID)
        {
            AlbumInfo albumInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", albumID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AlbumDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    albumInfo = new AlbumInfo(rdr);
                }
                rdr.Close();
            }

            return albumInfo;
        }

        public List<AlbumInfo> GetAlbumInfoListByKeywordID(int publishmentSystemID, int keywordID)
        {
            List<AlbumInfo> albumInfoList = new List<AlbumInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} <> '{3}'", AlbumAttribute.PublishmentSystemID, publishmentSystemID, AlbumAttribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", AlbumAttribute.KeywordID, keywordID);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    AlbumInfo albumInfo = new AlbumInfo(rdr);
                    albumInfoList.Add(albumInfo);
                }
                rdr.Close();
            }

            return albumInfoList;
        }

        public int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} <> '{4}' AND {5} = {6}", TABLE_NAME, AlbumAttribute.PublishmentSystemID, publishmentSystemID, AlbumAttribute.IsDisabled, true, AlbumAttribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetTitle(int albumID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", albumID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AlbumDAO.TABLE_NAME, 0, AlbumAttribute.Title, SQL_WHERE, null);

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
            string whereString = string.Format("WHERE {0} = {1}", AlbumAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(AlbumDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<AlbumInfo> GetAlbumInfoList(int publishmentSystemID)
        {
            List<AlbumInfo> albumInfoList = new List<AlbumInfo>();

            string SQL_WHERE = string.Format(" AND {0} = {1}", AlbumAttribute.PublishmentSystemID, publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    AlbumInfo albumInfo = new AlbumInfo(rdr);
                    albumInfoList.Add(albumInfo);
                }
                rdr.Close();
            }

            return albumInfoList;
        }
        
    }
}
