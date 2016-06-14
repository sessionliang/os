using System;

namespace SiteServer.WeiXin.Model
{
	public class CountInfo
	{
        private int countID;
        private int publishmentSystemID;
        private int countYear;
        private int countMonth;
        private int countDay;
        private ECountType countType;
        private int count;

		public CountInfo()
		{
            this.countID = 0;
            this.publishmentSystemID = 0;
            this.countYear = 0;
            this.countMonth = 0;
            this.countDay = 0;
            this.countType = ECountType.UserSubscribe;
            this.count = 0;
		}

        public CountInfo(int countID, int publishmentSystemID, int countYear, int countMonth, int countDay, ECountType countType, int count)
		{
            this.countID = countID;
            this.publishmentSystemID = publishmentSystemID;
            this.countYear = countYear;
            this.countMonth = countMonth;
            this.countDay = countDay;
            this.countType = countType;
            this.count = count;
		}

        public int CountID
        {
            get { return countID; }
            set { countID = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int CountYear
        {
            get { return countYear; }
            set { countYear = value; }
        }

        public int CountMonth
        {
            get { return countMonth; }
            set { countMonth = value; }
        }

        public int CountDay
        {
            get { return countDay; }
            set { countDay = value; }
        }

        public ECountType CountType
        {
            get { return countType; }
            set { countType = value; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
	}
}
