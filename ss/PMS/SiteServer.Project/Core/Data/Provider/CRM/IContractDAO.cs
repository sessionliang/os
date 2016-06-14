using SiteServer.Project.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.Project.Core
{
	public interface IContractDAO
	{
        int Insert(ContractInfo contractInfo);

        void Update(ContractInfo contractInfo);

        void Delete(ArrayList deleteIDArrayList);

        ContractInfo GetContractInfo(NameValueCollection form);

        ContractInfo GetContractInfo(int contractID);

        ContractInfo GetContractInfoByOrderID(int orderID);

        ArrayList GetOrderIDArrayList(ArrayList idArrayList);

        int GetCount();

        string GetSelectString();

        string GetSelectString(string keyword);

        string GetContractByString();

        string GetSortFieldName();
	}
}
