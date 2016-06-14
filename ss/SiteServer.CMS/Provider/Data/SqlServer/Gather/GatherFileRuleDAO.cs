using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
	public class GatherFileRuleDAO : DataProviderBase, IGatherFileRuleDAO
	{
        private const string SQL_SELECT_GATHER_FILE_RULE = "SELECT GatherRuleName, PublishmentSystemID, GatherUrl, Charset, LastGatherDate, IsToFile, FilePath, IsSaveRelatedFiles, IsRemoveScripts, StyleDirectoryPath, ScriptDirectoryPath, ImageDirectoryPath, NodeID, IsSaveImage, IsChecked, ContentExclude, ContentHtmlClearCollection, ContentHtmlClearTagCollection, ContentTitleStart, ContentTitleEnd, ContentContentStart, ContentContentEnd, ContentAttributes, ContentAttributesXML, IsAutoCreate FROM siteserver_GatherFileRule WHERE GatherRuleName = @GatherRuleName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ALL_GATHER_FILE_RULE_BY_PS_ID = "SELECT GatherRuleName, PublishmentSystemID, GatherUrl, Charset, LastGatherDate, IsToFile, FilePath, IsSaveRelatedFiles, IsRemoveScripts, StyleDirectoryPath, ScriptDirectoryPath, ImageDirectoryPath, NodeID, IsSaveImage, IsChecked, ContentExclude, ContentHtmlClearCollection, ContentHtmlClearTagCollection, ContentTitleStart, ContentTitleEnd, ContentContentStart, ContentContentEnd, ContentAttributes, ContentAttributesXML, IsAutoCreate FROM siteserver_GatherFileRule WHERE PublishmentSystemID = @PublishmentSystemID";

		private const string SQL_SELECT_GATHER_FILE_RULE_NAME_BY_PS_ID = "SELECT GatherRuleName FROM siteserver_GatherFileRule WHERE PublishmentSystemID = @PublishmentSystemID";

		private const string SQL_INSERT_GATHER_FILE_RULE = @"
INSERT INTO siteserver_GatherFileRule 
(GatherRuleName, PublishmentSystemID, GatherUrl, Charset, LastGatherDate, IsToFile, FilePath, IsSaveRelatedFiles, IsRemoveScripts, StyleDirectoryPath, ScriptDirectoryPath, ImageDirectoryPath, NodeID, IsSaveImage, IsChecked, ContentExclude, ContentHtmlClearCollection, ContentHtmlClearTagCollection, ContentTitleStart, ContentTitleEnd, ContentContentStart, ContentContentEnd, ContentAttributes, ContentAttributesXML, IsAutoCreate) VALUES (@GatherRuleName, @PublishmentSystemID, @GatherUrl, @Charset, @LastGatherDate, @IsToFile, @FilePath, @IsSaveRelatedFiles, @IsRemoveScripts, @StyleDirectoryPath, @ScriptDirectoryPath, @ImageDirectoryPath, @NodeID, @IsSaveImage, @IsChecked, @ContentExclude, @ContentHtmlClearCollection, @ContentHtmlClearTagCollection, @ContentTitleStart, @ContentTitleEnd, @ContentContentStart, @ContentContentEnd, @ContentAttributes, @ContentAttributesXML, @IsAutoCreate)";

		private const string SQL_UPDATE_GATHER_FILE_RULE = @"
UPDATE siteserver_GatherFileRule SET 
GatherUrl = @GatherUrl, Charset = @Charset, LastGatherDate = @LastGatherDate, IsToFile = @IsToFile, FilePath = @FilePath, IsSaveRelatedFiles = @IsSaveRelatedFiles, IsRemoveScripts = @IsRemoveScripts, StyleDirectoryPath = @StyleDirectoryPath, ScriptDirectoryPath = @ScriptDirectoryPath, ImageDirectoryPath = @ImageDirectoryPath, NodeID = @NodeID, IsSaveImage = @IsSaveImage, IsChecked = @IsChecked, ContentExclude = @ContentExclude, ContentHtmlClearCollection = @ContentHtmlClearCollection, ContentHtmlClearTagCollection = @ContentHtmlClearTagCollection, ContentTitleStart = @ContentTitleStart, ContentTitleEnd = @ContentTitleEnd, ContentContentStart = @ContentContentStart, ContentContentEnd = @ContentContentEnd, ContentAttributes = @ContentAttributes, ContentAttributesXML = @ContentAttributesXML, IsAutoCreate = @IsAutoCreate WHERE GatherRuleName = @GatherRuleName AND PublishmentSystemID = @PublishmentSystemID";

		private const string SQL_UPDATE_LAST_GATHER_DATE = "UPDATE siteserver_GatherFileRule SET LastGatherDate = @LastGatherDate WHERE GatherRuleName = @GatherRuleName AND PublishmentSystemID = @PublishmentSystemID";

		private const string SQL_DELETE_GATHER_FILE_RULE = "DELETE FROM siteserver_GatherFileRule WHERE GatherRuleName = @GatherRuleName AND PublishmentSystemID = @PublishmentSystemID";

		private const string PARM_GATHER_FILE_RULE_NAME = "@GatherRuleName";
		private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_GATHER_URL = "@GatherUrl";
        private const string PARM_CHARSET = "@Charset";
        private const string PARM_LAST_GATHER_DATE = "@LastGatherDate";
        private const string PARM_IS_TO_FILE = "@IsToFile";
        private const string PARM_FILE_PATH = "@FilePath";
        private const string PARM_IS_SAVE_RELATED_FILES = "@IsSaveRelatedFiles";
        private const string PARM_IS_REMOVE_SCRIPTS = "@IsRemoveScripts";
        private const string PARM_STYLE_DIRECTORY_PATH = "@StyleDirectoryPath";
        private const string PARM_SCRIPT_DIRECTORY_PATH = "@ScriptDirectoryPath";
        private const string PARM_IMAGE_DIRECTORY_PATH = "@ImageDirectoryPath";

        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_IS_SAVE_IMAGE = "@IsSaveImage";
        private const string PARM_IS_CHECKED = "@IsChecked";
        private const string PARM_CONTENT_EXCLUDE = "@ContentExclude";
        private const string PARM_CONTENT_HTML_CLEAR_COLLECTION = "@ContentHtmlClearCollection";
        private const string PARM_CONTENT_HTML_CLEAR_TAG_COLLECTION = "@ContentHtmlClearTagCollection";
		private const string PARM_CONTENT_TITLE_START = "@ContentTitleStart";
		private const string PARM_CONTENT_TITLE_END = "@ContentTitleEnd";
		private const string PARM_CONTENT_CONTENT_START = "@ContentContentStart";
		private const string PARM_CONTENT_CONTENT_END = "@ContentContentEnd";
        private const string PARM_CONTENT_ATTRIBUTES = "@ContentAttributes";
        private const string PARM_CONTENT_ATTRIBUTES_XML = "@ContentAttributesXML";
        private const string PARM_IS_AUTO_CREATE = "@IsAutoCreate";

        public void Insert(GatherFileRuleInfo gatherFileRuleInfo) 
		{
			IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(GatherFileRuleDAO.PARM_GATHER_FILE_RULE_NAME, EDataType.NVarChar, 50, gatherFileRuleInfo.GatherRuleName),
				this.GetParameter(GatherFileRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, gatherFileRuleInfo.PublishmentSystemID),
				this.GetParameter(GatherFileRuleDAO.PARM_GATHER_URL, EDataType.NVarChar, 255, gatherFileRuleInfo.GatherUrl),
				this.GetParameter(GatherFileRuleDAO.PARM_CHARSET, EDataType.VarChar, 50, ECharsetUtils.GetValue(gatherFileRuleInfo.Charset)),
                this.GetParameter(GatherFileRuleDAO.PARM_LAST_GATHER_DATE, EDataType.DateTime, gatherFileRuleInfo.LastGatherDate),
                this.GetParameter(GatherFileRuleDAO.PARM_IS_TO_FILE, EDataType.VarChar, 18, gatherFileRuleInfo.IsToFile.ToString()),
                this.GetParameter(GatherFileRuleDAO.PARM_FILE_PATH, EDataType.NVarChar, 255, gatherFileRuleInfo.FilePath),
                this.GetParameter(GatherFileRuleDAO.PARM_IS_SAVE_RELATED_FILES, EDataType.VarChar, 18, gatherFileRuleInfo.IsSaveRelatedFiles.ToString()),
                this.GetParameter(GatherFileRuleDAO.PARM_IS_REMOVE_SCRIPTS, EDataType.VarChar, 18, gatherFileRuleInfo.IsRemoveScripts.ToString()),
                this.GetParameter(GatherFileRuleDAO.PARM_STYLE_DIRECTORY_PATH, EDataType.NVarChar, 255, gatherFileRuleInfo.StyleDirectoryPath),
                this.GetParameter(GatherFileRuleDAO.PARM_SCRIPT_DIRECTORY_PATH, EDataType.NVarChar, 255, gatherFileRuleInfo.ScriptDirectoryPath),
                this.GetParameter(GatherFileRuleDAO.PARM_IMAGE_DIRECTORY_PATH, EDataType.NVarChar, 255, gatherFileRuleInfo.ImageDirectoryPath),

                this.GetParameter(GatherFileRuleDAO.PARM_NODE_ID, EDataType.Integer, gatherFileRuleInfo.NodeID),
				this.GetParameter(GatherFileRuleDAO.PARM_IS_SAVE_IMAGE, EDataType.VarChar, 18, gatherFileRuleInfo.IsSaveImage.ToString()),
                this.GetParameter(GatherFileRuleDAO.PARM_IS_CHECKED, EDataType.VarChar, 18, gatherFileRuleInfo.IsChecked.ToString()),
                this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_EXCLUDE, EDataType.NText, gatherFileRuleInfo.ContentExclude),
				this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_HTML_CLEAR_COLLECTION, EDataType.NVarChar, 255, gatherFileRuleInfo.ContentHtmlClearCollection),
                this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_HTML_CLEAR_TAG_COLLECTION, EDataType.NVarChar, 255, gatherFileRuleInfo.ContentHtmlClearTagCollection),
				this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_TITLE_START, EDataType.NText, gatherFileRuleInfo.ContentTitleStart),
				this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_TITLE_END, EDataType.NText, gatherFileRuleInfo.ContentTitleEnd),
				this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_CONTENT_START, EDataType.NText, gatherFileRuleInfo.ContentContentStart),
				this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_CONTENT_END, EDataType.NText, gatherFileRuleInfo.ContentContentEnd),
                this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_ATTRIBUTES, EDataType.NText, gatherFileRuleInfo.ContentAttributes),
                this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_ATTRIBUTES_XML, EDataType.NText, gatherFileRuleInfo.ContentAttributesXML),
                this.GetParameter(GatherFileRuleDAO.PARM_IS_AUTO_CREATE, EDataType.VarChar, 18, gatherFileRuleInfo.IsAutoCreate.ToString())
            };

            this.ExecuteNonQuery(SQL_INSERT_GATHER_FILE_RULE, insertParms);
		}

		public void UpdateLastGatherDate(string gatherRuleName, int publishmentSystemID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(GatherFileRuleDAO.PARM_LAST_GATHER_DATE, EDataType.DateTime, DateTime.Now),
				this.GetParameter(PARM_GATHER_FILE_RULE_NAME, EDataType.NVarChar, 50, gatherRuleName),
				this.GetParameter(GatherFileRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};
							
			this.ExecuteNonQuery(SQL_UPDATE_LAST_GATHER_DATE, parms);
		}

		public void Update(GatherFileRuleInfo gatherFileRuleInfo) 
		{

			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(GatherFileRuleDAO.PARM_GATHER_URL, EDataType.NVarChar, 255, gatherFileRuleInfo.GatherUrl),
				this.GetParameter(GatherFileRuleDAO.PARM_CHARSET, EDataType.VarChar, 50, ECharsetUtils.GetValue(gatherFileRuleInfo.Charset)),
                this.GetParameter(GatherFileRuleDAO.PARM_LAST_GATHER_DATE, EDataType.DateTime, gatherFileRuleInfo.LastGatherDate),
                this.GetParameter(GatherFileRuleDAO.PARM_IS_TO_FILE, EDataType.VarChar, 18, gatherFileRuleInfo.IsToFile.ToString()),
                this.GetParameter(GatherFileRuleDAO.PARM_FILE_PATH, EDataType.NVarChar, 255, gatherFileRuleInfo.FilePath),
                this.GetParameter(GatherFileRuleDAO.PARM_IS_SAVE_RELATED_FILES, EDataType.VarChar, 18, gatherFileRuleInfo.IsSaveRelatedFiles.ToString()),
                this.GetParameter(GatherFileRuleDAO.PARM_IS_REMOVE_SCRIPTS, EDataType.VarChar, 18, gatherFileRuleInfo.IsRemoveScripts.ToString()),
                this.GetParameter(GatherFileRuleDAO.PARM_STYLE_DIRECTORY_PATH, EDataType.NVarChar, 255, gatherFileRuleInfo.StyleDirectoryPath),
                this.GetParameter(GatherFileRuleDAO.PARM_SCRIPT_DIRECTORY_PATH, EDataType.NVarChar, 255, gatherFileRuleInfo.ScriptDirectoryPath),
                this.GetParameter(GatherFileRuleDAO.PARM_IMAGE_DIRECTORY_PATH, EDataType.NVarChar, 255, gatherFileRuleInfo.ImageDirectoryPath),

                this.GetParameter(GatherFileRuleDAO.PARM_NODE_ID, EDataType.Integer, gatherFileRuleInfo.NodeID),
				this.GetParameter(GatherFileRuleDAO.PARM_IS_SAVE_IMAGE, EDataType.VarChar, 18, gatherFileRuleInfo.IsSaveImage.ToString()),
                this.GetParameter(GatherFileRuleDAO.PARM_IS_CHECKED, EDataType.VarChar, 18, gatherFileRuleInfo.IsChecked.ToString()),
                this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_EXCLUDE, EDataType.NText, gatherFileRuleInfo.ContentExclude),
				this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_HTML_CLEAR_COLLECTION, EDataType.NVarChar, 255, gatherFileRuleInfo.ContentHtmlClearCollection),
                this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_HTML_CLEAR_TAG_COLLECTION, EDataType.NVarChar, 255, gatherFileRuleInfo.ContentHtmlClearTagCollection),
				this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_TITLE_START, EDataType.NText, gatherFileRuleInfo.ContentTitleStart),
				this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_TITLE_END, EDataType.NText, gatherFileRuleInfo.ContentTitleEnd),
				this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_CONTENT_START, EDataType.NText, gatherFileRuleInfo.ContentContentStart),
				this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_CONTENT_END, EDataType.NText, gatherFileRuleInfo.ContentContentEnd),
                this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_ATTRIBUTES, EDataType.NText, gatherFileRuleInfo.ContentAttributes),
                this.GetParameter(GatherFileRuleDAO.PARM_CONTENT_ATTRIBUTES_XML, EDataType.NText, gatherFileRuleInfo.ContentAttributesXML),
				this.GetParameter(GatherFileRuleDAO.PARM_GATHER_FILE_RULE_NAME, EDataType.NVarChar, 50, gatherFileRuleInfo.GatherRuleName),
                this.GetParameter(GatherFileRuleDAO.PARM_IS_AUTO_CREATE, EDataType.VarChar, 18, gatherFileRuleInfo.IsAutoCreate.ToString()),
                this.GetParameter(GatherFileRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, gatherFileRuleInfo.PublishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_GATHER_FILE_RULE, updateParms);
		}

		public void Delete(string gatherRuleName, int publishmentSystemID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GATHER_FILE_RULE_NAME, EDataType.NVarChar, 50, gatherRuleName),
				this.GetParameter(GatherFileRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};
							
			this.ExecuteNonQuery(SQL_DELETE_GATHER_FILE_RULE, parms);
		}

		public GatherFileRuleInfo GetGatherFileRuleInfo(string gatherRuleName, int publishmentSystemID)
		{
			GatherFileRuleInfo gatherFileRuleInfo = null;
			
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GATHER_FILE_RULE_NAME, EDataType.NVarChar, 50, gatherRuleName),
				this.GetParameter(GatherFileRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};
			
			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_GATHER_FILE_RULE, parms)) 
			{
				if (rdr.Read()) 
				{
                    gatherFileRuleInfo = new GatherFileRuleInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString(), ECharsetUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetDateTime(4), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), TranslateUtils.ToBool(rdr.GetValue(8).ToString()), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), TranslateUtils.ToBool(rdr.GetValue(13).ToString()), TranslateUtils.ToBool(rdr.GetValue(14).ToString()), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString(), rdr.GetValue(17).ToString(), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), rdr.GetValue(22).ToString(), rdr.GetValue(23).ToString(), TranslateUtils.ToBool(rdr.GetValue(24).ToString()));
				}
				rdr.Close();
			}

			return gatherFileRuleInfo;
		}

		public string GetImportGatherRuleName(int publishmentSystemID, string gatherRuleName)
		{
			string importGatherRuleName = "";
			if (gatherRuleName.IndexOf("_") != -1)
			{
				int gatherRuleName_Count = 0;
				string lastGatherRuleName = gatherRuleName.Substring(gatherRuleName.LastIndexOf("_") + 1);
				string firstGatherRuleName = gatherRuleName.Substring(0, gatherRuleName.Length - lastGatherRuleName.Length);
				try
				{
					gatherRuleName_Count = int.Parse(lastGatherRuleName);
				}
				catch { }
				gatherRuleName_Count++;
				importGatherRuleName = firstGatherRuleName + gatherRuleName_Count;
			}
			else
			{
				importGatherRuleName = gatherRuleName + "_1";
			}

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GATHER_FILE_RULE_NAME, EDataType.NVarChar, 50, importGatherRuleName),
				this.GetParameter(GatherFileRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_GATHER_FILE_RULE, parms))
			{
				if (rdr.Read())
				{
					importGatherRuleName = GetImportGatherRuleName(publishmentSystemID, importGatherRuleName);
				}
				rdr.Close();
			}

			return importGatherRuleName;
		}

		public IEnumerable GetDataSource(int publishmentSystemID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(GatherFileRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

			IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_GATHER_FILE_RULE_BY_PS_ID, parms);
			return enumerable;
		}

		public ArrayList GetGatherFileRuleInfoArrayList(int publishmentSystemID)
		{
			ArrayList list = new ArrayList();

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(GatherFileRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_GATHER_FILE_RULE_BY_PS_ID, parms))
			{
				while (rdr.Read())
				{
                    GatherFileRuleInfo gatherFileRuleInfo = new GatherFileRuleInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString(), ECharsetUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetDateTime(4), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), TranslateUtils.ToBool(rdr.GetValue(8).ToString()), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), TranslateUtils.ToBool(rdr.GetValue(13).ToString()), TranslateUtils.ToBool(rdr.GetValue(14).ToString()), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString(), rdr.GetValue(17).ToString(), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), rdr.GetValue(22).ToString(), rdr.GetValue(23).ToString(), TranslateUtils.ToBool(rdr.GetValue(24).ToString()));

                    list.Add(gatherFileRuleInfo);
				}
				rdr.Close();
			}

			return list;
		}

		public ArrayList GetGatherRuleNameArrayList(int publishmentSystemID)
		{
			ArrayList list = new ArrayList();

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_GATHER_FILE_RULE_NAME_BY_PS_ID, parms)) 
			{
				while (rdr.Read()) 
				{					
					list.Add(rdr.GetValue(0).ToString());
				}
				rdr.Close();
			}

			return list;
		}

        public void OpenAuto(int publishmentSystemID, ArrayList gatherRuleNameCollection)
        {
            string sql = string.Format("UPDATE siteserver_GatherFileRule SET IsAutoCreate = 'True' WHERE PublishmentSystemID = {0} AND GatherRuleName in ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(gatherRuleNameCollection));
            this.ExecuteNonQuery(sql);
        }

        public void CloseAuto(int publishmentSystemID, ArrayList gatherRuleNameCollection)
        {
            string sql = string.Format("UPDATE siteserver_GatherFileRule SET IsAutoCreate = 'False' WHERE PublishmentSystemID = {0} AND GatherRuleName in ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(gatherRuleNameCollection));
            this.ExecuteNonQuery(sql);
        }

    }
}
