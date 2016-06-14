using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.BBS.Core;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class FaceDAO : DataProviderBase, IFaceDAO
    {
        public List<FaceInfo> GetFaces(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, FaceName, Title, Taxis, IsEnabled FROM bbs_Face WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC", publishmentSystemID);

            List<FaceInfo> list = new List<FaceInfo>();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    FaceInfo info = new FaceInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), TranslateUtils.ToBool(rdr.GetString(5)));
                    list.Add(info);
                }
                rdr.Close();
            }

            return list;
        }

        public List<FaceInfo> GetFaces(int publishmentSystemID, bool isEnabled)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, FaceName, Title, Taxis, IsEnabled FROM bbs_Face WHERE PublishmentSystemID = {0} AND IsEnabled = '{1}' ORDER BY Taxis DESC", publishmentSystemID, isEnabled.ToString());

            List<FaceInfo> list = new List<FaceInfo>();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    FaceInfo info = new FaceInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), TranslateUtils.ToBool(rdr.GetString(5)));
                    list.Add(info);
                }
                rdr.Close();
            }

            return list;
        }

        public FaceInfo GetFaceInfo(int publishmentSystemID, int id)
        {
            FaceInfo faceInfo = null;
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, FaceName, Title, Taxis, IsEnabled FROM bbs_Face WHERE PublishmentSystemID = {0} AND ID = {1}", publishmentSystemID, id);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    faceInfo = new FaceInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), TranslateUtils.ToBool(rdr.GetString(5)));
                }
                rdr.Close();
            }

            return faceInfo;
        }

        public string GetFirstFaceName(int publishmentSystemID)
        {
            string firstFaceName = string.Empty;

            string sqlString = string.Format("SELECT TOP 1 FaceName FROM bbs_Face WHERE PublishmentSystemID = {0} AND IsEnabled = '{1}' ORDER BY Taxis DESC", publishmentSystemID, true.ToString());

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    firstFaceName = rdr.GetString(0);
                }
                rdr.Close();
            }

            return firstFaceName;
        }

        public void Delete(int publishmentSystemID, int id)
        {
            string sqlString = string.Format("DELETE FROM bbs_Face WHERE PublishmentSystemID = {0} AND ID = {1}", publishmentSystemID, id);
            this.ExecuteNonQuery(sqlString);
        }

        public void Update(FaceInfo info)
        {
            string sqlString = "UPDATE bbs_Face SET Title=@Title, IsEnabled=@IsEnabled WHERE FaceName=@FaceName";

            IDbDataParameter[] param = new IDbDataParameter[]
			{
                this.GetParameter("@Title", EDataType.NVarChar, 50, info.Title),
                this.GetParameter("@IsEnabled", EDataType.VarChar, 18, info.IsEnabled.ToString()),
                this.GetParameter("@FaceName", EDataType.VarChar, 50, info.FaceName)
            };

            this.ExecuteNonQuery(sqlString, param);
        }

        public void Insert(FaceInfo info)
        {
            string sqlString = "INSERT INTO bbs_Face(PublishmentSystemID, FaceName, Title, Taxis, IsEnabled) VALUES(@PublishmentSystemID, @FaceName, @Title, @Taxis, @IsEnabled)";

            info.Taxis = this.GetMaxTaxis(info.PublishmentSystemID) + 1;

            IDbDataParameter[] param = new IDbDataParameter[]
			{
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter("@FaceName", EDataType.VarChar, 50, info.FaceName),
                this.GetParameter("@Title", EDataType.NVarChar, 50, info.Title),
                this.GetParameter("@Taxis", EDataType.Integer, info.Taxis),
                this.GetParameter("@IsEnabled", EDataType.VarChar, 18, info.IsEnabled.ToString())
            };

            this.ExecuteNonQuery(sqlString, param);
        }

        public void UpdateTaxisToUp(int publishmentSystemID, int id)
        {
            //Get Higher Taxis and ClassID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bbs_Face WHERE Taxis > (SELECT Taxis FROM bbs_Face WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis", id, publishmentSystemID);
            
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

            //Get Taxis Of Selected Class
            int selectedTaxis = GetTaxis(id);

            if (higherID > 0)
            {
                //Set The Selected Class Taxis To Higher Level
                SetTaxis(id, higherTaxis);
                //Set The Higher Class Taxis To Lower Level
                SetTaxis(higherID, selectedTaxis);
            }
        }

        public void UpdateTaxisToDown(int publishmentSystemID, int id)
        {
            //Get Lower Taxis and ClassID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bbs_Face WHERE Taxis < (SELECT Taxis FROM bbs_Face WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis DESC", id, publishmentSystemID);

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

            //Get Taxis Of Selected Class
            int selectedTaxis = GetTaxis(id);

            if (lowerID > 0)
            {
                //Set The Selected Class Taxis To Lower Level
                SetTaxis(id, lowerTaxis);
                //Set The Lower Class Taxis To Higher Level
                SetTaxis(lowerID, selectedTaxis);
            }
        }

        private int GetTaxis(int id)
        {
            string sqlString = string.Format("SELECT Taxis FROM bbs_Face WHERE ID = {0}", id);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    taxis = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int id, int taxis)
        {
            string sqlString = string.Format("UPDATE bbs_Face SET Taxis = {0} WHERE ID = {1}", taxis, id);

            this.ExecuteNonQuery(sqlString);
        }

        public int GetMaxTaxis(int publishmentSystemID)
        {
            int maxTaxis = 0;
            string sqlString = string.Format("SELECT MAX(Taxis) FROM bbs_Face WHERE Taxis <> {0} AND PublishmentSystemID = {1}", int.MaxValue, publishmentSystemID);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                object o = this.ExecuteScalar(conn, sqlString);
                if (o is System.DBNull)
                    maxTaxis = 0;
                else
                    maxTaxis = int.Parse(o.ToString());
            }
            return maxTaxis;
        }

        public int GetMinTaxis(int publishmentSystemID)
        {
            int minTaxis = 0;
            string sqlString = string.Format("SELECT MIN(Taxis) FROM bbs_Face WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                object o = this.ExecuteScalar(conn, sqlString);
                if (o is System.DBNull)
                    minTaxis = 0;
                else
                    minTaxis = int.Parse(o.ToString());
            }
            return minTaxis;
        }

        public void CreateDefaultFace(int publishmentSystemID)
        {
            bool isExists = false;

            string sqlString = string.Format("SELECT FaceName FROM bbs_Face WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    isExists = true;
                }
                rdr.Close();
            }

            if (!isExists)
            {
                FaceInfo faceInfo = new FaceInfo(0, publishmentSystemID, "oniontou", "Ñó´ÐÍ·", 0, true);
                Insert(faceInfo);

                faceInfo = new FaceInfo(0, publishmentSystemID, "tuzki", "ÍÃË¹»ù", 0, true);
                Insert(faceInfo);

                faceInfo = new FaceInfo(0, publishmentSystemID, "yoci", "ÓÆÎûºï", 0, true);
                Insert(faceInfo);
            }
        }
    }
}