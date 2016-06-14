using System.Web.UI;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;

namespace BaiRong.Core
{
    public class TagUtils
    {
        private TagUtils()
        {
        }

        public static void AddTags(StringCollection tags, string productID, int publishmentSystemID, int contentID)
        {
            if (tags.Count > 0)
            {
                foreach (string tagName in tags)
                {
                    TagInfo tagInfo = BaiRongDataProvider.TagDAO.GetTagInfo(productID, publishmentSystemID,PageUtils.FilterXSS(tagName));
                    if (tagInfo != null)
                    {
                        ArrayList contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(tagInfo.ContentIDCollection);
                        if (!contentIDArrayList.Contains(tagName))
                        {
                            contentIDArrayList.Add(contentID);
                            tagInfo.ContentIDCollection = TranslateUtils.ObjectCollectionToString(contentIDArrayList);
                            tagInfo.UseNum = contentIDArrayList.Count;
                            BaiRongDataProvider.TagDAO.Update(tagInfo);
                        }
                    }
                    else
                    {
                        tagInfo = new TagInfo(0, productID, publishmentSystemID, contentID.ToString(), tagName, contentID > 0 ? 1 : 0);
                        BaiRongDataProvider.TagDAO.Insert(tagInfo);
                    }
                }
            }
        }

        public static void UpdateTags(string tagsLast, string tagsNow, StringCollection tagCollection, string productID, int publishmentSystemID, int contentID)
        {
            if (tagsLast != tagsNow)
            {
                ArrayList tagsArrayList = TranslateUtils.StringCollectionToArrayList(tagsLast);
                foreach (string tag in tagsArrayList)
                {
                    if (!tagCollection.Contains(tag))//删除
                    {
                        TagInfo tagInfo = BaiRongDataProvider.TagDAO.GetTagInfo(productID, publishmentSystemID, tag);
                        if (tagInfo != null)
                        {
                            ArrayList contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(tagInfo.ContentIDCollection);
                            contentIDArrayList.Remove(contentID);
                            tagInfo.ContentIDCollection = TranslateUtils.ObjectCollectionToString(contentIDArrayList);
                            tagInfo.UseNum = contentIDArrayList.Count;
                            BaiRongDataProvider.TagDAO.Update(tagInfo);
                        }
                    }
                }

                StringCollection tagsToAdd = new StringCollection();
                foreach (string tag in tagCollection)
                {
                    if (!tagsArrayList.Contains(tag))
                    {
                        tagsToAdd.Add(tag);
                    }
                }

                TagUtils.AddTags(tagsToAdd, productID, publishmentSystemID, contentID);
            }
        }

        public static void RemoveTags(string productID, int publishmentSystemID, ArrayList contentIDArrayList)
        {
            foreach (int contentID in contentIDArrayList)
            {
                TagUtils.RemoveTags(productID, publishmentSystemID, contentID);
            }
        }

        public static void RemoveTags(string productID, int publishmentSystemID, int contentID)
        {
            ArrayList tagInfoArrayList = BaiRongDataProvider.TagDAO.GetTagInfoArrayList(productID, publishmentSystemID, contentID);
            if (tagInfoArrayList.Count > 0)
            {
                foreach (TagInfo tagInfo in tagInfoArrayList)
                {
                    ArrayList contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(tagInfo.ContentIDCollection);
                    contentIDArrayList.Remove(contentID);
                    tagInfo.ContentIDCollection = TranslateUtils.ObjectCollectionToString(contentIDArrayList);
                    tagInfo.UseNum = contentIDArrayList.Count;
                    BaiRongDataProvider.TagDAO.Update(tagInfo);
                }
            }
        }

        public static string GetTagsString(StringCollection tags)
        {
            StringBuilder tagsBuilder = new StringBuilder();
            if (tags != null && tags.Count > 0)
            {
                foreach (string tag in tags)
                {
                    if (tag.Trim().IndexOf(",") != -1)
                    {
                        tagsBuilder.AppendFormat("\"{0}\"", tag);
                    }
                    else
                    {
                        tagsBuilder.Append(tag);
                    }
                    tagsBuilder.Append(" ");
                }
                --tagsBuilder.Length;
            }
            return tagsBuilder.ToString();
        }

