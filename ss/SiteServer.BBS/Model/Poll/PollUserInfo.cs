using System;

namespace SiteServer.BBS.Model
{
	public class PollUserInfo
	{
		private int id;
        private int publishmentSystemID;
		private int pollID;
        private string pollItemIDCollection;
		private string ipAddress;
        private string userName;
		private DateTime addDate;

        //public PollUserInfo()
        //{
        //    this.id = 0;
        //    this.publishmentSystemID = 0;
        //    this.pollID = 0;
        //    this.pollItemIDCollection = string.Empty;
        //    this.ipAddress = string.Empty;
        //    this.userName = string.Empty;
        //    this.addDate = DateTime.Now;
        //}

        public PollUserInfo(int id, int publishmentSystemID, int pollID, string pollItemIDCollection, string ipAddress, string userName, DateTime addDate) 
		{
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
			this.pollID = pollID;
            this.pollItemIDCollection = pollItemIDCollection;
			this.ipAddress = ipAddress;
            this.userName = userName;
			this.addDate = addDate;
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

        public string PollItemIDCollection
        {
            get { return pollItemIDCollection; }
            set { pollItemIDCollection = value; }
        }

		public string IPAddress
		{
			get{ return ipAddress; }
			set{ ipAddress = value; }
		}

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

		public DateTime AddDate
		{
			get{ return addDate; }
			set{ addDate = value; }
		}
	}
}
