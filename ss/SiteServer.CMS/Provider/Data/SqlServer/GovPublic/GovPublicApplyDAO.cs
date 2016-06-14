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
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;


namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class GovPublicApplyDAO : DataProviderBase, IGovPublicApplyDAO
	{
        public string TableName
        {
            get
            {
                return "siteserver_GovPublicApply";
            }
        }

        public int Insert(GovPublicApplyInfo info)
        {
            int applyID = 0;

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        applyID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TableName);

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

        public void Update(GovPublicApplyInfo info)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateState(int applyID, EGovPublicApplyState state)
        {
            string sqlString = string.Format("UPDATE {0} SET State = '{1}' WHERE ID = {2}", TableName, EGovPublicApplyStateUtils.GetValue(state), applyID);
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateDepartmentID(int applyID, int departmentID)
        {
            string sqlString = string.Format("UPDATE {0} SET DepartmentID = {1} WHERE ID = {2}", TableName, departmentID, applyID);
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateDepartmentID(ArrayList idCollection, int departmentID)
        {
            string sqlString = string.Format("UPDATE {0} SET DepartmentID = {1} WHERE ID IN ({2})", TableName, departmentID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idCollection));

            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int styleID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE StyleID ={1}", TableName, styleID);
            this.ExecuteNonQuery(sqlString);
        }

        public GovPublicApplyInfo GetApplyInfo(int publishmentSystemID, int styleID, NameValueCollection form)
        {
            string queryCode = GovPublicApplyManager.GetQueryCode();
            int departmentID = TranslateUtils.ToInt(form[GovPublicApplyAttribute.DepartmentID]);
            string departmentName = string.Empty;
            if (departmentID > 0)
            {
                departmentName = DepartmentManager.GetDepartmentName(departmentID);
            }
            GovPublicApplyInfo applyInfo = new GovPublicApplyInfo(0, styleID, publishmentSystemID, TranslateUtils.ToBool(form[GovPublicApplyAttribute.IsOrganization], false), form[GovPublicApplyAttribute.Title], departmentName, departmentID, DateTime.Now, queryCode, EGovPublicApplyState.New);

            foreach (string name in form.AllKeys)
            {
                if (GovPublicApplyAttribute.BasicAttributes.Contains(name.ToLower()))
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

        public GovPublicApplyInfo GetApplyInfo(int applyID)
        {
            GovPublicApplyInfo info = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", applyID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new GovPublicApplyInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public GovPublicApplyInfo GetApplyInfo(int publishmentSystemID, bool isOrganization, string queryName, string queryCode)
        {
            GovPublicApplyInfo info = null;
            string nameAttribute = GovPublicApplyAttribute.CivicName;
            if (isOrganization)
            {
                nameAttribute = GovPublicApplyAttribute.OrgName;
            }
            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0} AND {1} = '{2}' AND QueryCode = '{3}'", publishmentSystemID, nameAttribute, queryName, queryCode);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new GovPublicApplyInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public EGovPublicApplyState GetState(int applyID)
        {
            EGovPublicApplyState state = EGovPublicApplyState.New;
            string sqlString = string.Format("SELECT State FROM {0} WHERE ID = {1}", TableName, applyID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    state = EGovPublicApplyStateUtils.GetEnumType(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }
            return state;
        }

        public int GetCountByStyleID(int styleID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (StyleID = {1})", TableName, styleID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByPublishmentSystemID(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1}", TableName, publishmentSystemID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentID(int publishmentSystemID, int departmentID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND DepartmentID = {2}", TableName, publishmentSystemID, departmentID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentID(int publishmentSystemID, int departmentID, DateTime begin, DateTime end)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND DepartmentID = {2} AND (AddDate BETWEEN '{3}' AND '{4}')", TableName, publishmentSystemID, departmentID, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentIDAndState(int publishmentSystemID, int departmentID, EGovPublicApplyState state)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND DepartmentID = {2} AND State = '{3}'", TableName, publishmentSystemID, departmentID, EGovPublicApplyStateUtils.GetValue(state));
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentIDAndState(int publishmentSystemID, int departmentID, EGovPublicApplyState state, DateTime begin, DateTime end)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND DepartmentID = {2} AND State = '{3}' AND (AddDate BETWEEN '{4}' AND '{5}')", TableName, publishmentSystemID, departmentID, EGovPublicApplyStateUtils.GetValue(state), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectStringByState(int publishmentSystemID, params EGovPublicApplyState[] states)
        {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.AppendFormat("WHERE PublishmentSystemID = {0} AND (", publishmentSystemID);
            foreach (EGovPublicApplyState state in states)
            {
                whereBuilder.AppendFormat(" State = '{0}' OR", EGovPublicApplyStateUtils.GetValue(state));
            }
            whereBuilder.Length -= 2;
            whereBuilder.Append(") ORDER BY ID DESC");
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereBuilder.ToString());
        }

        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE PublishmentSystemID = {0}", publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString);
        }

        public string GetSelectString(int publishmentSystemID, string state, string keyword)
        {
            string whereString = string.Format("WHERE PublishmentSystemID = {0}", publishmentSystemID);
            if (!string.IsNullOrEmpty(state))
            {
                whereString += string.Format(" AND (State = '{0}')", state);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                whereString += string.Format(" AND (Title LIKE '{0}' OR Content LIKE '{0}')", keyword);
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString);
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
