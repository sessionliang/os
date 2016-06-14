using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Collections.Generic;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class PhotoDAO : DataProviderBase, IPhotoDAO
    {
        private const string SQL_UPDATE_PHOTO_CONTENT = "UPDATE siteserver_Photo SET SmallUrl = @SmallUrl, MiddleUrl = @MiddleUrl, LargeUrl = @LargeUrl, Taxis = @Taxis, Description = @Description WHERE ID = @ID";
        private const string SQL_DELETE_PHOTO_CONTENT = "DELETE FROM siteserver_Photo WHERE ID = @ID";
        private const string SQL_DELETE_PHOTO_CONTENTS = "DELETE FROM siteserver_Photo WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID";

        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_CONTENTID = "@ContentID";
        private const string PARM_SMALL_URL = "@SmallUrl";
        private const string PARM_MIDDLE_URL = "@MiddleUrl";
        private const string PARM_LARGE_URL = "@LargeUrl";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_DESCRIPTION = "@Description";


        public void Insert(PhotoInfo photoInfo)
        {
            int maxTaxis = this.GetMaxTaxis(photoInfo.PublishmentSystemID, photoInfo.ContentID);
            photoInfo.Taxis = maxTaxis + 1;

            string sqlString = "INSERT INTO siteserver_Photo (PublishmentSystemID, ContentID, SmallUrl, MiddleUrl, LargeUrl, Taxis, Description) VALUES (@PublishmentSystemID, @ContentID, @SmallUrl, @MiddleUrl, @LargeUrl, @Taxis, @Description)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_Photo (ID, PublishmentSystemID, ContentID, SmallUrl, MiddleUrl, LargeUrl, Taxis, Description) VALUES (siteserver_Photo_SEQ.NEXTVAL, @PublishmentSystemID, @ContentID, @SmallUrl, @MiddleUrl, @LargeUrl, @Taxis, @Description)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, photoInfo.PublishmentSystemID),
                this.GetParameter(PARM_CONTENTID, EDataType.Integer, photoInfo.ContentID),
                this.GetParameter(PARM_SMALL_URL, EDataType.VarChar, 200, photoInfo.SmallUrl),
                this.GetParameter(PARM_MIDDLE_URL, EDataType.VarChar, 200, photoInfo.MiddleUrl),
                this.GetParameter(PARM_LARGE_URL, EDataType.VarChar, 200, photoInfo.LargeUrl),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, photoInfo.Taxis),
				this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, photoInfo.Description)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Update(PhotoInfo photoInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_SMALL_URL, EDataType.VarChar, 200, photoInfo.SmallUrl),
                this.GetParameter(PARM_MIDDLE_URL, EDataType.VarChar, 200, photoInfo.MiddleUrl),
                this.GetParameter(PARM_LARGE_URL, EDataType.VarChar, 200, photoInfo.LargeUrl),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, photoInfo.Taxis),
				this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, photoInfo.Description),
                this.GetParameter(PARM_ID, EDataType.Integer, photoInfo.ID),
			};

            this.ExecuteNonQuery(SQL_UPDATE_PHOTO_CONTENT, updateParms);
        }

        public void Delete(int id)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_ID, EDataType.Integer, id)
			};

            this.ExecuteNonQuery(SQL_DELETE_PHOTO_CONTENT, parms);
        }

        public void Delete(List<int> idList)
        {
            if (idList != null && idList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM siteserver_Photo WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, int contentID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CONTENTID, EDataType.Integer, contentID)
			};

            this.ExecuteNonQuery(SQL_DELETE_PHOTO_CONTENTS, parms);
        }

        public PhotoInfo GetPhotoInfo(int id)
        {
            PhotoInfo photoInfo = null;

            string sqlString = string.Format("SELECT ID, PublishmentSystemID, ContentID, SmallUrl, MiddleUrl, LargeUrl, Taxis, Description FROM siteserver_Photo WHERE ID = {0}", id);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    photoInfo = new PhotoInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetInt32(6), rdr.GetValue(7).ToString());
                }
                rdr.Close();
            }

            return photoInfo;
        }

        public PhotoInfo GetFirstPhotoInfo(int publishmentSystemID, int contentID)
        {
            PhotoInfo photoInfo = null;

            string sqlString = string.Format("SELECT TOP 1 ID, PublishmentSystemID, ContentID, SmallUrl, MiddleUrl, LargeUrl, Taxis, Description FROM siteserver_Photo WHERE PublishmentSystemID = {0} AND ContentID = {1} ORDER BY Taxis", publishmentSystemID, contentID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    photoInfo = new PhotoInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetInt32(6), rdr.GetValue(7).ToString());
                }
                rdr.Close();
            }

            return photoInfo;
        }

        public int GetCount(int publishmentSystemID, int contentID)
        {
            string sqlString = string.Format("SELECT Count(*) FROM siteserver_Photo WHERE PublishmentSystemID = {0} AND ContentID = {1}", publishmentSystemID, contentID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSortFieldName()
        {
            return "Taxis";
        }

        public string GetSelectSqlString(int publishmentSystemID, int contentID)
        {
            return string.Format("SELECT ID, PublishmentSystemID, ContentID, SmallUrl, MiddleUrl, LargeUrl, Taxis, Description FROM siteserver_Photo WHERE PublishmentSystemID = {0} AND ContentID = {1} ORDER BY Taxis", publishmentSystemID, contentID);
        }

        public IEnumerable GetStlDataSource(int publishmentSystemID, int contentID, int startNum, int totalNum)
        {
            string tableName = "siteserver_Photo";
            string orderByString = "ORDER BY Taxis";
            string whereString = string.Format("WHERE (PublishmentSystemID = {0} AND ContentID = {1})", publishmentSystemID, contentID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, startNum, totalNum, SqlUtils.Asterisk, whereString, orderByString);

            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }

        public List<int> GetPhotoContentIDList(int publishmentSystemID, int contentID)
        {
            List<int> list = new List<int>();

            string sqlString = string.Format("SELECT ID FROM siteserver_Photo WHERE PublishmentSystemID = {0} AND ContentID = {1} ORDER BY Taxis", publishmentSystemID, contentID);

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

        public List<PhotoInfo> GetPhotoInfoList(int publishmentSystemID, int contentID)
        {
            List<PhotoInfo> list = new List<PhotoInfo>();

            string sqlString = string.Format("SELECT ID, PublishmentSystemID, ContentID, SmallUrl, MiddleUrl, LargeUrl, Taxis, Description FROM siteserver_Photo WHERE PublishmentSystemID = {0} AND ContentID = {1} ORDER BY Taxis", publishmentSystemID, contentID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    list.Add(new PhotoInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetInt32(6), rdr.GetValue(7).ToString()));
                }
                rdr.Close();
            }

            return list;
        }

        private int GetTaxis(int id)
        {
            string sqlString = string.Format("SELECT Taxis FROM siteserver_Photo WHERE (ID = {0})", id);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private void SetTaxis(int id, int taxis)
        {
            string sqlString = string.Format("UPDATE siteserver_Photo SET Taxis = {0} WHERE (ID = {1})", taxis, id);
            this.ExecuteNonQuery(sqlString);
        }

        private int GetMaxTaxis(int publishmentSystemID, int contentID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM siteserver_Photo WHERE (PublishmentSystemID = {0} AND ContentID = {1})", publishmentSystemID, contentID);
            int maxTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        maxTaxis = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return maxTaxis;
        }

        public bool UpdateTaxisToUp(int publishmentSystemID, int contentID, int id)
        {
            //Get Higher Taxis and ID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM siteserver_Photo WHERE (Taxis > (SELECT Taxis FROM siteserver_Photo WHERE ID = {0}) AND (PublishmentSystemID = {1} AND ContentID = {2})) ORDER BY Taxis", id, publishmentSystemID, contentID);

            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = rdr.GetInt32(0);
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            if (higherID > 0)
            {
                //Get Taxis Of Selected ID
                int selectedTaxis = GetTaxis(id);

                //Set The Selected Class Taxis To Higher Level
                SetTaxis(id, higherTaxis);
                //Set The Higher Class Taxis To Lower Level
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int publishmentSystemID, int contentID, int id)
        {
            //Get Lower Taxis and ID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM siteserver_Photo WHERE (Taxis < (SELECT Taxis FROM siteserver_Photo WHERE ID = {0}) AND (PublishmentSystemID = {1} AND ContentID = {2})) ORDER BY Taxis DESC", id, publishmentSystemID, contentID);

            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = rdr.GetInt32(0);
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            if (lowerID > 0)
            {
                //Get Taxis Of Selected Class
                int selectedTaxis = GetTaxis(id);

                //Set The Selected Class Taxis To Lower Level
                SetTaxis(id, lowerTaxis);
                //Set The Lower Class Taxis To Higher Level
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

    }
}