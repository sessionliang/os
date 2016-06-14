using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System.Web.UI;
using System;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlTabs
    {
        private StlTabs() { }
        public const string ElementName = "stl:tabs";           //ҳǩ�л�

        public const string Attribute_TabName = "tabname";			            //ҳǩ����
        public const string Attribute_IsHeader = "isheader";			        //�Ƿ�Ϊҳǩͷ��
        public const string Attribute_Action = "action";			            //ҳǩ�л���ʽ
        public const string Attribute_ClassActive = "classactive";				//��ǰ��ʾҳǩͷ����CSS��
        public const string Attribute_ClassNormal = "classnormal";              //��ǰ����ҳǩͷ����CSS��
        public const string Attribute_Current = "current";                      //��ǰҳǩ
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

        public const string Action_Click = "Click";			                    //���
        public const string Action_MouseOver = "MouseOver";			            //����ƶ�

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();

                attributes.Add(Attribute_TabName, "ҳǩ����");
                attributes.Add(Attribute_IsHeader, "�Ƿ�Ϊҳǩͷ��");
                attributes.Add(Attribute_Action, "ҳǩ�л���ʽ");
                attributes.Add(Attribute_ClassActive, "��ǰ��ʾҳǩͷ����CSS��");
                attributes.Add(Attribute_ClassNormal, "��ǰ����ҳǩͷ����CSS��");
                attributes.Add(Attribute_Current, "��ǰҳǩ");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");

                return attributes;
            }
        }

        internal static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();

                string tabName = string.Empty;
                bool isHeader = true;
                string action = Action_MouseOver;
                string classActive = "active";
                string classNormal = string.Empty;
                int current = 0;
                bool isDynamic = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlTabs.Attribute_TabName))
                    {
                        tabName = attr.Value;
                    }
                    else if (attributeName.Equals(StlTabs.Attribute_IsHeader))
                    {
                        isHeader = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlTabs.Attribute_Action))
                    {
                        action = attr.Value;
                    }
                    else if (attributeName.Equals(StlTabs.Attribute_ClassActive))
                    {
                        classActive = attr.Value;
                    }
                    else if (attributeName.Equals(StlTabs.Attribute_ClassNormal))
                    {
                        classNormal = attr.Value;
                    }
                    else if (attributeName.Equals(StlTabs.Attribute_Current))
                    {
                        current = TranslateUtils.ToInt(attr.Value, 1);
                    }
                    else if (attributeName.Equals(StlTabs.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                }

                pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, tabName, isHeader, action, classActive, classNormal, current);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, string tabName, bool isHeader, string action, string classActive, string classNormal, int current)
        {
            StringBuilder builder = new StringBuilder();
            int uniqueID = pageInfo.UniqueID;

            if (node.ChildNodes != null && node.ChildNodes.Count > 0)
            {
                if (isHeader)
                {
                    builder.AppendFormat(@"
<SCRIPT language=javascript>
function stl_tab_{0}(tabName, no){{
	for ( i = 1; i <= {1}; i++){{
		var el = jQuery('#{2}_tabContent_' + i);
		var li = $('#{2}_tabHeader_' + i);
		if (i == no){{
            try{{
			    el.show();
            }}catch(e){{}}
            li.removeClass('{4}');
            li.addClass('{3}');
		}}else{{
            try{{
			    el.hide();
            }}catch(e){{}}
            li.removeClass('{3}');
            li.addClass('{4}');
		}}
	}}
}}
</SCRIPT>
", uniqueID, node.ChildNodes.Count, tabName, classActive, classNormal);
                }

                int count = 0;
                foreach (XmlNode xmlNode in node.ChildNodes)
                {
                    if (xmlNode.NodeType != XmlNodeType.Element) continue;
                    NameValueCollection attributes = new NameValueCollection();
                    IEnumerator xmlIE = xmlNode.Attributes.GetEnumerator();
                    while (xmlIE.MoveNext())
                    {
                        XmlAttribute attr = (XmlAttribute)xmlIE.Current;
                        string attributeName = attr.Name.ToLower();
                        if (!attributeName.Equals("id") && !attributeName.Equals("onmouseover") && !attributeName.Equals("onclick"))
                        {
                            attributes[attributeName] = attr.Value;
                        }
                    }

                    count++;
                    if (isHeader)
                    {
                        attributes["id"] = string.Format("{0}_tabHeader_{1}", tabName, count);
                        if (StringUtils.EqualsIgnoreCase(action, Action_MouseOver))
                        {
                            attributes["onmouseover"] = string.Format("stl_tab_{0}('{1}', {2});return false;", uniqueID, tabName, count);
                        }
                        else
                        {
                            attributes["onclick"] = string.Format("stl_tab_{0}('{1}', {2});return false;", uniqueID, tabName, count);
                        }
                        if (current != 0)
                        {
                            if (count == current)
                            {
                                attributes["class"] = classActive;
                            }
                            else
                            {
                                attributes["class"] = classNormal;
                            }
                        }
                    }
                    else
                    {
                        attributes["id"] = string.Format("{0}_tabContent_{1}", tabName, count);
                        if (current != 0)
                        {
                            if (count != current)
                            {
                                attributes["style"] = string.Format("display:none;{0}", attributes["style"]);
                            }
                        }
                    }

                    string innerXml = string.Empty;
                    if (!string.IsNullOrEmpty(xmlNode.InnerXml))
                    {
                        StringBuilder innerBuilder = new StringBuilder(xmlNode.InnerXml);
                        StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                        StlParserUtility.XmlToHtml(innerBuilder);
                        innerXml = innerBuilder.ToString();
                    }

                    builder.AppendFormat("<{0} {1}>{2}</{0}>", xmlNode.Name, TranslateUtils.ToAttributesString(attributes), innerXml);
                }
            }

            return builder.ToString();
        }
    }
}
