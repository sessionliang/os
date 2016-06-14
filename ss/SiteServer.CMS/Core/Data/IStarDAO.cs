using System;
using System.Data;
using System.Collections;

namespace SiteServer.CMS.Core
{
    public interface IStarDAO
    {
        void AddCount(int publishmentSystemID, int channelID, int contentID, string userName, int point, string message, DateTime addDate);

        int[] GetCount(int publishmentSystemID, int channelID, int contentID);

        ArrayList GetContentIDArrayListByPoint(int publishmentSystemID);
    }
}
