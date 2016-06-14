using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model; 
 
namespace SiteServer.BBS.Model
{
    public class CreditRuleLogInfo
    {
        private int id;
        private int publishmentSystemID;
        private string userName;
        private ECreditRule ruleType;
        private int totalCount;
        private int periodCount;
        private int prestige;
        private int contribution;
        private int currency;
        private int extCredit1;
        private int extCredit2;
        private int extCredit3;
        private DateTime lastDate;

        //public CreditRuleLogInfo()
        //{
        //    this.id = 0;
        //    this.publishmentSystemID = 0;
        //    this.userName = string.Empty;
        //    this.ruleType = ECreditRule.Login;
        //    this.totalCount = 0;
        //    this.periodCount = 0;
        //    this.prestige = 0;
        //    this.contribution = 0;
        //    this.currency = 0;
        //    this.extCredit1 = 0;
        //    this.extCredit2 = 0;
        //    this.extCredit3 = 0;
        //    this.lastDate = DateTime.Now;
        //}

        public CreditRuleLogInfo(int id, int publishmentSystemID, string userName, ECreditRule ruleType, int totalCount, int periodCount, int prestige, int contribution, int currency, int extCredit1, int extCredit2, int extCredit3, DateTime lastDate)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.userName = userName;
            this.ruleType = ruleType;
            this.totalCount = totalCount;
            this.periodCount = periodCount;
            this.prestige = prestige;
            this.contribution = contribution;
            this.currency = currency;
            this.extCredit1 = extCredit1;
            this.extCredit2 = extCredit2;
            this.extCredit3 = extCredit3;
            this.lastDate = lastDate;
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

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public ECreditRule RuleType
        {
            get { return ruleType; }
            set { ruleType = value; }
        }

        public int TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; }
        }

        public int PeriodCount
        {
            get { return periodCount; }
            set { periodCount = value; }
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

        public DateTime LastDate
        {
            get { return lastDate; }
            set { lastDate = value; }
        }
    }
}
