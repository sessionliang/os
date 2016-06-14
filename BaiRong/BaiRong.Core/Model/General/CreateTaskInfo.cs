using BaiRong.Core;
using System;
using System.Collections;

namespace BaiRong.Model
{
    public class CreateTaskAttribute
    {
        protected CreateTaskAttribute() { }

        public const string ID = "ID";
        public const string UserName = "UserName";
        public const string TotalCount = "TotalCount";
        public const string ErrorCount = "ErrorCount";
        public const string Summary = "Summary";
        public const string State = "State";
        public const string AddDate = "AddDate";
        public const string IsVirsual = "IsVirsual";
        public const string RealID = "RealID";
        public const string StartTime = "StartTime";
        public const string EndTime = "EndTime";

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arrayList = new ArrayList();
                arrayList.Add(ID.ToLower());
                arrayList.Add(UserName.ToLower());
                arrayList.Add(TotalCount.ToLower());
                arrayList.Add(ErrorCount.ToLower());
                arrayList.Add(Summary.ToLower());
                arrayList.Add(State.ToLower());
                arrayList.Add(AddDate.ToLower());
                arrayList.Add(IsVirsual.ToLower());
                arrayList.Add(RealID.ToLower());
                arrayList.Add(StartTime.ToLower());
                arrayList.Add(EndTime.ToLower());
                return arrayList;
            }
        }
    }


    public class CreateTaskInfo : ExtendedAttributes
    {
        public const string TableName = "bairong_CreateTask";

        public CreateTaskInfo()
        {
            this.StartTime = DateUtils.SqlMinValue;
            this.EndTime = DateUtils.SqlMinValue;
        }

        public int ID
        {
            get { return base.GetInt(CreateTaskAttribute.ID, 0); }
            set { base.SetExtendedAttribute(CreateTaskAttribute.ID, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetString(CreateTaskAttribute.UserName, string.Empty); }
            set { base.SetExtendedAttribute(CreateTaskAttribute.UserName, value); }
        }

        public int TotalCount
        {
            get { return base.GetInt(CreateTaskAttribute.TotalCount, 0); }
            set { base.SetExtendedAttribute(CreateTaskAttribute.TotalCount, value.ToString()); }
        }

        public int ErrorCount
        {
            get { return base.GetInt(CreateTaskAttribute.ErrorCount, 0); }
            set { base.SetExtendedAttribute(CreateTaskAttribute.ErrorCount, value.ToString()); }
        }

        public string Summary
        {
            get { return base.GetString(CreateTaskAttribute.Summary, string.Empty); }
            set { base.SetExtendedAttribute(CreateTaskAttribute.Summary, value); }
        }

        public ECreateTaskType State
        {
            get { return ECreateTaskTypeUtils.GetEnumType(base.GetString(CreateTaskAttribute.State, string.Empty)); }
            set
            {
                base.SetExtendedAttribute(CreateTaskAttribute.State, ECreateTaskTypeUtils.GetValue(value));
            }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(CreateTaskAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(CreateTaskAttribute.AddDate, value.ToString()); }
        }

        public DateTime StartTime
        {
            get { return base.GetDateTime(CreateTaskAttribute.StartTime, DateUtils.SqlMinValue); }
            set { base.SetExtendedAttribute(CreateTaskAttribute.StartTime, value.ToString()); }
        }

        public DateTime EndTime
        {
            get { return base.GetDateTime(CreateTaskAttribute.EndTime, DateUtils.SqlMinValue); }
            set { base.SetExtendedAttribute(CreateTaskAttribute.EndTime, value.ToString()); }
        }
    }
}
