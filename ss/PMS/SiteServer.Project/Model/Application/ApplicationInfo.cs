using BaiRong.Core;
using BaiRong.Core.Data;
using System;
using System.Web;
using System.Xml.Serialization;

namespace SiteServer.Project.Model
{
    public class ApplicationInfo
    {
        private int id;
        private EApplicationType applicationType;
        private string applyResource;
        private string ipAddress;
        private DateTime addDate;
        private string contactPerson;
        private string email;
        private string mobile;
        private string qq;
        private string telephone;
        private string location;
        private string address;
        private string orgType;
        private string orgName;
        private bool isITDepartment;
        private string comment;
        private bool isHandle;
        private DateTime handleDate;
        private string handleUserName;
        private string handleSummary;

        public ApplicationInfo()
        {
            this.id = 0;
            this.applicationType = EApplicationType.Document;
            this.applyResource = string.Empty;
            this.ipAddress = string.Empty;
            this.addDate = DateTime.Now;
            this.contactPerson = string.Empty;
            this.email = string.Empty;
            this.mobile = string.Empty;
            this.qq = string.Empty;
            this.telephone = string.Empty;
            this.location = string.Empty;
            this.address = string.Empty;
            this.orgType = string.Empty;
            this.orgName = string.Empty;
            this.isITDepartment = false;
            this.comment = string.Empty;
            this.isHandle = false;
            this.handleDate = DateUtils.SqlMinValue;
            this.handleUserName = string.Empty;
            this.handleSummary = string.Empty;
        }

        public ApplicationInfo(int id, EApplicationType applicationType, string applyResource, string ipAddress, DateTime addDate, string contactPerson, string email, string mobile, string qq, string telephone, string location, string address, string orgType, string orgName, bool isITDepartment, string comment, bool isHandle, DateTime handleDate, string handleUserName, string handleSummary)
        {
            this.id = id;
            this.applicationType = applicationType;
            this.applyResource = applyResource;
            this.ipAddress = ipAddress;
            this.addDate = addDate;
            this.contactPerson = contactPerson;
            this.email = email;
            this.mobile = mobile;
            this.qq = qq;
            this.telephone = telephone;
            this.location = location;
            this.address = address;
            this.orgType = orgType;
            this.orgName = orgName;
            this.isITDepartment = isITDepartment;
            this.comment = comment;
            this.isHandle = isHandle;
            this.handleDate = handleDate;
            this.handleUserName = handleUserName;
            this.handleSummary = handleSummary;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public EApplicationType ApplicationType
        {
            get { return applicationType; }
            set { applicationType = value; }
        }

        public string ApplyResource
        {
            get { return applyResource; }
            set { applyResource = value; }
        }

        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public string ContactPerson
        {
            get { return contactPerson; }
            set { contactPerson = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        public string QQ
        {
            get { return qq; }
            set { qq = value; }
        }

        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string OrgType
        {
            get { return orgType; }
            set { orgType = value; }
        }

        public string OrgName
        {
            get { return orgName; }
            set { orgName = value; }
        }

        public bool IsITDepartment
        {
            get { return isITDepartment; }
            set { isITDepartment = value; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public bool IsHandle
        {
            get { return isHandle; }
            set { isHandle = value; }
        }

        public DateTime HandleDate
        {
            get { return handleDate; }
            set { handleDate = value; }
        }

        public string HandleUserName
        {
            get { return handleUserName; }
            set { handleUserName = value; }
        }

        public string HandleSummary
        {
            get { return handleSummary; }
            set { handleSummary = value; }
        }
    }
}
