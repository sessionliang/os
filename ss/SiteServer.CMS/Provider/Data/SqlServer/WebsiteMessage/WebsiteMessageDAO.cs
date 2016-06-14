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
    public class WebsiteMessageDAO : DataProviderBase, IWebsiteMessageDAO
    {
        private const string SQL_UPDATE_INPUT = "UPDATE siteserver_WebsiteMessage SET WebsiteMessageName = @WebsiteMessageName, IsChecked = @IsChecked, IsReply = @IsReply, IsTemplate = @IsTemplate, StyleTemplate = @StyleTemplate, ScriptTemplate = @ScriptTemplate, ContentTemplate = @ContentTemplate, IsTemplateList = @IsTemplateList, StyleTemplateList = @StyleTemplateList, ScriptTemplateList = @ScriptTemplateList, ContentTemplateList = @ContentTemplateList, IsTemplateDetail = @IsTemplateDetail, StyleTemplateDetail = @StyleTemplateDetail, ScriptTemplateDetail = @ScriptTemplateDetail, ContentTemplateDetail = @ContentTemplateDetail, SettingsXML = @SettingsXML WHERE WebsiteMessageID = @WebsiteMessageID";

        private const string SQL_DELETE_INPUT = "DELETE FROM siteserver_WebsiteMessage WHERE WebsiteMessageID = @WebsiteMessageID";

        private const string SQL_SELECT_INPUT_BY_WHERE = "SELECT WebsiteMessageID, WebsiteMessageName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, IsTemplateList, StyleTemplateList, ScriptTemplateList, ContentTemplateList, IsTemplateDetail, StyleTemplateDetail, ScriptTemplateDetail, ContentTemplateDetail, SettingsXML FROM siteserver_WebsiteMessage WHERE WebsiteMessageName = @WebsiteMessageName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_INPUT_ID_BY_WHERE = "SELECT WebsiteMessageID FROM siteserver_WebsiteMessage WHERE WebsiteMessageName = @WebsiteMessageName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_INPUT_BY_INPUT_ID = "SELECT WebsiteMessageID, WebsiteMessageName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, IsTemplateList, StyleTemplateList, ScriptTemplateList, ContentTemplateList, IsTemplateDetail, StyleTemplateDetail, ScriptTemplateDetail, ContentTemplateDetail, SettingsXML FROM siteserver_WebsiteMessage WHERE WebsiteMessageID = @WebsiteMessageID";

        private const string SQL_SELECT_INPUT_BY_ADDDATE = "SELECT TOP 1 WebsiteMessageID, WebsiteMessageName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, IsTemplateList, StyleTemplateList, ScriptTemplateList, ContentTemplateList, IsTemplateDetail, StyleTemplateDetail, ScriptTemplateDetail, ContentTemplateDetail, SettingsXML FROM siteserver_WebsiteMessage WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis DESC, AddDate DESC";

        private const string SQL_SELECT_ALL_INPUT = "SELECT WebsiteMessageID, WebsiteMessageName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, IsTemplateList, StyleTemplateList, ScriptTemplateList, ContentTemplateList, IsTemplateDetail, StyleTemplateDetail, ScriptTemplateDetail, ContentTemplateDetail, SettingsXML FROM siteserver_WebsiteMessage WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis DESC, AddDate DESC";

        private const string SQL_SELECT_INPUT_ID = "SELECT WebsiteMessageID FROM siteserver_WebsiteMessage WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis DESC, AddDate DESC";

        private const string SQL_SELECT_INPUT_NAME = "SELECT WebsiteMessageName FROM siteserver_WebsiteMessage WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis DESC, AddDate DESC";

        private const string PARM_INPUT_ID = "@WebsiteMessageID";
        private const string PARM_INPUT_NAME = "@WebsiteMessageName";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_IS_CHECKED = "@IsChecked";
        private const string PARM_IS_REPLY = "@IsReply";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_IS_TEMPLATE = "@IsTemplate";
        private const string PARM_STYLE_TEMPLATE = "@StyleTemplate";
        private const string PARM_SCRIPT_TEMPLATE = "@ScriptTemplate";
        private const string PARM_CONTENT_TEMPLATE = "@ContentTemplate";

        #region List
        private const string PARM_IS_TEMPLATE_LIST = "@IsTemplateList";
        private const string PARM_STYLE_TEMPLATE_LIST = "@StyleTemplateList";
        private const string PARM_SCRIPT_TEMPLATE_LIST = "@ScriptTemplateList";
        private const string PARM_CONTENT_TEMPLATE_LIST = "@ContentTemplateList";
        #endregion

        #region Detail
        private const string PARM_IS_TEMPLATE_DETAIL = "@IsTemplateDetail";
        private const string PARM_STYLE_TEMPLATE_DETAIL = "@StyleTemplateDetail";
        private const string PARM_SCRIPT_TEMPLATE_DETAIL = "@ScriptTemplateDetail";
        private const string PARM_CONTENT_TEMPLATE_DETAIL = "@ContentTemplateDetail";
        #endregion

        private const string PARM_SETTINGS_XML = "@SettingsXML";

        public int Insert(WebsiteMessageInfo websiteMessageInfo)
        {
            int websiteMessageID = 0;

            string sqlString = "INSERT INTO siteserver_WebsiteMessage (WebsiteMessageName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, IsTemplateList, StyleTemplateList, ScriptTemplateList, ContentTemplateList, IsTemplateDetail, StyleTemplateDetail, ScriptTemplateDetail, ContentTemplateDetail, SettingsXML) VALUES (@WebsiteMessageName, @PublishmentSystemID, @AddDate, @IsChecked, @IsReply, @Taxis, @IsTemplate, @StyleTemplate, @ScriptTemplate, @ContentTemplate, @IsTemplateList, @StyleTemplateList, @ScriptTemplateList, @ContentTemplateList, @IsTemplateDetail, @StyleTemplateDetail, @ScriptTemplateDetail, @ContentTemplateDetail, @SettingsXML)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_WebsiteMessage (WebsiteMessageID, WebsiteMessageName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, IsTemplateList, StyleTemplateList, ScriptTemplateList, ContentTemplateList, IsTemplateDetail, StyleTemplateDetail, ScriptTemplateDetail, ContentTemplateDetail, SettingsXML) VALUES (siteserver_WebsiteMessage_SEQ.NEXTVAL, @WebsiteMessageName, @PublishmentSystemID, @AddDate, @IsChecked, @IsReply, @Taxis, @IsTemplate, @StyleTemplate, @ScriptTemplate, @ContentTemplate, @IsTemplateList, @StyleTemplateList, @ScriptTemplateList, @ContentTemplateList, @IsTemplateDetail, @StyleTemplateDetail, @ScriptTemplateDetail, @ContentTemplateDetail, @SettingsXML)";
            }

            int taxis = this.GetMaxTaxis(websiteMessageInfo.PublishmentSystemID) + 1;
            IDbDataParameter[] insertParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_INPUT_NAME, EDataType.NVarChar, 50, websiteMessageInfo.WebsiteMessageName),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, websiteMessageInfo.PublishmentSystemID),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, websiteMessageInfo.AddDate),
                this.GetParameter(PARM_IS_CHECKED, EDataType.VarChar, 18, websiteMessageInfo.IsChecked.ToString()),
                this.GetParameter(PARM_IS_REPLY, EDataType.VarChar, 18, websiteMessageInfo.IsReply.ToString()),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis),
                this.GetParameter(PARM_IS_TEMPLATE, EDataType.VarChar, 18, websiteMessageInfo.IsTemplate.ToString()),
                this.GetParameter(PARM_STYLE_TEMPLATE, EDataType.NText, websiteMessageInfo.StyleTemplate),
                this.GetParameter(PARM_SCRIPT_TEMPLATE, EDataType.NText, websiteMessageInfo.ScriptTemplate),
                this.GetParameter(PARM_CONTENT_TEMPLATE, EDataType.NText, websiteMessageInfo.ContentTemplate),
                this.GetParameter(PARM_IS_TEMPLATE_LIST, EDataType.VarChar, 18, websiteMessageInfo.IsTemplateList.ToString()),
                this.GetParameter(PARM_STYLE_TEMPLATE_LIST, EDataType.NText, websiteMessageInfo.StyleTemplateList),
                this.GetParameter(PARM_SCRIPT_TEMPLATE_LIST, EDataType.NText, websiteMessageInfo.ScriptTemplateList),
                this.GetParameter(PARM_CONTENT_TEMPLATE_LIST, EDataType.NText, websiteMessageInfo.ContentTemplateList),
                this.GetParameter(PARM_IS_TEMPLATE_DETAIL, EDataType.VarChar, 18, websiteMessageInfo.IsTemplateDetail.ToString()),
                this.GetParameter(PARM_STYLE_TEMPLATE_DETAIL, EDataType.NText, websiteMessageInfo.StyleTemplateDetail),
                this.GetParameter(PARM_SCRIPT_TEMPLATE_DETAIL, EDataType.NText, websiteMessageInfo.ScriptTemplateDetail),
                this.GetParameter(PARM_CONTENT_TEMPLATE_DETAIL, EDataType.NText, websiteMessageInfo.ContentTemplateDetail),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, websiteMessageInfo.Additional.ToString())
            };

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, insertParms);
                        websiteMessageID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "siteserver_WebsiteMessage");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return websiteMessageID;
        }

        public void Update(WebsiteMessageInfo websiteMessageInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_INPUT_NAME, EDataType.NVarChar, 50, websiteMessageInfo.WebsiteMessageName),
                this.GetParameter(PARM_IS_CHECKED, EDataType.VarChar, 18, websiteMessageInfo.IsChecked.ToString()),
                this.GetParameter(PARM_IS_REPLY, EDataType.VarChar, 18, websiteMessageInfo.IsReply.ToString()),
                this.GetParameter(PARM_IS_TEMPLATE, EDataType.VarChar, 18, websiteMessageInfo.IsTemplate.ToString()),
                this.GetParameter(PARM_STYLE_TEMPLATE, EDataType.NText, websiteMessageInfo.StyleTemplate),
                this.GetParameter(PARM_SCRIPT_TEMPLATE, EDataType.NText, websiteMessageInfo.ScriptTemplate),
                this.GetParameter(PARM_CONTENT_TEMPLATE, EDataType.NText, websiteMessageInfo.ContentTemplate),
                this.GetParameter(PARM_IS_TEMPLATE_LIST, EDataType.VarChar, 18, websiteMessageInfo.IsTemplateList.ToString()),
                this.GetParameter(PARM_STYLE_TEMPLATE_LIST, EDataType.NText, websiteMessageInfo.StyleTemplateList),
                this.GetParameter(PARM_SCRIPT_TEMPLATE_LIST, EDataType.NText, websiteMessageInfo.ScriptTemplateList),
                this.GetParameter(PARM_CONTENT_TEMPLATE_LIST, EDataType.NText, websiteMessageInfo.ContentTemplateList),
                this.GetParameter(PARM_IS_TEMPLATE_DETAIL, EDataType.VarChar, 18, websiteMessageInfo.IsTemplateDetail.ToString()),
                this.GetParameter(PARM_STYLE_TEMPLATE_DETAIL, EDataType.NText, websiteMessageInfo.StyleTemplateDetail),
                this.GetParameter(PARM_SCRIPT_TEMPLATE_DETAIL, EDataType.NText, websiteMessageInfo.ScriptTemplateDetail),
                this.GetParameter(PARM_CONTENT_TEMPLATE_DETAIL, EDataType.NText, websiteMessageInfo.ContentTemplateDetail),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, websiteMessageInfo.Additional.ToString()),
                this.GetParameter(PARM_INPUT_ID, EDataType.Integer, websiteMessageInfo.WebsiteMessageID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_INPUT, updateParms);
        }

        public void Delete(int websiteMessageID)
        {
            IDbDataParameter[] deleteParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_INPUT_ID, EDataType.Integer, websiteMessageID)
            };

            this.ExecuteNonQuery(SQL_DELETE_INPUT, deleteParms);
        }

        public WebsiteMessageInfo GetWebsiteMessageInfo(string websiteMessageName, int publishmentSystemID)
        {
            WebsiteMessageInfo websiteMessageInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_INPUT_NAME, EDataType.NVarChar, 50, websiteMessageName),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_BY_WHERE, parms))
            {
                if (rdr.Read())
                {
                    websiteMessageInfo = new WebsiteMessageInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetDateTime(3), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), TranslateUtils.ToBool(rdr.GetValue(11).ToString()), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString(), TranslateUtils.ToBool(rdr.GetValue(15).ToString()), rdr.GetValue(16).ToString(), rdr.GetValue(17).ToString(), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString());
                }
                rdr.Close();
            }

            return websiteMessageInfo;
        }

        public WebsiteMessageInfo GetWebsiteMessageInfoAsPossible(int websiteMessageID, int publishmentSystemID)
        {
            WebsiteMessageInfo websiteMessageInfo = null;

            websiteMessageInfo = this.GetWebsiteMessageInfo(websiteMessageID);
            if (websiteMessageInfo == null)
            {
                websiteMessageInfo = this.GetLastAddWebsiteMessageInfo(publishmentSystemID);
            }

            return websiteMessageInfo;
        }

        public int GetWebsiteMessageIDAsPossible(string websiteMessageName, int publishmentSystemID)
        {
            int websiteMessageID = 0;

            if (!string.IsNullOrEmpty(websiteMessageName))
            {
                IDbDataParameter[] selectParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_INPUT_NAME, EDataType.NVarChar, 50, websiteMessageName),
                    this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_ID_BY_WHERE, selectParms))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        websiteMessageID = rdr.GetInt32(0);
                    }
                }
            }

            if (websiteMessageID == 0)
            {
                WebsiteMessageInfo websiteMessageInfo = this.GetLastAddWebsiteMessageInfo(publishmentSystemID);
                if (websiteMessageInfo != null)
                {
                    websiteMessageID = websiteMessageInfo.WebsiteMessageID;
                }
            }

            return websiteMessageID;
        }

        public WebsiteMessageInfo GetWebsiteMessageInfo(int websiteMessageID)
        {
            WebsiteMessageInfo websiteMessageInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_INPUT_ID, EDataType.Integer, websiteMessageID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_BY_INPUT_ID, parms))
            {
                if (rdr.Read())
                {
                    websiteMessageInfo = new WebsiteMessageInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetDateTime(3), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), TranslateUtils.ToBool(rdr.GetValue(11).ToString()), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString(), TranslateUtils.ToBool(rdr.GetValue(15).ToString()), rdr.GetValue(16).ToString(), rdr.GetValue(17).ToString(), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString());
                }
                rdr.Close();
            }

            return websiteMessageInfo;
        }

        public WebsiteMessageInfo GetLastAddWebsiteMessageInfo(int publishmentSystemID)
        {
            WebsiteMessageInfo websiteMessageInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_BY_ADDDATE, parms))
            {
                if (rdr.Read())
                {
                    websiteMessageInfo = new WebsiteMessageInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetDateTime(3), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), TranslateUtils.ToBool(rdr.GetValue(11).ToString()), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString(), TranslateUtils.ToBool(rdr.GetValue(15).ToString()), rdr.GetValue(16).ToString(), rdr.GetValue(17).ToString(), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString());
                }
                rdr.Close();
            }

            return websiteMessageInfo;
        }

        public bool IsExists(string websiteMessageName, int publishmentSystemID)
        {
            bool exists = false;

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_INPUT_NAME, EDataType.NVarChar, 50, websiteMessageName),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_BY_WHERE, selectParms))
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
            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_INPUT, selectParms);
            return enumerable;
        }

        public ArrayList GetWebsiteMessageIDArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_ID, selectParms))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetWebsiteMessageNameArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_NAME, selectParms))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return arraylist;
        }

        public string GetImportWebsiteMessageName(string websiteMessageName, int publishmentSystemID)
        {
            string importWebsiteMessageName = "";
            if (websiteMessageName.IndexOf("_") != -1)
            {
                int websiteMessageName_Count = 0;
                string lastWebsiteMessageName = websiteMessageName.Substring(websiteMessageName.LastIndexOf("_") + 1);
                string firstWebsiteMessageName = websiteMessageName.Substring(0, websiteMessageName.Length - lastWebsiteMessageName.Length);
                try
                {
                    websiteMessageName_Count = int.Parse(lastWebsiteMessageName);
                }
                catch { }
                websiteMessageName_Count++;
                importWebsiteMessageName = firstWebsiteMessageName + websiteMessageName_Count;
            }
            else
            {
                importWebsiteMessageName = websiteMessageName + "_1";
            }

            WebsiteMessageInfo websiteMessageInfo = this.GetWebsiteMessageInfo(websiteMessageName, publishmentSystemID);
            if (websiteMessageInfo != null)
            {
                importWebsiteMessageName = GetImportWebsiteMessageName(importWebsiteMessageName, publishmentSystemID);
            }

            return importWebsiteMessageName;
        }

        public bool UpdateTaxisToUp(int publishmentSystemID, int websiteMessageID)
        {
            string sqlString = string.Format("SELECT TOP 1 WebsiteMessageID, Taxis FROM siteserver_WebsiteMessage WHERE ((Taxis > (SELECT Taxis FROM siteserver_WebsiteMessage WHERE WebsiteMessageID = {0})) AND PublishmentSystemID ={1}) ORDER BY Taxis", websiteMessageID, publishmentSystemID);
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = Convert.ToInt32(rdr[0]);
                    higherTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(websiteMessageID);

            if (higherID != 0)
            {
                SetTaxis(websiteMessageID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int publishmentSystemID, int websiteMessageID)
        {
            string sqlString = string.Format("SELECT TOP 1 WebsiteMessageID, Taxis FROM siteserver_WebsiteMessage WHERE ((Taxis < (SELECT Taxis FROM siteserver_WebsiteMessage WHERE (WebsiteMessageID = {0}))) AND PublishmentSystemID = {1}) ORDER BY Taxis DESC", websiteMessageID, publishmentSystemID);
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = Convert.ToInt32(rdr[0]);
                    lowerTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(websiteMessageID);

            if (lowerID != 0)
            {
                SetTaxis(websiteMessageID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM siteserver_WebsiteMessage WHERE PublishmentSystemID = {0}", publishmentSystemID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int websiteMessageID)
        {
            string sqlString = string.Format("SELECT Taxis FROM siteserver_WebsiteMessage WHERE (WebsiteMessageID = {0})", websiteMessageID);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    taxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int websiteMessageID, int taxis)
        {
            string sqlString = string.Format("UPDATE siteserver_WebsiteMessage SET Taxis = {0} WHERE WebsiteMessageID = {1}", taxis, websiteMessageID);
            this.ExecuteNonQuery(sqlString);
        }

        public int SetDefaultWebsiteMessageInfo(int publishmentSystemID)
        {
            int id = 0;
            WebsiteMessageInfo wmInfo = new WebsiteMessageInfo();
            wmInfo.WebsiteMessageName = "Default";
            wmInfo.PublishmentSystemID = publishmentSystemID;
            if (!this.IsExists("Default", publishmentSystemID))
                id = this.Insert(wmInfo);
            return id;
        }
    }
}