using SiteServer.CRM.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.CRM.Core
{
	public interface IOrderMessageDAO
	{
        int Insert(OrderMessageInfo messageInfo);

        void Update(OrderMessageInfo messageInfo);

        void Delete(ArrayList deleteIDArrayList);

        OrderMessageInfo GetOrderMessageInfo(int messageID);

        string GetSelectString();

        string GetOrderByString();

        string GetSortFieldName();
	}
}
