using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model.MLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class MlibDAO : DataProviderBase, IMlibDAO
    {
        public int Insert(SubmissionInfo submissionInfo)
        {
            int returnId = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [ml_Submission](");
            strSql.Append("[AddUserName],[Title],[AddDate],[IsChecked],[CheckedLevel],[PassDate],[ReferenceTimes])");
            strSql.Append(" values (");
            strSql.Append("@AddUserName,@Title,@AddDate,@IsChecked,@CheckedLevel,@PassDate,@ReferenceTimes)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@AddUserName", SqlDbType.VarChar,200),
					new SqlParameter("@Title", SqlDbType.NVarChar,255),
					new SqlParameter("@AddDate", SqlDbType.DateTime),
					new SqlParameter("@IsChecked", SqlDbType.VarChar,18),
					new SqlParameter("@CheckedLevel", SqlDbType.Int,4),
					new SqlParameter("@PassDate", SqlDbType.DateTime),
					new SqlParameter("@ReferenceTimes", SqlDbType.Int)};
            parameters[0].Value = submissionInfo.AddUserName;
            parameters[1].Value = submissionInfo.Title;
            parameters[2].Value = submissionInfo.AddDate;
            parameters[3].Value = submissionInfo.IsChecked;
            parameters[4].Value = submissionInfo.CheckedLevel;
            parameters[5].Value = submissionInfo.PassDate;
            parameters[6].Value = submissionInfo.ReferenceTimes;
            var result = this.ExecuteScalar(strSql.ToString(), parameters);
            if (result != null)
            {
                int.TryParse(result.ToString(), out returnId);
            }
            return returnId;
        }

        public void Update(SubmissionInfo submissionInfo)
        {
            int returnId = 0;
            StringBuilder strSql = new StringBuilder();

            strSql.Append("UPDATE [ml_Submission]");
            strSql.Append("   SET [AddUserName] = @AddUserName");
            strSql.Append("      ,[Title] = @Title");
            strSql.Append("      ,[AddDate] = @AddDate");
            strSql.Append("      ,[IsChecked] = @IsChecked");
            strSql.Append("      ,[CheckedLevel] = @CheckedLevel");
            strSql.Append("      ,[PassDate] = @PassDate");
            strSql.Append("      ,[ReferenceTimes] = @ReferenceTimes");
            strSql.Append(" WHERE  SubmissionID=@SubmissionID");
            SqlParameter[] parameters = {
					new SqlParameter("@AddUserName", SqlDbType.VarChar,200),
					new SqlParameter("@Title", SqlDbType.NVarChar,255),
					new SqlParameter("@AddDate", SqlDbType.DateTime),
					new SqlParameter("@IsChecked", SqlDbType.VarChar,18),
					new SqlParameter("@CheckedLevel", SqlDbType.Int,4),
					new SqlParameter("@PassDate", SqlDbType.DateTime),
					new SqlParameter("@ReferenceTimes", SqlDbType.Int),
					new SqlParameter("@SubmissionID", SqlDbType.Int)};
            parameters[0].Value = submissionInfo.AddUserName;
            parameters[1].Value = submissionInfo.Title;
            parameters[2].Value = submissionInfo.AddDate;
            parameters[3].Value = submissionInfo.IsChecked;
            parameters[4].Value = submissionInfo.CheckedLevel;
            parameters[5].Value = submissionInfo.PassDate;
            parameters[6].Value = submissionInfo.ReferenceTimes;
            parameters[7].Value = submissionInfo.SubmissionID;
            this.ExecuteNonQuery(strSql.ToString(), parameters);
        }

        public void DeleteSubmission(int submissionID)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("Delete [ml_Submission]");
            strSql.AppendLine(" WHERE  SubmissionID=@SubmissionID ");
            strSql.Append("Delete [ml_Content]");
            strSql.Append(" WHERE  ReferenceID=@SubmissionID ");
            SqlParameter[] parameters = {
					new SqlParameter("@SubmissionID", SqlDbType.Int)};
            parameters[0].Value = submissionID;
            this.ExecuteNonQuery(strSql.ToString(), parameters);
        }

        public void ClearSubmission()
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("Delete [ml_Submission]");
            this.ExecuteNonQuery(strSql.ToString());
        }

        public SubmissionInfo GetSubmissionInfo(int submissionID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ml_Submission where SubmissionID=@SubmissionID");
            SqlParameter[] parameters = {
					new SqlParameter("@SubmissionID", SqlDbType.Int,4),};
            parameters[0].Value = submissionID;
            var ds = this.ExecuteDataset(strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
               SubmissionInfo submissionInfo = new SubmissionInfo();
                submissionInfo.SubmissionID = int.Parse(ds.Tables[0].Rows[0]["SubmissionID"].ToString());
                submissionInfo.AddUserName = ds.Tables[0].Rows[0]["AddUserName"].ToString();
                submissionInfo.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                submissionInfo.AddDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["AddDate"].ToString());
                submissionInfo.IsChecked = ds.Tables[0].Rows[0]["IsChecked"].ToString() == "True";
                submissionInfo.CheckedLevel = int.Parse(ds.Tables[0].Rows[0]["CheckedLevel"].ToString());
                if (ds.Tables[0].Rows[0]["PassDate"] is DBNull)
                {
                    submissionInfo.PassDate = null;
                }
                else
                {
                    submissionInfo.PassDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PassDate"].ToString());
                }
                submissionInfo.ReferenceTimes = int.Parse(ds.Tables[0].Rows[0]["ReferenceTimes"].ToString());
                return submissionInfo;
            }
            return null;
        }

        public DataSet GetNodeInfoBySubmissionID(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from siteserver_Node where NodeID=(select top 1 NodeID from ml_Content where ReferenceID=@SubmissionID order by AddDate desc)");
            SqlParameter[] parameters = {
					new SqlParameter("@SubmissionID", SqlDbType.Int,4),};
            parameters[0].Value = id;
            return this.ExecuteDataset(strSql.ToString(), parameters);
        }


        public DataSet GetContentIDsBySubmissionID(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ml_Content");
            strSql.Append(" where ID in ( select (select top 1 id from ml_Content t0 where ReferenceID=@SubmissionID and IsChecked='True' and CheckedLevel=t1.CheckedLevel order by LastEditDate DESC)as id  ");
            strSql.Append(" from ml_Content t1 where ReferenceID=@SubmissionID and IsChecked='True'  group by CheckedLevel)");
            SqlParameter[] parameters = {
					new SqlParameter("@SubmissionID", SqlDbType.Int,4),};
            parameters[0].Value = id;
            return this.ExecuteDataset(strSql.ToString(), parameters);
        }

        public DataSet GetContentIDsBySubmissionID1(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ml_Content");
            strSql.Append(" where ReferenceID=@SubmissionID order by LastEditDate");
            SqlParameter[] parameters = {
					new SqlParameter("@SubmissionID", SqlDbType.Int,4)};
            parameters[0].Value = id;
            return this.ExecuteDataset(strSql.ToString(), parameters);
        }

        public DataSet GetContentIDsAll()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select (select top 1 ID from ml_Content where ReferenceID = SubmissionID order by LastEditDate DESC) as ID from ml_Submission");

            return this.ExecuteDataset(strSql.ToString());
        }


        public int InsertReferenceType(string rtName)
        {
            int returnId = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO [ml_ReferenceType]");
            strSql.Append(" ([RTName]) VALUES (@RTName)");
            strSql.Append(" select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@RTName", SqlDbType.VarChar,200)};
            parameters[0].Value = rtName;
            var result = this.ExecuteScalar(strSql.ToString(), parameters);
            if (result != null)
            {
                int.TryParse(result.ToString(), out returnId);
            }
            return returnId;
        }


        public DataSet GetReferenceTypeList(string where = "1=1")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT RTID,RTName,AddDate FROM ml_ReferenceType where " + where + " order by AddDate");
            return this.ExecuteDataset(strSql.ToString());
        }

        public int InsertReferenceLogs(ReferenceLog referenceLogInfo)
        {
            int returnId = 0;
            StringBuilder strSql = new StringBuilder();

            strSql.Append("INSERT INTO [ml_ReferenceLogs]");
            strSql.Append("           ([RTID]");
            strSql.Append("           ,[PublishmentSystemID]");
            strSql.Append("           ,[NodeID]");
            strSql.Append("           ,[ToContentID]");
            strSql.Append("           ,[Operator]");
            strSql.Append("           ,[OperateDate]");
            strSql.Append("           ,[SubmissionID])");
            strSql.Append("     VALUES");
            strSql.Append("           (@RTID,");
            strSql.Append("            @PublishmentSystemID,");
            strSql.Append("            @NodeID,");
            strSql.Append("            @ToContentID,");
            strSql.Append("            @Operator,");
            strSql.Append("            @OperateDate,");
            strSql.Append("            @SubmissionID)");
            strSql.Append(" select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@RTID", SqlDbType.Int,4),
					new SqlParameter("@PublishmentSystemID", SqlDbType.Int,4),
					new SqlParameter("@NodeID", SqlDbType.Int,4),
					new SqlParameter("@ToContentID", SqlDbType.Int,4),
					new SqlParameter("@Operator", SqlDbType.VarChar,200),
					new SqlParameter("@OperateDate", SqlDbType.DateTime),
					new SqlParameter("@SubmissionID", SqlDbType.Int,4)};

            parameters[0].Value = referenceLogInfo.RTID;
            parameters[1].Value = referenceLogInfo.PublishmentSystemID;
            parameters[2].Value = referenceLogInfo.NodeID;
            parameters[3].Value = referenceLogInfo.ToContentID;
            parameters[4].Value = referenceLogInfo.Operator;
            parameters[5].Value = referenceLogInfo.OperateDate;
            parameters[6].Value = referenceLogInfo.SubmissionID;

            var result = this.ExecuteScalar(strSql.ToString(), parameters);
            if (result != null)
            {
                int.TryParse(result.ToString(), out returnId);
            }
            return returnId;

        }

        public int GetReferenceLogCount(int sid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) from ml_ReferenceLogs ");
            strSql.Append(" where SubmissionID=" + sid);

            return TranslateUtils.ToInt(this.ExecuteScalar(strSql.ToString()).ToString());
        }
        public string GetConfigAttr(string key)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [value] FROM ml_Config where Attrkey='" + key + "'");
            return this.ExecuteScalar(strSql.ToString()).ToString();
        }
        public void UpdateConfigAttr(string key, string value)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("update ml_Config set [value]='" + value + "' where Attrkey='" + key + "'");
            if (this.ExecuteNonQuery(strSql.ToString())==0) {
                strSql.Append("INSERT INTO [ml_Config]([AttrName],[Attrkey],[value],[discription]) VALUES('','" + key + "','" + value + "','')");
                this.ExecuteNonQuery(strSql.ToString());
            }
        }


        public DataSet GetRoleCheckLevel(string where = "1=1")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [ID],[RoleName],[CheckLevel]  FROM [ml_RoleCheckLevel] where " + where);
            return this.ExecuteDataset(strSql.ToString());
        }

        public void UpdateRoleCheckLevel(string roleName, string[] checkLevel)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("delete ml_RoleCheckLevel where RoleName='" + roleName + "' ");
            foreach (var item in checkLevel)
            {
                strSql.Append("insert into ml_RoleCheckLevel([RoleName],[CheckLevel]) values('" + roleName + "'," + item + ")");
            }
            this.ExecuteNonQuery(strSql.ToString());
        }

        public DataSet GetRCNode(string where = "1=1")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [ID],[RCID],[NodeID]  FROM [ml_RCNode] where " + where);
            return this.ExecuteDataset(strSql.ToString());
        }

        public void UpdateNodeAdminRoles(int RCID, string[] nodeID)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("delete ml_RCNode where RCID=" + RCID + "");
            foreach (var item in nodeID)
            {
                strSql.Append("insert into ml_RCNode([RCID],[NodeID]) values(" + RCID + "," + item + ")");
            }
            this.ExecuteNonQuery(strSql.ToString());
        }

        public int GetSubmissionCount(string where = "1=1")
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) from ml_Submission left join ml_Content");
            strSql.Append(" on ml_Submission.SubmissionID = ml_Content.ReferenceID and ml_Submission.CheckedLevel=ml_Content.CheckedLevel");
            strSql.Append(" where " + where);

            return TranslateUtils.ToInt(this.ExecuteScalar(strSql.ToString()).ToString());
        }

        public int GetDraftCount(int PublishmentSystemID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) from ml_Content");
            strSql.Append(" where NodeID="+ PublishmentSystemID + " AND  checkedlevel=0 and IsChecked='True'");

            return TranslateUtils.ToInt(this.ExecuteScalar(strSql.ToString()).ToString());
        }



        public void UpdateSubmissionTaxis(int nodeID, int contentID, bool isUp, int changeTaxis)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append(" select top " + changeTaxis + " ml_Content.ID,ml_Content.Taxis from ml_Submission left join ml_Content");
            strSql.Append(" on ml_Submission.SubmissionID = ml_Content.ReferenceID and ml_Submission.CheckedLevel=ml_Content.CheckedLevel");
            strSql.Append(" where ID in (select (select top 1 ID from ml_Content where ReferenceID = SubmissionID order by LastEditDate DESC) as ID from ml_Submission)");
            strSql.Append(" and NodeID =" + nodeID + " and Taxis" + (isUp ? ">" : "<") + "(select Taxis from ml_Content where ID =" + contentID + ")");
            strSql.Append(" order by Taxis " + (isUp ? "asc" : "desc"));

            var needChangeContents = this.ExecuteDataset(strSql.ToString());
            strSql = new StringBuilder();
            int newTaxis = 0;
            for (int i = 0; i < needChangeContents.Tables[0].Rows.Count; i++)
            {
                if (i == needChangeContents.Tables[0].Rows.Count - 1)
                {
                    newTaxis = TranslateUtils.ToInt(needChangeContents.Tables[0].Rows[i]["Taxis"].ToString());
                }
                strSql.Append(" UPDATE ml_Content SET Taxis=Taxis" + (isUp ? "-" : "+") + "1 WHERE ID=" + needChangeContents.Tables[0].Rows[i]["ID"].ToString());
            }
            strSql.Append(" UPDATE ml_Content SET Taxis=" + newTaxis + " WHERE ID=" + contentID);
            this.ExecuteNonQuery(strSql.ToString());
        }
    }
}
