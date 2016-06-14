using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Text;
using BaiRong.Model;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class AdvDAO : DataProviderBase, IAdvDAO
    {
        private const string SQL_INSERT_ADV = "INSERT INTO siteserver_Adv (PublishmentSystemID,AdAreaID,AdvName,Summary, IsEnabled, IsDateLimited, StartDate, EndDate,LevelType,Level,IsWeight,Weight,RotateType,RotateInterval,NodeIDCollectionToChannel,NodeIDCollectionToContent,FileTemplateIDCollection) VALUES (@PublishmentSystemID,@AdAreaID,@AdvName, @Summary, @IsEnabled, @IsDateLimited, @StartDate, @EndDate,@LevelType,@Level,@IsWeight,@Weight,@RotateType,@RotateInterval,@NodeIDCollectionToChannel,@NodeIDCollectionToContent,@FileTemplateIDCollection)";

        private const string SQL_UPDATE_ADV = "UPDATE siteserver_Adv SET AdAreaID=@AdAreaID,AdvName=@AdvName, Summary = @Summary,IsEnabled = @IsEnabled, IsDateLimited = @IsDateLimited, StartDate = @StartDate, EndDate = @EndDate,LevelType=@LevelType,Level=@Level,IsWeight=@IsWeight,Weight=@Weight,RotateType=@RotateType,RotateInterval=@RotateInterval,NodeIDCollectionToChannel=@NodeIDCollectionToChannel,NodeIDCollectionToContent=@NodeIDCollectionToContent,FileTemplateIDCollection=@FileTemplateIDCollection WHERE AdvID = @AdvID AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_DELETE_ADV = "DELETE FROM siteserver_Adv WHERE AdvID = @AdvID AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ADV = "SELECT AdvID ,PublishmentSystemID,AdAreaID,AdvName,Summary, IsEnabled, IsDateLimited, StartDate, EndDate,LevelType,Level,IsWeight,Weight ,RotateType,RotateInterval,NodeIDCollectionToChannel,NodeIDCollectionToContent,FileTemplateIDCollection  FROM siteserver_Adv WHERE AdvID = @AdvID AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ADV_NAME = "SELECT AdvName FROM siteserver_Adv WHERE AdvName = @AdvName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ALL_ADV = "SELECT AdvID , PublishmentSystemID,AdAreaID,AdvName,Summary, IsEnabled, IsDateLimited, StartDate, EndDate,LevelType,Level,IsWeight,Weight ,RotateType,RotateInterval,NodeIDCollectionToChannel,NodeIDCollectionToContent,FileTemplateIDCollection  FROM siteserver_Adv WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY StartDate DESC";

        private const string SQL_SELECT_ALL_ADV_BY_ADAREAID = "SELECT AdvID , PublishmentSystemID,AdAreaID,AdvName,Summary, IsEnabled, IsDateLimited, StartDate, EndDate,LevelType,Level,IsWeight,Weight ,RotateType,RotateInterval,NodeIDCollectionToChannel,NodeIDCollectionToContent,FileTemplateIDCollection  FROM siteserver_Adv WHERE AdAreaID=@AdAreaID AND PublishmentSystemID = @PublishmentSystemID ORDER BY StartDate DESC";

        //Ad Attributes
        private const string PARM_ADV_ID = "AdvID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_ADV_AREAID = "@AdAreaID";
        private const string PARM_ADV_NAME = "@AdvName";
        private const string PARM_SUMMARY = "@Summary";
        private const string PARM_IS_ENABLED = "@IsEnabled";
        private const string PARM_IS_DATE_LIMITED = "@IsDateLimited";
        private const string PARM_START_DATE = "@StartDate";
        private const string PARM_END_DATE = "@EndDate";
        private const string PARM_LEVEL_TYPE = "@LevelType";
        private const string PARM_LEVEL = "@Level";
        private const string PARM_IS_WEIGHT = "@IsWeight";
        private const string PARM_WEIGHT = "@Weight ";
        private const string PARM_ROTATE_TYPE = "@RotateType";
        private const string PARM_ROTATE_INTERVAL = "@RotateInterval";
        private const string PARM_NODE_ID_COLLECTION_TO_CHANNEL = "@NodeIDCollectionToChannel";
        private const string PARM_NODE_ID_COLLECTION_TO_CONTENT = "@NodeIDCollectionToContent";
        private const string PARM_FILETEMPLATE_ID_COLLECTION = "@FileTemplateIDCollection";

        public void Insert(AdvInfo advInfo)
        {
            IDbDataParameter[] adParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, advInfo.PublishmentSystemID),
                this.GetParameter(PARM_ADV_AREAID, EDataType.Integer, advInfo.AdAreaID),
                this.GetParameter(PARM_ADV_NAME, EDataType.NVarChar, 50, advInfo.AdvName),
                this.GetParameter(PARM_SUMMARY, EDataType.Text, advInfo.Summary),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, advInfo.IsEnabled.ToString()),
                this.GetParameter(PARM_IS_DATE_LIMITED, EDataType.VarChar, 18, advInfo.IsDateLimited.ToString()),
                this.GetParameter(PARM_START_DATE, EDataType.DateTime, advInfo.StartDate),
                this.GetParameter(PARM_END_DATE, EDataType.DateTime, advInfo.EndDate),
                this.GetParameter(PARM_LEVEL_TYPE, EDataType.NVarChar, 50,EAdvLevelTypeUtils.GetValue(advInfo.LevelType)),
                this.GetParameter(PARM_LEVEL , EDataType.Integer, advInfo.Level),
                this.GetParameter(PARM_IS_WEIGHT, EDataType.VarChar, 18, advInfo.IsWeight.ToString()),
                this.GetParameter(PARM_WEIGHT , EDataType.Integer,advInfo.Weight ),
                this.GetParameter(PARM_ROTATE_TYPE, EDataType.NVarChar,50,EAdvRotateTypeUtils.GetValue(advInfo.RotateType)),
                this.GetParameter(PARM_ROTATE_INTERVAL, EDataType.Integer, advInfo.RotateInterval),
                this.GetParameter(PARM_NODE_ID_COLLECTION_TO_CHANNEL, EDataType.NVarChar, 4000, advInfo.NodeIDCollectionToChannel),
                this.GetParameter(PARM_NODE_ID_COLLECTION_TO_CONTENT, EDataType.NVarChar, 4000, advInfo.NodeIDCollectionToContent),
                this.GetParameter(PARM_FILETEMPLATE_ID_COLLECTION, EDataType.NVarChar,4000, advInfo.FileTemplateIDCollection)

            };

            this.ExecuteNonQuery(SQL_INSERT_ADV, adParms);
        }

        public void Update(AdvInfo advInfo)
        {
            IDbDataParameter[] adParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, advInfo.PublishmentSystemID),
                this.GetParameter(PARM_ADV_AREAID, EDataType.Integer, advInfo.AdAreaID),
                this.GetParameter(PARM_ADV_NAME, EDataType.NVarChar, 50, advInfo.AdvName),
                this.GetParameter(PARM_SUMMARY, EDataType.Text, advInfo.Summary),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, advInfo.IsEnabled.ToString()),
                this.GetParameter(PARM_IS_DATE_LIMITED, EDataType.VarChar, 18, advInfo.IsDateLimited.ToString()),
                this.GetParameter(PARM_START_DATE, EDataType.DateTime, advInfo.StartDate),
                this.GetParameter(PARM_END_DATE, EDataType.DateTime, advInfo.EndDate),
                this.GetParameter(PARM_LEVEL_TYPE, EDataType.NVarChar, 50,EAdvLevelTypeUtils.GetValue(advInfo.LevelType)),
                this.GetParameter(PARM_LEVEL , EDataType.Integer, advInfo.Level),
                this.GetParameter(PARM_IS_WEIGHT, EDataType.VarChar, 18, advInfo.IsWeight.ToString()),
                this.GetParameter(PARM_WEIGHT , EDataType.Integer,advInfo.Weight ),
                this.GetParameter(PARM_ROTATE_TYPE, EDataType.NVarChar,50,EAdvRotateTypeUtils.GetValue(advInfo.RotateType)),
                this.GetParameter(PARM_ROTATE_INTERVAL, EDataType.Integer, advInfo.RotateInterval),
                this.GetParameter(PARM_NODE_ID_COLLECTION_TO_CHANNEL, EDataType.NVarChar, 4000, advInfo.NodeIDCollectionToChannel),
                this.GetParameter(PARM_NODE_ID_COLLECTION_TO_CONTENT, EDataType.NVarChar, 4000, advInfo.NodeIDCollectionToContent),
                this.GetParameter(PARM_FILETEMPLATE_ID_COLLECTION, EDataType.NVarChar,4000, advInfo.FileTemplateIDCollection),
                this.GetParameter(PARM_ADV_ID, EDataType.Integer, advInfo.AdvID)

            };

            this.ExecuteNonQuery(SQL_UPDATE_ADV, adParms);
        }

        public void Delete(int advID, int publishmentSystemID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ADV_ID, EDataType.Integer,advID),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };
            this.ExecuteNonQuery(SQL_DELETE_ADV, parms);
        }

        public void Delete(ArrayList advIDArrayList, int publishmentSystemID)
        {
            if (advIDArrayList.Count > 0)
            {
                foreach (int advID in advIDArrayList)
                {
                    this.Delete(advID, publishmentSystemID);
                }
            }
        }

        public AdvInfo GetAdvInfo(int advID, int publishmentSystemID)
        {
            AdvInfo advInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ADV_ID, EDataType.Integer,advID),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ADV, parms))
            {
                if (rdr.Read())
                {
                    advInfo = new AdvInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetDateTime(7), rdr.GetDateTime(8), EAdvLevelTypeUtils.GetEnumType(rdr.GetValue(9).ToString()), rdr.GetInt32(10), TranslateUtils.ToBool(rdr.GetValue(11).ToString()), rdr.GetInt32(12), EAdvRotateTypeUtils.GetEnumType(rdr.GetValue(13).ToString()), rdr.GetInt32(14), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString(), rdr.GetValue(17).ToString());
                }
                rdr.Close();
            }

            return advInfo;
        }

        public bool IsExists(string adertName, int publishmentSystemID)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ADV_NAME, EDataType.NVarChar, 50, adertName),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ADV_NAME, parms))
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

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_ADV, parms);
            return enumerable;
        }

        public IEnumerable GetDataSourceByAdAreaID(int adAreaID, int publishmentSystemID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ADV_AREAID, EDataType.Integer, adAreaID),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_ADV_BY_ADAREAID, parms);
            return enumerable;
        }

        public ArrayList GetAdvNameArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT AdvName FROM siteserver_Adv WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    string advName = rdr.GetValue(0).ToString();
                    arraylist.Add(advName);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetAdvIDArrayList(int adAreaID, int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT AdvID FROM siteserver_Adv WHERE PublishmentSystemID = {0} AND AdAreaID={1}", publishmentSystemID, adAreaID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int advID = rdr.GetInt32(0);
                    arraylist.Add(advID);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetAdvInfoArrayList(ETemplateType templateType, int adAreaID, int publishmentSystemID, int nodeID, int fileTemplateID)
        {
            ArrayList arraylist = new ArrayList();
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"SELECT AdvID,PublishmentSystemID,AdAreaID,AdvName,Summary, IsEnabled, IsDateLimited, StartDate, EndDate,LevelType,Level,IsWeight,Weight ,RotateType,RotateInterval,NodeIDCollectionToChannel,NodeIDCollectionToContent,FileTemplateIDCollection FROM (SELECT * FROM siteserver_Adv WHERE AdAreaID ={0} AND PublishmentSystemID ={1}) AS ADV", adAreaID, publishmentSystemID);
            if (templateType == ETemplateType.IndexPageTemplate || templateType == ETemplateType.ChannelTemplate)
            {
                strSql.AppendFormat(" WHERE NodeIDCollectionToChannel='{0}' OR NodeIDCollectionToChannel LIKE '{0},%' OR NodeIDCollectionToChannel LIKE '%,{0}' OR NodeIDCollectionToChannel LIKE '%,{0},%'", nodeID.ToString());
            }
            else if (templateType == ETemplateType.ContentTemplate)
            {
                strSql.AppendFormat(" WHERE NodeIDCollectionToContent='{0}' OR NodeIDCollectionToContent LIKE '{0},%' OR NodeIDCollectionToContent LIKE '%,{0}' OR NodeIDCollectionToContent LIKE '%,{0},%'", nodeID.ToString());
            }
            else if (templateType == ETemplateType.FileTemplate)
            {
                strSql.AppendFormat(" WHERE FileTemplateIDCollection='{0}' OR FileTemplateIDCollection LIKE '{0},%' OR FileTemplateIDCollection LIKE '%,{0}' OR FileTemplateIDCollection LIKE '%,{0},%'", fileTemplateID.ToString());
            }

            strSql.AppendFormat(@" ORDER BY StartDate ASC");
            using (IDataReader rdr = this.ExecuteReader(strSql.ToString()))
            {
                while (rdr.Read())
                {
                    AdvInfo advInfo = new AdvInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetDateTime(7), rdr.GetDateTime(8), EAdvLevelTypeUtils.GetEnumType(rdr.GetValue(9).ToString()), rdr.GetInt32(10), TranslateUtils.ToBool(rdr.GetValue(11).ToString()), rdr.GetInt32(12), EAdvRotateTypeUtils.GetEnumType(rdr.GetValue(13).ToString()), rdr.GetInt32(14), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString(), rdr.GetValue(17).ToString());
                    arraylist.Add(advInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

    }
}
