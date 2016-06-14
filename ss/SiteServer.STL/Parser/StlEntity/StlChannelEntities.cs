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

        public const string EntityName = "Channel";                  //��Ŀʵ��

        public static string ChannelID = "ChannelID";//��ĿID
        public static string ChannelName = "ChannelName";//��Ŀ����
        public static string ChannelIndex = "ChannelIndex";//��Ŀ����
		public static string Title = "Title";//��Ŀ����
        public static string Content = "Content";//��Ŀ����
        public static string NavigationUrl = "NavigationUrl";//��Ŀ���ӵ�ַ
        public static string ImageUrl = "ImageUrl";//��ĿͼƬ��ַ
        public static string AddDate = "AddDate";//��Ŀ�������
        public static string DirectoryName = "DirectoryName";//�����ļ�������
        public static string Group = "Group";//��Ŀ���
        public static string ItemIndex = "ItemIndex";//��Ŀ����

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(ChannelID, "��ĿID");
                attributes.Add(Title, "��Ŀ����");
                attributes.Add(ChannelName, "��Ŀ����");
                attributes.Add(ChannelIndex, "��Ŀ����");
                attributes.Add(Content, "��Ŀ����");
                attributes.Add(NavigationUrl, "��Ŀ���ӵ�ַ");
                attributes.Add(ImageUrl, "��ĿͼƬ��ַ");
                attributes.Add(AddDate, "��Ŀ�������");
                attributes.Add(DirectoryName, "�����ļ�������");
                attributes.Add(Group, "��Ŀ���");
                attributes.Add(ItemIndex, "��Ŀ����");
                
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

                if (StringUtils.EqualsIgnoreCase(StlChannelEntities.ChannelID, attributeName))//��ĿID
                {
                    parsedContent = nodeInfo.NodeID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.Title, attributeName) || StringUtils.EqualsIgnoreCase(StlChannelEntities.ChannelName, attributeName))//��Ŀ����
                {
                    parsedContent = nodeInfo.NodeName;
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.ChannelIndex, attributeName))//��Ŀ����
                {
                    parsedContent = nodeInfo.NodeIndexName;
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.Content, attributeName))//��Ŀ����
                {
                    parsedContent = StringUtility.TextEditorContentDecode(nodeInfo.Content, pageInfo.PublishmentSystemInfo);
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.NavigationUrl, attributeName))//��Ŀ���ӵ�ַ
                {
                    parsedContent = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, nodeInfo, pageInfo.VisualType);
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.ImageUrl, attributeName))//��ĿͼƬ��ַ
                {
                    parsedContent = nodeInfo.ImageUrl;

                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, parsedContent);
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.AddDate, attributeName))//��Ŀ�������
                {
                    parsedContent = DateUtils.Format(nodeInfo.AddDate, string.Empty);
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.DirectoryName, attributeName))//�����ļ�������
                {
                    parsedContent = PathUtils.GetDirectoryName(nodeInfo.FilePath);
                }
                else if (StringUtils.EqualsIgnoreCase(StlChannelEntities.Group, attributeName))//��Ŀ���
                {
                    parsedContent = nodeInfo.NodeGroupNameCollection;
                }
                else if (StringUtils.StartsWithIgnoreCase(attributeName, StlParserUtility.ItemIndex) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.ChannelItem != null)
                {
                    parsedContent = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.ChannelItem.ItemIndex, attributeName, contextInfo).ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(NodeAttribute.Keywords, attributeName))//��Ŀ���
                {
                    parsedContent = nodeInfo.Keywords;
                }
                else if (StringUtils.EqualsIgnoreCase(NodeAttribute.Description, attributeName))//��Ŀ���
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
