using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System.Text.RegularExpressions;
using System;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;
using SiteServer.CMS.Controls;
using System.Collections.Generic;

namespace SiteServer.STL.Parser.StlElement
{

    public class StlSubscribe
    {
        private StlSubscribe() { }
        public const string ElementName = "stl:subscribe";

        public const string SimpleElementName = "stl:sub";

        public const string Attribute_Subscribename = "subscribename";		        //提交表单名称
        public const string Attribute_SubscribeStyle = "subscribeStyle";		        //订阅内容展示方式select/checkbox

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Subscribename, "提交表单名称");
                attributes.Add(Attribute_SubscribeStyle, "订阅内容展示方式:SelectOne/CheckBox");
                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                string subscribeName = "Default";
                EInputType subscribeStyle = EInputType.SelectOne;

                string subscribeTemplateString = string.Empty;
                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;
                StlParserUtility.GetInnerTemplateStringOfSubscribe(node, out subscribeTemplateString, out successTemplateString, out failureTemplateString, pageInfo, contextInfo);

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlSubscribe.Attribute_Subscribename))
                    {
                        subscribeName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlSubscribe.Attribute_SubscribeStyle))
                    {
                        subscribeStyle = EInputTypeUtils.GetEnumType(attr.Value);
                    }
                }

                parsedContent = ParseImpl(pageInfo, contextInfo, subscribeName, subscribeStyle, subscribeTemplateString, successTemplateString, failureTemplateString);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string subscribeName, EInputType subscribeStyle, string subscribeTemplateString, string successTemplateString, string failureTemplateString)
        {
            string parsedContent = string.Empty;

            SubscribeSetInfo subscribeSetInfo = DataProvider.SubscribeSetDAO.GetSubscribeSetInfo(pageInfo.PublishmentSystemID);
            SiteServer.STL.StlTemplate.SubscribeTemplate subscribeTemplate = new SiteServer.STL.StlTemplate.SubscribeTemplate(pageInfo.PublishmentSystemInfo, subscribeSetInfo, subscribeStyle);
            parsedContent += subscribeTemplate.GetTemplate(subscribeSetInfo.IsTemplate, subscribeTemplateString, subscribeStyle);

            StringBuilder innerBuilder = new StringBuilder(parsedContent);

            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
            parsedContent = innerBuilder.ToString();
            return parsedContent;
        }

        private static string ParseTemplateToHtml(PageInfo pageInfo, ContextInfo contextInfo, string parsedContent, EInputType subscribeStyle, string subscribeTemplateString, string successTemplateString, string failureTemplateString, string fromID)
        {
            StringBuilder builder = new StringBuilder();

            StringBuilder innerBuilder = new StringBuilder();


            return builder.ToString();
        }
    }
}
