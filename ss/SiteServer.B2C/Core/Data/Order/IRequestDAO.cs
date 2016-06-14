using SiteServer.B2C.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.B2C.Core
{
	public interface IRequestDAO
	{
        int Insert(RequestInfo requestInfo);

        void Update(RequestInfo requestInfo);

        void UpdateStatus(int requestID, ERequestStatus status);

        void Delete(ArrayList deleteIDArrayList);

        RequestInfo GetRequestInfo(int requestID);

        List<RequestInfo> GetRequestInfoList(string userName);

        RequestInfo GetLastRequestInfo(string userName);

        int GetCount();

        string GetSelectString(string userName);

        string GetSelectString(string userName, string status, string keyword);

        string GetSortFieldName();

        void Estimate(int requestID, ERequestEstimate estimateValue, string estimateComment);
	}
}
