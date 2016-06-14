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
    public class MLibScopeDAO : DataProviderBase, IMLibScopeDAO
    {
        public string TableName
        {
            get
            {
                return MLibScopeInfo.TableName;
            }
        }

        public int Insert(MLibScopeInfo info)
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

        public void InsertWhithTrans(MLibScopeInfo info, IDbTransaction trans)
        {
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);

            this.ExecuteNonQuery(trans, SQL_INSERT, parms);

        }


        public void Insert(ArrayList infoList)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //删除原有的数据
                        string deleteSql = string.Format(@"DELETE FROM {0};", TableName);
                        this.ExecuteNonQuery(trans, deleteSql);

                        foreach (MLibScopeInfo info in infoList)
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


        public void Update(MLibScopeInfo info)
        {
            IDbDataParameter[] parms = null;
            string SQL_INSERT = string.Format(@" UPDATE {0} SET [ContentNum] = {1},[IsChecked] = '{2}' ,[Field] = '{3}',[AddDate] = '{4}' ,[UserName] = '{5}' ,[SetXML]='{6}' Where {7}={8} and {9}={10}", MLibScopeInfo.TableName, info.ContentNum, info.IsChecked, info.Field, info.AddDate, info.UserName, info.SetXML, MLibScopeAttribute.PublishmentSystemID, info.PublishmentSystemID, MLibScopeAttribute.NodeID, info.NodeID);

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

        public MLibScopeInfo GetMLibScopeInfo(int publishmentSystemID, int nodeID)
        {
            MLibScopeInfo info = null;
            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0} AND NodeID={1}", publishmentSystemID, nodeID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new MLibScopeInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public ArrayList GetInfoList(int publishmentSystemID)
        {
            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0} ", publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    MLibScopeInfo info = new MLibScopeInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    list.Add(info);
                }
                rdr.Close();
            }

            return list;
        }


        public ArrayList GetInfoList()
        {
            string SQL_WHERE = string.Format("WHERE 1=1");
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    MLibScopeInfo info = new MLibScopeInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    list.Add(info);
                }
                rdr.Close();
            }

            return list;
        }
    }
}
