using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class SubscribeUserAttribute
    {
        protected SubscribeUserAttribute()
        {
        }
         
        public const string SubscribeUserID = "SubscribeUserID";
        public const string Email = "Email";
        public const string UserID = "UserID";
        public const string Taxis = "Taxis";
        public const string Mobile = "Mobile";
        public const string SubscribeName = "SubscribeName";
        public const string PushNum = "PushNum";
        public const string SubscribeStatu = "SubscribeStatu";
        public const string IPAddress = "IPAddress";
        public const string AddDate = "AddDate";
        public const string PublishmentSystemID = "PublishmentSystemID"; 

        private static ArrayList allAttributes;
        public static ArrayList AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new ArrayList();
                    allAttributes.Add(SubscribeUserID.ToLower());
                    allAttributes.Add(Email.ToLower());
                    allAttributes.Add(UserID.ToLower());
                    allAttributes.Add(Taxis.ToLower());
                    allAttributes.Add(Mobile.ToLower());
                    allAttributes.Add(SubscribeName.ToLower());
                    allAttributes.Add(SubscribeStatu.ToLower());
                    allAttributes.Add(PushNum.ToLower());
                    allAttributes.Add(IPAddress.ToLower());
                    allAttributes.Add(AddDate.ToLower());
                    allAttributes.Add(PublishmentSystemID.ToLower()); 
                }
                return allAttributes;
            }
        } 
         
    }

    public class SubscribeUserInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_SubscribeUser";

        public SubscribeUserInfo()
        {
            this.SubscribeUserID = 0;
            this.Email = string.Empty;
            this.UserID = 0;
            this.Taxis = 0;
            this.Mobile = string.Empty;
            this.PushNum = 0;
            this.SubscribeName = string.Empty;
            this.SubscribeStatu = EBoolean.True;
            this.IPAddress = string.Empty;
            this.AddDate = DateTime.Now;
            this.PublishmentSystemID = 0; 
        }

        public SubscribeUserInfo(object dataItem)
            : base(dataItem)
		{
		}

        public SubscribeUserInfo(int id, string email, int userID, string mobile,int taxis,int pushNum,string subscribeName,string iPAddress,DateTime addDate,int publishmentSystemID, EBoolean subscribeStatu)
        {
            this.SubscribeUserID = id;
            this.Email = email;
            this.UserID = userID;
            this.Taxis = taxis;
            this.PushNum = pushNum;
            this.Mobile = mobile;
            this.SubscribeName = subscribeName;
            this.IPAddress = iPAddress;
            this.AddDate = addDate;
            this.PublishmentSystemID = publishmentSystemID;
            this.SubscribeStatu = subscribeStatu;
        }

        public int SubscribeUserID
        {
            get { return base.GetInt(SubscribeUserAttribute.SubscribeUserID, 0); }
            set { base.SetExtendedAttribute(SubscribeUserAttribute.SubscribeUserID, value.ToString()); }
        }

        public string Email
        {
            get { return base.GetExtendedAttribute(SubscribeUserAttribute.Email); }
            set { base.SetExtendedAttribute(SubscribeUserAttribute.Email, value); }
        }

        public int UserID
        {
            get { return base.GetInt(SubscribeUserAttribute.UserID,0); }
            set { base.SetExtendedAttribute(SubscribeUserAttribute.UserID, value.ToString()); }
        }

        public int Taxis
        {
            get { return base.GetInt(SubscribeUserAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(SubscribeUserAttribute.Taxis, value.ToString()); }
        }

        public string Mobile
        {
            get { return base.GetExtendedAttribute(SubscribeUserAttribute.Mobile); }
            set { base.SetExtendedAttribute(SubscribeUserAttribute.Mobile, value); }
        }

        public string SubscribeName
        {
            get { return base.GetExtendedAttribute(SubscribeUserAttribute.SubscribeName); }
            set { base.SetExtendedAttribute(SubscribeUserAttribute.SubscribeName, value); }
        }

        public int PushNum
        {
            get { return base.GetInt(SubscribeUserAttribute.PushNum,0); }
            set { base.SetExtendedAttribute(SubscribeUserAttribute.PushNum, value.ToString()); }
        }

        public string IPAddress
        {
            get { return base.GetExtendedAttribute(SubscribeUserAttribute.IPAddress); }
            set { base.SetExtendedAttribute(SubscribeUserAttribute.IPAddress, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(SubscribeSetAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.AddDate, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(SubscribeUserAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(SubscribeUserAttribute.PublishmentSystemID, value.ToString()); }
        }

        public EBoolean SubscribeStatu
        {
            get { return  EBooleanUtils.GetEnumType(SubscribeUserAttribute.SubscribeStatu); }
            set { base.SetExtendedAttribute(SubscribeUserAttribute.SubscribeStatu, value.ToString()); }
        }
            

        protected override ArrayList GetDefaultAttributesNames()
        {
            return SubscribeUserAttribute.AllAttributes;
        }
    }
}
