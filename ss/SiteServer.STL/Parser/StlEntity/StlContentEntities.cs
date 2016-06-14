using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;

using SiteServer.CMS.BackgroundPages;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.CMS.Services;

namespace SiteServer.STL.Parser.StlEntity
{
    public class StlContentEntities
    {
        private StlContentEntities()
        {
        }

        public const string EntityName = "Content";                  //内容实体

        public static string ContentID = "ContentID";//内容ID
        public static string Title = "Title";//内容标题
        public static string FullTitle = "FullTitle";//内容标题全称
        public static string NavigationUrl = "NavigationUrl";//内容链接地址
        public static string ImageUrl = "ImageUrl";//内容图片地址
        public static string VideoUrl = "VideoUrl";//内容视频地址
        public static string FileUrl = "FileUrl";//内容附件地址
        public static string DownloadUrl = "DownloadUrl";//内容附件地址(可统计下载量)
        public static string AddDate = "AddDate";//内容添加日期
        public static string LastEditDate = "LastEditDate";//内容最后修改日期
        public static string Content = "Content";//内容正文
        public static string Group = "Group";//内容组别
        public static string Tags = "Tags";//内容标签
        public static string AddUserName = "AddUserName";//内容添加人
        public static string ItemIndex = "ItemIndex";//内容排序

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(ContentID, "内容ID");
                attributes.Add(Title, "内容标题");
                attributes.Add(FullTitle, "内容标题全称");
                attributes.Add(Content, "内容正文");
                attributes.Add(NavigationUrl, "内容链接地址");
                attributes.Add(ImageUrl, "内容图片地址");
                attributes.Add(VideoUrl, "内容视频地址");
                attributes.Add(FileUrl, "内容附件地址");
                attributes.Add(DownloadUrl, "内容附件地址(可统计下载量)");
                attributes.Add(AddDate, "内容添加日期");
                attributes.Add(LastEditDate, "内容最后修改日期");
                attributes.Add(Group, "内容组别");
                attributes.Add(Tags, "内容标签");
                attributes.Add(AddUserName, "内容添加人");
                attributes.Add(ItemIndex, "内容排序");
                return attributes;
            }
        }

        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            if (contextInfo.ContentID != 0)
            {
                try
                {
                    if (contextInfo.ContentInfo != null && contextInfo.ContentInfo.ReferenceID > 0 && contextInfo.ContentInfo.SourceID > 0 && contextInfo.ContentInfo.GetExtendedAttribute(ContentAttribute.TranslateContentType) != ETranslateContentType.ReferenceContent.ToString())
                    {
                        int targetNodeID = contextInfo.ContentInfo.SourceID;
                        int targetPublishmentSystemID = DataProvider.NodeDAO.GetPublishmentSystemID(targetNodeID);
                        PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);
                        NodeInfo targetNodeInfo = NodeManager.GetNodeInfo(targetPublishmentSystemID, targetNodeID);

                        ETableStyle tableStyle = NodeManager.GetTableStyle(targetPublishmentSystemInfo, targetNodeInfo);
                        string tableName = NodeManager.GetTableName(targetPublishmentSystemInfo, targetNodeInfo);
                        ContentInfo targetContentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contextInfo.ContentInfo.ReferenceID);
                        if (targetContentInfo != null || targetContentInfo.NodeID > 0)
                        {
                            //标题可以使用自己的
                            targetContentInfo.Title = contextInfo.ContentInfo.Title;

                            contextInfo.ContentInfo = targetContentInfo;
                        }
                    }



                    string entityName = StlParserUtility.GetNameFromEntity(stlEntity);
                    string attributeName = entityName.Substring(9, entityName.Length - 10);

                    if (StringUtils.EqualsIgnoreCase(StlContentEntities.ContentID, attributeName) || StringUtils.EqualsIgnoreCase(ContentAttribute.ID, attributeName))//内容ID
                    {
                        if (contextInfo.ContentInfo != null)
                        {
                            if (contextInfo.ContentInfo.ReferenceID > 0)
                            {
                                parsedContent = contextInfo.ContentInfo.ReferenceID.ToString();
                            }
                            else
                            {
                                parsedContent = contextInfo.ContentInfo.ID.ToString();
                            }
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID));
                            parsedContent = BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, ContentAttribute.ID);
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.Title, attributeName))//内容标题
                    {
                        if (contextInfo.ContentInfo != null)
                        {
                            parsedContent = contextInfo.ContentInfo.Title;
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID));
                            parsedContent = BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, ContentAttribute.Title);
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.FullTitle, attributeName))//内容标题全称
                    {
                        if (contextInfo.ContentInfo != null)
                        {
                            parsedContent = contextInfo.ContentInfo.Title;
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID));
                            parsedContent = BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, ContentAttribute.Title);
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.NavigationUrl, attributeName))//内容链接地址
                    {
                        if (contextInfo.ContentInfo != null)
                        {
                            parsedContent = PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, contextInfo.ContentInfo, pageInfo.VisualType);
                        }
                        else
                        {
                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID);
                            parsedContent = PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, nodeInfo, contextInfo.ContentID, pageInfo.VisualType);
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.ImageUrl, attributeName))//内容图片地址
                    {
                        if (contextInfo.ContentInfo != null)
                        {
                            parsedContent = contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl);
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID));
                            parsedContent = BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, BackgroundContentAttribute.ImageUrl);
                        }

                        if (!string.IsNullOrEmpty(parsedContent))
                        {
                            parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, parsedContent);
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.VideoUrl, attributeName))//内容视频地址
                    {
                        if (contextInfo.ContentInfo != null)
                        {
                            parsedContent = contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl);
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID));
                            parsedContent = BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, BackgroundContentAttribute.VideoUrl);
                        }

                        if (!string.IsNullOrEmpty(parsedContent))
                        {
                            parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, parsedContent);
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.FileUrl, attributeName))//内容附件地址
                    {
                        if (contextInfo.ContentInfo != null)
                        {
                            parsedContent = contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl);
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID));
                            parsedContent = BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, BackgroundContentAttribute.FileUrl);
                        }

                        if (!string.IsNullOrEmpty(parsedContent))
                        {
                            parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, parsedContent);
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.DownloadUrl, attributeName))//内容附件地址(可统计下载量)
                    {
                        if (contextInfo.ContentInfo != null)
                        {
                            parsedContent = contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl);
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID));
                            parsedContent = BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, BackgroundContentAttribute.FileUrl);
                        }

                        if (!string.IsNullOrEmpty(parsedContent))
                        {
                            parsedContent = PageUtility.ServiceSTL.Utils.GetDownloadUrl(pageInfo.PublishmentSystemID, contextInfo.ChannelID, contextInfo.ContentID);
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.AddDate, attributeName))//内容添加日期
                    {
                        if (contextInfo.ContentInfo != null)
                        {
                            parsedContent = DateUtils.Format(contextInfo.ContentInfo.AddDate, string.Empty);
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
                            parsedContent = DateUtils.Format(BaiRongDataProvider.ContentDAO.GetAddDate(tableName, contextInfo.ContentID), string.Empty);
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.LastEditDate, attributeName))//替换最后修改日期
                    {
                        if (contextInfo.ContentInfo != null)
                        {
                            parsedContent = DateUtils.Format(contextInfo.ContentInfo.LastEditDate, string.Empty);
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
                            parsedContent = DateUtils.Format(BaiRongDataProvider.ContentDAO.GetLastEditDate(tableName, contextInfo.ContentID), string.Empty);
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.Content, attributeName))//内容正文
                    {
                        if (contextInfo.ContentInfo != null)
                        {
                            parsedContent = contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content);
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID));
                            parsedContent = BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, BackgroundContentAttribute.Content);
                        }
                        parsedContent = StringUtility.TextEditorContentDecode(parsedContent, pageInfo.PublishmentSystemInfo);
                    }
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.Group, attributeName))//内容组别
                    {
                        if (contextInfo.ContentInfo != null)
                        {
                            parsedContent = contextInfo.ContentInfo.ContentGroupNameCollection;
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID));
                            parsedContent = BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, ContentAttribute.ContentGroupNameCollection);
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.Tags, attributeName))//标签
                    {
                        if (contextInfo.ContentInfo != null)
                        {
                            parsedContent = contextInfo.ContentInfo.Tags;
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID));
                            parsedContent = BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, ContentAttribute.Tags);
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.AddUserName, attributeName))
                    {
                        string addUserName = string.Empty;
                        if (contextInfo.ContentInfo != null)
                        {
                            addUserName = contextInfo.ContentInfo.AddUserName;
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID));
                            addUserName = BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, ContentAttribute.AddUserName);
                        }
                        if (!string.IsNullOrEmpty(addUserName))
                        {
                            string displayName = BaiRongDataProvider.AdministratorDAO.GetDisplayName(addUserName);
                            if (string.IsNullOrEmpty(displayName))
                            {
                                parsedContent = addUserName;
                            }
                            else
                            {
                                parsedContent = displayName;
                            }
                        }
                    }
                    else if (StringUtils.StartsWithIgnoreCase(attributeName, StlParserUtility.ItemIndex) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.ContentItem != null)
                    {
                        parsedContent = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.ContentItem.ItemIndex, attributeName, contextInfo).ToString();
                    }
                    else
                    {
                        int contentNodeID = 0;
                        if (contextInfo.ContentInfo != null)
                        {
                            contentNodeID = contextInfo.ContentInfo.NodeID;
                            if (contextInfo.ContentInfo.ContainsKey(attributeName))
                            {
                                parsedContent = contextInfo.ContentInfo.GetExtendedAttribute(attributeName);
                            }
                        }
                        else
                        {
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
                            contentNodeID = BaiRongDataProvider.ContentDAO.GetNodeID(tableName, contextInfo.ContentID);
                            tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contentNodeID));
                            parsedContent = BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, attributeName);
                        }

                        if (!string.IsNullOrEmpty(parsedContent))
                        {
                            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(pageInfo.PublishmentSystemID, contentNodeID);
                            TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, attributeName, relatedIdentities);
                            parsedContent = InputTypeParser.GetContentByTableStyle(parsedContent, ",", pageInfo.PublishmentSystemInfo, ETableStyle.BackgroundContent, styleInfo, string.Empty, null, string.Empty, true);
                        }

                    }
                }
                catch { }
            }
            return parsedContent;
        }
    }
}
