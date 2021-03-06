using SiteServer.B2C.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.B2C.Core
{
	public interface IRequestAnswerDAO
	{
        int Insert(RequestAnswerInfo answerInfo);

        void Update(RequestAnswerInfo answerInfo);

        void Delete(ArrayList deleteIDArrayList);

        RequestAnswerInfo GetAnswerInfo(int answerID);

        RequestAnswerInfo GetFirstAnswerInfoByRequestID(int requestID);

        List<RequestAnswerInfo> GetAnswerInfoList(int requestID);

        int GetCount(int requestID);

        string GetSelectString(int requestID);

        string GetSortFieldName();
	}
}
