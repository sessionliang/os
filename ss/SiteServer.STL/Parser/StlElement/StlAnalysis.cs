using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using System.Text;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlAnalysis
	{
        private StlAnalysis() { }
        public const string ElementName = "stl:analysis";//��ʾ�����

        public const string Attribute_ChannelIndex = "channelindex";			//��Ŀ����
        public const string Attribute_ChannelName = "channelname";				//��Ŀ����

		public const string Attribute_Type = "type";		                    //��ʾ����
        public const string Attribute_Scope = "scope";		                    //ͳ�Ʒ�Χ
        public const string Attribute_IsAverage = "isaverage";		            //�Ƿ���ʾ�վ���
        public const string Attribute_Style = "style";		                    //��ʾ��ʽ
        public const string Attribute_AddNum = "addnum";		                //�����Ŀ
        public const string Attribute_Since = "since";				        //ʱ���
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

        public static readonly string Type_PageView = "PageView";               //��ʾ��������PV��
        public static readonly string Type_UniqueVisitor = "UniqueVisitor";     //��ʾ�����ÿ�����UV��
        public static readonly string Type_CurrentVisitor = "CurrentVisitor";   //��ʾ��ǰ����

        public static readonly string Scope_Site = "Site";                      //ͳ��ȫվ
        public static readonly string Scope_Page = "Page";                      //ͳ��ҳ��

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_ChannelIndex, "��Ŀ����");
                attributes.Add(Attribute_ChannelName, "��Ŀ����");
				attributes.Add(Attribute_Type, "ͳ������");
                attributes.Add(Attribute_Scope, "ͳ�Ʒ�Χ");
                attributes.Add(Attribute_IsAverage, "�Ƿ���ʾ�վ���");
                attributes.Add(Attribute_Style, "��ʾ��ʽ");
                attributes.Add(Attribute_AddNum, "�����Ŀ");
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

                bool isGetUrlFromAttribute = false;
                string channelIndex = string.Empty;
                string channelName = string.Empty;

                string type = StlAnalysis.Type_PageView;
                string scope = StlAnalysis.Scope_Page;
                bool isAverage = false;
                string style = string.Empty;
                int addNum = 0;
                string since = string.Empty;
                bool isDynamic = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlAnalysis.Attribute_ChannelIndex))
                    {
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        if (!string.IsNullOrEmpty(channelIndex))
                        {
                            isGetUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlAnalysis.Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        if (!string.IsNullOrEmpty(channelName))
                        {
                            isGetUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlAnalysis.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(StlAnalysis.Attribute_Scope))
                    {
                        scope = attr.Value;
                    }
                    else if (attributeName.Equals(StlAnalysis.Attribute_IsAverage))
                    {
                        isAverage = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlAnalysis.Attribute_Style))
                    {
                        style = ETrackerStyleUtils.GetValue(ETrackerStyleUtils.GetEnumType(attr.Value));
                    }
                    else if (attributeName.Equals(StlAnalysis.Attribute_AddNum))
                    {
                        addNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlAnalysis.Attribute_Since))
                    {
                        since = attr.Value;
                    }
                    else if (attributeName.Equals(StlAnalysis.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, isGetUrlFromAttribute, channelIndex, channelName, type, scope, isAverage, style, addNum, since);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, bool isGetUrlFromAttribute, string channelIndex, string channelName , string type, string scope, bool isAverage, string style, int addNum, string since)
        {
            int channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, contextInfo.ChannelID, channelIndex, channelName);

            int contentID = 0;
            //�ж��Ƿ����ӵ�ַ�ɱ�ǩ���Ի��
            if (isGetUrlFromAttribute == false)
            {
                contentID = contextInfo.ContentID;
            }

            int updaterID = pageInfo.UniqueID;

            ETemplateType templateType = pageInfo.TemplateInfo.TemplateType;
            if (contextInfo.ChannelID != 0)
            {
                templateType = ETemplateType.ChannelTemplate;
                if (contentID != 0)
                {
                    templateType = ETemplateType.ContentTemplate;
                }
            }

            return StlAnalysis.GetAnalysisValue(pageInfo.PublishmentSystemID, channelID, contentID, ETemplateTypeUtils.GetValue(templateType), type, scope, isAverage, style, addNum, since, string.Empty);
        }

        private static string GetAnalysisValue(int publishmentSystemID, int channelID, int contentID, string templateType, string type, string scope, bool isAverage, string style, int addNum, string since, string referrer)
        {
            string html = string.Empty;

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            if (publishmentSystemInfo != null)
            {
                ETrackerStyle eStyle = publishmentSystemInfo.Additional.TrackerStyle;
                if (!string.IsNullOrEmpty(style))
                {
                    eStyle = ETrackerStyleUtils.GetEnumType(style);
                }
                if (string.IsNullOrEmpty(scope) || !StringUtils.EqualsIgnoreCase(scope, StlAnalysis.Scope_Site))
                {
                    scope = StlAnalysis.Scope_Page;
                }
                ETemplateType eTemplateType = ETemplateTypeUtils.GetEnumType(templateType);

                int accessNum = 0;
                DateTime sinceDate = DateUtils.SqlMinValue;
                if (!string.IsNullOrEmpty(since))
                {
                    sinceDate = DateTime.Now.AddHours(-DateUtils.GetSinceHours(since));
                }

                if (StringUtils.EqualsIgnoreCase(type, StlAnalysis.Type_PageView))
                {
                    if (StringUtils.EqualsIgnoreCase(scope, StlAnalysis.Scope_Page))
                    {
                        if (eTemplateType != ETemplateType.FileTemplate)
                        {
                            accessNum = DataProvider.TrackingDAO.GetTotalAccessNumByPageInfo(publishmentSystemID, channelID, contentID, sinceDate);
                        }
                        else
                        {
                            accessNum = DataProvider.TrackingDAO.GetTotalAccessNumByPageUrl(publishmentSystemID, referrer, sinceDate);
                        }
                    }
                    else
                    {
                        accessNum = DataProvider.TrackingDAO.GetTotalAccessNum(publishmentSystemID, sinceDate);
                        accessNum = accessNum + publishmentSystemInfo.Additional.TrackerPageView;
                    }
                    if (isAverage)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, publishmentSystemID);
                        TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - nodeInfo.AddDate.Ticks);
                        if (!string.IsNullOrEmpty(since))
                        {
                            timeSpan = new TimeSpan(DateTime.Now.Ticks - sinceDate.Ticks);
                        }
                        int trackerDays = (timeSpan.Days == 0) ? 1 : timeSpan.Days;//��ͳ������
                        trackerDays = trackerDays + publishmentSystemInfo.Additional.TrackerDays;
                        accessNum = Convert.ToInt32(Math.Round(Convert.ToDouble(accessNum / trackerDays)));
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlAnalysis.Type_UniqueVisitor))
                {
                    if (StringUtils.EqualsIgnoreCase(scope, StlAnalysis.Scope_Page))
                    {
                        if (eTemplateType != ETemplateType.FileTemplate)
                        {
                            accessNum = DataProvider.TrackingDAO.GetTotalUniqueAccessNumByPageInfo(publishmentSystemID, channelID, contentID, sinceDate);
                        }
                        else
                        {
                            accessNum = DataProvider.TrackingDAO.GetTotalUniqueAccessNumByPageUrl(publishmentSystemID, referrer, sinceDate);
                        }
                    }
                    else
                    {
                        accessNum = DataProvider.TrackingDAO.GetTotalUniqueAccessNum(publishmentSystemID, sinceDate);
                        accessNum = accessNum + publishmentSystemInfo.Additional.TrackerUniqueVisitor;
                    }
                    if (isAverage)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, publishmentSystemID);
                        TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - nodeInfo.AddDate.Ticks);
                        int trackerDays = (timeSpan.Days == 0) ? 1 : timeSpan.Days;//��ͳ������
                        trackerDays = trackerDays + publishmentSystemInfo.Additional.TrackerDays;
                        accessNum = Convert.ToInt32(Math.Round(Convert.ToDouble(accessNum / trackerDays)));
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlAnalysis.Type_CurrentVisitor))
                {
                    accessNum = DataProvider.TrackingDAO.GetCurrentVisitorNum(publishmentSystemID, publishmentSystemInfo.Additional.TrackerCurrentMinute);
                }

                accessNum = accessNum + addNum;
                if (accessNum == 0) accessNum = 1;

                switch (eStyle)
                {
                    case ETrackerStyle.None:
                        break;
                    case ETrackerStyle.Number:
                        html = accessNum.ToString();
                        break;
                    default:
                        string numString = accessNum.ToString();
                        StringBuilder htmlBuilder = new StringBuilder();
                        string imgFolder = string.Format("{0}/{1}", PageUtility.GetIconUrl(publishmentSystemInfo, "tracker"), ETrackerStyleUtils.GetValue(eStyle));
                        for (int index = 0; index < numString.Length; index++)
                        {
                            string imgHtml = string.Format("<img src='{0}/{1}.gif' align=absmiddle border=0>", imgFolder, numString[index]);
                            htmlBuilder.Append(imgHtml);
                        }
                        html = htmlBuilder.ToString();
                        break;
                }
            }
            return html;
        }
	}
}
