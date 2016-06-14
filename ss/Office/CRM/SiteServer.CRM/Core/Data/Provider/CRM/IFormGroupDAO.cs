using SiteServer.CRM.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.CRM.Core
{
	public interface IFormGroupDAO
	{
        int Insert(FormGroupInfo formGroupInfo);

        void Update(FormGroupInfo formGroupInfo);

        void Delete(ArrayList deleteIDArrayList);

        FormGroupInfo GetFormGroupInfo(NameValueCollection form);

        FormGroupInfo GetFormGroupInfo(int formGroupID);

        List<FormGroupInfo> GetFormGroupInfoList(int pageID);

        int GetCount();



        IEnumerable GetDataSource(int pageID);

        string GetSelectString(int pageID);

        string GetOrderByString();

        string GetSortFieldName();

        bool UpdateTaxisToUp(int id, int pageID);

        bool UpdateTaxisToDown(int id, int pageID);
	}
}
