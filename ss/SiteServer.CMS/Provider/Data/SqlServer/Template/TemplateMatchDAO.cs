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
    public class TemplateMatchDAO : DataProviderBase, ITemplateMatchDAO
    {
        private const string SQL_SELECT = "SELECT NodeID, PublishmentSystemID, ChannelTemplateID, ContentTemplateID, FilePath, ChannelFilePathRule, ContentFilePathRule FROM siteserver_TemplateMatch WHERE NodeID = @NodeID";

        private const string SQL_SELECT_ALL = "SELECT NodeID, PublishmentSystemID, ChannelTemplateID, ContentTemplateID, FilePath, ChannelFilePathRule, ContentFilePathRule FROM siteserver_TemplateMatch WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_INSERT = "INSERT INTO siteserver_TemplateMatch (NodeID, PublishmentSystemID, ChannelTemplateID, ContentTemplateID, FilePath, ChannelFilePathRule, ContentFilePathRule) VALUES (@NodeID, @PublishmentSystemID, @ChannelTemplateID, @ContentTemplateID, @FilePath, @ChannelFilePathRule, @ContentFilePathRule)";

        private const string SQL_UPDATE = "UPDATE siteserver_TemplateMatch SET ChannelTemplateID = @ChannelTemplateID, ContentTemplateID = @ContentTemplateID, FilePath = @FilePath, ChannelFilePathRule = @ChannelFilePathRule, ContentFilePathRule = @ContentFilePathRule WHERE NodeID = @NodeID";

        private const string SQL_DELETE = "DELETE FROM siteserver_TemplateMatch WHERE NodeID = @NodeID";

        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_CHANNEL_TEMPLATE_ID = "@ChannelTemplateID";
        private const string PARM_CONTENT_TEMPLATE_ID = "@ContentTemplateID";
        private const string PARM_FILEPATH = "@FilePath";
        private const string PARM_CHANNEL_FILEPATH_RULE = "@ChannelFilePathRule";
        private const string PARM_CONTENT_FILEPATH_RULE = "@ContentFilePathRule";

        public void Insert(TemplateMatchInfo info)
        {
            IDbDataParameter[] insertParms = new IDbDataParameter[]
		    {
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, info.NodeID),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID),
			    this.GetParameter(PARM_CHANNEL_TEMPLATE_ID, EDataType.Integer, info.ChannelTemplateID),
                this.GetParameter(PARM_CONTENT_TEMPLATE_ID, EDataType.Integer, info.ContentTemplateID),
                this.GetParameter(PARM_FILEPATH, EDataType.VarChar, 200, info.FilePath),
                this.GetParameter(PARM_CHANNEL_FILEPATH_RULE, EDataType.VarChar, 200, info.ChannelFilePathRule),
                this.GetParameter(PARM_CONTENT_FILEPATH_RULE, EDataType.VarChar, 200, info.ContentFilePathRule)
		    };

            this.ExecuteNonQuery(SQL_INSERT, insertParms);
        }

        public void Update(TemplateMatchInfo info)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
		    {
			    this.GetParameter(PARM_CHANNEL_TEMPLATE_ID, EDataType.Integer, info.ChannelTemplateID),
                this.GetParameter(PARM_CONTENT_TEMPLATE_ID, EDataType.Integer, info.ContentTemplateID),
                this.GetParameter(PARM_FILEPATH, EDataType.VarChar, 200, info.FilePath),
                this.GetParameter(PARM_CHANNEL_FILEPATH_RULE, EDataType.VarChar, 200, info.ChannelFilePathRule),
                this.GetParameter(PARM_CONTENT_FILEPATH_RULE, EDataType.VarChar, 200, info.ContentFilePathRule),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, info.NodeID)
		    };

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);
        }

        public void Delete(int nodeID)
        {

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
        }

        public TemplateMatchInfo GetTemplateMatchInfo(int nodeID)
        {
            TemplateMatchInfo info = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    info = new TemplateMatchInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString());
                }
                rdr.Close();
            }

            return info;
        }

        public bool IsExists(int nodeID)
        {
            bool isExists = false;

            string sqlString = string.Format("SELECT NodeID FROM siteserver_TemplateMatch WHERE NodeID = {0}", nodeID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    isExists = true;
                }
                rdr.Close();
            }

            return isExists;
        }

        public Hashtable GetChannelTemplateIDHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string sqlString = string.Format("SELECT NodeID, ChannelTemplateID FROM siteserver_TemplateMatch WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    hashtable.Add(rdr.GetInt32(0), rdr.GetInt32(1));
                }
                rdr.Close();
            }

            return hashtable;
        }

        public Hashtable GetContentTemplateIDHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string sqlString = string.Format("SELECT NodeID, ContentTemplateID FROM siteserver_TemplateMatch WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    hashtable.Add(rdr.GetInt32(0), rdr.GetInt32(1));
                }
                rdr.Close();
            }

            return hashtable;
        }

        public int GetChannelTemplateID(int nodeID)
        {
            int templateID = 0;

            string sqlString = string.Format("SELECT ChannelTemplateID FROM siteserver_TemplateMatch WHERE NodeID = {0}", nodeID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    templateID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return templateID;
        }

        public int GetContentTemplateID(int nodeID)
        {
            int templateID = 0;

            string sqlString = string.Format("SELECT ContentTemplateID FROM siteserver_TemplateMatch WHERE NodeID = {0}", nodeID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    templateID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return templateID;
        }

        public string GetFilePath(int nodeID)
        {
            string filePath = string.Empty;

            string sqlString = string.Format("SELECT FilePath FROM siteserver_TemplateMatch WHERE NodeID = {0}", nodeID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    filePath = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return filePath;
        }

        public string GetChannelFilePathRule(int nodeID)
        {
            string filePathRule = string.Empty;

            string sqlString = string.Format("SELECT ChannelFilePathRule FROM siteserver_TemplateMatch WHERE NodeID = {0}", nodeID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    filePathRule = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return filePathRule;
        }

        public string GetContentFilePathRule(int nodeID)
        {
            string filePathRule = string.Empty;

            string sqlString = string.Format("SELECT ContentFilePathRule FROM siteserver_TemplateMatch WHERE NodeID = {0}", nodeID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    filePathRule = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return filePathRule;
        }

        public ArrayList GetAllFilePathByPublishmentSystemID(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT FilePath FROM siteserver_TemplateMatch WHERE FilePath <> '' AND PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }
            return arraylist;
        }
    }
}
