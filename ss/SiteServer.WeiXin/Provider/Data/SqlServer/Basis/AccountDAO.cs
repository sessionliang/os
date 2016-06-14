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
using System.Text;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class AccountDAO : DataProviderBase, IAccountDAO
    {
        private const string TABLE_NAME = "wx_Account";

        public int Insert(AccountInfo accountInfo)
        {
            int accountID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(accountInfo.ToNameValueCollection(), this.ConnectionString, AccountDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        accountID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, AccountDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return accountID;
        }

        public void Update(AccountInfo accountInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(accountInfo.ToNameValueCollection(), this.ConnectionString, AccountDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public AccountInfo GetAccountInfo(int publishmentSystemID)
        {
            AccountInfo accountInfo = null;

            string SQL_WHERE = string.Format("WHERE {0} = {1}", AccountAttribute.PublishmentSystemID, publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AccountDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    accountInfo = new AccountInfo(rdr);
                }
                rdr.Close();
            }

            if (accountInfo == null)
            {
                accountInfo = new AccountInfo();
                accountInfo.PublishmentSystemID = publishmentSystemID;
                accountInfo.Token = StringUtils.GetShortGUID();
                accountInfo.ID = this.Insert(accountInfo);
            }

            return accountInfo;
        }

        public List<AccountInfo> GetAccountInfoList(int publishmentSystemID)
        {
            List<AccountInfo> accountInfoList = new List<AccountInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", AccountAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AccountDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    AccountInfo accountInfo = new AccountInfo(rdr);
                    accountInfoList.Add(accountInfo);
                }
                rdr.Close();
            }

            return accountInfoList;
        }
    }
}
