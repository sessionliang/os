using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using BaiRong.Model;
using SiteServer.B2C.Model;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.Core
{
    public interface IGoodsContentDAO
    {
        ArrayList GetCountArrayListUnChecked(bool isSystemAdministrator, int publishmentSystemID, ArrayList owningNodeIDArrayList, string tableName);

        GoodsContentInfo GetContentInfo(string tableName, int contentID);

        GoodsContentInfo GetContentInfo(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID);

        void UpdateSpec(string tableName, int publishmentSystemID, int contentID, List<int> specIDList, List<int> specItemIDList);

        void UpdateGoodContentCount(string tableName, int contentID, int count);

        int GetCountCheckedImage(int publishmentSystemID, int nodeID);

        int GetCountOfGoods(int publishmentSystemID, int brandContentID);

        string GetStlWhereString(PublishmentSystemInfo publishmentSystemInfo, string group, string groupNot, string filterName, string filterValue, int channelID, string tags, AttributesInfo attributesInfo, string where);

        string GetSelectCommendByDownloads(string tableName, int publishmentSystemID);

        int GetBrandID(int publishmentSystemID, int contentID);

        decimal GetPriceSale(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID);
    }
}
