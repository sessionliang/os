using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.Project.Model;
using SiteServer.Project.Core;
using System.Collections.Generic;

namespace SiteServer.Project.Provider.Data.SqlServer
{
    public class ProjectDAO : DataProviderBase, IProjectDAO 
	{
        private const string SQL_UPDATE = "UPDATE pms_Project SET ProjectName = @ProjectName, ProjectType = @ProjectType, IsContract = @IsContract, AmountTotal = @AmountTotal, AmountCashBack = @AmountCashBack, ContractNO = @ContractNO, Description = @Description, AddDate = @AddDate, IsClosed = @IsClosed, CloseDate = @CloseDate, UserNameAM = @UserNameAM, UserNamePM = @UserNamePM, UserNameCollection = @UserNameCollection, SettingsXML = @SettingsXML WHERE ProjectID = @ProjectID";

        private const string SQL_DELETE = "DELETE FROM pms_Project WHERE ProjectID = @ProjectID";

        private const string SQL_SELECT = "SELECT ProjectID, ProjectName, ProjectType, IsContract, AmountTotal, AmountCashBack, ContractNO, Description, AddDate, IsClosed, CloseDate, UserNameAM, UserNamePM, UserNameCollection, SettingsXML FROM pms_Project WHERE ProjectID = @ProjectID";

        private const string SQL_SELECT_BY_NAME = "SELECT ProjectID, ProjectName, ProjectType, IsContract, AmountTotal, AmountCashBack, ContractNO, Description, AddDate, IsClosed, CloseDate, UserNameAM, UserNamePM, UserNameCollection, SettingsXML FROM pms_Project WHERE ProjectName = @ProjectName";

        private const string SQL_SELECT_BY_IS_CLOSED = "SELECT ProjectID, ProjectName, ProjectType, IsContract, AmountTotal, AmountCashBack, ContractNO, Description, AddDate, IsClosed, CloseDate, UserNameAM, UserNamePM, UserNameCollection, SettingsXML FROM pms_Project WHERE IsClosed = @IsClosed ORDER BY AddDate DESC";

        private const string SQL_SELECT_ALL = "SELECT ProjectID, ProjectName, ProjectType, IsContract, AmountTotal, AmountCashBack, ContractNO, Description, AddDate, IsClosed, CloseDate, UserNameAM, UserNamePM, UserNameCollection, SettingsXML FROM pms_Project ORDER BY AddDate DESC";

        private const string SQL_SELECT_PROJECT_NAME = "SELECT ProjectName FROM pms_Project ORDER BY AddDate DESC";

        private const string SQL_SELECT_PROJECT_NAME_BY_ID = "SELECT ProjectName FROM pms_Project WHERE ProjectID = @ProjectID";

        private const string PARM_PROJECT_ID = "@ProjectID";
        private const string PARM_PROJECT_NAME = "@ProjectName";
        private const string PARM_PROJECT_TYPE = "@ProjectType";
        private const string PARM_IS_CONTRACT = "@IsContract";
        private const string PARM_AMOUNT_TOTAL = "@AmountTotal";
        private const string PARM_AMOUNT_CASH_BACK = "@AmountCashBack";
        private const string PARM_CONTRACT_NO = "@ContractNO";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_IS_CLOSED = "@IsClosed";
        private const string PARM_CLOSE_DATE = "@CloseDate";
        private const string PARM_USER_NAME_AM = "@UserNameAM";
        private const string PARM_USER_NAME_PM = "@UserNamePM";
        private const string PARM_USER_NAME_COLLECTION = "@UserNameCollection";
        private const string PARM_SETTINGS_XML = "@SettingsXML";

