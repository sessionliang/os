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
    public class AlbumContentDAO : DataProviderBase, IAlbumContentDAO
    {
        private const string TABLE_NAME = "wx_AlbumContent";
         
        private const string SQL_SELECT_ALL = "SELECT ID, PublishmentSystemID, AlbumID, ParentID, Title, ImageUrl, LargeImageUrl FROM wx_AlbumContent WHERE PublishmentSystemID = @PublishmentSystemID AND AlbumID = @AlbumID AND ParentID = 0  ORDER BY ID ASC";

        private const string SQL_SELECT_ALL_BY_PARENTID = "SELECT ID, PublishmentSystemID, AlbumID, ParentID, Title, ImageUrl, LargeImageUrl FROM wx_AlbumContent WHERE PublishmentSystemID = @PublishmentSystemID AND AlbumID = @AlbumID  AND  ParentID=@ParentID AND ParentID <> 0  ORDER BY ID ASC";
         
        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_ALBUM_ID = "@AlbumID";
        private const string PARM_PARENT_ID = "@ParentID";
        private const string PARM_TITLE = "@Title";
        private const string PARM_IMAGEURL = "@ImageUrl";
        private const string PARM_LARGEIMAGEURL = "@LargeImageUrl";
         
        public int Insert(AlbumContentInfo albumContentInfo)
        {
            int albumID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(albumContentInfo.ToNameValueCollection(), this.ConnectionString, AlbumContentDAO.TABLE_NAME, out parms);
             
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        albumID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, AlbumContentDAO.TABLE_NAME);

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

        public void Update(AlbumContentInfo albumContentInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(albumContentInfo.ToNameValueCollection(), this.ConnectionString, AlbumContentDAO.TABLE_NAME, out parms);
             
            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int publishmentSystemID, int albumContentID)
        {
            if (albumContentID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1}= {2} AND {3}= {4}", AlbumContentDAO.TABLE_NAME,AlbumContentAttribute.PublishmentSystemID, publishmentSystemID,AlbumContentAttribute.ID, albumContentID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> albumContentIDList)
        {
            if (albumContentIDList != null && albumContentIDList.Count > 0)
            {
               string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2} AND ID IN ({3})", AlbumContentDAO.TABLE_NAME,AlbumContentAttribute.PublishmentSystemID, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(albumContentIDList));
               this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteByParentID(int publishmentSystemID, int parentID)
        {
            if (parentID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2} AND {3} = {4}", AlbumContentDAO.TABLE_NAME,AlbumContentAttribute.PublishmentSystemID, publishmentSystemID,AlbumContentAttribute.ParentID, parentID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public AlbumContentInfo GetAlbumContentInfo(int albumContentID)
        {
            AlbumContentInfo albumContentInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", albumContentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AlbumContentDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    albumContentInfo = new AlbumContentInfo(rdr);
                }
                rdr.Close();
            }

            return albumContentInfo;
        }

        public List<AlbumContentInfo> GetAlbumContentInfoList(int publishmentSystemID, int albumID, int parentID)
        {
            List<AlbumContentInfo> list = new List<AlbumContentInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3} AND {4} = {5}",AlbumContentAttribute.PublishmentSystemID, publishmentSystemID,AlbumContentAttribute.AlbumID, albumID, AlbumContentAttribute.ParentID, parentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AlbumContentDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    AlbumContentInfo albumContentInfo = new AlbumContentInfo(rdr);
                    list.Add(albumContentInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<int> GetAlbumContentIDList(int publishmentSystemID, int albumID, int parentID)
        {
            List<int> list = new List<int>();
            string sqlString = string.Format("SELECT ID FROM {0} WHERE {1} = {2} AND {3} = {4} AND {5} = {6} ORDER BY ID",AlbumContentDAO.TABLE_NAME,AlbumContentAttribute.PublishmentSystemID, publishmentSystemID,AlbumContentAttribute.AlbumID,albumID, AlbumContentAttribute.ParentID, parentID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return list;
        }
  
        public string GetTitle(int albumContentID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", albumContentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AlbumContentDAO.TABLE_NAME, 0, AlbumContentAttribute.Title, SQL_WHERE, null);

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

        public int GetCount(int publishmentSystemID, int parentID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = {4}", AlbumContentDAO.TABLE_NAME,AlbumContentAttribute.PublishmentSystemID, publishmentSystemID,AlbumContentAttribute.ParentID, parentID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }
         
        public IEnumerable GetDataSource(int publishmentSystemID, int albumID)
        {
            
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_ALBUM_ID,EDataType.Integer,albumID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL, parms);
            return enumerable;
        }

        public IEnumerable GetDataSource(int publishmentSystemID, int albumID,int parentID)
        {

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_ALBUM_ID,EDataType.Integer,albumID),
                this.GetParameter(PARM_PARENT_ID,EDataType.Integer,parentID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_BY_PARENTID, parms);
            return enumerable;
        }

        public List<AlbumContentInfo> GetAlbumContentInfoList(int publishmentSystemID)
        {
            List<AlbumContentInfo> list = new List<AlbumContentInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}",AlbumContentAttribute.PublishmentSystemID, publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AlbumContentDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    AlbumContentInfo albumContentInfo = new AlbumContentInfo(rdr);
                    list.Add(albumContentInfo);
                }
                rdr.Close();
            }

            return list;
        }
        
    }
}
