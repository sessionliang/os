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
	public class StlFlash
	{
		private StlFlash(){}
		public const string ElementName = "stl:flash";//��ʾFlash

		public const string Attribute_ChannelIndex = "channelindex";			//��Ŀ����
		public const string Attribute_ChannelName = "channelname";				//��Ŀ����
		public const string Attribute_Parent = "parent";						//��ʾ����Ŀ
		public const string Attribute_UpLevel = "uplevel";						//�ϼ���Ŀ�ļ���
        public const string Attribute_TopLevel = "toplevel";					//����ҳ���µ���Ŀ����
        public const string Attribute_Type = "type";							//ָ���洢flash���ֶ�
		public const string Attribute_Src = "src";								//��ʾ��flash��ַ
        public const string Attribute_AltSrc = "altsrc";						//��ָ����flash������ʱ��ʾ��flash��ַ

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
				attributes.Add(Attribute_Parent, "��ʾ����Ŀ");
				attributes.Add(Attribute_UpLevel, "�ϼ���Ŀ�ļ���");
                attributes.Add(Attribute_TopLevel, "����ҳ���µ���Ŀ����");
                attributes.Add(Attribute_Type, "ָ���洢flash���ֶ�");
                attributes.Add(Attribute_Src, "��ʾ��flash��ַ");
                attributes.Add(Attribute_AltSrc, "��ָ����flash������ʱ��ʾ��flash��ַ");
				attributes.Add(Attribute_Width, "���");
				attributes.Add(Attribute_Height, "�߶�");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
				return attributes;
			}
		}


		//�ԡ���Ŀ���ӡ���stl:flash��Ԫ�ؽ��н���
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
			try
			{
				IEnumerator ie = node.Attributes.GetEnumerator();
				bool isGetPicUrlFromAttribute = false;
				string channelIndex = string.Empty;
				string channelName = string.Empty;
				int upLevel = 0;
                int topLevel = -1;
                string type = BackgroundContentAttribute.ImageUrl;
				string src = string.Empty;
                string altSrc = string.Empty;
				string width = "100%";
                string height = "180";
                bool isDynamic = false;
                NameValueCollection parameters = new NameValueCollection();

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(StlFlash.Attribute_ChannelIndex))
					{
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
						if (!string.IsNullOrEmpty(channelIndex))
						{
							isGetPicUrlFromAttribute = true;
						}
					}
					else if (attributeName.Equals(StlFlash.Attribute_ChannelName))
					{
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
						if (!string.IsNullOrEmpty(channelName))
						{
							isGetPicUrlFromAttribute = true;
						}
					}
					else if (attributeName.Equals(StlFlash.Attribute_Parent))
					{					
						if (TranslateUtils.ToBool(attr.Value))
						{
							upLevel = 1;
							isGetPicUrlFromAttribute = true;
						}
					}
					else if (attributeName.Equals(StlFlash.Attribute_UpLevel))
					{
						upLevel = TranslateUtils.ToInt(attr.Value);
						if (upLevel > 0)
						{
							isGetPicUrlFromAttribute = true;
						}
                    }
                    else if (attributeName.Equals(StlFlash.Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                        if (topLevel >= 0)
                        {
                            isGetPicUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlFlash.Attribute_Type))
                    {
                        type = attr.Value;
                    }
					else if (attributeName.Equals(StlFlash.Attribute_Src))
					{
						src = attr.Value;
                    }
                    else if (attributeName.Equals(StlFlash.Attribute_AltSrc))
                    {
                        altSrc = attr.Value;
                    }
					else if (attributeName.Equals(StlFlash.Attribute_Width))
					{
                        width = attr.Value;
					}
					else if (attributeName.Equals(StlFlash.Attribute_Height))
					{
                        height = attr.Value;
                    }
                    else if (attributeName.Equals(StlFlash.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value, false);
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
                    parsedContent = ParseImpl(stlElement, node, pageInfo, contextInfo, isGetPicUrlFromAttribute, channelIndex, channelName, upLevel, topLevel, type, src, altSrc, width, height, parameters);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, bool isGetPicUrlFromAttribute, string channelIndex, string channelName, int upLevel, int topLevel, string type, string src, string altSrc, string width, string height, NameValueCollection parameters)
        {
            string parsedContent = string.Empty;

            int contentID = 0;
            //�ж��Ƿ�ͼƬ��ַ�ɱ�ǩ���Ի��
            if (!isGetPicUrlFromAttribute)
            {
                contentID = contextInfo.ContentID;
            }
            ContentInfo contentInfo = contextInfo.ContentInfo;

            string picUrl;
            if (!string.IsNullOrEmpty(src))
            {
                picUrl = src;
            }
            else
            {
                if (contentID != 0)//��ȡ����Flash
                {
                    if (contentInfo == null)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(contextInfo.PublishmentSystemInfo.PublishmentSystemID, contextInfo.ChannelID);
                        string tableName = NodeManager.GetTableName(contextInfo.PublishmentSystemInfo, nodeInfo);

                        picUrl = BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, type);
                    }
                    else
                    {
                        picUrl = contextInfo.ContentInfo.GetExtendedAttribute(type);
                    }
                }
                else//��ȡ��ĿFlash
                {
                    int channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);

                    channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);
                    NodeInfo channel = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, channelID);

                    picUrl = channel.ImageUrl;
                }
            }

            if (string.IsNullOrEmpty(picUrl))
            {
                picUrl = altSrc;
            }

            if (!string.IsNullOrEmpty(picUrl))
            {
                string extension = PathUtils.GetExtension(picUrl);
                if (EFileSystemTypeUtils.IsImage(extension))
                {
                    parsedContent = StlImage.Parse(stlElement, node, pageInfo, contextInfo);
                }
                else if (EFileSystemTypeUtils.IsPlayer(extension))
                {
                    parsedContent = StlPlayer.Parse(stlElement, node, pageInfo, contextInfo);
                }
                else
                {
                    pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ac_SWFObject);

                    picUrl = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, picUrl);

                    if (string.IsNullOrEmpty(parameters["quality"]))
                    {
                        parameters["quality"] = "high";
                    }
                    if (string.IsNullOrEmpty(parameters["wmode"]))
                    {
                        parameters["wmode"] = "transparent";
                    }
                    StringBuilder paramBuilder = new StringBuilder();
                    int uniqueID = pageInfo.UniqueID;
                    foreach (string key in parameters.Keys)
                    {
                        paramBuilder.AppendFormat(@"    so_{0}.addParam(""{1}"", ""{2}"");", uniqueID, key, parameters[key]).Append(StringUtils.Constants.ReturnAndNewline);
                    }

                    parsedContent = string.Format(@"
<div id=""flashcontent_{0}""></div>
<script type=""text/javascript"">
    // <![CDATA[
    var so_{0} = new SWFObject(""{1}"", ""flash_{0}"", ""{2}"", ""{3}"", ""7"", """");
{4}
    so_{0}.write(""flashcontent_{0}"");
    // ]]>
</script>
", uniqueID, picUrl, width, height, paramBuilder);
                }
            }

            return parsedContent;
        }
	}
}
