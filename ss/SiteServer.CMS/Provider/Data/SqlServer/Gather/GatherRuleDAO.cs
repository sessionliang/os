using System;
using System.Collections;
using System.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class GatherRuleDAO : DataProviderBase, IGatherRuleDAO
    {
        private const string SQL_SELECT_GATHER_RULE = "SELECT GatherRuleName, PublishmentSystemID, CookieString, GatherUrlIsCollection, GatherUrlCollection, GatherUrlIsSerialize, GatherUrlSerialize, SerializeFrom, SerializeTo, SerializeInterval, SerializeIsOrderByDesc, SerializeIsAddZero, NodeID, Charset, UrlInclude, TitleInclude, ContentExclude, ContentHtmlClearCollection, ContentHtmlClearTagCollection, LastGatherDate, ListAreaStart, ListAreaEnd, ContentChannelStart, ContentChannelEnd, ContentTitleStart, ContentTitleEnd, ContentContentStart, ContentContentEnd, ContentNextPageStart, ContentNextPageEnd, ContentAttributes, ContentAttributesXML, ExtendValues FROM siteserver_GatherRule WHERE GatherRuleName = @GatherRuleName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ALL_GATHER_RULE_BY_PS_ID = "SELECT GatherRuleName, PublishmentSystemID, CookieString, GatherUrlIsCollection, GatherUrlCollection, GatherUrlIsSerialize, GatherUrlSerialize, SerializeFrom, SerializeTo, SerializeInterval, SerializeIsOrderByDesc, SerializeIsAddZero, NodeID, Charset, UrlInclude, TitleInclude, ContentExclude, ContentHtmlClearCollection, ContentHtmlClearTagCollection, LastGatherDate, ListAreaStart, ListAreaEnd, ContentChannelStart, ContentChannelEnd, ContentTitleStart, ContentTitleEnd, ContentContentStart, ContentContentEnd, ContentNextPageStart, ContentNextPageEnd, ContentAttributes, ContentAttributesXML, ExtendValues FROM siteserver_GatherRule WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_GATHER_RULE_NAME_BY_PS_ID = "SELECT GatherRuleName FROM siteserver_GatherRule WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_INSERT_GATHER_RULE = @"
INSERT INTO siteserver_GatherRule 
(GatherRuleName, PublishmentSystemID, CookieString, GatherUrlIsCollection, GatherUrlCollection, GatherUrlIsSerialize, GatherUrlSerialize, SerializeFrom, SerializeTo, SerializeInterval, SerializeIsOrderByDesc, SerializeIsAddZero, NodeID, Charset, UrlInclude, TitleInclude, ContentExclude, ContentHtmlClearCollection, ContentHtmlClearTagCollection, LastGatherDate, ListAreaStart, ListAreaEnd, ContentChannelStart, ContentChannelEnd, ContentTitleStart, ContentTitleEnd, ContentContentStart, ContentContentEnd, ContentNextPageStart, ContentNextPageEnd, ContentAttributes, ContentAttributesXML, ExtendValues) VALUES (@GatherRuleName, @PublishmentSystemID, @CookieString, @GatherUrlIsCollection, @GatherUrlCollection, @GatherUrlIsSerialize, @GatherUrlSerialize, @SerializeFrom, @SerializeTo, @SerializeInterval, @SerializeIsOrderByDesc, @SerializeIsAddZero, @NodeID, @Charset, @UrlInclude, @TitleInclude, @ContentExclude, @ContentHtmlClearCollection, @ContentHtmlClearTagCollection, @LastGatherDate, @ListAreaStart, @ListAreaEnd, @ContentChannelStart, @ContentChannelEnd, @ContentTitleStart, @ContentTitleEnd, @ContentContentStart, @ContentContentEnd, @ContentNextPageStart, @ContentNextPageEnd, @ContentAttributes, @ContentAttributesXML, @ExtendValues)";

        private const string SQL_UPDATE_GATHER_RULE = @"
UPDATE siteserver_GatherRule SET 
CookieString = @CookieString, GatherUrlIsCollection = @GatherUrlIsCollection, GatherUrlCollection = @GatherUrlCollection, GatherUrlIsSerialize = @GatherUrlIsSerialize, GatherUrlSerialize = @GatherUrlSerialize, SerializeFrom = @SerializeFrom, SerializeTo = @SerializeTo, SerializeInterval = @SerializeInterval, SerializeIsOrderByDesc = @SerializeIsOrderByDesc, SerializeIsAddZero = @SerializeIsAddZero, NodeID = @NodeID, Charset = @Charset, UrlInclude = @UrlInclude, TitleInclude = @TitleInclude, ContentExclude = @ContentExclude, ContentHtmlClearCollection = @ContentHtmlClearCollection, ContentHtmlClearTagCollection = @ContentHtmlClearTagCollection, LastGatherDate = @LastGatherDate, ListAreaStart = @ListAreaStart, ListAreaEnd = @ListAreaEnd, ContentChannelStart = @ContentChannelStart, ContentChannelEnd = @ContentChannelEnd, ContentTitleStart = @ContentTitleStart, ContentTitleEnd = @ContentTitleEnd, ContentContentStart = @ContentContentStart, ContentContentEnd = @ContentContentEnd, ContentNextPageStart = @ContentNextPageStart, ContentNextPageEnd = @ContentNextPageEnd, ContentAttributes = @ContentAttributes, ContentAttributesXML = @ContentAttributesXML, ExtendValues = @ExtendValues WHERE GatherRuleName = @GatherRuleName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_UPDATE_LAST_GATHER_DATE = "UPDATE siteserver_GatherRule SET LastGatherDate = @LastGatherDate WHERE GatherRuleName = @GatherRuleName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_DELETE_GATHER_RULE = "DELETE FROM siteserver_GatherRule WHERE GatherRuleName = @GatherRuleName AND PublishmentSystemID = @PublishmentSystemID";

        private const string PARM_GATHER_RULE_NAME = "@GatherRuleName";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";

        private const string PARM_COOKIE_STRING = "@CookieString";
        private const string PARM_GATHER_URL_IS_COLLECTION = "@GatherUrlIsCollection";
        private const string PARM_GATHER_URL_COLLECTION = "@GatherUrlCollection";
        private const string PARM_GATHER_URL_IS_SERIALIZE = "@GatherUrlIsSerialize";
        private const string PARM_GATHER_URL_SERIALIZE = "@GatherUrlSerialize";
        private const string PARM_GATHER_SERIALIZE_FROM = "@SerializeFrom";
        private const string PARM_GATHER_SERIALIZE_TO = "@SerializeTo";
        private const string PARM_GATHER_SERIALIZE_INTERNAL = "@SerializeInterval";
        private const string PARM_GATHER_SERIALIZE_ORDER_BY_DESC = "@SerializeIsOrderByDesc";
        private const string PARM_GATHER_SERIALIZE_IS_ADD_ZERO = "@SerializeIsAddZero";

        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_CHARSET = "@Charset";
        private const string PARM_URL_INCLUDE = "@UrlInclude";
        private const string PARM_TITLE_INCLUDE = "@TitleInclude";
        private const string PARM_CONTENT_EXCLUDE = "@ContentExclude";
        private const string PARM_CONTENT_HTML_CLEAR_COLLECTION = "@ContentHtmlClearCollection";
        private const string PARM_CONTENT_HTML_CLEAR_TAG_COLLECTION = "@ContentHtmlClearTagCollection";
        private const string PARM_LAST_GATHER_DATE = "@LastGatherDate";

        private const string PARM_LIST_AREA_START = "@ListAreaStart";
        private const string PARM_LIST_AREA_END = "@ListAreaEnd";
        private const string PARM_LIST_CONTENT_CHANNEL_START = "@ContentChannelStart";
        private const string PARM_LIST_CONTENT_CHANNEL_END = "@ContentChannelEnd";
        private const string PARM_CONTENT_TITLE_START = "@ContentTitleStart";
        private const string PARM_CONTENT_TITLE_END = "@ContentTitleEnd";
        private const string PARM_CONTENT_CONTENT_START = "@ContentContentStart";
        private const string PARM_CONTENT_CONTENT_END = "@ContentContentEnd";
        private const string PARM_CONTENT_NEXT_PAGE_START = "@ContentNextPageStart";
        private const string PARM_CONTENT_NEXT_PAGE_END = "@ContentNextPageEnd";
        private const string PARM_CONTENT_ATTRIBUTES = "@ContentAttributes";
        private const string PARM_CONTENT_ATTRIBUTES_XML = "@ContentAttributesXML";
        private const string PARM_EXTEND_VALUES = "@ExtendValues";

        public void Insert(GatherRuleInfo gatherRuleInfo)
        {
            IDbDataParameter[] insertParms = new IDbDataParameter[]
            {
                this.GetParameter(GatherRuleDAO.PARM_GATHER_RULE_NAME, EDataType.NVarChar, 50, gatherRuleInfo.GatherRuleName),
                this.GetParameter(GatherRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, gatherRuleInfo.PublishmentSystemID),
                this.GetParameter(GatherRuleDAO.PARM_COOKIE_STRING, EDataType.Text, gatherRuleInfo.CookieString),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_URL_IS_COLLECTION, EDataType.VarChar, 18, gatherRuleInfo.GatherUrlIsCollection.ToString()),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_URL_COLLECTION, EDataType.Text, gatherRuleInfo.GatherUrlCollection),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_URL_IS_SERIALIZE, EDataType.VarChar, 18, gatherRuleInfo.GatherUrlIsSerialize.ToString()),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_URL_SERIALIZE, EDataType.VarChar, 200, gatherRuleInfo.GatherUrlSerialize),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_SERIALIZE_FROM, EDataType.Integer, gatherRuleInfo.SerializeFrom),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_SERIALIZE_TO, EDataType.Integer, gatherRuleInfo.SerializeTo),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_SERIALIZE_INTERNAL, EDataType.Integer, gatherRuleInfo.SerializeInterval),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_SERIALIZE_ORDER_BY_DESC, EDataType.VarChar, 18, gatherRuleInfo.SerializeIsOrderByDesc.ToString()),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_SERIALIZE_IS_ADD_ZERO, EDataType.VarChar, 18, gatherRuleInfo.SerializeIsAddZero.ToString()),
                this.GetParameter(GatherRuleDAO.PARM_NODE_ID, EDataType.Integer, gatherRuleInfo.NodeID),
                this.GetParameter(GatherRuleDAO.PARM_CHARSET, EDataType.VarChar, 50, ECharsetUtils.GetValue(gatherRuleInfo.Charset)),
                this.GetParameter(GatherRuleDAO.PARM_URL_INCLUDE, EDataType.VarChar, 200, gatherRuleInfo.UrlInclude),
                this.GetParameter(GatherRuleDAO.PARM_TITLE_INCLUDE, EDataType.NVarChar, 255, gatherRuleInfo.TitleInclude),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_EXCLUDE, EDataType.NText, gatherRuleInfo.ContentExclude),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_HTML_CLEAR_COLLECTION, EDataType.NVarChar, 255, gatherRuleInfo.ContentHtmlClearCollection),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_HTML_CLEAR_TAG_COLLECTION, EDataType.NVarChar, 255, gatherRuleInfo.ContentHtmlClearTagCollection),
                this.GetParameter(GatherRuleDAO.PARM_LAST_GATHER_DATE, EDataType.DateTime, gatherRuleInfo.LastGatherDate),
                this.GetParameter(GatherRuleDAO.PARM_LIST_AREA_START, EDataType.NText, gatherRuleInfo.ListAreaStart),
                this.GetParameter(GatherRuleDAO.PARM_LIST_AREA_END, EDataType.NText, gatherRuleInfo.ListAreaEnd),
                this.GetParameter(GatherRuleDAO.PARM_LIST_CONTENT_CHANNEL_START, EDataType.NText, gatherRuleInfo.ContentChannelStart),
                this.GetParameter(GatherRuleDAO.PARM_LIST_CONTENT_CHANNEL_END, EDataType.NText, gatherRuleInfo.ContentChannelEnd),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_TITLE_START, EDataType.NText, gatherRuleInfo.ContentTitleStart),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_TITLE_END, EDataType.NText, gatherRuleInfo.ContentTitleEnd),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_CONTENT_START, EDataType.NText, gatherRuleInfo.ContentContentStart),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_CONTENT_END, EDataType.NText, gatherRuleInfo.ContentContentEnd),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_NEXT_PAGE_START, EDataType.NText, gatherRuleInfo.ContentNextPageStart),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_NEXT_PAGE_END, EDataType.NText, gatherRuleInfo.ContentNextPageEnd),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_ATTRIBUTES, EDataType.NText, gatherRuleInfo.ContentAttributes),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_ATTRIBUTES_XML, EDataType.NText, gatherRuleInfo.ContentAttributesXML),
                this.GetParameter(GatherRuleDAO.PARM_EXTEND_VALUES, EDataType.NText, gatherRuleInfo.Additional.ToString())
            };

            this.ExecuteNonQuery(SQL_INSERT_GATHER_RULE, insertParms);
        }

        public void UpdateLastGatherDate(string gatherRuleName, int publishmentSystemID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(GatherRuleDAO.PARM_LAST_GATHER_DATE, EDataType.DateTime, DateTime.Now),
                this.GetParameter(PARM_GATHER_RULE_NAME, EDataType.NVarChar, 50, gatherRuleName),
                this.GetParameter(GatherRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_LAST_GATHER_DATE, parms);
        }

        public void Update(GatherRuleInfo gatherRuleInfo)
        {

            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(GatherRuleDAO.PARM_COOKIE_STRING, EDataType.Text, gatherRuleInfo.CookieString),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_URL_IS_COLLECTION, EDataType.VarChar, 18, gatherRuleInfo.GatherUrlIsCollection.ToString()),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_URL_COLLECTION, EDataType.Text, gatherRuleInfo.GatherUrlCollection),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_URL_IS_SERIALIZE, EDataType.VarChar, 18, gatherRuleInfo.GatherUrlIsSerialize.ToString()),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_URL_SERIALIZE, EDataType.VarChar, 200, gatherRuleInfo.GatherUrlSerialize),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_SERIALIZE_FROM, EDataType.Integer, gatherRuleInfo.SerializeFrom),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_SERIALIZE_TO, EDataType.Integer, gatherRuleInfo.SerializeTo),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_SERIALIZE_INTERNAL, EDataType.Integer, gatherRuleInfo.SerializeInterval),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_SERIALIZE_ORDER_BY_DESC, EDataType.VarChar, 18, gatherRuleInfo.SerializeIsOrderByDesc.ToString()),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_SERIALIZE_IS_ADD_ZERO, EDataType.VarChar, 18, gatherRuleInfo.SerializeIsAddZero.ToString()),
                this.GetParameter(GatherRuleDAO.PARM_NODE_ID, EDataType.Integer, gatherRuleInfo.NodeID),
                this.GetParameter(GatherRuleDAO.PARM_CHARSET, EDataType.VarChar, 50, ECharsetUtils.GetValue(gatherRuleInfo.Charset)),
                this.GetParameter(GatherRuleDAO.PARM_URL_INCLUDE, EDataType.VarChar, 200, gatherRuleInfo.UrlInclude),
                this.GetParameter(GatherRuleDAO.PARM_TITLE_INCLUDE, EDataType.NVarChar, 255, gatherRuleInfo.TitleInclude),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_EXCLUDE, EDataType.NText, gatherRuleInfo.ContentExclude),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_HTML_CLEAR_COLLECTION, EDataType.NVarChar, 255, gatherRuleInfo.ContentHtmlClearCollection),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_HTML_CLEAR_TAG_COLLECTION, EDataType.NVarChar, 255, gatherRuleInfo.ContentHtmlClearTagCollection),
                this.GetParameter(GatherRuleDAO.PARM_LAST_GATHER_DATE, EDataType.DateTime, gatherRuleInfo.LastGatherDate),
                this.GetParameter(GatherRuleDAO.PARM_LIST_AREA_START, EDataType.NText, gatherRuleInfo.ListAreaStart),
                this.GetParameter(GatherRuleDAO.PARM_LIST_AREA_END, EDataType.NText, gatherRuleInfo.ListAreaEnd),
                this.GetParameter(GatherRuleDAO.PARM_LIST_CONTENT_CHANNEL_START, EDataType.NText, gatherRuleInfo.ContentChannelStart),
                this.GetParameter(GatherRuleDAO.PARM_LIST_CONTENT_CHANNEL_END, EDataType.NText, gatherRuleInfo.ContentChannelEnd),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_TITLE_START, EDataType.NText, gatherRuleInfo.ContentTitleStart),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_TITLE_END, EDataType.NText, gatherRuleInfo.ContentTitleEnd),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_CONTENT_START, EDataType.NText, gatherRuleInfo.ContentContentStart),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_CONTENT_END, EDataType.NText, gatherRuleInfo.ContentContentEnd),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_NEXT_PAGE_START, EDataType.NText, gatherRuleInfo.ContentNextPageStart),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_NEXT_PAGE_END, EDataType.NText, gatherRuleInfo.ContentNextPageEnd),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_ATTRIBUTES, EDataType.NText, gatherRuleInfo.ContentAttributes),
                this.GetParameter(GatherRuleDAO.PARM_CONTENT_ATTRIBUTES_XML, EDataType.NText, gatherRuleInfo.ContentAttributesXML),
                this.GetParameter(GatherRuleDAO.PARM_EXTEND_VALUES, EDataType.NText, gatherRuleInfo.Additional.ToString()),
                this.GetParameter(GatherRuleDAO.PARM_GATHER_RULE_NAME, EDataType.NVarChar, 50, gatherRuleInfo.GatherRuleName),
                this.GetParameter(GatherRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, gatherRuleInfo.PublishmentSystemID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_GATHER_RULE, updateParms);
        }


        public void Delete(string gatherRuleName, int publishmentSystemID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GATHER_RULE_NAME, EDataType.NVarChar, 50, gatherRuleName),
                this.GetParameter(GatherRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            this.ExecuteNonQuery(SQL_DELETE_GATHER_RULE, parms);
        }

        public GatherRuleInfo GetGatherRuleInfo(string gatherRuleName, int publishmentSystemID)
        {
            GatherRuleInfo gatherRuleInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GATHER_RULE_NAME, EDataType.NVarChar, 50, gatherRuleName),
                this.GetParameter(GatherRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_GATHER_RULE, parms))
            {
                if (rdr.Read())
                {
                    gatherRuleInfo = new GatherRuleInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9), TranslateUtils.ToBool(rdr.GetValue(10).ToString()), TranslateUtils.ToBool(rdr.GetValue(11).ToString()), rdr.GetInt32(12), ECharsetUtils.GetEnumType(rdr.GetValue(13).ToString()), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString(), rdr.GetValue(17).ToString(), rdr.GetValue(18).ToString(), rdr.GetDateTime(19), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), rdr.GetValue(22).ToString(), rdr.GetValue(23).ToString(), rdr.GetValue(24).ToString(), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString(), rdr.GetValue(28).ToString(), rdr.GetValue(29).ToString(), rdr.GetValue(30).ToString(), rdr.GetValue(31).ToString(), rdr.GetValue(32).ToString());
                }
                rdr.Close();
            }

            return gatherRuleInfo;
        }

        public string GetImportGatherRuleName(int publishmentSystemID, string gatherRuleName)
        {
            string importGatherRuleName;
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
                this.GetParameter(PARM_GATHER_RULE_NAME, EDataType.NVarChar, 50, importGatherRuleName),
                this.GetParameter(GatherRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_GATHER_RULE, parms))
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
                this.GetParameter(GatherRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_GATHER_RULE_BY_PS_ID, parms);
            return enumerable;
        }

        public ArrayList GetGatherRuleInfoArrayList(int publishmentSystemID)
        {
            ArrayList list = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(GatherRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_GATHER_RULE_BY_PS_ID, parms))
            {
                while (rdr.Read())
                {
                    GatherRuleInfo gatherRuleInfo = new GatherRuleInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9), TranslateUtils.ToBool(rdr.GetValue(10).ToString()), TranslateUtils.ToBool(rdr.GetValue(11).ToString()), rdr.GetInt32(12), ECharsetUtils.GetEnumType(rdr.GetValue(13).ToString()), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString(), rdr.GetValue(17).ToString(), rdr.GetValue(18).ToString(), rdr.GetDateTime(19), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), rdr.GetValue(22).ToString(), rdr.GetValue(23).ToString(), rdr.GetValue(24).ToString(), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString(), rdr.GetValue(28).ToString(), rdr.GetValue(29).ToString(), rdr.GetValue(30).ToString(), rdr.GetValue(31).ToString(), rdr.GetValue(32).ToString());
                    list.Add(gatherRuleInfo);
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

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_GATHER_RULE_NAME_BY_PS_ID, parms))
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
            string sql = string.Format("UPDATE siteserver_GatherRule SET IsAutoCreate = 'True' WHERE PublishmentSystemID = {0} AND GatherRuleName in ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(gatherRuleNameCollection));
            this.ExecuteNonQuery(sql);
        }

        public void CloseAuto(int publishmentSystemID, ArrayList gatherRuleNameCollection)
        {
            string sql = string.Format("UPDATE siteserver_GatherRule SET IsAutoCreate = 'False' WHERE PublishmentSystemID = {0} AND GatherRuleName in ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(gatherRuleNameCollection));
            this.ExecuteNonQuery(sql);
        }
    }
}
