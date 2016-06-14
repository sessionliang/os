using System;

namespace SiteServer.BBS.Model
{
	public class PollItemInfo
	{
		private int id;
        private int publishmentSystemID;
		private int pollID;
		private string title;
		private int num;

        //public PollItemInfo()
        //{
        //    this.id = 0;
        //    this.publishmentSystemID = 0;
        //    this.pollID = 0;
        //    this.title = string.Empty;
        //    this.num = 0;
        //}

        public PollItemInfo(int id, int publishmentSystemID, int pollID, string title, int num) 
		{
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
			this.pollID = pollID;
            this.title = title;
            this.num = num;
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

		public int PollID
		{
			get{ return pollID; }
			set{ pollID = value; }
		}

        public string Title
		{
            get { return title; }
            set { title = value; }
		}

        public int Num
		{
            get { return num; }
            set { num = value; }
		}
	}
}
