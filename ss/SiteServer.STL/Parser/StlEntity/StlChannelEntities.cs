using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlEntity
{
	public class StlChannelEntities
	{
        private StlChannelEntities()
		{
		}

        public const string EntityName = "Channel";                  //栏目实体

        public static string ChannelID = "ChannelID";//栏目ID
        public static string ChannelName = "ChannelName";//栏目名称
        public static string ChannelIndex = "ChannelIndex";//栏目索引
		public static string Title = "Title";//栏目名称
        public static string Content = "Content";//栏目正文
        public static string NavigationUrl = "NavigationUrl";//栏目链接地址
        public static string ImageUrl = "ImageUrl";//栏目图片地址
        public static string AddDate = "AddDate";//栏目添加日期
        public static string DirectoryName = "DirectoryName";//生成文件夹名称
        public static string Group = "Group";//栏目组别
        public static string ItemIndex = "ItemIndex";//栏目排序

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(ChannelID, "栏目ID");
                attributes.Add(Title, "栏目名称");
                attributes.Add(ChannelName, "栏目名称");
                attributes.Add(ChannelIndex, "栏目索引");
                attributes.Add(Content, "栏目正文");
                attributes.Add(NavigationUrl, "栏目链接地址");
                attributes.Add(ImageUrl, "栏目图片地址");
                attributes.Add(AddDate, "栏目添加日期");
                attributes.Add(DirectoryName, "生成文件夹名称");
                attributes.Add(Group, "栏目组别");
                attributes.Add(ItemIndex, "栏目排序");
                
                return attributes;
            }
        }

        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            try
            {
                string entityName = StlParserUtility.GetNameFromEntity(stlEntity);
                string channelIndex = StlParserUtility.GetValueFromEntity(stlEntity);
                string attributeName = entityName.Substring(9, entityName.Length - 10);

                int upLevel = 0;
                int topLevel = -1;
                int channelID = contextInfo.ChannelID;
                if (!string.IsNullOrEmpty(channelIndex))
                {
                    channelID = DataProvider.NodeDAO.GetNodeIDByNodeIndexName(pageInfo.PublishmentSystemID, channelIndex);
                    if (channelID == 0)
                    {
                        channelID = contextInfo.ChannelID;
                    }
                }
                
                if (attributeName.ToLower().StartsWith("up") && attributeName.IndexOf(".") != -1)
                {
                    if (attributeName.ToLower().StartsWith("up."))
                    {
                        upLevel = 1;
                    }
                    else
                    {
                        string upLevelStr = attributeName.Substring(2, attributeName.IndexOf(".") - 2);
                        upLevel = TranslateUtils.ToInt(upLevelStr);
                    }
                    topLevel = -1;
                    attributeName = attributeName.Substring(attributeName.IndexOf(".") + 1);
                }
                else if (attributeName.ToLower().StartsWith("top") && attributeName.IndexOf(".") != -1)
                {
                    if (attributeName.ToLower().StartsWith("top."))
                    {
                        topLevel = 1;
                    }
                    else
                    {
                        string topLevelStr = attributeName.Substring(3, attributeName.IndexOf(".") - 3);
                        topLevel = TranslateUtils.ToInt(topLevelStr);
                    }
                    upLevel = 0;
                    attributeName = attributeName.Substring(attributeName.IndexOf(".") + 1);
                }

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, channelID, upLevel, topLevel));

                if (StringUtils.EqualsIgnoreCase(StlChannelEntities.ChannelID, attributeName))//栏目ID
                {
                    parsedContent = nodeInfo.NodeID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.Title, attributeName) || StringUtils.EqualsIgnoreCase(StlChannelEntities.ChannelName, attributeName))//栏目名称
                {
                    parsedContent = nodeInfo.NodeName;
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.ChannelIndex, attributeName))//栏目索引
                {
                    parsedContent = nodeInfo.NodeIndexName;
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.Content, attributeName))//栏目正文
                {
                    parsedContent = StringUtility.TextEditorContentDecode(nodeInfo.Content, pageInfo.PublishmentSystemInfo);
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.NavigationUrl, attributeName))//栏目链接地址
                {
                    parsedContent = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, nodeInfo, pageInfo.VisualType);
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.ImageUrl, attributeName))//栏目图片地址
                {
                    parsedContent = nodeInfo.ImageUrl;

                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, parsedContent);
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.AddDate, attributeName))//栏目添加日期
                {
                    parsedContent = DateUtils.Format(nodeInfo.AddDate, string.Empty);
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.DirectoryName, attributeName))//生成文件夹名称
                {
                    parsedContent = PathUtils.GetDirectoryName(nodeInfo.FilePath);
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.Group, attributeName))//栏目组别
                {
                    parsedContent = nodeInfo.NodeGroupNameCollection;
                }
                else if (StringUtils.StartsWithIgnoreCase(attributeName, StlParserUtility.ItemIndex) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.ChannelItem != null)
                {
                    parsedContent = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.ChannelItem.ItemIndex, attributeName, contextInfo).ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(NodeAttribute.Keywords, attributeName))//栏目组别
                {
                    parsedContent = nodeInfo.Keywords;
                }
                else if (StringUtils.EqualsIgnoreCase(NodeAttribute.Description, attributeName))//栏目组别
                {
                    parsedContent = nodeInfo.Description;
                }
                else
                {
                    TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.Channel, DataProvider.NodeDAO.TableName, attributeName, RelatedIdentities.GetChannelRelatedIdentities(pageInfo.PublishmentSystemID, nodeInfo.NodeID));
                    parsedContent = InputTypeParser.GetContentByTableStyle(parsedContent, ",", pageInfo.PublishmentSystemInfo, ETableStyle.Channel, styleInfo, string.Empty, null, string.Empty, true);
                }
            }
            catch { }

            return parsedContent;
        }
	}
}
