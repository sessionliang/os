using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.Model
{
    public class OrganizationInfoAttribute
    {
        protected OrganizationInfoAttribute()
        {
        }

        public const string ID = "ID";
        public const string ClassifyID = "ClassifyID";
        public const string OrganizationName = "OrganizationName";
        public const string AreaID = "AreaID";
        public const string OrganizationAddress = "OrganizationAddress";
        public const string Explain = "Explain";
        public const string Phone = "Phone";
        public const string Longitude = "Longitude";
        public const string Latitude = "Latitude";
        public const string LogoUrl = "LogoUrl";
        public const string ContentNum = "ContentNum";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string Enabled = "Enabled"; 
        public const string Taxis = "Taxis";
        public const string AddDate = "AddDate";
        public const string UserName = "UserName";

        private static ArrayList allAttributes;
        public static ArrayList AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new ArrayList(otherAttributes);
                    allAttributes.Add(ID.ToLower()); 
                }
                return allAttributes;
            }
        }


        private static ArrayList otherAttributes;
        public static ArrayList OtherAttributes
        {
            get
            {
                if (otherAttributes == null)
                {
                    otherAttributes = new ArrayList();
                    otherAttributes.Add(OrganizationName.ToLower());
                    otherAttributes.Add(ClassifyID.ToLower());
                    otherAttributes.Add(Taxis.ToLower()); 
                    otherAttributes.Add(AddDate.ToLower());
                    otherAttributes.Add(PublishmentSystemID.ToLower());
                    otherAttributes.Add(UserName.ToLower());
                    otherAttributes.Add(AreaID.ToLower());
                    otherAttributes.Add(OrganizationAddress.ToLower());
                    otherAttributes.Add(Explain.ToLower());
                    otherAttributes.Add(Longitude.ToLower());
                    otherAttributes.Add(Phone.ToLower());
                    otherAttributes.Add(Latitude.ToLower());
                    otherAttributes.Add(LogoUrl.ToLower());
                    otherAttributes.Add(ContentNum.ToLower());
                    otherAttributes.Add(Enabled.ToLower()); 
                }
                return otherAttributes;
            }
        }
         
    }

    public class OrganizationInfo :ExtendedAttributes
    {
        public const string TableName = "siteserver_OrganizationInfo";

        public OrganizationInfo()
        {
            this.ID = 0;
            this.ClassifyID = 0;
            this.OrganizationName = string.Empty;
            this.AreaID = 0;
            this.OrganizationAddress = string.Empty;
            this.Explain = string.Empty;
            this.Phone = string.Empty;
            this.Longitude = string.Empty;
            this.Latitude = string.Empty;
            this.LogoUrl = string.Empty; 
            this.ContentNum = 0;
            this.Enabled =false;
            this.PublishmentSystemID = 0;
            this.Taxis = 0;  
            this.AddDate = DateTime.Now;
            this.UserName = string.Empty;
        }

        public OrganizationInfo(object dataItem)
            : base(dataItem)
		{
		}


        public OrganizationInfo(int id, int classifyID, string name, int areaID, string address, string explain, string phone, string longitude, string latitude, string logoUrl, int contentNum, bool enabled, int publishmentSystemID, int taxis, DateTime addDate, string userName)
        {
            this.ID = id;
            this.ClassifyID = classifyID;
            this.OrganizationName = name;
            this.AreaID = areaID;
            this.OrganizationAddress = address;
            this.Explain = explain;
            this.Phone = phone;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.LogoUrl = logoUrl;
            this.ContentNum = contentNum;
            this.Enabled = enabled;
            this.PublishmentSystemID = publishmentSystemID;
            this.Taxis = taxis;
            this.AddDate = addDate;
            this.UserName = userName;
        }

        public int ID
        {
            get { return base.GetInt(OrganizationInfoAttribute.ID, 0); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.ID, value.ToString()); }
        }

        public string OrganizationName
        {
            get { return base.GetExtendedAttribute(OrganizationInfoAttribute.OrganizationName); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.OrganizationName, value); }
        }

        public int ClassifyID
        {
            get { return base.GetInt(OrganizationInfoAttribute.ClassifyID, 0); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.ClassifyID, value.ToString()); }
        }

        public new int Taxis
        {
            get { return base.GetInt(OrganizationInfoAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.Taxis, value.ToString()); }
        }
          
        public DateTime AddDate
        {
            get { return base.GetDateTime(OrganizationInfoAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.AddDate, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(OrganizationInfoAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.PublishmentSystemID, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(OrganizationInfoAttribute.UserName); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.UserName, value); }
        }

        public string OrganizationAddress
        {
            get { return base.GetExtendedAttribute(OrganizationInfoAttribute.OrganizationAddress); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.OrganizationAddress, value); }
        }
        public string Explain
        {
            get { return base.GetExtendedAttribute(OrganizationInfoAttribute.Explain); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.Explain, value); }
        }

        public int AreaID
        {
            get { return base.GetInt(OrganizationInfoAttribute.AreaID, 0); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.AreaID, value.ToString()); }
        }

        public string Phone
        {
            get { return base.GetExtendedAttribute(OrganizationInfoAttribute.Phone); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.Phone, value); }
        }

        public string Longitude
        {
            get { return base.GetString(OrganizationInfoAttribute.Longitude, string.Empty); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.Longitude, value); }
        }


        public string Latitude
        {
            get { return base.GetString(OrganizationInfoAttribute.Latitude, string.Empty); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.Latitude, value); }
        }

        public string LogoUrl
        {
            get { return base.GetExtendedAttribute(OrganizationInfoAttribute.LogoUrl); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.LogoUrl, value); }
        }

        public int ContentNum
        {
            get { return base.GetInt(OrganizationInfoAttribute.ContentNum, 0); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.ContentNum, value.ToString()); }
        }


        public bool Enabled
        {
            get { return base.GetBool(OrganizationInfoAttribute.Enabled,false); }
            set { base.SetExtendedAttribute(OrganizationInfoAttribute.Enabled, value.ToString()); }
        }


        protected override ArrayList GetDefaultAttributesNames()
        {
            return OrganizationInfoAttribute.OtherAttributes;
        }
    }
}
