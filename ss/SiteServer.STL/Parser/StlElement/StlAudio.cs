using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlAudio
	{
        private StlAudio() { }
		public const string ElementName = "stl:audio";      //��Ƶ����

		public const string Attribute_ChannelIndex = "channelindex";			//��Ŀ����
		public const string Attribute_ChannelName = "channelname";				//��Ŀ����
		public const string Attribute_Parent = "parent";						//��ʾ����Ŀ
		public const string Attribute_UpLevel = "uplevel";						//�ϼ���Ŀ�ļ���
        public const string Attribute_TopLevel = "toplevel";					//����ҳ���µ���Ŀ����
        public const string Attribute_Type = "type";                            //ָ���洢ý����ֶ�

        public const string Attribute_PlayUrl = "playurl";      				    //��Ƶ��ַ
        public const string Attribute_IsAutoPlay = "isautoplay";                    //�Ƿ��Զ�����
        public const string Attribute_IsPreLoad = "ispreload";                      //�Ƿ�Ԥ����
        public const string Attribute_IsLoop = "isloop";                            //�Ƿ�ѭ������
        public const string Attribute_IsDynamic = "isdynamic";                      //�Ƿ�̬��ʾ

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_ChannelIndex, "��Ŀ����");
				attributes.Add(Attribute_ChannelName, "��Ŀ����");
				attributes.Add(Attribute_Parent, "��ʾ����Ŀ");
				attributes.Add(Attribute_UpLevel, "�ϼ���Ŀ�ļ���");
                attributes.Add(Attribute_TopLevel, "����ҳ���µ���Ŀ����");
                attributes.Add(Attribute_Type, "ָ����Ƶ���ֶ�");
                attributes.Add(Attribute_PlayUrl, "��Ƶ��ַ");
                attributes.Add(Attribute_IsAutoPlay, "�Ƿ��Զ�����");
                attributes.Add(Attribute_IsPreLoad, "�Ƿ�Ԥ����");
                attributes.Add(Attribute_IsLoop, "�Ƿ�ѭ������");
                
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
				int upLevel = 0;
                int topLevel = -1;
                string type = BackgroundContentAttribute.VideoUrl;
				string playUrl = string.Empty;
                bool isAutoPlay = false;
                bool isPreLoad = true;
                bool isLoop = false;
                bool isDynamic = false;
                NameValueCollection parameters = new NameValueCollection();

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(StlAudio.Attribute_ChannelIndex))
					{
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
						if (!string.IsNullOrEmpty(channelIndex))
						{
                            isGetUrlFromAttribute = true;
						}
					}
                    else if (attributeName.Equals(StlAudio.Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        if (!string.IsNullOrEmpty(channelName))
                        {
                            isGetUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlAudio.Attribute_Parent))
                    {
                        if (TranslateUtils.ToBool(attr.Value))
                        {
                            upLevel = 1;
                            isGetUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlAudio.Attribute_UpLevel))
                    {
                        upLevel = TranslateUtils.ToInt(attr.Value);
                        if (upLevel > 0)
                        {
                            isGetUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlAudio.Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                        if (topLevel >= 0)
                        {
                            isGetUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlAudio.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(StlAudio.Attribute_PlayUrl))
                    {
                        playUrl = attr.Value;
                    }
                    else if (attributeName.Equals(StlAudio.Attribute_IsAutoPlay))
                    {
                        isAutoPlay = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlAudio.Attribute_IsPreLoad))
                    {
                        isPreLoad = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlAudio.Attribute_IsLoop))
                    {
                        isLoop = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlAudio.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                    else
                    {
                        parameters.Add(attr.Name, attr.Value);
                    }
				}

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(stlElement, node, pageInfo, contextInfo, isGetUrlFromAttribute, channelIndex, channelName, upLevel, topLevel, type, playUrl, isAutoPlay, isPreLoad, isLoop, parameters);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, bool isGetUrlFromAttribute, string channelIndex, string channelName, int upLevel, int topLevel, string type, string playUrl, bool isAutoPlay, bool isPreLoad, bool isLoop, NameValueCollection parameters)
        {
            string parsedContent = string.Empty;

            int contentID = 0;
            //�ж��Ƿ��ַ�ɱ�ǩ���Ի��
            if (!isGetUrlFromAttribute)
            {
                contentID = contextInfo.ContentID;
            }

            if (string.IsNullOrEmpty(playUrl))
            {
                if (contentID != 0)//��ȡ������Ƶ
                {
                    if (contextInfo.ContentInfo == null)
                    {
                        playUrl = BaiRongDataProvider.ContentDAO.GetValue(pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, type);
                        if (string.IsNullOrEmpty(playUrl))
                        {
                            if (!StringUtils.EqualsIgnoreCase(type, BackgroundContentAttribute.VideoUrl))
                            {
                                playUrl = BaiRongDataProvider.ContentDAO.GetValue(pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, BackgroundContentAttribute.VideoUrl);
                            }
                        }
                        if (string.IsNullOrEmpty(playUrl))
                        {
                            if (!StringUtils.EqualsIgnoreCase(type, BackgroundContentAttribute.FileUrl))
                            {
                                playUrl = BaiRongDataProvider.ContentDAO.GetValue(pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, BackgroundContentAttribute.FileUrl);
                            }
                        }
                    }
                    else
                    {
                        playUrl = contextInfo.ContentInfo.GetExtendedAttribute(type);
                        if (string.IsNullOrEmpty(playUrl))
                        {
                            playUrl = contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl);
                        }
                        if (string.IsNullOrEmpty(playUrl))
                        {
                            playUrl = contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(playUrl))
            {
                playUrl = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, playUrl);

                pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);
                pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ac_MediaElement);

                parsedContent = string.Format(@"
<audio src=""{0}"" {1} {2} {3}>
    <object width=""460"" height=""40"" type=""application/x-shockwave-flash"" data=""{4}"">
        <param name=""movie"" value=""{4}"" />
        <param name=""flashvars"" value=""controls=true&file={0}"" />
    </object>
</audio>
<script>$('audio').mediaelementplayer();</script>
", playUrl, isAutoPlay ? "autoplay" : string.Empty, isPreLoad ? string.Empty : @"preload=""none""", isLoop ? "loop" : string.Empty, PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.MediaElement.Swf));

                //pageInfo.AddPageHeadScriptsIfNotExists(PageInfo.Js_Ac_AudioJs);

                //parsedContent = string.Format(@"<audio src=""{0}"" {1} {2} {3}></audio>", playUrl, isAutoPlay ? "autoplay" : string.Empty, isPreLoad ? string.Empty : @"preload=""none""", isLoop ? "loop" : string.Empty);
            }

            return parsedContent;
        }
	}
}
