using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.BBS.Core;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class AnnouncementDAO : DataProviderBase, IAnnouncementDAO
    {
        public IList<AnnouncementInfo> GetAnnouncements(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, Taxis, AddDate, Title, FormatString, LinkUrl, IsBlank FROM bbs_Announcement WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC", publishmentSystemID);

            IList<AnnouncementInfo> list = new List<AnnouncementInfo>();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    AnnouncementInfo info = new AnnouncementInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetDateTime(3), rdr.GetString(4), rdr.GetString(5), rdr.GetString(6), TranslateUtils.ToBool(rdr.GetString(7)));
                    list.Add(info);
                }
                rdr.Close();
            }

            return list;
        }

        public AnnouncementInfo GetAnnouncementInfo(int id)
        {
            AnnouncementInfo announcementInfo = null;
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, Taxis, AddDate, Title, FormatString, LinkUrl, IsBlank FROM bbs_Announcement WHERE ID= {0}", id);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    announcementInfo = new AnnouncementInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetDateTime(3), rdr.GetString(4), rdr.GetString(5), rdr.GetString(6), TranslateUtils.ToBool(rdr.GetString(7)));
                }
                rdr.Close();
            }

            return announcementInfo;
        }

        public void Delete(int id)
        {
            string sqlString = string.Format("DELETE FROM bbs_Announcement WHERE ID = {0}", id);
            this.ExecuteNonQuery(sqlString);
        }

        public void Update(AnnouncementInfo info)
        {
            string sqlString = "UPDATE bbs_Announcement SET Title=@Title, FormatString=@FormatString, LinkUrl=@LinkUrl, IsBlank = @IsBlank WHERE ID=@ID";

            IDbDataParameter[] param = new IDbDataParameter[]
			{
                this.GetParameter("@Title", EDataType.NVarChar, 255, info.Title),
                this.GetParameter("@FormatString", EDataType.VarChar, 50, info.FormatString),
                this.GetParameter("@LinkUrl", EDataType.VarChar, 200, info.LinkUrl),
                this.GetParameter("@IsBlank", EDataType.VarChar, 18, info.IsBlank.ToString()),
                this.GetParameter("@ID", EDataType.Integer, info.ID)
            };

            this.ExecuteNonQuery(sqlString, param);
        }

        public void Insert(AnnouncementInfo info)
        {
            string sqlString = "INSERT INTO bbs_Announcement(PublishmentSystemID, Taxis, AddDate, Title, FormatString, LinkUrl, IsBlank) VALUES(@PublishmentSystemID, @Taxis, @AddDate, @Title, @FormatString, @LinkUrl, @IsBlank)";

            info.Taxis = this.GetMaxTaxis(info.PublishmentSystemID) + 1;

            IDbDataParameter[] param = new IDbDataParameter[]
			{
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter("@Taxis", EDataType.Integer, info.Taxis),
                this.GetParameter("@AddDate", EDataType.DateTime, info.AddDate),
                this.GetParameter("@Title", EDataType.NVarChar, 255, info.Title),
                this.GetParameter("@FormatString", EDataType.VarChar, 50, info.FormatString),
                this.GetParameter("@LinkUrl", EDataType.VarChar, 200, info.LinkUrl),
                this.GetParameter("@IsBlank", EDataType.VarChar, 18, info.IsBlank.ToString()),
            };

            this.ExecuteNonQuery(sqlString, param);
        }

        public string GetSqlString(int publishmentSystemID)
        {
            return string.Format("SELECT ID, PublishmentSystemID, Taxis, AddDate, Title, FormatString, LinkUrl, IsBlank FROM bbs_Announcement WHERE PublishmentSystemID = {0}", publishmentSystemID);
        }

        public void UpdateTaxisToUp(int publishmentSystemID, int id)
        {
            //Get Higher Taxis and ClassID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bbs_Announcement WHERE Taxis > (SELECT Taxis FROM bbs_Announcement WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis", id, publishmentSystemID);
            int HigherID = 0;
            int HigherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    HigherID = rdr.GetInt32(0);
                    HigherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            //Get Taxis Of Selected Class
            int SelectedTaxis = GetTaxis(id);

            if (HigherID != 0)
            {
                //Set The Selected Class Taxis To Higher Level
                SetTaxis(id, HigherTaxis);
                //Set The Higher Class Taxis To Lower Level
                SetTaxis(HigherID, SelectedTaxis);
            }
        }

        public void UpdateTaxisToDown(int publishmentSystemID, int id)
        {
            //Get Lower Taxis and ClassID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bbs_Announcement WHERE Taxis < (SELECT Taxis FROM bbs_Announcement WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis DESC", id, publishmentSystemID);
            int LowerID = 0;
            int LowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    LowerID = rdr.GetInt32(0);
                    LowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            //Get Taxis Of Selected Class
            int SelectedTaxis = GetTaxis(id);

            if (LowerID != 0)
            {
                //Set The Selected Class Taxis To Lower Level
                SetTaxis(id, LowerTaxis);
                //Set The Lower Class Taxis To Higher Level
                SetTaxis(LowerID, SelectedTaxis);
            }
        }

        private int GetTaxis(int id)
        {
            string sqlString = string.Format("SELECT Taxis FROM bbs_Announcement WHERE ID = {0}", id);
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
            string sqlString = string.Format("UPDATE bbs_Announcement SET Taxis = {0} WHERE ID = {1}", taxis, id);

            this.ExecuteNonQuery(sqlString);
        }

        public int GetMaxTaxis(int publishmentSystemID)
        {
            int maxTaxis = 0;
            string sqlString = string.Format("SELECT MAX(Taxis) FROM bbs_Announcement WHERE Taxis <> {0} AND PublishmentSystemID = {1}", int.MaxValue, publishmentSystemID);
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
            string sqlString = string.Format("SELECT MIN(Taxis) FROM bbs_Announcement WHERE PublishmentSystemID = {0}", publishmentSystemID);
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
    }
}