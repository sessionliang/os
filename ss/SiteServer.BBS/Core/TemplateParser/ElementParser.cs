using System;
using System.Text;
using System.Collections;
using System.Xml;
using SiteServer.BBS.Core.TemplateParser.Element;
using SiteServer.BBS.Core.TemplateParser.Model;

namespace SiteServer.BBS.Core.TemplateParser
{
    public class ElementParser
	{
        private ElementParser()
		{
		}

        public static void ReplaceElements(StringBuilder parsedBuilder, PageInfo pageInfo, ContextInfo contextInfo)
        {
            ArrayList elementArrayList = ParserUtility.GetElementArrayList(parsedBuilder.ToString());

            foreach (string element in elementArrayList)
            {
                try
                {
                    int startIndex = parsedBuilder.ToString().IndexOf(element);
                    if (startIndex != -1)
                    {
                        string resultContent = ElementParser.ParseElement(element, pageInfo, contextInfo);
                        parsedBuilder.Replace(element, resultContent, startIndex, element.Length);
                    }
                }
                catch { }
            }
        }

        internal static string ParseElement(string element, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent;
            XmlDocument xmlDocument = ParserUtility.GetXmlDocument(element, contextInfo.IsInnerElement);
			XmlNode node = xmlDocument.DocumentElement;
			node = node.FirstChild;

			if (node != null && node.Name != null)
			{
				string elementName = node.Name.ToLower();

                if (string.Equals(elementName, Include.ElementName))
				{
                    parsedContent = Include.Parse(element, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, Forums.ElementName))
                {
                    parsedContent = Forums.Parse(element, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, Forum.ElementName))
                {
                    parsedContent = Forum.Parse(element, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, Threads.ElementName))
                {
                    parsedContent = Threads.Parse(element, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, Thread.ElementName))
                {
                    parsedContent = Thread.Parse(element, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, Posts.ElementName))
                {
                    parsedContent = Posts.Parse(element, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, Post.ElementName))
                {
                    parsedContent = Post.Parse(element, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, If.ElementName))
                {
                    parsedContent = If.Parse(element, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, A.ElementName))
                {
                    parsedContent = A.Parse(element, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, Run.ElementName))
                {
                    parsedContent = Run.Parse(element, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, Value.ElementName))
                {
                    parsedContent = Value.Parse(elementName, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, Navigation.ElementName))
                {
                    parsedContent = Navigation.Parse(elementName, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, Location.ElementName))
                {
                    parsedContent = Location.Parse(elementName, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, SqlContents.ElementName))
                {
                    parsedContent = SqlContents.Parse(elementName, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, SqlContent.ElementName))
                {
                    parsedContent = SqlContent.Parse(elementName, node, pageInfo, contextInfo);
                }
                else if (string.Equals(elementName, Dynamic.ElementName))
                {
                    parsedContent = Dynamic.Parse(node, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = element;
                }
            }
			else
			{
				parsedContent = element;
			}

			//return parsedContent;
            if (contextInfo.IsInnerElement)
            {
                return parsedContent;
            }
            else
            {
                return ParserUtility.GetBackHtml(parsedContent);
            }
		}
		
	}
}
