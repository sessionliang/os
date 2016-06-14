using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class AdMaterialDAO : DataProviderBase, IAdMaterialDAO
	{
        private const string SQL_INSERT_ADMATERIAL = "INSERT INTO siteserver_AdMaterial ( PublishmentSystemID,AdvID,AdMaterialName, AdMaterialType, Code, TextWord, TextLink, TextColor, TextFontSize, ImageUrl, ImageLink, ImageWidth, ImageHeight, ImageAlt,Weight ,IsEnabled) VALUES ( @PublishmentSystemID,@AdvID,@AdMaterialName, @AdMaterialType, @Code, @TextWord, @TextLink, @TextColor, @TextFontSize, @ImageUrl, @ImageLink, @ImageWidth, @ImageHeight, @ImageAlt,@Weight , @IsEnabled)";

        private const string SQL_UPDATE_ADMATERIAL = "UPDATE siteserver_AdMaterial SET AdvID=@AdvID, AdMaterialName=@AdMaterialName, AdMaterialType = @AdMaterialType, Code = @Code, TextWord = @TextWord, TextLink = @TextLink, TextColor = @TextColor, TextFontSize = @TextFontSize, ImageUrl = @ImageUrl, ImageLink = @ImageLink, ImageWidth = @ImageWidth, ImageHeight = @ImageHeight, ImageAlt = @ImageAlt,Weight =@Weight , IsEnabled = @IsEnabled WHERE AdMaterialID = @AdMaterialID AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_DELETE_ADMATERIAL = "DELETE FROM siteserver_AdMaterial WHERE AdMaterialID = @AdMaterialID AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ADMATERIAL = "SELECT AdMaterialID, PublishmentSystemID,AdvID,AdMaterialName, AdMaterialType, Code, TextWord, TextLink, TextColor, TextFontSize, ImageUrl, ImageLink, ImageWidth, ImageHeight, ImageAlt,Weight , IsEnabled  FROM siteserver_AdMaterial WHERE AdMaterialID = @AdMaterialID AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ADMATERIAL_NAME = "SELECT AdMaterialName FROM siteserver_AdMaterial WHERE AdMaterialName = @AdMaterialName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ALL_ADMATERIAL = "SELECT AdMaterialID, PublishmentSystemID,AdvID,AdMaterialName, AdMaterialType, Code, TextWord, TextLink, TextColor, TextFontSize, ImageUrl, ImageLink, ImageWidth, ImageHeight, ImageAlt,Weight , IsEnabled  FROM siteserver_AdMaterial WHERE AdvID=@AdvID AND PublishmentSystemID = @PublishmentSystemID ORDER BY AdMaterialID DESC";

        private const string SQL_SELECT_ALL_ADMATERIAL_BY_TYPE = "SELECT AdMaterialID, PublishmentSystemID,AdvID,AdMaterialName, AdMaterialType, Code, TextWord, TextLink, TextColor, TextFontSize, ImageUrl, ImageLink, ImageWidth, ImageHeight, ImageAlt,Weight , IsEnabled  FROM siteserver_AdMaterial WHERE AdMaterialType = @AdMaterialType AND PublishmentSystemID = @PublishmentSystemID ORDER BY AdMaterialID DESC ";

        private const string SQL_SELECT_ALL_ADMATERIAL_BY_ADVERID = "SELECT AdMaterialID, PublishmentSystemID,AdvID,AdMaterialName, AdMaterialType, Code, TextWord, TextLink, TextColor, TextFontSize, ImageUrl, ImageLink, ImageWidth, ImageHeight, ImageAlt,Weight , IsEnabled  FROM siteserver_AdMaterial WHERE AdvID = @AdvID AND PublishmentSystemID = @PublishmentSystemID ORDER BY AdMaterialID DESC ";	

		//Ad Attributes
        private const string PARM_ADMATERIAL_ID = "@AdMaterialID";
        private const string PARM_ADMATERIAL_NAME = "@AdMaterialName";
		private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_ADVERT_ID = "@AdvID";
        private const string PARM_ADMATERIAL_TYPE = "@AdMaterialType";
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
        private const string PARM_WEIGHT  = "@Weight";
        private const string PARM_IS_ENABLED = "@IsEnabled";

        public void Insert(AdMaterialInfo adMaterialInfo) 
		{
			IDbDataParameter[] adParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ADMATERIAL_NAME, EDataType.NVarChar, 50, adMaterialInfo.AdMaterialName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, adMaterialInfo.PublishmentSystemID),
                this.GetParameter(PARM_ADVERT_ID,EDataType.Integer,adMaterialInfo.AdvID),
				this.GetParameter(PARM_ADMATERIAL_TYPE, EDataType.VarChar, 50, EAdvTypeUtils.GetValue(adMaterialInfo.AdMaterialType)),
                this.GetParameter(PARM_CODE, EDataType.NText, adMaterialInfo.Code),
                this.GetParameter(PARM_TEXT_WORD, EDataType.NVarChar, 255, adMaterialInfo.TextWord),
                this.GetParameter(PARM_TEXT_LINK, EDataType.VarChar, 200, adMaterialInfo.TextLink),
                this.GetParameter(PARM_TEXT_COLOR, EDataType.VarChar, 10, adMaterialInfo.TextColor),
                this.GetParameter(PARM_TEXT_FONT_SIZE, EDataType.Integer, adMaterialInfo.TextFontSize),
                this.GetParameter(PARM_IMAGE_URL, EDataType.VarChar, 200, adMaterialInfo.ImageUrl),
                this.GetParameter(PARM_IMAGE_LINK, EDataType.VarChar, 200, adMaterialInfo.ImageLink),
                this.GetParameter(PARM_IMAGE_WIDTH, EDataType.Integer, adMaterialInfo.ImageWidth),
                this.GetParameter(PARM_IMAGE_HEIGHT, EDataType.Integer, adMaterialInfo.ImageHeight),
                this.GetParameter(PARM_IMAGE_ALT, EDataType.NVarChar, 50, adMaterialInfo.ImageAlt),
                this.GetParameter(PARM_WEIGHT , EDataType.Integer, adMaterialInfo.Weight ),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, adMaterialInfo.IsEnabled.ToString())
			 
			};

            this.ExecuteNonQuery(SQL_INSERT_ADMATERIAL, adParms);
		}

        public void Update(AdMaterialInfo adMaterialInfo)
		{
			IDbDataParameter[] adParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_ADVERT_ID,EDataType.Integer,adMaterialInfo.AdvID),
				this.GetParameter(PARM_ADMATERIAL_NAME, EDataType.NVarChar, 50, adMaterialInfo.AdMaterialName),
				this.GetParameter(PARM_ADMATERIAL_TYPE, EDataType.VarChar, 50, EAdvTypeUtils.GetValue(adMaterialInfo.AdMaterialType)),
                this.GetParameter(PARM_CODE, EDataType.NText, adMaterialInfo.Code),
                this.GetParameter(PARM_TEXT_WORD, EDataType.NVarChar, 255, adMaterialInfo.TextWord),
                this.GetParameter(PARM_TEXT_LINK, EDataType.VarChar, 200, adMaterialInfo.TextLink),
                this.GetParameter(PARM_TEXT_COLOR, EDataType.VarChar, 10, adMaterialInfo.TextColor),
                this.GetParameter(PARM_TEXT_FONT_SIZE, EDataType.Integer, adMaterialInfo.TextFontSize),
                this.GetParameter(PARM_IMAGE_URL, EDataType.VarChar, 200, adMaterialInfo.ImageUrl),
                this.GetParameter(PARM_IMAGE_LINK, EDataType.VarChar, 200, adMaterialInfo.ImageLink),
                this.GetParameter(PARM_IMAGE_WIDTH, EDataType.Integer, adMaterialInfo.ImageWidth),
                this.GetParameter(PARM_IMAGE_HEIGHT, EDataType.Integer, adMaterialInfo.ImageHeight),
                this.GetParameter(PARM_IMAGE_ALT, EDataType.NVarChar, 50, adMaterialInfo.ImageAlt),
                this.GetParameter(PARM_WEIGHT , EDataType.Integer, adMaterialInfo.Weight ),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, adMaterialInfo.IsEnabled.ToString()),
                this.GetParameter(PARM_ADMATERIAL_ID, EDataType.Integer, adMaterialInfo.AdMaterialID),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, adMaterialInfo.PublishmentSystemID),
			};

            this.ExecuteNonQuery(SQL_UPDATE_ADMATERIAL, adParms);
		}

        public void Delete(int adMaterialID, int publishmentSystemID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ADMATERIAL_ID, EDataType.Integer, adMaterialID),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_DELETE_ADMATERIAL, parms);
		}

        public void Delete(ArrayList adMaterialIDArrarList, int publishmentSystemID)
        {
            if (adMaterialIDArrarList.Count > 0)
            {
                string strSql = string.Format(@"DELETE FROM siteserver_AdMaterial WHERE AdMaterialID IN ({0}) AND PublishmentSystemID={1}", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(adMaterialIDArrarList), publishmentSystemID);

                this.ExecuteNonQuery(strSql);
            }
        }

        public AdMaterialInfo GetAdMaterialInfo(int adMaterialD, int publishmentSystemID)
		{
            AdMaterialInfo adMaterialInfo = null;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ADMATERIAL_ID, EDataType.Integer, adMaterialD),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ADMATERIAL, parms)) 
			{
				if (rdr.Read()) 
				{
                    adMaterialInfo = new AdMaterialInfo(rdr.GetInt32(0),rdr.GetInt32(1),rdr.GetInt32(2), rdr.GetValue(3).ToString(),EAdvTypeUtils.GetEnumType(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetInt32(9), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetInt32(13), rdr.GetValue(14).ToString(),rdr.GetInt32(15), TranslateUtils.ToBool(rdr.GetValue(16).ToString()));
				}
				rdr.Close();
			}

            return adMaterialInfo;
		}

		public bool IsExists(string adMaterialName, int publishmentSystemID)
		{
			bool exists = false;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ADMATERIAL_NAME, EDataType.NVarChar, 50, adMaterialName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ADMATERIAL_NAME, parms)) 
			{
				if (rdr.Read()) 
				{					
					exists = true;
				}
				rdr.Close();
			}

			return exists;
		}

		public IEnumerable GetDataSource(int advertID, int publishmentSystemID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                 this.GetParameter(PARM_ADVERT_ID,EDataType.Integer,advertID),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_ADMATERIAL, parms);
			return enumerable;
		}

		public IEnumerable GetDataSourceByType(EAdvType adType, int publishmentSystemID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ADMATERIAL_TYPE, EDataType.VarChar, 50, EAdvTypeUtils.GetValue(adType)),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

			IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_ADMATERIAL_BY_TYPE, parms);
			return enumerable;
		}

		public ArrayList GetAdMaterialNameArrayList(int publishmentSystemID)
		{
			ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT AdMaterialName FROM siteserver_AdMaterial WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    string adMaterialName = rdr.GetValue(0).ToString();
                    arraylist.Add(adMaterialName);
                }
                rdr.Close();
            }

			return arraylist;
		}

        public ArrayList GetAdMaterialIDArrayList( int advertID,int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT AdMaterialID FROM siteserver_AdMaterial WHERE PublishmentSystemID = {0} AND AdvID={1}", publishmentSystemID,advertID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int adMaterialID = rdr.GetInt32(0);
                    arraylist.Add(adMaterialID);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetAdMaterialInfoArrayList(int advertID, int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_ADVERT_ID,EDataType.Integer,advertID),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_ADMATERIAL_BY_ADVERID, parms))
            {
                while (rdr.Read())
                {
                    AdMaterialInfo adMaterialInfo = new AdMaterialInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), EAdvTypeUtils.GetEnumType(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetInt32(9), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetInt32(13), rdr.GetValue(14).ToString(), rdr.GetInt32(15), TranslateUtils.ToBool(rdr.GetValue(16).ToString()));
                    arraylist.Add(adMaterialInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }
	}
}
