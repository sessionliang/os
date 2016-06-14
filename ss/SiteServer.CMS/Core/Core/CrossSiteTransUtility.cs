using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public class CrossSiteTransUtility
    {
        private CrossSiteTransUtility()
        {
        }

        public static bool IsTranslatable(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo)
        {
            bool isTranslatable = false;

            if (nodeInfo != null && nodeInfo.Additional.TransType != ECrossSiteTransType.None)
            {
                ECrossSiteTransType transType = nodeInfo.Additional.TransType;
                if (transType != ECrossSiteTransType.None)
                {
                    if (transType == ECrossSiteTransType.AllParentSite)
                    {
                        int parentPublishmentSystemID = PublishmentSystemManager.GetParentPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);
                        if (parentPublishmentSystemID != 0)
                        {
                            isTranslatable = true;
                        }
                    }
                    else if (transType == ECrossSiteTransType.SelfSite)
                    {
                        isTranslatable = true;
                    }
                    else if (transType == ECrossSiteTransType.AllSite)
                    {
                        isTranslatable = true;
                    }
                    else if (transType == ECrossSiteTransType.SpecifiedSite)
                    {
                        if (nodeInfo.Additional.TransPublishmentSystemID > 0)
                        {
                            PublishmentSystemInfo thePublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(nodeInfo.Additional.TransPublishmentSystemID);
                            if (thePublishmentSystemInfo != null)
                            {
                                isTranslatable = true;
                            }
                        }
                    }
                    else if (transType == ECrossSiteTransType.ParentSite)
                    {
                        int parentPublishmentSystemID = PublishmentSystemManager.GetParentPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);
                        if (parentPublishmentSystemID != 0)
                        {
                            isTranslatable = true;
                        }
                    }
                }
            }

            return isTranslatable;
        }

        public static bool IsAutomatic(NodeInfo nodeInfo)
        {
            bool isAutomatic = false;

            if (nodeInfo != null)
            {
                if (nodeInfo.Additional.TransType == ECrossSiteTransType.SelfSite || nodeInfo.Additional.TransType == ECrossSiteTransType.ParentSite || nodeInfo.Additional.TransType == ECrossSiteTransType.SpecifiedSite)
                {
                    isAutomatic = nodeInfo.Additional.TransIsAutomatic;
                }
            }

            return isAutomatic;
        }

        public static void LoadPublishmentSystemIDDropDownList(DropDownList publishmentSystemIDDropDownList, PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {
            publishmentSystemIDDropDownList.Items.Clear();

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
            if (nodeInfo.Additional.TransType == ECrossSiteTransType.SelfSite || nodeInfo.Additional.TransType == ECrossSiteTransType.SpecifiedSite || nodeInfo.Additional.TransType == ECrossSiteTransType.ParentSite)
            {
                int thePublishmentSystemID;
                if (nodeInfo.Additional.TransType == ECrossSiteTransType.SelfSite)
                {
                    thePublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                }
                else if (nodeInfo.Additional.TransType == ECrossSiteTransType.SpecifiedSite)
                {
                    thePublishmentSystemID = nodeInfo.Additional.TransPublishmentSystemID;
                }
                else
                {
                    thePublishmentSystemID = PublishmentSystemManager.GetParentPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);
                }
                if (thePublishmentSystemID > 0)
                {
                    PublishmentSystemInfo thePublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(thePublishmentSystemID);
                    if (thePublishmentSystemInfo != null)
                    {
                        ListItem listitem = new ListItem(thePublishmentSystemInfo.PublishmentSystemName, thePublishmentSystemInfo.PublishmentSystemID.ToString());
                        publishmentSystemIDDropDownList.Items.Add(listitem);
                    }
                }
            }
            else if (nodeInfo.Additional.TransType == ECrossSiteTransType.AllParentSite || nodeInfo.Additional.TransType == ECrossSiteTransType.AllSite)
            {
                ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();

                ArrayList allParentPublishmentSystemIDArrayList = new ArrayList();
                if (nodeInfo.Additional.TransType == ECrossSiteTransType.AllParentSite)
                {
                    PublishmentSystemManager.GetAllParentPublishmentSystemIDArrayList(allParentPublishmentSystemIDArrayList, publishmentSystemIDArrayList, publishmentSystemInfo.PublishmentSystemID);
                }

                foreach (int psID in publishmentSystemIDArrayList)
                {
                    if (psID == publishmentSystemInfo.PublishmentSystemID) continue;
                    PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(psID);
                    bool show = false;
                    if (nodeInfo.Additional.TransType == ECrossSiteTransType.AllSite)
                    {
                        show = true;
                    }
                    else if (nodeInfo.Additional.TransType == ECrossSiteTransType.AllParentSite)
                    {
                        if (psInfo.IsHeadquarters || allParentPublishmentSystemIDArrayList.Contains(psInfo.PublishmentSystemID))
                        {
                            show = true;
                        }
                    }
                    if (show)
                    {
                        ListItem listitem = new ListItem(psInfo.PublishmentSystemName, psID.ToString());
                        if (psInfo.IsHeadquarters) listitem.Selected = true;
                        publishmentSystemIDDropDownList.Items.Add(listitem);
                    }
                }
            }
        }

        public static void LoadNodeIDListBox(ListBox nodeIDListBox, PublishmentSystemInfo publishmentSystemInfo, int psID, NodeInfo nodeInfo)
        {
            nodeIDListBox.Items.Clear();

            bool isUseNodeNames = false;
            if (nodeInfo.Additional.TransType == ECrossSiteTransType.AllParentSite || nodeInfo.Additional.TransType == ECrossSiteTransType.AllSite)
            {
                isUseNodeNames = true;
            }

            if (!isUseNodeNames)
            {
                ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(nodeInfo.Additional.TransNodeIDs);
                foreach (int theNodeID in nodeIDArrayList)
                {
                    NodeInfo theNodeInfo = NodeManager.GetNodeInfo(psID, theNodeID);
                    if (theNodeInfo != null)
                    {
                        ListItem listitem = new ListItem(theNodeInfo.NodeName, theNodeInfo.NodeID.ToString());
                        nodeIDListBox.Items.Add(listitem);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(nodeInfo.Additional.TransNodeNames))
                {
                    ArrayList nodeNameArrayList = TranslateUtils.StringCollectionToArrayList(nodeInfo.Additional.TransNodeNames);
                    Hashtable hashtable = NodeManager.GetNodeInfoHashtableByPublishmentSystemID(psID);
                    if (hashtable != null)
                    {
                        foreach (string nodeName in nodeNameArrayList)
                        {
                            foreach (int theNodeID in hashtable.Keys)
                            {
                                NodeInfo theNodeInfo = NodeManager.GetNodeInfo(psID, theNodeID);
                                if (theNodeInfo.NodeName == nodeName)
                                {
                                    ListItem listitem = new ListItem(theNodeInfo.NodeName, theNodeInfo.NodeID.ToString());
                                    nodeIDListBox.Items.Add(listitem);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    NodeManager.AddListItemsForAddContent(nodeIDListBox.Items, PublishmentSystemManager.GetPublishmentSystemInfo(psID), false);
                }
            }
        }

        public static string GetDescription(int publishmentSystemID, NodeInfo nodeInfo)
        {
            string results = string.Empty;

            if (nodeInfo != null)
            {
                results = ECrossSiteTransTypeUtils.GetText(nodeInfo.Additional.TransType);

                if (nodeInfo.Additional.TransType == ECrossSiteTransType.AllParentSite || nodeInfo.Additional.TransType == ECrossSiteTransType.AllSite)
                {
                    if (!string.IsNullOrEmpty(nodeInfo.Additional.TransNodeNames))
                    {
                        results += string.Format("({0})", nodeInfo.Additional.TransNodeNames);
                    }
                }
                else if (nodeInfo.Additional.TransType == ECrossSiteTransType.SelfSite || nodeInfo.Additional.TransType == ECrossSiteTransType.SpecifiedSite || nodeInfo.Additional.TransType == ECrossSiteTransType.ParentSite)
                {
                    PublishmentSystemInfo publishmentSystemInfo = null;

                    if (nodeInfo.Additional.TransType == ECrossSiteTransType.SelfSite)
                    {
                        publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    }
                    else if (nodeInfo.Additional.TransType == ECrossSiteTransType.SpecifiedSite)
                    {
                        publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(nodeInfo.Additional.TransPublishmentSystemID);
                    }
                    else
                    {
                        int parentPublishmentSystemID = PublishmentSystemManager.GetParentPublishmentSystemID(publishmentSystemID);
                        if (parentPublishmentSystemID != 0)
                        {
                            publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(parentPublishmentSystemID);
                        }
                    }

                    if (publishmentSystemInfo != null && !string.IsNullOrEmpty(nodeInfo.Additional.TransNodeIDs))
                    {
                        StringBuilder nodeNameBuilder = new StringBuilder();
                        ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(nodeInfo.Additional.TransNodeIDs);
                        foreach (int nodeID in nodeIDArrayList)
                        {
                            NodeInfo theNodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                            if (theNodeInfo != null)
                            {
                                nodeNameBuilder.Append(theNodeInfo.NodeName).Append(",");
                            }
                        }
                        if (nodeNameBuilder.Length > 0)
                        {
                            nodeNameBuilder.Length--;
                            results += string.Format("({0}:{1})", publishmentSystemInfo.PublishmentSystemName, nodeNameBuilder);
                        }
                    }
                }
            }
            return results;
        }

        public static void TransContentInfo(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, int contentID, PublishmentSystemInfo targetPublishmentSystemInfo, int targetNodeID)
        {
            string targetTableName = NodeManager.GetTableName(targetPublishmentSystemInfo, targetNodeID);

            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
            FileUtility.MoveFileByContentInfo(publishmentSystemInfo, targetPublishmentSystemInfo, contentInfo);
            contentInfo.PublishmentSystemID = targetPublishmentSystemInfo.PublishmentSystemID;
            contentInfo.SourceID = nodeInfo.NodeID;
            contentInfo.NodeID = targetNodeID;
            if (targetPublishmentSystemInfo.Additional.IsCrossSiteTransChecked)
            {
                contentInfo.IsChecked = true;
            }
            else
            {
                contentInfo.IsChecked = false;
            }
            contentInfo.CheckedLevel = 0;

            //复制
            if (ETranslateContentTypeUtils.Equals(nodeInfo.Additional.TransDoneType, ETranslateContentType.Copy))
            {
                contentInfo.Attributes.Add(ContentAttribute.TranslateContentType, ETranslateContentType.Copy.ToString());
            }
            //引用地址
            else if (ETranslateContentTypeUtils.Equals(nodeInfo.Additional.TransDoneType, ETranslateContentType.Reference))
            {
                contentInfo.PublishmentSystemID = targetPublishmentSystemInfo.PublishmentSystemID;
                contentInfo.SourceID = nodeInfo.NodeID;
                contentInfo.NodeID = targetNodeID;
                contentInfo.ReferenceID = contentID;
                contentInfo.Attributes.Add(ContentAttribute.TranslateContentType, ETranslateContentType.Reference.ToString());
            }
            //引用内容
            else if (ETranslateContentTypeUtils.Equals(nodeInfo.Additional.TransDoneType, ETranslateContentType.ReferenceContent))
            {
                contentInfo.PublishmentSystemID = targetPublishmentSystemInfo.PublishmentSystemID;
                contentInfo.SourceID = nodeInfo.NodeID;
                contentInfo.NodeID = targetNodeID;
                contentInfo.ReferenceID = contentID;
                contentInfo.Attributes.Add(ContentAttribute.TranslateContentType, ETranslateContentType.ReferenceContent.ToString());
            }


            if (!string.IsNullOrEmpty(targetTableName))
            {
                int theContentID = DataProvider.ContentDAO.Insert(targetTableName, targetPublishmentSystemInfo, contentInfo);

                #region 复制资源
                PublishmentSystemInfo targetPulishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemInfo.PublishmentSystemID);
                ETableStyle targetTableStyle = NodeManager.GetTableStyle(targetPulishmentSystemInfo, targetNodeID);
                ContentInfo targetContentInfo = DataProvider.ContentDAO.GetContentInfo(targetTableStyle, targetTableName, theContentID);
                //资源：图片，文件，视频
                if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl)))
                {
                    //修改图片
                    string sourceImageUrl = PathUtility.MapPath(publishmentSystemInfo, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl));
                    CopyReferenceFiles(targetPublishmentSystemInfo, sourceImageUrl, publishmentSystemInfo);

                }
                else if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.ImageUrl))))
                {
                    ArrayList sourceImageUrls = TranslateUtils.StringCollectionToArrayList(contentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.ImageUrl)));

                    foreach (string imageUrl in sourceImageUrls)
                    {
                        string sourceImageUrl = PathUtility.MapPath(publishmentSystemInfo, imageUrl);
                        CopyReferenceFiles(targetPublishmentSystemInfo, sourceImageUrl, publishmentSystemInfo);
                    }
                }
                if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl)))
                {
                    //修改附件
                    string sourceFileUrl = PathUtility.MapPath(publishmentSystemInfo, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl));
                    CopyReferenceFiles(targetPublishmentSystemInfo, sourceFileUrl, publishmentSystemInfo);

                }
                else if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.FileUrl))))
                {
                    ArrayList sourceFileUrls = TranslateUtils.StringCollectionToArrayList(contentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.FileUrl)));

                    foreach (string FileUrl in sourceFileUrls)
                    {
                        string sourceFileUrl = PathUtility.MapPath(publishmentSystemInfo, FileUrl);
                        CopyReferenceFiles(targetPublishmentSystemInfo, sourceFileUrl, publishmentSystemInfo);
                    }
                }
                if (targetTableStyle == ETableStyle.GoodsContent)
                {
                    if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(GoodsContentAttribute.ThumbUrl)))
                    {
                        //修改附件
                        string sourceThumbUrl = PathUtility.MapPath(publishmentSystemInfo, contentInfo.GetExtendedAttribute(GoodsContentAttribute.ThumbUrl));
                        CopyReferenceFiles(targetPublishmentSystemInfo, sourceThumbUrl, publishmentSystemInfo);

                    }
                }
                #endregion
            }
        }

        private static void CopyReferenceFiles(PublishmentSystemInfo targetPublishmentSystemInfo, string sourceUrl, PublishmentSystemInfo sourcePublishmentSystemInfo)
        {
            string targetUrl = StringUtils.ReplaceFirst(sourcePublishmentSystemInfo.PublishmentSystemDir, sourceUrl, targetPublishmentSystemInfo.PublishmentSystemDir);
            if (!FileUtils.IsFileExists(targetUrl))
            {
                FileUtils.CopyFile(sourceUrl, targetUrl, true);
            }
        }
    }
}