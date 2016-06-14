using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
	public class InputContentInfo : ExtendedAttributes
	{
		public InputContentInfo()
		{
			this.ID = 0;
			this.InputID = 0;
			this.Taxis = 0;
			this.IsChecked = false;
            this.UserName = string.Empty;
            this.IPAddress = string.Empty;
            this.Location = string.Empty;
			this.AddDate = DateTime.Now;
            this.Reply = string.Empty;
		}

        public InputContentInfo(int id, int inputID, int taxis, bool isChecked, string userName, string ipAddress, string location, DateTime addDate, string reply)
		{
			this.ID = id;
            this.InputID = inputID;
			this.Taxis = taxis;
			this.IsChecked = isChecked;
            this.UserName = userName;
            this.IPAddress = ipAddress;
            this.Location = location;
			this.AddDate = addDate;
            this.Reply = reply;
		}

		public int ID
		{
			get { return base.GetInt(InputContentAttribute.ID, 0); }
            set { base.SetExtendedAttribute(InputContentAttribute.ID, value.ToString()); }
		}

        public int InputID
		{
            get { return base.GetInt(InputContentAttribute.InputID, 0); }
            set { base.SetExtendedAttribute(InputContentAttribute.InputID, value.ToString()); }
		}

		public int Taxis
		{
            get { return base.GetInt(InputContentAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(InputContentAttribute.Taxis, value.ToString()); }
		}

		public bool IsChecked
		{
            get { return base.GetBool(InputContentAttribute.IsChecked, false); }
            set { base.SetExtendedAttribute(InputContentAttribute.IsChecked, value.ToString()); }
		}

        public string UserName
        {
            get { return base.GetExtendedAttribute(InputContentAttribute.UserName); }
            set { base.SetExtendedAttribute(InputContentAttribute.UserName, value); }
        }

        public string IPAddress
        {
            get { return base.GetExtendedAttribute(InputContentAttribute.IPAddress); }
            set { base.SetExtendedAttribute(InputContentAttribute.IPAddress, value); }
        }

        public string Location
        {
            get { return base.GetExtendedAttribute(InputContentAttribute.Location); }
            set { base.SetExtendedAttribute(InputContentAttribute.Location, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(InputContentAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(InputContentAttribute.AddDate, value.ToString()); }
        }

        public string Reply
        {
            get { return base.GetExtendedAttribute(InputContentAttribute.Reply); }
            set { base.SetExtendedAttribute(InputContentAttribute.Reply, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return InputContentAttribute.AllAttributes;
        }
	}
}
