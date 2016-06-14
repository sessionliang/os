using SiteServer.CRM.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;

namespace SiteServer.CRM.Core
{
	public interface IRequestAnswerDAO
	{
        int Insert(RequestAnswerInfo answerInfo);

        void Update(RequestAnswerInfo answerInfo);

        void Delete(ArrayList deleteIDArrayList);

        RequestAnswerInfo GetAnswerInfo(NameValueCollection form);

        RequestAnswerInfo GetAnswerInfo(int answerID);

        RequestAnswerInfo GetFirstAnswerInfoByRequestID(int requestID);

        int GetCount(int requestID);

        string GetSelectString(int requestID);

        string GetSortFieldName();
	}
}