        public static StringCollection ParseTagsString(string tagsString)
        {
            StringCollection stringCollection = new StringCollection();

            if (!string.IsNullOrEmpty(tagsString))
            {
                Regex regex = new Regex("\"([^\"]*)\"", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                MatchCollection mc = regex.Matches(tagsString);
                for (int i = 0; i < mc.Count; i++)
                {
                    if (!string.IsNullOrEmpty(mc[i].Value))
                    {
                        string tag = mc[i].Value.Replace("\"", string.Empty);
                        if (!stringCollection.Contains(tag))
                        {
                            stringCollection.Add(tag);
                        }

                        int startIndex = tagsString.IndexOf(mc[i].Value);
                        if (startIndex != -1)
                        {
                            tagsString = tagsString.Substring(0, startIndex) + tagsString.Substring(startIndex + mc[i].Value.Length);
                        }
                    }
                }

                regex = new Regex("([^,;\\s]+)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                mc = regex.Matches(tagsString);
                for (int i = 0; i < mc.Count; i++)
                {
                    if (!string.IsNullOrEmpty(mc[i].Value))
                    {
                        string tag = mc[i].Value.Replace("\"", string.Empty);
                        if (!stringCollection.Contains(tag))
                        {
                            stringCollection.Add(tag);
                        }
                    }
                }
            }

            return stringCollection;
        }

        public static ArrayList GetTagInfoArrayList(string productID, int publishmentSystemID)
        {
            return GetTagInfoArrayList(productID, publishmentSystemID, 0, 0, false, 0);
        }

        public static ArrayList GetTagInfoArrayList(string productID, int publishmentSystemID, int contentID, int totalNum, bool isOrderByCount, int tagLevel)
        {
            ArrayList tagInfoArrayList = BaiRongDataProvider.TagDAO.GetTagInfoArrayList(productID, publishmentSystemID, contentID, isOrderByCount, totalNum);

            int totalCount = BaiRongDataProvider.TagDAO.GetTagCount(null, productID, publishmentSystemID);

            ArrayList arraylist = new ArrayList();
            SortedList sortedlist = new SortedList();
            foreach (TagInfo tagInfo in tagInfoArrayList)
            {
                arraylist.Add(tagInfo);

                ArrayList tagNames = (ArrayList)sortedlist[tagInfo.UseNum];
                if (tagNames == null || tagNames.Count == 0)
                {
                    tagNames = new ArrayList();
                }
                tagNames.Add(tagInfo.Tag);
                sortedlist[tagInfo.UseNum] = tagNames;
            }

            int count1 = 1;
            int count2 = 2;
            int count3 = 3;
            if (sortedlist.Keys.Count > 3)
            {
                count1 = (int)Math.Ceiling(0.3 * sortedlist.Keys.Count);
                if (count1 < 1)
                {
                    count1 = 1;
                }
                count2 = (int)Math.Ceiling(0.7 * sortedlist.Keys.Count);
                if (count2 == sortedlist.Keys.Count)
                {
                    count2--;
                }
                if (count2 <= count1)
                {
                    count2++;
                }
                count3 = count2 + 1;
            }

            int currentCount = 0;
            foreach (int count in sortedlist.Keys)
            {
                currentCount++;

                int level = 1;

                if (currentCount <= count1)
                {
                    level = 1;
                }
                else if (currentCount > count1 && currentCount <= count2)
                {
                    level = 2;
                }
                else if (currentCount > count2 && currentCount <= count3)
                {
                    level = 3;
                }
                else if (currentCount > count3)
                {
                    level = 4;
                }

                ArrayList tagNames = (ArrayList)sortedlist[count];
                foreach (TagInfo tagInfo in arraylist)
                {
                    if (tagNames.Contains(tagInfo.Tag))
                    {
                        tagInfo.Level = level;
                    }
                }
            }

            if (tagLevel > 1)
            {
                ArrayList levelArrayList = new ArrayList();
                foreach (TagInfo tagInfo in arraylist)
                {
                    if (tagInfo.Level >= tagLevel)
                    {
                        levelArrayList.Add(tagInfo);
                    }
                    if (totalNum > 0 && levelArrayList.Count > totalNum)
                    {
                        break;
                    }
                }
                arraylist = levelArrayList;
            }

            return arraylist;
        }
    }
}
