using System;
using BaiRong.Model;

namespace BaiRong.Model
{
	public class UserBindingInfo
	{
        private string userName;
        private string bindingType;
        private int bindingID;
        private string bindingName;

		public UserBindingInfo()
		{
            this.userName = string.Empty;
            this.bindingType = string.Empty;
            this.bindingID = 0;
            this.bindingName = string.Empty;
		}

        public UserBindingInfo(string userName, string bindingType, int bindingID, string bindingName) 
		{
            this.userName = userName;
            this.bindingType = bindingType;
            this.bindingID = bindingID;
            this.bindingName = bindingName;
		}

        public string UserName
		{
            get { return userName; }
            set { userName = value; }
		}

        public string BindingType
        {
            get { return bindingType; }
            set { bindingType = value; }
        }

        public int BindingID
        {
            get { return bindingID; }
            set { bindingID = value; }
        }

        public string BindingName
        {
            get { return bindingName; }
            set { bindingName = value; }
        }
	}
}
