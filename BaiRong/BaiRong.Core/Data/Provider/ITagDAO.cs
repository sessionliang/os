using BaiRong.Model;
using System.Collections;
using System;
using System.Data;
using System.Collections.Specialized;

namespace BaiRong.Core.Data.Provider
{
    public interface ITagDAO
    {
        int Insert(TagInfo tagInfo);

        void Update(TagInfo tagInfo);

        TagInfo GetTagInfo(string productID, int publishmentSystemID, string tag);

        ArrayList GetTagInfoArrayList(string productID, int publishmentSystemID, int contentID);

        ArrayList GetTagInfoArrayList(string productID, int publishmentSystemID, int contentID, bool isOrderByCount, int totalNum);

        ArrayList GetTagArrayListByStartString(string productID, int publishmentSystemID, string startString, int totalNum);

        ArrayList GetTagArrayList(string productID, int publishmentSystemID);

        void DeleteTags(string productID, int publishmentSystemID);

        void DeleteTag(string tag, string productID, int publishmentSystemID);

        int GetTagCount(string tag, string productID, int publishmentSystemID);

        ArrayList GetContentIDArrayListByTag(string tag, string productID, int publishmentSystemID);

        ArrayList GetContentIDArrayListByTagCollection(StringCollection tagCollection, string productID, int publishmentSystemID);
    }
}
