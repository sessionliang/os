using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core.Data.Provider;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core.Data;
using BaiRong.Model;
using BaiRong.Core;
using System;


namespace SiteServer.Project.Provider.Data.SqlServer
{
    public class ApplicationDAO : DataProviderBase, IApplicationDAO
	{
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.InnerConnectionString;
            }
        }

        private const string SQL_INSERT = "INSERT INTO brs_Application (ApplicationType, ApplyResource, IPAddress, AddDate, ContactPerson, Email, Mobile, QQ, Telephone, Location, Address, OrgType, OrgName, IsITDepartment, Comment, IsHandle, HandleDate, HandleUserName, HandleSummary) VALUES (@ApplicationType, @ApplyResource, @IPAddress, @AddDate, @ContactPerson, @Email, @Mobile, @QQ, @Telephone, @Location, @Address, @OrgType, @OrgName, @IsITDepartment, @Comment, @IsHandle, @HandleDate, @HandleUserName, @HandleSummary)";

        private const string SQL_SELECT = "SELECT ID, ApplicationType, ApplyResource, IPAddress, AddDate, ContactPerson, Email, Mobile, QQ, Telephone, Location, Address, OrgType, OrgName, IsITDepartment, Comment, IsHandle, HandleDate, HandleUserName, HandleSummary FROM brs_Application WHERE ID = @ID";

        private const string SQL_UPDATE = "UPDATE brs_Application SET IsHandle = @IsHandle, HandleDate = @HandleDate, HandleUserName = @HandleUserName, HandleSummary = @HandleSummary WHERE ID = @ID";

        private const string SQL_DELETE = "DELETE brs_Application WHERE ID = @ID";

        public void Insert(ApplicationInfo info)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@ApplicationType", EDataType.VarChar, 50, EApplicationTypeUtils.GetValue(info.ApplicationType)),
                this.GetParameter("@ApplyResource", EDataType.NVarChar, 255, info.ApplyResource),
                this.GetParameter("@IPAddress", EDataType.VarChar, 200, info.IPAddress),
				this.GetParameter("@AddDate", EDataType.DateTime, info.AddDate),
                this.GetParameter("@ContactPerson", EDataType.NVarChar, 50, info.ContactPerson),
                this.GetParameter("@Email", EDataType.NVarChar, 200, info.Email),
                this.GetParameter("@Mobile", EDataType.VarChar, 50, info.Mobile),
                this.GetParameter("@QQ", EDataType.VarChar, 50, info.QQ),
                this.GetParameter("@Telephone", EDataType.VarChar, 50, info.Telephone),
                this.GetParameter("@Location", EDataType.NVarChar, 200, info.Location),
                this.GetParameter("@Address", EDataType.NVarChar, 255, info.Address),
                this.GetParameter("@OrgType", EDataType.NVarChar, 50, info.OrgType),
                this.GetParameter("@OrgName", EDataType.NVarChar, 255, info.OrgName),
                this.GetParameter("@IsITDepartment", EDataType.VarChar, 18, info.IsITDepartment.ToString()),
                this.GetParameter("@Comment", EDataType.NText, info.Comment),
                this.GetParameter("@IsHandle", EDataType.VarChar, 18, info.IsHandle.ToString()),
                this.GetParameter("@HandleDate", EDataType.DateTime, info.HandleDate),
                this.GetParameter("@HandleUserName", EDataType.NVarChar, 50, info.HandleUserName),
                this.GetParameter("@HandleSummary", EDataType.NVarChar, 255, info.HandleSummary)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
		}

        public void Delete(int id)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@ID", EDataType.Integer, id)
			};

            this.ExecuteReader(SQL_DELETE, parms);
        }

        public void Handle(int id, string handleSummary)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@IsHandle", EDataType.VarChar, 18, true.ToString()),
                this.GetParameter("@HandleDate", EDataType.DateTime, DateTime.Now),
                this.GetParameter("@HandleUserName", EDataType.NVarChar, 50, AdminManager.Current.UserName),
                this.GetParameter("@HandleSummary", EDataType.NVarChar, 255, handleSummary),
                this.GetParameter("@ID", EDataType.Integer, id)
			};

            this.ExecuteReader(SQL_UPDATE, parms);
        }

        public ApplicationInfo GetApplicationInfo(int id)
		{
            ApplicationInfo info = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@ID", EDataType.Integer, id)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    info = new ApplicationInfo(rdr.GetInt32(0), EApplicationTypeUtils.GetEnumType(rdr.GetValue(1).ToString()), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetDateTime(4), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), TranslateUtils.ToBool(rdr.GetValue(14).ToString()), rdr.GetValue(15).ToString(), TranslateUtils.ToBool(rdr.GetValue(16).ToString()), rdr.GetDateTime(17), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString());
                }
                rdr.Close();
            }

			return info;
		}

        public string GetSelectSqlStringNotHandled()
        {
            return "SELECT ID, ApplicationType, ApplyResource, IPAddress, AddDate, ContactPerson, Email, Mobile, QQ, Telephone, Location, Address, OrgType, OrgName, IsITDepartment, Comment, IsHandle, HandleDate, HandleUserName, HandleSummary FROM brs_Application WHERE IsHandle <> 'True'";
        }

        public string GetSelectSqlStringNotHandled(string applicationType, string applyResource, string keyword)
        {
            string sqlString = "SELECT ID, ApplicationType, ApplyResource, IPAddress, AddDate, ContactPerson, Email, Mobile, QQ, Telephone, Location, Address, OrgType, OrgName, IsITDepartment, Comment, IsHandle, HandleDate, HandleUserName, HandleSummary FROM brs_Application";

            string whereString = " WHERE IsHandle <> 'True'";
            if (!string.IsNullOrEmpty(applicationType))
            {
                whereString += string.Format(" AND ApplicationType = '{0}' ", applicationType);
            }
            if (!string.IsNullOrEmpty(applyResource))
            {
                whereString += string.Format(" AND ApplyResource LIKE '%{0}%' ", applyResource);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                whereString += string.Format(" AND (ContactPerson LIKE '%{0}%' OR Email LIKE '%{0}%' OR Mobile LIKE '%{0}%' OR QQ LIKE '%{0}%' OR Telephone LIKE '%{0}%' OR Location LIKE '%{0}%' OR Address LIKE '%{0}%' OR OrgType LIKE '%{0}%' OR OrgName LIKE '%{0}%') ", keyword);
            }

            return sqlString + whereString;
        }

        public string GetSelectSqlStringHandled()
        {
            return "SELECT ID, ApplicationType, ApplyResource, IPAddress, AddDate, ContactPerson, Email, Mobile, QQ, Telephone, Location, Address, OrgType, OrgName, IsITDepartment, Comment, IsHandle, HandleDate, HandleUserName, HandleSummary FROM brs_Application WHERE IsHandle = 'True'";
        }

        public string GetSelectSqlStringHandled(string applicationType, string applyResource, string keyword)
        {
            string sqlString = "SELECT ID, ApplicationType, ApplyResource, IPAddress, AddDate, ContactPerson, Email, Mobile, QQ, Telephone, Location, Address, OrgType, OrgName, IsITDepartment, Comment, IsHandle, HandleDate, HandleUserName, HandleSummary FROM brs_Application";

            string whereString = " WHERE IsHandle = 'True'";
            if (!string.IsNullOrEmpty(applicationType))
            {
                whereString += string.Format(" AND ApplicationType = '{0}' ", applicationType);
            }
            if (!string.IsNullOrEmpty(applyResource))
            {
                whereString += string.Format(" AND ApplyResource LIKE '%{0}%' ", applyResource);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                whereString += string.Format(" AND (ContactPerson LIKE '%{0}%' OR Email LIKE '%{0}%' OR Mobile LIKE '%{0}%' OR QQ LIKE '%{0}%' OR Telephone LIKE '%{0}%' OR Location LIKE '%{0}%' OR Address LIKE '%{0}%' OR OrgType LIKE '%{0}%' OR OrgName LIKE '%{0}%') ", keyword);
            }

            return sqlString + whereString;
        }

        public string GetSortFieldName()
        {
            return "ID";
        }

        public string GetSortFieldNameOfHandled()
        {
            return "HandleDate";
        }

        public int GetTotalCount()
        {
            string sqlString = string.Format("select count(*) from brs_Application");

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }
	}
}
