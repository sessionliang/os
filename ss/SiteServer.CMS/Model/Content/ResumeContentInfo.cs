using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
	public class ResumeContentInfo : ExtendedAttributes
	{
		public ResumeContentInfo()
		{
			this.ID = 0;
            this.StyleID = 0;
            this.PublishmentSystemID = 0;
            this.JobContentID = 0;
            this.UserName = string.Empty;
			this.AddDate = DateTime.Now;
		}

        public ResumeContentInfo(int id, int styleID, int publishmentSystemID, int jobContentID, string userName, DateTime addDate)
		{
			this.ID = id;
            this.StyleID = styleID;
            this.PublishmentSystemID = publishmentSystemID;
            this.JobContentID = jobContentID;
            this.UserName = userName;
			this.AddDate = addDate;
		}

		public int ID
		{
			get { return base.GetInt(ResumeContentAttribute.ID, 0); }
            set { base.SetExtendedAttribute(ResumeContentAttribute.ID, value.ToString()); }
		}

        public int StyleID
		{
            get { return base.GetInt(ResumeContentAttribute.StyleID, 0); }
            set { base.SetExtendedAttribute(ResumeContentAttribute.StyleID, value.ToString()); }
		}

        public int PublishmentSystemID
        {
            get { return base.GetInt(ResumeContentAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(ResumeContentAttribute.PublishmentSystemID, value.ToString()); }
        }

        public int JobContentID
        {
            get { return base.GetInt(ResumeContentAttribute.JobContentID, 0); }
            set { base.SetExtendedAttribute(ResumeContentAttribute.JobContentID, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(ResumeContentAttribute.UserName); }
            set { base.SetExtendedAttribute(ResumeContentAttribute.UserName, value); }
        }

        public bool IsView
        {
            get { return base.GetBool(ResumeContentAttribute.IsView, false); }
            set { base.SetExtendedAttribute(ResumeContentAttribute.IsView, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(ResumeContentAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ResumeContentAttribute.AddDate, value.ToString()); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return ResumeContentAttribute.AllAttributes;
        }
	}
}
