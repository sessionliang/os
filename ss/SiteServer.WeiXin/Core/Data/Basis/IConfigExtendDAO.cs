using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IConfigExtendDAO
    {
        int Insert(ConfigExtendInfo configExtendInfo);

        void Update(ConfigExtendInfo configExtendInfo);

        void UpdateFuctionID(int publishmentSystemID, int functionID);

        void DeleteAllNotInIDList(int publishmentSystemID, int fuctionID, List<int> idList);

        List<ConfigExtendInfo> GetConfigExtendInfoList(int publishmentSystemID, int functionID, string keywordType);
    }
}
