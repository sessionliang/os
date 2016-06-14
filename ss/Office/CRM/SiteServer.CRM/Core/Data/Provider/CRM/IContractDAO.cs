using SiteServer.CRM.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.CRM.Core
{
	public interface IContractDAO
	{
        int Insert(ContractInfo contractInfo);

        void Update(ContractInfo contractInfo);

        void Delete(List<int> deleteIDList);

        ContractInfo GetContractInfo(int contractID);

        ContractInfo GetContractInfoByOrderID(int orderID);

        List<int> GetOrderIDList(List<int> idList);

        int GetCount();

        string GetSelectString();

        string GetSelectString(string keyword);

        string GetContractByString();

        string GetSortFieldName();
	}
}
