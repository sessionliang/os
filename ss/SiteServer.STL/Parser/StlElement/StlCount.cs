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
		public const string ElementName = "stl:count";              //显示数值

		public const string Attribute_Type = "type";		        //需要获取值的类型
        public const string Attribute_ChannelIndex = "channelindex";			//栏目索引
        public const string Attribute_ChannelName = "channelname";				//栏目名称
        public const string Attribute_UpLevel = "uplevel";						//上级栏目的级别
        public const string Attribute_TopLevel = "toplevel";					//从首页向下的栏目级别
        public const string Attribute_Scope = "scope";							//内容范围
        public const string Attribute_Since = "since";				        //时间段
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public const string Type_Channels = "Channels";	            //栏目数
        public const string Type_Contents = "Contents";	            //内容数
        public const string Type_Comments = "Comments";	            //内容数
        public const string Type_Downloads = "Downloads";	        //下载次数

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_Type, "需要获取值的类型");
                attributes.Add(Attribute_ChannelIndex, "栏目索引");
                attributes.Add(Attribute_ChannelName, "栏目名称");
                attributes.Add(Attribute_UpLevel, "上级栏目的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的栏目级别");
                attributes.Add(Attribute_Scope, "内容范围");
                attributes.Add(Attribute_Since, "时间段");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
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
