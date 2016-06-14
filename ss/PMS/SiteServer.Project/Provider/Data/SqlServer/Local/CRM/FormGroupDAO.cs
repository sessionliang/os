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
using SiteServer.Project.Model;
using SiteServer.Project.Core;

using System.Collections.Generic;

namespace SiteServer.Project.Provider.Data.SqlServer
{
    public class FormGroupDAO : DataProviderBase, IFormGroupDAO
    {
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.InnerConnectionString;
            }
        }

        public int Insert(FormGroupInfo formGroupInfo)
        {
            int formGroupID = 0;

            formGroupInfo.Taxis = this.GetMaxTaxis(formGroupInfo.PageID) + 1;

            formGroupInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(formGroupInfo.Attributes, this.ConnectionString, FormGroupInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        formGroupID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, FormGroupInfo.TableName);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return formGroupID;
        }

        public void Update(FormGroupInfo formGroupInfo)
        {
            formGroupInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(formGroupInfo.Attributes, this.ConnectionString, FormGroupInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", FormGroupInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public FormGroupInfo GetFormGroupInfo(NameValueCollection form)
        {
            FormGroupInfo formGroupInfo = new FormGroupInfo(0);

            foreach (string name in form.AllKeys)
            {
                if (FormGroupAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        formGroupInfo.SetExtendedAttribute(name, value);
                    }
                }
            }

            return formGroupInfo;
        }

        public FormGroupInfo GetFormGroupInfo(int formGroupID)
        {
            FormGroupInfo formGroupInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", formGroupID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, FormGroupInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    formGroupInfo = new FormGroupInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, formGroupInfo);
                }
                rdr.Close();
            }

            if (formGroupInfo != null) formGroupInfo.AfterExecuteReader();
            return formGroupInfo;
        }

        public List<FormGroupInfo> GetFormGroupInfoList(int pageID)
        {
            List<FormGroupInfo> list = new List<FormGroupInfo>();

            string SQL_WHERE = string.Format("WHERE PageID = {0}", pageID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, FormGroupInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, this.GetOrderByString());

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    FormGroupInfo formGroupInfo = new FormGroupInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, formGroupInfo);
                    formGroupInfo.AfterExecuteReader();
                    list.Add(formGroupInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public int GetCount()
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0}", FormGroupInfo.TableName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public IEnumerable GetDataSource(int pageID)
        {
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(this.ConnectionString, this.GetSelectString(pageID));
            return enumerable;
        }

        public string GetSelectString(int pageID)
        {
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, FormGroupInfo.TableName, 0, SqlUtils.Asterisk, string.Format("PageID = {0}", pageID), this.GetOrderByString());
        }

        public string GetOrderByString()
        {
            return "ORDER BY Taxis";
        }

        public string GetSortFieldName()
        {
            return "ID";
        }

        public bool UpdateTaxisToUp(int id, int pageID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM crm_FormGroup WHERE ((Taxis > (SELECT Taxis FROM crm_FormGroup WHERE ID = {0})) AND PageID ={1}) ORDER BY Taxis", id, pageID);
            
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(this.ConnectionString, sqlString))
            {
                if (rdr.Read())
                {
                    higherID = rdr.GetInt32(0);
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(id);

            if (higherID > 0)
            {
                SetTaxis(id, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int id, int pageID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM crm_FormGroup WHERE ((Taxis < (SELECT Taxis FROM crm_FormGroup WHERE ID = {0})) AND PageID = {1}) ORDER BY Taxis DESC", id, pageID);
            
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(this.ConnectionString, sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = rdr.GetInt32(0);
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(id);

            if (selectedTaxis > 0)
            {
                SetTaxis(id, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int pageID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM crm_FormGroup WHERE PageID = {0}", pageID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        private int GetTaxis(int id)
        {
            string sqlString = string.Format("SELECT Taxis FROM crm_FormGroup WHERE ID = {0}", id);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(this.ConnectionString, sqlString))
            {
                if (rdr.Read())
                {
                    taxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int id, int taxis)
        {
            string sqlString = string.Format("UPDATE crm_FormGroup SET Taxis = {0} WHERE ID = {1}", taxis, id);
            this.ExecuteNonQuery(this.ConnectionString, sqlString);
        }
	}
}
