using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.Project.Model
{
    public class ApplyAttribute
    {
        protected ApplyAttribute()
        {
        }

        //hidden
        public const string ID = "ID";
        public const string ProjectID = "ProjectID";
        public const string Priority = "Priority";
        public const string TypeID = "TypeID";
        public const string AddUserName = "AddUserName";
        public const string AddDate = "AddDate";
        public const string AcceptUserName = "AcceptUserName";
        public const string AcceptDate = "AcceptDate";
        public const string CheckUserName = "CheckUserName";
        public const string CheckDate = "CheckDate";
        public const string DepartmentID = "DepartmentID";
        public const string UserName = "UserName";
        public const string FileCount = "FileCount";
        public const string State = "State";
        public const string Title = "Title";

        //basic
        public const string ExpectedDate = "ExpectedDate";
        public const string StartDate = "StartDate";
        public const string EndDate = "EndDate";
        public const string Content = "Content";
        public const string Summary = "Summary";

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arraylist = new ArrayList(HiddenAttributes);
                arraylist.AddRange(BasicAttributes);
                return arraylist;
            }
        }

        private static ArrayList hiddenAttributes;
        public static ArrayList HiddenAttributes
        {
            get
            {
                if (hiddenAttributes == null)
                {
                    hiddenAttributes = new ArrayList();
                    hiddenAttributes.Add(ID.ToLower());
                    hiddenAttributes.Add(ProjectID.ToLower());
                    hiddenAttributes.Add(Priority.ToLower());
                    hiddenAttributes.Add(TypeID.ToLower());
                    hiddenAttributes.Add(AddUserName.ToLower());
                    hiddenAttributes.Add(AddDate.ToLower());
                    hiddenAttributes.Add(AcceptUserName.ToLower());
                    hiddenAttributes.Add(AcceptDate.ToLower());
                    hiddenAttributes.Add(CheckUserName.ToLower());
                    hiddenAttributes.Add(CheckDate.ToLower());
                    hiddenAttributes.Add(DepartmentID.ToLower());
                    hiddenAttributes.Add(UserName.ToLower());
                    hiddenAttributes.Add(FileCount.ToLower());
                    hiddenAttributes.Add(State.ToLower());
                    hiddenAttributes.Add(Title.ToLower());
                }

                return hiddenAttributes;
            }
        }

        private static ArrayList basicAttributes;
        public static ArrayList BasicAttributes
        {
            get
            {
                if (basicAttributes == null)
                {
                    basicAttributes = new ArrayList();
                    basicAttributes.Add(ExpectedDate.ToLower());
                    basicAttributes.Add(StartDate.ToLower());
                    basicAttributes.Add(EndDate.ToLower());
                    basicAttributes.Add(Content.ToLower());
                    basicAttributes.Add(Summary.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class ApplyInfo : ExtendedAttributes
    {
        public const string TableName = "pms_Apply";

        public ApplyInfo()
        {
            this.ID = 0;
            this.ProjectID = 0;
            this.Priority = 0;
            this.TypeID = 0;
            this.AddUserName = string.Empty;
            this.AddDate = DateTime.Now;
            this.AcceptUserName = string.Empty;
            this.AcceptDate = DateTime.Now;
            this.CheckUserName = string.Empty;
            this.CheckDate = DateTime.Now;
            this.DepartmentID = 0;
            this.UserName = string.Empty;
            this.FileCount = 0;
            this.State = EApplyState.New;
            this.Title = string.Empty;
        }

        public ApplyInfo(object dataItem)
            : base(dataItem)
		{
		}

        public ApplyInfo(int id, int projectID, int priority, int typeID, string addUserName, DateTime addDate, string acceptUserName, DateTime acceptDate, string checkUserName, DateTime checkDate, int departmentID, string userName, int fileCount, EApplyState state, string title)
        {
            this.ID = id;
            this.ProjectID = projectID;
            this.Priority = priority;
            this.TypeID = typeID;
            this.AddUserName = addUserName;
            this.AddDate = addDate;
            this.AcceptUserName = acceptUserName;
            this.AcceptDate = acceptDate;
            this.CheckUserName = checkUserName;
            this.CheckDate = checkDate;
            this.DepartmentID = departmentID;
            this.UserName = userName;
            this.FileCount = fileCount;
            this.State = state;            
            this.Title = title;
        }

        public int ID
        {
            get { return base.GetInt(ApplyAttribute.ID, 0); }
            set { base.SetExtendedAttribute(ApplyAttribute.ID, value.ToString()); }
        }

        public int ProjectID
        {
            get { return base.GetInt(ApplyAttribute.ProjectID, 0); }
            set { base.SetExtendedAttribute(ApplyAttribute.ProjectID, value.ToString()); }
        }

        public int Priority
        {
            get { return base.GetInt(ApplyAttribute.Priority, 0); }
            set { base.SetExtendedAttribute(ApplyAttribute.Priority, value.ToString()); }
        }

        public int TypeID
        {
            get { return base.GetInt(ApplyAttribute.TypeID, 0); }
            set { base.SetExtendedAttribute(ApplyAttribute.TypeID, value.ToString()); }
        }

        public string AddUserName
        {
            get { return base.GetExtendedAttribute(ApplyAttribute.AddUserName); }
            set { base.SetExtendedAttribute(ApplyAttribute.AddUserName, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(ApplyAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ApplyAttribute.AddDate, value.ToString()); }
        }

        public string AcceptUserName
        {
            get { return base.GetExtendedAttribute(ApplyAttribute.AcceptUserName); }
            set { base.SetExtendedAttribute(ApplyAttribute.AcceptUserName, value); }
        }

        public DateTime AcceptDate
        {
            get { return base.GetDateTime(ApplyAttribute.AcceptDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ApplyAttribute.AcceptDate, value.ToString()); }
        }

        public string CheckUserName
        {
            get { return base.GetExtendedAttribute(ApplyAttribute.CheckUserName); }
            set { base.SetExtendedAttribute(ApplyAttribute.CheckUserName, value); }
        }

        public DateTime CheckDate
        {
            get { return base.GetDateTime(ApplyAttribute.CheckDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ApplyAttribute.CheckDate, value.ToString()); }
        }

        public int DepartmentID
        {
            get { return base.GetInt(ApplyAttribute.DepartmentID, 0); }
            set { base.SetExtendedAttribute(ApplyAttribute.DepartmentID, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(ApplyAttribute.UserName); }
            set { base.SetExtendedAttribute(ApplyAttribute.UserName, value); }
        }

        public int FileCount
        {
            get { return base.GetInt(ApplyAttribute.FileCount, 0); }
            set { base.SetExtendedAttribute(ApplyAttribute.FileCount, value.ToString()); }
        }

        public EApplyState State
        {
            get { return EApplyStateUtils.GetEnumType(base.GetExtendedAttribute(ApplyAttribute.State)); }
            set { base.SetExtendedAttribute(ApplyAttribute.State, EApplyStateUtils.GetValue(value)); }
        }

        public string Title
        {
            get { return base.GetExtendedAttribute(ApplyAttribute.Title); }
            set { base.SetExtendedAttribute(ApplyAttribute.Title, value); }
        }

        public DateTime ExpectedDate
        {
            get { return base.GetDateTime(ApplyAttribute.ExpectedDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ApplyAttribute.ExpectedDate, value.ToString()); }
        }

        public DateTime StartDate
        {
            get { return base.GetDateTime(ApplyAttribute.StartDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ApplyAttribute.StartDate, value.ToString()); }
        }

        public DateTime EndDate
        {
            get { return base.GetDateTime(ApplyAttribute.EndDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ApplyAttribute.EndDate, value.ToString()); }
        }

        public string Content
        {
            get { return base.GetExtendedAttribute(ApplyAttribute.Content); }
            set { base.SetExtendedAttribute(ApplyAttribute.Content, value); }
        }

        public string Summary
        {
            get { return base.GetExtendedAttribute(ApplyAttribute.Summary); }
            set { base.SetExtendedAttribute(ApplyAttribute.Summary, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return ApplyAttribute.AllAttributes;
        }
    }
}
