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

using System.Collections.Generic;

namespace SiteServer.CRM.Provider.Data.SqlServer
{
    public class MobanDAO : DataProviderBase, IMobanDAO
	{
        public int Insert(MobanInfo mobanInfo)
        {
            int mobanID = 0;

            mobanInfo.SN = mobanInfo.SN.ToUpper().Trim();

            mobanInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(mobanInfo.Attributes, MobanInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        mobanID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, MobanInfo.TableName);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return mobanID;
        }

        public void Update(MobanInfo mobanInfo)
        {
            mobanInfo.SN = mobanInfo.SN.ToUpper().Trim();

            mobanInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(mobanInfo.Attributes, MobanInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", MobanInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public MobanInfo GetMobanInfo(NameValueCollection form)
        {
            MobanInfo mobanInfo = new MobanInfo(0);

            foreach (string name in form.AllKeys)
            {
                if (MobanAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        mobanInfo.SetExtendedAttribute(name, value.Trim());
                    }
                }
            }

            return mobanInfo;
        }

        public MobanInfo GetMobanInfo(int mobanID)
        {
            MobanInfo mobanInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", mobanID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(MobanInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    mobanInfo = new MobanInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, mobanInfo);
                }
                rdr.Close();
            }

            if (mobanInfo != null) mobanInfo.AfterExecuteReader();
            return mobanInfo;
        }

        public int GetMobanID(string sn)
        {
            if (!string.IsNullOrEmpty(sn))
            {
                string sqlString = string.Format("SELECT ID FROM {0} WHERE SN = '{1}'", MobanInfo.TableName, sn.ToUpper().Trim());
                return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
            }
            return 0;
        }

        public bool IsInitializationForm(string sn)
        {
            bool isInitializationForm = false;

            if (!string.IsNullOrEmpty(sn))
            {
                string sqlString = string.Format("SELECT IsInitializationForm FROM {0} WHERE SN = '{1}'", MobanInfo.TableName, sn.ToUpper().Trim());
                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        isInitializationForm = TranslateUtils.ToBool(rdr.GetValue(0).ToString());
                    }
                    rdr.Close();
                }
            }

            return isInitializationForm;
        }

        public MobanInfo GetMobanInfo(string sn)
        {
            MobanInfo mobanInfo = null;
            string SQL_WHERE = string.Format("WHERE SN = '{0}'", sn.ToUpper().Trim());
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(MobanInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    mobanInfo = new MobanInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, mobanInfo);
                }
                rdr.Close();
            }

            if (mobanInfo != null) mobanInfo.AfterExecuteReader();
            return mobanInfo;
        }

        public string GetMobanUrl(MobanInfo mobanInfo)
        {
            return string.Format("http://{0}.moban.siteserver.cn/{1}/", mobanInfo.Category.ToLower(), StringUtils.ReplaceFirst(mobanInfo.Category.ToLower(), mobanInfo.SN.ToLower(), string.Empty));
        }

        public int GetCount()
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0}", MobanInfo.TableName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectString()
        {
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(MobanInfo.TableName, 0, SqlUtils.Asterisk, null, null);
        }

        public string GetSelectString(string sn, string keyword)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(sn))
            {
                whereString += string.Format(" SN = '{0}'", sn);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (Category LIKE '%{0}%' OR SN LIKE '%{0}%' OR Industry LIKE '%{0}%' OR Color LIKE '%{0}%' OR Summary LIKE '%{0}%') ", keyword);
            }

            if (!string.IsNullOrEmpty(whereString))
            {
                whereString = "WHERE" + whereString;
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(MobanInfo.TableName, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetOrderByString()
        {
            return "ORDER BY ID DESC";
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
