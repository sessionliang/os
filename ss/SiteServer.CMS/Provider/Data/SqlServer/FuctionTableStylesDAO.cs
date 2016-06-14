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
    public class FunctionTableStylesDAO : DataProviderBase, IFunctionTableStylesDAO
    {
        public string TableName
        {
            get
            {
                return FunctionTableStyles.TableName;
            }
        }

        public int Insert(FunctionTableStyles info)
        {
            int contentID = 0;
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

        public void InsertWhithTrans(FunctionTableStyles info, IDbTransaction trans)
        {
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);

            this.ExecuteNonQuery(trans, SQL_INSERT, parms);

        }


        public void Insert(ArrayList infoList, bool deleteAll)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (deleteAll)
                        {
                            FunctionTableStyles info = infoList[0] as FunctionTableStyles;
                            //删除原有的数据
                            string deleteSql = string.Format(@"DELETE FROM {0} where PublishmentSystemID={1} and NodeID={2} and ContentID={3} and tableStyle='{4}';", TableName, info.PublishmentSystemID, info.NodeID, info.ContentID, info.TableStyle);
                            this.ExecuteNonQuery(trans, deleteSql);
                        }
                        foreach (FunctionTableStyles info in infoList)
                            InsertWhithTrans(info, trans);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(int publishmentSystemID, int nodeID, int contentID, string tableStyle, ArrayList idList)
        {
            IDbDataParameter[] parms = null;
            string SQL_Delete = string.Format(@"DELETE FROM {0} where id in({1}) and PublishmentSystemID={2} and NodeID={3} and ContentID={4} and tableStyle='{5}' ", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idList), publishmentSystemID, nodeID, contentID, tableStyle);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_Delete, parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

        }


        public void Update(FunctionTableStyles info)
        {
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms); ;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

        }

        public ArrayList GetInfoList(int publishmentSystemID, int nodeID, int contentID, string tableStyle, string type)
        {
            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0} and NodeID={1} and ContentID={2} and tableStyle='{3}'  ", publishmentSystemID, nodeID, contentID, tableStyle);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        if (type == "all")
                        {
                            FunctionTableStyles info = new FunctionTableStyles();
                            BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                            list.Add(info);
                        }
                        if (type == "files")
                        {
                            int id = TranslateUtils.ToInt(rdr["TableStyleID"].ToString());
                            list.Add(id);
                        }
                    }
                }
                rdr.Close();
            }

            return list;
        }


        public DataTable getContentAnalysis(string sql)
        {
            return this.ExecuteDataset(sql).Tables[0];
        }

    }
}
