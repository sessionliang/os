using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CRM.Model;
using SiteServer.CRM.Core;


namespace SiteServer.CRM.Provider.Data.SqlServer
{
    public class ApplyDAO : DataProviderBase, IApplyDAO
	{
        public int Insert(ApplyInfo applyInfo)
        {
            int applyID = 0;

            applyInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(applyInfo.Attributes, ApplyInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        applyID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, ApplyInfo.TableName);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return applyID;
        }

        public void Update(ApplyInfo applyInfo)
        {
            applyInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(applyInfo.Attributes, ApplyInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateState(int applyID, EApplyState state)
        {
            string sqlString = string.Format("UPDATE {0} SET State = '{1}' WHERE ID = {2}", ApplyInfo.TableName, EApplyStateUtils.GetValue(state), applyID);
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", ApplyInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public ApplyInfo GetApplyInfo(int projectID, NameValueCollection form)
        {
            int departmentID = TranslateUtils.ToInt(form[ApplyAttribute.DepartmentID]);
            ApplyInfo applyInfo = new ApplyInfo(0, projectID, TranslateUtils.ToInt(form[ApplyAttribute.Priority]), TranslateUtils.ToInt(form[ApplyAttribute.TypeID]), AdminManager.Current.UserName, DateTime.Now, string.Empty, DateTime.Now, string.Empty, DateTime.Now, departmentID, string.Empty, TranslateUtils.ToInt(form[ApplyAttribute.FileCount]), EApplyState.New, form[ApplyAttribute.Title]);

            foreach (string name in form.AllKeys)
            {
                if (ApplyAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        applyInfo.SetExtendedAttribute(name, value);
                    }
                }
            }

            return applyInfo;
        }

        public ApplyInfo GetApplyInfo(int applyID)
        {
            ApplyInfo applyInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", applyID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(ApplyInfo.TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    applyInfo = new ApplyInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, applyInfo);
                }
                rdr.Close();
            }

            if (applyInfo != null) applyInfo.AfterExecuteReader();
            return applyInfo;
        }

        public ApplyInfo GetApplyInfo(int projectID, string queryCode)
        {
            ApplyInfo applyInfo = null;
            string SQL_WHERE = string.Format("WHERE ProjectID = {0} AND QueryCode = '{1}'", projectID, queryCode);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(ApplyInfo.TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    applyInfo = new ApplyInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, applyInfo);
                }
                rdr.Close();
            }

