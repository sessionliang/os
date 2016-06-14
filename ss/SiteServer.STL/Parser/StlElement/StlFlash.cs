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
		public const string ElementName = "stl:flash";//显示Flash

		public const string Attribute_ChannelIndex = "channelindex";			//栏目索引
		public const string Attribute_ChannelName = "channelname";				//栏目名称
		public const string Attribute_Parent = "parent";						//显示父栏目
		public const string Attribute_UpLevel = "uplevel";						//上级栏目的级别
        public const string Attribute_TopLevel = "toplevel";					//从首页向下的栏目级别
        public const string Attribute_Type = "type";							//指定存储flash的字段
		public const string Attribute_Src = "src";								//显示的flash地址
        public const string Attribute_AltSrc = "altsrc";						//当指定的flash不存在时显示的flash地址

		public const string Attribute_Width = "width";							//宽度
		public const string Attribute_Height = "height";						//高度
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_ChannelIndex, "栏目索引");
				attributes.Add(Attribute_ChannelName, "栏目名称");
				attributes.Add(Attribute_Parent, "显示父栏目");
				attributes.Add(Attribute_UpLevel, "上级栏目的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的栏目级别");
                attributes.Add(Attribute_Type, "指定存储flash的字段");
                attributes.Add(Attribute_Src, "显示的flash地址");
                attributes.Add(Attribute_AltSrc, "当指定的flash不存在时显示的flash地址");
				attributes.Add(Attribute_Width, "宽度");
				attributes.Add(Attribute_Height, "高度");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
				return attributes;
			}
		}


		//对“栏目链接”（stl:flash）元素进行解析
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
            //判断是否图片地址由标签属性获得
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
                if (contentID != 0)//获取内容Flash
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
                else//获取栏目Flash
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
