using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using System.Web.UI;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlImage
	{
		private StlImage(){}
		public const string ElementName = "stl:image";//��ʾͼƬ

		public const string Attribute_ChannelIndex = "channelindex";			//��Ŀ����
		public const string Attribute_ChannelName = "channelname";				//��Ŀ����
        public const string Attribute_NO = "no";                            //��ʾ�ֶε�˳��
		public const string Attribute_Parent = "parent";						//��ʾ����Ŀ
		public const string Attribute_UpLevel = "uplevel";						//�ϼ���Ŀ�ļ���
        public const string Attribute_TopLevel = "toplevel";					//����ҳ���µ���Ŀ����
        public const string Attribute_Type = "type";							//ָ���洢ͼƬ���ֶ�
        public const string Attribute_IsOriginal = "isoriginal";            //������������ݣ��Ƿ��ȡ���������ݵ�ֵ
		public const string Attribute_Src = "src";								//��ʾ��ͼƬ��ַ
        public const string Attribute_AltSrc = "altsrc";						//��ָ����ͼƬ������ʱ��ʾ��ͼƬ��ַ
        public const string Attribute_Width = "width";							//���
        public const string Attribute_Height = "height";						//�߶�
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_ChannelIndex, "��Ŀ����");
				attributes.Add(Attribute_ChannelName, "��Ŀ����");
                attributes.Add(Attribute_NO, "��ʾ�ֶε�˳��");
				attributes.Add(Attribute_Parent, "��ʾ����Ŀ");
				attributes.Add(Attribute_UpLevel, "�ϼ���Ŀ�ļ���");
                attributes.Add(Attribute_TopLevel, "����ҳ���µ���Ŀ����");
                attributes.Add(Attribute_Type, "ָ���洢ͼƬ���ֶ�");
                attributes.Add(Attribute_IsOriginal, "������������ݣ��Ƿ��ȡ���������ݵ�ֵ");
                attributes.Add(Attribute_Src, "��ʾ��ͼƬ��ַ");
                attributes.Add(Attribute_AltSrc, "��ָ����ͼƬ������ʱ��ʾ��ͼƬ��ַ");
                attributes.Add(Attribute_Width, "���");
                attributes.Add(Attribute_Height, "�߶�");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
				return attributes;
			}
		}


		//�ԡ���Ŀ���ӡ���stl:image��Ԫ�ؽ��н���
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
			try
			{
				HtmlImage stlImage = new HtmlImage();
				IEnumerator ie = node.Attributes.GetEnumerator();
				bool isGetPicUrlFromAttribute = false;
				string channelIndex = string.Empty;
				string channelName = string.Empty;
                int no = 0;
				int upLevel = 0;
                int topLevel = -1;
                string type = BackgroundContentAttribute.ImageUrl;
                bool isOriginal = false;
                string src = string.Empty;
				string altSrc = string.Empty;
                bool isDynamic = false;

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(StlImage.Attribute_ChannelIndex))
					{
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
						if (!string.IsNullOrEmpty(channelIndex))
						{
							isGetPicUrlFromAttribute = true;
						}
					}
					else if (attributeName.Equals(StlImage.Attribute_ChannelName))
					{
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
						if (!string.IsNullOrEmpty(channelName))
						{
							isGetPicUrlFromAttribute = true;
						}
                    }
                    else if (attributeName.Equals(StlImage.Attribute_NO))
                    {
                        no = TranslateUtils.ToInt(attr.Value);
                    }
					else if (attributeName.Equals(StlImage.Attribute_Parent))
					{
						if (TranslateUtils.ToBool(attr.Value))
						{
							upLevel = 1;
							isGetPicUrlFromAttribute = true;
						}
					}
					else if (attributeName.Equals(StlImage.Attribute_UpLevel))
					{
						upLevel = TranslateUtils.ToInt(attr.Value);
						if (upLevel > 0)
						{
							isGetPicUrlFromAttribute = true;
						}
                    }
                    else if (attributeName.Equals(StlImage.Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                        if (topLevel >= 0)
                        {
                            isGetPicUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlImage.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(StlImage.Attribute_IsOriginal))
                    {
                        isOriginal = TranslateUtils.ToBool(attr.Value, true);
                    }
					else if (attributeName.Equals(StlImage.Attribute_Src))
					{
                        src = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlImage.Attribute_AltSrc))
                    {
                        altSrc = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlImage.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
					else
					{
						stlImage.Attributes.Remove(attributeName);
						stlImage.Attributes.Add(attributeName, attr.Value);
					}
				}

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(stlElement, node, pageInfo, contextInfo, stlImage, isGetPicUrlFromAttribute, channelIndex, channelName, no, upLevel, topLevel, type, isOriginal, src, altSrc);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, HtmlImage stlImage, bool isGetPicUrlFromAttribute, string channelIndex, string channelName, int no, int upLevel, int topLevel, string type, bool isOriginal, string src, string altSrc)
        {
            string parsedContent = string.Empty;

            int contentID = 0;
            //�ж��Ƿ�ͼƬ��ַ�ɱ�ǩ���Ի��
            if (!isGetPicUrlFromAttribute)
            {
                contentID = contextInfo.ContentID;
            }
            EContextType contextType = contextInfo.ContextType;

            string picUrl = string.Empty;
            if (!string.IsNullOrEmpty(src))
            {
                picUrl = src;
            }
            else
            {
                if (contextType == EContextType.Undefined)
                {
                    if (contentID != 0)
                    {
                        contextType = EContextType.Content;
                    }
                    else
                    {
                        contextType = EContextType.Channel;
                    }
                }

                if (contextType == EContextType.Content)//��ȡ����ͼƬ
                {
                    ContentInfo contentInfo = contextInfo.ContentInfo;

                    if (isOriginal)
                    {
                        if (contentInfo != null && contentInfo.ReferenceID > 0 && contentInfo.SourceID > 0)
                        {
                            int targetNodeID = contentInfo.SourceID;
                            int targetPublishmentSystemID = DataProvider.NodeDAO.GetPublishmentSystemID(targetNodeID);
                            PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);
                            NodeInfo targetNodeInfo = NodeManager.GetNodeInfo(targetPublishmentSystemID, targetNodeID);

                            ETableStyle tableStyle = NodeManager.GetTableStyle(targetPublishmentSystemInfo, targetNodeInfo);
                            string tableName = NodeManager.GetTableName(targetPublishmentSystemInfo, targetNodeInfo);
                            ContentInfo targetContentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentInfo.ReferenceID);
                            if (targetContentInfo != null || targetContentInfo.NodeID > 0)
                            {
                                contentInfo = targetContentInfo;
                            }
                        }
                    }

                    if (contentInfo == null)
                    {
                        contentInfo = DataProvider.ContentDAO.GetContentInfo(ETableStyle.BackgroundContent, pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentID);
                    }

                    if (contentInfo != null)
                    {
                        if (no <= 1)
                        {
                            picUrl = contentInfo.GetExtendedAttribute(type);
                        }
                        else
                        {
                            string extendAttributeName = ContentAttribute.GetExtendAttributeName(type);
                            string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                            if (!string.IsNullOrEmpty(extendValues))
                            {
                                int index = 2;
                                foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                                {
                                    if (index == no)
                                    {
                                        picUrl = extendValue;
                                        break;
                                    }
                                    index++;
                                }
                            }
                        }
                    }
                }
                else if (contextType == EContextType.Channel)//��ȡ��ĿͼƬ
                {
                    int channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);

                    channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);

                    NodeInfo channel = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, channelID);

                    picUrl = channel.ImageUrl;
                }
                else if (contextType == EContextType.Each)
                {
                    picUrl = contextInfo.ItemContainer.EachItem.DataItem as string;
                }
            }

            if (string.IsNullOrEmpty(picUrl))
            {
                picUrl = altSrc;
            }

            if (!string.IsNullOrEmpty(picUrl))
            {
                string extension = PathUtils.GetExtension(picUrl);
                if (EFileSystemTypeUtils.IsFlash(extension))
                {
                    parsedContent = StlFlash.Parse(stlElement, node, pageInfo, contextInfo);
                }
                else if (EFileSystemTypeUtils.IsPlayer(extension))
                {
                    parsedContent = StlPlayer.Parse(stlElement, node, pageInfo, contextInfo);
                }
                else
                {
                    stlImage.Src = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, picUrl);
                    parsedContent = ControlUtils.GetControlRenderHtml(stlImage);
                }
            }

            return parsedContent;
        }
	}
}
