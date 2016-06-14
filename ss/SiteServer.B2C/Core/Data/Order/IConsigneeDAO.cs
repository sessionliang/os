using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.B2C.Model;
using System.Collections;

namespace SiteServer.B2C.Core
{
    public interface IConsigneeDAO
    {
        int Insert(ConsigneeInfo consigneeInfo);

        void Update(ConsigneeInfo consigneeInfo);

        void SetDefault(int consigneeID);

        void Delete(int consigneeID);

        ConsigneeInfo GetConsigneeInfo(int consigneeID);

        List<ConsigneeInfo> GetConsigneeInfoList(string groupSN, string userName);
     }
}
