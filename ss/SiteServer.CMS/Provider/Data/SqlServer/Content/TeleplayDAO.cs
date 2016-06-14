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
    public class TeleplayDAO : DataProviderBase, ITeleplayDAO
    {
        private const string SQL_UPDATE_Teleplay_CONTENT = "UPDATE siteserver_Teleplay SET StillUrl = @StillUrl, Taxis = @Taxis, Description = @Description, Title = @Title WHERE ID = @ID";
        private const string SQL_DELETE_Teleplay_CONTENT = "DELETE FROM siteserver_Teleplay WHERE ID = @ID";
        private const string SQL_DELETE_Teleplay_CONTENTS = "DELETE FROM siteserver_Teleplay WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID";

        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_CONTENTID = "@ContentID";
        private const string PARM_STILL_URL = "@StillUrl";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_TITLE = "@Title";


        public void Insert(TeleplayInfo TeleplayInfo)
        {
            int maxTaxis = this.GetMaxTaxis(TeleplayInfo.PublishmentSystemID, TeleplayInfo.ContentID);
            TeleplayInfo.Taxis = maxTaxis + 1;

            string sqlString = "INSERT INTO siteserver_Teleplay (PublishmentSystemID, ContentID, StillUrl, Taxis, Description, Title) VALUES (@PublishmentSystemID, @ContentID, @StillUrl, @Taxis, @Description, @Title)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_Teleplay (ID, PublishmentSystemID, ContentID, StillUrl, Taxis, Description, Title) VALUES (siteserver_Teleplay_SEQ.NEXTVAL, @PublishmentSystemID, @ContentID, @StillUrl, @Taxis, @Description, @Title)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, TeleplayInfo.PublishmentSystemID),
                this.GetParameter(PARM_CONTENTID, EDataType.Integer, TeleplayInfo.ContentID),
                this.GetParameter(PARM_STILL_URL, EDataType.VarChar, 200, TeleplayInfo.StillUrl),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, TeleplayInfo.Taxis),
				this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, TeleplayInfo.Description),
                this.GetParameter(PARM_TITLE,EDataType.NVarChar,255,TeleplayInfo.Title)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Update(TeleplayInfo TeleplayInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_STILL_URL, EDataType.VarChar, 200, TeleplayInfo.StillUrl),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, TeleplayInfo.Taxis),
				this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, TeleplayInfo.Description),
                this.GetParameter(PARM_TITLE,EDataType.NVarChar,255,TeleplayInfo.Title),
                this.GetParameter(PARM_ID, EDataType.Integer, TeleplayInfo.ID),
			};

            this.ExecuteNonQuery(SQL_UPDATE_Teleplay_CONTENT, updateParms);
        }

        public void Delete(int id)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_ID, EDataType.Integer, id)
			};

            this.ExecuteNonQuery(SQL_DELETE_Teleplay_CONTENT, parms);
        }

        public void Delete(List<int> idList)
        {
            if (idList != null && idList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM siteserver_Teleplay WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idList));
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

            this.ExecuteNonQuery(SQL_DELETE_Teleplay_CONTENTS, parms);
        }

        public TeleplayInfo GetTeleplayInfo(int id)
        {
            TeleplayInfo TeleplayInfo = null;

            string sqlString = string.Format("SELECT ID, PublishmentSystemID, ContentID, StillUrl, Taxis, Description, Title FROM siteserver_Teleplay WHERE ID = {0}", id);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    TeleplayInfo = new TeleplayInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetValue(5).ToString(),rdr.GetValue(6).ToString());
                }
                rdr.Close();
            }

            return TeleplayInfo;
        }

        public TeleplayInfo GetFirstTeleplayInfo(int publishmentSystemID, int contentID)
        {
            TeleplayInfo TeleplayInfo = null;

            string sqlString = string.Format("SELECT TOP 1 ID, PublishmentSystemID, ContentID, StillUrl, Taxis, Description, Title FROM siteserver_Teleplay WHERE PublishmentSystemID = {0} AND ContentID = {1} ORDER BY Taxis DESC", publishmentSystemID, contentID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    TeleplayInfo = new TeleplayInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString());
                }
                rdr.Close();
            }

            return TeleplayInfo;
        }

        public int GetCount(int publishmentSystemID, int contentID)
        {
            string sqlString = string.Format("SELECT Count(*) FROM siteserver_Teleplay WHERE PublishmentSystemID = {0} AND ContentID = {1}", publishmentSystemID, contentID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSortFieldName()
        {
            return "Taxis";
        }

        public string GetSelectSqlString(int publishmentSystemID, int contentID)
        {
            return string.Format("SELECT ID, PublishmentSystemID, ContentID, StillUrl, Taxis, Description, Title FROM siteserver_Teleplay WHERE PublishmentSystemID = {0} AND ContentID = {1} ORDER BY Taxis DESC", publishmentSystemID, contentID);
        }

        public IEnumerable GetStlDataSource(int publishmentSystemID, int contentID, int startNum, int totalNum)
        {
            string tableName = "siteserver_Teleplay";
            string orderByString = "ORDER BY Taxis DESC";
            string whereString = string.Format("WHERE (PublishmentSystemID = {0} AND ContentID = {1})", publishmentSystemID, contentID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, startNum, totalNum, SqlUtils.Asterisk, whereString, orderByString);

            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }

        public List<int> GetTeleplayContentIDList(int publishmentSystemID, int contentID)
        {
            List<int> list = new List<int>();

            string sqlString = string.Format("SELECT ID FROM siteserver_Teleplay WHERE PublishmentSystemID = {0} AND ContentID = {1} ORDER BY Taxis DESC", publishmentSystemID, contentID);

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

        public List<TeleplayInfo> GetTeleplayInfoList(int publishmentSystemID, int contentID)
        {
            List<TeleplayInfo> list = new List<TeleplayInfo>();

            string sqlString = string.Format("SELECT ID, PublishmentSystemID, ContentID, StillUrl, Taxis, Description, Title FROM siteserver_Teleplay WHERE PublishmentSystemID = {0} AND ContentID = {1} ORDER BY Taxis DESC", publishmentSystemID, contentID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    list.Add(new TeleplayInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString()));
                }
                rdr.Close();
            }

            return list;
        }

        private int GetTaxis(int id)
        {
            string sqlString = string.Format("SELECT Taxis FROM siteserver_Teleplay WHERE (ID = {0})", id);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private void SetTaxis(int id, int taxis)
        {
            string sqlString = string.Format("UPDATE siteserver_Teleplay SET Taxis = {0} WHERE (ID = {1})", taxis, id);
            this.ExecuteNonQuery(sqlString);
        }

        private int GetMaxTaxis(int publishmentSystemID, int contentID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM siteserver_Teleplay WHERE (PublishmentSystemID = {0} AND ContentID = {1})", publishmentSystemID, contentID);
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
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM siteserver_Teleplay WHERE (Taxis > (SELECT Taxis FROM siteserver_Teleplay WHERE ID = {0}) AND (PublishmentSystemID = {1} AND ContentID = {2})) ORDER BY Taxis DESC", id, publishmentSystemID, contentID);

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
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM siteserver_Teleplay WHERE (Taxis < (SELECT Taxis FROM siteserver_Teleplay WHERE ID = {0}) AND (PublishmentSystemID = {1} AND ContentID = {2})) ORDER BY Taxis DESC", id, publishmentSystemID, contentID);

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