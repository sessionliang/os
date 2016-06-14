using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class GovPublicApplyAttribute
    {
        protected GovPublicApplyAttribute()
        {
        }

        //hidden
        public const string ID = "ID";
        public const string StyleID = "StyleID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string IsOrganization = "IsOrganization";
        public const string Title = "Title";
        public const string DepartmentName = "DepartmentName";
        public const string DepartmentID = "DepartmentID";
        public const string AddDate = "AddDate";
        public const string QueryCode = "QueryCode";
        public const string State = "State";

        //basic
        public const string CivicName = "CivicName";
        public const string CivicOrganization = "CivicOrganization";
        public const string CivicCardType = "CivicCardType";
        public const string CivicCardNo = "CivicCardNo";
        public const string CivicPhone = "CivicPhone";
        public const string CivicPostCode = "CivicPostCode";
        public const string CivicAddress = "CivicAddress";
        public const string CivicEmail = "CivicEmail";
        public const string CivicFax = "CivicFax";
        public const string OrgName = "OrgName";
        public const string OrgUnitCode = "OrgUnitCode";
        public const string OrgLegalPerson = "OrgLegalPerson";
        public const string OrgLinkName = "OrgLinkName";
        public const string OrgPhone = "OrgPhone";
        public const string OrgPostCode = "OrgPostCode";
        public const string OrgAddress = "OrgAddress";
        public const string OrgEmail = "OrgEmail";
        public const string OrgFax = "OrgFax";
        public const string Content = "Content";
        public const string Purpose = "Purpose";
        public const string IsApplyFree = "IsApplyFree";
        public const string ProvideType = "ProvideType";
        public const string ObtainType = "ObtainType";

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
                    hiddenAttributes.Add(StyleID.ToLower());
                    hiddenAttributes.Add(PublishmentSystemID.ToLower());
                    hiddenAttributes.Add(IsOrganization.ToLower());
                    hiddenAttributes.Add(Title.ToLower());
                    hiddenAttributes.Add(DepartmentName.ToLower());
                    hiddenAttributes.Add(DepartmentID.ToLower());
                    hiddenAttributes.Add(AddDate.ToLower());
                    hiddenAttributes.Add(QueryCode.ToString());
                    hiddenAttributes.Add(State.ToLower());
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
                    basicAttributes.Add(CivicName.ToLower());
                    basicAttributes.Add(CivicOrganization.ToLower());
                    basicAttributes.Add(CivicCardType.ToLower());
                    basicAttributes.Add(CivicCardNo.ToLower());
                    basicAttributes.Add(CivicPhone.ToLower());
                    basicAttributes.Add(CivicPostCode.ToLower());
                    basicAttributes.Add(CivicAddress.ToLower());
                    basicAttributes.Add(CivicEmail.ToLower());
                    basicAttributes.Add(CivicFax.ToLower());
                    basicAttributes.Add(OrgName.ToLower());
                    basicAttributes.Add(OrgUnitCode.ToLower());
                    basicAttributes.Add(OrgLegalPerson.ToLower());
                    basicAttributes.Add(OrgLinkName.ToLower());
                    basicAttributes.Add(OrgPhone.ToLower());
                    basicAttributes.Add(OrgPostCode.ToLower());
                    basicAttributes.Add(OrgAddress.ToLower());
                    basicAttributes.Add(OrgEmail.ToLower());
                    basicAttributes.Add(OrgFax.ToLower());
                    basicAttributes.Add(Content.ToLower());
                    basicAttributes.Add(Purpose.ToLower());
                    basicAttributes.Add(IsApplyFree.ToLower());
                    basicAttributes.Add(ProvideType.ToLower());
                    basicAttributes.Add(ObtainType.ToLower());
                }

                return basicAttributes;
            }
        }
    }

    public class GovPublicApplyInfo : ExtendedAttributes
    {
        public GovPublicApplyInfo()
        {
            this.ID = 0;
            this.StyleID = 0;
            this.PublishmentSystemID = 0;
            this.IsOrganization = false;
            this.Title = string.Empty;
            this.DepartmentName = string.Empty;
            this.DepartmentID = 0;
            this.AddDate = DateTime.Now;
            this.QueryCode = string.Empty;
            this.State = EGovPublicApplyState.New;
        }

        public GovPublicApplyInfo(object dataItem)
            : base(dataItem)
		{
		}

        public GovPublicApplyInfo(int id, int styleID, int publishmentSystemID, bool isOrganization, string title, string departmentName, int departmentID, DateTime addDate, string queryCode, EGovPublicApplyState state)
        {
            this.ID = id;
            this.StyleID = styleID;
            this.PublishmentSystemID = publishmentSystemID;
            this.IsOrganization = isOrganization;
            this.Title = title;
            this.DepartmentName = departmentName;
            this.DepartmentID = departmentID;
            this.AddDate = addDate;
            this.QueryCode = queryCode;
            this.State = state;
        }

        public int ID
        {
            get { return base.GetInt(GovPublicApplyAttribute.ID, 0); }
            set { base.SetExtendedAttribute(GovPublicApplyAttribute.ID, value.ToString()); }
        }

        public int StyleID
        {
            get { return base.GetInt(GovPublicApplyAttribute.StyleID, 0); }
            set { base.SetExtendedAttribute(GovPublicApplyAttribute.StyleID, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(GovPublicApplyAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(GovPublicApplyAttribute.PublishmentSystemID, value.ToString()); }
        }

        public bool IsOrganization
        {
            get { return base.GetBool(GovPublicApplyAttribute.IsOrganization, false); }
            set { base.SetExtendedAttribute(GovPublicApplyAttribute.IsOrganization, value.ToString()); }
        }

        public string Title
        {
            get { return base.GetExtendedAttribute(GovPublicApplyAttribute.Title); }
            set { base.SetExtendedAttribute(GovPublicApplyAttribute.Title, value); }
        }

        public string DepartmentName
        {
            get { return base.GetExtendedAttribute(GovPublicApplyAttribute.DepartmentName); }
            set { base.SetExtendedAttribute(GovPublicApplyAttribute.DepartmentName, value); }
        }

        public int DepartmentID
        {
            get { return base.GetInt(GovPublicApplyAttribute.DepartmentID, 0); }
            set { base.SetExtendedAttribute(GovPublicApplyAttribute.DepartmentID, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(GovPublicApplyAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(GovPublicApplyAttribute.AddDate, value.ToString()); }
        }

        public string QueryCode
        {
            get { return base.GetExtendedAttribute(GovPublicApplyAttribute.QueryCode); }
            set { base.SetExtendedAttribute(GovPublicApplyAttribute.QueryCode, value); }
        }

        public EGovPublicApplyState State
        {
            get { return EGovPublicApplyStateUtils.GetEnumType(base.GetExtendedAttribute(GovPublicApplyAttribute.State)); }
            set { base.SetExtendedAttribute(GovPublicApplyAttribute.State, EGovPublicApplyStateUtils.GetValue(value)); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return GovPublicApplyAttribute.AllAttributes;
        }
    }

    //public class GovPublicApplyInfo
    //{
    //    private int applicantionID;
    //    private int publishmentSystemID;
    //    private bool isOrganization;
    //    private string civicName;
    //    private string civicOrganization;
    //    private string civicCardType;
    //    private string civicCardNo;
    //    private string civicPhone;
    //    private string civicPostCode;
    //    private string civicAddress;
    //    private string civicEmail;
    //    private string civicFax;
    //    private string orgName;
    //    private string orgUnitCode;
    //    private string orgLegalPerson;
    //    private string orgLinkName;
    //    private string orgPhone;
    //    private string orgPostCode;
    //    private string orgAddress;
    //    private string orgEmail;
    //    private string orgFax;
    //    private string title;
    //    private string content;
    //    private string purpose;
    //    private bool isApplyFree;
    //    private string provideType;
    //    private string obtainType;
    //    private int departmentID;
    //    private DateTime addDate;

    //    public GovPublicApplyInfo()
    //    {
    //        this.applicantionID = 0;
    //        this.publishmentSystemID = 0;
    //        this.isOrganization = false;
    //        this.civicName = string.Empty;
    //        this.civicOrganization = string.Empty;
    //        this.civicCardType = string.Empty;
    //        this.civicCardNo = string.Empty;
    //        this.civicPhone = string.Empty;
    //        this.civicPostCode = string.Empty;
    //        this.civicAddress = string.Empty;
    //        this.civicEmail = string.Empty;
    //        this.civicFax = string.Empty;
    //        this.orgName = string.Empty;
    //        this.orgUnitCode = string.Empty;
    //        this.orgLegalPerson = string.Empty;
    //        this.orgLinkName = string.Empty;
    //        this.orgPhone = string.Empty;
    //        this.orgPostCode = string.Empty;
    //        this.orgAddress = string.Empty;
    //        this.orgEmail = string.Empty;
    //        this.orgFax = string.Empty;
    //        this.title = string.Empty;
    //        this.content = string.Empty;
    //        this.purpose = string.Empty;
    //        this.isApplyFree = false;
    //        this.provideType = string.Empty;
    //        this.obtainType = string.Empty;
    //        this.departmentID = 0;
    //        this.addDate = DateTime.Now;
    //    }

    //    public GovPublicApplyInfo(int applicantionID, int publishmentSystemID, bool isOrganization, string civicName, string civicOrganization, string civicCardType, string civicCardNo, string civicPhone, string civicPostCode, string civicAddress, string civicEmail, string civicFax, string orgName, string orgUnitCode, string orgLegalPerson, string orgLinkName, string orgPhone, string orgPostCode, string orgAddress, string orgEmail, string orgFax, string title, string content, string purpose, bool isApplyFree, string provideType, string obtainType, int departmentID, DateTime addDate) 
    //    {
    //        this.applicantionID = applicantionID;
    //        this.publishmentSystemID = publishmentSystemID;
    //        this.isOrganization = isOrganization;
    //        this.civicName = civicName;
    //        this.civicOrganization = civicOrganization;
    //        this.civicCardType = civicCardType;
    //        this.civicCardNo = civicCardNo;
    //        this.civicPhone = civicPhone;
    //        this.civicPostCode = civicPostCode;
    //        this.civicAddress = civicAddress;
    //        this.civicEmail = civicEmail;
    //        this.civicFax = civicFax;
    //        this.orgName = orgName;
    //        this.orgUnitCode = orgUnitCode;
    //        this.orgLegalPerson = orgLegalPerson;
    //        this.orgLinkName = orgLinkName;
    //        this.orgPhone = orgPhone;
    //        this.orgPostCode = orgPostCode;
    //        this.orgAddress = orgAddress;
    //        this.orgEmail = orgEmail;
    //        this.orgFax = orgFax;
    //        this.title = title;
    //        this.content = content;
    //        this.purpose = purpose;
    //        this.isApplyFree = isApplyFree;
    //        this.provideType = provideType;
    //        this.obtainType = obtainType;
    //        this.departmentID = departmentID;
    //        this.addDate = addDate;
    //    }

    //    public int ApplicantionID
    //    {
    //        get { return applicantionID; }
    //        set { applicantionID = value; }
    //    }

    //    public int PublishmentSystemID
    //    {
    //        get { return publishmentSystemID; }
    //        set { publishmentSystemID = value; }
    //    }

    //    public bool IsOrganization
    //    {
    //        get { return isOrganization; }
    //        set { isOrganization = value; }
    //    }

    //    public string CivicName
    //    {
    //        get { return civicName; }
    //        set { civicName = value; }
    //    }

    //    public string CivicOrganization
    //    {
    //        get { return civicOrganization; }
    //        set { civicOrganization = value; }
    //    }

    //    public string CivicCardType
    //    {
    //        get { return civicCardType; }
    //        set { civicCardType = value; }
    //    }

    //    public string CivicCardNo
    //    {
    //        get { return civicCardNo; }
    //        set { civicCardNo = value; }
    //    }

    //    public string CivicPhone
    //    {
    //        get { return civicPhone; }
    //        set { civicPhone = value; }
    //    }

    //    public string CivicPostCode
    //    {
    //        get { return civicPostCode; }
    //        set { civicPostCode = value; }
    //    }

    //    public string CivicAddress
    //    {
    //        get { return civicAddress; }
    //        set { civicAddress = value; }
    //    }

    //    public string CivicEmail
    //    {
    //        get { return civicEmail; }
    //        set { civicEmail = value; }
    //    }

    //    public string CivicFax
    //    {
    //        get { return civicFax; }
    //        set { civicFax = value; }
    //    }

    //    public string OrgName
    //    {
    //        get { return orgName; }
    //        set { orgName = value; }
    //    }

    //    public string OrgUnitCode
    //    {
    //        get { return orgUnitCode; }
    //        set { orgUnitCode = value; }
    //    }

    //    public string OrgLegalPerson
    //    {
    //        get { return orgLegalPerson; }
    //        set { orgLegalPerson = value; }
    //    }

    //    public string OrgLinkName
    //    {
    //        get { return orgLinkName; }
    //        set { orgLinkName = value; }
    //    }

    //    public string OrgPhone
    //    {
    //        get { return orgPhone; }
    //        set { orgPhone = value; }
    //    }

    //    public string OrgPostCode
    //    {
    //        get { return orgPostCode; }
    //        set { orgPostCode = value; }
    //    }

    //    public string OrgAddress
    //    {
    //        get { return orgAddress; }
    //        set { orgAddress = value; }
    //    }

    //    public string OrgEmail
    //    {
    //        get { return orgEmail; }
    //        set { orgEmail = value; }
    //    }

    //    public string OrgFax
    //    {
    //        get { return orgFax; }
    //        set { orgFax = value; }
    //    }

    //    public string Title
    //    {
    //        get { return title; }
    //        set { title = value; }
    //    }

    //    public string Content
    //    {
    //        get { return content; }
    //        set { content = value; }
    //    }

    //    public string Purpose
    //    {
    //        get { return purpose; }
    //        set { purpose = value; }
    //    }

    //    public bool IsApplyFree
    //    {
    //        get { return isApplyFree; }
    //        set { isApplyFree = value; }
    //    }

    //    public string ProvideType
    //    {
    //        get { return provideType; }
    //        set { provideType = value; }
    //    }

    //    public string ObtainType
    //    {
    //        get { return obtainType; }
    //        set { obtainType = value; }
    //    }

    //    public int DepartmentID
    //    {
    //        get { return departmentID; }
    //        set { departmentID = value; }
    //    }

    //    public DateTime AddDate
    //    {
    //        get{ return addDate; }
    //        set{ addDate = value; }
    //    }
    //}
}
