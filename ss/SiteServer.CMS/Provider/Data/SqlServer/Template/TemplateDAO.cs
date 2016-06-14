using System;
using System.Collections;
using System.Data;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Generic;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class TemplateDAO : DataProviderBase, ITemplateDAO
    {
        private const string SQL_SELECT_TEMPLATE = "SELECT TemplateID, PublishmentSystemID, TemplateName, TemplateType, RelatedFileName, CreatedFileFullName, CreatedFileExtName, Charset, IsDefault FROM siteserver_Template WHERE PublishmentSystemID = @PublishmentSystemID AND TemplateID = @TemplateID";

        private const string SQL_SELECT_TEMPLATE_BY_TEMPLATE_NAME = "SELECT TemplateID, PublishmentSystemID, TemplateName, TemplateType, RelatedFileName, CreatedFileFullName, CreatedFileExtName, Charset, IsDefault FROM siteserver_Template WHERE PublishmentSystemID = @PublishmentSystemID AND TemplateType = @TemplateType AND TemplateName = @TemplateName";

        private const string SQL_SELECT_ALL_TEMPLATE_BY_TYPE = "SELECT TemplateID, PublishmentSystemID, TemplateName, TemplateType, RelatedFileName, CreatedFileFullName, CreatedFileExtName, Charset, IsDefault FROM siteserver_Template WHERE PublishmentSystemID = @PublishmentSystemID AND TemplateType = @TemplateType ORDER BY RelatedFileName";

        private const string SQL_SELECT_ALL_TEMPLATE_ID_BY_TYPE = "SELECT TemplateID FROM siteserver_Template WHERE PublishmentSystemID = @PublishmentSystemID AND TemplateType = @TemplateType ORDER BY RelatedFileName";

        private const string SQL_SELECT_ALL_TEMPLATE_BY_PUBLISHMENT_SYSTEM_ID = "SELECT TemplateID, PublishmentSystemID, TemplateName, TemplateType, RelatedFileName, CreatedFileFullName, CreatedFileExtName, Charset, IsDefault FROM siteserver_Template WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY TemplateType, RelatedFileName";

        private const string SQL_SELECT_TEMPLATE_NAMES = "SELECT TemplateName FROM siteserver_Template WHERE PublishmentSystemID = @PublishmentSystemID AND TemplateType = @TemplateType";

        private const string SQL_SELECT_TEMPLATE_COUNT = "SELECT TemplateType, COUNT(*) FROM siteserver_Template WHERE PublishmentSystemID = @PublishmentSystemID GROUP BY TemplateType";

        private const string SQL_SELECT_RELATED_FILE_NAME_BY_TEMPLATE_TYPE = "SELECT RelatedFileName FROM siteserver_Template WHERE PublishmentSystemID = @PublishmentSystemID AND TemplateType = @TemplateType";

        private const string SQL_UPDATE_TEMPLATE = "UPDATE siteserver_Template SET TemplateName = @TemplateName, TemplateType = @TemplateType, RelatedFileName = @RelatedFileName, CreatedFileFullName = @CreatedFileFullName, CreatedFileExtName = @CreatedFileExtName, Charset = @Charset, IsDefault = @IsDefault WHERE  TemplateID = @TemplateID";

        private const string SQL_DELETE_TEMPLATE = "DELETE FROM siteserver_Template WHERE  TemplateID = @TemplateID";

        //by 20151106 sofuny
        private const string SQL_SELECT_TEMPLATE_BY_URL_TYPE = "SELECT * FROM siteserver_Template WHERE PublishmentSystemID = @PublishmentSystemID AND TemplateType = @TemplateType and CreatedFileFullName=@CreatedFileFullName ";
        private const string SQL_SELECT_TEMPLATE_BY_TEMPLATEID = "SELECT * FROM siteserver_Template WHERE PublishmentSystemID = @PublishmentSystemID AND TemplateType = @TemplateType and TemplateID = @TemplateID ";

        private const string PARM_TEMPLATE_ID = "@TemplateID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_TEMPLATE_NAME = "@TemplateName";
        private const string PARM_TEMPLATE_TYPE = "@TemplateType";
        private const string PARM_RELATED_FILE_NAME = "@RelatedFileName";
        private const string PARM_CREATED_FILE_FULL_NAME = "@CreatedFileFullName";
        private const string PARM_CREATED_FILE_EXT_NAME = "@CreatedFileExtName";
        private const string PARM_CHARSET = "@Charset";
        private const string PARM_IS_DEFAULT = "@IsDefault";

        public int Insert(TemplateInfo templateInfo, string templateContent)
        {
            int templateID = 0;
            if (templateInfo.IsDefault)
            {
                this.SetAllTemplateDefaultToFalse(templateInfo.PublishmentSystemID, templateInfo.TemplateType);
            }

            string SQL_INSERT_TEMPLATE = "INSERT INTO siteserver_Template (PublishmentSystemID, TemplateName, TemplateType, RelatedFileName, CreatedFileFullName, CreatedFileExtName, Charset, IsDefault) VALUES (@PublishmentSystemID, @TemplateName, @TemplateType, @RelatedFileName, @CreatedFileFullName, @CreatedFileExtName, @Charset, @IsDefault)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT_TEMPLATE = "INSERT INTO siteserver_Template (TemplateID, PublishmentSystemID, TemplateName, TemplateType, RelatedFileName, CreatedFileFullName, CreatedFileExtName, Charset, IsDefault) VALUES (siteserver_Template_SEQ.NEXTVAL, @PublishmentSystemID, @TemplateName, @TemplateType, @RelatedFileName, @CreatedFileFullName, @CreatedFileExtName, @Charset, @IsDefault)";
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, templateInfo.PublishmentSystemID),
				this.GetParameter(PARM_TEMPLATE_NAME, EDataType.NVarChar, 50, templateInfo.TemplateName),
				this.GetParameter(PARM_TEMPLATE_TYPE, EDataType.VarChar, 50, ETemplateTypeUtils.GetValue(templateInfo.TemplateType)),
				this.GetParameter(PARM_RELATED_FILE_NAME, EDataType.NVarChar, 50, templateInfo.RelatedFileName),
				this.GetParameter(PARM_CREATED_FILE_FULL_NAME, EDataType.NVarChar, 50, templateInfo.CreatedFileFullName),
				this.GetParameter(PARM_CREATED_FILE_EXT_NAME, EDataType.VarChar, 50, templateInfo.CreatedFileExtName),
                this.GetParameter(PARM_CHARSET, EDataType.VarChar, 50, ECharsetUtils.GetValue(templateInfo.Charset)),
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, templateInfo.IsDefault.ToString())
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT_TEMPLATE, insertParms);

                        templateID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "siteserver_Template");

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(templateInfo.PublishmentSystemID);
            TemplateManager.WriteContentToTemplateFile(publishmentSystemInfo, templateInfo, templateContent);

            TemplateManager.RemoveCache(templateInfo.PublishmentSystemID);

            return templateID;
        }

        public void Update(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string templateContent)
        {
            if (templateInfo.IsDefault)
            {
                this.SetAllTemplateDefaultToFalse(publishmentSystemInfo.PublishmentSystemID, templateInfo.TemplateType);
            }

            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TEMPLATE_NAME, EDataType.NVarChar, 50, templateInfo.TemplateName),
				this.GetParameter(PARM_TEMPLATE_TYPE, EDataType.VarChar, 50, ETemplateTypeUtils.GetValue(templateInfo.TemplateType)),
				this.GetParameter(PARM_RELATED_FILE_NAME, EDataType.NVarChar, 50, templateInfo.RelatedFileName),
				this.GetParameter(PARM_CREATED_FILE_FULL_NAME, EDataType.NVarChar, 50, templateInfo.CreatedFileFullName),
				this.GetParameter(PARM_CREATED_FILE_EXT_NAME, EDataType.VarChar, 50, templateInfo.CreatedFileExtName),
				this.GetParameter(PARM_CHARSET, EDataType.VarChar, 50, ECharsetUtils.GetValue(templateInfo.Charset)),
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, templateInfo.IsDefault.ToString()),
				this.GetParameter(PARM_TEMPLATE_ID, EDataType.Integer, templateInfo.TemplateID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_TEMPLATE, updateParms);

            TemplateManager.WriteContentToTemplateFile(publishmentSystemInfo, templateInfo, templateContent);

            TemplateManager.RemoveCache(templateInfo.PublishmentSystemID);
        }

        private void SetAllTemplateDefaultToFalse(int publishmentSystemID, ETemplateType templateType)
        {
            string sqlString = "UPDATE siteserver_Template SET IsDefault = @IsDefault WHERE PublishmentSystemID = @PublishmentSystemID AND TemplateType = @TemplateType";

            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, false.ToString()),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TEMPLATE_TYPE, EDataType.VarChar, 50, ETemplateTypeUtils.GetValue(templateType))
			};

            this.ExecuteNonQuery(sqlString, updateParms);

        }

        public void SetDefault(int publishmentSystemID, int templateID)
        {
            TemplateInfo info = TemplateManager.GetTemplateInfo(publishmentSystemID, templateID);
            this.SetAllTemplateDefaultToFalse(info.PublishmentSystemID, info.TemplateType);

            string sqlString = "UPDATE siteserver_Template SET IsDefault = @IsDefault WHERE TemplateID = @TemplateID";

            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, true.ToString()),
				this.GetParameter(PARM_TEMPLATE_ID, EDataType.Integer, templateID)
			};

            this.ExecuteNonQuery(sqlString, updateParms);

            TemplateManager.RemoveCache(publishmentSystemID);
        }

        public void Delete(int publishmentSystemID, int templateID)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(publishmentSystemID, templateID);
            string filePath = TemplateManager.GetTemplateFilePath(publishmentSystemInfo, templateInfo);

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TEMPLATE_ID, EDataType.Integer, templateID)
			};

            this.ExecuteNonQuery(SQL_DELETE_TEMPLATE, parms);
            FileUtils.DeleteFileIfExists(filePath);

            TemplateManager.RemoveCache(publishmentSystemID);
        }

        public string GetImportTemplateName(int publishmentSystemID, string templateName)
        {
            string importTemplateName = "";
            if (templateName.IndexOf("_") != -1)
            {
                int templateName_Count = 0;
                string lastTemplateName = templateName.Substring(templateName.LastIndexOf("_") + 1);
                string firstTemplateName = templateName.Substring(0, templateName.Length - lastTemplateName.Length);
                try
                {
                    templateName_Count = int.Parse(lastTemplateName);
                }
                catch { }
                templateName_Count++;
                importTemplateName = firstTemplateName + templateName_Count;
            }
            else
            {
                importTemplateName = templateName + "_1";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TEMPLATE_NAME, EDataType.NVarChar, 50, importTemplateName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TEMPLATE_BY_TEMPLATE_NAME, parms))
            {
                if (rdr.Read())
                {
                    importTemplateName = GetImportTemplateName(publishmentSystemID, importTemplateName);
                }
                rdr.Close();
            }

            return importTemplateName;
        }

        public Dictionary<ETemplateType, int> GetCountDictionary(int publishmentSystemID)
        {
            Dictionary<ETemplateType, int> dictionary = new Dictionary<ETemplateType, int>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(TemplateDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TEMPLATE_COUNT, parms))
            {
                while (rdr.Read())
                {
                    ETemplateType templateType = ETemplateTypeUtils.GetEnumType(rdr.GetValue(0).ToString());
                    int count = rdr.GetInt32(1);

                    dictionary.Add(templateType, count);
                }
                rdr.Close();
            }

            return dictionary;
        }

        public IEnumerable GetDataSourceByType(int publishmentSystemID, ETemplateType type)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(TemplateDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(TemplateDAO.PARM_TEMPLATE_TYPE, EDataType.VarChar, 50, ETemplateTypeUtils.GetValue(type))
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_TEMPLATE_BY_TYPE, parms);
            return enumerable;
        }

        public IEnumerable GetDataSource(int publishmentSystemID, string searchText, string templateTypeString)
        {
            if (string.IsNullOrEmpty(searchText) && string.IsNullOrEmpty(templateTypeString))
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
				{
					this.GetParameter(TemplateDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
				};

                IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_TEMPLATE_BY_PUBLISHMENT_SYSTEM_ID, parms);
                return enumerable;
            }
            else if (!string.IsNullOrEmpty(searchText))
            {
                string whereString = (string.IsNullOrEmpty(templateTypeString)) ? string.Empty : string.Format("AND TemplateType = '{0}' ", templateTypeString);
                whereString += string.Format("AND (TemplateName LIKE '%{0}%' OR RelatedFileName LIKE '%{0}%' OR CreatedFileFullName LIKE '%{0}%' OR CreatedFileExtName LIKE '%{0}%')", PageUtils.FilterSql(searchText));
                string sqlString = string.Format("SELECT TemplateID, PublishmentSystemID, TemplateName, TemplateType, RelatedFileName, CreatedFileFullName, CreatedFileExtName, Charset, IsDefault FROM siteserver_Template WHERE PublishmentSystemID = {0} {1} ORDER BY TemplateType, RelatedFileName", publishmentSystemID, whereString);

                IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
                return enumerable;
            }
            else
            {
                return this.GetDataSourceByType(publishmentSystemID, ETemplateTypeUtils.GetEnumType(templateTypeString));
            }
        }

        public ArrayList GetTemplateIDArrayListByType(int publishmentSystemID, ETemplateType type)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(TemplateDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(TemplateDAO.PARM_TEMPLATE_TYPE, EDataType.VarChar, 50, ETemplateTypeUtils.GetValue(type))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_TEMPLATE_ID_BY_TYPE, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int templateID = Convert.ToInt32(rdr[0]);
                        arraylist.Add(templateID);
                    }
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetTemplateInfoArrayListByType(int publishmentSystemID, ETemplateType type)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(TemplateDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(TemplateDAO.PARM_TEMPLATE_TYPE, EDataType.VarChar, 50, ETemplateTypeUtils.GetValue(type))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_TEMPLATE_BY_TYPE, parms))
            {
                while (rdr.Read())
                {
                    TemplateInfo info = new TemplateInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), ETemplateTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), ECharsetUtils.GetEnumType(rdr.GetValue(7).ToString()), TranslateUtils.ToBool(rdr.GetValue(8).ToString()));
                    arraylist.Add(info);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetTemplateInfoArrayListOfFile(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT TemplateID, PublishmentSystemID, TemplateName, TemplateType, RelatedFileName, CreatedFileFullName, CreatedFileExtName, Charset, IsDefault FROM siteserver_Template WHERE PublishmentSystemID = {0} AND TemplateType = '{1}' ORDER BY RelatedFileName", publishmentSystemID, ETemplateTypeUtils.GetValue(ETemplateType.FileTemplate));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    TemplateInfo info = new TemplateInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), ETemplateTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), ECharsetUtils.GetEnumType(rdr.GetValue(7).ToString()), TranslateUtils.ToBool(rdr.GetValue(8).ToString()));
                    arraylist.Add(info);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetTemplateInfoArrayListByPublishmentSystemID(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(TemplateDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_TEMPLATE_BY_PUBLISHMENT_SYSTEM_ID, parms))
            {
                while (rdr.Read())
                {
                    TemplateInfo info = new TemplateInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), ETemplateTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), ECharsetUtils.GetEnumType(rdr.GetValue(7).ToString()), TranslateUtils.ToBool(rdr.GetValue(8).ToString()));
                    arraylist.Add(info);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetTemplateNameArrayList(int publishmentSystemID, ETemplateType templateType)
        {
            ArrayList list = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(TemplateDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TEMPLATE_TYPE, EDataType.VarChar, 50, ETemplateTypeUtils.GetValue(templateType))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TEMPLATE_NAMES, parms))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return list;
        }

        public ArrayList GetLowerRelatedFileNameArrayList(int publishmentSystemID, ETemplateType templateType)
        {
            ArrayList list = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(TemplateDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TEMPLATE_TYPE, EDataType.VarChar, 50, ETemplateTypeUtils.GetValue(templateType))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_RELATED_FILE_NAME_BY_TEMPLATE_TYPE, parms))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetValue(0).ToString().ToLower());
                }
                rdr.Close();
            }

            return list;
        }

        public void CreateDefaultTemplateInfo(int publishmentSystemID)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            if (publishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.BBS)
            {
                string sourcePath = PathUtils.Combine(PathUtils.GetAppPath(AppManager.BBS.AppID), DirectoryUtils.Products.Apps.Directory_Files);
                string targetPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);

                if (DirectoryUtils.IsDirectoryExists(sourcePath))
                {
                    DirectoryUtils.Copy(sourcePath, targetPath, true);
                }
            }
            else if (publishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.CRM)
            {

            }
            else
            {
                List<TemplateInfo> templateInfoList = new List<TemplateInfo>();
                ECharset charset = ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset);

                TemplateInfo templateInfo = new TemplateInfo(0, publishmentSystemInfo.PublishmentSystemID, "系统首页模板", ETemplateType.IndexPageTemplate, "T_系统首页模板.htm", "@/index.htm", ".htm", charset, true);
                templateInfoList.Add(templateInfo);

                templateInfo = new TemplateInfo(0, publishmentSystemInfo.PublishmentSystemID, "系统栏目模板", ETemplateType.ChannelTemplate, "T_系统栏目模板.html", "index.html", ".html", charset, true);
                templateInfoList.Add(templateInfo);

                templateInfo = new TemplateInfo(0, publishmentSystemInfo.PublishmentSystemID, "系统内容模板", ETemplateType.ContentTemplate, "T_系统内容模板.html", "index.html", ".html", charset, true);
                templateInfoList.Add(templateInfo);

                foreach (TemplateInfo theTemplateInfo in templateInfoList)
                {
                    this.Insert(theTemplateInfo, theTemplateInfo.Content);
                }
            }
        }

        public int GetTemplateUseCount(int publishmentSystemID, int templateID, ETemplateType templateType, bool isDefault)
        {
            int useCount = 0;
            string sqlString = string.Empty;

            if (templateType == ETemplateType.IndexPageTemplate)
            {
                if (isDefault)
                {
                    return 1;
                }
                return 0;
            }
            else if (templateType == ETemplateType.FileTemplate)
            {
                return 1;
            }
            else if (templateType == ETemplateType.ChannelTemplate)
            {
                if (isDefault)
                {
                    sqlString = string.Format("SELECT count(*) FROM siteserver_Node WHERE (ChannelTemplateID = {0} OR ChannelTemplateID = 0) AND PublishmentSystemID = {1}", templateID, publishmentSystemID);
                }
                else
                {
                    sqlString = string.Format("SELECT count(*) FROM siteserver_Node WHERE ChannelTemplateID = {0}", templateID);
                }
            }
            else if (templateType == ETemplateType.ContentTemplate)
            {
                if (isDefault)
                {
                    sqlString = string.Format("SELECT count(*) FROM siteserver_Node WHERE (ContentTemplateID = {0} OR ContentTemplateID = 0) AND PublishmentSystemID = {1}", templateID, publishmentSystemID);
                }
                else
                {
                    sqlString = string.Format("SELECT count(*) FROM siteserver_Node WHERE ContentTemplateID = {0}", templateID);
                }
            }

            useCount = BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);

            return useCount;
        }

        public ArrayList GetNodeIDArrayList(TemplateInfo templateInfo)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Empty;

            if (templateInfo.TemplateType == ETemplateType.ChannelTemplate)
            {
                if (templateInfo.IsDefault)
                {
                    sqlString = string.Format("SELECT NodeID FROM siteserver_Node WHERE (ChannelTemplateID = {0} OR ChannelTemplateID = 0) AND PublishmentSystemID = {1}", templateInfo.TemplateID, templateInfo.PublishmentSystemID);
                }
                else
                {
                    sqlString = string.Format("SELECT NodeID FROM siteserver_Node WHERE ChannelTemplateID = {0} AND PublishmentSystemID = {1}", templateInfo.TemplateID, templateInfo.PublishmentSystemID);
                }
            }
            else if (templateInfo.TemplateType == ETemplateType.ContentTemplate)
            {
                if (templateInfo.IsDefault)
                {
                    sqlString = string.Format("SELECT NodeID FROM siteserver_Node WHERE (ContentTemplateID = {0} OR ContentTemplateID = 0) AND PublishmentSystemID = {1}", templateInfo.TemplateID, templateInfo.PublishmentSystemID);
                }
                else
                {
                    sqlString = string.Format("SELECT NodeID FROM siteserver_Node WHERE ContentTemplateID = {0} AND PublishmentSystemID = {1}", templateInfo.TemplateID, templateInfo.PublishmentSystemID);
                }
            }

            if (!string.IsNullOrEmpty(sqlString))
            {
                arraylist = BaiRongDataProvider.DatabaseDAO.GetIntArrayList(sqlString);
            }

            return arraylist;
        }

        public Dictionary<int, TemplateInfo> GetTemplateInfoDictionaryByPublishmentSystemID(int publishmentSystemID)
        {
            Dictionary<int, TemplateInfo> dictionary = new Dictionary<int, TemplateInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(TemplateDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_TEMPLATE_BY_PUBLISHMENT_SYSTEM_ID, parms))
            {
                while (rdr.Read())
                {
                    TemplateInfo info = new TemplateInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), ETemplateTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), ECharsetUtils.GetEnumType(rdr.GetValue(7).ToString()), TranslateUtils.ToBool(rdr.GetValue(8).ToString()));
                    dictionary.Add(info.TemplateID, info);
                }
                rdr.Close();
            }

            return dictionary;
        }


        public TemplateInfo GetTemplateByURLType(int publishmentSystemID, ETemplateType type, string createdFileFullName)
        {
            TemplateInfo info = null;
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(TemplateDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(TemplateDAO.PARM_TEMPLATE_TYPE, EDataType.VarChar, 50, ETemplateTypeUtils.GetValue(type)),
				this.GetParameter(TemplateDAO.PARM_CREATED_FILE_FULL_NAME, EDataType.VarChar, 50, createdFileFullName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TEMPLATE_BY_URL_TYPE, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        info = new TemplateInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), ETemplateTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), ECharsetUtils.GetEnumType(rdr.GetValue(7).ToString()), TranslateUtils.ToBool(rdr.GetValue(8).ToString()));
                    }
                }
                rdr.Close();
            }
            return info;
        }

        public TemplateInfo GetTemplateByTemplateID(int publishmentSystemID, ETemplateType type, string tID)
        {
            TemplateInfo info = null;
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(TemplateDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(TemplateDAO.PARM_TEMPLATE_TYPE, EDataType.VarChar, 50, ETemplateTypeUtils.GetValue(type)),
				this.GetParameter(TemplateDAO.PARM_TEMPLATE_ID, EDataType.Integer, tID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TEMPLATE_BY_TEMPLATEID, parms))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        info = new TemplateInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), ETemplateTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), ECharsetUtils.GetEnumType(rdr.GetValue(7).ToString()), TranslateUtils.ToBool(rdr.GetValue(8).ToString()));
                    }
                }
                rdr.Close();
            }
            return info;
        }

    }
}
