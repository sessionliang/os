using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.Project.Model;
using SiteServer.Project.Core;

namespace SiteServer.Project.Provider.Data.SqlServer
{
    public class ProjectDocumentDAO : DataProviderBase, IProjectDocumentDAO
	{
        private const string SQL_DELETE = "DELETE FROM pms_ProjectDocument WHERE DocumentID = @DocumentID";

        private const string SQL_SELECT = "SELECT DocumentID, ProjectID, FileName, Description, UserName, AddDate FROM pms_ProjectDocument WHERE DocumentID = @DocumentID";

        private const string SQL_SELECT_ALL = "SELECT DocumentID, ProjectID, FileName, Description, UserName, AddDate FROM pms_ProjectDocument WHERE ProjectID = @ProjectID ORDER BY DocumentID";

        private const string PARM_DOCUMENT_ID = "@DocumentID";
        private const string PARM_PROJECT_ID = "@ProjectID";
        private const string PARM_FILE_NAME = "@FileName";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_ADD_DATE = "@AddDate";

		public void Insert(ProjectDocumentInfo documentInfo) 
		{
            string sqlString = "INSERT INTO pms_ProjectDocument (ProjectID, FileName, Description, UserName, AddDate) VALUES (@ProjectID, @FileName, @Description, @UserName, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO pms_ProjectDocument(DocumentID, ProjectID, FileName, Description, UserName, AddDate) VALUES (pms_ProjectDocument_SEQ.NEXTVAL, @ProjectID, @FileName, @Description, @UserName, @AddDate)";
            }

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PROJECT_ID, EDataType.Integer, documentInfo.ProjectID),
                this.GetParameter(PARM_FILE_NAME, EDataType.NVarChar, 255, documentInfo.FileName),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, documentInfo.Description),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 50, documentInfo.UserName),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, documentInfo.AddDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
		}

		public void Delete(int documentID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_DOCUMENT_ID, EDataType.Integer, documentID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
		}

        public ProjectDocumentInfo GetDocumentInfo(int documentID)
		{
            ProjectDocumentInfo documentInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_DOCUMENT_ID, EDataType.Integer, documentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    documentInfo = new ProjectDocumentInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetDateTime(5));
                }
                rdr.Close();
            }

            return documentInfo;
		}

		public IEnumerable GetDataSource(int projectID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PROJECT_ID, EDataType.Integer, projectID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL, parms);
			return enumerable;
		}

        public int GetCount(int projectID)
        {
            string sqlString = "SELECT COUNT(*) FROM pms_ProjectDocument WHERE ProjectID = " + projectID;
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }
	}
}