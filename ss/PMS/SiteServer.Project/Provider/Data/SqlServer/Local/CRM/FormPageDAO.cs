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
    public class FormPageDAO : DataProviderBase, IFormPageDAO
	{
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.InnerConnectionString;
            }
        }

        public int Insert(FormPageInfo formPageInfo)
        {
            int formPageID = 0;

            formPageInfo.Taxis = this.GetMaxTaxis(formPageInfo.MobanID) + 1;

            formPageInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(formPageInfo.Attributes, this.ConnectionString, FormPageInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        formPageID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, FormPageInfo.TableName);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return formPageID;
        }

        public void Update(FormPageInfo formPageInfo)
        {
            formPageInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(formPageInfo.Attributes, this.ConnectionString, FormPageInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", FormPageInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public FormPageInfo GetFormPageInfo(NameValueCollection form)
        {
            FormPageInfo formPageInfo = new FormPageInfo(0);

            foreach (string name in form.AllKeys)
            {
                if (FormPageAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        formPageInfo.SetExtendedAttribute(name, value);
                    }
                }
            }

            return formPageInfo;
        }

        public FormPageInfo GetFormPageInfo(int formPageID)
        {
            FormPageInfo formPageInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", formPageID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, FormPageInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    formPageInfo = new FormPageInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, formPageInfo);
                }
                rdr.Close();
            }

            if (formPageInfo != null) formPageInfo.AfterExecuteReader();
            return formPageInfo;
        }

        public int GetNextPageID(int mobanID, int formPageID)
        {
            int pageID = 0;

            List<int> pageIDList = this.GetPageIDList(mobanID);
            bool isCurrent = false;
            foreach (int currentPageID in pageIDList)
            {
                if (isCurrent)
                {
                    pageID = currentPageID;
                    break;
                }
                if (currentPageID == formPageID)
                {
                    isCurrent = true;
                }
            }

            return pageID;
        }

        public bool IsCompleted(int mobanID, int formPageID)
        {
            List<int> pageIDList = this.GetPageIDList(mobanID);
            if (pageIDList.Count > 0)
            {
                return pageIDList[pageIDList.Count - 1] == formPageID;
            }

            return false;
        }

        public Dictionary<int, string> GetPages(int mobanID)
        {
            Dictionary<int, string> retval = new Dictionary<int, string>();

            string SQL_SELECT = string.Format("SELECT ID, Title FROM {0} WHERE MobanID = {1} ORDER BY Taxis", FormPageInfo.TableName, mobanID);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    retval.Add(rdr.GetInt32(0), rdr.GetValue(1).ToString());
                }
                rdr.Close();
            }

            return retval;
        }

        public List<int> GetPageIDList(int mobanID)
        {
            List<int> retval = new List<int>();

            string SQL_SELECT = string.Format("SELECT ID FROM {0} WHERE MobanID = {1} ORDER BY Taxis", FormPageInfo.TableName, mobanID);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    retval.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return retval;
        }

        public int GetCount()
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0}", FormPageInfo.TableName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public IEnumerable GetDataSource(int mobanID)
        {
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(this.ConnectionString, this.GetSelectString(mobanID));
            return enumerable;
        }

        public string GetSelectString(int mobanID)
        {
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, FormPageInfo.TableName, 0, SqlUtils.Asterisk, string.Format("MobanID = {0}", mobanID), this.GetOrderByString());
        }

        public string GetOrderByString()
        {
            return "ORDER BY Taxis";
        }

        public string GetSortFieldName()
        {
            return "ID";
        }

        public bool UpdateTaxisToUp(int id, int mobanID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM crm_FormPage WHERE ((Taxis > (SELECT Taxis FROM crm_FormPage WHERE ID = {0})) AND MobanID ={1}) ORDER BY Taxis", id, mobanID);
            
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

        public bool UpdateTaxisToDown(int id, int mobanID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM crm_FormPage WHERE ((Taxis < (SELECT Taxis FROM crm_FormPage WHERE ID = {0})) AND MobanID = {1}) ORDER BY Taxis DESC", id, mobanID);
            
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

        private int GetMaxTaxis(int mobanID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM crm_FormPage WHERE MobanID = {0}", mobanID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        private int GetTaxis(int id)
        {
            string sqlString = string.Format("SELECT Taxis FROM crm_FormPage WHERE ID = {0}", id);
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
            string sqlString = string.Format("UPDATE crm_FormPage SET Taxis = {0} WHERE ID = {1}", taxis, id);
            this.ExecuteNonQuery(this.ConnectionString, sqlString);
        }
	}
}
