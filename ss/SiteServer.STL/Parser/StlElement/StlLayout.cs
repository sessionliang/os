using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using System.Collections.Generic;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlLayout
    {
        private StlLayout() { }
        public const string ElementName = "stl:layout";                     //布局

        public const string Attribute_Cols = "cols";                        //各列宽度
        public const string Attribute_Margin_Top = "margintop";             //上边距
        public const string Attribute_Margin_Bottom = "marginbottom";       //下边距
        public const string Attribute_Context = "context";                  //所处上下文

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Cols, "各列宽度");
                attributes.Add(Attribute_Margin_Top, "上边距");
                attributes.Add(Attribute_Margin_Bottom, "下边距");
                attributes.Add(Attribute_Context, "所处上下文");
                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
        {
            string parsedContent = string.Empty;
            string cols = string.Empty;
            int marginTop = 5;
            int marginBottom = 5;
            ContextInfo contextInfo = contextInfoRef.Clone();

            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlLayout.Attribute_Cols))
                    {
                        cols = attr.Value;
                    }
                    else if (attributeName.Equals(StlLayout.Attribute_Margin_Top))
                    {
                        marginTop = TranslateUtils.ToInt(attr.Value, 5);
                    }
                    else if (attributeName.Equals(StlLayout.Attribute_Margin_Bottom))
                    {
                        marginBottom = TranslateUtils.ToInt(attr.Value, 5);
                    }
                    else if (attributeName.Equals(StlLayout.Attribute_Context))
                    {
                        contextInfo.ContextType = EContextTypeUtils.GetEnumType(attr.Value);
                    }
                }

                if (!string.IsNullOrEmpty(node.InnerXml))
                {
                    string innerHtml = RegexUtils.GetInnerContent(StlLayout.ElementName, stlElement);

                    List<string> containerList = RegexUtils.GetTagContents(StlContainer.ElementName, innerHtml);

                    List<string> widthList = new List<string>();
                    for (int i = 0; i < containerList.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(cols) && cols.Split(',').Length == i + 1)
                        {
                            string width = cols.Split(',')[i];
                            if (width == "*")
                            {
                                width = string.Empty;
                            }
                            widthList.Add(width);
                        }
                        else
                        {
                            widthList.Add(string.Empty);
                        }
                    }

                    StringBuilder builder = new StringBuilder();

                    string style = string.Empty;
                    if (marginTop > 0)
                    {
                        style += string.Format("margin-top:{0}px;", marginTop);
                    }
                    if (marginBottom > 0)
                    {
                        style += string.Format("margin-bottom:{0}px;", marginTop);
                    }
                    if (!string.IsNullOrEmpty(style))
                    {
                        style = string.Format(@"style=""{0}""", style);
                    }
                    builder.AppendFormat(@"<table width=""100%"" cellspacing=""0"" cellpadding=""0"" {0}><tr>", style);

                    for (int i = 0; i < widthList.Count; i++)
                    {
                        string container = containerList[i] as string;
                        string width = widthList[i] as string;
                        if (!string.IsNullOrEmpty(width))
                        {
                            width = string.Format(@" width=""{0}""", width);
                        }
                        builder.AppendFormat(@"<td{0} valign=""top"">{1}</td>", width, container);
                    }

                    builder.Append(@"</tr></table>");

                    StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                    parsedContent = builder.ToString();
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }
    }
}
