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
	public class StlGovInteractApply
	{
        private StlGovInteractApply() { }
        public const string ElementName = "stl:govinteractapply";     //���������ύ

        public const string Attribute_ChannelID = "channelid";			        //��ĿID
        public const string Attribute_ChannelIndex = "channelindex";			//��Ŀ����
        public const string Attribute_ChannelName = "channelname";				//��Ŀ����
        public const string Attribute_InteractName = "interactname";				//������������

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_ChannelID, "��ĿID");
                attributes.Add(Attribute_ChannelIndex, "��Ŀ����");
                attributes.Add(Attribute_ChannelName, "��Ŀ����");
                attributes.Add(Attribute_InteractName, "������������");

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
                TagStyleGovInteractApplyInfo applyInfo = new TagStyleGovInteractApplyInfo(string.Empty);

                string inputTemplateString = string.Empty;
                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;
                StlParserUtility.GetInnerTemplateStringOfInput(node, out inputTemplateString, out successTemplateString, out failureTemplateString, pageInfo, contextInfo);

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlGovInteractApply.Attribute_ChannelID))
                    {
                        channelID = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlGovInteractApply.Attribute_ChannelIndex))
                    {
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlGovInteractApply.Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlGovInteractApply.Attribute_InteractName))
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
                int applyStyleID = DataProvider.GovInteractChannelDAO.GetApplyStyleID(nodeInfo.PublishmentSystemID, nodeInfo.NodeID);

                TagStyleInfo styleInfo = TagStyleManager.GetTagStyleInfo(applyStyleID);
                if (styleInfo == null)
                {
                    styleInfo = new TagStyleInfo();
                }
                TagStyleGovInteractApplyInfo applyInfo = new TagStyleGovInteractApplyInfo(styleInfo.SettingsXML);

                GovInteractApplyTemplate applyTemplate = new GovInteractApplyTemplate(pageInfo.PublishmentSystemInfo, nodeInfo.NodeID, styleInfo, applyInfo);
                StringBuilder contentBuilder = new StringBuilder(applyTemplate.GetTemplate(styleInfo.IsTemplate, inputTemplateString, successTemplateString, failureTemplateString));
                contentBuilder.Replace("{ChannelID}", nodeInfo.NodeID.ToString());

                StlParserManager.ParseTemplateContent(contentBuilder, pageInfo, contextInfo);
                parsedContent = contentBuilder.ToString();
            }

            return parsedContent;
        }
	}
}
