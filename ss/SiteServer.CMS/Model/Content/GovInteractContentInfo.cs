using System;
using BaiRong.Model;
using System.Collections;
using BaiRong.Core;
using SiteServer.CMS.Core;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.Model
{
	public class GovInteractContentInfo : ContentInfo
	{
		public GovInteractContentInfo() : base()
		{
            this.DepartmentName = string.Empty;
            this.QueryCode = GovInteractApplyManager.GetQueryCode();
            this.State = EGovInteractState.New;
            this.IPAddress = PageUtils.GetIPAddress();
            this.Location = BaiRongDataProvider.IP2CityDAO.GetCity(this.IPAddress);
            this.AddDate = DateTime.Now;
		}

        public GovInteractContentInfo(object dataItem)
            : base(dataItem)
		{
		}

        public string DepartmentName
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.DepartmentName); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.DepartmentName, value); }
        }

        public string QueryCode
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.QueryCode); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.QueryCode, value); }
        }

        public EGovInteractState State
        {
            get { return EGovInteractStateUtils.GetEnumType(base.GetExtendedAttribute(GovInteractContentAttribute.State)); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.State, EGovInteractStateUtils.GetValue(value)); }
        }

        public string IPAddress
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.IPAddress); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.IPAddress, value); }
        }

        public string Location
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.Location); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.Location, value); }
        }

        //basic

        public string RealName
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.RealName); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.RealName, value); }
        }

        public string Organization
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.Organization); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.Organization, value); }
        }

        public string CardType
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.CardType); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.CardType, value); }
        }

        public string CardNo
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.CardNo); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.CardNo, value); }
        }

        public string Phone
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.Phone); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.Phone, value); }
        }

        public string PostCode
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.PostCode); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.PostCode, value); }
        }

        public string Address
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.Address); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.Address, value); }
        }

        public string Email
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.Email); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.Email, value); }
        }

        public string Fax
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.Fax); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.Fax, value); }
        }

        public int TypeID
        {
            get { return base.GetInt(GovInteractContentAttribute.TypeID, 0); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.TypeID, value.ToString()); }
        }

        public bool IsPublic
        {
            get { return base.GetBool(GovInteractContentAttribute.IsPublic, true); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.IsPublic, value.ToString()); }
        }

        public string Content
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.Content); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.Content, value); }
        }

        public string FileUrl
        {
            get { return base.GetExtendedAttribute(GovInteractContentAttribute.FileUrl); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.FileUrl, value); }
        }

        public int DepartmentID
        {
            get { return base.GetInt(GovInteractContentAttribute.DepartmentID, 0); }
            set { base.SetExtendedAttribute(GovInteractContentAttribute.DepartmentID, value.ToString()); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return GovInteractContentAttribute.AllAttributes;
        }
	}
}
