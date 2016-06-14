using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using System.Text.RegularExpressions;
using System;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlDigg
	{
        private StlDigg() { }
        public const string ElementName = "stl:digg";

        public const string Attribute_Type = "type";				        //����
        public const string Attribute_GoodText = "goodtext";				//��ͬ����
        public const string Attribute_BadText = "badtext";				    //����ͬ����
        public const string Attribute_Theme = "theme";			            //������ʽ
        public const string Attribute_IsNumber = "isnumber";                //����ʾ�������
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

		public static ListDictionary AttributeList
		{
			get
			{
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Type, "����");
                attributes.Add(Attribute_GoodText, "��ͬ����");
                attributes.Add(Attribute_BadText, "����ͬ����");
                attributes.Add(Attribute_Theme, "������ʽ");
                attributes.Add(Attribute_IsNumber, "����ʾ�������");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
                return attributes;
			}
		}

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
			try
			{
                EDiggType diggType = EDiggType.All;
                string goodText = "��һ��";
                string badText = "��һ��";
                string theme = "style1";
                bool isNumber = false;
                bool isDynamic = false;

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlDigg.Attribute_Type))
                    {
                        diggType = EDiggTypeUtils.GetEnumType(attr.Value);
                    }
                    else if (attributeName.Equals(StlDigg.Attribute_GoodText))
                    {
                        goodText = attr.Value;
                    }
                    else if (attributeName.Equals(StlDigg.Attribute_BadText))
                    {
                        badText = attr.Value;
                    }
                    else if (attributeName.Equals(StlDigg.Attribute_Theme))
                    {
                        theme = attr.Value;
                    }
                    else if (attributeName.Equals(StlDigg.Attribute_IsNumber))
                    {
                        isNumber = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlDigg.Attribute_IsDynamic))
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
                    parsedContent = ParseImpl(pageInfo, contextInfo, diggType, goodText, badText, theme, isNumber);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, EDiggType diggType, string goodText, string badText, string theme, bool isNumber)
        {
            if (isNumber)
            {
                int count = 0;

                int relatedIdentity = contextInfo.ContentID;
                if (relatedIdentity == 0 || contextInfo.ContextType == EContextType.Channel)
                {
                    relatedIdentity = contextInfo.ChannelID;
                }

                int[] counts = BaiRongDataProvider.DiggDAO.GetCount(pageInfo.PublishmentSystemID, relatedIdentity);
                int goodNum = counts[0];
                int badNum = counts[1];

                if (diggType == EDiggType.Good)
                {
                    count = goodNum;
                }
                else if (diggType == EDiggType.Bad)
                {
                    count = badNum;
                }
                else
                {
                    count = goodNum + badNum;
                }

                return count.ToString();
            }
            else
            {
                int updaterID = pageInfo.UniqueID;
                string ajaxDivID = StlParserUtility.GetAjaxDivID(updaterID);

                pageInfo.AddPageScriptsIfNotExists(StlDigg.ElementName, string.Format(@"<script language=""javascript"" src=""{0}""></script>", StlTemplateManager.Digg.ScriptUrl));

                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(@"<link rel=""stylesheet"" href=""{0}"" type=""text/css"" />", StlTemplateManager.Digg.GetStyleUrl(theme));
                builder.AppendFormat(@"<div id=""{0}"">", ajaxDivID);

                int relatedIdentity = contextInfo.ContentID;
                if (relatedIdentity == 0 || contextInfo.ContextType == EContextType.Channel)
                {
                    relatedIdentity = contextInfo.ChannelID;
                }

                string innerPageUrl = StlTemplateManager.Digg.GetInnerPageUrl(pageInfo.PublishmentSystemInfo, relatedIdentity, updaterID, diggType, goodText, badText, theme);
                string innerPageUrlWithGood = StlTemplateManager.Digg.GetInnerPageUrlWithAction(pageInfo.PublishmentSystemInfo, relatedIdentity, updaterID, diggType, goodText, badText, theme, true);
                string innerPageUrlWithBad = StlTemplateManager.Digg.GetInnerPageUrlWithAction(pageInfo.PublishmentSystemInfo, relatedIdentity, updaterID, diggType, goodText, badText, theme, false);

                string loadingHtml = string.Format(@"<img src=""{0}"" />", PageUtility.GetIconUrl(pageInfo.PublishmentSystemInfo, "loading.gif"));

                builder.AppendFormat(loadingHtml);

                builder.Append("</div>");

                builder.AppendFormat(@"
<script type=""text/javascript"" language=""javascript"">
function stlDigg_{0}(url)
{{
    try
    {{
        var cnum=Math.ceil(Math.random()*1000);
        url = url + '&r=' + cnum;

        jQuery.get(url, '', function(data, textStatus){{
            jQuery('#{1}').html(data);
        }});

    }}catch(e){{}}
}}

stlDigg_{0}('{2}');

function stlDiggSet_{0}(isGood)
{{
    if (stlDiggCheck({3}, {4})){{
        jQuery('#{1}').html('{5}');
        if (isGood)
        {{
            stlDigg_{0}('{6}');
        }}else{{
            stlDigg_{0}('{7}');
        }}
    }}
}}
</script>
", updaterID, ajaxDivID, innerPageUrl, pageInfo.PublishmentSystemID, relatedIdentity, loadingHtml, innerPageUrlWithGood, innerPageUrlWithBad);

                return builder.ToString();
            }
        }


	}
}
