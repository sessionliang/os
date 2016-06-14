using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;
using System.Collections;

namespace BaiRong.Core.Data.Provider
{
    public interface IAreaDAO
    {
        int Insert(AreaInfo areaInfo);

        void Update(AreaInfo areaInfo);

        void UpdateTaxis(int selectedAreaID, bool isSubtract);

        void UpdateCountOfAdmin();

        void Delete(int areaID);

        int GetNodeCount(int areaID);

        ArrayList GetAreaIDArrayListByParentID(int parentID);

        ArrayList GetAreaIDArrayListForDescendant(int areaID);

        ArrayList GetAreaIDArrayListByAreaIDCollection(string areaIDCollection);

        ArrayList GetAreaIDArrayListByFirstAreaIDArrayList(ArrayList firstIDArrayList);

        DictionaryEntryArrayList GetAreaInfoDictionaryEntryArrayList();
    }
}
