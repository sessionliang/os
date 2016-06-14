using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;

namespace BaiRong.Provider.Data.SqlServer
{
    public class IP2CityDAO : DataProviderBase, IIP2CityDAO
	{
        public IP2CityDAO()
		{
		}

        private const string PARM_ID = "@ID";
        private const string PARM_START_NUM = "@StartNum";
        private const string PARM_END_NUM = "@EndNum";
        private const string PARM_PROVINCE = "@Province";
        private const string PARM_CITY = "@City";

        public SortedList GetCityWithNumSortedList(ArrayList ipAddressArrayList)
        {
            SortedList sortedlist = new SortedList();
            if (ipAddressArrayList != null && ipAddressArrayList.Count > 0)
            {
                using (IDbConnection conn = this.GetConnection())
                {
                    conn.Open();
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        foreach (string ipAddress in ipAddressArrayList)
                        {
                            string city = GetCity(ipAddress, trans);
                            if (sortedlist[city] != null)
                            {
                                sortedlist[city] = (int)sortedlist[city] + 1;
                            }
                            else
                            {
                                sortedlist[city] = 1;
                            }
                        }
                    }
                }
            }
            return sortedlist;
        }

        public string GetCity(string ipAddress)
        {
            string city = string.Empty;
            if (!string.IsNullOrEmpty(ipAddress))
            {
                using (IDbConnection conn = this.GetConnection())
                {
                    conn.Open();
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        city = GetCity(ipAddress, trans);
                    }
                }
            }
            return city;
        }

        private string GetCity(string ipAddress, IDbTransaction trans)
        {
            string retval = string.Empty;
            if (ipAddress == "127.0.0.1")
            {
                retval = "±¾»ú";
            }
            else
            {
                string sqlString = string.Format(@"SELECT TOP 1 Province, City as Num FROM bairong_IP2City WHERE {0} >= StartNum AND {0} <= EndNum ORDER BY ID DESC", GetIPNum(ipAddress));
                using (IDataReader rdr = this.ExecuteReader(trans, sqlString))
                {
                    if (rdr.Read())
                    {
                        string province = rdr.GetValue(0).ToString();
                        string city = rdr.GetValue(1).ToString();
                        if (province == city)
                        {
                            retval = city;
                        }
                        else
                        {
                            retval = rdr.GetValue(0).ToString() + " " + rdr.GetValue(1).ToString();
                        }
                    }
                    rdr.Close();
                }
            }
            return retval;
        }

        private long GetIPNum(string ipAddress)
        {
            long num = 0;
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] arr = ipAddress.Split('.');
                if (arr.Length == 4)
                {
                    num = Convert.ToInt64(arr[0]) * 256 * 256 * 256 + Convert.ToInt64(arr[1]) * 256 * 256 + Convert.ToInt64(arr[2]) * 256 + Convert.ToInt32(arr[3]);
                }
            }
            
            return num;
        }

        public void TranslateIP2City()
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        OleDbConnection aConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathUtils.MapPath("~/SiteFiles/Data.asax"));
                        OleDbCommand aCommand = new OleDbCommand("SELECT StartNum, EndNum, Province, City FROM bairong_IP2City ORDER BY ID", aConnection);
                        aConnection.Open();
                        OleDbDataReader rdr = aCommand.ExecuteReader();

                        while (rdr.Read())
                        {
                            if (!rdr.IsDBNull(0))
                            {
                                string sqlString = string.Format("INSERT INTO bairong_IP2City(StartNum, EndNum, Province, City) VALUES ({0}, {1}, '{2}', '{3}')", rdr.GetDouble(0), rdr.GetDouble(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString());
                                if (this.DataBaseType == EDatabaseType.Oracle)
                                {
                                    sqlString = string.Format("INSERT INTO bairong_IP2City(ID, StartNum, EndNum, Province, City) VALUES (bairong_IP2City_SEQ.NEXTVAL, {0}, {1}, '{2}', '{3}')", rdr.GetDouble(0), rdr.GetDouble(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString());
                                }

                                this.ExecuteNonQuery(trans, sqlString);
                            }
                        }

                        rdr.Close();
                        aConnection.Close();

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                }
            }
        }

    }
}
