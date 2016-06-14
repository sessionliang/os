using System;
using BaiRong.Model;

namespace BaiRong.Model
{
	public class UserCreditsLogInfo
	{
        private int id;
        private string userName;
        private string productID;
        private bool isIncreased;
        private int num;
        private string action;
        private string description;
        private DateTime addDate;

		public UserCreditsLogInfo()
		{
            this.id = 0;
            this.userName = string.Empty;
            this.productID = string.Empty;
            this.isIncreased = true;
            this.num = 0;
            this.action = string.Empty;
            this.description = string.Empty;
            this.addDate = DateTime.Now;
		}

        public UserCreditsLogInfo(int id, string userName, string productID, bool isIncreased, int num, string action, string description, DateTime addDate) 
		{
            this.id = id;
            this.userName = userName;
            this.productID = productID;
            this.isIncreased = isIncreased;
            this.num = num;
            this.action = action;
            this.description = description;
            this.addDate = addDate;
		}

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string UserName
		{
            get { return userName; }
            set { userName = value; }
		}

        public string ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public bool IsIncreased
        {
            get { return isIncreased; }
            set { isIncreased = value; }
        }

        public int Num
        {
            get { return num; }
            set { num = value; }
        }

        public string Action
        {
            get { return action; }
            set { action = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }
	}
}
