using System;
using BaiRong.Model;
using System.Collections;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public class JobContentInfo : ContentInfo
	{
		public JobContentInfo() : base()
		{
            this.Department = string.Empty;
            this.Location = string.Empty;
            this.NumberOfPeople = string.Empty;
            this.IsUrgent = false;
            this.Responsibility = string.Empty;
            this.Requirement = string.Empty;
		}

        public JobContentInfo(object dataItem)
            : base(dataItem)
		{
		}

        public string Department
		{
            get { return this.GetExtendedAttribute(JobContentAttribute.Department); }
            set { base.SetExtendedAttribute(JobContentAttribute.Department, value); }
		}

        public string Location
		{
            get { return this.GetExtendedAttribute(JobContentAttribute.Location); }
            set { base.SetExtendedAttribute(JobContentAttribute.Location, value); }
		}

        public string NumberOfPeople
		{
            get { return this.GetExtendedAttribute(JobContentAttribute.NumberOfPeople); }
            set { base.SetExtendedAttribute(JobContentAttribute.NumberOfPeople, value); }
		}

        public bool IsUrgent
		{
            get { return base.GetBool(JobContentAttribute.IsUrgent, false); }
            set { base.SetExtendedAttribute(JobContentAttribute.IsUrgent, value.ToString()); }
		}

        public string Responsibility
        {
            get { return this.GetExtendedAttribute(JobContentAttribute.Responsibility); }
            set { base.SetExtendedAttribute(JobContentAttribute.Responsibility, value); }
        }

        public string Requirement
        {
            get { return this.GetExtendedAttribute(JobContentAttribute.Requirement); }
            set { base.SetExtendedAttribute(JobContentAttribute.Requirement, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return JobContentAttribute.AllAttributes;
        }
	}
}
