using System;
using System.Text;
using System.Xml;
using System.Collections;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.IO;
using BaiRong.Core.Net;
using SiteServer.CMS.Model;
using System.Web.Caching;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.Core
{
    public class CreateCacheManager
    {
        private CreateCacheManager()
        {
        }

        public class FirstContentID
        {
            private FirstContentID()
            {
            }

            private const string cacheKey = "SiteServer.CreateCache.FirstContentID";

            public static Hashtable GetHashtable()
            {
                Hashtable ht = CacheUtils.Get(cacheKey) as Hashtable;
                if (ht == null)
                {
                    ht = new Hashtable();
                    CacheUtils.Insert(cacheKey, ht, null, CacheUtils.SecondFactorCalculate(15));
                }
                return ht;
            }

            public static int GetValue(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo)
            {
                int firstContentID = 0;

                Hashtable hashtable = GetHashtable();
                if (hashtable[nodeInfo.NodeID] != null)
                {
                    firstContentID = (int)hashtable[nodeInfo.NodeID];
                }
                else
                {
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                    firstContentID = BaiRongDataProvider.ContentDAO.GetContentID(tableName, nodeInfo.NodeID, ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc));
                    hashtable[nodeInfo.NodeID] = firstContentID;
                }

                return firstContentID;
            }
        }

        public class NodeIDArrayList
        {
            private NodeIDArrayList()
            {
            }

            private const string cacheKey = "SiteServer.CreateCache.NodeIDArrayList";

            public static Hashtable GetHashtable()
            {
                Hashtable ht = CacheUtils.Get(cacheKey) as Hashtable;
                if (ht == null)
                {
                    ht = new Hashtable();
                    CacheUtils.Insert(cacheKey, ht, null, CacheUtils.SecondFactorCalculate(15));
                }
                return ht;
            }

            public static ArrayList GetNodeIDArrayListByScopeType(NodeInfo nodeInfo, EScopeType scopeType)
            {
                ArrayList arraylist = null;

                Hashtable hashtable = GetHashtable();
                string key = EScopeTypeUtils.GetValue(scopeType) + nodeInfo.NodeID;
                if (hashtable[key] != null)
                {
                    arraylist = hashtable[key] as ArrayList;
                }
                else
                {
                    arraylist = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, scopeType, string.Empty, string.Empty);
                    hashtable[key] = arraylist;
                }

                return arraylist;
            }
        }

        public class NodeID
        {
            private NodeID()
            {
            }

            private const string cacheKey = "SiteServer.CreateCache.NodeID";

            public static Hashtable GetHashtable()
            {
                Hashtable ht = CacheUtils.Get(cacheKey) as Hashtable;
                if (ht == null)
                {
                    ht = new Hashtable();
                    CacheUtils.Insert(cacheKey, ht, null, CacheUtils.SecondFactorCalculate(15));
                }
                return ht;
            }

            public static int GetNodeIDByChannelIDOrChannelIndexOrChannelName(int publishmentSystemID, int channelID, string channelIndex, string channelName)
            {
                int retval = channelID;

                Hashtable hashtable = GetHashtable();
                string key = string.Format("{0}_{1}_{2}_{3}", publishmentSystemID, channelID, channelIndex, channelName);
                if (hashtable[key] != null)
                {
                    retval = (int)hashtable[key];
                }
                else
                {
                    if (!string.IsNullOrEmpty(channelIndex))
                    {
                        int theNodeID = DataProvider.NodeDAO.GetNodeIDByNodeIndexName(publishmentSystemID, channelIndex);
                        if (theNodeID != 0)
                        {
                            retval = theNodeID;
                        }
                    }
                    if (!string.IsNullOrEmpty(channelName))
                    {
                        int theNodeID = DataProvider.NodeDAO.GetNodeIDByParentIDAndNodeName(publishmentSystemID, retval, channelName, true);
                        if (theNodeID == 0)
                        {
                            theNodeID = DataProvider.NodeDAO.GetNodeIDByParentIDAndNodeName(publishmentSystemID, publishmentSystemID, channelName, true);
                        }
                        if (theNodeID != 0)
                        {
                            retval = theNodeID;
                        }
                    }

                    hashtable[key] = retval;
                }

                return retval;
            }
        }

        public class FileContent
        {
            private FileContent()
            {
            }

            public static string GetTemplateContent(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo)
            {
                string filePath = TemplateManager.GetTemplateFilePath(publishmentSystemInfo, templateInfo);
                return GetContentByFilePath(filePath, templateInfo.Charset);
            }

            public static string GetIncludeContent(PublishmentSystemInfo publishmentSystemInfo, string file, ECharset charset)
            {
                string filePath = PathUtility.MapPath(publishmentSystemInfo, PathUtility.AddVirtualToPath(file));
                return GetContentByFilePath(filePath, charset);
            }

            public static string GetContentByFilePath(string filePath, ECharset charset)
            {
                try
                {
                    if (CacheUtils.Get(filePath) == null)
                    {
                        string content = string.Empty;
                        if (FileUtils.IsFileExists(filePath))
                            content = FileUtils.ReadText(filePath, charset);

                        CacheUtils.Insert(filePath, content, new CacheDependency(filePath), CacheUtils.HourFactor);
                        return content;
                    }
                    return CacheUtils.Get(filePath) as string;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
    }
}
