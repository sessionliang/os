using System;
using System.Collections.Generic;
using System.Text;

using System.Xml.Serialization;
using BaiRong.Core;

namespace SiteServer.BBS.Model
{
    public class BBSUserInfo
    {
        private int userID;
        private string groupSN;
        private string userName;
        private int postCount;
        private int postDigestCount;
        private int prestige;
        private int contribution;
        private int currency;
        private int extCredit1;
        private int extCredit2;
        private int extCredit3;
        private DateTime lastPostDate = DateTime.Now;

        public BBSUserInfo(int userID, string groupSN, string userName)
        {
            this.userID = userID;
            this.groupSN = groupSN;
            this.userName = userName;
        }

        public BBSUserInfo(int userID, string groupSN, string userName, int postCount, int postDigestCount, int prestige, int contribution, int currency, int extCredit1, int extCredit2, int extCredit3, DateTime lastPostDate)
        {
            this.userID = userID;
            this.groupSN = groupSN;
            this.userName = userName;
            this.postCount = postCount;
            this.postDigestCount = postDigestCount;
            this.prestige = prestige;
            this.contribution = contribution;
            this.currency = currency;
            this.extCredit1 = extCredit1;
            this.extCredit2 = extCredit2;
            this.extCredit3 = extCredit3;
            this.lastPostDate = lastPostDate;
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public string GroupSN
        {
            get { return groupSN; }
            set { groupSN = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public int PostCount
        {
            get { return postCount; }
            set { postCount = value; }
        }

        public int PostDigestCount
        {
            get { return postDigestCount; }
            set { postDigestCount = value; }
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

        public DateTime LastPostDate
        {
            get { return lastPostDate; }
            set { lastPostDate = value; }
        }
    }
}