using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System.Text.RegularExpressions;
using BaiRong.Model;
using System;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlGovInteractQuery
	{
        private StlGovInteractQuery() { }
        public const string ElementName = "stl:govinteractquery";     //互动交流查询

        public const string Attribute_ChannelID = "channelid";			        //栏目ID
        public const string Attribute_ChannelIndex = "channelindex";			//栏目索引
        public const string Attribute_ChannelName = "channelname";				//栏目名称
        public const string Attribute_InteractName = "interactname";				//互动交流名称

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_ChannelID, "栏目ID");
                attributes.Add(Attribute_ChannelIndex, "栏目索引");
                attributes.Add(Attribute_ChannelName, "栏目名称");
                attributes.Add(Attribute_InteractName, "互动交流名称");

				return attributes;
			}
		}

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                string channelID = string.Empty;
                string channelIndex = string.Empty;
                string channelName = string.Empty;
                string interactName = string.Empty;

                string inputTemplateString = string.Empty;
                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;
                StlParserUtility.GetInnerTemplateStringOfInput(node, out inputTemplateString, out successTemplateString, out failureTemplateString, pageInfo, contextInfo);

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlGovInteractQuery.Attribute_ChannelID))
                    {
                        channelID = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlGovInteractQuery.Attribute_ChannelIndex))
                    {
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlGovInteractQuery.Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlGovInteractQuery.Attribute_InteractName))
                    {
                        interactName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                }

                parsedContent = ParseImpl(pageInfo, contextInfo, TranslateUtils.ToInt(channelID), channelIndex, channelName, interactName, inputTemplateString, successTemplateString, failureTemplateString);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        public static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, int channelID, string channelIndex, string channelName, string interactName, string inputTemplateString, string successTemplateString, string failureTemplateString)
        {
            string parsedContent = string.Empty;

            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);
            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_ShowLoading);
            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_JTemplates);
            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.B_Validate);

            int nodeID = channelID;
            if (!string.IsNullOrEmpty(interactName))
            {
                nodeID = DataProvider.GovInteractChannelDAO.GetNodeIDByInteractName(pageInfo.PublishmentSystemID, interactName);
            }
            if (nodeID == 0)
            {
                nodeID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, pageInfo.PublishmentSystemID, channelIndex, channelName);
            }
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, nodeID);
            if (nodeInfo == null || !EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.GovInteract))
            {
                nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, pageInfo.PublishmentSystemInfo.Additional.GovInteractNodeID);
            }
            if (nodeInfo != null)
            {
                int queryStyleID = DataProvider.GovInteractChannelDAO.GetQueryStyleID(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);

                TagStyleInfo styleInfo = TagStyleManager.GetTagStyleInfo(queryStyleID);
                if (styleInfo == null)
                {
                    styleInfo = new TagStyleInfo();
                }

                GovInteractQueryTemplate queryTemplate = new GovInteractQueryTemplate(pageInfo.PublishmentSystemInfo, nodeInfo.NodeID, styleInfo);
                StringBuilder contentBuilder = new StringBuilder(queryTemplate.GetTemplate(styleInfo.IsTemplate, inputTemplateString, successTemplateString, failureTemplateString));

                StlParserManager.ParseTemplateContent(contentBuilder, pageInfo, contextInfo);
                parsedContent = contentBuilder.ToString();
            }

            return parsedContent;
        }
	}
}
