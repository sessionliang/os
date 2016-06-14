using System.Collections;
using System.Collections.Specialized;
using System.Text;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlEntity
{
	public class StlNavigationEntities
	{
        private StlNavigationEntities()
		{
		}

        public const string EntityName = "Navigation";              //����ʵ��

        public static string PreviousChannel = "PreviousChannel";          //��һ��Ŀ����
        public static string NextChannel = "NextChannel";                  //��һ��Ŀ����
        public static string PreviousContent = "PreviousContent";          //��һ��������
        public static string NextContent = "NextContent";                  //��һ��������

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(PreviousChannel, "��һ��Ŀ����");
                attributes.Add(NextChannel, "��һ��Ŀ����");
                attributes.Add(PreviousContent, "��һ��������");
                attributes.Add(NextContent, "��һ��������");

                return attributes;
            }
        }

        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                string entityName = StlParserUtility.GetNameFromEntity(stlEntity);
                string attributeName = entityName.Substring(12, entityName.Length - 13);

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID);

                if (StringUtils.EqualsIgnoreCase(StlNavigationEntities.PreviousChannel, attributeName) || StringUtils.EqualsIgnoreCase(StlNavigationEntities.NextChannel, attributeName))
                {
                    int taxis = nodeInfo.Taxis;
                    bool isNextChannel = true;
                    if (StringUtils.EqualsIgnoreCase(attributeName, StlNavigationEntities.PreviousChannel))
                    {
                        isNextChannel = false;
                    }
                    int siblingNodeID = DataProvider.NodeDAO.GetNodeIDByParentIDAndTaxis(nodeInfo.ParentID, taxis, isNextChannel);
                    if (siblingNodeID != 0)
                    {
                        NodeInfo siblingNodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, siblingNodeID);
                        parsedContent = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, siblingNodeInfo, pageInfo.VisualType);
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(StlNavigationEntities.PreviousContent, attributeName) || StringUtils.EqualsIgnoreCase(StlNavigationEntities.NextContent, attributeName))
                {
                    if (contextInfo.ContentID != 0)
                    {
                        int taxis = contextInfo.ContentInfo.Taxis;
                        bool isNextContent = true;
                        if (StringUtils.EqualsIgnoreCase(attributeName, StlNavigationEntities.PreviousContent))
                        {
                            isNextContent = false;
                        }
                        ETableStyle tableStyle = NodeManager.GetTableStyle(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
                        string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
                        int siblingContentID = BaiRongDataProvider.ContentDAO.GetContentID(tableName, contextInfo.ChannelID, taxis, isNextContent);
                        if (siblingContentID != 0)
                        {
                            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, siblingContentID);
                            parsedContent = PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, contentInfo, pageInfo.VisualType);
                        }
                    }
                }
            }
            catch { }

            if (string.IsNullOrEmpty(parsedContent))
            {
                parsedContent = PageUtils.UNCLICKED_URL;
            }

            return parsedContent;
        }
	}
}
