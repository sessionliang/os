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
    public class StlVideo
	{
        private StlVideo() { }
		public const string ElementName = "stl:video";      //��Ƶ����

		public const string Attribute_ChannelIndex = "channelindex";			//��Ŀ����
		public const string Attribute_ChannelName = "channelname";				//��Ŀ����
		public const string Attribute_Parent = "parent";						//��ʾ����Ŀ
		public const string Attribute_UpLevel = "uplevel";						//�ϼ���Ŀ�ļ���
        public const string Attribute_TopLevel = "toplevel";					//����ҳ���µ���Ŀ����
        public const string Attribute_Type = "type";                            //ָ���洢ý����ֶ�

		public const string Attribute_PlayUrl = "playurl";      				    //��Ƶ��ַ
        public const string Attribute_ImageUrl = "imageurl";						//ͼƬ��ַ
        public const string Attribute_Width = "width";							    //���
        public const string Attribute_Height = "height";						    //�߶�
        public const string Attribute_IsAutoPlay = "isautoplay";                    //�Ƿ��Զ�����
        public const string Attribute_IsControls = "iscontrols";                    //�Ƿ��ṩ���ſؼ�
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
                attributes.Add(Attribute_ImageUrl, "ͼƬ��ַ");
                attributes.Add(Attribute_Width, "���");
                attributes.Add(Attribute_Height, "�߶�");
                attributes.Add(Attribute_IsAutoPlay, "�Ƿ��Զ�����");
                attributes.Add(Attribute_IsControls, "�Ƿ���ʾ���ſؼ�");
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
                string imageUrl = string.Empty;
                int width = pageInfo.PublishmentSystemInfo.Additional.Config_VideoContentInsertWidth;
                int height = pageInfo.PublishmentSystemInfo.Additional.Config_VideoContentInsertHeight;
                bool isAutoPlay = true;
                bool isControls = true;
                bool isPreLoad = true;
                bool isLoop = false;
                bool isDynamic = false;
                NameValueCollection parameters = new NameValueCollection();

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(StlVideo.Attribute_ChannelIndex))
					{
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
						if (!string.IsNullOrEmpty(channelIndex))
						{
                            isGetUrlFromAttribute = true;
						}
					}
                    else if (attributeName.Equals(StlVideo.Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        if (!string.IsNullOrEmpty(channelName))
                        {
                            isGetUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlVideo.Attribute_Parent))
                    {
                        if (TranslateUtils.ToBool(attr.Value))
                        {
                            upLevel = 1;
                            isGetUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlVideo.Attribute_UpLevel))
                    {
                        upLevel = TranslateUtils.ToInt(attr.Value);
                        if (upLevel > 0)
                        {
                            isGetUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlVideo.Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                        if (topLevel >= 0)
                        {
                            isGetUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlVideo.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(StlVideo.Attribute_PlayUrl))
                    {
                        playUrl = attr.Value;
                    }
                    else if (attributeName.Equals(StlVideo.Attribute_ImageUrl))
                    {
                        imageUrl = attr.Value;
                    }
                    else if (attributeName.Equals(StlVideo.Attribute_Width))
                    {
                        width = TranslateUtils.ToInt(attr.Value, width);
                    }
                    else if (attributeName.Equals(StlVideo.Attribute_Height))
                    {
                        height = TranslateUtils.ToInt(attr.Value, height);
                    }
                    else if (attributeName.Equals(StlVideo.Attribute_IsAutoPlay))
                    {
                        isAutoPlay = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlVideo.Attribute_IsControls))
                    {
                        isControls = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlVideo.Attribute_IsPreLoad))
                    {
                        isPreLoad = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlVideo.Attribute_IsLoop))
                    {
                        isLoop = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlVideo.Attribute_IsDynamic))
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
                    parsedContent = ParseImpl(stlElement, node, pageInfo, contextInfo, isGetUrlFromAttribute, channelIndex, channelName, upLevel, topLevel, type, playUrl, imageUrl, width, height, isAutoPlay, isControls, isPreLoad, isLoop, parameters);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, bool isGetUrlFromAttribute, string channelIndex, string channelName, int upLevel, int topLevel, string type, string playUrl, string imageUrl, int width, int height, bool isAutoPlay, bool isControls, bool isPreLoad, bool isLoop, NameValueCollection parameters)
        {
            string parsedContent = string.Empty;

            int contentID = 0;
            //�ж��Ƿ��ַ�ɱ�ǩ���Ի��
            if (!isGetUrlFromAttribute)
            {
                contentID = contextInfo.ContentID;
            }

            string videoUrl = string.Empty;
            if (!string.IsNullOrEmpty(playUrl))
            {
                videoUrl = playUrl;
            }
            else
            {
                if (contextInfo.ContextType == EContextType.Content)
                {
                    if (contentID != 0)//��ȡ������Ƶ
                    {
                        if (contextInfo.ContentInfo == null)
                        {
                            videoUrl = BaiRongDataProvider.ContentDAO.GetValue(pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, type);
                            if (string.IsNullOrEmpty(videoUrl))
                            {
                                if (!StringUtils.EqualsIgnoreCase(type, BackgroundContentAttribute.VideoUrl))
                                {
                                    videoUrl = BaiRongDataProvider.ContentDAO.GetValue(pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, BackgroundContentAttribute.VideoUrl);
                                }
                            }
                            if (string.IsNullOrEmpty(videoUrl))
                            {
                                if (!StringUtils.EqualsIgnoreCase(type, BackgroundContentAttribute.FileUrl))
                                {
                                    videoUrl = BaiRongDataProvider.ContentDAO.GetValue(pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, BackgroundContentAttribute.FileUrl);
                                }
                            }
                        }
                        else
                        {
                            videoUrl = contextInfo.ContentInfo.GetExtendedAttribute(type);
                            if (string.IsNullOrEmpty(videoUrl))
                            {
                                videoUrl = contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl);
                            }
                            if (string.IsNullOrEmpty(videoUrl))
                            {
                                videoUrl = contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl);
                            }
                        }
                    }
                }
                else if (contextInfo.ContextType == EContextType.Each)
                {
                    videoUrl = contextInfo.ItemContainer.EachItem.DataItem as string;
                }
            }
            
            if (string.IsNullOrEmpty(imageUrl))
            {
                if (contentID != 0)
                {
                    if (contextInfo.ContentInfo == null)
                    {
                        imageUrl = BaiRongDataProvider.ContentDAO.GetValue(pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, BackgroundContentAttribute.ImageUrl);
                    }
                    else
                    {
                        imageUrl = contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl);
                    }
                }
            }

            if (string.IsNullOrEmpty(imageUrl))
            {
                int channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);
                channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);
                NodeInfo channel = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, channelID);
                imageUrl = channel.ImageUrl;
            }

            if (!string.IsNullOrEmpty(videoUrl))
            {
                videoUrl = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, videoUrl);
                imageUrl = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, imageUrl);

                pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ac_VideoJs);

                parsedContent = string.Format(@"<video class=""video-js vjs-default-skin"" src=""{0}"" width=""{1}"" height=""{2}"" {3} {4} {5} {6} {7}></video>", videoUrl, width, height, isAutoPlay ? "autoplay" : string.Empty, isControls ? "controls" : string.Empty, isPreLoad ? string.Empty : @"preload=""none""", isLoop ? "loop" : string.Empty, string.IsNullOrEmpty(imageUrl) ? string.Empty : string.Format(@"poster=""{0}""", imageUrl));
            }

            return parsedContent;
        }
	}
}
