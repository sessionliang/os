using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;

namespace BaiRong.Provider.Data.SqlServer
{
    public class DiggDAO : DataProviderBase, IDiggDAO
	{
        private const string SQL_SELECT_DIGG = "SELECT Good, Bad FROM bairong_Digg WHERE PublishmentSystemID = @PublishmentSystemID AND RelatedIdentity = @RelatedIdentity";

        private const string SQL_SELECT_DIGG_ID = "SELECT DiggID FROM bairong_Digg WHERE PublishmentSystemID = @PublishmentSystemID AND RelatedIdentity = @RelatedIdentity";

        private const string SQL_UPDATE_DIGG_GOOD = "UPDATE bairong_Digg SET Good = Good + 1 WHERE PublishmentSystemID = @PublishmentSystemID AND RelatedIdentity = @RelatedIdentity";

        private const string SQL_UPDATE_DIGG_BAD = "UPDATE bairong_Digg SET Bad = Bad + 1 WHERE PublishmentSystemID = @PublishmentSystemID AND RelatedIdentity = @RelatedIdentity";

        private const string SQL_UPDATE_DIGG = "UPDATE bairong_Digg SET Good = @Good, Bad = @Bad WHERE PublishmentSystemID = @PublishmentSystemID AND RelatedIdentity = @RelatedIdentity";

        private const string SQL_DELETE_DIGG = "DELETE FROM bairong_Digg WHERE PublishmentSystemID = @PublishmentSystemID AND RelatedIdentity = @RelatedIdentity";

        private const string PARM_DIGG_ID = "@DiggID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
		private const string PARM_RELATED_IDENTITY = "@RelatedIdentity";
        private const string PARM_GOOD = "@Good";
        private const string PARM_BAD = "@Bad";		

		private void Insert(int publishmentSystemID, int relatedIdentity, bool isGood)
		{
            int good = (isGood) ? 1 : 0;
            int bad = (isGood) ? 0 : 1;

            string sqlString = "INSERT INTO bairong_Digg (PublishmentSystemID, RelatedIdentity, Good, Bad) VALUES (@PublishmentSystemID, @RelatedIdentity, @Good, @Bad)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_Digg (DiggID, PublishmentSystemID, RelatedIdentity, Good, Bad) VALUES (bairong_Digg_SEQ.NEXTVAL, @PublishmentSystemID, @RelatedIdentity, @Good, @Bad)";
            }

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, relatedIdentity),
				this.GetParameter(PARM_GOOD, EDataType.Integer, good),
				this.GetParameter(PARM_BAD, EDataType.Integer, bad)
			};

            this.ExecuteNonQuery(sqlString, parms);
		}

        private void Insert(int publishmentSystemID, int relatedIdentity, int goodNum, int badNum)
        {
            string sqlString = "INSERT INTO bairong_Digg (PublishmentSystemID, RelatedIdentity, Good, Bad) VALUES (@PublishmentSystemID, @RelatedIdentity, @Good, @Bad)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_Digg (DiggID, PublishmentSystemID, RelatedIdentity, Good, Bad) VALUES (bairong_Digg_SEQ.NEXTVAL, @PublishmentSystemID, @RelatedIdentity, @Good, @Bad)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, relatedIdentity),
				this.GetParameter(PARM_GOOD, EDataType.Integer, goodNum),
				this.GetParameter(PARM_BAD, EDataType.Integer, badNum)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        private void Update(int publishmentSystemID, int relatedIdentity, bool isGood)
		{
            string sqlString = (isGood) ? SQL_UPDATE_DIGG_GOOD : SQL_UPDATE_DIGG_BAD;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, relatedIdentity)
			};

            this.ExecuteNonQuery(sqlString, parms);
		}

        public void AddCount(int publishmentSystemID, int relatedIdentity, bool isGood)
        {
            bool isExists = IsExists(publishmentSystemID, relatedIdentity);
            if (isExists)
            {
                Update(publishmentSystemID, relatedIdentity, isGood);
            }
            else
            {
                Insert(publishmentSystemID, relatedIdentity, isGood);
            }
		}

        public void Delete(int publishmentSystemID, int relatedIdentity)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, relatedIdentity)
			};

            this.ExecuteNonQuery(SQL_DELETE_DIGG, parms);
		}

        public bool IsExists(int publishmentSystemID, int relatedIdentity)
        {
            bool isExists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, relatedIdentity)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_DIGG_ID, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        isExists = true;
                    }
                }
                rdr.Close();
            }
            return isExists;
        }

        public int[] GetCount(int publishmentSystemID, int relatedIdentity)
        {
            int good = 0;
            int bad = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, relatedIdentity)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_DIGG, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        good = rdr.GetInt32(0);
                        bad = rdr.GetInt32(1);
                    }
                }
                rdr.Close();
            }
            return new int[] { good, bad };
        }

        public void SetCount(int publishmentSystemID, int relatedIdentity, int goodNum, int badNum)
        {
            if (!this.IsExists(publishmentSystemID, relatedIdentity))
            {
                this.Insert(publishmentSystemID, relatedIdentity, goodNum, badNum);
            }
            else
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
                    this.GetParameter(PARM_GOOD, EDataType.Integer, goodNum),
                    this.GetParameter(PARM_BAD, EDataType.Integer, badNum),
				    this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				    this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, relatedIdentity)
			    };

                this.ExecuteNonQuery(SQL_UPDATE_DIGG, parms);
            }
        }

        public ArrayList GetRelatedIdentityArrayListByTotal(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format(@"
SELECT RelatedIdentity, (Good + Bad) AS NUM
FROM bairong_Digg
WHERE (PublishmentSystemID = {0} AND RelatedIdentity > 0)
GROUP BY RelatedIdentity, Good, Bad
ORDER BY NUM DESC", publishmentSystemID);

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
