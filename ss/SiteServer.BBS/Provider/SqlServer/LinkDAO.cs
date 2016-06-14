using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using BaiRong.Core.Data.Provider;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class LinkDAO : DataProviderBase, ILinkDAO
    {
        public void Insert(LinkInfo info)
        {
            int maxTaxis = this.GetMaxTaxis(info.PublishmentSystemID);
            info.Taxis = maxTaxis + 1;

            string sqlString = "INSERT INTO bbs_Link(PublishmentSystemID, LinkName, LinkUrl, IconUrl, Taxis) VALUES(@PublishmentSystemID, @LinkName, @LinkUrl, @IconUrl, @Taxis)";
            IDbDataParameter[] param = new IDbDataParameter[]
            {
                this.GetParameter("@PublishmentSystemID", EDataType.Integer,  info.PublishmentSystemID),
                this.GetParameter("@LinkName", EDataType.NVarChar, 50, info.LinkName),
                this.GetParameter("@LinkUrl", EDataType.VarChar, 200,  info.LinkUrl),
                this.GetParameter("@IconUrl", EDataType.VarChar, 200,  info.IconUrl),
                this.GetParameter("@Taxis", EDataType.Integer,  info.Taxis)
            };

            this.ExecuteNonQuery(sqlString, param);
        }

        public void Update(LinkInfo info)
        {
            string sqlString = "UPDATE bbs_Link SET LinkName=@LinkName,LinkUrl=@LinkUrl, IconUrl=@IconUrl WHERE ID=@ID";
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter("@LinkName", EDataType.NVarChar, 50, info.LinkName),
                this.GetParameter("@LinkUrl", EDataType.VarChar, 200, info.LinkUrl),
                this.GetParameter("@IconUrl", EDataType.VarChar, 200, info.IconUrl),
                this.GetParameter("@ID", EDataType.Integer,  info.ID)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(int id)
        {
            string sqlString = "DELETE FROM bbs_Link WHERE ID = @ID";

            IDbDataParameter[] param = new IDbDataParameter[]
			{
                this.GetParameter("@ID", EDataType.Integer, id)
            };

            this.ExecuteNonQuery(sqlString, param);
        }

        public LinkInfo GetLinksInfo(int id)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, LinkName, LinkUrl, IconUrl, Taxis FROM bbs_Link WHERE ID = {0}", id);

            LinkInfo linksInfo = null;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    linksInfo = new LinkInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5));
                }
                rdr.Close();
            }
            return linksInfo;
        }

        public List<LinkInfo> GetLinksList(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, LinkName, LinkUrl, IconUrl, Taxis FROM bbs_Link WHERE PublishmentSystemID = {0}", publishmentSystemID);

            List<LinkInfo> list = new List<LinkInfo>();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    LinkInfo linksInfo = new LinkInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5));
                    list.Add(linksInfo);
                }
                rdr.Close();
            }
            return list;
        }

        public string GetSelectCommend(int publishmentSystemID)
        {
            return string.Format("SELECT ID, PublishmentSystemID, LinkName, LinkUrl, IconUrl, Taxis FROM bbs_Link WHERE PublishmentSystemID = {0}", publishmentSystemID);
        }

        public string GetSqlString(int publishmentSystemID, bool isIcon)
        {
            if (isIcon)
            {
                return string.Format("SELECT ID, LinkName, LinkUrl, IconUrl, Taxis FROM bbs_Link WHERE PublishmentSystemID = {0} AND IconUrl <> ''", publishmentSystemID);
            }
            else
            {
                return string.Format("SELECT ID, LinkName, LinkUrl, IconUrl, Taxis FROM bbs_Link WHERE PublishmentSystemID = {0} AND IconUrl = ''", publishmentSystemID);
            }
        }

        private int GetTaxis(int id)
        {
            string sqlString = string.Format("SELECT Taxis FROM bbs_Link WHERE ID = {0}", id);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private void SetTaxis(int id, int taxis)
        {
            string sqlString = string.Format("UPDATE bbs_Link SET Taxis = {0} WHERE ID = {1}", taxis, id);
            this.ExecuteNonQuery(sqlString);
        }

        private int GetMaxTaxis(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM bbs_Link WHERE PublishmentSystemID = {0}", publishmentSystemID);

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

        public bool UpdateTaxisToUp(int publishmentSystemID, int id)
        {
            //Get Higher Taxis and ID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bbs_Link WHERE Taxis > (SELECT Taxis FROM bbs_Link WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis", id, publishmentSystemID);

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

        public bool UpdateTaxisToDown(int publishmentSystemID, int id)
        {
            //Get Lower Taxis and ID
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bbs_Link WHERE Taxis < (SELECT Taxis FROM bbs_Link WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis DESC", id, publishmentSystemID);

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