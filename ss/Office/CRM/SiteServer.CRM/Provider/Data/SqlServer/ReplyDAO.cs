using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CRM.Model;
using SiteServer.CRM.Core;
using System.Text;
using BaiRong.Model;

namespace SiteServer.CRM.Provider.Data.SqlServer
{
    public class ReplyDAO : DataProviderBase, IReplyDAO
    {
        private const string SQL_SELECT = "SELECT ReplyID, ApplyID, Reply, FileUrl, DepartmentID, UserName, AddDate FROM pms_Reply WHERE ReplyID = @ReplyID";

        private const string SQL_SELECT_BY_APPLY_ID = "SELECT ReplyID, ApplyID, Reply, FileUrl, DepartmentID, UserName, AddDate FROM pms_Reply WHERE ApplyID = @ApplyID";

        private const string PARM_REPLY_ID = "@ReplyID";
        private const string PARM_APPLY_ID = "@ApplyID";
        private const string PARM_REPLY = "@Reply";
        private const string PARM_FILE_URL = "@FileUrl";
        private const string PARM_DEPARTMENT_ID = "@DepartmentID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_ADD_DATE = "@AddDate";

        public void Insert(ReplyInfo replyInfo)
        {
            string sqlString = "INSERT INTO pms_Reply(ApplyID, Reply, FileUrl, DepartmentID, UserName, AddDate) VALUES (@ApplyID, @Reply, @FileUrl, @DepartmentID, @UserName, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO pms_Reply(ReplyID, ApplyID, Reply, FileUrl, DepartmentID, UserName, AddDate) VALUES (pms_Reply_SEQ.NEXTVAL, @ApplyID, @Reply, @FileUrl, @DepartmentID, @UserName, @AddDate)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(ReplyDAO.PARM_APPLY_ID, EDataType.Integer, replyInfo.ApplyID),
                this.GetParameter(ReplyDAO.PARM_REPLY, EDataType.NText, replyInfo.Reply),
                this.GetParameter(ReplyDAO.PARM_FILE_URL, EDataType.NVarChar, 255, replyInfo.FileUrl),
                this.GetParameter(ReplyDAO.PARM_DEPARTMENT_ID, EDataType.Integer, replyInfo.DepartmentID),
				this.GetParameter(ReplyDAO.PARM_USER_NAME, EDataType.VarChar, 50, replyInfo.UserName),
                this.GetParameter(ReplyDAO.PARM_ADD_DATE, EDataType.DateTime, replyInfo.AddDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(int replyID)
        {
            string sqlString = string.Format("DELETE FROM pms_Reply WHERE ReplyID = {0}", replyID);
            this.ExecuteNonQuery(sqlString);
        }

        public void DeleteByApplyID(int applyID)
        {
            string sqlString = string.Format("DELETE FROM pms_Reply WHERE ApplyID = {0}", applyID);
            this.ExecuteNonQuery(sqlString);
        }

        public ReplyInfo GetReplyInfo(int replayID)
        {
            ReplyInfo replyInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_REPLY_ID, EDataType.Integer, replayID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    replyInfo = new ReplyInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetDateTime(6));
                }
                rdr.Close();
            }

            return replyInfo;
        }

        public ReplyInfo GetReplyInfoByApplyID(int applyID)
        {
            ReplyInfo replyInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_APPLY_ID, EDataType.Integer, applyID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_APPLY_ID, parms))
            {
                if (rdr.Read())
                {
                    replyInfo = new ReplyInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetDateTime(6));
                }
                rdr.Close();
            }

            return replyInfo;
        }
    }
}
