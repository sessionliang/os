using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Model;
using System.Collections;
using System;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.ListTemplate
{
	public class TemplateUtility
	{
		private TemplateUtility()
		{
		}

        public static string GetContentsItemTemplateString(string templateString, LowerNameValueCollection selectedItems, LowerNameValueCollection selectedValues, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);
            BackgroundContentInfo contentInfo = new BackgroundContentInfo(itemContainer.ContentItem.DataItem);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContextType = contextType;
            contextInfo.ItemContainer = itemContainer;
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ChannelID = contentInfo.NodeID;
            contextInfo.ContentID = contentInfo.ID;
            contextInfo.ContentInfo = contentInfo;

            string theTemplateString = string.Empty;

            if (selectedItems != null && selectedItems.Count > 0)
            {
                foreach (string itemTypes in selectedItems.Keys)
                {
                    ArrayList itemTypeArrayList = TranslateUtils.StringCollectionToArrayList(itemTypes);
                    bool isTrue = true;
                    foreach (string itemType in itemTypeArrayList)
                    {
                        if (!IsContentTemplateString(itemType, itemTypes, ref theTemplateString, selectedItems, selectedValues, pageInfo, contextInfo))
                        {
                            isTrue = false;
                        }
                    }
                    if (isTrue)
                    {
                        break;
                    }
                    else
                    {
                        theTemplateString = string.Empty;
                    }
                }
            }

            if (string.IsNullOrEmpty(theTemplateString))
            {
                theTemplateString = templateString;
            }

            StringBuilder innerBuilder = new StringBuilder(theTemplateString);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopContentItem(pageInfo);

            return innerBuilder.ToString();
        }

        private static bool IsContentTemplateString(string itemType, string itemTypes, ref string templateString, LowerNameValueCollection selectedItems, LowerNameValueCollection selectedValues, PageInfo pageInfo, ContextInfo contextInfo)
        {
            if (itemType == StlItemTemplate.ContentsItem.Selected_Current)//当前内容
            {
                if (contextInfo.ContentInfo.ID == pageInfo.PageContentID)
                {
                    templateString = selectedItems[itemTypes];
                    return true;
                }
            }
            else if (itemType == StlItemTemplate.ContentsItem.Selected_IsTop)//置顶内容
            {
                if (contextInfo.ContentInfo.IsTop)
                {
                    templateString = selectedItems[itemTypes];
                    return true;
                }
            }
            else if (itemType == StlItemTemplate.ContentsItem.Selected_Image)//带图片的内容
            {
                if (!string.IsNullOrEmpty(contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl)))
                {
                    templateString = selectedItems[itemTypes];
                    return true;
                }
            }
            else if (itemType == StlItemTemplate.ContentsItem.Selected_Video)//带视频的内容
            {
                if (!string.IsNullOrEmpty(contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl)))
                {
                    templateString = selectedItems[itemTypes];
                    return true;
                }
            }
            else if (itemType == StlItemTemplate.ContentsItem.Selected_File)//带附件的内容
            {
                if (!string.IsNullOrEmpty(contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl)))
                {
                    templateString = selectedItems[itemTypes];
                    return true;
                }
            }
            else if (itemType == StlItemTemplate.ContentsItem.Selected_IsRecommend)//推荐的内容
            {
                if (TranslateUtils.ToBool(contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.IsRecommend)))
                {
                    templateString = selectedItems[itemTypes];
                    return true;
                }
            }
            else if (itemType == StlItemTemplate.ContentsItem.Selected_IsHot)//热点内容
            {
                if (TranslateUtils.ToBool(contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.IsHot)))
                {
                    templateString = selectedItems[itemTypes];
                    return true;
                }
            }
            else if (itemType == StlItemTemplate.ContentsItem.Selected_IsColor)//醒目内容
            {
                if (TranslateUtils.ToBool(contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.IsColor)))
                {
                    templateString = selectedItems[itemTypes];
                    return true;
                }
            }
            else if (itemType == StlItemTemplate.ContentsItem.Selected_ChannelName)//带有附件的内容
            {
                if (selectedValues.Count > 0)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(contextInfo.ContentInfo.PublishmentSystemID, contextInfo.ContentInfo.NodeID);
                    if (nodeInfo != null)
                    {
                        if (selectedValues[nodeInfo.NodeName] != null)
                        {
                            templateString = selectedValues[nodeInfo.NodeName];
                            return true;
                        }
                    }
                }
            }
            else if (TemplateUtility.IsNumberInRange(contextInfo.ItemContainer.ContentItem.ItemIndex + 1, itemType))
            {
                templateString = selectedItems[itemTypes];
                return true;
            }
            return false;
        }

        public static string GetChannelsItemTemplateString(string templateString, LowerNameValueCollection selectedItems, LowerNameValueCollection selectedValues, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            int nodeID = TranslateUtils.EvalInt(itemContainer.ChannelItem.DataItem, NodeAttribute.NodeID);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContextType = contextType;
            contextInfo.ItemContainer = itemContainer;
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ChannelID = nodeID;

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, nodeID);
            if (selectedItems != null && selectedItems.Count > 0)
            {
                foreach (string itemType in selectedItems.Keys)
                {
                    if (itemType == StlItemTemplate.ChannelsItem.Selected_Current)//当前栏目
                    {
                        if (nodeID == pageInfo.PageNodeID)
                        {
                            templateString = selectedItems[itemType];
                            break;
                        }
                    }
                    else if (itemType == StlItemTemplate.ChannelsItem.Selected_Image)//带有图片的栏目
                    {
                        if (!string.IsNullOrEmpty(nodeInfo.ImageUrl))
                        {
                            templateString = selectedItems[itemType];
                            break;
                        }
                    }
                    else if (itemType.StartsWith(StlItemTemplate.ChannelsItem.Selected_Up))//当前栏目的上级栏目
                    {
                        int upLevel = 1;
                        if (itemType == StlItemTemplate.ChannelsItem.Selected_Up)
                        {
                            upLevel = 1;
                        }
                        else
                        {
                            upLevel = TranslateUtils.ToInt(itemType.Substring(2));
                        }
                        if (upLevel > 0)
                        {
                            int theNodeID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, pageInfo.PageNodeID, upLevel, -1);
                            if (nodeID == theNodeID)
                            {
                                templateString = selectedItems[itemType];
                                break;
                            }
                        }
                    }
                    else if (itemType.StartsWith(StlItemTemplate.ChannelsItem.Selected_Top))//当前栏目从首页向下的上级栏目栏目
                    {
                        int topLevel = 1;
                        if (itemType == StlItemTemplate.ChannelsItem.Selected_Top)
                        {
                            topLevel = 1;
                        }
                        else
                        {
                            topLevel = TranslateUtils.ToInt(itemType.Substring(3));
                        }
                        if (topLevel >= 0)
                        {
                            int theNodeID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, pageInfo.PageNodeID, 0, topLevel);
                            if (nodeID == theNodeID)
                            {
                                templateString = selectedItems[itemType];
                                break;
                            }
                        }
                    }
                    else if (TemplateUtility.IsNumberInRange(itemContainer.ChannelItem.ItemIndex + 1, itemType))
                    {
                        templateString = selectedItems[itemType];
                        break;
                    }
                }
            }

            StringBuilder innerBuilder = new StringBuilder(templateString);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopChannelItem(pageInfo);

            return innerBuilder.ToString();
        }

        public static string GetCommentsTemplateString(string templateString, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContextType = EContextType.Comment;
            contextInfo.ItemContainer = itemContainer;
            contextInfo.ContainerClientID = containerClientID;

            StringBuilder innerBuilder = new StringBuilder(templateString);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopCommentItem(pageInfo);

            return innerBuilder.ToString();
        }

        public static string GetInputContentsTemplateString(string templateString, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ItemContainer = itemContainer;
            contextInfo.ContextType = contextType;

            StringBuilder innerBuilder = new StringBuilder(templateString);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopInputItem(pageInfo);

            return innerBuilder.ToString();
        }


        public static string GetWebsiteMessageContentsTemplateString(string templateString, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ItemContainer = itemContainer;
            contextInfo.ContextType = contextType;

            StringBuilder innerBuilder = new StringBuilder(templateString);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopWebsiteMessageItem(pageInfo);

            return innerBuilder.ToString();
        }

        public static string GetSqlContentsTemplateString(string templateString, LowerNameValueCollection selectedItems, LowerNameValueCollection selectedValues, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContextType = contextType;
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ItemContainer = itemContainer;

            if (selectedItems != null && selectedItems.Count > 0)
            {
                foreach (string itemType in selectedItems.Keys)
                {
                    if (TemplateUtility.IsNumberInRange(itemContainer.SqlItem.ItemIndex + 1, itemType))
                    {
                        templateString = selectedItems[itemType];
                        break;
                    }
                }
            }

            StringBuilder innerBuilder = new StringBuilder(templateString);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopSqlItem(pageInfo);

            return innerBuilder.ToString();
        }

        public static string GetSitesTemplateString(string templateString, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            int publishmentSystemID = TranslateUtils.EvalInt(itemContainer.SiteItem.DataItem, PublishmentSystemAttribute.PublishmentSystemID);
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ItemContainer = itemContainer;
            contextInfo.ContextType = contextType;

            PublishmentSystemInfo prePublishmentSystemInfo = pageInfo.PublishmentSystemInfo;
            int prePageNodeID = pageInfo.PageNodeID;
            int prePageContentID = pageInfo.PageContentID;
            EVisualType visualType = pageInfo.VisualType;
            pageInfo.ChangeSite(publishmentSystemInfo, publishmentSystemInfo.PublishmentSystemID, 0, contextInfo, publishmentSystemInfo.Additional.VisualType);

            StringBuilder innerBuilder = new StringBuilder(templateString);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopInputItem(pageInfo);

            pageInfo.ChangeSite(prePublishmentSystemInfo, prePageNodeID, prePageContentID, contextInfo, visualType);

            return innerBuilder.ToString();
        }

        public static string GetPhotosTemplateString(string templateString, LowerNameValueCollection selectedItems, LowerNameValueCollection selectedValues, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContextType = contextType;
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ItemContainer = itemContainer;

            if (selectedItems != null && selectedItems.Count > 0)
            {
                foreach (string itemType in selectedItems.Keys)
                {
                    if (TemplateUtility.IsNumberInRange(itemContainer.SqlItem.ItemIndex + 1, itemType))
                    {
                        templateString = selectedItems[itemType];
                        break;
                    }
                }
            }

            StringBuilder innerBuilder = new StringBuilder(templateString);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopPhotoItem(pageInfo);

            return innerBuilder.ToString();
        }

        public static string GetTeleplaysTemplateString(string templateString, LowerNameValueCollection selectedItems, LowerNameValueCollection selectedValues, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContextType = contextType;
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ItemContainer = itemContainer;

            if (selectedItems != null && selectedItems.Count > 0)
            {
                foreach (string itemType in selectedItems.Keys)
                {
                    if (TemplateUtility.IsNumberInRange(itemContainer.SqlItem.ItemIndex + 1, itemType))
                    {
                        templateString = selectedItems[itemType];
                        break;
                    }
                }
            }

            StringBuilder innerBuilder = new StringBuilder(templateString);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopTeleplayItem(pageInfo);

            return innerBuilder.ToString();
        }

        public static string GetEachsTemplateString(string templateString, LowerNameValueCollection selectedItems, LowerNameValueCollection selectedValues, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContextType = contextType;
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ItemContainer = itemContainer;

            if (selectedItems != null && selectedItems.Count > 0)
            {
                foreach (string itemType in selectedItems.Keys)
                {
                    if (TemplateUtility.IsNumberInRange(itemContainer.SqlItem.ItemIndex + 1, itemType))
                    {
                        templateString = selectedItems[itemType];
                        break;
                    }
                }
            }

            StringBuilder innerBuilder = new StringBuilder(templateString);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopEachItem(pageInfo);

            return innerBuilder.ToString();
        }

        public static string GetSpecsTemplateString(string templateString, LowerNameValueCollection selectedItems, LowerNameValueCollection selectedValues, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContextType = contextType;
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ItemContainer = itemContainer;

            if (selectedItems != null && selectedItems.Count > 0)
            {
                foreach (string itemType in selectedItems.Keys)
                {
                    if (TemplateUtility.IsNumberInRange(itemContainer.SqlItem.ItemIndex + 1, itemType))
                    {
                        templateString = selectedItems[itemType];
                        break;
                    }
                }
            }

            StringBuilder innerBuilder = new StringBuilder(templateString);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopSpecItem(pageInfo);

            return innerBuilder.ToString();
        }

        public static string GetFiltersTemplateString(string templateString, LowerNameValueCollection selectedItems, LowerNameValueCollection selectedValues, string containerClientID, PageInfo pageInfo, EContextType contextType, ContextInfo contextInfoRef)
        {
            DbItemContainer itemContainer = DbItemContainer.GetItemContainer(pageInfo);

            ContextInfo contextInfo = contextInfoRef.Clone();
            contextInfo.ContextType = contextType;
            contextInfo.ContainerClientID = containerClientID;
            contextInfo.ItemContainer = itemContainer;

            if (selectedItems != null && selectedItems.Count > 0)
            {
                foreach (string itemType in selectedItems.Keys)
                {
                    if (TemplateUtility.IsNumberInRange(itemContainer.SqlItem.ItemIndex + 1, itemType))
                    {
                        templateString = selectedItems[itemType];
                        break;
                    }
                }
            }

            StringBuilder innerBuilder = new StringBuilder(templateString);
            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

            DbItemContainer.PopFilterItem(pageInfo);

            return innerBuilder.ToString();
        }

		public static void PutContentsDisplayInfoToMyDataList(ParsedDataList MyDataList, ContentsDisplayInfo displayInfo)
		{
			MyDataList.RepeatColumns = displayInfo.Columns;
			MyDataList.RepeatDirection = displayInfo.Direction;
			MyDataList.Height = displayInfo.Height;
			MyDataList.Width = displayInfo.Width;
            if (!string.IsNullOrEmpty(displayInfo.Align))
            {
                MyDataList.HorizontalAlign = Converter.ToHorizontalAlign(displayInfo.Align);
            }
			MyDataList.ItemStyle.Height = displayInfo.ItemHeight;
			MyDataList.ItemStyle.Width = displayInfo.ItemWidth;
            MyDataList.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            if (!string.IsNullOrEmpty(displayInfo.ItemAlign))
            {
                MyDataList.ItemStyle.HorizontalAlign = Converter.ToHorizontalAlign(displayInfo.ItemAlign);
            }
            MyDataList.ItemStyle.VerticalAlign = VerticalAlign.Top;
            if (!string.IsNullOrEmpty(displayInfo.ItemVerticalAlign))
            {
                MyDataList.ItemStyle.VerticalAlign = Converter.ToVerticalAlign(displayInfo.ItemVerticalAlign);
            }
            if (!string.IsNullOrEmpty(displayInfo.ItemClass))
            {
                MyDataList.ItemStyle.CssClass = displayInfo.ItemClass;
            }

            if (displayInfo.Layout == ELayout.Table)
			{
				MyDataList.RepeatLayout = RepeatLayout.Table;
			}
            else if (displayInfo.Layout == ELayout.Flow)
            {
                MyDataList.RepeatLayout = RepeatLayout.Flow;
            }

            foreach (string attributeName in displayInfo.OtherAttributes)
            {
                MyDataList.AddAttribute(attributeName, displayInfo.OtherAttributes[attributeName]);
            }
        }

        /// <summary>
        ///  by 20151125 sofuny
        /// 培生智能推送
        /// 增加智能推送内容列表标签
        /// </summary>
        public static void PutContentsDisplayInfoToMyDataList(ParsedDataList MyDataList, IPushContentsDisplayInfo displayInfo)
        {
            MyDataList.RepeatColumns = displayInfo.Columns;
            MyDataList.RepeatDirection = displayInfo.Direction;
            MyDataList.Height = displayInfo.Height;
            MyDataList.Width = displayInfo.Width;
            if (!string.IsNullOrEmpty(displayInfo.Align))
            {
                MyDataList.HorizontalAlign = Converter.ToHorizontalAlign(displayInfo.Align);
            }
            MyDataList.ItemStyle.Height = displayInfo.ItemHeight;
            MyDataList.ItemStyle.Width = displayInfo.ItemWidth;
            MyDataList.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            if (!string.IsNullOrEmpty(displayInfo.ItemAlign))
            {
                MyDataList.ItemStyle.HorizontalAlign = Converter.ToHorizontalAlign(displayInfo.ItemAlign);
            }
            MyDataList.ItemStyle.VerticalAlign = VerticalAlign.Top;
            if (!string.IsNullOrEmpty(displayInfo.ItemVerticalAlign))
            {
                MyDataList.ItemStyle.VerticalAlign = Converter.ToVerticalAlign(displayInfo.ItemVerticalAlign);
            }
            if (!string.IsNullOrEmpty(displayInfo.ItemClass))
            {
                MyDataList.ItemStyle.CssClass = displayInfo.ItemClass;
            }

            if (displayInfo.Layout == ELayout.Table)
            {
                MyDataList.RepeatLayout = RepeatLayout.Table;
            }
            else if (displayInfo.Layout == ELayout.Flow)
            {
                MyDataList.RepeatLayout = RepeatLayout.Flow;
            }

            foreach (string attributeName in displayInfo.OtherAttributes)
            {
                MyDataList.AddAttribute(attributeName, displayInfo.OtherAttributes[attributeName]);
            }
        }
        private static bool IsNumberInRange(int number, string range)
        {
            if (!string.IsNullOrEmpty(range))
            {
                if (range.IndexOf(',') != -1)
                {
                    string[] intArr = range.Split(',');
                    foreach (string intStr in intArr)
                    {
                        if (TranslateUtils.ToInt(intStr.Trim()) == number)
                        {
                            return true;
                        }
                    }
                }
                else if (range.IndexOf('_') != -1)
                {
                    int startVal = TranslateUtils.ToInt(range.Split('_')[0].Trim());
                    int endVal = TranslateUtils.ToInt(range.Split('_')[1].Trim());
                    if (number >= startVal && number <= endVal)
                    {
                        return true;
                    }
                }
                else if (range.IndexOf('-') != -1)
                {
                    int startVal = TranslateUtils.ToInt(range.Split('_')[0].Trim());
                    int endVal = TranslateUtils.ToInt(range.Split('_')[1].Trim());
                    if (number >= startVal && number <= endVal)
                    {
                        return true;
                    }
                }
                else if (TranslateUtils.ToInt(range.Trim()) == number)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