            if (applyInfo != null) applyInfo.AfterExecuteReader();
            return applyInfo;
        }

        public EApplyState GetState(int applyID)
        {
            EApplyState state = EApplyState.New;
            string sqlString = string.Format("SELECT State FROM {0} WHERE ID = {1}", ApplyInfo.TableName, applyID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    state = EApplyStateUtils.GetEnumType(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }
            return state;
        }

        public ArrayList GetAddUserNameArrayList(int projectID)
        {
            string sqlString;
            if (projectID > 0)
            {
                sqlString = string.Format("SELECT DISTINCT pms_Apply.AddUserName FROM pms_Apply INNER JOIN bairong_Administrator ON pms_Apply.AddUserName = bairong_Administrator.UserName WHERE ProjectID = {0}", projectID);
            }
            else
            {
                sqlString = "SELECT DISTINCT pms_Apply.AddUserName FROM pms_Apply INNER JOIN bairong_Administrator ON pms_Apply.AddUserName = bairong_Administrator.UserName";
            }
            return BaiRongDataProvider.DatabaseDAO.GetStringArrayList(sqlString);
        }

        public ArrayList GetUserNameArrayList(int projectID)
        {
            string sqlString;
            if (projectID > 0)
            {
                sqlString = string.Format("SELECT DISTINCT pms_Apply.UserName FROM pms_Apply INNER JOIN bairong_Administrator ON pms_Apply.UserName = bairong_Administrator.UserName WHERE ProjectID = {0}", projectID);
            }
            else
            {
                sqlString = "SELECT DISTINCT pms_Apply.UserName FROM pms_Apply INNER JOIN bairong_Administrator ON pms_Apply.UserName = bairong_Administrator.UserName";
            }
            return BaiRongDataProvider.DatabaseDAO.GetStringArrayList(sqlString);
        }

        public int GetCountByProjectID(int projectID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (ProjectID = {1})", ApplyInfo.TableName, projectID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCount()
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0}", ApplyInfo.TableName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentID(int departmentID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE DepartmentID = {1}", ApplyInfo.TableName, departmentID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentID(int departmentID, int projectID, DateTime begin, DateTime end)
        {
            string sqlString = string.Empty;
            if (projectID == 0)
            {
                sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE DepartmentID = {1} AND (AddDate BETWEEN '{2}' AND '{3}')", ApplyInfo.TableName, departmentID, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE ProjectID = {1} AND DepartmentID = {2} AND (AddDate BETWEEN '{3}' AND '{4}')", ApplyInfo.TableName, projectID, departmentID, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentIDAndState(int departmentID, EApplyState state)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE DepartmentID = {1} AND State = '{2}'", ApplyInfo.TableName, departmentID, EApplyStateUtils.GetValue(state));
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentIDAndState(int departmentID, int projectID, EApplyState state, DateTime begin, DateTime end)
        {
            string sqlString = string.Empty;
            if (projectID == 0)
            {
                sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE DepartmentID = {1} AND State = '{2}' AND (AddDate BETWEEN '{3}' AND '{4}')", ApplyInfo.TableName, departmentID, EApplyStateUtils.GetValue(state), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE DepartmentID = {1} AND ProjectID = {2} AND State = '{3}' AND (AddDate BETWEEN '{4}' AND '{5}')", ApplyInfo.TableName, departmentID, projectID, EApplyStateUtils.GetValue(state), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectStringByState(int projectID, params EApplyState[] states)
        {
            StringBuilder whereBuilder = new StringBuilder();
            if (projectID > 0)
            {
                whereBuilder.AppendFormat("WHERE ProjectID = {0} AND (", projectID);
            }
            else
            {
                whereBuilder.Append("WHERE (");
            }
            foreach (EApplyState state in states)
            {
                whereBuilder.AppendFormat(" State = '{0}' OR", EApplyStateUtils.GetValue(state));
            }
            whereBuilder.Length -= 2;
            whereBuilder.Append(") ORDER BY ID DESC");
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(ApplyInfo.TableName, SqlUtils.Asterisk, whereBuilder.ToString());
        }

        public string GetSelectString(int projectID)
        {
            string whereString = string.Empty;
            if (projectID > 0)
            {
                whereString = string.Format("WHERE ProjectID = {0}", projectID);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(ApplyInfo.TableName, SqlUtils.Asterisk, whereString);
        }

        public string GetSelectStringToWork(int projectID)
        {
            return GetSelectStringByState(projectID, EApplyState.Accepted, EApplyState.New, EApplyState.Redo, EApplyState.Replied);
        }

        public string GetSelectString(int projectID, string state, int typeID, string addUserName, string userName, string keyword)
        {
            string whereString = string.Empty;
            if (projectID > 0)
            {
                whereString += string.Format(" ProjectID = {0}", projectID);
            }
            if (!string.IsNullOrEmpty(state))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (State = '{0}')", state);
            }
            if (typeID > 0)
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (TypeID = {0})", typeID);
            }
            if (!string.IsNullOrEmpty(addUserName))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (AddUserName = '{0}')", addUserName);
            }
            if (!string.IsNullOrEmpty(userName))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (UserName = '{0}')", userName);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (Title LIKE '{0}' OR Content LIKE '{0}')", keyword);
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(ApplyInfo.TableName, SqlUtils.Asterisk, "WHERE" + whereString);
        }

        public string GetSelectStringToWork(int projectID, string state, int typeID, string addUserName, string userName, string keyword)
        {
            string whereString = string.Empty;
            if (projectID > 0)
            {
                whereString += string.Format(" ProjectID = {0}", projectID);
            }
            if (!string.IsNullOrEmpty(state))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (State = '{0}')", state);
            }
            else
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (State = '{0}' OR  State = '{1}' OR  State = '{2}' OR  State = '{3}') ", EApplyStateUtils.GetValue(EApplyState.Accepted), EApplyStateUtils.GetValue(EApplyState.New), EApplyStateUtils.GetValue(EApplyState.Redo), EApplyStateUtils.GetValue(EApplyState.Replied));
            }
            if (typeID > 0)
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (TypeID = {0})", typeID);
            }
            if (!string.IsNullOrEmpty(addUserName))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (AddUserName = '{0}')", addUserName);
            }
            if (!string.IsNullOrEmpty(userName))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (UserName = '{0}')", userName);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (Title LIKE '{0}' OR Content LIKE '{0}')", keyword);
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(ApplyInfo.TableName, SqlUtils.Asterisk, "WHERE" + whereString);
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
