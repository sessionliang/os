using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Model;
using BaiRong.Core.Service;

namespace SiteServer.CMS.Core
{
	public sealed class NodeManager
	{
        private NodeManager()
		{
			
		}

        private const string CacheFileName = "NodeCache.txt";

        static NodeManager()
        {
            FileWatcherClass fileWatcher = new FileWatcherClass(PathUtility.GetCacheFilePath(CacheFileName));
            fileWatcher.OnFileChange += new FileWatcherClass.FileChange(fileWatcher_OnFileChange);
        }

        private static void fileWatcher_OnFileChange(object sender, EventArgs e)
        {
            CacheUtils.Remove(cacheKey);
        }

		public static Hashtable GetNodeInfoHashtableByPublishmentSystemID (int publishmentSystemID) 
		{
			return GetNodeInfoHashtableByPublishmentSystemID(publishmentSystemID, false);
		}

		public static Hashtable GetNodeInfoHashtableByPublishmentSystemID (int publishmentSystemID, bool flush) 
		{
			Hashtable ht = GetActiveHashtable();
            
			Hashtable nodeInfoHashtable = null;
            
			if(!flush)
			{
				nodeInfoHashtable = ht[publishmentSystemID] as Hashtable;
			}

			if(nodeInfoHashtable == null)
			{
				nodeInfoHashtable = DataProvider.NodeDAO.GetNodeInfoHashtableByPublishmentSystemID(publishmentSystemID);

				if(nodeInfoHashtable != null)
				{
					UpdateCache(ht, nodeInfoHashtable, publishmentSystemID);
				}
			}
			return nodeInfoHashtable;
		}

		public static NodeInfo GetNodeInfo(int publishmentSystemID, int nodeID)
		{
			NodeInfo nodeInfo = null;
			Hashtable hashtable = NodeManager.GetNodeInfoHashtableByPublishmentSystemID(publishmentSystemID);
			if (hashtable != null)
			{
				nodeInfo = hashtable[nodeID] as NodeInfo;
			}
			return nodeInfo;
		}

        public static bool IsExists(int publishmentSystemID, int nodeID)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            return nodeInfo != null;
        }

        public static bool IsExists(int nodeID)
        {
            ArrayList arraylist = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
            foreach (int publishmentSystemID in arraylist)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
                if (nodeInfo != null) return true;
            }
            
