using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data;
using System.Text;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class PublishmentSystemDAO : DataProviderBase, IPublishmentSystemDAO
    {
        private const string SQL_SELECT_PUBLISHMENT_SYSTEM = "SELECT PublishmentSystemID, PublishmentSystemName, PublishmentSystemType, AuxiliaryTableForContent, AuxiliaryTableForGoods, AuxiliaryTableForBrand, AuxiliaryTableForGovPublic, AuxiliaryTableForGovInteract, AuxiliaryTableForVote, AuxiliaryTableForJob, IsCheckContentUseLevel, CheckContentLevel, PublishmentSystemDir, PublishmentSystemUrl, IsHeadquarters, ParentPublishmentSystemID, GroupSN, Taxis, SettingsXML FROM siteserver_PublishmentSystem WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_PUBLISHMENT_SYSTEM_ALL = "SELECT PublishmentSystemID, PublishmentSystemName, PublishmentSystemType, AuxiliaryTableForContent, AuxiliaryTableForGoods, AuxiliaryTableForBrand, AuxiliaryTableForGovPublic, AuxiliaryTableForGovInteract, AuxiliaryTableForVote, AuxiliaryTableForJob, IsCheckContentUseLevel, CheckContentLevel, PublishmentSystemDir, PublishmentSystemUrl, IsHeadquarters, ParentPublishmentSystemID, GroupSN, Taxis, SettingsXML FROM siteserver_PublishmentSystem ORDER BY Taxis";

        private const string SQL_SELECT_ALL_WITH_NODE = "SELECT p.PublishmentSystemID, p.PublishmentSystemName, p.PublishmentSystemType, p.AuxiliaryTableForContent, p.AuxiliaryTableForGoods, p.AuxiliaryTableForBrand, p.AuxiliaryTableForGovPublic, p.AuxiliaryTableForGovInteract, p.AuxiliaryTableForVote, p.AuxiliaryTableForJob, p.IsCheckContentUseLevel, p.CheckContentLevel, p.PublishmentSystemDir, p.PublishmentSystemUrl, p.IsHeadquarters, p.ParentPublishmentSystemID, p.GroupSN, p.Taxis, n.NodeName FROM siteserver_PublishmentSystem p INNER JOIN siteserver_Node n ON (p.PublishmentSystemID = n.NodeID) ORDER BY p.IsHeadquarters DESC, p.ParentPublishmentSystemID, p.Taxis DESC, n.NodeID";

        private const string SQL_INSERT_PUBLISHMENT_SYSTEM = "INSERT INTO siteserver_PublishmentSystem (PublishmentSystemID, PublishmentSystemName, PublishmentSystemType, AuxiliaryTableForContent, AuxiliaryTableForGoods, AuxiliaryTableForBrand, AuxiliaryTableForGovPublic, AuxiliaryTableForGovInteract, AuxiliaryTableForVote, AuxiliaryTableForJob, IsCheckContentUseLevel, CheckContentLevel, PublishmentSystemDir, PublishmentSystemUrl, IsHeadquarters, ParentPublishmentSystemID, GroupSN, Taxis, SettingsXML) VALUES (@PublishmentSystemID, @PublishmentSystemName, @PublishmentSystemType, @AuxiliaryTableForContent, @AuxiliaryTableForGoods, @AuxiliaryTableForBrand, @AuxiliaryTableForGovPublic, @AuxiliaryTableForGovInteract, @AuxiliaryTableForVote, @AuxiliaryTableForJob, @IsCheckContentUseLevel, @CheckContentLevel, @PublishmentSystemDir, @PublishmentSystemUrl, @IsHeadquarters, @ParentPublishmentSystemID, @GroupSN, @Taxis, @SettingsXML)";

        private const string SQL_UPDATE_PUBLISHMENT_SYSTEM = "UPDATE siteserver_PublishmentSystem SET PublishmentSystemName = @PublishmentSystemName, PublishmentSystemType = @PublishmentSystemType, AuxiliaryTableForContent = @AuxiliaryTableForContent, AuxiliaryTableForGoods = @AuxiliaryTableForGoods, AuxiliaryTableForBrand = @AuxiliaryTableForBrand, AuxiliaryTableForGovPublic = @AuxiliaryTableForGovPublic, AuxiliaryTableForGovInteract = @AuxiliaryTableForGovInteract, AuxiliaryTableForVote = @AuxiliaryTableForVote, AuxiliaryTableForJob = @AuxiliaryTableForJob, IsCheckContentUseLevel = @IsCheckContentUseLevel, CheckContentLevel = @CheckContentLevel, PublishmentSystemDir = @PublishmentSystemDir, PublishmentSystemUrl = @PublishmentSystemUrl, IsHeadquarters = @IsHeadquarters, ParentPublishmentSystemID = @ParentPublishmentSystemID, GroupSN = @GroupSN, Taxis = @Taxis, SettingsXML = @SettingsXML WHERE  PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_UPDATE_ALL_IS_HEADQUARTERS = "UPDATE siteserver_PublishmentSystem SET IsHeadquarters = @IsHeadquarters";

        private const string SQL_SELECT_ALL_PUBLISHMENTSYSTEM_ID = "SELECT PublishmentSystemID FROM siteserver_PublishmentSystem ORDER BY IsHeadquarters DESC, ParentPublishmentSystemID, Taxis DESC, PublishmentSystemID";

        private const string SQL_SELECT_PUBLISHMENTSYSTEM_DIR_BY_IS_HEADQUARTERS = "SELECT PublishmentSystemDir FROM siteserver_PublishmentSystem WHERE IsHeadquarters = @IsHeadquarters";

        private const string SQL_SELECT_PUBLISHMENTSYSTEM_ID_BY_PARENT = "SELECT PublishmentSystemID FROM siteserver_PublishmentSystem WHERE ParentPublishmentSystemID = @ParentPublishmentSystemID";

        private const string SQL_SELECT_PUBLISHMENTSYSTEM_ID_BY_IS_HEADQUARTERS = "SELECT PublishmentSystemID FROM siteserver_PublishmentSystem WHERE IsHeadquarters = @IsHeadquarters";

        private const string SQL_SELECT_PUBLISHMENTSYSTEM_ID_BY_PUBLISHMENTSYSTEM_DIR = "SELECT PublishmentSystemID FROM siteserver_PublishmentSystem WHERE PublishmentSystemDir = @PublishmentSystemDir";

        private const string PARM_PUBLISHMENTSYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_PUBLISHMENTSYSTEM_NAME = "@PublishmentSystemName";
        private const string PARM_PUBLISHMENTSYSTEM_TYPE = "@PublishmentSystemType";
        private const string PARM_AUXILIARY_TABLE_FOR_CONTENT = "@AuxiliaryTableForContent";
        private const string PARM_AUXILIARY_TABLE_FOR_GOODS = "@AuxiliaryTableForGoods";
        private const string PARM_AUXILIARY_TABLE_FOR_BRAND = "@AuxiliaryTableForBrand";
        private const string PARM_AUXILIARY_TABLE_FOR_GOVPUBLIC = "@AuxiliaryTableForGovPublic";
        private const string PARM_AUXILIARY_TABLE_FOR_GOVINTERACT = "@AuxiliaryTableForGovInteract";
        private const string PARM_AUXILIARY_TABLE_FOR_VOTE = "@AuxiliaryTableForVote";
        private const string PARM_AUXILIARY_TABLE_FOR_JOB = "@AuxiliaryTableForJob";
        private const string PARM_IS_CHECK_CONTENT_USE_LEVEL = "@IsCheckContentUseLevel";
        private const string PARM_CHECK_CONTENT_LEVEL = "@CheckContentLevel";
        private const string PARM_PUBLISHMENTSYSTEM_DIR = "@PublishmentSystemDir";
        private const string PARM_PUBLISHMENTSYSTEM_URL = "@PublishmentSystemUrl";
        private const string PARM_IS_HEADQUARTERS = "@IsHeadquarters";
        private const string PARM_PARENT_PUBLISHMENTSYSTEMID = "@ParentPublishmentSystemID";
        private const string PARM_GROUP_SN = "@GroupSN";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_SETTINGS_XML = "@SettingsXML";

        public string TableName { get { return "siteserver_PublishmentSystem"; } }

        public void InsertWithTrans(PublishmentSystemInfo info, IDbTransaction trans)
        {
            //»ñÈ¡ÅÅÐòÖµ
            int taxis = this.GetMaxTaxis() + 1;
            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, info.PublishmentSystemID),
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_NAME, EDataType.NVarChar, 50, info.PublishmentSystemName),
                this.GetParameter(PARM_PUBLISHMENTSYSTEM_TYPE, EDataType.VarChar, 50, EPublishmentSystemTypeUtils.GetValue(info.PublishmentSystemType)),
				this.GetParameter(PARM_AUXILIARY_TABLE_FOR_CONTENT, EDataType.VarChar, 50, info.AuxiliaryTableForContent),
                this.GetParameter(PARM_AUXILIARY_TABLE_FOR_GOODS, EDataType.VarChar, 50, info.AuxiliaryTableForGoods),
                this.GetParameter(PARM_AUXILIARY_TABLE_FOR_BRAND, EDataType.VarChar, 50, info.AuxiliaryTableForBrand),
                this.GetParameter(PARM_AUXILIARY_TABLE_FOR_GOVPUBLIC, EDataType.VarChar, 50, info.AuxiliaryTableForGovPublic),
                this.GetParameter(PARM_AUXILIARY_TABLE_FOR_GOVINTERACT, EDataType.VarChar, 50, info.AuxiliaryTableForGovInteract),
                this.GetParameter(PARM_AUXILIARY_TABLE_FOR_VOTE, EDataType.VarChar, 50, info.AuxiliaryTableForVote),
                this.GetParameter(PARM_AUXILIARY_TABLE_FOR_JOB, EDataType.VarChar, 50, info.AuxiliaryTableForJob),
				this.GetParameter(PARM_IS_CHECK_CONTENT_USE_LEVEL, EDataType.VarChar, 18, info.IsCheckContentUseLevel.ToString()),
				this.GetParameter(PARM_CHECK_CONTENT_LEVEL, EDataType.Integer, info.CheckContentLevel),
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_DIR, EDataType.VarChar, 50, info.PublishmentSystemDir),
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_URL, EDataType.VarChar, 200, info.PublishmentSystemUrl),
				this.GetParameter(PARM_IS_HEADQUARTERS, EDataType.VarChar, 18, info.IsHeadquarters.ToString()),
                this.GetParameter(PARM_PARENT_PUBLISHMENTSYSTEMID, EDataType.Integer, info.ParentPublishmentSystemID),
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, info.GroupSN),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis),
				this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, info.Additional.ToString())
			};

            this.ExecuteNonQuery(trans, SQL_INSERT_PUBLISHMENT_SYSTEM, insertParms);
            PublishmentSystemManager.ClearCache(true);
        }

        public void Delete(int publishmentSystemID)
        {
            PublishmentSystemManager.ClearCache(true);
            NodeManager.RemoveCache(publishmentSystemID);
            BaiRongDataProvider.TaskDAO.DeleteSystemTask(AppManager.CMS.AppID, publishmentSystemID, BaiRong.Model.Service.EServiceType.Publish);
            BaiRongDataProvider.TagDAO.DeleteTags(AppManager.CMS.AppID, publishmentSystemID);
            DataProvider.PublishmentSystemDAO.UpdateParentPublishmentSystemIDToZero(publishmentSystemID);

            PublishmentSystemManager.UpdateUrlRewriteFile();
        }


        public void Update(PublishmentSystemInfo info)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_NAME, EDataType.NVarChar, 50, info.PublishmentSystemName),
                this.GetParameter(PARM_PUBLISHMENTSYSTEM_TYPE, EDataType.VarChar, 50, EPublishmentSystemTypeUtils.GetValue(info.PublishmentSystemType)),
				this.GetParameter(PARM_AUXILIARY_TABLE_FOR_CONTENT, EDataType.VarChar, 50, info.AuxiliaryTableForContent),
                this.GetParameter(PARM_AUXILIARY_TABLE_FOR_GOODS, EDataType.VarChar, 50, info.AuxiliaryTableForGoods),
                this.GetParameter(PARM_AUXILIARY_TABLE_FOR_BRAND, EDataType.VarChar, 50, info.AuxiliaryTableForBrand),
                this.GetParameter(PARM_AUXILIARY_TABLE_FOR_GOVPUBLIC, EDataType.VarChar, 50, info.AuxiliaryTableForGovPublic),
                this.GetParameter(PARM_AUXILIARY_TABLE_FOR_GOVINTERACT, EDataType.VarChar, 50, info.AuxiliaryTableForGovInteract),
                this.GetParameter(PARM_AUXILIARY_TABLE_FOR_VOTE, EDataType.VarChar, 50, info.AuxiliaryTableForVote),
                this.GetParameter(PARM_AUXILIARY_TABLE_FOR_JOB, EDataType.VarChar, 50, info.AuxiliaryTableForJob),
				this.GetParameter(PARM_IS_CHECK_CONTENT_USE_LEVEL, EDataType.VarChar, 18, info.IsCheckContentUseLevel.ToString()),
				this.GetParameter(PARM_CHECK_CONTENT_LEVEL, EDataType.Integer, info.CheckContentLevel),
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_DIR, EDataType.VarChar, 50, info.PublishmentSystemDir),
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_URL, EDataType.VarChar, 200, info.PublishmentSystemUrl),
				this.GetParameter(PARM_IS_HEADQUARTERS, EDataType.VarChar, 18, info.IsHeadquarters.ToString()),
                this.GetParameter(PARM_PARENT_PUBLISHMENTSYSTEMID, EDataType.Integer, info.ParentPublishmentSystemID),
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, info.GroupSN),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, info.Taxis),
				this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, info.Additional.ToString()),
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, info.PublishmentSystemID)
			};

            if (info.IsHeadquarters)
            {
                this.UpdateAllIsHeadquarters();
            }

            this.ExecuteNonQuery(SQL_UPDATE_PUBLISHMENT_SYSTEM, updateParms);
            PublishmentSystemManager.ClearCache(true);
        }

        public void UpdateParentPublishmentSystemIDToZero(int parentPublishmentSystemID)
        {
            string sqlString = "UPDATE siteserver_PublishmentSystem SET ParentPublishmentSystemID = 0 WHERE ParentPublishmentSystemID = " + parentPublishmentSystemID;

            this.ExecuteNonQuery(sqlString);
            PublishmentSystemManager.ClearCache(true);
        }

        public ArrayList GetLowerPublishmentSystemDirArrayListThatNotIsHeadquarters()
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_HEADQUARTERS, EDataType.VarChar, 18, false.ToString())
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PUBLISHMENTSYSTEM_DIR_BY_IS_HEADQUARTERS, parms))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString().ToLower());
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetPublishmentSystemIDArrayListByParent(int parentPublishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PARENT_PUBLISHMENTSYSTEMID, EDataType.Integer, parentPublishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PUBLISHMENTSYSTEM_ID_BY_PARENT, parms))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }
            return arraylist;
        }

        private void UpdateAllIsHeadquarters()
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_HEADQUARTERS, EDataType.VarChar, 18, false.ToString())
			};

            this.ExecuteNonQuery(SQL_UPDATE_ALL_IS_HEADQUARTERS, updateParms);
            PublishmentSystemManager.ClearCache(true);
        }

        public DictionaryEntryArrayList GetPublishmentSystemInfoDictionaryEntryArrayList()
        {
            DictionaryEntryArrayList dictionary = new DictionaryEntryArrayList();

            ArrayList publishmentSystemInfoArrayList = this.GetPublishmentSystemInfoArrayList();
            foreach (PublishmentSystemInfo publishmentSystemInfo in publishmentSystemInfoArrayList)
            {
                DictionaryEntry entry = new DictionaryEntry(publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo);
                dictionary.Add(entry);
            }

            return dictionary;
        }

        private ArrayList GetPublishmentSystemIDArrayList()
        {
            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_PUBLISHMENTSYSTEM_ID))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }
            return arraylist;
        }

        protected virtual ArrayList GetPublishmentSystemIDArrayList(DateTime sinceDate)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT p.PublishmentSystemID FROM siteserver_PublishmentSystem p INNER JOIN siteserver_Node n ON (p.PublishmentSystemID = n.NodeID AND (n.AddDate BETWEEN '{0}' AND '{1}')) ORDER BY p.IsHeadquarters DESC, p.ParentPublishmentSystemID, p.Taxis DESC, n.NodeID", sinceDate.ToShortDateString(), DateTime.Now.ToShortDateString());

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }
            return arraylist;
        }

        private PublishmentSystemInfo GetPublishmentSystemInfo(int publishmentSystemID)
        {
            PublishmentSystemInfo publishmentSystemInfo = null;

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PUBLISHMENT_SYSTEM, selectParms))
            {
                if (rdr.Read())
                {
                    publishmentSystemInfo = new PublishmentSystemInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), EPublishmentSystemTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), TranslateUtils.ToBool(rdr.GetValue(10).ToString()), rdr.GetInt32(11), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), TranslateUtils.ToBool(rdr.GetValue(14).ToString()), rdr.GetInt32(15), rdr.GetValue(16).ToString(), rdr.GetInt32(17), rdr.GetValue(18).ToString());
                }
                rdr.Close();
            }

            return publishmentSystemInfo;
        }

        private ArrayList GetPublishmentSystemInfoArrayList()
        {
            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PUBLISHMENT_SYSTEM_ALL))
            {
                while (rdr.Read())
                {
                    PublishmentSystemInfo publishmentSystemInfo = new PublishmentSystemInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), EPublishmentSystemTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), TranslateUtils.ToBool(rdr.GetValue(10).ToString()), rdr.GetInt32(11), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), TranslateUtils.ToBool(rdr.GetValue(14).ToString()), rdr.GetInt32(15), rdr.GetValue(16).ToString(), rdr.GetInt32(17), rdr.GetValue(18).ToString());
                    arraylist.Add(publishmentSystemInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public IEnumerable GetDataSource()
        {
            return (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_WITH_NODE);
        }

        public string GetSelectCommand()
        {
            return SQL_SELECT_ALL_WITH_NODE;
        }

        public string GetDatabasePublishmentSystemUrl(int publishmentSystemID)
        {
            string publishmentSystemUrl = string.Empty;

            string sqlString = "SELECT PublishmentSystemUrl FROM siteserver_PublishmentSystem WHERE PublishmentSystemID = " + publishmentSystemID;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    publishmentSystemUrl = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return publishmentSystemUrl;
        }

        public int GetPublishmentSystemCount()
        {
            int count = 0;

            string sqlString = "SELECT Count(*) FROM siteserver_PublishmentSystem";

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        count = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return count;
        }

        public int GetPublishmentSystemIDByIsHeadquarters()
        {
            int publishmentSystemID = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_HEADQUARTERS, EDataType.VarChar, 18, true.ToString())
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PUBLISHMENTSYSTEM_ID_BY_IS_HEADQUARTERS, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        publishmentSystemID = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return publishmentSystemID;
        }

        public int GetPublishmentSystemIDByPublishmentSystemDir(string publishmentSystemDir)
        {
            int publishmentSystemID = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEM_DIR, EDataType.VarChar, 50, publishmentSystemDir)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PUBLISHMENTSYSTEM_ID_BY_PUBLISHMENTSYSTEM_DIR, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        publishmentSystemID = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return publishmentSystemID;
        }

        public IEnumerable GetStlDataSource(string siteName, string directory, int startNum, int totalNum, string whereString, EScopeType scopeType, string orderByString, string since)
        {
            IEnumerable ie = null;

            string tableName = "siteserver_PublishmentSystem";

            string sqlWhereString = string.Empty;

            PublishmentSystemInfo publishmentSystemInfo = null;
            if (!string.IsNullOrEmpty(siteName))
            {
                publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfoBySiteName(siteName);
            }
            else if (!string.IsNullOrEmpty(directory))
            {
                publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfoByDirectory(directory);
            }

            if (publishmentSystemInfo != null)
            {
                sqlWhereString = string.Format("WHERE (ParentPublishmentSystemID = {0})", publishmentSystemInfo.PublishmentSystemID);
            }
            else
            {
                if (scopeType == EScopeType.Children)
                {
                    sqlWhereString = "WHERE (ParentPublishmentSystemID = 0 AND IsHeadquarters = 'False')";
                }
                else if (scopeType == EScopeType.Descendant)
                {
                    sqlWhereString = "WHERE (IsHeadquarters = 'False')";
                }
            }

            if (!string.IsNullOrEmpty(whereString))
            {
                if (string.IsNullOrEmpty(sqlWhereString))
                {
                    sqlWhereString = string.Format("WHERE ({0})", whereString);
                }
                else
                {
                    sqlWhereString = string.Format("{0} AND ({1})", sqlWhereString, whereString);
                }
            }

            if (string.IsNullOrEmpty(orderByString) || StringUtils.EqualsIgnoreCase(orderByString, "default"))
            {
                orderByString = "ORDER BY IsHeadquarters DESC, ParentPublishmentSystemID, Taxis DESC, PublishmentSystemID";

                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, startNum, totalNum, SqlUtils.Asterisk, sqlWhereString, orderByString);

                if (!string.IsNullOrEmpty(since))
                {
                    DateTime sinceDate = DateTime.Now.AddHours(-DateUtils.GetSinceHours(since));
                    ArrayList siteIDArrayList = this.GetPublishmentSystemIDArrayList(sinceDate);

                    if (string.IsNullOrEmpty(sqlWhereString))
                    {
                        sqlWhereString = "WHERE ";
                    }
                    else
                    {
                        sqlWhereString += " AND ";
                    }

                    sqlWhereString += string.Format("(PublishmentSystemID IN ({0}))", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(siteIDArrayList));
                }

                ie = (IEnumerable)this.ExecuteReader(SQL_SELECT);
            }
            else if (StringUtils.EqualsIgnoreCase(orderByString, "contents"))
            {
                SortedList countWithSiteID = new SortedList();
                ArrayList siteIDArrayList = null;
                DateTime sinceDate = DateUtils.SqlMinValue;
                if (string.IsNullOrEmpty(since))
                {
                    siteIDArrayList = this.GetPublishmentSystemIDArrayList();
                }
                else
                {
                    sinceDate = DateTime.Now.AddHours(-DateUtils.GetSinceHours(since));
                    siteIDArrayList = this.GetPublishmentSystemIDArrayList(sinceDate);
                }

                foreach (int siteID in siteIDArrayList)
                {
                    PublishmentSystemInfo siteInfo = PublishmentSystemManager.GetPublishmentSystemInfo(siteID);
                    int count = DataProvider.ContentDAO.GetCountOfContentAdd(siteInfo.AuxiliaryTableForContent, siteID, siteID, sinceDate, DateTime.Now, string.Empty);
                    if (countWithSiteID.ContainsKey(count))
                    {
                        countWithSiteID[count] = countWithSiteID[count] + "," + siteID;
                    }
                    else
                    {
                        countWithSiteID.Add(count, siteID);
                    }
                }

                if (string.IsNullOrEmpty(sqlWhereString))
                {
                    sqlWhereString = "WHERE ";
                }
                else
                {
                    sqlWhereString += " AND ";
                }

                ArrayList arraylist = new ArrayList(countWithSiteID.Values);
                arraylist.Reverse();
                string siteIDCollection = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(arraylist);
                sqlWhereString += string.Format("(PublishmentSystemID IN ({0}))", siteIDCollection);

                orderByString = string.Format("ORDER BY CHARINDEX(CAST(PublishmentSystemID AS VARCHAR),'{0}')", siteIDCollection);

                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, startNum, totalNum, SqlUtils.Asterisk, sqlWhereString, orderByString);

                ie = (IEnumerable)this.ExecuteReader(SQL_SELECT);
            }
            else if (StringUtils.EqualsIgnoreCase(orderByString, "hits"))
            {
                SortedList countWithSiteID = new SortedList();
                ArrayList siteIDArrayList = null;
                DateTime sinceDate = DateUtils.SqlMinValue;
                if (string.IsNullOrEmpty(since))
                {
                    siteIDArrayList = this.GetPublishmentSystemIDArrayList();
                }
                else
                {
                    sinceDate = DateTime.Now.AddHours(-DateUtils.GetSinceHours(since));
                    siteIDArrayList = this.GetPublishmentSystemIDArrayList(sinceDate);
                }
                foreach (int siteID in siteIDArrayList)
                {
                    int count = DataProvider.TrackingDAO.GetTotalAccessNum(siteID, sinceDate);
                    if (countWithSiteID.ContainsKey(count))
                    {
                        countWithSiteID[count] = countWithSiteID[count] + "," + siteID;
                    }
                    else
                    {
                        countWithSiteID.Add(count, siteID);
                    }
                }

                if (string.IsNullOrEmpty(sqlWhereString))
                {
                    sqlWhereString = "WHERE ";
                }
                else
                {
                    sqlWhereString += " AND ";
                }

                ArrayList arraylist = new ArrayList(countWithSiteID.Values);
                arraylist.Reverse();
                string siteIDCollection = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(arraylist);
                sqlWhereString += string.Format("(PublishmentSystemID IN ({0}))", siteIDCollection);

                orderByString = string.Format("ORDER BY CHARINDEX(CAST(PublishmentSystemID AS VARCHAR),'{0}')", siteIDCollection);

                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, startNum, totalNum, SqlUtils.Asterisk, sqlWhereString, orderByString);

                ie = (IEnumerable)this.ExecuteReader(SQL_SELECT);
            }

            return ie;
        }

        public bool UpdateTaxisToDown(int publishmentSystemID)
        {
            this.SetTaxisNotZero();
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("SELECT TOP 1 PublishmentSystemID, Taxis FROM siteserver_PublishmentSystem ");
            sbSql.AppendFormat(" WHERE Taxis > (SELECT Taxis FROM siteserver_PublishmentSystem WHERE PublishmentSystemID = {0}) ", publishmentSystemID);
            sbSql.AppendFormat(" ORDER BY Taxis ");
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader reader = this.ExecuteReader(sbSql.ToString()))
            {
                if (reader.Read())
                {
                    lowerID = Convert.ToInt32(reader[0]);
                    lowerTaxis = Convert.ToInt32(reader[1]);
                }
                reader.Close();
            }

            int selectedTaxis = GetTaxis(publishmentSystemID);
            if (lowerID != 0)
            {
                SetTaxis(publishmentSystemID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToUp(int publishmentSystemID)
        {
            this.SetTaxisNotZero();
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("SELECT TOP 1 PublishmentSystemID, Taxis FROM siteserver_PublishmentSystem ");
            sbSql.AppendFormat(" WHERE Taxis < (SELECT Taxis FROM siteserver_PublishmentSystem WHERE PublishmentSystemID = {0}) ", publishmentSystemID);
            sbSql.AppendFormat(" ORDER BY Taxis DESC");
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader reader = this.ExecuteReader(sbSql.ToString()))
            {
                if (reader.Read())
                {
                    higherID = Convert.ToInt32(reader[0]);
                    higherTaxis = Convert.ToInt32(reader[1]);
                }
                reader.Close();
            }

            int selectedTaxis = GetTaxis(publishmentSystemID);
            if (higherID != 0)
            {
                SetTaxis(publishmentSystemID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        private void SetTaxis(int publishmentSystemID, int taxis)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" UPDATE siteserver_PublishmentSystem SET Taxis = {0} ", taxis);
            sbSql.AppendFormat(" WHERE PublishmentSystemID = {0} ", publishmentSystemID);
            this.ExecuteNonQuery(sbSql.ToString());
        }

        private int GetTaxis(int publishmentSystemID)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" SELECT Taxis FROM siteserver_PublishmentSystem ");
            sbSql.AppendFormat(" WHERE PublishmentSystemID = {0} ", publishmentSystemID);

            int taxis = 0;
            using (IDataReader reader = this.ExecuteReader(sbSql.ToString()))
            {
                if (reader.Read())
                {
                    taxis = Convert.ToInt32(reader[0]);
                }
                reader.Close();
            }
            return taxis;
        }

        private int GetMaxTaxis()
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM siteserver_PublishmentSystem");
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private void SetTaxisNotZero()
        {
            string sqlString = string.Format(@"UPDATE siteserver_PublishmentSystem SET Taxis = PublishmentSystemID where Taxis = 0");
            this.ExecuteNonQuery(sqlString);
        }
    }
}
