using System;
using System.Data;
using System.Collections;

namespace SiteServer.CMS.Core
{
    public interface IStarSettingDAO
    {
        void SetStarSetting(int publishmentSystemID, int channelID, int contentID, int totalCount, decimal pointAverage);

        object[] GetTotalCountAndPointAverage(int publishmentSystemID, int contentID);
    }
}