            return false;
        }

        public static int GetNodeIDByParentsCount(int publishmentSystemID, int nodeID, int parentsCount)
        {
            if (parentsCount == 0) return publishmentSystemID;
            if (nodeID == 0 || nodeID == publishmentSystemID) return publishmentSystemID;

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (nodeInfo != null)
            {
                if (nodeInfo.ParentsCount == parentsCount)
                {
                    return nodeInfo.NodeID;
                }
                else
                {
                    return GetNodeIDByParentsCount(publishmentSystemID, nodeInfo.ParentID, parentsCount);
                }
            }
            return publishmentSystemID;
        }

        public static string GetTableName(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {
            return GetTableName(publishmentSystemInfo, NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID));
        }

        public static string GetTableName(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo)
        {
            if (nodeInfo != null)
            {
                return GetTableName(publishmentSystemInfo, nodeInfo.ContentModelID);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetTableName(PublishmentSystemInfo publishmentSystemInfo, EAuxiliaryTableType tableType)
        {
            string tableName = string.Empty;
            if (EAuxiliaryTableTypeUtils.Equals(EAuxiliaryTableType.BackgroundContent, tableType))
            {
                tableName = publishmentSystemInfo.AuxiliaryTableForContent;
            }
            else if (EAuxiliaryTableTypeUtils.Equals(EAuxiliaryTableType.GoodsContent, tableType))
            {
                tableName = publishmentSystemInfo.AuxiliaryTableForGoods;
            }
            else if (EContentModelTypeUtils.Equals(EAuxiliaryTableType.BrandContent, tableType))
            {
                tableName = publishmentSystemInfo.AuxiliaryTableForBrand;
            }
            else if (EAuxiliaryTableTypeUtils.Equals(EAuxiliaryTableType.GovInteractContent, tableType))
            {
                tableName = publishmentSystemInfo.AuxiliaryTableForGovInteract;
            }
            else if (EContentModelTypeUtils.Equals(EAuxiliaryTableType.GovPublicContent, tableType))
            {
                tableName = publishmentSystemInfo.AuxiliaryTableForGovPublic;
            }
            else if (EContentModelTypeUtils.Equals(EAuxiliaryTableType.JobContent, tableType))
            {
                tableName = publishmentSystemInfo.AuxiliaryTableForJob;
            }
            else if (EContentModelTypeUtils.Equals(EAuxiliaryTableType.VoteContent, tableType))
            {
                tableName = publishmentSystemInfo.AuxiliaryTableForVote;
            }
            return tableName;
        }

        public static string GetTableName(PublishmentSystemInfo publishmentSystemInfo, string contentModelID)
        {
            ContentModelInfo modelInfo = ContentModelManager.GetContentModelInfo(publishmentSystemInfo, contentModelID);
            if (modelInfo != null && !string.IsNullOrEmpty(modelInfo.TableName))
            {
                return modelInfo.TableName;
            }
            else
            {
                string tableName = publishmentSystemInfo.AuxiliaryTableForContent;
                if (EContentModelTypeUtils.Equals(EContentModelType.Goods, contentModelID))
                {
                    tableName = publishmentSystemInfo.AuxiliaryTableForGoods;
                }
                else if (EContentModelTypeUtils.Equals(EContentModelType.Brand, contentModelID))
                {
                    tableName = publishmentSystemInfo.AuxiliaryTableForBrand;
                }
                else if (EContentModelTypeUtils.Equals(EContentModelType.GovPublic, contentModelID))
                {
                    tableName = publishmentSystemInfo.AuxiliaryTableForGovPublic;
                }
                else if (EContentModelTypeUtils.Equals(EContentModelType.GovInteract, contentModelID))
                {
                    tableName = publishmentSystemInfo.AuxiliaryTableForGovInteract;
                }
                else if (EContentModelTypeUtils.Equals(EContentModelType.Vote, contentModelID))
                {
                    tableName = publishmentSystemInfo.AuxiliaryTableForVote;
                }
                else if (EContentModelTypeUtils.Equals(EContentModelType.Job, contentModelID))
                {
                    tableName = publishmentSystemInfo.AuxiliaryTableForJob;
                }
                return tableName;
            }
        }

        public static ETableStyle GetTableStyle(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {
            return GetTableStyle(publishmentSystemInfo, NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID));
        }

        public static ETableStyle GetTableStyle(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo)
        {
            ContentModelInfo modelInfo = ContentModelManager.GetContentModelInfo(publishmentSystemInfo, nodeInfo.ContentModelID);
            if (modelInfo != null && !string.IsNullOrEmpty(modelInfo.TableName))
            {
                return EAuxiliaryTableTypeUtils.GetTableStyle(modelInfo.TableType);
            }
            else
            {
                ETableStyle tableStyle = ETableStyle.BackgroundContent;
                if (EContentModelTypeUtils.Equals(EContentModelType.Goods, nodeInfo.ContentModelID))
                {
                    tableStyle = ETableStyle.GoodsContent;
                }
                else if (EContentModelTypeUtils.Equals(EContentModelType.Brand, nodeInfo.ContentModelID))
                {
                    tableStyle = ETableStyle.BrandContent;
                }
                else if (EContentModelTypeUtils.Equals(EContentModelType.GovPublic, nodeInfo.ContentModelID))
                {
                    tableStyle = ETableStyle.GovPublicContent;
                }
                else if (EContentModelTypeUtils.Equals(EContentModelType.GovInteract, nodeInfo.ContentModelID))
                {
                    tableStyle = ETableStyle.GovInteractContent;
                }
                else if (EContentModelTypeUtils.Equals(EContentModelType.Vote, nodeInfo.ContentModelID))
                {
                    tableStyle = ETableStyle.VoteContent;
                }
                else if (EContentModelTypeUtils.Equals(EContentModelType.Job, nodeInfo.ContentModelID))
                {
                    tableStyle = ETableStyle.JobContent;
                }
                else if (EContentModelTypeUtils.Equals(EContentModelType.UserDefined, nodeInfo.ContentModelID))
                {
                    tableStyle = ETableStyle.UserDefined;
                }
                return tableStyle;
            }
        }

        public static string GetNodeTreeLastImageHtml(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo)
        {
            string treeDirectoryUrl = PageUtils.GetIconUrl("tree");

            string imageHtml = string.Empty;
            if (nodeInfo.NodeType == ENodeType.BackgroundPublishNode)
            {
                if (publishmentSystemInfo.IsHeadquarters == false)
                {
                    imageHtml = string.Format(@"<img align=""absmiddle"" alt=""应用"" border=""0"" src=""{0}"" /></a>", PageUtils.Combine(treeDirectoryUrl, "site.gif"));
                }
                else
                {
                    imageHtml = string.Format(@"<img align=""absmiddle"" alt=""应用"" border=""0"" src=""{0}"" />", PageUtils.Combine(treeDirectoryUrl, "siteHQ.gif"));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(nodeInfo.ContentModelID))
                {
                    ContentModelInfo modelInfo = ContentModelManager.GetContentModelInfo(publishmentSystemInfo, nodeInfo.ContentModelID);
                    if (!string.IsNullOrEmpty(modelInfo.IconUrl))
                    {
                        imageHtml += string.Format(@"&nbsp;<img align=""absmiddle"" alt=""{0}"" border=""0"" src=""{1}"" /></a>", modelInfo.ModelName, PageUtils.Combine(treeDirectoryUrl, modelInfo.IconUrl));
                    }
                }
            }
            return imageHtml;
        }

		public static DateTime GetAddDate(int publishmentSystemID, int nodeID)
		{
			DateTime retval = DateTime.MinValue;
			NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
			if (nodeInfo != null)
			{
				retval = nodeInfo.AddDate;
			}
			return retval;
		}

		public static int GetParentID(int publishmentSystemID, int nodeID)
		{
			int retval = 0;
			NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
			if (nodeInfo != null)
			{
				retval = nodeInfo.ParentID;
			}
			return retval;
		}

		public static string GetParentsPath(int publishmentSystemID, int nodeID)
		{
			string retval = string.Empty;
			NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
			if (nodeInfo != null)
			{
				retval = nodeInfo.ParentsPath;
			}
			return retval;
		}

        public static int GetTopLevel(int publishmentSystemID, int nodeID)
        {
            string parentsPath = NodeManager.GetParentsPath(publishmentSystemID, nodeID);
            if (string.IsNullOrEmpty(parentsPath))
            {
                return 0;
            }

            return parentsPath.Split(',').Length;
        }

		public static string GetNodeName(int publishmentSystemID, int nodeID)
		{
			string retval = string.Empty;
			NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
			if (nodeInfo != null)
			{
				retval = nodeInfo.NodeName;
			}
			return retval;
		}

        public static string GetNodeNameNavigation(int publishmentSystemID, int nodeID)
        {
            ArrayList nodeNameArrayList = new ArrayList();

            if (nodeID == 0) nodeID = publishmentSystemID;

            if (nodeID == publishmentSystemID)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, publishmentSystemID);
                return nodeInfo.NodeName;
            }
            else
            {
                string parentsPath = NodeManager.GetParentsPath(publishmentSystemID, nodeID);
                ArrayList nodeIDArrayList = new ArrayList();
                if (!string.IsNullOrEmpty(parentsPath))
                {
                    nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(parentsPath);
                }
                nodeIDArrayList.Add(nodeID);
                nodeIDArrayList.Remove(publishmentSystemID);

                foreach (int theNodeID in nodeIDArrayList)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, theNodeID);
                    if (nodeInfo != null)
                    {
                        nodeNameArrayList.Add(nodeInfo.NodeName);
                    }
                }

                return TranslateUtils.ObjectCollectionToString(nodeNameArrayList, " > ");
            }

            //NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            //if (nodeInfo != null)
            //{
            //    retval = nodeInfo.NodeName;
            //    if (nodeInfo.ParentID != publishmentSystemID)
            //    {
            //        nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeInfo.ParentID);
            //        if (nodeInfo != null)
            //        {
            //            retval += " &gt; " + nodeInfo.NodeName;
            //            if (nodeInfo.ParentID != publishmentSystemID)
            //            {
            //                nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeInfo.ParentID);
            //                if (nodeInfo != null)
            //                {
            //                    retval += " &gt; " + nodeInfo.NodeName;
            //                    if (nodeInfo.NodeID != 0)
            //                    {
            //                        nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeInfo.ParentID);
            //                        if (nodeInfo != null)
            //                        {
            //                            retval += " &gt; " + nodeInfo.NodeName;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            
            //return retval;
        }

        public static string GetNodeNameNavigationByGovPublic(int publishmentSystemID, int nodeID)
        {
            if (nodeID == 0 || publishmentSystemID == nodeID) return string.Empty;

            ArrayList nodeNameArrayList = new ArrayList();

            string parentsPath = NodeManager.GetParentsPath(publishmentSystemID, nodeID);
            ArrayList nodeIDArrayList = new ArrayList();
            if (!string.IsNullOrEmpty(parentsPath))
            {
                nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(parentsPath);
            }
            nodeIDArrayList.Add(nodeID);
            nodeIDArrayList.Remove(publishmentSystemID);

            foreach (int theNodeID in nodeIDArrayList)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, theNodeID);
                if (nodeInfo != null && nodeInfo.ParentsCount >= 1)
                {
                    nodeNameArrayList.Add(nodeInfo.NodeName);
                }
            }

            return TranslateUtils.ObjectCollectionToString(nodeNameArrayList, " > ");
        }

		public static ENodeType GetNodeType(int publishmentSystemID, int nodeID)
		{
			ENodeType retval = ENodeType.BackgroundNormalNode;
			NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
			if (nodeInfo != null)
			{
				retval = nodeInfo.NodeType;
			}
			return retval;
		}


		private static void UpdateCache(IDictionary ht, Hashtable nodeInfoHashtable, int publishmentSystemID)
		{
			lock(ht.SyncRoot)
			{
				ht[publishmentSystemID] = nodeInfoHashtable;
			}
		}

		public static void RemoveCache(int publishmentSystemID)
		{
			Hashtable ht = GetActiveHashtable();

			lock(ht.SyncRoot)
			{
				ht.Remove(publishmentSystemID);
			}

            CacheManager.UpdateTemporaryCacheFile(CacheFileName);
		}

        private const string cacheKey = "SiteServer.CMS.Core.NodeManager";        

		/// <summary>
		/// Returns a collection of SiteSettings which exist in the current Application Domain.
		/// </summary>
		/// <returns></returns>
		public static Hashtable GetActiveHashtable()
		{
			Hashtable ht = CacheUtils.Get(cacheKey) as Hashtable;
			if(ht == null)
			{
				ht = new Hashtable();
				CacheUtils.Insert(cacheKey, ht, null, CacheUtils.DayFactor);
			}
			return ht;
		}

        public static void AddListItems(ListItemCollection listItemCollection, PublishmentSystemInfo publishmentSystemInfo, bool isSeeOwning, bool isShowContentNum)
        {
            AddListItems(listItemCollection, publishmentSystemInfo, isSeeOwning, isShowContentNum, false);
        }

        public static void AddListItems(ListItemCollection listItemCollection, PublishmentSystemInfo publishmentSystemInfo, bool isSeeOwning, bool isShowContentNum, bool isShowContentModel)
        {
            ArrayList arraylist = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);
            int nodeCount = arraylist.Count;
            bool[] isLastNodeArray = new bool[nodeCount];
            foreach (int nodeID in arraylist)
            {
                bool enabled = true;
                if (isSeeOwning)
                {
                    enabled = AdminUtility.IsOwningNodeID(nodeID);
                    if (!enabled)
                    {
                        if (!AdminUtility.IsHasChildOwningNodeID(nodeID)) continue;
                    }
                }
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);

                ListItem listitem = new ListItem(NodeManager.GetSelectText(publishmentSystemInfo, nodeInfo, isLastNodeArray, isShowContentNum, isShowContentModel), nodeInfo.NodeID.ToString());
                if (!enabled)
                {
                    listitem.Attributes.Add("style", "color:gray;");
                }
                listItemCollection.Add(listitem);
            }
        }

        public static void AddListItems(ListItemCollection listItemCollection, PublishmentSystemInfo publishmentSystemInfo, bool isSeeOwning, bool isShowContentNum, bool isShowContentModel, EContentModelType contentModel)
        {
            ArrayList arraylist = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);
            int nodeCount = arraylist.Count;
            bool[] isLastNodeArray = new bool[nodeCount];
            foreach (int nodeID in arraylist)
            {
                bool enabled = true;
                if (isSeeOwning)
                {
                    enabled = AdminUtility.IsOwningNodeID(nodeID);
                    if (!enabled)
                    {
                        if (!AdminUtility.IsHasChildOwningNodeID(nodeID)) continue;
                    }
                }
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);

                ListItem listitem = new ListItem(NodeManager.GetSelectText(publishmentSystemInfo, nodeInfo, isLastNodeArray, isShowContentNum, isShowContentModel), nodeInfo.NodeID.ToString());
                if (!enabled)
                {
                    listitem.Attributes.Add("style", "color:gray;");
                }
                if (nodeInfo.ContentModelID != contentModel.ToString())
                {
                    listitem.Attributes.Add("disabled", "disabled");
                }
                listItemCollection.Add(listitem);
            }
        }

        public static bool AddListItemsForBrand(ListItemCollection listItemCollection, PublishmentSystemInfo publishmentSystemInfo, bool isCountContent, int parentNodeID)
        {
            bool isBrandExists = false;
            ArrayList arraylist = new ArrayList();
            if (parentNodeID > 0)
            {
                arraylist = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(parentNodeID, EScopeType.All, string.Empty, string.Empty);
            }
            else
            {
                arraylist = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);
            }

            foreach (int nodeID in arraylist)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);

                if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Brand))
                {
                    if (!isCountContent || nodeInfo.ContentNum > 0)
                    {
                        ListItem listitem = new ListItem(NodeManager.GetNodeNameNavigation(publishmentSystemInfo.PublishmentSystemID, nodeID), nodeID.ToString());

                        listItemCollection.Add(listitem);

                        isBrandExists = true;
                    }
                }


            }
            return isBrandExists;
        }

        public static void AddListItemsForAddContent(ListItemCollection listItemCollection, PublishmentSystemInfo publishmentSystemInfo, bool isSeeOwning)
        {
            ArrayList arraylist = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);
            int nodeCount = arraylist.Count;
            bool[] isLastNodeArray = new bool[nodeCount];
            foreach (int nodeID in arraylist)
            {
                bool enabled = true;
                if (isSeeOwning)
                {
                    enabled = AdminUtility.IsOwningNodeID(nodeID);
                }

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                if (enabled)
                {
                    if (nodeInfo.Additional.IsContentAddable == false) enabled = false;
                }

                if (!enabled)
                {
                    continue;
                }

                ListItem listitem = new ListItem(NodeManager.GetSelectText(publishmentSystemInfo, nodeInfo, isLastNodeArray, true, false), nodeInfo.NodeID.ToString());
                listItemCollection.Add(listitem);
            }
        }

        /// <summary>
        /// 得到栏目，并且不对（栏目是否可添加内容）进行判断
        /// 提供给触发器页面使用
        /// 使用场景：其他栏目的内容变动之后，设置某个栏目（此栏目不能添加内容）触发生成
        /// </summary>
        /// <param name="listItemCollection"></param>
        /// <param name="publishmentSystemInfo"></param>
        /// <param name="isSeeOwning"></param>
        public static void AddListItemsForCreateChannel(ListItemCollection listItemCollection, PublishmentSystemInfo publishmentSystemInfo, bool isSeeOwning)
        {
            ArrayList arraylist = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);
            int nodeCount = arraylist.Count;
            bool[] isLastNodeArray = new bool[nodeCount];
            foreach (int nodeID in arraylist)
            {
                bool enabled = true;
                if (isSeeOwning)
                {
                    enabled = AdminUtility.IsOwningNodeID(nodeID);
                }

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);

                if (!enabled)
                {
                    continue;
                }

                ListItem listitem = new ListItem(NodeManager.GetSelectText(publishmentSystemInfo, nodeInfo, isLastNodeArray, true, false), nodeInfo.NodeID.ToString());
                listItemCollection.Add(listitem);
            }
        }

        public static string GetSelectText(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, bool[] isLastNodeArray, bool isShowContentNum, bool isShowContentModel)
        {
            string retval = string.Empty;
            if (nodeInfo.NodeID == nodeInfo.PublishmentSystemID)
            {
                nodeInfo.IsLastNode = true;
            }
            if (nodeInfo.IsLastNode == false)
            {
                isLastNodeArray[nodeInfo.ParentsCount] = false;
            }
            else
            {
                isLastNodeArray[nodeInfo.ParentsCount] = true;
            }
            for (int i = 0; i < nodeInfo.ParentsCount; i++)
            {
                if (isLastNodeArray[i])
                {
                    retval = String.Concat(retval, "　");
                }
                else
                {
                    retval = String.Concat(retval, "│");
                }
            }
            if (nodeInfo.IsLastNode)
            {
                retval = String.Concat(retval, "└");
            }
            else
            {
                retval = String.Concat(retval, "├");
            }
            retval = String.Concat(retval, nodeInfo.NodeName);

            if (isShowContentNum)
            {
                retval = String.Concat(retval, " (", nodeInfo.ContentNum, ")");
            }

            if (isShowContentModel)
            {
                retval = String.Concat(retval, " - ", ContentModelManager.GetContentModelInfo(publishmentSystemInfo, nodeInfo.ContentModelID).ModelName);
            }
            return retval;
        }

        public static string GetContentAttributesOfDisplay(int publishmentSystemID, int nodeID)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (nodeInfo == null) return string.Empty;
            if (publishmentSystemID != nodeID && string.IsNullOrEmpty(nodeInfo.Additional.ContentAttributesOfDisplay))
            {
                return GetContentAttributesOfDisplay(publishmentSystemID, nodeInfo.ParentID);
            }
            return nodeInfo.Additional.ContentAttributesOfDisplay;
        }
	}

}
