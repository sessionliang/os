using System;
using BaiRong.Core;

namespace SiteServer.BBS.Model
{
	public class PollInfo
	{
		private int id;
        private int publishmentSystemID;
        private int threadID;
        private bool isVoteFirst;
        private int maxNum;
        private EPollRestrictType restrictType;
		private DateTime addDate;
        private DateTime deadline;

        //public PollInfo()
        //{
        //    this.id = 0;
        //    this.publishmentSystemID = 0;
        //    this.threadID = 0;
        //    this.isVoteFirst = false;
        //    this.maxNum = 0;
        //    this.restrictType = EPollRestrictType.RestrictOnlyOnce;
        //    this.addDate = DateTime.Now;
        //    this.deadline = DateTime.Now;
        //}

        public PollInfo(int id, int publishmentSystemID, int threadID, bool isVoteFirst, int maxNum, EPollRestrictType restrictType, DateTime addDate, DateTime deadline) 
		{
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.threadID = threadID;
            this.isVoteFirst = isVoteFirst;
            this.maxNum = maxNum;
            this.restrictType = restrictType;
			this.addDate = addDate;
            this.deadline = deadline;
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

        public int ThreadID
        {
            get { return threadID; }
            set { threadID = value; }
        }

        public bool IsVoteFirst
        {
            get { return isVoteFirst; }
            set { isVoteFirst = value; }
        }

		public int MaxNum
		{
            get { return maxNum; }
            set { maxNum = value; }
		}

        public EPollRestrictType RestrictType
		{
            get { return restrictType; }
            set { restrictType = value; }
		}

		public DateTime AddDate
		{
			get{ return addDate; }
			set{ addDate = value; }
		}

        public DateTime Deadline
        {
            get { return deadline; }
            set { deadline = value; }
        }

        public bool IsOverTime
        {
            get
            {
                return (this.deadline < DateTime.Now);
            }
        }
	}
}