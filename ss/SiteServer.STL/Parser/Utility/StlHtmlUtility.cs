using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Text.Sgml;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser.StlEntity;
using SiteServer.CMS.Model;
using System;

namespace SiteServer.STL.Parser
{
	public class StlHtmlUtility
	{
        public static string GetStlSubmitButtonElement(string inputTemplate)
        {
            RegexOptions options = ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline) | RegexOptions.IgnoreCase);

            string regex = "<input\\s*[^>]*?id\\s*=\\s*(\"submit\"|\'submit\'|submit).*?>";
            Regex reg = new Regex(regex, options);
            Match match = reg.Match(inputTemplate);
            if (match.Success)
            {
                return match.Value;
            }

            regex = "<\\w+\\s*[^>]*?id\\s*=\\s*(\"submit\"|\'submit\'|submit)[^>]*/\\s*>";
            reg = new Regex(regex, options);
            match = reg.Match(inputTemplate);
            if (match.Success)
            {
                return match.Value;
            }

            regex = "<(\\w+?)\\s*[^>]*?id\\s*=\\s*(\"submit\"|\'submit\'|submit).*?>[^>]*</\\1[^>]*>";
            reg = new Regex(regex, options);
            match = reg.Match(inputTemplate);
            if (match.Success)
            {
                return match.Value;
            }

