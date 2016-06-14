using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlLocation
	{
		private StlLocation(){}
		public const string ElementName = "stl:location";//显示位置

		public const string Attribute_Separator = "separator";				//当前位置分隔符
		public const string Attribute_Target = "target";					//打开窗口的目标
		public const string Attribute_LinkClass = "linkclass";				//链接CSS样式
        public const string Attribute_WordNum = "wordnum";                  //链接字数
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_Separator, "当前位置分隔符");
				attributes.Add(Attribute_Target, "打开窗口的目标");
				attributes.Add(Attribute_LinkClass, "链接CSS样式");
                attributes.Add(Attribute_WordNum, "链接字数");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
				return attributes;
			}
		}


		//对“当前位置”（stl:location）元素进行解析
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
			try
			{
				IEnumerator ie = node.Attributes.GetEnumerator();
				string separator = " - ";
				string target = string.Empty;
				string linkClass = string.Empty;
                int wordNum = 0;
                bool isDynamic = false;

				StringDictionary attributes = new StringDictionary();

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(Attribute_Separator))
					{
						separator = attr.Value;
					}
					else if (attributeName.Equals(Attribute_Target))
					{
						target = attr.Value;
					}
					else if (attributeName.Equals(Attribute_LinkClass))
					{
						linkClass = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
					else
					{
						attributes.Add(attributeName, attr.Value);
					}
				}

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, separator, target, linkClass, wordNum, attributes);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }
			
			return parsedContent;
		}

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, string separator, string target, string linkClass, int wordNum, StringDictionary attributes)
        {
            if (!string.IsNullOrEmpty(node.InnerXml))
            {
                separator = node.InnerXml;
            }

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID);

            StringBuilder builder = new StringBuilder();

            string parentsPath = nodeInfo.ParentsPath;
            int parentsCount = nodeInfo.ParentsCount;
            if (parentsPath.Length != 0)
            {
                string nodePath = parentsPath + "," + contextInfo.ChannelID;
                ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToArrayList(nodePath);
                foreach (string nodeIDStr in nodeIDArrayList)
                {
                    int currentID = int.Parse(nodeIDStr);
                    NodeInfo currentNodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, currentID);
                    if (currentID == pageInfo.PublishmentSystemID)
                    {
                        HtmlAnchor stlAnchor = new HtmlAnchor();
                        if (!string.IsNullOrEmpty(target))
                        {
                            stlAnchor.Target = target;
                        }
                        if (!string.IsNullOrEmpty(linkClass))
                        {
                            stlAnchor.Attributes.Add("class", linkClass);
                        }
                        string url = PageUtility.GetIndexPageUrl(pageInfo.PublishmentSystemInfo, pageInfo.VisualType);
                        if (url.Equals(PageUtils.UNCLICKED_URL))
                        {
                            stlAnchor.Target = string.Empty;
                        }
                        stlAnchor.HRef = url;
                        stlAnchor.InnerHtml = StringUtils.MaxLengthText(currentNodeInfo.NodeName, wordNum);

                        ControlUtils.AddAttributesIfNotExists(stlAnchor, attributes);

                        builder.Append(ControlUtils.GetControlRenderHtml(stlAnchor));

                        if (parentsCount > 0)
                        {
                            builder.Append(separator);
                        }
                    }
                    else if (currentID == contextInfo.ChannelID)
                    {
                        HtmlAnchor stlAnchor = new HtmlAnchor();
                        if (!string.IsNullOrEmpty(target))
                        {
                            stlAnchor.Target = target;
                        }
                        if (!string.IsNullOrEmpty(linkClass))
                        {
                            stlAnchor.Attributes.Add("class", linkClass);
                        }
                        string url = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, currentNodeInfo, pageInfo.VisualType);
                        if (url.Equals(PageUtils.UNCLICKED_URL))
                        {
                            stlAnchor.Target = string.Empty;
                        }
                        stlAnchor.HRef = url;
                        stlAnchor.InnerHtml = StringUtils.MaxLengthText(currentNodeInfo.NodeName, wordNum);

                        ControlUtils.AddAttributesIfNotExists(stlAnchor, attributes);

                        builder.Append(ControlUtils.GetControlRenderHtml(stlAnchor));
                    }
                    else
                    {
                        HtmlAnchor stlAnchor = new HtmlAnchor();
                        if (!string.IsNullOrEmpty(target))
                        {
                            stlAnchor.Target = target;
                        }
                        if (!string.IsNullOrEmpty(linkClass))
                        {
                            stlAnchor.Attributes.Add("class", linkClass);
                        }
                        string url = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, currentNodeInfo, pageInfo.VisualType);
                        if (url.Equals(PageUtils.UNCLICKED_URL))
                        {
                            stlAnchor.Target = string.Empty;
                        }
                        stlAnchor.HRef = url;
                        stlAnchor.InnerHtml = StringUtils.MaxLengthText(currentNodeInfo.NodeName, wordNum);

                        ControlUtils.AddAttributesIfNotExists(stlAnchor, attributes);

                        builder.Append(ControlUtils.GetControlRenderHtml(stlAnchor));

                        if (parentsCount > 0)
                        {
                            builder.Append(separator);
                        }
                    }
                }
            }

            return builder.ToString();
        }
	}
}
