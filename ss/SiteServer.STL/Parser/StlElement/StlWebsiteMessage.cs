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
    public class StlWebsiteMessage
    {
        private StlWebsiteMessage() { }
        public const string ElementName = "stl:websitemessage";

        public const string SimpleElementName = "stl:wsmessage";

        public const string Attribute_WebsiteMessageName = "websiteMessagename";		        //提交表单名称
        //public const string Attribute_ChannelIndex = "channelindex";		//栏目索引
        //public const string Attribute_ChannelName = "channelname";		    //栏目名称

        //public const string Attribute_CheckMethod = "checkmethod";			//验证提交的JS函数
        //public const string Attribute_Width = "width";				        //宽度
        //public const string Attribute_IsCheckCode = "ischeckcode";          //是否显示验证码
        //public const string Attribute_IsSuccessHide = "issuccesshide";      //提交成功后是否隐藏
        //public const string Attribute_IsSuccessReload = "issuccessreload";  //当提交成功后是否刷新页面
        public const string Attribute_IsLoadValues = "isloadvalues";        //是否载入URL参数
        //public const string Attribute_IsQuickSubmit = "isquicksubmit";		//是否启用Ctrl+Enter快速提交
        //public const string Attribute_IsChooseSubmit = "ischoosesubmit";	//是否可选择提交目的地
        //public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_WebsiteMessageName, "提交表单名称");
                //attributes.Add(Attribute_ChannelIndex, "栏目索引");
                //attributes.Add(Attribute_ChannelName, "栏目名称");
                //attributes.Add(Attribute_CheckMethod, "验证提交的JS函数");
                //attributes.Add(Attribute_Width, "宽度");
                //attributes.Add(Attribute_IsCheckCode, "是否显示验证码");
                //attributes.Add(Attribute_IsSuccessHide, "提交成功后是否隐藏");
                //attributes.Add(Attribute_IsSuccessReload, "当提交成功后是否刷新页面");
                attributes.Add(Attribute_IsLoadValues, "是否载入URL参数");
                //attributes.Add(Attribute_IsQuickSubmit, "是否启用Ctrl+Enter快速提交");
                //attributes.Add(Attribute_IsChooseSubmit, "是否可选择提交目的地");
                //attributes.Add(Attribute_IsDynamic, "是否动态显示");
                return attributes;
            }
        }

        public sealed class WebsiteMessageTemplate
        {
            public const string ElementName = "stl:websiteMessagetemplate";
        }

        public sealed class WebstieMessageClassifyTemplate
        {
            public const string ElementName = "stl:webstiemessageclassifytemplate";
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                string websiteMessageName = "Default";
                //string channelIndex = string.Empty;
                //string channelName = string.Empty;
                //string checkMethod = string.Empty;
                //string width = "95%";
                //bool isCheckCode = true;
                //bool isSuccessHide = true;
                //bool isSuccessReload = false;
                bool isLoadValues = false;
                //bool isQuickSubmit = false;
                //bool isChooseSubmit = false;
                //bool isDynamic = false;

                string websiteMessageTemplateString = string.Empty;
                string classifyTemplateString = string.Empty;
                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;
                StlParserUtility.GetInnerTemplateStringOfWebsiteMessage(node, out websiteMessageTemplateString, out classifyTemplateString, out successTemplateString, out failureTemplateString, pageInfo, contextInfo);

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlWebsiteMessage.Attribute_WebsiteMessageName))
                    {
                        websiteMessageName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    //else if (attributeName.Equals(StlWebsiteMessage.Attribute_ChannelIndex))
                    //{
                    //    channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    //}
                    //else if (attributeName.Equals(StlWebsiteMessage.Attribute_ChannelName))
                    //{
                    //    channelName  = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    //}
                    //else if (attributeName.Equals(StlWebsiteMessage.Attribute_CheckMethod))
                    //{
                    //    checkMethod = attr.Value;
                    //}
                    //else if (attributeName.Equals(StlWebsiteMessage.Attribute_Width))
                    //{
                    //    width = attr.Value;
                    //}
                    //else if (attributeName.Equals(StlWebsiteMessage.Attribute_IsCheckCode))
                    //{
                    //    isCheckCode = TranslateUtils.ToBool(attr.Value, true);
                    //}
                    //else if (attributeName.Equals(StlWebsiteMessage.Attribute_IsSuccessHide))
                    //{
                    //    isSuccessHide = TranslateUtils.ToBool(attr.Value, true);
                    //}
                    //else if (attributeName.Equals(StlWebsiteMessage.Attribute_IsSuccessReload))
                    //{
                    //    isSuccessReload = TranslateUtils.ToBool(attr.Value, false);
                    //}
                    else if (attributeName.Equals(StlWebsiteMessage.Attribute_IsLoadValues))
                    {
                        isLoadValues = TranslateUtils.ToBool(attr.Value, false);
                    }
                    //else if (attributeName.Equals(StlWebsiteMessage.Attribute_IsQuickSubmit))
                    //{
                    //    isQuickSubmit = TranslateUtils.ToBool(attr.Value, false);
                    //}
                    //else if (attributeName.Equals(StlWebsiteMessage.Attribute_IsChooseSubmit))
                    //{
                    //    isChooseSubmit = TranslateUtils.ToBool(attr.Value, false);
                    //}
                    //else if (attributeName.Equals(StlWebsiteMessage.Attribute_IsDynamic))
                    //{
                    //    isDynamic = TranslateUtils.ToBool(attr.Value);
                    //}
                }

                //pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Aa_Prototype);
                //pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ab_Scriptaculous);
                //pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Page_ShowPopWin);

                //pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Aj_JSON);
                //pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Inner_FCKEditor);
                //pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Inner_Calendar);
                //pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Page_Script);
                //pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Validate_Script);

                //if (isDynamic)
                //{
                //    parsedContent = StlDynamic.ParseDynamicElement(stlElement, pageInfo, contextInfo);
                //}
                //else
                //{
                //    parsedContent = ParseImpl(pageInfo, contextInfo, websiteMessageName, channelIndex, channelName, checkMethod, width, isCheckCode, isSuccessHide, isSuccessReload, isQuickSubmit, isLoadValues, isChooseSubmit, websiteMessageTemplateString, successTemplateString, failureTemplateString);
                //}

                parsedContent = ParseImpl(pageInfo, contextInfo, websiteMessageName, isLoadValues, websiteMessageTemplateString, classifyTemplateString, successTemplateString, failureTemplateString);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, string websiteMessageName, bool isLoadValues, string websiteMessageTemplateString, string classifyTemplateString, string successTemplateString, string failureTemplateString)
        {
            string parsedContent = string.Empty;
            string classifyTemplate = string.Empty;//分类模板{classify.ID}  {classify.Name}
            int websiteMessageID = DataProvider.WebsiteMessageDAO.GetWebsiteMessageIDAsPossible(websiteMessageName, pageInfo.PublishmentSystemID);

            if (websiteMessageID > 0)
            {
                WebsiteMessageInfo websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(websiteMessageID);
                SiteServer.STL.StlTemplate.WebsiteMessageTemplate websiteMessageTemplate = new SiteServer.STL.StlTemplate.WebsiteMessageTemplate(pageInfo.PublishmentSystemInfo, websiteMessageInfo);
                parsedContent += websiteMessageTemplate.GetTemplate(websiteMessageInfo.IsTemplate, isLoadValues, websiteMessageTemplateString, classifyTemplateString, successTemplateString, failureTemplateString);

                StringBuilder innerBuilder = new StringBuilder(parsedContent);

                StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                parsedContent = innerBuilder.ToString();

                if (isLoadValues)
                {
                    pageInfo.AddPageScriptsIfNotExists(StlWebsiteMessage.ElementName, string.Format(@"
<script language=""vbscript""> 
Function str2asc(strstr) 
 str2asc = hex(asc(strstr)) 
End Function 
Function asc2str(ascasc) 
 asc2str = chr(ascasc) 
End Function 
</script>
<script type=""text/javascript"" src=""{0}""></script>", StlTemplateManager.WebsiteMessage.ScriptUrl));
                }
            }

            return parsedContent;
        }
    }
}
