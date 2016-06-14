using SiteServer.Project.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.Project.Core
{
	public interface IContactDAO
	{
        int Insert(ContactInfo contactInfo);

        void Update(ContactInfo contactInfo);

        void Delete(ArrayList deleteIDArrayList);

        ContactInfo GetContactInfo(string loginName, string addUserName, int accountID, int leadID, NameValueCollection form);

        List<ContactInfo> GetContactInfoList(int leadID);

        ContactInfo GetContactInfo(int contactID);

        int GetCountByAccountID(int accountID);

        int GetCountByLeadID(int leadID);

        string GetSelectString(string addUserName);

        string GetSelectString(string addUserName, string keyword);

        string GetSortFieldName();
	}
}
