using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CRM.Model;
using SiteServer.CRM.Core;

namespace SiteServer.CRM.Provider.Data.SqlServer
{
    public class DocumentDAO : DataProviderBase, IDocumentDAO
	{
        private const string SQL_DELETE = "DELETE FROM pms_Document WHERE DocumentID = @DocumentID";

        private const string SQL_SELECT = "SELECT DocumentID, DocumentType, ContractID, TypeID, FileName, Version, Description, UserName, AddDate FROM pms_Document WHERE DocumentID = @DocumentID";

        private const string SQL_SELECT_ALL_BY_CATEGORY = "SELECT DocumentID, DocumentType, ContractID, TypeID, FileName, Version, Description, UserName, AddDate FROM pms_Document WHERE TypeID = @TypeID ORDER BY DocumentID";

        private const string SQL_SELECT_ALL_BY_CONTRACT = "SELECT DocumentID, DocumentType, ContractID, TypeID, FileName, Version, Description, UserName, AddDate FROM pms_Document WHERE ContractID = @ContractID ORDER BY DocumentID";

        private const string SQL_SELECT_ALL = "SELECT DocumentID, DocumentType, ContractID, TypeID, FileName, Version, Description, UserName, AddDate FROM pms_Document ORDER BY DocumentID";

        private const string SQL_SELECT_BY_USERNAME = "SELECT DocumentID, DocumentType, ContractID, TypeID, FileName, Version, Description, UserName, AddDate FROM pms_Document WHERE UserName = @UserName ORDER BY DocumentID";

        private const string PARM_DOCUMENT_ID = "@DocumentID";
        private const string PARM_DOCUMENT_TYPE = "@DocumentType";
        private const string PARM_CONTRACT_ID = "@ContractID";
        private const string PARM_TYPE_ID = "@TypeID";
        private const string PARM_FILE_NAME = "@FileName";
        private const string PARM_VERSION = "@Version";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_ADD_DATE = "@AddDate";

		public void Insert(DocumentInfo documentInfo) 
		{
            string sqlString = "INSERT INTO pms_Document (DocumentType, ContractID, TypeID, FileName, Version, Description, UserName, AddDate) VALUES (@DocumentType, @ContractID, @TypeID, @FileName, @Version, @Description, @UserName, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO pms_Document(DocumentID, DocumentType, ContractID, TypeID, FileName, Version, Description, UserName, AddDate) VALUES (pms_Document_SEQ.NEXTVAL, @DocumentType, @ContractID, @TypeID, @FileName, @Version, @Description, @UserName, @AddDate)";
            }

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_DOCUMENT_TYPE, EDataType.VarChar, 50, EDocumentTypeUtils.GetValue(documentInfo.DocumentType)),
                this.GetParameter(PARM_CONTRACT_ID, EDataType.Integer, documentInfo.ContractID),
                this.GetParameter(PARM_TYPE_ID, EDataType.Integer, documentInfo.TypeID),
                this.GetParameter(PARM_FILE_NAME, EDataType.NVarChar, 255, documentInfo.FileName),
                this.GetParameter(PARM_VERSION, EDataType.VarChar, 50, documentInfo.Version),
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

        public DocumentInfo GetDocumentInfo(int documentID)
		{
            DocumentInfo documentInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_DOCUMENT_ID, EDataType.Integer, documentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    documentInfo = new DocumentInfo(rdr.GetInt32(0), EDocumentTypeUtils.GetEnumType(rdr.GetValue(1).ToString()), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetDateTime(8));
                }
                rdr.Close();
            }

            return documentInfo;
		}

		public IEnumerable GetDataSourceByCategory(int typeID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TYPE_ID, EDataType.Integer, typeID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_BY_CATEGORY, parms);
			return enumerable;
		}

        public IEnumerable GetDataSourceByContract(int contractID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CONTRACT_ID, EDataType.Integer, contractID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_BY_CONTRACT, parms);
            return enumerable;
        }

        public IEnumerable GetDataSource(string userName)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 50, userName)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_BY_USERNAME, parms);
            return enumerable;
        }

        public IEnumerable GetDataSource()
        {
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL);
            return enumerable;
        }

        public int GetCountByCategory(int typeID)
        {
            string sqlString = "SELECT COUNT(*) FROM pms_Document WHERE TypeID = " + typeID;
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByContract(int contractID)
        {
            string sqlString = "SELECT COUNT(*) FROM pms_Document WHERE ContractID = " + contractID;
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }
	}
}