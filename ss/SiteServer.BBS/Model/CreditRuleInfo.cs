using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model; 
 
namespace SiteServer.BBS.Model
{
    public class CreditRuleInfo
    {
        private int id;
        private int publishmentSystemID;
        private ECreditRule ruleType;
        private int forumID;
        private EPeriodType periodType;
        private int periodCount;
        private int maxNum;
        private int prestige;
        private int contribution;
        private int currency;
        private int extCredit1;
        private int extCredit2;
        private int extCredit3;

        public CreditRuleInfo()
        {
            this.id = 0;
            this.publishmentSystemID = 0;
            this.ruleType = ECreditRule.Login;
            this.forumID = 0;
            this.periodType = EPeriodType.None;
            this.periodCount = 0;
            this.maxNum = 0;
            this.prestige = 0;
            this.contribution = 0;
            this.currency = 0;
            this.extCredit1 = 0;
            this.extCredit2 = 0;
            this.extCredit3 = 0;
        }

        public CreditRuleInfo(int id, int publishmentSystemID, ECreditRule ruleType, int forumID, EPeriodType periodType, int periodCount, int maxNum, int prestige, int contribution, int currency, int extCredit1, int extCredit2, int extCredit3)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.ruleType = ruleType;
            this.forumID = forumID;
            this.periodType = periodType;
            this.periodCount = periodCount;
            this.maxNum = maxNum;
            this.prestige = prestige;
            this.contribution = contribution;
            this.currency = currency;
            this.extCredit1 = extCredit1;
            this.extCredit2 = extCredit2;
            this.extCredit3 = extCredit3;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public ECreditRule RuleType
        {
            get { return ruleType; }
            set { ruleType = value; }
        }

        public int ForumID
        {
            get { return forumID; }
            set { forumID = value; }
        }

        public EPeriodType PeriodType
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

        public int Prestige
        {
            get { return prestige; }
            set { prestige = value; }
        }

        public int Contribution
        {
            get { return contribution; }
            set { contribution = value; }
        }

        public int Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        public int ExtCredit1
        {
            get { return extCredit1; }
            set { extCredit1 = value; }
        }

        public int ExtCredit2
        {
            get { return extCredit2; }
            set { extCredit2 = value; }
        }

        public int ExtCredit3
        {
            get { return extCredit3; }
            set { extCredit3 = value; }
        }
    }
}
