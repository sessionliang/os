using System.Web.UI;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using SiteServer.B2C.Model;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.Core
{
	public class SpecManager
    {
        #region Cache

        private static readonly object lockObject = new object();
        private const string cacheKeyPrefix = "SiteServer.B2C.Core.SpecManager";

        public static SpecInfo GetSpecInfo(int publishmentSystemID, int specID)
        {
            Dictionary<int, SpecInfo> dictionary = SpecManager.GetSpecInfoDictionary(publishmentSystemID);

            foreach (int key in dictionary.Keys)
            {
                if (key == specID)
                {
                    return dictionary[key];
                }
            }
            return null;
        }

        public static List<int> GetSpecIDList(int publishmentSystemID)
        {
            List<int> list = new List<int>();
            Dictionary<int, SpecInfo> dictionary = SpecManager.GetSpecInfoDictionary(publishmentSystemID);
            foreach (int key in dictionary.Keys)
            {
                list.Add(key);
            }
            return list;
        }

        public static List<SpecInfo> GetSpecInfoList(int publishmentSystemID)
        {
            List<SpecInfo> list = new List<SpecInfo>();
            Dictionary<int, SpecInfo> dictionary = SpecManager.GetSpecInfoDictionary(publishmentSystemID);
            foreach (int key in dictionary.Keys)
            {
                list.Add(dictionary[key]);
            }
            return list;
        }

        public static void ClearCache(int publishmentSystemID)
        {
            string cacheKey = cacheKeyPrefix + "." + publishmentSystemID;
            CacheUtils.Remove(cacheKey);
        }

        public static Dictionary<int, SpecInfo> GetSpecInfoDictionary(int publishmentSystemID)
        {
            lock (lockObject)
            {
                string cacheKey = cacheKeyPrefix + "." + publishmentSystemID;
                if (CacheUtils.Get(cacheKey) == null)
                {
                    Dictionary<int, SpecInfo> dictionary = DataProviderB2C.SpecDAO.GetSpecInfoDictionary(publishmentSystemID);
                    CacheUtils.Max(cacheKey, dictionary);
                    return dictionary;
                }
                return CacheUtils.Get(cacheKey) as Dictionary<int, SpecInfo>;
            }
        }

        #endregion

        public static int GetSKUCount(int publishmentSystemID, List<int> specIDList, List<int> specItemIDList)
        {
            int count = 1;
            foreach (int specID in specIDList)
            {
                List<int> list = SpecItemManager.GetSpecItemIDList(publishmentSystemID, specID);

                if (list.Count > 0)
                {
                    int itemCount = 0;
                    foreach (int specItemID in specItemIDList)
                    {
                        if (list.Contains(specItemID))
                        {
                            itemCount++;
                        }
                    }
                    if (itemCount > 0)
                    {
                        count *= itemCount;
                    }
                }
            }
            return count;
        }

        //public static List<int> GetSpecIDList(int publishmentSystemID, int nodeID)
        //{
        //    return DataProviderB2C.SpecChannelDAO.GetSpecIDList(publishmentSystemID, nodeID);
        //}

        //public static List<KeyValuePair<int, string>> GetSpecIDAndSpecNameList(int publishmentSystemID, int nodeID)
        //{
        //    return DataProviderB2C.SpecChannelDAO.GetSpecIDAndSpecNameList(publishmentSystemID, nodeID);
        //}

        public static string GetSpecValue(PublishmentSystemInfo publishmentSystemInfo, SpecComboInfo comboInfo)
        {
            string retval = string.Empty;

            SpecInfo specInfo = SpecManager.GetSpecInfo(publishmentSystemInfo.PublishmentSystemID, comboInfo.SpecID);
            if (specInfo != null)
            {
                if (specInfo.IsIcon)
                {
                    if (!string.IsNullOrEmpty(comboInfo.IconUrl))
                    {
                        retval = string.Format(@"<div><img src=""{0}"" align=""absmiddle"" /> {1}</div>", PageUtility.ParseNavigationUrl(publishmentSystemInfo, comboInfo.IconUrl), comboInfo.Title);
                    }
                    else
                    {
                        SpecItemInfo specItemInfo = SpecItemManager.GetSpecItemInfo(publishmentSystemInfo.PublishmentSystemID, comboInfo.ItemID);
                        if (specItemInfo != null)
                        {
                            retval = string.Format(@"<div><img src=""{0}"" align=""absmiddle"" /> {1}</div>", PageUtility.ParseNavigationUrl(publishmentSystemInfo, specItemInfo.IconUrl), comboInfo.Title);
                        }
                    }
                }
                else
                {
                    retval = string.Format(@"<div>{0}</div>", comboInfo.Title);
                }
            }
            return retval;
        }

        public static string GetSpecValues(PublishmentSystemInfo publishmentSystemInfo, int specID)
        {
            StringBuilder builder = new StringBuilder();

            SpecInfo specInfo = SpecManager.GetSpecInfo(publishmentSystemInfo.PublishmentSystemID, specID);
            if (specInfo != null)
            {
                List<SpecItemInfo> specItemInfoList = SpecItemManager.GetSpecItemInfoList(publishmentSystemInfo.PublishmentSystemID, specID);

                foreach (SpecItemInfo itemInfo in specItemInfoList)
                {
                    if (itemInfo.IsDefault)
                    {
                        builder.Append(@"<div class=""cur"">");
                    }
                    else
                    {
                        builder.Append("<div>");
                    }
                    if (specInfo.IsIcon)
                    {
                        builder.AppendFormat(@"<img src=""{0}"" align=""absmiddle"" /> {1}", PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.IconUrl), itemInfo.Title);
                    }
                    else
                    {
                        builder.Append(itemInfo.Title);
                    }
                    builder.Append("</div>");
                }
            }

            return builder.ToString();
        }

        public static string GetSpecValuesToAdd(PublishmentSystemInfo publishmentSystemInfo, int contentID, int specID, ArrayList specItemIDArrayList)
        {
            StringBuilder builder = new StringBuilder();

            SpecInfo specInfo = SpecManager.GetSpecInfo(publishmentSystemInfo.PublishmentSystemID, specID);
            if (specInfo != null)
            {
                List<SpecItemInfo> specItemInfoList = SpecItemManager.GetSpecItemInfoList(publishmentSystemInfo.PublishmentSystemID, specID);

                foreach (SpecItemInfo itemInfo in specItemInfoList)
                {
                    string iconClickString = SiteServer.B2C.BackgroundPages.Modal.UploadOrUrlImage.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, "comboIconUrl_" + itemInfo.ItemID, "preview_" + itemInfo.ItemID, string.Empty);
                    string selectPhotosClickString = SiteServer.B2C.BackgroundPages.Modal.GoodsPhotoSelect.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, contentID, "comboPhotoIDCollection_" + itemInfo.ItemID, "urls_" + itemInfo.ItemID);
                    string name = itemInfo.Title;
                    if (specInfo.IsIcon)
                    {
                        name = string.Format(@"<img src=""{0}"" align=""absmiddle"" /> {1}", PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.IconUrl), itemInfo.Title);
                    }
                    if (specItemIDArrayList.Contains(itemInfo.ItemID))
                    {
                        builder.AppendFormat(@"<div isSelected=""true"" itemID=""{0}"" itemTitle=""{1}"" iconUrl=""{2}"" iconClickString=""{3}"" selectPhotosClickString=""{4}"" class=""cur"" isCombo=""true""><a href=""javascript:;"" hidefocus>{5}</a></div>",
    itemInfo.ItemID, itemInfo.Title, itemInfo.IconUrl, iconClickString, selectPhotosClickString, name);
                    }
                    else
                    {
                        builder.AppendFormat(@"<div isSelected=""false"" itemID=""{0}"" itemTitle=""{1}"" iconUrl=""{2}"" iconClickString=""{3}"" selectPhotosClickString=""{4}""><a href=""javascript:;"" hidefocus>{5}</a></div>",
    itemInfo.ItemID, itemInfo.Title, itemInfo.IconUrl, iconClickString, selectPhotosClickString, name);
                    }
                }
            }

            return builder.ToString();
        }

        public static string GetSpecValues(PublishmentSystemInfo publishmentSystemInfo, GoodsInfo goodsInfo)
        {
            StringBuilder builder = new StringBuilder();

            if (goodsInfo != null)
            {
                List<SpecComboInfo> comboInfoList = DataProviderB2C.SpecComboDAO.GetSpecComboInfoList(goodsInfo.ComboIDCollection);

                foreach (SpecComboInfo comboInfo in comboInfoList)
                {
                    builder.Append(GetSpecValue(publishmentSystemInfo, comboInfo));
                }
            }

            return builder.ToString();
        }

        public static ArrayList GetComboIDCollectionArrayList(int publishmentSystemID, Dictionary<int, List<SpecComboInfo>> dictionary)
        {
            ArrayList comboIDCollectionArrayList = new ArrayList();            

            ArrayList comboInfoArrayList1 = new ArrayList();
            ArrayList comboInfoArrayList2 = new ArrayList();
            ArrayList comboInfoArrayList3 = new ArrayList();

            int i = 0;
            foreach (int specID in dictionary.Keys)
            {
                List<SpecComboInfo> comboInfoList = dictionary[specID];
                foreach (SpecComboInfo comboInfo in comboInfoList)
                {
                    if (i == 0)
                    {
                        comboInfoArrayList1.Add(comboInfo);
                    }
                    else if (i == 1)
                    {
                        comboInfoArrayList2.Add(comboInfo);
                    }
                    else if (i == 2)
                    {
                        comboInfoArrayList3.Add(comboInfo);
                    }
                }
                i++;
            }

            if (comboInfoArrayList1.Count > 0)
            {
                foreach (SpecComboInfo comboInfo1 in comboInfoArrayList1)
                {
                    string comboIDCollection = comboInfo1.ComboID.ToString();

                    if (comboInfoArrayList2.Count > 0)
                    {
                        foreach (SpecComboInfo comboInfo2 in comboInfoArrayList2)
                        {
                            comboIDCollection = comboInfo1.ComboID + "," + comboInfo2.ComboID;
                            if (comboInfoArrayList3.Count > 0)
                            {
                                foreach (SpecComboInfo comboInfo3 in comboInfoArrayList3)
                                {
                                    comboIDCollection = comboInfo1.ComboID + "," + comboInfo2.ComboID + "," + comboInfo3.ComboID;
                                    comboIDCollectionArrayList.Add(comboIDCollection);
                                }
                            }
                            else
                            {
                                comboIDCollectionArrayList.Add(comboIDCollection);
                            }
                        }
                    }
                    else if (comboInfoArrayList3.Count > 0)
                    {
                        foreach (SpecComboInfo comboInfo3 in comboInfoArrayList3)
                        {
                            comboIDCollection = comboInfo1.ComboID + "," + comboInfo3.ComboID;
                            comboIDCollectionArrayList.Add(comboIDCollection);
                        }
                    }
                    else
                    {
                        comboIDCollectionArrayList.Add(comboIDCollection);
                    }
                }
            }
            else if (comboInfoArrayList2.Count > 0)
            {
                foreach (SpecComboInfo comboInfo2 in comboInfoArrayList2)
                {
                    string comboIDCollection = comboInfo2.ComboID.ToString();
                    if (comboInfoArrayList3.Count > 0)
                    {
                        foreach (SpecComboInfo comboInfo3 in comboInfoArrayList3)
                        {
                            comboIDCollection = comboInfo2.ComboID + "," + comboInfo3.ComboID;
                            comboIDCollectionArrayList.Add(comboIDCollection);
                        }
                    }
                    else
                    {
                        comboIDCollectionArrayList.Add(comboIDCollection);
                    }
                }
            }
            else if (comboInfoArrayList3.Count > 0)
            {
                foreach (SpecComboInfo comboInfo3 in comboInfoArrayList3)
                {
                    string comboIDCollection = comboInfo3.ComboID.ToString();
                    comboIDCollectionArrayList.Add(comboIDCollection);
                }
            }

            return comboIDCollectionArrayList;
        }

        //public static ArrayList GetComboIDCollectionArrayList(int publishmentSystemID, SortedList<int, List<SpecComboInfo>> sortedList)
        //{
        //    ArrayList comboIDCollectionArrayList = new ArrayList();

        //    List<int> specIDList = new List<int>();
        //    List<int> specItemIDList = new List<int>();
        //    foreach (int specID in sortedList.Keys)
        //    {
        //        specIDList.Add(specID);
        //        List<SpecComboInfo> comboInfoList = sortedList[specID];
        //        foreach (SpecComboInfo comboInfo in comboInfoList)
        //        {
        //            specItemIDList.Add(comboInfo.ItemID);
        //        }
        //    }

        //    int count = SpecManager.GetGoodsCount(publishmentSystemID, specIDList, specItemIDList);

        //    SortedDictionary<int, int> dicIndex = new SortedDictionary<int, int>();
        //    foreach (int specID in sortedList.Keys)
        //    {
        //        dicIndex[specID] = 0;
        //    }

        //    for (int row = 0; row < 100; row++)
        //    {
        //        ArrayList arraylist = new ArrayList();

        //        foreach (int specID in sortedList.Keys)
        //        {
        //            List<SpecComboInfo> comboInfoList = sortedList[specID];

        //            int index = dicIndex[specID];

        //            if (index >= comboInfoList.Count)
        //            {
        //                index = 0;
        //            }

        //            SpecComboInfo comboInfo = comboInfoList[index];
        //            arraylist.Add(comboInfo.ComboID);

        //            dicIndex[specID] = index + 1;
        //        }

        //        string comboIDCollection = TranslateUtils.ObjectCollectionToString(arraylist);
        //        if (!comboIDCollectionArrayList.Contains(comboIDCollection))
        //        {
        //            comboIDCollectionArrayList.Add(comboIDCollection);
        //        }

        //        if (comboIDCollectionArrayList.Count == count) break;
        //    }

        //    ////i为行数
        //    //for (int i = 0; i < count; i++)
        //    //{
        //    //    StringBuilder builder = new StringBuilder();
        //    //    //j为列数
        //    //    for (int j = 0; j < sortedList.Keys.Count; j++)
        //    //    {
        //    //        int x = 0;
        //    //        foreach (int specID in sortedList.Keys)
        //    //        {
        //    //            List<SpecComboInfo> comboInfoList = sortedList[specID];
        //    //            int y = 0;
        //    //            foreach (SpecComboInfo comboInfo in comboInfoList)
        //    //            {
        //    //                if (
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    //foreach (int specID in sortedList.Keys)
        //    //{
        //    //    List<SpecComboInfo> comboInfoList = sortedList[specID];
        //    //    foreach (SpecComboInfo comboInfo in comboInfoList)
        //    //    {

        //    //        int index = 1;
        //    //        foreach (int otherSpecID in sortedList.Keys)
        //    //        {
        //    //            if (specID != otherSpecID)
        //    //            {
        //    //                List<SpecComboInfo> otherComboInfoList = sortedList[otherSpecID];
        //    //                foreach (SpecComboInfo otherComboInfo in otherComboInfoList)
        //    //                {
        //    //                    int[] array = new int[sortedList.Keys.Count];
        //    //                    array[0] = comboInfo.ComboID;
        //    //                    array[index++] = otherComboInfo.ComboID;

        //    //                    arraylist.Add(array);
        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    //ArrayList comboIDCollectionArrayList = new ArrayList();


        //    //foreach (int specID1 in sortedList.Keys)
        //    //{
        //    //    List<int> comboIDList = new List<int>();

        //    //    List<SpecComboInfo> comboInfoList1 = sortedList[specID1];

        //    //    foreach (SpecComboInfo comboInfo1 in comboInfoList1)
        //    //    {
        //    //        comboIDList.Add(comboInfo1.ComboID);

        //    //        foreach (int specID2 in sortedList.Keys)
        //    //        {
        //    //            if (specID1 != specID2)
        //    //            {
        //    //                List<SpecComboInfo> comboInfoList2 = sortedList[specID2];

        //    //                foreach (SpecComboInfo comboInfo2 in comboInfoList2)
        //    //                {
        //    //                    comboIDList.Add(comboInfo2.ComboID);
        //    //                }
        //    //            }
        //    //        }

        //    //        comboIDCollectionArrayList.Add(TranslateUtils.ObjectCollectionToString(comboIDList));
        //    //    }
        //    //}

        //    return comboIDCollectionArrayList;

        //    //if (comboInfoArrayList1.Count > 0)
        //    //{
        //    //    foreach (SpecComboInfo comboInfo1 in comboInfoArrayList1)
        //    //    {
        //    //        string comboIDCollection = comboInfo1.ComboID.ToString();

        //    //        if (comboInfoArrayList2.Count > 0)
        //    //        {
        //    //            foreach (SpecComboInfo comboInfo2 in comboInfoArrayList2)
        //    //            {
        //    //                comboIDCollection = comboInfo1.ComboID + "," + comboInfo2.ComboID;
        //    //                if (comboInfoArrayList3.Count > 0)
        //    //                {
        //    //                    foreach (SpecComboInfo comboInfo3 in comboInfoArrayList3)
        //    //                    {
        //    //                        comboIDCollection = comboInfo1.ComboID + "," + comboInfo2.ComboID + "," + comboInfo3.ComboID;
        //    //                        comboIDCollectionArrayList.Add(comboIDCollection);
        //    //                    }
        //    //                }
        //    //                else
        //    //                {
        //    //                    comboIDCollectionArrayList.Add(comboIDCollection);
        //    //                }
        //    //            }
        //    //        }
        //    //        else if (comboInfoArrayList3.Count > 0)
        //    //        {
        //    //            foreach (SpecComboInfo comboInfo3 in comboInfoArrayList3)
        //    //            {
        //    //                comboIDCollection = comboInfo1.ComboID + "," + comboInfo3.ComboID;
        //    //                comboIDCollectionArrayList.Add(comboIDCollection);
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            comboIDCollectionArrayList.Add(comboIDCollection);
        //    //        }                    
        //    //    }
        //    //}
        //    //else if (comboInfoArrayList2.Count > 0)
        //    //{
        //    //    foreach (SpecComboInfo comboInfo2 in comboInfoArrayList2)
        //    //    {
        //    //        string comboIDCollection = comboInfo2.ComboID.ToString();
        //    //        if (comboInfoArrayList3.Count > 0)
        //    //        {
        //    //            foreach (SpecComboInfo comboInfo3 in comboInfoArrayList3)
        //    //            {
        //    //                comboIDCollection = comboInfo2.ComboID + "," + comboInfo3.ComboID;
        //    //                comboIDCollectionArrayList.Add(comboIDCollection);
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            comboIDCollectionArrayList.Add(comboIDCollection);
        //    //        }
        //    //    }
        //    //}
        //    //else if (comboInfoArrayList3.Count > 0)
        //    //{
        //    //    foreach (SpecComboInfo comboInfo3 in comboInfoArrayList3)
        //    //    {
        //    //        string comboIDCollection = comboInfo3.ComboID.ToString();
        //    //        comboIDCollectionArrayList.Add(comboIDCollection);
        //    //    }
        //    //}            

        //    //return comboIDCollectionArrayList;
        //}

        public static string GetGoodsSN(string sn, int index)
        {
            return sn + "-" + index;
        }

        public static List<GoodsInfo> GetGoodsInfoList(int publishmentSystemID, NodeInfo nodeInfo, int contentID, string sn, out Dictionary<int, SpecComboInfo> comboDictionary, out List<int> specIDList)
        {
            comboDictionary = new Dictionary<int, SpecComboInfo>();
            specIDList = new List<int>();

            Dictionary<int, List<SpecComboInfo>> dictionary = new Dictionary<int, List<SpecComboInfo>>();

            foreach (int specID in DataProviderB2C.SpecDAO.GetSpecIDList(publishmentSystemID, nodeInfo.NodeID))
            {
                List<SpecComboInfo> comboInfoList = DataProviderB2C.SpecComboDAO.GetSpecComboInfoList(publishmentSystemID, contentID, specID);
                if (comboInfoList.Count > 0)
                {
                    foreach (SpecComboInfo comboInfo in comboInfoList)
                    {
                        comboDictionary[comboInfo.ComboID] = comboInfo;
                    }
                    dictionary[specID] = comboInfoList;
                    specIDList.Add(specID);
                }
            }

            List<GoodsInfo> goodsInfoList = new List<GoodsInfo>();
            List<GoodsInfo> goodsInfoListInDB = DataProviderB2C.GoodsDAO.GetGoodsInfoList(publishmentSystemID, contentID);

            ArrayList comboIDCollectionArrayList = SpecManager.GetComboIDCollectionArrayList(publishmentSystemID, dictionary);
            if (comboIDCollectionArrayList.Count > 0)
            {
                int index = 0;
                foreach (string comboIDCollection in comboIDCollectionArrayList)
                {
                    string goodsSN = SpecManager.GetGoodsSN(sn, ++index);
                    GoodsInfo goodsInfo = new GoodsInfo(0, publishmentSystemID, contentID, comboIDCollection, string.Empty, string.Empty, goodsSN, -1, -1, -1, true);
                    foreach (GoodsInfo goodsInfoInDB in goodsInfoListInDB)
                    {
                        if (goodsInfoInDB.GoodsSN == goodsSN && goodsInfoInDB.ComboIDCollection == comboIDCollection)
                        {
                            goodsInfo = goodsInfoInDB;
                            break;
                        }
                    }

                    goodsInfoList.Add(goodsInfo);
                }
            }
            else
            {
                GoodsInfo goodsInfo = new GoodsInfo(0, publishmentSystemID, contentID, string.Empty, string.Empty, string.Empty, sn, -1, -1, -1, true);
                if (goodsInfoListInDB.Count > 0)
                {
                    goodsInfo = goodsInfoListInDB[0] as GoodsInfo;
                }
                goodsInfoList.Add(goodsInfo);
            }

            return goodsInfoList;
        }

        public static void SetContentInfoToDefaultSpec(GoodsContentInfo contentInfo)
        {
            //NodeInfo nodeInfo = NodeManager.GetNodeInfo(contentInfo.PublishmentSystemID, contentInfo.NodeID);
            //ArrayList specIDArrayList = SpecManager.GetSpecIDArrayList(contentInfo.PublishmentSystemID, nodeInfo);

            //int count1 = 0;
            //if (nodeInfo.Additional.SpecID1 > 0)
            //{
            //    ArrayList itemIDArrayList = DataProvider.SpecItemDAO.GetItemIDArrayList(nodeInfo.Additional.SpecID1, ETriState.True);
            //    contentInfo.Spec1 = TranslateUtils.ObjectCollectionToString(itemIDArrayList);
            //    count1 = itemIDArrayList.Count;
            //}
            //int count2 = 0;
            //if (nodeInfo.Additional.SpecID2 > 0)
            //{
            //    ArrayList itemIDArrayList = DataProvider.SpecItemDAO.GetItemIDArrayList(nodeInfo.Additional.SpecID2, ETriState.True);
            //    contentInfo.Spec2 = TranslateUtils.ObjectCollectionToString(itemIDArrayList);
            //    count2 = itemIDArrayList.Count;
            //}
            //int count3 = 0;
            //if (nodeInfo.Additional.SpecID3 > 0)
            //{
            //    ArrayList itemIDArrayList = DataProvider.SpecItemDAO.GetItemIDArrayList(nodeInfo.Additional.SpecID3, ETriState.True);
            //    contentInfo.Spec3 = TranslateUtils.ObjectCollectionToString(itemIDArrayList);
            //    count3 = itemIDArrayList.Count;
            //}
            //int specCount = 0;
            //if (count1 > 0)
            //{
            //    specCount = (specCount == 0 ? 1 : specCount) * count1;
            //}
            //if (count2 > 0)
            //{
            //    specCount = (specCount == 0 ? 1 : specCount) * count2;
            //}
            //if (count3 > 0)
            //{
            //    specCount = (specCount == 0 ? 1 : specCount) * count3;
            //}
            //contentInfo.SpecCount = specCount;
        }

        public static void AddDefaultSpecComboListToContent(NodeInfo nodeInfo, int contentID)
        {
            //SpecManager.AddSpecComboInfoList(nodeInfo.PublishmentSystemID, contentID, nodeInfo.Additional.SpecID1);
            //SpecManager.AddSpecComboInfoList(nodeInfo.PublishmentSystemID, contentID, nodeInfo.Additional.SpecID2);
            //SpecManager.AddSpecComboInfoList(nodeInfo.PublishmentSystemID, contentID, nodeInfo.Additional.SpecID3);
        }

        private static void AddSpecComboInfoList(int publishmentSystemID, int contentID, int specID)
        {
            if (specID > 0)
            {
                List<SpecItemInfo> specItemInfoList = SpecItemManager.GetSpecItemInfoList(publishmentSystemID, specID);
                foreach (SpecItemInfo itemInfo in specItemInfoList)
                {
                    if (itemInfo.IsDefault)
                    {
                        SpecComboInfo comboInfo = new SpecComboInfo(0, publishmentSystemID, contentID, specID, itemInfo.ItemID, itemInfo.Title, string.Empty, string.Empty, 0);
                        DataProviderB2C.SpecComboDAO.Insert(comboInfo);
                    }
                }

                //List<int> itemIDList = DataProvider.SpecItemDAO.GetItemIDList(specID, ETriState.True);
                //if (itemIDList.Count > 0)
                //{
                //    foreach(int itemID in itemIDList)
                //    {
                //        SpecItemInfo itemInfo = DataProvider.SpecItemDAO.GetSpecItemInfo(itemID);
                //        if (itemInfo != null)
                //        {
                //            SpecComboInfo comboInfo = new SpecComboInfo(0, publishmentSystemID, contentID, specID, itemID, itemInfo.Title, string.Empty, string.Empty, 0);
                //            DataProvider.SpecComboDAO.Insert(comboInfo);
                //        }
                //    }
                //}
            }
        }
	}
}
