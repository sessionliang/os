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
    public class ConsoleLogDAO : DataProviderBase, IConsoleLogDAO
    {
        public string TableName
        {
            get
            {
                return ConsoleLogInfo.TableNameStr;
            }
        }

        public int Insert(ConsoleLogInfo info)
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

        public void InsertWhithTrans(ConsoleLogInfo info, IDbTransaction trans)
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
                        foreach (ConsoleLogInfo info in infoList)
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

        public ArrayList GetInfoList(int publishmentSystemID, int nodeID, int contentID, string actionType, string type)
        {
            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0} and NodeID={1} and ContentID={2} and ActionType='{3}'  ", publishmentSystemID, nodeID, contentID, actionType);
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
                            ConsoleLogInfo info = new ConsoleLogInfo();
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


        public ArrayList GetInfoList(string actionType, string userName)
        {
            string SQL_WHERE = string.Format("WHERE ActionType='{0}' and UserName'{1}'  ", actionType, userName);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        FunctionTableStyles info = new FunctionTableStyles();
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                        list.Add(info);

                    }
                }
                rdr.Close();
            }

            return list;
        }

        public ArrayList GetInfoList(string actionType, string userName, string startdate, string enddate, int pageIndex, int prePageNum)
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
            whereString.AppendFormat(" WHERE UserName = '{0}'", userName);
            whereString.Append(dateString);
            whereString.AppendFormat(" and ActionType = '{0}'", actionType);

            //StringBuilder SQL_SELECT = new StringBuilder();
            //SQL_SELECT.AppendFormat(" SELECT tmp.* from ( ");
            //SQL_SELECT.AppendFormat(" SELECT *, ROW_NUMBER() OVER(ORDER BY IsViewed, AddDate DESC) as rowNum FROM {0} WHERE {1} ", TableName, whereString.ToString());
            //SQL_SELECT.AppendFormat(" ) as tmp ");
            //SQL_SELECT.AppendFormat(" WHERE tmp.rowNum >= {0} and tmp.rowNum <= {1} ", (pageIndex - 1) * prePageNum + 1, pageIndex * prePageNum);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, (pageIndex - 1) * prePageNum + 1, pageIndex * prePageNum, SqlUtils.Asterisk, whereString.ToString(), "ORDER BY AddDate DESC");



            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT.ToString()))
            {
                while (rdr.Read())
                {
                    ConsoleLogInfo info = new ConsoleLogInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    list.Add(info);
                }
                rdr.Close();
            }

            return list;
        }



        public int GetCount(string actionType, string userName)
        {
            int count = 0;
            string SQL_WHERE = string.Format("WHERE ActionType='{0}' and UserName = '{1}'  ", actionType, userName);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, "COUNT(*)", SQL_WHERE);

            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        count = TranslateUtils.ToInt(rdr[0].ToString());
                    }
                }
                rdr.Close();
            }

            return count;
        }
    }
}
