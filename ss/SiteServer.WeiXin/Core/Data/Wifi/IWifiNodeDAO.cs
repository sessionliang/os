using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Core
{
    public interface IWifiNodeDAO
    {
        int Insert(WifiNodeInfo wifiNodeInfo);
        void Update(WifiNodeInfo wifiNodeInfo);
        List<WifiNodeInfo> GetWifiNodeInfoList(int publishmentSystemID);
    }
}