            return string.Empty;
        }

        public static ArrayList GetStlFormElementsArrayList(string content)
        {
            ArrayList arraylist = new ArrayList();

            RegexOptions options = ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline) | RegexOptions.IgnoreCase);

            string regex = "<input\\s*[^>]*?/>|<input\\s*[^>]*?></input>";
            Regex reg = new Regex(regex, options);
            MatchCollection mc = reg.Matches(content);
            for (int i = 0; i < mc.Count; i++)
            {
                string element = mc[i].Value;
                arraylist.Add(element);
            }

            regex = "<textarea\\s*[^>]*?/>|<textarea\\s*[^>]*?></textarea>";
            reg = new Regex(regex, options);
            mc = reg.Matches(content);
            for (int i = 0; i < mc.Count; i++)
            {
                string element = mc[i].Value;
                arraylist.Add(element);
            }

            regex = "<select\\s*[^>]*?>.*?</select>";
            reg = new Regex(regex, options);
            mc = reg.Matches(content);
            for (int i = 0; i < mc.Count; i++)
            {
                string element = mc[i].Value;
                arraylist.Add(element);
            }

            return arraylist;
        }

        public static string GetStlInputElementByID(string inputTemplate, string id)
        {
            RegexOptions options = ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline) | RegexOptions.IgnoreCase);

            string regex = string.Format("<input\\s*[^>]*?type\\s*=\\s*(\"{0}\"|\'{0}\'|{0}).*?>", id);
            Regex reg = new Regex(regex, options);
            Match match = reg.Match(inputTemplate);
            if (match.Success)
            {
                return match.Value;
            }

            regex = string.Format("<\\w+\\s*[^>]*?id\\s*=\\s*(\"{0}\"|\'{0}\'|{0})[^>]*/\\s*>", id);
            reg = new Regex(regex, options);
            match = reg.Match(inputTemplate);
            if (match.Success)
            {
                return match.Value;
            }

            regex = string.Format("<(\\w+?)\\s*[^>]*?id\\s*=\\s*(\"{0}\"|\'{0}\'|{0}).*?>[^>]*</\\1[^>]*>", id);
            reg = new Regex(regex, options);
            match = reg.Match(inputTemplate);
            if (match.Success)
            {
                return match.Value;
            }

            return string.Empty;
        }

        public static void ReWriteFormElements(string stlFormElement, out XmlNode elementNode, out NameValueCollection attributes)
        {
            XmlDocument document = StlParserUtility.GetXmlDocument(stlFormElement, false);
            elementNode = document.DocumentElement;
            elementNode = elementNode.FirstChild;
            IEnumerator elementIE = elementNode.Attributes.GetEnumerator();
            attributes = new NameValueCollection();
            while (elementIE.MoveNext())
            {
                XmlAttribute attr = (XmlAttribute)elementIE.Current;
                string attributeName = attr.Name;
                if (StringUtils.EqualsIgnoreCase(attributeName, "id") || StringUtils.EqualsIgnoreCase(attributeName, "name"))
                {
                    attributeName = attributeName.ToLower();
                }
                attributes.Add(attributeName, attr.Value);
            }
            //if (!attributes.ContainsKey("name") && !string.IsNullOrEmpty(attributes["id"]))
            //{
            //    attributes.Add("name", attributes["id"]);
            //}
            if (string.IsNullOrEmpty(attributes["name"]) && !string.IsNullOrEmpty(attributes["id"]))
            {
                attributes["name"] = attributes["id"];
            }
        }

        public static void ReWriteSubmitButton(StringBuilder builder, string clickString)
        {
            string submitElement = StlHtmlUtility.GetStlSubmitButtonElement(builder.ToString());
            if (!string.IsNullOrEmpty(submitElement))
            {
                XmlDocument document = StlParserUtility.GetXmlDocument(submitElement, false);
                XmlNode elementNode = document.DocumentElement;
                elementNode = elementNode.FirstChild;
                IEnumerator elementIE = elementNode.Attributes.GetEnumerator();
                StringDictionary attributes = new StringDictionary();
                while (elementIE.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)elementIE.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName == "href")
                    {
                        attributes.Add(attr.Name, PageUtils.UNCLICKED_URL);
                    }
                    else if (attributeName != "onclick")
                    {
                        attributes.Add(attr.Name, attr.Value);
                    }
                }
                attributes.Add("onclick", clickString);
                attributes.Remove("id");
                attributes.Remove("name");

                //attributes.Add("id", "submit_" + styleID);

                if (StringUtils.EqualsIgnoreCase(elementNode.Name, "a"))
                {
                    attributes.Remove("href");
                    attributes.Add("href", PageUtils.UNCLICKED_URL);
                }

                if (!string.IsNullOrEmpty(elementNode.InnerXml))
                {
                    builder.Replace(submitElement, string.Format(@"<{0} {1}>{2}</{0}>", elementNode.Name, TranslateUtils.ToAttributesString(attributes), elementNode.InnerXml));
                }
                else
                {
                    builder.Replace(submitElement, string.Format(@"<{0} {1}/>", elementNode.Name, TranslateUtils.ToAttributesString(attributes)));
                }
            }
        }

        public static string ReWriteOnClick(string originalHtml, string id, string clickString)
        {
            string rewriteHtml = originalHtml;
            string submitElement = StlHtmlUtility.GetStlInputElementByID(rewriteHtml, id);
            if (!string.IsNullOrEmpty(submitElement))
            {
                XmlDocument document = StlParserUtility.GetXmlDocument(submitElement, false);
                XmlNode elementNode = document.DocumentElement;
                elementNode = elementNode.FirstChild;
                IEnumerator elementIE = elementNode.Attributes.GetEnumerator();
                StringDictionary attributes = new StringDictionary();
                while (elementIE.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)elementIE.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName == "href")
                    {
                        attributes.Add(attr.Name, PageUtils.UNCLICKED_URL);
                    }
                    else if (attributeName != "onclick")
                    {
                        attributes.Add(attr.Name, attr.Value);
                    }
                }
                attributes.Add("onclick", clickString);
                attributes.Remove("id");
                attributes.Remove("name");
                if (StringUtils.EqualsIgnoreCase(elementNode.Name, "a"))
                {
                    attributes.Remove("href");
                    attributes.Add("href", PageUtils.UNCLICKED_URL);
                }

                if (!string.IsNullOrEmpty(elementNode.InnerXml))
                {
                    rewriteHtml = rewriteHtml.Replace(submitElement, string.Format(@"<{0} {1}>{2}</{0}>", elementNode.Name, TranslateUtils.ToAttributesString(attributes), elementNode.InnerXml));
                }
                else
                {
                    rewriteHtml = rewriteHtml.Replace(submitElement, string.Format(@"<{0} {1}/>", elementNode.Name, TranslateUtils.ToAttributesString(attributes)));
                }
            }
            return rewriteHtml;
        }
	}
}
