using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;

namespace BaiRong.Provider.Data.Oracle
{
    public class ContentDAO : BaiRong.Provider.Data.SqlServer.ContentDAO
	{
		protected override string ADOType
		{
			get
			{
				return SqlUtils.ORACLE;
			}
		}

		protected override EDatabaseType DataBaseType
		{
			get
			{
                return EDatabaseType.Oracle;
			}
		}

        public override int GetCountOfContentAdd(string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, DateTime begin, DateTime end, string userName)
        {
            string sqlString = string.Empty;
            if (string.IsNullOrEmpty(userName))
            {
                sqlString = string.Format("SELECT COUNT(ID) AS Num FROM {0} WHERE PublishmentSystemID = {1} AND NodeID IN ({2}) AND (AddDate BETWEEN {3} AND {4})", tableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end.AddDays(1)));
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(ID) AS Num FROM {0} WHERE PublishmentSystemID = {1} AND NodeID IN ({2}) AND (AddDate BETWEEN {3} AND {4}) AND (AddUserName = '{5}')", tableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end.AddDays(1)), userName);
            }

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public override int GetCountOfContentUpdate(string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, DateTime begin, DateTime end, string userName)
        {
            string sqlString = string.Empty;
            if (string.IsNullOrEmpty(userName))
            {
                sqlString = string.Format("SELECT COUNT(ID) AS Num FROM {0} WHERE PublishmentSystemID = {1} AND NodeID IN ({2}) AND (LastEditDate BETWEEN {3} AND {4}) AND (LastEditDate <> AddDate)", tableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end.AddDays(1)));
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(ID) AS Num FROM {0} WHERE PublishmentSystemID = {1} AND NodeID IN ({2}) AND (LastEditDate BETWEEN {3} AND {4}) AND (LastEditDate <> AddDate) AND (AddUserName = '{5}')", tableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end.AddDays(1)), userName);
            }

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public override int GetCountChecked(string tableName, int nodeID, int days)
        {
            string whereString = string.Empty;
            if (days > 0)
            {
                whereString = string.Format("AND (DATEDIFF([Day], AddDate, sysdate) < {0})", days);
            }
            return GetCountChecked(tableName, nodeID, whereString);
        }

        public override ArrayList GetNodeIDArrayListCheckedByLastEditDateHour(string tableName, int publishmentSystemID, int hour)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT DISTINCT NodeID FROM [{0}] WHERE (PublishmentSystemID = {1}) AND (IsChecked = '{2}') AND (LastEditDate BETWEEN sysdate-{3}/24 AND sysdate)", tableName, publishmentSystemID, true.ToString(), hour);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int nodeID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(nodeID);
                }
                rdr.Close();
            }
            return arraylist;
        }
    }
}
