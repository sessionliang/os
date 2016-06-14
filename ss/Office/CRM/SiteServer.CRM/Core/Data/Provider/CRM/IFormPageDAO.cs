using SiteServer.CRM.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.CRM.Core
{
	public interface IFormPageDAO
	{
        int Insert(FormPageInfo formPageInfo);

        void Update(FormPageInfo formPageInfo);

        void Delete(ArrayList deleteIDArrayList);

        FormPageInfo GetFormPageInfo(NameValueCollection form);

        FormPageInfo GetFormPageInfo(int formPageID);

        int GetNextPageID(int mobanID, int formPageID);

        bool IsCompleted(int mobanID, int formPageID);

        List<int> GetPageIDList(int mobanID);

        Dictionary<int, string> GetPages(int mobanID);

        int GetCount();

        IEnumerable GetDataSource(int mobanID);

        string GetSelectString(int mobanID);

        string GetOrderByString();

        string GetSortFieldName();

        bool UpdateTaxisToUp(int id, int mobanID);

        bool UpdateTaxisToDown(int id, int mobanID);
	}
}
