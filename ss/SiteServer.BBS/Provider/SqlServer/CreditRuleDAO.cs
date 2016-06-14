using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using BaiRong.Core.Data.Provider;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class CreditRuleDAO : DataProviderBase, ICreditRuleDAO
    {
        private const string SQL_INSERT = "INSERT INTO bbs_CreditRule (PublishmentSystemID, RuleType, ForumID, PeriodType, PeriodCount, MaxNum, Prestige, Contribution, Currency, ExtCredit1, ExtCredit2, ExtCredit3) VALUES (@PublishmentSystemID, @RuleType, @ForumID, @PeriodType, @PeriodCount, @MaxNum, @Prestige, @Contribution, @Currency, @ExtCredit1, @ExtCredit2, @ExtCredit3)";

        private const string SQL_UPDATE = "UPDATE bbs_CreditRule SET RuleType = @RuleType, ForumID = @ForumID, PeriodType = @PeriodType, PeriodCount = @PeriodCount, MaxNum = @MaxNum, Prestige = @Prestige, Contribution = @Contribution, Currency = @Currency, ExtCredit1 = @ExtCredit1, ExtCredit2 = @ExtCredit2, ExtCredit3 = @ExtCredit3 WHERE ID = @ID";

        private const string SQL_DELETE = "DELETE FROM bbs_CreditRule WHERE ID = @ID";

        private const string SQL_SELECT = "SELECT ID, PublishmentSystemID, RuleType, ForumID, PeriodType, PeriodCount, MaxNum, Prestige, Contribution, Currency, ExtCredit1, ExtCredit2, ExtCredit3 FROM bbs_CreditRule WHERE ID = @ID";

        private const string SQL_SELECT_NAME = "SELECT RuleType FROM bbs_CreditRule WHERE ID = @ID";

        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_RULE_TYPE = "@RuleType";
        private const string PARM_FORUM_ID = "@ForumID";
        private const string PARM_PERIOD_TYPE = "@PeriodType";
        private const string PARM_PERIOD_COUNT = "@PeriodCount";
        private const string PARM_MAX_NUM = "@MaxNum";
        private const string PARM_PRESTIGE = "@Prestige";
        private const string PARM_CONTRIBUTION = "@Contribution";
        private const string PARM_CURRENCY = "@Currency";
        private const string PARM_EXT_CREDIT1 = "@ExtCredit1";
        private const string PARM_EXT_CREDIT2 = "@ExtCredit2";
        private const string PARM_EXT_CREDIT3 = "@ExtCredit3";

        public void Insert(int publishmentSystemID, CreditRuleInfo ruleInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, ruleInfo.PublishmentSystemID),
				this.GetParameter(PARM_RULE_TYPE, EDataType.VarChar, 50, ECreditRuleUtils.GetValue(ruleInfo.RuleType)),
				this.GetParameter(PARM_FORUM_ID, EDataType.Integer, ruleInfo.ForumID),
                this.GetParameter(PARM_PERIOD_TYPE, EDataType.VarChar, 50, EPeriodTypeUtils.GetValue(ruleInfo.PeriodType)),
                this.GetParameter(PARM_PERIOD_COUNT, EDataType.Integer, ruleInfo.PeriodCount),
                this.GetParameter(PARM_MAX_NUM, EDataType.Integer, ruleInfo.MaxNum),
                this.GetParameter(PARM_PRESTIGE, EDataType.Integer, ruleInfo.Prestige),
                this.GetParameter(PARM_CONTRIBUTION, EDataType.Integer, ruleInfo.Contribution),
                this.GetParameter(PARM_CURRENCY, EDataType.Integer, ruleInfo.Currency),
                this.GetParameter(PARM_EXT_CREDIT1, EDataType.Integer, ruleInfo.ExtCredit1),
                this.GetParameter(PARM_EXT_CREDIT2, EDataType.Integer, ruleInfo.ExtCredit2),
                this.GetParameter(PARM_EXT_CREDIT3, EDataType.Integer, ruleInfo.ExtCredit3)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);

            CreditRuleManager.RemoveCache(publishmentSystemID);
        }

        public void Update(int publishmentSystemID, CreditRuleInfo ruleInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_RULE_TYPE, EDataType.VarChar, 50, ECreditRuleUtils.GetValue(ruleInfo.RuleType)),
				this.GetParameter(PARM_FORUM_ID, EDataType.Integer, ruleInfo.ForumID),
                this.GetParameter(PARM_PERIOD_TYPE, EDataType.VarChar, 50, EPeriodTypeUtils.GetValue(ruleInfo.PeriodType)),
                this.GetParameter(PARM_PERIOD_COUNT, EDataType.Integer, ruleInfo.PeriodCount),
                this.GetParameter(PARM_MAX_NUM, EDataType.Integer, ruleInfo.MaxNum),
                this.GetParameter(PARM_PRESTIGE, EDataType.Integer, ruleInfo.Prestige),
                this.GetParameter(PARM_CONTRIBUTION, EDataType.Integer, ruleInfo.Contribution),
                this.GetParameter(PARM_CURRENCY, EDataType.Integer, ruleInfo.Currency),
                this.GetParameter(PARM_EXT_CREDIT1, EDataType.Integer, ruleInfo.ExtCredit1),
                this.GetParameter(PARM_EXT_CREDIT2, EDataType.Integer, ruleInfo.ExtCredit2),
                this.GetParameter(PARM_EXT_CREDIT3, EDataType.Integer, ruleInfo.ExtCredit3),
				this.GetParameter(PARM_ID, EDataType.Integer, ruleInfo.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);

            CreditRuleManager.RemoveCache(publishmentSystemID);
        }

        public void Delete(int publishmentSystemID, int id)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, id)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);

            CreditRuleManager.RemoveCache(publishmentSystemID);
        }

        public CreditRuleInfo GetCreditRuleInfo(int id)
        {
            CreditRuleInfo ruleInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, id)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    ruleInfo = new CreditRuleInfo(rdr.GetInt32(0), rdr.GetInt32(1), ECreditRuleUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), EPeriodTypeUtils.GetEnumType(rdr.GetValue(4).ToString()), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetInt32(10), rdr.GetInt32(11), rdr.GetInt32(12));
                }
                rdr.Close();
            }

            return ruleInfo;
        }

        public Hashtable GetCreditRuleInfoHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, RuleType, ForumID, PeriodType, PeriodCount, MaxNum, Prestige, Contribution, Currency, ExtCredit1, ExtCredit2, ExtCredit3 FROM bbs_CreditRule WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    CreditRuleInfo ruleInfo = new CreditRuleInfo(rdr.GetInt32(0), rdr.GetInt32(1), ECreditRuleUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), EPeriodTypeUtils.GetEnumType(rdr.GetValue(4).ToString()), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetInt32(10), rdr.GetInt32(11), rdr.GetInt32(12));
                    if (ruleInfo.ForumID > 0)
                    {
                        hashtable[ECreditRuleUtils.GetValue(ruleInfo.RuleType) + ruleInfo.ForumID] = ruleInfo;
                    }
                    else
                    {
                        hashtable[ECreditRuleUtils.GetValue(ruleInfo.RuleType)] = ruleInfo;
                    }
                }
                rdr.Close();
            }

            return hashtable;
        }
    }
}