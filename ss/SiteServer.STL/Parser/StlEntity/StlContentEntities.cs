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

        public const string EntityName = "Content";                  //����ʵ��

        public static string ContentID = "ContentID";//����ID
        public static string Title = "Title";//���ݱ���
        public static string FullTitle = "FullTitle";//���ݱ���ȫ��
        public static string NavigationUrl = "NavigationUrl";//�������ӵ�ַ
        public static string ImageUrl = "ImageUrl";//����ͼƬ��ַ
        public static string VideoUrl = "VideoUrl";//������Ƶ��ַ
        public static string FileUrl = "FileUrl";//���ݸ�����ַ
        public static string DownloadUrl = "DownloadUrl";//���ݸ�����ַ(��ͳ��������)
        public static string AddDate = "AddDate";//�����������
        public static string LastEditDate = "LastEditDate";//��������޸�����
        public static string Content = "Content";//��������
        public static string Group = "Group";//�������
        public static string Tags = "Tags";//���ݱ�ǩ
        public static string AddUserName = "AddUserName";//���������
        public static string ItemIndex = "ItemIndex";//��������

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(ContentID, "����ID");
                attributes.Add(Title, "���ݱ���");
                attributes.Add(FullTitle, "���ݱ���ȫ��");
                attributes.Add(Content, "��������");
                attributes.Add(NavigationUrl, "�������ӵ�ַ");
                attributes.Add(ImageUrl, "����ͼƬ��ַ");
                attributes.Add(VideoUrl, "������Ƶ��ַ");
                attributes.Add(FileUrl, "���ݸ�����ַ");
                attributes.Add(DownloadUrl, "���ݸ�����ַ(��ͳ��������)");
                attributes.Add(AddDate, "�����������");
                attributes.Add(LastEditDate, "��������޸�����");
                attributes.Add(Group, "�������");
                attributes.Add(Tags, "���ݱ�ǩ");
                attributes.Add(AddUserName, "���������");
                attributes.Add(ItemIndex, "��������");
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
                            //�������ʹ���Լ���
                            targetContentInfo.Title = contextInfo.ContentInfo.Title;

                            contextInfo.ContentInfo = targetContentInfo;
                        }
                    }



                    string entityName = StlParserUtility.GetNameFromEntity(stlEntity);
                    string attributeName = entityName.Substring(9, entityName.Length - 10);

                    if (StringUtils.EqualsIgnoreCase(StlContentEntities.ContentID, attributeName) || StringUtils.EqualsIgnoreCase(ContentAttribute.ID, attributeName))//����ID
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
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.Title, attributeName))//���ݱ���
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
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.FullTitle, attributeName))//���ݱ���ȫ��
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
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.NavigationUrl, attributeName))//�������ӵ�ַ
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
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.ImageUrl, attributeName))//����ͼƬ��ַ
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
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.VideoUrl, attributeName))//������Ƶ��ַ
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
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.FileUrl, attributeName))//���ݸ�����ַ
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
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.DownloadUrl, attributeName))//���ݸ�����ַ(��ͳ��������)
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
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.AddDate, attributeName))//�����������
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
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.LastEditDate, attributeName))//�滻����޸�����
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
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.Content, attributeName))//��������
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
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.Group, attributeName))//�������
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
                    else if (StringUtils.EqualsIgnoreCase(StlContentEntities.Tags, attributeName))//��ǩ
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
