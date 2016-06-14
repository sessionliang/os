using SiteServer.Project.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.Project.Core
{
    public interface ITouchDAO
    {
        int Insert(TouchInfo touchInfo);

        void Update(TouchInfo touchInfo);

        void Delete(int touchID);

        TouchInfo GetTouchInfo(int touchID);

        List<TouchInfo> GetTouchInfoList(int leadID, int orderID);

        Dictionary<int, int> GetCountByLeadIDList(List<int> leadIDList);

        Dictionary<int, int> GetCountByOrderIDList(List<int> orderIDList);

        bool IsMessageIDExists(int leadID, int orderID, int messageID);
    }
}
