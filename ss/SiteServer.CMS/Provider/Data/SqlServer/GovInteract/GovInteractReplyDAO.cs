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
    public class GovInteractReplyDAO : DataProviderBase, IGovInteractReplyDAO
    {
        private const string SQL_SELECT = "SELECT ReplyID, PublishmentSystemID, NodeID, ContentID, Reply, FileUrl, DepartmentID, UserName, AddDate FROM siteserver_GovInteractReply WHERE ReplyID = @ReplyID";

        private const string SQL_SELECT_BY_CONTENT_ID = "SELECT ReplyID, PublishmentSystemID, NodeID, ContentID, Reply, FileUrl, DepartmentID, UserName, AddDate FROM siteserver_GovInteractReply WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID";

        private const string PARM_REPLY_ID = "@ReplyID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_CONTENT_ID = "@ContentID";
        private const string PARM_REPLY = "@Reply";
        private const string PARM_FILE_URL = "@FileUrl";
        private const string PARM_DEPARTMENT_ID = "@DepartmentID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_ADD_DATE = "@AddDate";

        public void Insert(GovInteractReplyInfo replyInfo)
        {
            string sqlString = "INSERT INTO siteserver_GovInteractReply(PublishmentSystemID, NodeID, ContentID, Reply, FileUrl, DepartmentID, UserName, AddDate) VALUES (@PublishmentSystemID, @NodeID, @ContentID, @Reply, @FileUrl, @DepartmentID, @UserName, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_GovInteractReply(ReplyID, PublishmentSystemID, NodeID, ContentID, Reply, FileUrl, DepartmentID, UserName, AddDate) VALUES (siteserver_GovInteractReply_SEQ.NEXTVAL, @PublishmentSystemID, @NodeID, @ContentID, @Reply, @FileUrl, @DepartmentID, @UserName, @AddDate)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(GovInteractReplyDAO.PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, replyInfo.PublishmentSystemID),
                this.GetParameter(GovInteractReplyDAO.PARM_NODE_ID, EDataType.Integer, replyInfo.NodeID),
                this.GetParameter(GovInteractReplyDAO.PARM_CONTENT_ID, EDataType.Integer, replyInfo.ContentID),
                this.GetParameter(GovInteractReplyDAO.PARM_REPLY, EDataType.NText, replyInfo.Reply),
                this.GetParameter(GovInteractReplyDAO.PARM_FILE_URL, EDataType.NVarChar, 255, replyInfo.FileUrl),
                this.GetParameter(GovInteractReplyDAO.PARM_DEPARTMENT_ID, EDataType.Integer, replyInfo.DepartmentID),
				this.GetParameter(GovInteractReplyDAO.PARM_USER_NAME, EDataType.VarChar, 50, replyInfo.UserName),
                this.GetParameter(GovInteractReplyDAO.PARM_ADD_DATE, EDataType.DateTime, replyInfo.AddDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(int replyID)
        {
            string sqlString = string.Format("DELETE FROM siteserver_GovInteractReply WHERE ReplyID = {0}", replyID);
            this.ExecuteNonQuery(sqlString);
        }

        public void DeleteByContentID(int publishmentSystemID, int contentID)
        {
            string sqlString = string.Format("DELETE FROM siteserver_GovInteractReply WHERE PublishmentSystemID = {0} AND ContentID = {1}", publishmentSystemID, contentID);
            this.ExecuteNonQuery(sqlString);
        }

        public GovInteractReplyInfo GetReplyInfo(int replayID)
        {
            GovInteractReplyInfo replyInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_REPLY_ID, EDataType.Integer, replayID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    replyInfo = new GovInteractReplyInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetInt32(6), rdr.GetValue(7).ToString(), rdr.GetDateTime(8));
                }
                rdr.Close();
            }

            return replyInfo;
        }

        public GovInteractReplyInfo GetReplyInfoByContentID(int publishmentSystemID, int contentID)
        {
            GovInteractReplyInfo replyInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_CONTENT_ID, parms))
            {
                if (rdr.Read())
                {
                    replyInfo = new GovInteractReplyInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetInt32(6), rdr.GetValue(7).ToString(), rdr.GetDateTime(8));
                }
                rdr.Close();
            }

            return replyInfo;
        }
    }
}
