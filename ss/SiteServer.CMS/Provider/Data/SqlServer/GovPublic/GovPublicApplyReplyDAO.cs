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
    public class GovPublicApplyReplyDAO : DataProviderBase, IGovPublicApplyReplyDAO
    {
        private const string SQL_SELECT = "SELECT ReplyID, PublishmentSystemID, ApplyID, Reply, FileUrl, DepartmentID, UserName, AddDate FROM siteserver_GovPublicApplyReply WHERE ReplyID = @ReplyID";

        private const string SQL_SELECT_BY_APPLY_ID = "SELECT ReplyID, PublishmentSystemID, ApplyID, Reply, FileUrl, DepartmentID, UserName, AddDate FROM siteserver_GovPublicApplyReply WHERE ApplyID = @ApplyID";

        private const string PARM_REPLY_ID = "@ReplyID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_APPLY_ID = "@ApplyID";
        private const string PARM_REPLY = "@Reply";
        private const string PARM_FILE_URL = "@FileUrl";
        private const string PARM_DEPARTMENT_ID = "@DepartmentID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_ADD_DATE = "@AddDate";

        public void Insert(GovPublicApplyReplyInfo replyInfo)
        {
            string sqlString = "INSERT INTO siteserver_GovPublicApplyReply(PublishmentSystemID, ApplyID, Reply, FileUrl, DepartmentID, UserName, AddDate) VALUES (@PublishmentSystemID, @ApplyID, @Reply, @FileUrl, @DepartmentID, @UserName, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_GovPublicApplyReply(ReplyID, PublishmentSystemID, ApplyID, Reply, FileUrl, DepartmentID, UserName, AddDate) VALUES (siteserver_GovPublicApplyReply_SEQ.NEXTVAL, @PublishmentSystemID, @ApplyID, @Reply, @FileUrl, @DepartmentID, @UserName, @AddDate)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(GovPublicApplyReplyDAO.PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, replyInfo.PublishmentSystemID),
                this.GetParameter(GovPublicApplyReplyDAO.PARM_APPLY_ID, EDataType.Integer, replyInfo.ApplyID),
                this.GetParameter(GovPublicApplyReplyDAO.PARM_REPLY, EDataType.NText, replyInfo.Reply),
                this.GetParameter(GovPublicApplyReplyDAO.PARM_FILE_URL, EDataType.NVarChar, 255, replyInfo.FileUrl),
                this.GetParameter(GovPublicApplyReplyDAO.PARM_DEPARTMENT_ID, EDataType.Integer, replyInfo.DepartmentID),
				this.GetParameter(GovPublicApplyReplyDAO.PARM_USER_NAME, EDataType.VarChar, 50, replyInfo.UserName),
                this.GetParameter(GovPublicApplyReplyDAO.PARM_ADD_DATE, EDataType.DateTime, replyInfo.AddDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(int replyID)
        {
            string sqlString = string.Format("DELETE FROM siteserver_GovPublicApplyReply WHERE ReplyID = {0}", replyID);
            this.ExecuteNonQuery(sqlString);
        }

        public void DeleteByApplyID(int applyID)
        {
            string sqlString = string.Format("DELETE FROM siteserver_GovPublicApplyReply WHERE ApplyID = {0}", applyID);
            this.ExecuteNonQuery(sqlString);
        }

        public GovPublicApplyReplyInfo GetReplyInfo(int replayID)
        {
            GovPublicApplyReplyInfo replyInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_REPLY_ID, EDataType.Integer, replayID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    replyInfo = new GovPublicApplyReplyInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetDateTime(7));
                }
                rdr.Close();
            }

            return replyInfo;
        }

        public GovPublicApplyReplyInfo GetReplyInfoByApplyID(int applyID)
        {
            GovPublicApplyReplyInfo replyInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_APPLY_ID, EDataType.Integer, applyID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_APPLY_ID, parms))
            {
                if (rdr.Read())
                {
                    replyInfo = new GovPublicApplyReplyInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetDateTime(7));
                }
                rdr.Close();
            }

            return replyInfo;
        }
    }
}
