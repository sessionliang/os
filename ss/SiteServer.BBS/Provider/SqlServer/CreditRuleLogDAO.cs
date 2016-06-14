using System;
using System.Data;
using System.Collections;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class CreditRuleLogDAO : DataProviderBase, ICreditRuleLogDAO
    {
        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_USERNAME = "@UserName";
        private const string PARM_RULE_TYPE = "@RuleType";
        private const string PARM_TOTAL_COUNT = "@TotalCount";
        private const string PARM_PERIOD_COUNT = "@PeriodCount";
        private const string PARM_PRESTIGE = "@Prestige";
        private const string PARM_CONTRIBUTION = "@Contribution";
        private const string PARM_CURRENCY = "@Currency";
        private const string PARM_EXT_CREDIT1 = "@ExtCredit1";
        private const string PARM_EXT_CREDIT2 = "@ExtCredit2";
        private const string PARM_EXT_CREDIT3 = "@ExtCredit3";
        private const string PARM_LAST_DATE = "@LastDate";

        public void Insert(CreditRuleLogInfo info)
        {
            string sqlString = "INSERT INTO bbs_CreditRuleLog (PublishmentSystemID, UserName, RuleType, TotalCount, PeriodCount, Prestige, Contribution, Currency, ExtCredit1, ExtCredit2, ExtCredit3, LastDate) VALUES (@PublishmentSystemID, @UserName, @RuleType, @TotalCount, @PeriodCount, @Prestige, @Contribution, @Currency, @ExtCredit1, @ExtCredit2, @ExtCredit3, @LastDate)";

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID),
				this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, info.UserName),
                this.GetParameter(PARM_RULE_TYPE, EDataType.VarChar, 50, ECreditRuleUtils.GetValue(info.RuleType)),
                this.GetParameter(PARM_TOTAL_COUNT, EDataType.Integer, info.TotalCount),
                this.GetParameter(PARM_PERIOD_COUNT, EDataType.Integer, info.PeriodCount),
                this.GetParameter(PARM_PRESTIGE, EDataType.Integer, info.Prestige),
                this.GetParameter(PARM_CONTRIBUTION, EDataType.Integer, info.Contribution),
                this.GetParameter(PARM_CURRENCY, EDataType.Integer, info.Currency),
                this.GetParameter(PARM_EXT_CREDIT1, EDataType.Integer, info.ExtCredit1),
                this.GetParameter(PARM_EXT_CREDIT2, EDataType.Integer, info.ExtCredit2),
                this.GetParameter(PARM_EXT_CREDIT3, EDataType.Integer, info.ExtCredit3),
                this.GetParameter(PARM_LAST_DATE, EDataType.DateTime, info.LastDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Update(CreditRuleLogInfo info)
        {
            string sqlString = "UPDATE bbs_CreditRuleLog SET UserName = @UserName, RuleType = @RuleType, TotalCount = @TotalCount, PeriodCount = @PeriodCount, Prestige = @Prestige, Contribution = @Contribution, Currency = @Currency, ExtCredit1 = @ExtCredit1, ExtCredit2 = @ExtCredit2, ExtCredit3 = @ExtCredit3, LastDate = @LastDate WHERE ID = @ID";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, info.UserName),
                this.GetParameter(PARM_RULE_TYPE, EDataType.VarChar, 50, ECreditRuleUtils.GetValue(info.RuleType)),
                this.GetParameter(PARM_TOTAL_COUNT, EDataType.Integer, info.TotalCount),
                this.GetParameter(PARM_PERIOD_COUNT, EDataType.Integer, info.PeriodCount),
                this.GetParameter(PARM_PRESTIGE, EDataType.Integer, info.Prestige),
                this.GetParameter(PARM_CONTRIBUTION, EDataType.Integer, info.Contribution),
                this.GetParameter(PARM_CURRENCY, EDataType.Integer, info.Currency),
                this.GetParameter(PARM_EXT_CREDIT1, EDataType.Integer, info.ExtCredit1),
                this.GetParameter(PARM_EXT_CREDIT2, EDataType.Integer, info.ExtCredit2),
                this.GetParameter(PARM_EXT_CREDIT3, EDataType.Integer, info.ExtCredit3),
                this.GetParameter(PARM_LAST_DATE, EDataType.DateTime, info.LastDate),
				this.GetParameter(PARM_ID, EDataType.Integer, info.ID)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public ArrayList GetCreditRuleLogInfoArrayList(int publishmentSystemID, string userName)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, UserName, RuleType, TotalCount, PeriodCount, Prestige, Contribution, Currency, ExtCredit1, ExtCredit2, ExtCredit3, LastDate FROM bbs_CreditRuleLog WHERE PublishmentSystemID = {0} AND UserName = @UserName", publishmentSystemID);

            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)                
			};
            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                while (rdr.Read())
                {
                    CreditRuleLogInfo logInfo = new CreditRuleLogInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), ECreditRuleUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetInt32(10), rdr.GetInt32(11), rdr.GetDateTime(12));
                    arraylist.Add(logInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public CreditRuleLogInfo GetCreditRuleLogInfo(int publishmentSystemID, string userName, ECreditRule ruleType)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, UserName, RuleType, TotalCount, PeriodCount, Prestige, Contribution, Currency, ExtCredit1, ExtCredit2, ExtCredit3, LastDate FROM bbs_CreditRuleLog WHERE PublishmentSystemID = {0} AND UserName = @UserName AND RuleType = '{1}'", publishmentSystemID, ECreditRuleUtils.GetValue(ruleType));

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)                
			};

            CreditRuleLogInfo logInfo = null;

            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                if (rdr.Read())
                {
                    logInfo = new CreditRuleLogInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), ECreditRuleUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetInt32(10), rdr.GetInt32(11), rdr.GetDateTime(12));
                }
                rdr.Close();
            }

            return logInfo;
        }

        public void Delete(int publishmentSystemID, string userName)
        {
            string deleteSqlString = string.Format("DELETE bbs_CreditRuleLog WHERE PublishmentSystemID = {0} AND UserName = @UserName", publishmentSystemID);
            IDbDataParameter[] deleteParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)                
			};
            this.ExecuteNonQuery(deleteSqlString, deleteParms);
        }

        public string GetSqlString(int publishmentSystemID, string userName)
        {
            return string.Format("SELECT ID, PublishmentSystemID, UserName, RuleType, TotalCount, PeriodCount, Prestige, Contribution, Currency, ExtCredit1, ExtCredit2, ExtCredit3, LastDate FROM bbs_CreditRuleLog WHERE PublishmentSystemID = {0} AND UserName = '{1}'", publishmentSystemID, userName);
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
    }
}