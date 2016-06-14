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
    public class MLibDraftContentDAO : DataProviderBase, IMLibDraftContentDAO
    {
        public string TableName
        {
            get
            {
                return MLibDraftContentInfo.TableName;
            }
        }

        public int Insert(MLibDraftContentInfo info)
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

        public void InsertWhithTrans(MLibDraftContentInfo info, IDbTransaction trans)
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

                        foreach (MLibDraftContentInfo info in infoList)
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

        public void Delete(ArrayList idList)
        {
            IDbDataParameter[] parms = null;
            string SQL_Delete = string.Format(@"DELETE FROM {0} where id in({1})", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idList));

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


        public void Update(MLibDraftContentInfo info)
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

        public MLibDraftContentInfo GetMLibDraftContentInfo(int id)
        {
            MLibDraftContentInfo info = null;
            string SQL_WHERE = string.Format("WHERE  ID={0}", id);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new MLibDraftContentInfo();
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
                    MLibDraftContentInfo info = new MLibDraftContentInfo();
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
                    MLibDraftContentInfo info = new MLibDraftContentInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    list.Add(info);
                }
                rdr.Close();
            }

            return list;
        }

        /// <summary>
        /// 获取草稿
        /// </summary>
        /// <param name="addUserName"></param>
        /// <param name="title"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="prePageNum"></param>
        /// <returns></returns>
        public ArrayList GetUserMLibDraftContentList(string addUserName, string title, string startdate, string enddate, int pageIndex, int prePageNum)
        {
            string dateString = string.Empty;
            if (!string.IsNullOrEmpty(startdate))
            {
                dateString = string.Format(" AND AddDate >= '{0}' ", startdate);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString = string.Format(" AND to_char(AddDate,'YYYY-MM-DD') >= '{0}' ", startdate);
                }
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                enddate = DateUtils.GetDateString(TranslateUtils.ToDateTime(enddate).AddDays(1));
                dateString += string.Format(" AND AddDate <= '{0}' ", enddate);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString += string.Format(" AND to_char(AddDate,'YYYY-MM-DD') <= '{0}' ", enddate);
                }
            }
            StringBuilder whereString = new StringBuilder();
            whereString.AppendFormat(" WHERE AddUserName = '{0}'", addUserName);
            whereString.Append(dateString);
            if (!string.IsNullOrEmpty(title))
            {
                whereString.AppendFormat(" AND Title like '%{0}%'", title);
            }

            StringBuilder SQL_SELECT = new StringBuilder();

            SQL_SELECT.AppendFormat(" SELECT tmp.* from ( ");
            SQL_SELECT.AppendFormat(" SELECT *, ROW_NUMBER() OVER(ORDER BY IsViewed, AddDate DESC) as rowNum FROM {0} WHERE {1} ", TableName, whereString.ToString());
            SQL_SELECT.AppendFormat(" ) as tmp ");
            SQL_SELECT.AppendFormat(" WHERE tmp.rowNum >= {0} and tmp.rowNum <= {1} ", (pageIndex - 1) * prePageNum + 1, pageIndex * prePageNum);

            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT.ToString()))
            {
                while (rdr.Read())
                {
                    MLibDraftContentInfo info = new MLibDraftContentInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    list.Add(info);
                }
                rdr.Close();
            }

            return list;
        }
    }
}
