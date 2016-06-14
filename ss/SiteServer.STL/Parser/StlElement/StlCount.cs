using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlCount
	{
        private StlCount() { }
		public const string ElementName = "stl:count";              //��ʾ��ֵ

		public const string Attribute_Type = "type";		        //��Ҫ��ȡֵ������
        public const string Attribute_ChannelIndex = "channelindex";			//��Ŀ����
        public const string Attribute_ChannelName = "channelname";				//��Ŀ����
        public const string Attribute_UpLevel = "uplevel";						//�ϼ���Ŀ�ļ���
        public const string Attribute_TopLevel = "toplevel";					//����ҳ���µ���Ŀ����
        public const string Attribute_Scope = "scope";							//���ݷ�Χ
        public const string Attribute_Since = "since";				        //ʱ���
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

        public const string Type_Channels = "Channels";	            //��Ŀ��
        public const string Type_Contents = "Contents";	            //������
        public const string Type_Comments = "Comments";	            //������
        public const string Type_Downloads = "Downloads";	        //���ش���

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_Type, "��Ҫ��ȡֵ������");
                attributes.Add(Attribute_ChannelIndex, "��Ŀ����");
                attributes.Add(Attribute_ChannelName, "��Ŀ����");
                attributes.Add(Attribute_UpLevel, "�ϼ���Ŀ�ļ���");
                attributes.Add(Attribute_TopLevel, "����ҳ���µ���Ŀ����");
                attributes.Add(Attribute_Scope, "���ݷ�Χ");
                attributes.Add(Attribute_Since, "ʱ���");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
				return attributes;
			}
		}

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
			try
			{
				IEnumerator ie = node.Attributes.GetEnumerator();

				string type = string.Empty;
                string channelIndex = string.Empty;
                string channelName = string.Empty;
                int upLevel = 0;
                int topLevel = -1;
                EScopeType scope = EScopeType.Self;
                string since = string.Empty;
                bool isDynamic = false;

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(StlCount.Attribute_Type))
					{
						type = attr.Value;
                    }
                    else if (attributeName.Equals(StlCount.Attribute_ChannelIndex))
                    {
                        channelIndex = attr.Value;
                    }
                    else if (attributeName.Equals(StlCount.Attribute_ChannelName))
                    {
                        channelName = attr.Value;
                    }
                    else if (attributeName.Equals(StlCount.Attribute_UpLevel))
                    {
                        upLevel = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlCount.Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlCount.Attribute_Scope))
                    {
                        scope = EScopeTypeUtils.GetEnumType(attr.Value);
                    }
                    else if (attributeName.Equals(StlCount.Attribute_Since))
                    {
                        since = attr.Value;
                    }
                    else if (attributeName.Equals(StlCount.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value, false);
                    }
				}

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, type, channelIndex, channelName, upLevel, topLevel, scope, since);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string type, string channelIndex, string channelName, int upLevel, int topLevel, EScopeType scope, string since)
        {
            int count = 0;

            DateTime sinceDate = DateUtils.SqlMinValue;
            if (!string.IsNullOrEmpty(since))
            {
                sinceDate = DateTime.Now.AddHours(-DateUtils.GetSinceHours(since));
            }

            if (string.IsNullOrEmpty(type) || StringUtils.EqualsIgnoreCase(type, StlCount.Type_Contents))
            {
                int channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);
                channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, channelID);

                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, scope, string.Empty, string.Empty);
                foreach (int nodeID in nodeIDArrayList)
                {
                    string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, nodeID);
                    count += DataProvider.ContentDAO.GetCountOfContentAdd(tableName, pageInfo.PublishmentSystemID, nodeID, sinceDate, DateTime.Now.AddDays(1), string.Empty);
                }
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlCount.Type_Channels))
            {
                int channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);
                channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, channelID);
                count = nodeInfo.ChildrenCount;
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlCount.Type_Comments))
            {
                count = DataProvider.CommentDAO.GetCountChecked(pageInfo.PublishmentSystemID, contextInfo.ChannelID, contextInfo.ContentID);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlCount.Type_Downloads))
            {
                if (contextInfo.ContentID > 0)
                {
                    count = CountManager.GetCount(AppManager.CMS.AppID, pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contextInfo.ContentID.ToString(), ECountType.Download);
                }
            }

            return count.ToString();
        }
	}
}
