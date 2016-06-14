using System;
using System.Data;
using System.Collections;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace BaiRong.Core.Data.Provider
{
	public interface IUserContactDAO
	{
        void Insert(UserContactInfo contactInfo);

        void Update(UserContactInfo contactInfo);

        void Delete(ArrayList deleteIDArrayList);

        void Delete(int contactID);

        UserContactInfo GetContactInfo(string relatedUserName, string createUserName, int taxis, NameValueCollection form);

        UserContactInfo GetContactInfo(int contactID);

        UserContactInfo GetContactInfo(string relatedUserName);

        string GetSelectString();

        string GetSelectString(string createUserName);

        string GetSortFieldName();
	}
}
