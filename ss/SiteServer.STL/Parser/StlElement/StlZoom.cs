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
        public const string ElementName = "stl:zoom";    //文字缩放

        public const string Attribute_ZoomID = "zoomid";		        //页面HTML中缩放对象的ID属性
        public const string Attribute_FontSize = "fontsize";		    //缩放字体大小
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_ZoomID, "页面HTML中缩放对象的ID属性");
                attributes.Add(Attribute_FontSize, "缩放字体大小");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
				return attributes;
			}
		}


        //对“文字缩放”（stl:zoom）元素进行解析
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
                stlAnchor.InnerHtml = "缩放";
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
