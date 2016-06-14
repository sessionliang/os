using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Model;
using BaiRong.Model;
using System.Data;
using BaiRong.Core;
using System.Collections;
using SiteServer.B2C.Core;
using BaiRong.Core.Data;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class ConsigneeDAO : DataProviderBase, IConsigneeDAO
    {
        private const string TABLE_NAME = "b2c_Consignee";

        public const string PARM_Consignee_ID = "@ConsigneeID";
        public const string PARM_USER_NAME = "@UserName";
        public const string PARM_IS_ORDER = "@IsOrder";
        public const string PARM_IP_ADDRESS = "@IPAddress";
        public const string PARM_IS_DEFAULT = "@IsDefault";
        public const string PARM_CONSIGNEE = "@Consignee";
        public const string PARM_COUNTRY = "@Country";
        public const string PARM_PROVINCE = "@Province";
        public const string PARM_CITY = "@City";
        public const string PARM_AREA = "@Area";
        public const string PARM_ADDRESS = "@Address";
        public const string PARM_ZIP_CODE = "@Zipcode";
        public const string PARM_MOBILE = "@Mobile";
        public const string PARM_TEL = "@Tel";
        public const string PARM_EMAIL = "@Email";
        public const string PARM_GROUP_SN = "@GroupSN";


        public int Insert(ConsigneeInfo consigneeInfo)
        {
            if (consigneeInfo.IsDefault)
            {
                this.SetAllDefaultToFalse(consigneeInfo.GroupSN, consigneeInfo.UserName);
            }

            int consigneeID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(consigneeInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        consigneeID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return consigneeID;
        }

        public void Update(ConsigneeInfo consigneeInfo)
        {
            if (consigneeInfo.IsDefault)
            {
                this.SetAllDefaultToFalse(consigneeInfo.GroupSN, consigneeInfo.UserName);
            }

            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(consigneeInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int consigneeID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", TABLE_NAME, consigneeID);
            this.ExecuteNonQuery(sqlString);
        }

        public ConsigneeInfo GetConsigneeInfo(int consigneeID)
        {
            ConsigneeInfo consigneeInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", consigneeID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    consigneeInfo = new ConsigneeInfo(rdr);
                }
                rdr.Close();
            }

            return consigneeInfo;
        }

        public List<ConsigneeInfo> GetConsigneeInfoList(string groupSN, string userName)
        {
            // Update By wujq
            List<ConsigneeInfo> list = new List<ConsigneeInfo>();

            string SQL_WHERE = string.Format("WHERE GroupSN = @GroupSN AND UserName = @UserName AND IsOrder = 'false'");

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY IsDefault DESC, ID DESC");

            IDbDataParameter[] selectParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN), 
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)	                    　
                };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, selectParms))
            {
                while (rdr.Read())
                {
                    ConsigneeInfo consigneeInfo = new ConsigneeInfo(rdr);

                    list.Add(consigneeInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public void SetDefault(int consigneeID)
        {
            ConsigneeInfo consigneeInfo = this.GetConsigneeInfo(consigneeID);
            if (consigneeInfo != null)
            {
                this.SetAllDefaultToFalse(consigneeInfo.GroupSN, consigneeInfo.UserName);

                string sqlString = string.Format("UPDATE {0} SET IsDefault = '{1}' WHERE ID = {2}", TABLE_NAME, true.ToString(), consigneeID);

                this.ExecuteNonQuery(sqlString);
            }
        }

        private void SetAllDefaultToFalse(string groupSN, string userName)
        {
            // Update By wujq
            string sqlString = string.Format("UPDATE {0} SET IsDefault = 'false' WHERE GroupSN = @GroupSN AND UserName = @UserName AND IsOrder = 'false'", TABLE_NAME);

            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN), 
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)	                    　
            };

            this.ExecuteNonQuery(sqlString, updateParms);
        }
    }
}
