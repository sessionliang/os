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
    public class AdDAO : DataProviderBase, IAdDAO
    {
        private const string SQL_INSERT_AD = "INSERT INTO bbs_Ad (PublishmentSystemID, AdName, AdType, AdLocation, Code, TextWord, TextLink, TextColor, TextFontSize, ImageUrl, ImageLink, ImageWidth, ImageHeight, ImageAlt, IsEnabled, IsDateLimited, StartDate, EndDate) VALUES (@PublishmentSystemID, @AdName, @AdType, @AdLocation, @Code, @TextWord, @TextLink, @TextColor, @TextFontSize, @ImageUrl, @ImageLink, @ImageWidth, @ImageHeight, @ImageAlt, @IsEnabled, @IsDateLimited, @StartDate, @EndDate)";

        private const string SQL_UPDATE_AD = "UPDATE bbs_Ad SET AdName = @AdName, AdType = @AdType, AdLocation = @AdLocation, Code = @Code, TextWord = @TextWord, TextLink = @TextLink, TextColor = @TextColor, TextFontSize = @TextFontSize, ImageUrl = @ImageUrl, ImageLink = @ImageLink, ImageWidth = @ImageWidth, ImageHeight = @ImageHeight, ImageAlt = @ImageAlt, IsEnabled = @IsEnabled, IsDateLimited = @IsDateLimited, StartDate = @StartDate, EndDate = @EndDate WHERE ID = @ID";

        private const string SQL_DELETE_AD = "DELETE FROM bbs_Ad WHERE ID = @ID";

        private const string SQL_SELECT_AD = "SELECT ID, PublishmentSystemID, AdName, AdType, AdLocation, Code, TextWord, TextLink, TextColor, TextFontSize, ImageUrl, ImageLink, ImageWidth, ImageHeight, ImageAlt, IsEnabled, IsDateLimited, StartDate, EndDate FROM bbs_Ad WHERE ID = @ID";

        private const string SQL_SELECT_AD_NAME = "SELECT AdName FROM bbs_Ad WHERE PublishmentSystemID = @PublishmentSystemID AND AdName = @AdName";

        private const string SQL_SELECT_COUNT = "SELECT COUNT(*) FROM bbs_Ad WHERE PublishmentSystemID = @PublishmentSystemID AND AdLocation = @AdLocation";

        private const string SQL_SELECT_ALL_AD = "SELECT ID, PublishmentSystemID, AdName, AdType, AdLocation, Code, TextWord, TextLink, TextColor, TextFontSize, ImageUrl, ImageLink, ImageWidth, ImageHeight, ImageAlt, IsEnabled, IsDateLimited, StartDate, EndDate FROM bbs_Ad WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY StartDate DESC";

        private const string SQL_SELECT_ALL_AD_BY_LOCATION = "SELECT ID, PublishmentSystemID, AdName, AdType, AdLocation, Code, TextWord, TextLink, TextColor, TextFontSize, ImageUrl, ImageLink, ImageWidth, ImageHeight, ImageAlt, IsEnabled, IsDateLimited, StartDate, EndDate FROM bbs_Ad WHERE PublishmentSystemID = @PublishmentSystemID AND AdLocation = @AdLocation ORDER BY StartDate DESC";

        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_AD_NAME = "@AdName";
        private const string PARM_AD_TYPE = "@AdType";
        private const string PARM_AD_LOCATION = "@AdLocation";
        private const string PARM_CODE = "@Code";
        private const string PARM_TEXT_WORD = "@TextWord";
        private const string PARM_TEXT_LINK = "@TextLink";
        private const string PARM_TEXT_COLOR = "@TextColor";
        private const string PARM_TEXT_FONT_SIZE = "@TextFontSize";
        private const string PARM_IMAGE_URL = "@ImageUrl";
        private const string PARM_IMAGE_LINK = "@ImageLink";
        private const string PARM_IMAGE_WIDTH = "@ImageWidth";
        private const string PARM_IMAGE_HEIGHT = "@ImageHeight";
        private const string PARM_IMAGE_ALT = "@ImageAlt";
        private const string PARM_IS_ENABLED = "@IsEnabled";
        private const string PARM_IS_DATE_LIMITED = "@IsDateLimited";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_END_DATE = "@EndDate";

        public void Insert(int publishmentSystemID, AdInfo adInfo)
        {
            IDbDataParameter[] adParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, adInfo.PublishmentSystemID),
				this.GetParameter(PARM_AD_NAME, EDataType.NVarChar, 50, adInfo.AdName),
				this.GetParameter(PARM_AD_TYPE, EDataType.VarChar, 50, EAdTypeUtils.GetValue(adInfo.AdType)),
                this.GetParameter(PARM_AD_LOCATION, EDataType.VarChar, 50, EAdLocationUtils.GetValue(adInfo.AdLocation)),
                this.GetParameter(PARM_CODE, EDataType.NText, adInfo.Code),
                this.GetParameter(PARM_TEXT_WORD, EDataType.NVarChar, 255, adInfo.TextWord),
                this.GetParameter(PARM_TEXT_LINK, EDataType.VarChar, 200, adInfo.TextLink),
                this.GetParameter(PARM_TEXT_COLOR, EDataType.VarChar, 10, adInfo.TextColor),
                this.GetParameter(PARM_TEXT_FONT_SIZE, EDataType.Integer, adInfo.TextFontSize),
                this.GetParameter(PARM_IMAGE_URL, EDataType.VarChar, 200, adInfo.ImageUrl),
                this.GetParameter(PARM_IMAGE_LINK, EDataType.VarChar, 200, adInfo.ImageLink),
                this.GetParameter(PARM_IMAGE_WIDTH, EDataType.Integer, adInfo.ImageWidth),
                this.GetParameter(PARM_IMAGE_HEIGHT, EDataType.Integer, adInfo.ImageHeight),
                this.GetParameter(PARM_IMAGE_ALT, EDataType.NVarChar, 50, adInfo.ImageAlt),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, adInfo.IsEnabled.ToString()),
				this.GetParameter(PARM_IS_DATE_LIMITED, EDataType.VarChar, 18, adInfo.IsDateLimited.ToString()),
				this.GetParameter(PARM_START_DATE, EDataType.DateTime, adInfo.StartDate),
				this.GetParameter(PARM_END_DATE, EDataType.DateTime, adInfo.EndDate)
			};

            this.ExecuteNonQuery(SQL_INSERT_AD, adParms);

            AdManager.RemoveCache(publishmentSystemID);
        }

        public void Update(int publishmentSystemID, AdInfo adInfo)
        {
            IDbDataParameter[] adParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_AD_NAME, EDataType.VarChar, 50, adInfo.AdName),
				this.GetParameter(PARM_AD_TYPE, EDataType.VarChar, 50, EAdTypeUtils.GetValue(adInfo.AdType)),
                this.GetParameter(PARM_AD_LOCATION, EDataType.VarChar, 50, EAdLocationUtils.GetValue(adInfo.AdLocation)),
                this.GetParameter(PARM_CODE, EDataType.NText, adInfo.Code),
                this.GetParameter(PARM_TEXT_WORD, EDataType.NVarChar, 255, adInfo.TextWord),
                this.GetParameter(PARM_TEXT_LINK, EDataType.VarChar, 200, adInfo.TextLink),
                this.GetParameter(PARM_TEXT_COLOR, EDataType.VarChar, 10, adInfo.TextColor),
                this.GetParameter(PARM_TEXT_FONT_SIZE, EDataType.Integer, adInfo.TextFontSize),
                this.GetParameter(PARM_IMAGE_URL, EDataType.VarChar, 200, adInfo.ImageUrl),
                this.GetParameter(PARM_IMAGE_LINK, EDataType.VarChar, 200, adInfo.ImageLink),
                this.GetParameter(PARM_IMAGE_WIDTH, EDataType.Integer, adInfo.ImageWidth),
                this.GetParameter(PARM_IMAGE_HEIGHT, EDataType.Integer, adInfo.ImageHeight),
                this.GetParameter(PARM_IMAGE_ALT, EDataType.NVarChar, 50, adInfo.ImageAlt),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, adInfo.IsEnabled.ToString()),
				this.GetParameter(PARM_IS_DATE_LIMITED, EDataType.VarChar, 18, adInfo.IsDateLimited.ToString()),
				this.GetParameter(PARM_START_DATE, EDataType.DateTime, adInfo.StartDate),
				this.GetParameter(PARM_END_DATE, EDataType.DateTime, adInfo.EndDate),
                this.GetParameter(PARM_ID, EDataType.Integer, adInfo.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_AD, adParms);

            AdManager.RemoveCache(publishmentSystemID);
        }

        public void Delete(int publishmentSystemID, int id)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, id)
			};

            this.ExecuteNonQuery(SQL_DELETE_AD, parms);

            AdManager.RemoveCache(publishmentSystemID);
        }

        public AdInfo GetAdInfo(int id)
        {
            AdInfo adInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, id)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_AD, parms))
            {
                if (rdr.Read())
                {
                    adInfo = new AdInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), EAdTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), EAdLocationUtils.GetEnumType(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetInt32(9), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetInt32(13), rdr.GetValue(14).ToString(), TranslateUtils.ToBool(rdr.GetValue(15).ToString()), TranslateUtils.ToBool(rdr.GetValue(16).ToString()), rdr.GetDateTime(17), rdr.GetDateTime(18));
                }
                rdr.Close();
            }

            return adInfo;
        }

        public ArrayList GetAdInfoArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_AD, parms))
            {
                while (rdr.Read())
                {
                    AdInfo adInfo = new AdInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), EAdTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), EAdLocationUtils.GetEnumType(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetInt32(9), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetInt32(13), rdr.GetValue(14).ToString(), TranslateUtils.ToBool(rdr.GetValue(15).ToString()), TranslateUtils.ToBool(rdr.GetValue(16).ToString()), rdr.GetDateTime(17), rdr.GetDateTime(18));

                    arraylist.Add(adInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public bool IsExists(int publishmentSystemID, string adName)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_AD_NAME, EDataType.VarChar, 50, adName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_AD_NAME, parms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public int GetCount(int publishmentSystemID, EAdLocation adLocation)
        {
            int count = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_AD_LOCATION, EDataType.VarChar, 50, EAdLocationUtils.GetValue(adLocation))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_COUNT, parms))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    count = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return count;
        }

        public IEnumerable GetDataSource(int publishmentSystemID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_AD, parms);
            return enumerable;
        }

        public IEnumerable GetDataSource(int publishmentSystemID, EAdLocation adLocation)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_AD_LOCATION, EDataType.VarChar, 50, EAdLocationUtils.GetValue(adLocation))
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_AD_BY_LOCATION, parms);
            return enumerable; 
        }

        public ArrayList GetAdNameArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = "SELECT AdName FROM bbs_Ad WHERE PublishmentSystemID = " + publishmentSystemID;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    string adName = rdr.GetValue(0).ToString();
                    arraylist.Add(adName);
                }
                rdr.Close();
            }

            return arraylist;
        }
    }
}