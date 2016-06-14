using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Advertisement;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
	public class AdvertisementDAO : DataProviderBase, IAdvertisementDAO
	{
		// Static constants
        private const string SQL_INSERT_AD = "INSERT INTO siteserver_Advertisement (AdvertisementName, PublishmentSystemID, AdvertisementType, IsDateLimited, StartDate, EndDate, AddDate, NodeIDCollectionToChannel, NodeIDCollectionToContent, FileTemplateIDCollection, Settings) VALUES (@AdvertisementName, @PublishmentSystemID, @AdvertisementType, @IsDateLimited, @StartDate, @EndDate, @AddDate, @NodeIDCollectionToChannel, @NodeIDCollectionToContent, @FileTemplateIDCollection, @Settings)";

        private const string SQL_UPDATE_AD = "UPDATE siteserver_Advertisement SET AdvertisementType = @AdvertisementType, IsDateLimited = @IsDateLimited, StartDate = @StartDate, EndDate = @EndDate, NodeIDCollectionToChannel = @NodeIDCollectionToChannel, NodeIDCollectionToContent = @NodeIDCollectionToContent, FileTemplateIDCollection = @FileTemplateIDCollection, Settings = @Settings WHERE AdvertisementName = @AdvertisementName AND PublishmentSystemID = @PublishmentSystemID";

		private const string SQL_DELETE_AD = "DELETE FROM siteserver_Advertisement WHERE AdvertisementName = @AdvertisementName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_AD = "SELECT AdvertisementName, PublishmentSystemID, AdvertisementType, IsDateLimited, StartDate, EndDate, AddDate, NodeIDCollectionToChannel, NodeIDCollectionToContent, FileTemplateIDCollection, Settings FROM siteserver_Advertisement WHERE AdvertisementName = @AdvertisementName AND PublishmentSystemID = @PublishmentSystemID";

		private const string SQL_SELECT_AD_NAME = "SELECT AdvertisementName FROM siteserver_Advertisement WHERE AdvertisementName = @AdvertisementName AND PublishmentSystemID = @PublishmentSystemID";

		private const string SQL_SELECT_AD_TYPE = "SELECT AdvertisementType FROM siteserver_Advertisement WHERE AdvertisementName = @AdvertisementName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ALL_AD = "SELECT AdvertisementName, PublishmentSystemID, AdvertisementType, IsDateLimited, StartDate, EndDate, AddDate, NodeIDCollectionToChannel, NodeIDCollectionToContent, FileTemplateIDCollection, Settings FROM siteserver_Advertisement WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY AddDate DESC";

        private const string SQL_SELECT_ALL_AD_BY_TYPE = "SELECT AdvertisementName, PublishmentSystemID, AdvertisementType, IsDateLimited, StartDate, EndDate, AddDate, NodeIDCollectionToChannel, NodeIDCollectionToContent, FileTemplateIDCollection, Settings FROM siteserver_Advertisement WHERE AdvertisementType = @AdvertisementType AND PublishmentSystemID = @PublishmentSystemID ORDER BY AddDate DESC";	

		//Advertisement Attributes
		private const string PARM_AD_NAME = "@AdvertisementName";
		private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
		private const string PARM_AD_TYPE = "@AdvertisementType";
		private const string PARM_IS_DATE_LIMITED = "@IsDateLimited";
		private const string PARM_START_DATE = "@StartDate";
		private const string PARM_END_DATE = "@EndDate";
		private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_NODE_ID_COLLECTION_TO_CHANNEL = "@NodeIDCollectionToChannel";
        private const string PARM_NODE_ID_COLLECTION_TO_CONTENT = "@NodeIDCollectionToContent";
        private const string PARM_FILE_TEMPLATE_ID_COLLECTION = "@FileTemplateIDCollection";
        private const string PARM_SETTINGS = "@Settings";

		public void Insert(AdvertisementInfo adInfo) 
		{
			IDbDataParameter[] adParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AD_NAME, EDataType.VarChar, 50, adInfo.AdvertisementName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, adInfo.PublishmentSystemID),
				this.GetParameter(PARM_AD_TYPE, EDataType.VarChar, 50, EAdvertisementTypeUtils.GetValue(adInfo.AdvertisementType)),
				this.GetParameter(PARM_IS_DATE_LIMITED, EDataType.VarChar, 18, adInfo.IsDateLimited.ToString()),
				this.GetParameter(PARM_START_DATE, EDataType.DateTime, adInfo.StartDate),
				this.GetParameter(PARM_END_DATE, EDataType.DateTime, adInfo.EndDate),
				this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, adInfo.AddDate),
				this.GetParameter(PARM_NODE_ID_COLLECTION_TO_CHANNEL, EDataType.NVarChar, 255, adInfo.NodeIDCollectionToChannel),
                this.GetParameter(PARM_NODE_ID_COLLECTION_TO_CONTENT, EDataType.NVarChar, 255, adInfo.NodeIDCollectionToContent),
                this.GetParameter(PARM_FILE_TEMPLATE_ID_COLLECTION, EDataType.NVarChar, 255, adInfo.FileTemplateIDCollection),
                this.GetParameter(PARM_SETTINGS, EDataType.NText, adInfo.Settings)
			};

            this.ExecuteNonQuery(SQL_INSERT_AD, adParms);
            AdvertisementManager.RemoveCache(adInfo.PublishmentSystemID);
		}

		public void Update(AdvertisementInfo adInfo)
		{
			IDbDataParameter[] adParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AD_TYPE, EDataType.VarChar, 50, EAdvertisementTypeUtils.GetValue(adInfo.AdvertisementType)),
				this.GetParameter(PARM_IS_DATE_LIMITED, EDataType.VarChar, 18, adInfo.IsDateLimited.ToString()),
				this.GetParameter(PARM_START_DATE, EDataType.DateTime, adInfo.StartDate),
				this.GetParameter(PARM_END_DATE, EDataType.DateTime, adInfo.EndDate),
				this.GetParameter(PARM_NODE_ID_COLLECTION_TO_CHANNEL, EDataType.NVarChar, 255, adInfo.NodeIDCollectionToChannel),
                this.GetParameter(PARM_NODE_ID_COLLECTION_TO_CONTENT, EDataType.NVarChar, 255, adInfo.NodeIDCollectionToContent),
                this.GetParameter(PARM_FILE_TEMPLATE_ID_COLLECTION, EDataType.NVarChar, 255, adInfo.FileTemplateIDCollection),
                this.GetParameter(PARM_SETTINGS, EDataType.NText, adInfo.Settings),
				this.GetParameter(PARM_AD_NAME, EDataType.VarChar, 50, adInfo.AdvertisementName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, adInfo.PublishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_AD, adParms);

            AdvertisementManager.RemoveCache(adInfo.PublishmentSystemID);
		}

		public void Delete(string advertisementName, int publishmentSystemID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AD_NAME, EDataType.VarChar, 50, advertisementName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_DELETE_AD, parms);
            AdvertisementManager.RemoveCache(publishmentSystemID);
		}

		public AdvertisementInfo GetAdvertisementInfo(string advertisementName, int publishmentSystemID)
		{
			AdvertisementInfo adInfo = null;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AD_NAME, EDataType.VarChar, 50, advertisementName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};
			
			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_AD, parms)) 
			{
				if (rdr.Read()) 
				{
                    adInfo = new AdvertisementInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), EAdvertisementTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetDateTime(4), rdr.GetDateTime(5), rdr.GetDateTime(6), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString());
				}
				rdr.Close();
			}

			return adInfo;
		}

		public EAdvertisementType GetAdvertisementType(string advertisementName, int publishmentSystemID)
		{
			EAdvertisementType adType = EAdvertisementType.FloatImage;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AD_NAME, EDataType.VarChar, 50, advertisementName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_AD_TYPE, parms)) 
			{
				if (rdr.Read()) 
				{
                    adType = EAdvertisementTypeUtils.GetEnumType(rdr.GetValue(0).ToString());
				}
				rdr.Close();
			}

			return adType;
		}

		public bool IsExists(string advertisementName, int publishmentSystemID)
		{
			bool exists = false;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AD_NAME, EDataType.VarChar, 50, advertisementName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
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

		public IEnumerable GetDataSource(int publishmentSystemID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

			IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_AD, parms);
			return enumerable;
		}

		public IEnumerable GetDataSourceByType(EAdvertisementType advertisementType, int publishmentSystemID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AD_TYPE, EDataType.VarChar, 50, EAdvertisementTypeUtils.GetValue(advertisementType)),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

			IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_AD_BY_TYPE, parms);
			return enumerable;
		}

		public ArrayList GetAdvertisementNameArrayList(int publishmentSystemID)
		{
			ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT AdvertisementName FROM siteserver_Advertisement WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    string advertisementName = rdr.GetValue(0).ToString();
                    arraylist.Add(advertisementName);
                }
                rdr.Close();
            }

			return arraylist;
		}

        public ArrayList[] GetAdvertisementArrayLists(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT NodeIDCollectionToChannel, NodeIDCollectionToContent, FileTemplateIDCollection FROM siteserver_Advertisement WHERE PublishmentSystemID = {0}", publishmentSystemID);

            ArrayList arraylist1 = new ArrayList();
            ArrayList arraylist2 = new ArrayList();
            ArrayList arraylist3 = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    string collection1 = rdr.GetValue(0).ToString();
                    string collection2 = rdr.GetValue(1).ToString();
                    string collection3 = rdr.GetValue(2).ToString();

                    if (!string.IsNullOrEmpty(collection1))
                    {
                        ArrayList list = TranslateUtils.StringCollectionToIntArrayList(collection1);
                        foreach (int id in list)
                        {
                            if (!arraylist1.Contains(id))
                            {
                                arraylist1.Add(id);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(collection2))
                    {
                        ArrayList list = TranslateUtils.StringCollectionToIntArrayList(collection2);
                        foreach (int id in list)
                        {
                            if (!arraylist2.Contains(id))
                            {
                                arraylist2.Add(id);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(collection3))
                    {
                        ArrayList list = TranslateUtils.StringCollectionToIntArrayList(collection3);
                        foreach (int id in list)
                        {
                            if (!arraylist3.Contains(id))
                            {
                                arraylist3.Add(id);
                            }
                        }
                    }
                }
                rdr.Close();
            }

            return new ArrayList[] { arraylist1, arraylist2, arraylist3 };
        }
	}
}
