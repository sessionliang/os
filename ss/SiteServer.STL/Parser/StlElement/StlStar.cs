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
	public class StlStar
	{
        private StlStar() { }
        public const string ElementName = "stl:star";

        public const string Attribute_TotalStar = "totalstar";              //最高评分
        public const string Attribute_InitStar = "initstar";                //初始评分
        public const string Attribute_SuccessMessage = "successmessage";    //评分成功提示信息
        public const string Attribute_FailureMessage = "failuremessage";    //评分失败提示信息
        public const string Attribute_Theme = "theme";			            //主题样式
        public const string Attribute_IsTextOnly = "istextonly";            //仅显示评分数
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

		public static ListDictionary AttributeList
		{
			get
			{
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_TotalStar, "最高评分");
                attributes.Add(Attribute_InitStar, "初始评分");
                attributes.Add(Attribute_SuccessMessage, "评分成功提示信息");
                attributes.Add(Attribute_FailureMessage, "评分失败提示信息");
                attributes.Add(Attribute_Theme, "主题样式");
                attributes.Add(Attribute_IsTextOnly, "仅显示评分数");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                return attributes;
			}
		}

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
			try
			{
                int totalStar = 10;
                int initStar = 0;
                string successMessage = string.Empty;
                string failureMessage = "对不起，不能重复操作!";
                string theme = "style1";
                bool isTextOnly = false;
                bool isDynamic = false;

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlStar.Attribute_TotalStar))
                    {
                        totalStar = TranslateUtils.ToInt(attr.Value, totalStar);
                    }
                    else if (attributeName.Equals(StlStar.Attribute_InitStar))
                    {
                        initStar = TranslateUtils.ToInt(attr.Value, 0);
                    }
                    else if (attributeName.Equals(StlStar.Attribute_SuccessMessage))
                    {
                        successMessage = attr.Value;
                    }
                    else if (attributeName.Equals(StlStar.Attribute_FailureMessage))
                    {
                        failureMessage = attr.Value;
                    }
                    else if (attributeName.Equals(StlStar.Attribute_Theme))
                    {
                        theme = attr.Value;
                    }
                    else if (attributeName.Equals(StlStar.Attribute_IsTextOnly))
                    {
                        isTextOnly = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(StlStar.Attribute_IsDynamic))
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
                    parsedContent = ParseImpl(pageInfo, contextInfo, totalStar, initStar, successMessage, failureMessage, theme, isTextOnly);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, int totalStar, int initStar, string successMessage, string failureMessage, string theme, bool isTextOnly)
        {
            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
            ETableStyle tableStyle = NodeManager.GetTableStyle(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
            int contentID = ContentUtility.GetRealContentID(tableStyle, tableName, contextInfo.ContentID);
            int channelID = BaiRongDataProvider.ContentDAO.GetNodeID(tableName, contextInfo.ContentID);

            if (isTextOnly)
            {
                int[] counts = DataProvider.StarDAO.GetCount(pageInfo.PublishmentSystemID, channelID, contentID);
                int totalCount = counts[0];
                int totalPoint = counts[1];

                object[] totalCountAndPointAverage = DataProvider.StarSettingDAO.GetTotalCountAndPointAverage(pageInfo.PublishmentSystemID, contentID);
                int settingTotalCount = (int)totalCountAndPointAverage[0];
                decimal settingPointAverage = (decimal)totalCountAndPointAverage[1];
                if (settingTotalCount > 0 || settingPointAverage > 0)
                {
                    totalCount += settingTotalCount;
                    totalPoint += Convert.ToInt32(settingPointAverage * settingTotalCount);
                }

                decimal num = 0;
                if (totalCount > 0)
                {
                    num = Convert.ToDecimal(totalPoint) / Convert.ToDecimal(totalCount);
                    initStar = 0;
                }
                else
                {
                    num = initStar;
                }

                if (num > totalStar)
                {
                    num = totalStar;
                }

                string numString = num.ToString();
                if (numString.IndexOf('.') == -1)
                {
                    return numString + ".0";
                }
                else
                {
                    string first = numString.Substring(0, numString.IndexOf('.'));
                    string second = numString.Substring(numString.IndexOf('.') + 1, 1);
                    return first + "." + second;
                }
            }
            else
            {
                int updaterID = pageInfo.UniqueID;
                string ajaxDivID = StlParserUtility.GetAjaxDivID(updaterID);

                pageInfo.AddPageScriptsIfNotExists(StlStar.ElementName, string.Format(@"<script language=""javascript"" src=""{0}""></script>", StlTemplateManager.Star.ScriptUrl));

                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(@"<link rel=""stylesheet"" href=""{0}"" type=""text/css"" />", StlTemplateManager.Star.GetStyleUrl(theme));
                builder.AppendFormat(@"<div id=""{0}"">", ajaxDivID);

                string innerPageUrl = StlTemplateManager.Star.GetInnerPageUrl(pageInfo.PublishmentSystemInfo, channelID, contentID, updaterID, totalStar, initStar, theme);
                string innerPageUrlWithAction = StlTemplateManager.Star.GetInnerPageUrlWithAction(pageInfo.PublishmentSystemInfo, channelID, contentID, updaterID, totalStar, initStar, theme);

                string loadingHtml = string.Format(@"<img src=""{0}"" />", PageUtility.GetIconUrl(pageInfo.PublishmentSystemInfo, "loading.gif"));

                builder.AppendFormat(loadingHtml);

                builder.Append("</div>");

                string successAlert = string.Empty;
                if (!string.IsNullOrEmpty(successMessage))
                {
                    successAlert = string.Format("stlSuccessAlert('{0}');", successMessage);
                }

                builder.AppendFormat(@"
<script type=""text/javascript"" language=""javascript"">
function stlStar_{0}(url)
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

stlStar_{0}('{2}');

function stlStarPoint_{0}(point)
{{
    if (stlStarCheck({3}, {4}, {5}, '{6}')){{
        jQuery('#{1}').innerHTML = '{7}';
        stlStar_{0}('{8}' + point);
        {9}
    }}
}}
</script>
", updaterID, ajaxDivID, innerPageUrl, pageInfo.PublishmentSystemID, channelID, contentID, failureMessage, loadingHtml, innerPageUrlWithAction, successAlert);

                return builder.ToString();
            }
        }
	}
}
