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
    public class TagStyleDAO : DataProviderBase, ITagStyleDAO
	{
        private const string SQL_UPDATE = "UPDATE siteserver_TagStyle SET StyleName = @StyleName, IsTemplate = @IsTemplate, StyleTemplate = @StyleTemplate, ScriptTemplate = @ScriptTemplate, ContentTemplate = @ContentTemplate, SuccessTemplate = @SuccessTemplate, FailureTemplate = @FailureTemplate, SettingsXML = @SettingsXML WHERE StyleID = @StyleID";

        private const string SQL_DELETE = "DELETE FROM siteserver_TagStyle WHERE StyleID = @StyleID";

        private const string SQL_SELECT = "SELECT StyleID, StyleName, ElementName, PublishmentSystemID, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SuccessTemplate, FailureTemplate, SettingsXML FROM siteserver_TagStyle WHERE StyleID = @StyleID";

        private const string SQL_SELECT_ALL_BY_ELEMENT_NAME = "SELECT StyleID, StyleName, ElementName, PublishmentSystemID, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SuccessTemplate, FailureTemplate, SettingsXML FROM siteserver_TagStyle WHERE PublishmentSystemID = @PublishmentSystemID AND ElementName = @ElementName ORDER BY StyleID";

        private const string SQL_SELECT_ALL = "SELECT StyleID, StyleName, ElementName, PublishmentSystemID, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SuccessTemplate, FailureTemplate, SettingsXML FROM siteserver_TagStyle WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY StyleID";

        private const string SQL_SELECT_STYLE_NAME = "SELECT StyleName FROM siteserver_TagStyle WHERE PublishmentSystemID = @PublishmentSystemID AND ElementName = @ElementName";

        private const string PARM_STYLE_ID = "@StyleID";
        private const string PARM_STYLE_NAME = "@StyleName";
        private const string PARM_ELEMENT_NAME = "@ElementName";
		private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_ISTEMPLATE = "@IsTemplate";
        private const string PARM_STYLE_TEMPLATE = "@StyleTemplate";
        private const string PARM_SCRIPT_TEMPLATE = "@ScriptTemplate";
        private const string PARM_CONTENT_TEMPLATE = "@ContentTemplate";
        private const string PARM_SUCCESS_TEMPLATE = "@SuccessTemplate";
        private const string PARM_FAILURE_TEMPLATE = "@FailureTemplate";
        private const string PARM_SETTINGS_XML = "@SettingsXML";

		public int Insert(TagStyleInfo tagStyleInfo) 
		{
            int styleID = 0;

            string sqlString = "INSERT INTO siteserver_TagStyle (StyleName, ElementName, PublishmentSystemID, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SuccessTemplate, FailureTemplate, SettingsXML) VALUES (@StyleName, @ElementName, @PublishmentSystemID, @IsTemplate, @StyleTemplate, @ScriptTemplate, @ContentTemplate, @SuccessTemplate, @FailureTemplate, @SettingsXML)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_TagStyle (StyleID, StyleName, ElementName, PublishmentSystemID, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SuccessTemplate, FailureTemplate, SettingsXML) VALUES (siteserver_TagStyle_SEQ.NEXTVAL, @StyleName, @ElementName, @PublishmentSystemID, @IsTemplate, @StyleTemplate, @ScriptTemplate, @ContentTemplate, @SuccessTemplate, @FailureTemplate, @SettingsXML)";
            }

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_STYLE_NAME, EDataType.NVarChar, 50, tagStyleInfo.StyleName),
                this.GetParameter(PARM_ELEMENT_NAME, EDataType.VarChar, 50, tagStyleInfo.ElementName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, tagStyleInfo.PublishmentSystemID),
                this.GetParameter(PARM_ISTEMPLATE, EDataType.VarChar, 18, tagStyleInfo.IsTemplate.ToString()),
                this.GetParameter(PARM_STYLE_TEMPLATE, EDataType.NText, tagStyleInfo.StyleTemplate),
                this.GetParameter(PARM_SCRIPT_TEMPLATE, EDataType.NText, tagStyleInfo.ScriptTemplate),
                this.GetParameter(PARM_CONTENT_TEMPLATE, EDataType.NText, tagStyleInfo.ContentTemplate),
                this.GetParameter(PARM_SUCCESS_TEMPLATE, EDataType.NText, tagStyleInfo.SuccessTemplate),
                this.GetParameter(PARM_FAILURE_TEMPLATE, EDataType.NText, tagStyleInfo.FailureTemplate),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, tagStyleInfo.SettingsXML)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);

                        styleID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "siteserver_TagStyle");

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return styleID;
		}

        public void Update(TagStyleInfo tagStyleInfo) 
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_STYLE_NAME, EDataType.NVarChar, 50, tagStyleInfo.StyleName),
                this.GetParameter(PARM_ISTEMPLATE, EDataType.VarChar, 18, tagStyleInfo.IsTemplate.ToString()),
                this.GetParameter(PARM_STYLE_TEMPLATE, EDataType.NText, tagStyleInfo.StyleTemplate),
                this.GetParameter(PARM_SCRIPT_TEMPLATE, EDataType.NText, tagStyleInfo.ScriptTemplate),
                this.GetParameter(PARM_CONTENT_TEMPLATE, EDataType.NText, tagStyleInfo.ContentTemplate),
                this.GetParameter(PARM_SUCCESS_TEMPLATE, EDataType.NText, tagStyleInfo.SuccessTemplate),
                this.GetParameter(PARM_FAILURE_TEMPLATE, EDataType.NText, tagStyleInfo.FailureTemplate),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, tagStyleInfo.SettingsXML),
				this.GetParameter(PARM_STYLE_ID, EDataType.Integer, tagStyleInfo.StyleID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);

            TagStyleManager.RemoveCache(tagStyleInfo.PublishmentSystemID, tagStyleInfo.ElementName, tagStyleInfo.StyleName);
		}

		public void Delete(int styleID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_STYLE_ID, EDataType.Integer, styleID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
		}

        public TagStyleInfo GetTagStyleInfo(int styleID)
		{
            TagStyleInfo tagStyleInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_STYLE_ID, EDataType.Integer, styleID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    tagStyleInfo = new TagStyleInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetInt32(3), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString());
                }
                rdr.Close();
            }

            return tagStyleInfo;
		}

        public TagStyleInfo GetTagStyleInfo(int publishmentSystemID, string elementName, string styleName)
        {
            TagStyleInfo tagStyleInfo = null;

            string sqlString = "SELECT StyleID, StyleName, ElementName, PublishmentSystemID, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SuccessTemplate, FailureTemplate, SettingsXML FROM siteserver_TagStyle WHERE PublishmentSystemID = @PublishmentSystemID AND ElementName = @ElementName AND StyleName = @StyleName";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_ELEMENT_NAME, EDataType.VarChar, 50, elementName),
                this.GetParameter(PARM_STYLE_NAME, EDataType.NVarChar, 50, styleName)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    tagStyleInfo = new TagStyleInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetInt32(3), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString());
                }
                rdr.Close();
            }

            return tagStyleInfo;
        }

        public ArrayList GetTagStyleInfoArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    TagStyleInfo tagStyleInfo = new TagStyleInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetInt32(3), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString());
                    arraylist.Add(tagStyleInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public IEnumerable GetDataSource(int publishmentSystemID, string elementName)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_ELEMENT_NAME, EDataType.VarChar, 50, elementName)
			};
            return (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_BY_ELEMENT_NAME, parms);
		}

        public ArrayList GetStyleNameArrayList(int publishmentSystemID, string elementName)
		{
			ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_ELEMENT_NAME, EDataType.VarChar, 50, elementName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_STYLE_NAME, parms)) 
			{
				while (rdr.Read()) 
				{
                    arraylist.Add(rdr.GetValue(0).ToString());
				}
				rdr.Close();
			}

			return arraylist;
		}

        public string GetImportStyleName(int publishmentSystemID, string elementName, string styleName)
        {
            string importStyleName = "";
            if (styleName.IndexOf("_") != -1)
            {
                int styleName_Count = 0;
                string lastStyleName = styleName.Substring(styleName.LastIndexOf("_") + 1);
                string firstStyleName = styleName.Substring(0, styleName.Length - lastStyleName.Length);
                try
                {
                    styleName_Count = int.Parse(lastStyleName);
                }
                catch { }
                styleName_Count++;
                importStyleName = firstStyleName + styleName_Count;
            }
            else
            {
                importStyleName = styleName + "_1";
            }

            TagStyleInfo styleInfo = this.GetTagStyleInfo(publishmentSystemID, elementName, importStyleName);

            if (styleInfo != null)
            {
                importStyleName = GetImportStyleName(publishmentSystemID, elementName, importStyleName);
            }

            return importStyleName;
        }
	}
}