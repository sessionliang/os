using System;
using System.Data;
using System.Collections.Generic;
using BaiRong.Core.Data.Provider;
using System.Collections;
using BaiRong.Model;
using BaiRong.Core;

namespace BaiRong.Provider.Data.SqlServer
{
    public class LevelRuleDAO : DataProviderBase, ILevelRuleDAO
    {
        private const string SQL_INSERT = "INSERT INTO bairong_LevelRule (RuleType, PeriodType, PeriodCount, MaxNum, CreditNum, CashNum) VALUES (@RuleType, @PeriodType, @PeriodCount, @MaxNum, @CreditNum, @CashNum)";

        private const string SQL_UPDATE = "UPDATE bairong_LevelRule SET RuleType = @RuleType, PeriodType = @PeriodType, PeriodCount = @PeriodCount, MaxNum = @MaxNum, CreditNum = @CreditNum, CashNum = @CashNum WHERE ID = @ID";

        private const string SQL_DELETE = "DELETE FROM bairong_LevelRule WHERE ID = @ID";

        private const string SQL_SELECT = "SELECT ID, RuleType, PeriodType, PeriodCount, MaxNum,CreditNum,CashNum FROM bairong_LevelRule WHERE ID = @ID";

        private const string SQL_SELECT_NAME = "SELECT RuleType FROM bairong_LevelRule WHERE ID = @ID";

        private const string PARM_ID = "@ID";
        private const string PARM_RULE_TYPE = "@RuleType";
        private const string PARM_PERIOD_TYPE = "@PeriodType";
        private const string PARM_PERIOD_COUNT = "@PeriodCount";
        private const string PARM_MAX_NUM = "@MaxNum";
        private const string PARM_CREDIT_NUM = "@CreditNum";
        private const string PARM_CASH_NUM = "@CashNum";

        public void Insert(LevelRuleInfo ruleInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{                
				this.GetParameter(PARM_RULE_TYPE, EDataType.VarChar, 50, ELevelRuleUtils.GetValue(ruleInfo.RuleType)),
				this.GetParameter(PARM_PERIOD_TYPE, EDataType.VarChar, 50, ELevelPeriodTypeUtils.GetValue(ruleInfo.PeriodType)),
                this.GetParameter(PARM_PERIOD_COUNT, EDataType.Integer, ruleInfo.PeriodCount),
                this.GetParameter(PARM_MAX_NUM, EDataType.Integer, ruleInfo.MaxNum),                 
                this.GetParameter(PARM_CREDIT_NUM, EDataType.Integer, ruleInfo.CreditNum),
                this.GetParameter(PARM_CASH_NUM, EDataType.Integer, ruleInfo.CashNum)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);

            LevelRuleManager.RemoveCache();
        }

        public void Update(LevelRuleInfo ruleInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_ID, EDataType.Integer, ruleInfo.ID),
                this.GetParameter(PARM_RULE_TYPE, EDataType.VarChar, 50, ELevelRuleUtils.GetValue(ruleInfo.RuleType)),
				this.GetParameter(PARM_PERIOD_TYPE, EDataType.VarChar, 50, ELevelPeriodTypeUtils.GetValue(ruleInfo.PeriodType)),
                this.GetParameter(PARM_PERIOD_COUNT, EDataType.Integer, ruleInfo.PeriodCount),
                this.GetParameter(PARM_MAX_NUM, EDataType.Integer, ruleInfo.MaxNum),                 
                this.GetParameter(PARM_CREDIT_NUM, EDataType.Integer, ruleInfo.CreditNum),
                this.GetParameter(PARM_CASH_NUM, EDataType.Integer, ruleInfo.CashNum)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);

            LevelRuleManager.RemoveCache();
        }

        public void Delete(int id)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, id)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);

            LevelRuleManager.RemoveCache();
        }

        public LevelRuleInfo GetLevelRuleInfo(int id)
        {
            LevelRuleInfo ruleInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, id)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    ruleInfo = new LevelRuleInfo(rdr.GetInt32(0), ELevelRuleUtils.GetEnumType(rdr.GetValue(1).ToString()), ELevelPeriodTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6));
                }
                rdr.Close();
            }

            return ruleInfo;
        }

        public Hashtable GetLevelRuleInfoHashtable()
        {
            Hashtable hashtable = new Hashtable();
            string sqlString = string.Format("SELECT ID, RuleType, PeriodType, PeriodCount, MaxNum,CreditNum,CashNum,AppID FROM bairong_LevelRule");

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    LevelRuleInfo ruleInfo = new LevelRuleInfo(rdr.GetInt32(0), ELevelRuleUtils.GetEnumType(rdr.GetValue(1).ToString()), ELevelPeriodTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6));

                    hashtable[ELevelRuleUtils.GetValue(ruleInfo.RuleType)] = ruleInfo;

                }
                rdr.Close();
            }

            return hashtable;
        }
    }
}