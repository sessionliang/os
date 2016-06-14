using System;
using System.Data;
using System.Collections;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using BaiRong.Model;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class StarDAO : DataProviderBase, IStarDAO
	{
        private const string SQL_SELECT_STAR = "SELECT Point FROM siteserver_Star WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID";

        private const string PARM_STAR_ID = "@StarID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_CHANNEL_ID = "@ChannelID";
        private const string PARM_CONTENT_ID = "@ContentID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_POINT = "@Point";
        private const string PARM_MESSAGE = "@Message";
        private const string PARM_ADDDATE = "@AddDate";

        public void AddCount(int publishmentSystemID, int channelID, int contentID, string userName, int point, string message, DateTime addDate)
		{
            string sqlString = "INSERT INTO siteserver_Star (PublishmentSystemID, ChannelID, ContentID, UserName, Point, Message, AddDate) VALUES (@PublishmentSystemID, @ChannelID, @ContentID, @UserName, @Point, @Message, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_Star (StarID, PublishmentSystemID, ChannelID, ContentID, UserName, Point, Message, AddDate) VALUES (siteserver_Star_SEQ.NEXTVAL, @PublishmentSystemID, @ChannelID, @ContentID, @UserName, @Point, @Message, @AddDate)";
            }

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, channelID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID),
				this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName),
				this.GetParameter(PARM_POINT, EDataType.Integer, point),
                this.GetParameter(PARM_MESSAGE, EDataType.NVarChar, 255, message),
                this.GetParameter(PARM_ADDDATE, EDataType.DateTime, addDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
		}

        public int[] GetCount(int publishmentSystemID, int channelID, int contentID)
        {
            int totalCount = 0;
            int totalPoint = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_STAR, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        totalCount++;
                        totalPoint += rdr.GetInt32(0);
                    }
                }
                rdr.Close();
            }
            return new int[] { totalCount, totalPoint };
        }

        public ArrayList GetContentIDArrayListByPoint(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format(@"
SELECT ContentID, (SUM(Point) * 100)/Count(*) AS Num
FROM siteserver_Star
WHERE (PublishmentSystemID = {0} AND ContentID > 0)
GROUP BY ContentID
ORDER BY Num DESC", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int relatedIdentity = rdr.GetInt32(0);
                        arraylist.Add(relatedIdentity);
                    }
                }
                rdr.Close();
            }

            return arraylist;
        }
	}
}
