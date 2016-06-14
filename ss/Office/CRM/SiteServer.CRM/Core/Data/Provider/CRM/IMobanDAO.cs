using SiteServer.CRM.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.CRM.Core
{
	public interface IMobanDAO
	{
        int Insert(MobanInfo mobanInfo);

        void Update(MobanInfo mobanInfo);

        void Delete(ArrayList deleteIDArrayList);

        MobanInfo GetMobanInfo(NameValueCollection form);

        MobanInfo GetMobanInfo(int mobanID);

        int GetMobanID(string sn);

        MobanInfo GetMobanInfo(string sn);

        string GetMobanUrl(MobanInfo mobanInfo);

        bool IsInitializationForm(string sn);

        int GetCount();

        string GetSelectString();

        string GetSelectString(string sn, string keyword);

        string GetOrderByString();

        string GetSortFieldName();
	}
}
