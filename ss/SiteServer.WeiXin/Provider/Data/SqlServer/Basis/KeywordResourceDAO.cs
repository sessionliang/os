using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class KeywordResourceDAO : DataProviderBase, IKeywordResourceDAO
	{
        private const string SQL_UPDATE = "UPDATE wx_KeywordResource SET PublishmentSystemID = @PublishmentSystemID, KeywordID = @KeywordID, Title = @Title, ImageUrl = @ImageUrl, Summary = @Summary, ResourceType = @ResourceType, IsShowCoverPic = @IsShowCoverPic, Content = @Content, NavigationUrl = @NavigationUrl, ChannelID = @ChannelID, ContentID = @ContentID, Taxis = @Taxis WHERE ResourceID = @ResourceID";

        private const string SQL_DELETE = "DELETE FROM wx_KeywordResource WHERE ResourceID = @ResourceID";

        private const string SQL_SELECT = "SELECT ResourceID, PublishmentSystemID, KeywordID, Title, ImageUrl, Summary, ResourceType, IsShowCoverPic, Content, NavigationUrl, ChannelID, ContentID, Taxis FROM wx_KeywordResource WHERE ResourceID = @ResourceID";

        private const string SQL_SELECT_FIRST = "SELECT TOP 1 ResourceID, PublishmentSystemID, KeywordID, Title, ImageUrl, Summary, ResourceType, IsShowCoverPic, Content, NavigationUrl, ChannelID, ContentID, Taxis FROM wx_KeywordResource WHERE KeywordID = @KeywordID ORDER BY Taxis";

        private const string SQL_SELECT_ALL = "SELECT ResourceID, PublishmentSystemID, KeywordID, Title, ImageUrl, Summary, ResourceType, IsShowCoverPic, Content, NavigationUrl, ChannelID, ContentID, Taxis FROM wx_KeywordResource WHERE KeywordID = @KeywordID ORDER BY Taxis";

        private const string SQL_SELECT_ALL_ID = "SELECT ResourceID FROM wx_KeywordResource WHERE KeywordID = @KeywordID ORDER BY Taxis";

        private const string PARM_RESOURCE_ID = "@ResourceID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_KEYWORD_ID = "@KeywordID";
        private const string PARM_TITLE = "@Title";
        private const string PARM_IMAGE_URL = "@ImageUrl";
        private const string PARM_SUMMARY = "@Summary";
        private const string PARM_RESOURCE_TYPE = "@ResourceType";
        private const string PARM_IS_SHOW_COVER_PIC = "@IsShowCoverPic";
        private const string PARM_CONTENT = "@Content";
        private const string PARM_NAVIGATION_URL = "@NavigationUrl";
        private const string PARM_CHANNEL_ID = "@ChannelID";
        private const string PARM_CONTENT_ID = "@ContentID";
        private const string PARM_TAXIS = "@Taxis";

        public int Insert(KeywordResourceInfo resourceInfo)
        {
            int resourceID = 0;

            string sqlString = "INSERT INTO wx_KeywordResource (PublishmentSystemID, KeywordID, Title, ImageUrl, Summary, ResourceType, IsShowCoverPic, Content, NavigationUrl, ChannelID, ContentID, Taxis) VALUES (@PublishmentSystemID, @KeywordID, @Title, @ImageUrl, @Summary, @ResourceType, @IsShowCoverPic, @Content, @NavigationUrl, @ChannelID, @ContentID, @Taxis)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO wx_KeywordResource(ResourceID, PublishmentSystemID, KeywordID, Title, ImageUrl, Summary, ResourceType, IsShowCoverPic, Content, NavigationUrl, ChannelID, ContentID, Taxis) VALUES (wx_KeywordResource_SEQ.NEXTVAL, @PublishmentSystemID, @KeywordID, @Title, @ImageUrl, @Summary, @ResourceType, @IsShowCoverPic, @Content, @NavigationUrl, @ChannelID, @ContentID, @Taxis)";
            }

            int taxis = this.GetMaxTaxis(resourceInfo.KeywordID) + 1;
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, resourceInfo.PublishmentSystemID),
                this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, resourceInfo.KeywordID),
                this.GetParameter(PARM_TITLE, EDataType.NVarChar, 255, resourceInfo.Title),
                this.GetParameter(PARM_IMAGE_URL, EDataType.VarChar, 200, resourceInfo.ImageUrl),
                this.GetParameter(PARM_SUMMARY, EDataType.NVarChar, 255, resourceInfo.Summary),
                this.GetParameter(PARM_RESOURCE_TYPE, EDataType.VarChar, 50, EResourceTypeUtils.GetValue(resourceInfo.ResourceType)),
                this.GetParameter(PARM_IS_SHOW_COVER_PIC, EDataType.VarChar, 18, resourceInfo.IsShowCoverPic.ToString()),
                this.GetParameter(PARM_CONTENT, EDataType.NText, resourceInfo.Content),
                this.GetParameter(PARM_NAVIGATION_URL, EDataType.VarChar, 200, resourceInfo.NavigationUrl),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, resourceInfo.ChannelID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, resourceInfo.ContentID),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        resourceID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "wx_KeywordResource");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return resourceID;
        }

        public void Update(KeywordResourceInfo resourceInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, resourceInfo.PublishmentSystemID),
                this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, resourceInfo.KeywordID),
                this.GetParameter(PARM_TITLE, EDataType.NVarChar, 255, resourceInfo.Title),
                this.GetParameter(PARM_IMAGE_URL, EDataType.VarChar, 200, resourceInfo.ImageUrl),
                this.GetParameter(PARM_SUMMARY, EDataType.NVarChar, 255, resourceInfo.Summary),
                this.GetParameter(PARM_RESOURCE_TYPE, EDataType.VarChar, 50, EResourceTypeUtils.GetValue(resourceInfo.ResourceType)),
                this.GetParameter(PARM_IS_SHOW_COVER_PIC, EDataType.VarChar, 18, resourceInfo.IsShowCoverPic.ToString()),
                this.GetParameter(PARM_CONTENT, EDataType.NText, resourceInfo.Content),
                this.GetParameter(PARM_NAVIGATION_URL, EDataType.VarChar, 200, resourceInfo.NavigationUrl),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, resourceInfo.ChannelID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, resourceInfo.ContentID),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, resourceInfo.Taxis),
                this.GetParameter(PARM_RESOURCE_ID, EDataType.Integer, resourceInfo.ResourceID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int resourceID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_RESOURCE_ID, EDataType.Integer, resourceID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
        }

        public KeywordResourceInfo GetResourceInfo(int resourceID)
        {
            KeywordResourceInfo resourceInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_RESOURCE_ID, EDataType.Integer, resourceID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    resourceInfo = new KeywordResourceInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), EResourceTypeUtils.GetEnumType(rdr.GetValue(6).ToString()), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetInt32(10), rdr.GetInt32(11), rdr.GetInt32(12));
                }
                rdr.Close();
            }

            return resourceInfo;
        }

        public KeywordResourceInfo GetFirstResourceInfo(int keywordID)
        {
            KeywordResourceInfo resourceInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_FIRST, parms))
            {
                if (rdr.Read())
                {
                    resourceInfo = new KeywordResourceInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), EResourceTypeUtils.GetEnumType(rdr.GetValue(6).ToString()), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetInt32(10), rdr.GetInt32(11), rdr.GetInt32(12));
                }
                rdr.Close();
            }

            return resourceInfo;
        }

        public IEnumerable GetDataSource(int keywordID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL, parms);
            return enumerable;
        }

        public int GetCount(int keywordID)
        {
            string sqlString = "SELECT COUNT(*) FROM wx_KeywordResource WHERE KeywordID = " + keywordID;
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public List<KeywordResourceInfo> GetResourceInfoList(int keywordID)
        {
            List<KeywordResourceInfo> list = new List<KeywordResourceInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    KeywordResourceInfo resourceInfo = new KeywordResourceInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), EResourceTypeUtils.GetEnumType(rdr.GetValue(6).ToString()), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetInt32(10), rdr.GetInt32(11), rdr.GetInt32(12));
                    list.Add(resourceInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<int> GetResourceIDList(int keywordID)
        {
            List<int> list = new List<int>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_ID, parms))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return list;
        }

        public bool UpdateTaxisToUp(int keywordID, int resourceID)
        {
            string sqlString = string.Format("SELECT TOP 1 ResourceID, Taxis FROM wx_KeywordResource WHERE (Taxis > (SELECT Taxis FROM wx_KeywordResource WHERE ResourceID = {0} AND KeywordID = {1})) AND KeywordID = {1} ORDER BY Taxis", resourceID, keywordID);
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

            int selectedTaxis = GetTaxis(resourceID);

            if (higherID > 0)
            {
                SetTaxis(resourceID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int keywordID, int resourceID)
        {
            string sqlString = string.Format("SELECT TOP 1 ResourceID, Taxis FROM wx_KeywordResource WHERE (Taxis < (SELECT Taxis FROM wx_KeywordResource WHERE ResourceID = {0} AND KeywordID = {1})) AND KeywordID = {1} ORDER BY Taxis DESC", resourceID, keywordID);
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

            int selectedTaxis = GetTaxis(resourceID);

            if (lowerID > 0)
            {
                SetTaxis(resourceID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int keywordID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM wx_KeywordResource WHERE KeywordID = {0}", keywordID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int resourceID)
        {
            string sqlString = string.Format("SELECT Taxis FROM wx_KeywordResource WHERE ResourceID = {0}", resourceID);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    taxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int resourceID, int taxis)
        {
            string sqlString = string.Format("UPDATE wx_KeywordResource SET Taxis = {0} WHERE ResourceID = {1}", taxis, resourceID);
            this.ExecuteNonQuery(sqlString);
        }
        public List<KeywordResourceInfo> GetKeywordResourceInfoList(int publishmentSystemID,int keywordID)
        {
            List<KeywordResourceInfo> list = new List<KeywordResourceInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    KeywordResourceInfo resourceInfo = new KeywordResourceInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), EResourceTypeUtils.GetEnumType(rdr.GetValue(6).ToString()), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetInt32(10), rdr.GetInt32(11), rdr.GetInt32(12));
                    list.Add(resourceInfo);
                }
                rdr.Close();
            }

            return list;
        }
    
	}
}