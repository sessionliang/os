using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlZoom
	{
        private StlZoom() { }
        public const string ElementName = "stl:zoom";    //��������

        public const string Attribute_ZoomID = "zoomid";		        //ҳ��HTML�����Ŷ����ID����
        public const string Attribute_FontSize = "fontsize";		    //���������С
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_ZoomID, "ҳ��HTML�����Ŷ����ID����");
                attributes.Add(Attribute_FontSize, "���������С");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
				return attributes;
			}
		}


        //�ԡ��������š���stl:zoom��Ԫ�ؽ��н���
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
			try
			{
                HtmlAnchor stlAnchor = new HtmlAnchor();
				IEnumerator ie = node.Attributes.GetEnumerator();

                string zoomID = string.Empty;
                int fontSize = 16;
                bool isDynamic = false;

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(StlZoom.Attribute_ZoomID))
					{
                        zoomID = attr.Value;
                    }
                    else if (attributeName.Equals(StlZoom.Attribute_FontSize))
                    {
                        fontSize = TranslateUtils.ToInt(attr.Value, 16);
                    }
                    else if (attributeName.Equals(StlZoom.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                    else
                    {
                        ControlUtils.AddAttributeIfNotExists(stlAnchor, attributeName, attr.Value);
                    }
				}

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, stlAnchor, zoomID, fontSize);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, HtmlAnchor stlAnchor, string zoomID, int fontSize)
        {
            string parsedContent = string.Empty;

            if (string.IsNullOrEmpty(zoomID))
            {
                zoomID = "content";
            }

            pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ae_StlZoom, @"
<script language=""JavaScript"" type=""text/javascript"">
function stlDoZoom(zoomId, size){
    var artibody = document.getElementById(zoomId);
    if(!artibody){
        return;
    }
    var artibodyChild = artibody.childNodes;
    artibody.style.fontSize = size + 'px';
    for(var i = 0; i < artibodyChild.length; i++){
        if(artibodyChild[i].nodeType == 1){
            artibodyChild[i].style.fontSize = size + 'px';
        }
    }
}
</script>
");

            if (node.InnerXml.Trim().Length == 0)
            {
                stlAnchor.InnerHtml = "����";
            }
            else
            {
                StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                stlAnchor.InnerHtml = innerBuilder.ToString();
            }
            stlAnchor.Attributes["href"] = string.Format("javascript:stlDoZoom('{0}', {1});", zoomID, fontSize);

            return ControlUtils.GetControlRenderHtml(stlAnchor);
        }
	}
}
