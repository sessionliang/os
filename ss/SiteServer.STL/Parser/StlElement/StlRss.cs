using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using BaiRong.Core.Rss;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlRss
    {
        private StlRss() { }
        public const string ElementName = "stl:rss";//Rss����

        public const string Attribute_Version = "version";		                //Rss�汾
        public const string Attribute_Title = "title";		                    //Rss���ı���
        public const string Attribute_Description = "description";		        //Rss����ժҪ
        public const string Attribute_Scope = "scope";							//���ݷ�Χ

        public const string Attribute_GroupChannel = "groupchannel";		    //ָ����ʾ����Ŀ��
        public const string Attribute_GroupChannelNot = "groupchannelnot";	    //ָ������ʾ����Ŀ��
        public const string Attribute_GroupContent = "groupcontent";		    //ָ����ʾ��������
        public const string Attribute_GroupContentNot = "groupcontentnot";	    //ָ������ʾ��������
        public const string Attribute_Tags = "tags";	                        //ָ����ǩ

        public const string Attribute_TotalNum = "totalnum";					//��ʾ������Ŀ
        public const string Attribute_StartNum = "startnum";					//�ӵڼ�����Ϣ��ʼ��ʾ
        public const string Attribute_Order = "order";						//����
        public const string Attribute_IsTop = "istop";                       //����ʾ�ö�����
        public const string Attribute_IsRecommend = "isrecommend";           //����ʾ�Ƽ�����
        public const string Attribute_IsHot = "ishot";                       //����ʾ�ȵ�����
        public const string Attribute_IsColor = "iscolor";                   //����ʾ��Ŀ����

        public const string Attribute_ChannelIndex = "channelindex";			//��Ŀ����
        public const string Attribute_ChannelName = "channelname";				//��Ŀ����

        public const string Version_RSS090 = "Rss0.9";		    //Rss0.9
        public const string Version_RSS10 = "Rss1.0";			//Rss1.0
        public const string Version_RSS20 = "Rss2.0";			//Rss2.0

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();

                attributes.Add(Attribute_ChannelIndex, "��Ŀ����");
                attributes.Add(Attribute_ChannelName, "��Ŀ����");
                attributes.Add(Attribute_Scope, "���ݷ�Χ");
                attributes.Add(Attribute_GroupChannel, "ָ����ʾ����Ŀ��");
                attributes.Add(Attribute_GroupChannelNot, "ָ������ʾ����Ŀ��");
                attributes.Add(Attribute_GroupContent, "ָ����ʾ��������");
                attributes.Add(Attribute_GroupContentNot, "ָ������ʾ��������");
                attributes.Add(Attribute_Tags, "ָ����ǩ");
                attributes.Add(Attribute_Version, "Rss�汾");
                attributes.Add(Attribute_Title, "Rss���ı���");
                attributes.Add(Attribute_Description, "Rss����ժҪ");
                attributes.Add(Attribute_TotalNum, "��ʾ������Ŀ");
                attributes.Add(Attribute_StartNum, "�ӵڼ�����Ϣ��ʼ��ʾ");
                attributes.Add(Attribute_Order, "����");
                attributes.Add(Attribute_IsTop, "����ʾ�ö�����");
                attributes.Add(Attribute_IsRecommend, "����ʾ�Ƽ�����");
                attributes.Add(Attribute_IsHot, "����ʾ�ȵ�����");
                attributes.Add(Attribute_IsColor, "����ʾ��Ŀ����");

                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();

                string version = string.Empty;
                string title = string.Empty;
                string description = string.Empty;
                string scopeTypeString = string.Empty;
                string groupChannel = string.Empty;
                string groupChannelNot = string.Empty;
                string groupContent = string.Empty;
                string groupContentNot = string.Empty;
                string tags = string.Empty;
                string channelIndex = string.Empty;
                string channelName = string.Empty;

                int totalNum = 0;
                int startNum = 1;
                string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);
                bool isTop = false;
                bool isTopExists = false;
                bool isRecommend = false;
                bool isRecommendExists = false;
                bool isHot = false;
                bool isHotExists = false;
                bool isColor = false;
                bool isColorExists = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlRss.Attribute_Version))
                    {
                        version = attr.Value;
                    }
                    else if (attributeName.Equals(StlRss.Attribute_Title))
                    {
                        title = attr.Value;
                    }
                    else if (attributeName.Equals(StlRss.Attribute_Description))
                    {
                        description = attr.Value;
                    }
                    else if (attributeName.Equals(StlRss.Attribute_Scope))
                    {
                        scopeTypeString = attr.Value;
                    }
                    else if (attributeName.Equals(StlRss.Attribute_ChannelIndex))
                    {
                        channelIndex = attr.Value;
                    }
                    else if (attributeName.Equals(StlRss.Attribute_ChannelName))
                    {
                        channelName = attr.Value;
                    }
                    else if (attributeName.Equals(StlRss.Attribute_GroupChannel))
                    {
                        groupChannel = attr.Value;
                    }
                    else if (attributeName.Equals(StlRss.Attribute_GroupChannelNot))
                    {
                        groupChannelNot = attr.Value;
                    }
                    else if (attributeName.Equals(StlRss.Attribute_GroupContent))
                    {
                        groupContent = attr.Value;
                    }
                    else if (attributeName.Equals(StlRss.Attribute_GroupContentNot))
                    {
                        groupContentNot = attr.Value;
                    }
                    else if (attributeName.Equals(StlRss.Attribute_Tags))
                    {
                        tags = attr.Value;
                    }
                    else if (attributeName.Equals(StlRss.Attribute_TotalNum))
                    {
                        totalNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlRss.Attribute_StartNum))
                    {
                        startNum = TranslateUtils.ToInt(attr.Value, 1);
                    }
                    else if (attributeName.Equals(StlRss.Attribute_Order))
                    {
                        orderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, attr.Value, ETableStyle.BackgroundContent, ETaxisType.OrderByTaxisDesc);
                    }
                    else if (attributeName.Equals(StlRss.Attribute_IsTop))
                    {
                        isTopExists = true;
                        isTop = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlRss.Attribute_IsRecommend))
                    {
                        isRecommendExists = true;
                        isRecommend = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlRss.Attribute_IsHot))
                    {
                        isHotExists = true;
                        isHot = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlRss.Attribute_IsColor))
                    {
                        isColorExists = true;
                        isColor = TranslateUtils.ToBool(attr.Value);
                    }
                }

                parsedContent = ParseImpl(pageInfo, contextInfo, version, title, description, scopeTypeString, groupChannel, groupChannelNot, groupContent, groupContentNot, tags, channelIndex, channelName, totalNum, startNum, orderByString, isTop, isTopExists, isRecommend, isRecommendExists, isHot, isHotExists, isColor, isColorExists);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string version, string title, string description, string scopeTypeString, string groupChannel, string groupChannelNot, string groupContent, string groupContentNot, string tags, string channelIndex, string channelName, int totalNum, int startNum, string orderByString, bool isTop, bool isTopExists, bool isRecommend, bool isRecommendExists, bool isHot, bool isHotExists, bool isColor, bool isColorExists)
        {
            string parsedContent = string.Empty;

            RssFeed feed = new RssFeed();
            feed.Encoding = ECharsetUtils.GetEncoding(pageInfo.TemplateInfo.Charset);
            if (string.IsNullOrEmpty(version))
            {
                feed.Version = RssVersion.RSS20;
            }
            else
            {
                feed.Version = (RssVersion)TranslateUtils.ToEnum(typeof(RssVersion), version, RssVersion.RSS20);
            }

            RssChannel channel = new RssChannel();
            channel.Title = title;
            channel.Description = description;

            EScopeType scopeType;
            if (!string.IsNullOrEmpty(scopeTypeString))
            {
                scopeType = EScopeTypeUtils.GetEnumType(scopeTypeString);
            }
            else
            {
                scopeType = EScopeType.All;
            }

            int channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, contextInfo.ChannelID, channelIndex, channelName);

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, channelID);
            if (string.IsNullOrEmpty(channel.Title))
            {
                channel.Title = nodeInfo.NodeName;
            }
            if (string.IsNullOrEmpty(channel.Description))
            {
                channel.Description = nodeInfo.Content;
                if (string.IsNullOrEmpty(channel.Description))
                {
                    channel.Description = nodeInfo.NodeName;
                }
                else
                {
                    channel.Description = StringUtils.MaxLengthText(channel.Description, 200);
                }
            }
            channel.Link = new Uri(PageUtils.AddProtocolToUrl(PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, nodeInfo, pageInfo.VisualType)));

            IEnumerable dataSource = StlDataUtility.GetContentsDataSource(pageInfo.PublishmentSystemInfo, channelID, 0, groupContent, groupContentNot, tags, false, false, false, false, false, false, false, false, startNum, totalNum, orderByString, isTopExists, isTop, isRecommendExists, isRecommend, isHotExists, isHot, isColorExists, isColor, string.Empty, scopeType, groupChannel, groupChannelNot, null);

            if (dataSource != null)
            {
                foreach (object dataItem in dataSource)
                {
                    RssItem item = new RssItem();

                    BackgroundContentInfo contentInfo = new BackgroundContentInfo(dataItem);
                    item.Title = StringUtils.Replace("&", contentInfo.Title, "&amp;");
                    item.Description = contentInfo.Summary;
                    if (string.IsNullOrEmpty(item.Description))
                    {
                        item.Description = StringUtils.StripTags(contentInfo.Content);
                        if (string.IsNullOrEmpty(item.Description))
                        {
                            item.Description = contentInfo.Title;
                        }
                        else
                        {
                            item.Description = StringUtils.MaxLengthText(item.Description, 200);
                        }
                    }
                    item.Description = StringUtils.Replace("&", item.Description, "&amp;");
                    item.PubDate = contentInfo.AddDate;
                    item.Link = new Uri(PageUtils.AddProtocolToUrl(PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, contentInfo, pageInfo.VisualType)));

                    channel.Items.Add(item);
                }
            }

            feed.Channels.Add(channel);

            StringBuilder builder = new StringBuilder();
            EncodedStringWriter textWriter = new EncodedStringWriter(builder, ECharsetUtils.GetEncoding(pageInfo.TemplateInfo.Charset));
            feed.Write(textWriter);

            parsedContent = builder.ToString();

            return parsedContent;
        }
    }
}
