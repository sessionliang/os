using System;
using System.Data;
using System.Collections;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using BaiRong.Model;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class StarSettingDAO : DataProviderBase, IStarSettingDAO
	{
        private const string SQL_SELECT_STAR_SETTING = "SELECT TotalCount, PointAverage FROM siteserver_StarSetting WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID";

        private const string SQL_SELECT_STAR_SETTING_ID = "SELECT StarSettingID FROM siteserver_StarSetting WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID";

        private const string SQL_UPDATE_STAR_SETTING = "UPDATE siteserver_StarSetting SET TotalCount = @TotalCount, PointAverage = @PointAverage WHERE StarSettingID = @StarSettingID";

        private const string PARM_STAR_SETTING_ID = "@StarSettingID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_CHANNEL_ID = "@ChannelID";
        private const string PARM_CONTENT_ID = "@ContentID";
        private const string PARM_TOTAL_COUNT = "@TotalCount";
        private const string PARM_POINT_AVERAGE = "@PointAverage";

        public void SetStarSetting(int publishmentSystemID, int channelID, int contentID, int totalCount, decimal pointAverage)
		{
            int starSettingID = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, channelID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_STAR_SETTING_ID, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        starSettingID = rdr.GetInt32(0);
                    }
                }
            }

            if (starSettingID > 0)
            {
                parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_TOTAL_COUNT, EDataType.Integer, totalCount),
				    this.GetParameter(PARM_POINT_AVERAGE, EDataType.Decimal, 18, pointAverage),
                    this.GetParameter(PARM_STAR_SETTING_ID, EDataType.Integer, starSettingID)
			    };

                this.ExecuteNonQuery(SQL_UPDATE_STAR_SETTING, parms);
            }
            else
            {
                string SQL_INSERT_STAR_SETTING = "INSERT INTO siteserver_StarSetting (PublishmentSystemID, ChannelID, ContentID, TotalCount, PointAverage) VALUES (@PublishmentSystemID, @ChannelID, @ContentID, @TotalCount, @PointAverage)";
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    SQL_INSERT_STAR_SETTING = "INSERT INTO siteserver_StarSetting (StarSettingID, PublishmentSystemID, ChannelID, ContentID, TotalCount, PointAverage) VALUES (siteserver_StarSetting_SEQ.NEXTVAL, @PublishmentSystemID, @ChannelID, @ContentID, @TotalCount, @PointAverage)";
                }

                parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				    this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, channelID),
                    this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID),
				    this.GetParameter(PARM_TOTAL_COUNT, EDataType.Integer, totalCount),
				    this.GetParameter(PARM_POINT_AVERAGE, EDataType.Decimal, 18, pointAverage)
			    };

                this.ExecuteNonQuery(SQL_INSERT_STAR_SETTING, parms);
            }
		}

        public object[] GetTotalCountAndPointAverage(int publishmentSystemID, int contentID)
        {
            int totalCount = 0;
            decimal pointAverage = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_STAR_SETTING, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        totalCount = rdr.GetInt32(0);
                        pointAverage = rdr.GetDecimal(1);
                    }
                }
                rdr.Close();
            }
            return new object[] { totalCount, pointAverage };
        }
	}
}
