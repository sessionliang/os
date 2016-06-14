using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using System;
using SiteServer.BBS.Core.TemplateParser.Model;
using System.Collections.Generic;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Core.TemplateParser.Element
{
    public class Value
	{
        private Value() { }
		public const string ElementName = "bbs:value";//获取值

		public const string Attribute_Type = "type";		//需要获取值的类型
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public const string Type_FaceLinks = "FaceLinks";
        public const string Type_FaceDefaultContents = "FaceDefaultContents";

        public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_Type, "需要获取值的类型");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
				return attributes;
			}
		}

        public static string Parse(string element, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
			try
			{
				IEnumerator ie = node.Attributes.GetEnumerator();

				string type = string.Empty;
                bool isDynamic = false;

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(Value.Attribute_Type))
					{
						type = attr.Value;
                    }
                    else if (attributeName.Equals(Value.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
				}

                if (isDynamic)
                {
                    parsedContent = Dynamic.ParseDynamicElement(ElementName, element, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, type);
                }
			}
            catch (Exception ex)
            {
                parsedContent = ParserUtility.GetErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string type)
        {
            string parsedContent = string.Empty;

            if (type.Length > 0)
            {
                if (type.ToLower().Equals(Value.Type_FaceLinks.ToLower()))
                {
                    parsedContent = StringUtilityBBS.GetFaceLinks(pageInfo.PublishmentSystemID);
                }
                else if (type.ToLower().Equals(Value.Type_FaceDefaultContents.ToLower()))
                {
                    parsedContent = StringUtilityBBS.GetFaceDefaultContents(pageInfo.PublishmentSystemID);
                }
            }

            return parsedContent;
        }
	}
}
