using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;
using ECountType = SiteServer.WeiXin.Model.ECountType;
using ECountTypeUtils = SiteServer.WeiXin.Model.ECountTypeUtils;
using BaiRong.Model;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class CountDAO : DataProviderBase, SiteServer.WeiXin.Core.ICountDAO
    {
        private const string TABLE_NAME = "wx_Count";


        public int Insert(CountInfo countInfo)
        {
            int countID = 0;

            string sqlString = @"INSERT INTO wx_Count (PublishmentSystemID, CountYear, CountMonth, CountDay, CountType, Count) VALUES (@PublishmentSystemID, @CountYear, @CountMonth, @CountDay, @CountType, @Count)";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, countInfo.PublishmentSystemID),
                this.GetParameter("@CountYear", EDataType.Integer, countInfo.CountYear),
                this.GetParameter("@CountMonth", EDataType.Integer, 255, countInfo.CountMonth),
                this.GetParameter("@CountDay", EDataType.Integer, 200, countInfo.CountDay),        
                this.GetParameter("@CountType", EDataType.VarChar, 50, ECountTypeUtils.GetValue(countInfo.CountType)), 
                this.GetParameter("@Count", EDataType.Integer, countInfo.Count),
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        countID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "wx_Count");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return countID;
        }
       

        public void AddCount(int publishmentSystemID, ECountType countType)
        {
            int count = this.GetCount(publishmentSystemID, countType);
            DateTime now = DateTime.Now;

            if (count == 0)
            {
                string sqlString = "INSERT INTO wx_Count (PublishmentSystemID, CountYear, CountMonth, CountDay, CountType, Count) VALUES ({0}, {1}, {2}, {3}, '{4}', 1)";
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    sqlString = "INSERT INTO wx_Count(CountID, PublishmentSystemID, CountYear, CountMonth, CountDay, CountType, Count) VALUES (wx_Count_SEQ.NEXTVAL, {0}, {1}, {2}, {3}, '{4}', 1)";
                }

                this.ExecuteNonQuery(string.Format(sqlString, publishmentSystemID, now.Year, now.Month, now.Day, ECountTypeUtils.GetValue(countType)));
            }
            else
            {
                string sqlString = string.Format("UPDATE wx_Count SET Count = Count + 1 WHERE PublishmentSystemID = {0} AND CountYear = {1} AND CountMonth = {2} AND CountDay = {3} AND CountType = '{4}'", publishmentSystemID, now.Year, now.Month, now.Day, ECountTypeUtils.GetValue(countType));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public int GetCount(int publishmentSystemID, ECountType countType)
        {
            int count = 0;

            DateTime now = DateTime.Now;
            string sqlString = string.Format("SELECT Count FROM wx_Count WHERE PublishmentSystemID = {0} AND CountYear = {1} AND CountMonth = {2} AND CountDay = {3} AND CountType = '{4}'", publishmentSystemID, now.Year, now.Month, now.Day, ECountTypeUtils.GetValue(countType));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    count = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return count;
        }

        public int GetCount(int publishmentSystemID, int year, int month, ECountType countType)
        {
            int count = 0;

            string sqlString = string.Format("SELECT Count FROM wx_Count WHERE PublishmentSystemID = {0} AND CountYear = {1} AND CountMonth = {2} AND CountType = '{3}'", publishmentSystemID, year, month, ECountTypeUtils.GetValue(countType));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    count = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return count;
        }

        public Dictionary<int, int> GetDayCount(int publishmentSystemID, int year, int month, ECountType countType)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            string sqlString = string.Format("SELECT CountDay, Count FROM wx_Count WHERE PublishmentSystemID = {0} AND CountYear = {1} AND CountMonth = {2} AND CountType = '{3}'", publishmentSystemID, year, month, ECountTypeUtils.GetValue(countType));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    int day = rdr.GetInt32(0);
                    int count = rdr.GetInt32(1);
                    dictionary[day] = count;
                }
                rdr.Close();
            }

            return dictionary;
        }

        public List<CountInfo> GetCountInfoList(int publishmentSystemID)
        {
            List<CountInfo> countInfoList = new List<CountInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", CountAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CountDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CountInfo countInfo = new CountInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4), ECountTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), rdr.GetInt32(6));
                    countInfoList.Add(countInfo);
                }
                rdr.Close();
            }

            return countInfoList;
        }
    }
}