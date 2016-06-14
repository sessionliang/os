using BaiRong.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.STL.Parser.TemplateDesign
{
    public class TemplateDesignUtility
    {
        public const string IMG_SRC_PLACEHOLDER = "IMG_SRC_PLACEHOLDER";
        public static string AddClassAndAttribute(string html, string clazz, string attributeName, string attributeValue, bool isImageOnly)
        {
            string retval = html;

            try
            {
                StringBuilder parsedBuilder = new StringBuilder(html);

                List<string> editableElementList = new List<string>();
                if (isImageOnly)
                {
                    List<string> imgElementList = RegexUtils.GetTagContents("img", html);
                    foreach (string imgElement in imgElementList)
                    {
                        if (imgElement.IndexOf("{") == -1 && imgElement.IndexOf("}") == -1)
                        {
                            editableElementList.Add(imgElement);
                        }
                    }
                }
                else
                {
                    editableElementList = StlParserUtility.GetEditableElementList(html);
                }

                foreach (string editableElement in editableElementList)
                {
                    try
                    {
                        int startIndex = parsedBuilder.ToString().IndexOf(editableElement);
                        if (startIndex != -1)
                        {
                            string parsedContent = editableElement;

                            string seperator = " ";
                            if (parsedContent.IndexOf(seperator) == -1)
                            {
                                seperator = "/>";
                                if (parsedContent.IndexOf(seperator) == -1)
                                {
                                    seperator = ">";
                                }
                            }

                            if (!string.IsNullOrEmpty(clazz))
                            {
                                string originalClass = RegexUtils.GetAttributeContent("class", editableElement);
                                string newClass = clazz;

                                if (!string.IsNullOrEmpty(originalClass))
                                {
                                    newClass = originalClass + " " + clazz;
                                    parsedContent = StringUtils.ReplaceFirst(originalClass, parsedContent, newClass);
                                }
                                else
                                {
                                    parsedContent = StringUtils.ReplaceFirst(seperator, parsedContent, string.Format(@" class=""{0}""{1}", newClass, seperator));
                                }
                            }
                            if (!string.IsNullOrEmpty(attributeName) && !string.IsNullOrEmpty(attributeValue))
                            {
                                string value = attributeValue;
                                if (isImageOnly)
                                {
                                    string src = RegexUtils.GetAttributeContent("src", editableElement);
                                    value = attributeValue.Replace(TemplateDesignUtility.IMG_SRC_PLACEHOLDER, src);
                                }

                                parsedContent = StringUtils.ReplaceFirst(seperator, parsedContent, string.Format(@" {0}=""{1}""{2}", attributeName, value, seperator));
                            }

                            parsedBuilder.Replace(editableElement, parsedContent, startIndex, editableElement.Length);
                        }
                    }
                    catch { }
                }

                retval = parsedBuilder.ToString();
            }
            catch { }

            return retval;

            //try
            //{
            //    XmlDocument xmlDocument = null;
            //    if (StringUtils.EqualsIgnoreCase(stlElementName, StlInclude.ElementName))
            //    {
            //        xmlDocument = StlParserUtility.GetXmlDocument(string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?><root>{0}</root>", html.Trim()), true);
            //    }
            //    else
            //    {
            //        xmlDocument = StlParserUtility.GetXmlDocument(string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?><root>{0}</root>", html.Trim()), false);
            //    }
            //    XmlNode node = xmlDocument.DocumentElement;

            //    foreach (XmlNode xmlNode in node.FirstChild.ChildNodes)
            //    {
            //        if (xmlNode.NodeType == XmlNodeType.Element)
            //        {
            //            XmlElement element = (XmlElement)xmlNode;
            //            if (!StringUtils.EqualsIgnoreCase("script", element.Name))
            //            {
            //                if (!string.IsNullOrEmpty(clazz))
            //                {
            //                    if (element.Attributes["class"] != null)
            //                    {
            //                        element.SetAttribute("class", element.Attributes["class"].Value + " " + clazz);
            //                    }
            //                    else
            //                    {
            //                        element.SetAttribute("class", clazz);
            //                    }
            //                }

            //                element.SetAttribute(attributeName, attributeValue);
            //            }
            //        }
            //    }

            //    retval = node.InnerXml;
            //    retval = retval.Replace(@"<root xmlns=""http://www.siteserver.cn/stl"">", string.Empty);
            //    retval = retval.Replace("</root>", string.Empty);
            //}
            //catch { }

            //return retval;
        }
    }
}
