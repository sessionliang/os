using System;
using System.Collections;

namespace BaiRong.Model
{
	public class JobContentAttribute
	{
        protected JobContentAttribute()
		{
		}

        //system
        public const string Department = "Department";
        public const string Location = "Location";
        public const string NumberOfPeople = "NumberOfPeople";
        public const string Responsibility = "Responsibility";
        public const string Requirement = "Requirement";
        public const string IsUrgent = "IsUrgent";

        public const string CheckTaskDate = "CheckTaskDate";                    //审核时间
        public const string UnCheckTaskDate = "UnCheckTaskDate";                //下架时间

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arraylist = new ArrayList(ContentAttribute.AllAttributes);
                arraylist.AddRange(SystemAttributes);
                return arraylist;
            }
        }

        private static ArrayList systemAttributes;
        public static ArrayList SystemAttributes
        {
            get
            {
                if (systemAttributes == null)
                {
                    systemAttributes = new ArrayList();
                    systemAttributes.Add(Department.ToLower());
                    systemAttributes.Add(Location.ToLower());
                    systemAttributes.Add(NumberOfPeople.ToLower());
                    systemAttributes.Add(Responsibility.ToLower());
                    systemAttributes.Add(Requirement.ToLower());
                    systemAttributes.Add(IsUrgent.ToLower());
                }

                return systemAttributes;
            }
        }

        private static ArrayList excludeAttributes;
        public static ArrayList ExcludeAttributes
        {
            get
            {
                if (excludeAttributes == null)
                {
                    excludeAttributes = new ArrayList(ContentAttribute.ExcludeAttributes);
                    excludeAttributes.Add(IsUrgent.ToLower());
                }

                return excludeAttributes;
            }
        }
	}
}
