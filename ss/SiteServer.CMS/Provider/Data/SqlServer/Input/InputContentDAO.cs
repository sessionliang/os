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
    public class InputContentDAO : DataProviderBase, IInputContentDAO
    {
        public string TableName
        {
            get
            {
                return "siteserver_InputContent";
            }
        }

        public int Insert(InputContentInfo info)
        {
            int contentID = 0;

            info.Taxis = this.GetMaxTaxis(info.InputID) + 1;
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

                        contentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TableName);

                        //using (IDataReader rdr = this.ExecuteReader(trans, "SELECT @@IDENTITY AS 'ContentID'"))
                        //{
                        //    rdr.Read();
                        //    contentID = Convert.ToInt32(rdr[0].ToString());
                        //    rdr.Close();
                        //}

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return contentID;
        }

        public void Update(InputContentInfo info)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateIsChecked(ArrayList contentIDArrayList)
        {
            string sqlString = string.Format("UPDATE {0} SET IsChecked = '{1}' WHERE ID IN ({2})", TableName, true.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));

            this.ExecuteNonQuery(sqlString);
        }

        public bool UpdateTaxisToUp(int inputID, int contentID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE ((Taxis > (SELECT Taxis FROM {0} WHERE ID = {1})) AND InputID ={2}) ORDER BY Taxis", TableName, contentID, inputID);
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = Convert.ToInt32(rdr[0]);
                    higherTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(contentID);

            if (higherID != 0)
            {
                SetTaxis(contentID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int inputID, int contentID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE ((Taxis < (SELECT Taxis FROM {0} WHERE (ID = {1}))) AND InputID = {2}) ORDER BY Taxis DESC", TableName, contentID, inputID);
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = Convert.ToInt32(rdr[0]);
                    lowerTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(contentID);

            if (lowerID != 0)
            {
                SetTaxis(contentID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        public void Delete(int inputID, ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public void Check(int inputID, ArrayList contentIDArrayList)
        {
            string sqlString = string.Format("UPDATE {0} SET IsChecked = '{1}' WHERE ID IN ({2})", TableName, true.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int inputID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE InputID ={1}", TableName, inputID);
            this.ExecuteNonQuery(sqlString);
        }

        public InputContentInfo GetContentInfo(int contentID)
        {
            InputContentInfo info = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", contentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new InputContentInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public int GetCountChecked(int inputID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (InputID = {1} AND {2} = '{3}')", TableName, inputID, InputContentAttribute.IsChecked, true.ToString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountUnChecked(int inputID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (InputID = {1} AND {2} = '{3}')", TableName, inputID, InputContentAttribute.IsChecked, false.ToString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public DataSet GetDataSetNotChecked(int inputID)
        {
            string whereString = string.Format("WHERE (InputID = {0} AND IsChecked = '{1}') ORDER BY Taxis DESC", inputID, false.ToString());
            return GetDataSetByWhereString(whereString);
        }

        public DataSet GetDataSetWithChecked(int inputID)
        {
            return this.GetDataSetWithChecked(inputID, 0, ETaxisTypeUtils.GetInputContentOrderByString(ETaxisType.OrderByTaxisDesc), "", EScopeType.Self);
        }

        private DataSet GetDataSetByWhereString(string whereString)
        {
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString);
            return this.ExecuteDataset(SQL_SELECT);
        }

        private DataSet GetDataSetWithChecked(int inputID, int contentNum, string orderByString, string whereString, EScopeType scopeType)
        {
            string where = string.Format("WHERE (InputID = {0} AND IsChecked = '{1}' {2}) {3}", inputID, true.ToString(), whereString, orderByString);
            return this.GetDataSetByWhereString(where);
        }

        public IEnumerable GetStlDataSourceChecked(int inputID, int totalNum, string orderByString, string whereString)
        {
            string sqlWhereString = string.Format("WHERE (InputID = {0} AND IsChecked = '{1}' {2})", inputID, true.ToString(), whereString);
            return GetDataSourceByContentNumAndWhereString(totalNum, sqlWhereString, orderByString);
        }

        private IEnumerable GetDataSourceByContentNumAndWhereString(int totalNum, string whereString, string orderByString)
        {
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, totalNum, SqlUtils.Asterisk, whereString, orderByString);
            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }

        public ArrayList GetContentIDArrayListWithChecked(int inputID)
        {
            ArrayList arraylist = new ArrayList();
            string taxisString = ETaxisTypeUtils.GetInputContentOrderByString(ETaxisType.OrderByTaxisDesc);
            string sqlString = string.Format("SELECT ID FROM {0} WHERE (InputID = {1} AND IsChecked = '{2}') {3}", TableName, inputID, true.ToString(), taxisString);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int contentID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(contentID);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetContentIDArrayListWithChecked(int inputID, ArrayList searchFields, string keyword)
        {
            ArrayList contentIDArrayList = new ArrayList();
            string TaxisString = ETaxisTypeUtils.GetInputContentOrderByString(ETaxisType.OrderByTaxisDesc);
            StringBuilder whereStringBuilder = new StringBuilder();
            foreach (string field in searchFields)
            {
                if (!string.IsNullOrEmpty(field))
                {
                    whereStringBuilder.AppendFormat(" {0} LIKE '%{1}%' OR", field, PageUtils.FilterSql(keyword));
                }
            }
            if (whereStringBuilder.Length > 0)
            {
                whereStringBuilder.Remove(whereStringBuilder.Length - 3, 3);
            }

            string sqlString = string.Format("SELECT ID FROM {0} WHERE (InputID = {1} AND IsChecked = '{2}' AND ({3})) {4}", TableName, inputID, true.ToString(), whereStringBuilder.ToString(), TaxisString);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int contentID = Convert.ToInt32(rdr[0]);
                    contentIDArrayList.Add(contentID);
                }
                rdr.Close();
            }
            return contentIDArrayList;
        }

        public ArrayList GetContentIDArrayListByUserName(string userName)
        {
            ArrayList contentIDArrayList = new ArrayList();

            string sqlString = string.Format("SELECT ID FROM {0} WHERE UserName = @UserName ORDER BY AddDate DESC, ID DESC", TableName);

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter("@UserName", EDataType.NVarChar, 255,userName)
			};
            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                while (rdr.Read())
                {
                    int contentID = Convert.ToInt32(rdr[0]);
                    contentIDArrayList.Add(contentID);
                }
                rdr.Close();
            }
            return contentIDArrayList;
        }

        public string GetValue(int contentID, string attributeName)
        {
            string sqlString = string.Format("SELECT [{0}] FROM [{1}] WHERE ([ID] = {2})", attributeName, TableName, contentID);
            return BaiRongDataProvider.DatabaseDAO.GetString(sqlString);
        }

        private int GetTaxis(int contentID)
        {
            string sqlString = string.Format("SELECT Taxis FROM {0} WHERE (ID = {1})", TableName, contentID);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    taxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int contentID, int taxis)
        {
            string sqlString = string.Format("UPDATE {0} SET Taxis = {1} WHERE  ID = {2}", TableName, taxis, contentID);
            this.ExecuteNonQuery(sqlString);
        }

        private int GetMaxTaxis(int inputID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM {0} WHERE InputID = {1}", TableName, inputID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectStringOfContentID(int inputID, string whereString)
        {
            string orderByString = ETaxisTypeUtils.GetInputContentOrderByString(ETaxisType.OrderByTaxisDesc);
            string where = string.Format("WHERE (InputID = {0} {1}) {2}", inputID, whereString, orderByString);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, "ID, Taxis", where);
        }

        public string GetSelectSqlStringWithChecked(int publishmentSystemID, int inputID, bool isReplyExists, bool isReply, int startNum, int totalNum, string whereString, string orderByString, NameValueCollection otherAttributes)
        {
            if (!string.IsNullOrEmpty(whereString) && !StringUtils.StartsWithIgnoreCase(whereString.Trim(), "AND "))
            {
                whereString = "AND " + whereString.Trim();
            }
            string sqlWhereString = string.Format("WHERE InputID = {0} AND IsChecked = '{1}' {2}", inputID, true.ToString(), whereString);
            if (isReplyExists)
            {
                if (isReply)
                {
                    sqlWhereString += string.Format(" AND datalength(Reply) > 0");
                }
                else
                {
                    sqlWhereString += string.Format(" AND datalength(Reply) = 0");
                }
            }
            if (otherAttributes != null && otherAttributes.Count > 0)
            {
                ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.InputContent, publishmentSystemID, inputID);
                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.InputContent, TableName, relatedIdentities);
                foreach (TableStyleInfo tableStyleInfo in styleInfoArrayList)
                {
                    if (!string.IsNullOrEmpty(otherAttributes[tableStyleInfo.AttributeName.ToLower()]))
                    {
                        sqlWhereString += string.Format(" AND ({0} like '%{1}={2}%')", InputContentAttribute.SettingsXML, tableStyleInfo.AttributeName, otherAttributes[tableStyleInfo.AttributeName.ToLower()]);
                    }
                }
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, startNum, totalNum, SqlUtils.Asterisk, sqlWhereString, orderByString);
        }

        public string GetSortFieldName()
        {
            return "Taxis";
        }


        public bool IsExistsPro(int inputId, string uniquePro, string value)
        {
            bool retval = false;

            string sqlString = string.Format("SELECT ID FROM {0} WHERE InputID = {3} AND (SettingsXML like '%{1}={2}%' OR SettingsXML like '{1}={2}%' OR SettingsXML like '%{1}={2}') ", TableName, uniquePro, PageUtils.FilterSql(value), inputId);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int contentID = Convert.ToInt32(rdr[0]);
                    retval = contentID > 0;
                    break;
                }
                rdr.Close();
            }
            return retval;
        }
    }
}
