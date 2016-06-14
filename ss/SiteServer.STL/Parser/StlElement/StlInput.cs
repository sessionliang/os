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

namespace SiteServer.STL.Parser.StlElement
{
    public class StlInput
    {
        private StlInput() { }
        public const string ElementName = "stl:input";

        public const string Attribute_InputName = "inputname";		        //提交表单名称
        //public const string Attribute_ChannelIndex = "channelindex";		//栏目索引
        //public const string Attribute_ChannelName = "channelname";		    //栏目名称

        //public const string Attribute_CheckMethod = "checkmethod";			//验证提交的JS函数
        //public const string Attribute_Width = "width";				        //宽度
        //public const string Attribute_IsCheckCode = "ischeckcode";          //是否显示验证码
        //public const string Attribute_IsSuccessHide = "issuccesshide";      //提交成功后是否隐藏
        //public const string Attribute_IsSuccessReload = "issuccessreload";  //当提交成功后是否刷新页面
        public const string Attribute_IsLoadValues = "isloadvalues";		//是否载入URL参数
        //public const string Attribute_IsQuickSubmit = "isquicksubmit";		//是否启用Ctrl+Enter快速提交
        //public const string Attribute_IsChooseSubmit = "ischoosesubmit";	//是否可选择提交目的地
        //public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示


        public const string Attribute_SiteName = "sitename"; //站点名称

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_InputName, "提交表单名称");
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
                attributes.Add(Attribute_SiteName, "站点名称");
                return attributes;
            }
        }

        public sealed class InputTemplate
        {
            public const string ElementName = "stl:inputtemplate";
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                string inputName = string.Empty;
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
                string siteName = string.Empty;

                string inputTemplateString = string.Empty;
                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;
                StlParserUtility.GetInnerTemplateStringOfInput(node, out inputTemplateString, out successTemplateString, out failureTemplateString, pageInfo, contextInfo);

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlInput.Attribute_InputName))
                    {
                        inputName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(StlInput.Attribute_SiteName))
                    {
                        siteName = attr.Value;
                    }
                    //else if (attributeName.Equals(StlInput.Attribute_ChannelIndex))
                    //{
                    //    channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    //}
                    //else if (attributeName.Equals(StlInput.Attribute_ChannelName))
                    //{
                    //    channelName  = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    //}
                    //else if (attributeName.Equals(StlInput.Attribute_CheckMethod))
                    //{
                    //    checkMethod = attr.Value;
                    //}
                    //else if (attributeName.Equals(StlInput.Attribute_Width))
                    //{
                    //    width = attr.Value;
                    //}
                    //else if (attributeName.Equals(StlInput.Attribute_IsCheckCode))
                    //{
                    //    isCheckCode = TranslateUtils.ToBool(attr.Value, true);
                    //}
                    //else if (attributeName.Equals(StlInput.Attribute_IsSuccessHide))
                    //{
                    //    isSuccessHide = TranslateUtils.ToBool(attr.Value, true);
                    //}
                    //else if (attributeName.Equals(StlInput.Attribute_IsSuccessReload))
                    //{
                    //    isSuccessReload = TranslateUtils.ToBool(attr.Value, false);
                    //}
                    else if (attributeName.Equals(StlInput.Attribute_IsLoadValues))
                    {
                        isLoadValues = TranslateUtils.ToBool(attr.Value, false);
                    }
                    //else if (attributeName.Equals(StlInput.Attribute_IsQuickSubmit))
                    //{
                    //    isQuickSubmit = TranslateUtils.ToBool(attr.Value, false);
                    //}
                    //else if (attributeName.Equals(StlInput.Attribute_IsChooseSubmit))
                    //{
                    //    isChooseSubmit = TranslateUtils.ToBool(attr.Value, false);
                    //}
                    //else if (attributeName.Equals(StlInput.Attribute_IsDynamic))
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
                //    parsedContent = ParseImpl(pageInfo, contextInfo, inputName, channelIndex, channelName, checkMethod, width, isCheckCode, isSuccessHide, isSuccessReload, isQuickSubmit, isLoadValues, isChooseSubmit, inputTemplateString, successTemplateString, failureTemplateString);
                //}

                parsedContent = ParseImpl(node, pageInfo, contextInfo, inputName, isLoadValues, inputTemplateString, successTemplateString, failureTemplateString, siteName);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, string inputName, bool isLoadValues, string inputTemplateString, string successTemplateString, string failureTemplateString, string siteName)
        {
            string parsedContent = string.Empty;
            int publishmentSystemID = pageInfo.PublishmentSystemID;
            #region 指定了sitename
            PublishmentSystemInfo publishmentSystemInfo = null;
            PublishmentSystemInfo prePublishmentSystemInfo = pageInfo.PublishmentSystemInfo;
            int prePageNodeID = pageInfo.PageNodeID;
            int prePageContentID = pageInfo.PageContentID;
            EVisualType visualType = pageInfo.VisualType;
            if (!string.IsNullOrEmpty(siteName))
            {
                publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfoBySiteName(siteName);
            }
            if (publishmentSystemInfo != null)
            {
                pageInfo.ChangeSite(publishmentSystemInfo, publishmentSystemInfo.PublishmentSystemID, 0, contextInfo, publishmentSystemInfo.Additional.VisualType);

                publishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
            }
            #endregion

            int inputID = DataProvider.InputDAO.GetInputIDAsPossible(inputName, publishmentSystemID);

            if (inputID > 0)
            {
                InputInfo inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);
                SiteServer.STL.StlTemplate.InputTemplate inputTemplate = new SiteServer.STL.StlTemplate.InputTemplate(pageInfo.PublishmentSystemInfo, inputInfo);
                parsedContent = inputTemplate.GetTemplate(inputInfo.IsTemplate, isLoadValues, inputTemplateString, successTemplateString, failureTemplateString);

                StringBuilder innerBuilder = new StringBuilder(parsedContent);
                StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                parsedContent = innerBuilder.ToString();

                if (isLoadValues)
                {
                    pageInfo.AddPageScriptsIfNotExists(StlInput.ElementName, string.Format(@"
<script language=""vbscript""> 
Function str2asc(strstr) 
 str2asc = hex(asc(strstr)) 
End Function 
Function asc2str(ascasc) 
 asc2str = chr(ascasc) 
End Function 
</script>
<script type=""text/javascript"" src=""{0}""></script>", StlTemplateManager.Input.ScriptUrl));
                }
            }

            pageInfo.ChangeSite(prePublishmentSystemInfo, prePageNodeID, prePageContentID, contextInfo, visualType);
            return parsedContent;
        }
    }
}
