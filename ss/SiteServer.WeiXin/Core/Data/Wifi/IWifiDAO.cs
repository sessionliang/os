using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Core
{
    public interface IWifiDAO
    {
        int Insert(WifiInfo wifiInfo);

        void Update(WifiInfo wifiInfo);

        void AddPVCount(int wifiID);

        string GetSelectString(int publishmentSystemID);

        WifiInfo GetWifiInfo(int wifiID);

        WifiInfo GetWifiInfoByPublishmentSystemID(int publishmentSystemID);

        List<WifiInfo> GetWifiInfoListByKeywordID(int publishmentSystemID, int keywordID);

        int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID);

        List<WifiInfo> GetWifiInfoList(int publishmentSystemID);
    }
}
