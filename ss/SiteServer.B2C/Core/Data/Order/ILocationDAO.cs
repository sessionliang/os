using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;
using System.Collections;
using BaiRong.Core;
using SiteServer.B2C.Model;

namespace SiteServer.B2C.Core
{
    public interface ILocationDAO
    {
        int Insert(int publishmentSystemID, LocationInfo locationInfo);

        void Update(int publishmentSystemID, LocationInfo locationInfo);

        void UpdateTaxis(int publishmentSystemID, int selectedID, bool isSubtract);

        void Delete(int publishmentSystemID, int locationID);

        int GetNodeCount(int publishmentSystemID, int locationID);

        ArrayList GetIDArrayListByParentID(int publishmentSystemID, int parentID);

        ArrayList GetIDArrayListForDescendant(int publishmentSystemID, int locationID);

        ArrayList GetIDArrayListByIDCollection(int publishmentSystemID, string locationIDCollection);

        ArrayList GetIDArrayListByFirstIDArrayList(int publishmentSystemID, ArrayList firstIDArrayList);

        DictionaryEntryArrayList GetLocationInfoDictionaryEntryArrayList(int publishmentSystemID);
    }
}
