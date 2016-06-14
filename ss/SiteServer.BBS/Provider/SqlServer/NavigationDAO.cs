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
    public class NavigationDAO : DataProviderBase, INavigationDAO
    {
        public List<NavigationInfo> GetNavigations(int publishmentSystemID, ENavType navType)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, Taxis, NavType, Title, FormatString, LinkUrl, IsBlank FROM bbs_Navigation WHERE PublishmentSystemID = {0} AND NavType = '{1}' ORDER BY Taxis", publishmentSystemID, ENavTypeUtils.GetValue(navType));

            List<NavigationInfo> list = new List<NavigationInfo>();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    NavigationInfo info = new NavigationInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), ENavTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), TranslateUtils.ToBool(rdr.GetValue(7).ToString()));
                    list.Add(info);
                }
                rdr.Close();
            }

            return list;
        }

        public NavigationInfo GetNavigationInfo(int id)
        {
            NavigationInfo NavigationInfo = null;
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, Taxis, NavType, Title, FormatString, LinkUrl, IsBlank FROM bbs_Navigation WHERE ID = {0}", id);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    NavigationInfo = new NavigationInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), ENavTypeUtils.GetEnumType(rdr.GetString(3)), rdr.GetString(4), rdr.GetString(5), rdr.GetString(6), TranslateUtils.ToBool(rdr.GetString(7)));
                }
                rdr.Close();
            }

            return NavigationInfo;
        }

        public void Delete(int id)
        {
            string sqlString = "DELETE FROM bbs_Navigation WHERE ID = @ID";

            IDbDataParameter[] param = new IDbDataParameter[]
			{
                this.GetParameter("@ID", EDataType.Integer, id)
            };

            this.ExecuteNonQuery(sqlString, param);
        }

        public void Update(NavigationInfo info)
        {
            string sqlString = "UPDATE bbs_Navigation SET Title=@Title, FormatString=@FormatString, LinkUrl=@LinkUrl, IsBlank = @IsBlank WHERE ID=@ID";

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

        public void Insert(NavigationInfo info)
        {
            string sqlString = "INSERT INTO bbs_Navigation(PublishmentSystemID, Taxis, NavType, Title, FormatString, LinkUrl, IsBlank) VALUES(@PublishmentSystemID, @Taxis, @NavType, @Title, @FormatString, @LinkUrl, @IsBlank)";

            info.Taxis = this.GetMaxTaxis(info.PublishmentSystemID, info.NavType) + 1;

            IDbDataParameter[] param = new IDbDataParameter[]
			{
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter("@Taxis", EDataType.Integer, info.Taxis),
                this.GetParameter("@NavType", EDataType.VarChar, 50, ENavTypeUtils.GetValue(info.NavType)),
                this.GetParameter("@Title", EDataType.NVarChar, 255, info.Title),
                this.GetParameter("@FormatString", EDataType.VarChar, 50, info.FormatString),
                this.GetParameter("@LinkUrl", EDataType.VarChar, 200, info.LinkUrl),
                this.GetParameter("@IsBlank", EDataType.VarChar, 18, info.IsBlank.ToString()),
            };

            this.ExecuteNonQuery(sqlString, param);
        }

        public string GetSqlString(int publishmentSystemID, ENavType navType)
        {
            return string.Format("SELECT ID, PublishmentSystemID, Taxis, NavType, Title, FormatString, LinkUrl, IsBlank FROM bbs_Navigation WHERE PublishmentSystemID = {0} AND NavType = '{1}' ORDER BY Taxis", publishmentSystemID, ENavTypeUtils.GetValue(navType));
        }

        public void UpdateTaxisToDown(int publishmentSystemID, int id, ENavType navType)
        {
            //Get Higher Taxis and ClassID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bbs_Navigation WHERE (Taxis > (SELECT Taxis FROM bbs_Navigation WHERE ID = {0}) AND PublishmentSystemID = {1} AND NavType = '{2}') ORDER BY Taxis", id, publishmentSystemID, ENavTypeUtils.GetValue(navType));

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

        public void UpdateTaxisToUp(int publishmentSystemID, int id, ENavType navType)
        {
            //Get Lower Taxis and ClassID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bbs_Navigation WHERE (Taxis < (SELECT Taxis FROM bbs_Navigation WHERE ID = {0}) AND PublishmentSystemID = {1} AND NavType = '{2}') ORDER BY Taxis DESC", id, publishmentSystemID, ENavTypeUtils.GetValue(navType));
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
            string sqlString = string.Format("SELECT Taxis FROM bbs_Navigation WHERE ID = {0}", id);
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
            string sqlString = string.Format("UPDATE bbs_Navigation SET Taxis = {0} WHERE ID = {1}", taxis, id);

            this.ExecuteNonQuery(sqlString);
        }

        public int GetMaxTaxis(int publishmentSystemID, ENavType navType)
        {
            int maxTaxis = 0;
            string sqlString = string.Format("SELECT MAX(Taxis) FROM bbs_Navigation WHERE PublishmentSystemID = {0} AND Taxis != {1} AND NavType = '{2}'", publishmentSystemID, int.MaxValue, ENavTypeUtils.GetValue(navType));
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

        public int GetMinTaxis(int publishmentSystemID, ENavType navType)
        {
            int minTaxis = 0;
            string sqlString = string.Format("SELECT MIN(Taxis) WHERE PublishmentSystemID = {0} AND NavType = '{1}' FROM bbs_Navigation", publishmentSystemID, ENavTypeUtils.GetValue(navType));
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

        public void CreateDefaultNavigation(int publishmentSystemID)
        {
            bool isExists = false;

            string sqlString = string.Format("SELECT ID FROM bbs_Navigation WHERE PublishmentSystemID = {0}", publishmentSystemID);

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
                NavigationInfo navigationInfo = new NavigationInfo(0, publishmentSystemID, 0, ENavType.Header, "ÂÛÌ³", string.Empty, "default.aspx", false);
                Insert(navigationInfo);
            }
        }
    }
}