		public int Insert(ProjectInfo projectInfo) 
		{
            int projectID = 0;

            string sqlString = "INSERT INTO pms_Project (ProjectName, ProjectType, IsContract, AmountTotal, AmountCashBack, ContractNO, Description, AddDate, IsClosed, CloseDate, UserNameAM, UserNamePM, UserNameCollection, SettingsXML) VALUES (@ProjectName, @ProjectType, @IsContract, @AmountTotal, @AmountCashBack, @ContractNO, @Description, @AddDate, @IsClosed, @CloseDate, @UserNameAM, @UserNamePM, @UserNameCollection, @SettingsXML)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO pms_Project(ProjectID, ProjectName, ProjectType, IsContract, AmountTotal, AmountCashBack, ContractNO, Description, AddDate, IsClosed, CloseDate, UserNameAM, UserNamePM, UserNameCollection, SettingsXML) VALUES (pms_Project_SEQ.NEXTVAL, @ProjectName, @ProjectType, @IsContract, @AmountTotal, @AmountCashBack, @ContractNO, @Description, @AddDate, @IsClosed, @CloseDate, @UserNameAM, @UserNamePM, @UserNameCollection, @SettingsXML)";
            }

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PROJECT_NAME, EDataType.NVarChar, 50, projectInfo.ProjectName),
                this.GetParameter(PARM_PROJECT_TYPE, EDataType.NVarChar, 50, projectInfo.ProjectType),
                this.GetParameter(PARM_IS_CONTRACT, EDataType.VarChar, 18, projectInfo.IsContract.ToString()),
                this.GetParameter(PARM_AMOUNT_TOTAL, EDataType.Integer, projectInfo.AmountTotal),
                this.GetParameter(PARM_AMOUNT_CASH_BACK, EDataType.Integer, projectInfo.AmountCashBack),
                this.GetParameter(PARM_CONTRACT_NO, EDataType.NVarChar, 50, projectInfo.ContractNO),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, projectInfo.Description),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, projectInfo.AddDate),
                this.GetParameter(PARM_IS_CLOSED, EDataType.VarChar, 18, projectInfo.IsClosed.ToString()),
                this.GetParameter(PARM_CLOSE_DATE, EDataType.DateTime, projectInfo.CloseDate),
                this.GetParameter(PARM_USER_NAME_AM, EDataType.NVarChar, 50, projectInfo.UserNameAM),
                this.GetParameter(PARM_USER_NAME_PM, EDataType.NVarChar, 50, projectInfo.UserNamePM),
                this.GetParameter(PARM_USER_NAME_COLLECTION, EDataType.NVarChar, 255, projectInfo.UserNameCollection),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, projectInfo.Additional.ToString())
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        projectID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "pms_Project");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            if (projectID > 0)
            {
                ProjectManager.AddDefaultTypeInfos(projectID);
            }

            return projectID;
		}

        public void Update(ProjectInfo projectInfo) 
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PROJECT_NAME, EDataType.NVarChar, 50, projectInfo.ProjectName),
                this.GetParameter(PARM_PROJECT_TYPE, EDataType.NVarChar, 50, projectInfo.ProjectType),
                this.GetParameter(PARM_IS_CONTRACT, EDataType.VarChar, 18, projectInfo.IsContract.ToString()),
                this.GetParameter(PARM_AMOUNT_TOTAL, EDataType.Integer, projectInfo.AmountTotal),
                this.GetParameter(PARM_AMOUNT_CASH_BACK, EDataType.Integer, projectInfo.AmountCashBack),
                this.GetParameter(PARM_CONTRACT_NO, EDataType.NVarChar, 50, projectInfo.ContractNO),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, projectInfo.Description),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, projectInfo.AddDate),
                this.GetParameter(PARM_IS_CLOSED, EDataType.VarChar, 18, projectInfo.IsClosed.ToString()),
                this.GetParameter(PARM_CLOSE_DATE, EDataType.DateTime, projectInfo.CloseDate),
                this.GetParameter(PARM_USER_NAME_AM, EDataType.NVarChar, 50, projectInfo.UserNameAM),
                this.GetParameter(PARM_USER_NAME_PM, EDataType.NVarChar, 50, projectInfo.UserNamePM),
                this.GetParameter(PARM_USER_NAME_COLLECTION, EDataType.NVarChar, 255, projectInfo.UserNameCollection),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, projectInfo.Additional.ToString()),
                this.GetParameter(PARM_PROJECT_ID, EDataType.Integer, projectInfo.ProjectID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
		}

		public void Delete(int projectID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PROJECT_ID, EDataType.Integer, projectID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
		}

        public ProjectInfo GetProjectInfo(int projectID)
		{
            ProjectInfo projectInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PROJECT_ID, EDataType.Integer, projectID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    projectInfo = new ProjectInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetDateTime(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetDateTime(10), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString());
                }
                rdr.Close();
            }

            return projectInfo;
		}

        public ProjectInfo GetProjectInfo(string projectName)
        {
            ProjectInfo projectInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PROJECT_NAME, EDataType.NVarChar, 50, projectName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_NAME, parms))
            {
                if (rdr.Read())
                {
                    projectInfo = new ProjectInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetDateTime(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetDateTime(10), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString());
                }
                rdr.Close();
            }

            return projectInfo;
        }

        public string GetProjectName(int projectID)
        {
            string projectName = string.Empty;

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PROJECT_ID, EDataType.Integer, projectID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PROJECT_NAME_BY_ID, selectParms))
            {
                if (rdr.Read())
                {
                    projectName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return projectName;
        }

		public IEnumerable GetDataSource(bool isClosed)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_IS_CLOSED, EDataType.VarChar, 18, isClosed.ToString())
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_BY_IS_CLOSED, parms);
            return enumerable;
		}

        public IEnumerable GetDataSource(string type)
        {
            string sqlString = "SELECT ProjectID, ProjectName, ProjectType, IsContract, AmountTotal, AmountCashBack, ContractNO, Description, AddDate, IsClosed, CloseDate, UserNameAM, UserNamePM, UserNameCollection, SettingsXML FROM pms_Project WHERE IsContract = 'True' ORDER BY AddDate DESC";

            if (!string.IsNullOrEmpty(type))
            {
                if (type == "Year")
                {
                    sqlString = string.Format("SELECT ProjectID, ProjectName, ProjectType, IsContract, AmountTotal, AmountCashBack, ContractNO, Description, AddDate, IsClosed, CloseDate, UserNameAM, UserNamePM, UserNameCollection, SettingsXML FROM pms_Project WHERE IsContract = 'True' AND year(AddDate) = {0} ORDER BY AddDate DESC", DateTime.Now.Year);
                }
                else if (type == "Month")
                {
                    sqlString = string.Format("SELECT ProjectID, ProjectName, ProjectType, IsContract, AmountTotal, AmountCashBack, ContractNO, Description, AddDate, IsClosed, CloseDate, UserNameAM, UserNamePM, UserNameCollection, SettingsXML FROM pms_Project WHERE IsContract = 'True' AND year(AddDate) = {0} AND month(AddDate) = {1} ORDER BY AddDate DESC", DateTime.Now.Year, DateTime.Now.Month);
                }
            }

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }

        public int GetCount()
        {
            string sqlString = "SELECT COUNT(*) FROM pms_Project";
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public ArrayList GetProjectInfoArrayList(bool isClosed)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_IS_CLOSED, EDataType.VarChar, 18, isClosed.ToString())
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_IS_CLOSED, parms))
            {
                while (rdr.Read())
                {
                    ProjectInfo projectInfo = new ProjectInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetDateTime(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetDateTime(10), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString());
                    arraylist.Add(projectInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public Dictionary<int, string> GetProjectIDWithNameDictionary(string userName)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            string sqlString = string.Format("SELECT ProjectID, ProjectName, ProjectType, IsContract, AmountTotal, AmountCashBack, ContractNO, Description, AddDate, IsClosed, CloseDate, UserNameAM, UserNamePM, UserNameCollection, SettingsXML FROM pms_Project WHERE UserNameCollection LIKE '{0},%' OR UserNameCollection LIKE '%,{0},%' OR UserNameCollection LIKE '%,{0}' ORDER BY AddDate DESC", userName);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    dictionary.Add(rdr.GetInt32(0), rdr.GetValue(1).ToString());
                }
                rdr.Close();
            }

            return dictionary;
        }

        public ArrayList GetProjectNameArrayList()
        {
            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PROJECT_NAME))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return arraylist;
        }

        public int GetAmountNet(int year, int month)
        {
            string sqlString = string.Format("SELECT SUM(AmountTotal) - SUM(AmountCashBack) FROM pms_Project WHERE IsContract = 'True' AND year(AddDate) = {0} AND month(AddDate) = {1}", year, month);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public Dictionary<int, int> GetProjectIDAmountNetDictionary(int year, int month)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            string sqlString = string.Format("SELECT ProjectID, SUM(AmountTotal) - SUM(AmountCashBack) FROM pms_Project WHERE IsContract = 'True' AND year(AddDate) = {0} AND month(AddDate) = {1} GROUP BY ProjectID", year, month);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int projectID = rdr.GetInt32(0);
                    int amountNet = rdr.GetInt32(1);
                    dictionary.Add(projectID, amountNet);
                }
                rdr.Close();
            }

            return dictionary;
        }
	}
}