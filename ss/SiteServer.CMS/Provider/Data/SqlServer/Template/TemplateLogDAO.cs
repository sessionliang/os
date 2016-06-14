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
using System.Collections.Generic;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class TemplateLogDAO : DataProviderBase, SiteServer.CMS.Core.ITemplateLogDAO
    {
        private const string PARM_ID = "@ID";
        private const string PARM_TEMPLATE_ID = "@TemplateID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_ADD_USER_NAME = "@AddUserName";
        private const string PARM_CONTENT_LENGTH = "@ContentLength";
        private const string PARM_TEMPLATE_CONTENT = "@TemplateContent";

        public void Insert(TemplateLogInfo logInfo)
        {
            string sqlString = "INSERT INTO siteserver_TemplateLog(TemplateID, PublishmentSystemID, AddDate, AddUserName, ContentLength, TemplateContent) VALUES (@TemplateID, @PublishmentSystemID, @AddDate, @AddUserName, @ContentLength, @TemplateContent)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_TemplateLog(ID, TemplateID, PublishmentSystemID, AddDate, AddUserName, ContentLength, TemplateContent) VALUES (siteserver_TemplateLog_SEQ.NEXTVAL, @TemplateID, @PublishmentSystemID, @AddDate, @AddUserName, @ContentLength, @TemplateContent)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(TemplateLogDAO.PARM_TEMPLATE_ID, EDataType.Integer, logInfo.TemplateID),
                this.GetParameter(TemplateLogDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, logInfo.PublishmentSystemID),
                this.GetParameter(TemplateLogDAO.PARM_ADD_DATE, EDataType.DateTime, logInfo.AddDate),
                this.GetParameter(TemplateLogDAO.PARM_ADD_USER_NAME, EDataType.NVarChar, 255, logInfo.AddUserName),
                this.GetParameter(TemplateLogDAO.PARM_CONTENT_LENGTH, EDataType.Integer, logInfo.ContentLength),
				this.GetParameter(TemplateLogDAO.PARM_TEMPLATE_CONTENT, EDataType.NText, logInfo.TemplateContent)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public string GetSelectCommend(int publishmentSystemID, int templateID)
        {
            return string.Format("SELECT ID, TemplateID, PublishmentSystemID, AddDate, AddUserName, ContentLength, TemplateContent FROM siteserver_TemplateLog WHERE PublishmentSystemID = {0} AND TemplateID = {1}", publishmentSystemID, templateID);
        }

        public string GetTemplateContent(int logID)
        {
            string templateContent = string.Empty;

            string sqlString = string.Format("SELECT TemplateContent FROM siteserver_TemplateLog WHERE ID = {0}", logID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    templateContent = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return templateContent;
        }

        public Dictionary<int, string> GetLogIDWithNameDictionary(int publishmentSystemID, int templateID)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            string sqlString = string.Format("SELECT ID, AddDate, AddUserName, ContentLength FROM siteserver_TemplateLog WHERE TemplateID = {0}", templateID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int id = rdr.GetInt32(0);
                    DateTime addDate = rdr.GetDateTime(1);
                    string addUserName = rdr.GetValue(2).ToString();
                    int contentLength = rdr.GetInt32(3);

                    string name = string.Format("修订时间：{0}，修订人：{1}，字符数：{2}", DateUtils.GetDateAndTimeString(addDate), addUserName, contentLength);

                    dictionary.Add(id, name);
                }
                rdr.Close();
            }

            return dictionary;
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList != null || idArrayList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM siteserver_TemplateLog WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));

                this.ExecuteNonQuery(sqlString);
            }
        }
    }
}
