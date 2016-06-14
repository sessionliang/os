using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiRong.Model
{
    public class LevelRuleInfo
    {
        private int id;
        private ELevelRule ruleType;
        private ELevelPeriodType periodType;
        private int periodCount;
        private int maxNum;
        private int creditNum;
        private int cashNum;

        public LevelRuleInfo()
        {
            this.id = 0;
            this.ruleType = ELevelRule.PlatFormLogin;
            this.periodType = ELevelPeriodType.None;
            this.periodCount = 0;
            this.maxNum = 0;
            this.creditNum = 0;
            this.cashNum = 0;
        }

        public LevelRuleInfo(int id, ELevelRule ruleType, ELevelPeriodType periodType, int periodCount, int maxNum, int creditNum, int cashNum)
        {
            this.id = id;
            this.ruleType = ruleType;
            this.periodType = periodType;
            this.periodCount = periodCount;
            this.maxNum = maxNum;
            this.creditNum = creditNum;
            this.cashNum = cashNum;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }



        public ELevelRule RuleType
        {
            get { return ruleType; }
            set { ruleType = value; }
        }



        public ELevelPeriodType PeriodType
        {
            get { return periodType; }
            set { periodType = value; }
        }

        public int PeriodCount
        {
            get
            {
                if (periodCount <= 0)
                {
                    return 1;
                }
                else
                {
                    return periodCount;
                }
            }
            set { periodCount = value; }
        }

        public int MaxNum
        {
            get { return maxNum; }
            set { maxNum = value; }
        }

        public int CreditNum
        {
            get { return creditNum; }
            set { creditNum = value; }
        }

        public int CashNum
        {
            get { return cashNum; }
            set { cashNum = value; }
        }     
    }
}
