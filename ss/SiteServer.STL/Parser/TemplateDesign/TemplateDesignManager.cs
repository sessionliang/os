using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.BackgroundPages;
using SiteServer.STL.BackgroundPages.Modal.StlTemplate;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Model;
using SiteServer.CMS.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SiteServer.CMS.Core;
using SiteServer.STL.BackgroundPages;

namespace SiteServer.STL.Parser.TemplateDesign
{
    public class TemplateDesignManager
    {
        private const int SequenceNotExists = -1;
        private const string EmptyPlaceHolder = "<input type=\"hidden\" class=\"stl-hidden\" style=\"display:none\" />";

        public static string GetStlElement(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string includeUrl, int stlSequence)
        {
            StringBuilder contentBuilder = new StringBuilder(TemplateDesignUndoRedo.GetTemplateContent(publishmentSystemInfo, templateInfo, includeUrl));

            List<string> stlElementList = StlParserUtility.GetStlElementList(contentBuilder.ToString());

            if (stlSequence < stlElementList.Count)
            {
                return stlElementList[stlSequence] as string;
            }

            return string.Empty;
        }

        public static void UpdateStlElement(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string includeUrl, int stlSequence, string newContent)
        {
            string templateContent = TemplateDesignUndoRedo.GetTemplateContent(publishmentSystemInfo, templateInfo, includeUrl);
            if (string.IsNullOrEmpty(newContent))
            {
                newContent = TemplateDesignManager.EmptyPlaceHolder;
            }

            string stlElement = string.Empty;
            List<string> stlElementList = StlParserUtility.GetStlElementList(templateContent);
            if (stlSequence < stlElementList.Count)
            {
                stlElement = stlElementList[stlSequence] as string;
            }

            if (!string.IsNullOrEmpty(stlElement))
            {
                int identical = 0;
                for (int i = 0; i < stlElementList.Count && i < stlSequence; i++)
                {
                    if ((string)stlElementList[i] == stlElement)
                    {
                        identical++;
                    }
                }

                templateContent = StringUtils.ReplaceSpecified(stlElement, templateContent, newContent, identical + 1);

                TemplateDesignManager.UpdateTemplateInfo(publishmentSystemInfo, templateInfo, includeUrl, templateContent);
            }
        }

        public static void UpdateStlElement(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string includeUrl, int stlIndex, string oldContent, string newContent)
        {
            string templateContent = TemplateDesignUndoRedo.GetTemplateContent(publishmentSystemInfo, templateInfo, includeUrl);
            if (string.IsNullOrEmpty(newContent))
            {
                newContent = TemplateDesignManager.EmptyPlaceHolder;
            }

            if (!string.IsNullOrEmpty(oldContent))
            {
                templateContent = templateContent.Substring(0, stlIndex) + newContent + templateContent.Substring(stlIndex + oldContent.Length);

                TemplateDesignManager.UpdateTemplateInfo(publishmentSystemInfo, templateInfo, includeUrl, templateContent);
            }
        }

        public static void MoveStlElement(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string includeUrl, int stlSequence, string stlSequenceCollection, int stlElementIndex, int stlDivIndex)
        {
            string templateContent = TemplateDesignUndoRedo.GetTemplateContent(publishmentSystemInfo, templateInfo, includeUrl);

            string stlElement = string.Empty;
            List<string> stlElementList = StlParserUtility.GetStlElementList(templateContent);
            if (stlSequence < stlElementList.Count)
            {
                stlElement = stlElementList[stlSequence] as string;
            }

            if (!string.IsNullOrEmpty(stlElement))
            {
                int topSequence = -1;
                int bottomSequence = -1;

                List<int> sequenceList = TranslateUtils.StringCollectionToIntList(stlSequenceCollection);

                int index = sequenceList.IndexOf(stlSequence);
                if (index - 1 >= 0 && index - 1 < sequenceList.Count)
                {
                    topSequence = sequenceList[index - 1];
                }
                if (index + 1 >= 0 && index + 1 < sequenceList.Count)
                {
                    bottomSequence = sequenceList[index + 1];
                }

                //element move to stl exist container
                if (topSequence >= 0 || bottomSequence >= 0)
                {
                    //remove element
                    int identical = 0;
                    for (int i = 0; i < stlElementList.Count && i < stlSequence; i++)
                    {
                        if ((string)stlElementList[i] == stlElement)
                        {
                            identical++;
                        }
                    }
                    templateContent = StringUtils.ReplaceSpecified(stlElement, templateContent, TemplateDesignManager.EmptyPlaceHolder, identical + 1);

                    //add element
                    if (topSequence >= 0)
                    {
                        string topElement = (string)stlElementList[topSequence];

                        identical = 0;
                        for (int i = 0; i < stlElementList.Count && i < topSequence; i++)
                        {
                            if ((string)stlElementList[i] == topElement)
                            {
                                identical++;
                            }
                        }

                        templateContent = StringUtils.ReplaceSpecified(topElement, templateContent, topElement + StringUtils.Constants.ReturnAndNewline + stlElement, identical + 1);
                    }
                    else if (bottomSequence >= 0)
                    {
                        string bottomElement = (string)stlElementList[bottomSequence];

                        identical = 0;
                        for (int i = 0; i < stlElementList.Count && i < bottomSequence; i++)
                        {
                            if ((string)stlElementList[i] == bottomElement)
                            {
                                identical++;
                            }
                        }

                        templateContent = StringUtils.ReplaceSpecified(bottomElement, templateContent, stlElement + StringUtils.Constants.ReturnAndNewline + bottomElement, identical + 1);
                    }
                }
                //element move to an empty container
                else
                {
                    //add element
                    templateContent = StringUtils.ReplaceAfterIndex(">", templateContent, ">" + StringUtils.Constants.ReturnAndNewline + stlElement, stlDivIndex);

                    //remove element
                    int identical = 0;
                    for (int i = 0; i < stlElementList.Count && i < stlSequence; i++)
                    {
                        if ((string)stlElementList[i] == stlElement)
                        {
                            identical++;
                        }
                    }

                    if (stlElementIndex > stlDivIndex)
                    {
                        identical++;
                    }

                    templateContent = StringUtils.ReplaceSpecified(stlElement, templateContent, TemplateDesignManager.EmptyPlaceHolder, identical + 1);
                }

                TemplateDesignManager.UpdateTemplateInfo(publishmentSystemInfo, templateInfo, includeUrl, templateContent);
            }
        }

        public static void InsertStlElement(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string includeUrl, string stlSequenceCollection, string stlElementToAdd, int stlDivIndex, bool isContainer)
        {
            string templateContent = TemplateDesignUndoRedo.GetTemplateContent(publishmentSystemInfo, templateInfo, includeUrl);

            List<string> stlElementList = StlParserUtility.GetStlElementList(templateContent);

            if (!string.IsNullOrEmpty(stlElementToAdd))
            {
                if (isContainer)
                {
                    stlElementToAdd = StlContainer.GetContainer(stlElementToAdd);
                }

                int topSequence = -1;
                int bottomSequence = -1;

                List<int> sequenceList = TranslateUtils.StringCollectionToIntList(stlSequenceCollection);

                int index = sequenceList.IndexOf(TemplateDesignManager.SequenceNotExists);
                if (index - 1 >= 0 && index - 1 < sequenceList.Count)
                {
                    topSequence = sequenceList[index - 1];
                }
                if (index + 1 >= 0 && index + 1 < sequenceList.Count)
                {
                    bottomSequence = sequenceList[index + 1];
                }

                //element move to stl exist container
                if (topSequence >= 0 || bottomSequence >= 0)
                {
                    if (topSequence >= 0)
                    {
                        string topElement = (string)stlElementList[topSequence];

                        int identical = 0;
                        for (int i = 0; i < stlElementList.Count && i < topSequence; i++)
                        {
                            if ((string)stlElementList[i] == topElement)
                            {
                                identical++;
                            }
                        }

                        templateContent = StringUtils.ReplaceSpecified(topElement, templateContent, topElement + StringUtils.Constants.ReturnAndNewline + stlElementToAdd, identical + 1);
                    }
                    else if (bottomSequence >= 0)
                    {
                        string bottomElement = (string)stlElementList[bottomSequence];

                        int identical = 0;
                        for (int i = 0; i < stlElementList.Count && i < bottomSequence; i++)
                        {
                            if ((string)stlElementList[i] == bottomElement)
                            {
                                identical++;
                            }
                        }

                        templateContent = StringUtils.ReplaceSpecified(bottomElement, templateContent, stlElementToAdd + StringUtils.Constants.ReturnAndNewline + bottomElement, identical + 1);
                    }
                }
                //element move to an empty container
                else
                {
                    //add element
                    templateContent = StringUtils.ReplaceAfterIndex(">", templateContent, ">" + StringUtils.Constants.ReturnAndNewline + stlElementToAdd, stlDivIndex);
                }

                TemplateDesignManager.UpdateTemplateInfo(publishmentSystemInfo, templateInfo, includeUrl, templateContent);
            }
        }

        public static void UpdateTemplateInfo(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string includeUrl, string templateContent)
        {
            if (!string.IsNullOrEmpty(templateContent))
            {
                string doublePlaceHolder = TemplateDesignManager.EmptyPlaceHolder + StringUtils.Constants.ReturnAndNewline + TemplateDesignManager.EmptyPlaceHolder;
                templateContent = templateContent.Replace(doublePlaceHolder, TemplateDesignManager.EmptyPlaceHolder);

                TemplateDesignUndoRedo undoRedo = TemplateDesignUndoRedo.GetInstance(publishmentSystemInfo, templateInfo, includeUrl);
                undoRedo.InsertObjectforUndoRedo(templateContent);
            }
        }

        public static void SaveTemplateInfo(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string includeUrl)
        {
            TemplateDesignUndoRedo undoRedo = TemplateDesignUndoRedo.GetInstance(publishmentSystemInfo, templateInfo, includeUrl);
            if (string.IsNullOrEmpty(includeUrl))
            {
                DataProvider.TemplateDAO.Update(publishmentSystemInfo, templateInfo, undoRedo.TemplateContent);
            }
            else
            {
                string filePath = PathUtility.MapPath(publishmentSystemInfo, RuntimeUtils.DecryptStringByTranslate(includeUrl));
                FileUtils.WriteText(filePath, templateInfo.Charset, undoRedo.TemplateContent);
            }
            
            undoRedo.IsDirty = false;
        }

        public static string ParseImgElements(string templateHtml, PublishmentSystemInfo publishmentSystemInfo, int templateID, string includeUrl)
        {
            string parsedContent = templateHtml;

            string clazz = "stl-element-tips";

            string selectImageClick = StlSelectImage.GetOpenWindowStringToImageUrl(publishmentSystemInfo.PublishmentSystemID, TemplateDesignUtility.IMG_SRC_PLACEHOLDER, templateID, includeUrl);
            string uploadImageClick = StlUploadImageSingle.GetOpenWindowStringToImageUrl(publishmentSystemInfo.PublishmentSystemID, TemplateDesignUtility.IMG_SRC_PLACEHOLDER, templateID, includeUrl);
            string cuttingImageClick = StlCuttingImage.GetOpenWindowStringToImageUrl(publishmentSystemInfo.PublishmentSystemID, TemplateDesignUtility.IMG_SRC_PLACEHOLDER);

            string imgButtons = string.Format(@"
    <a href=""javascript:;"" onclick=""{0};return false;"">选择</a>
    <a href=""javascript:;"" onclick=""{1};return false;"">上传</a>
    <a href=""javascript:;"" onclick=""{2};return false;"">裁切/旋转</a>
", selectImageClick, uploadImageClick, cuttingImageClick);

            return TemplateDesignUtility.AddClassAndAttribute(parsedContent, clazz, "tipHtml", HtmlAttributesEncode(imgButtons), true);
        }

        public static string ParseStlElement(PageInfo pageInfo, string includeUrl, string stlElement, string parsedContent, int stlSequence, bool isInnerElement)
        {
            TemplateDesignManager.RegistDesignScript(pageInfo, includeUrl);

            int startIndex = stlElement.Substring(0, stlElement.IndexOf('>')).IndexOf(@" stl-index=""");
            if (startIndex != -1)
            {
                int stlIndex = TranslateUtils.ToInt(RegexUtils.GetAttributeContent("stl-index", stlElement));
                stlElement = RegexUtils.Replace(@"\sstl-index=""\w+?""", stlElement, string.Empty);
                parsedContent = TemplateDesignManager.GetDesignHtml(pageInfo, includeUrl, stlSequence, stlIndex, stlElement, parsedContent, isInnerElement);
            }

            return parsedContent;
        }

        public static void ChangeTemplateContent(StringBuilder contentBuilder, PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string includeUrl)
        {
            string contentHtml = TemplateDesignManager.ParseImgElements(contentBuilder.ToString(), publishmentSystemInfo, templateInfo.TemplateID, includeUrl);
            contentBuilder.Clear().Append(contentHtml);

            string templateContent = contentBuilder.ToString();

            List<int> indexDivListOriginal = new List<int>();
            List<int> indexDivListNow = new List<int>();
            List<int> indexStlList = new List<int>();
            List<string> stlElementList = new List<string>();

            Regex r = new Regex("<div\\s+[^><]*>|<div>", RegexOptions.IgnoreCase);
            MatchCollection mc = r.Matches(templateContent);
            for (int i = 0; i < mc.Count; i++) //在输入字符串中找到所有匹配
            {
                indexDivListOriginal.Add(mc[i].Index);
            }

            Regex reg = new Regex(@"<stl:(\w+?)[^>]*>", RegexOptions.IgnoreCase);
            mc = reg.Matches(templateContent);
            for (int i = 0; i < mc.Count; i++)
            {
                indexStlList.Add(mc[i].Index);
                stlElementList.Add(mc[i].Value);
            }

            int addedLength = 0;
            for (int i = 0; i < indexStlList.Count; i++)
            {
                int stlIndex = indexStlList[i];
                string stlElement = stlElementList[i];
                string addedString = string.Format(@" stl-index=""{0}""", stlIndex);
                int length = stlElement.IndexOf('>');
                contentBuilder.Insert(stlIndex + length + addedLength, addedString);
                addedLength += addedString.Length;
            }

            mc = r.Matches(contentBuilder.ToString());
            for (int i = 0; i < mc.Count; i++) //在输入字符串中找到所有匹配
            {
                indexDivListNow.Add(mc[i].Index);
            }

            addedLength = 0;
            for (int i = 0; i < indexDivListNow.Count; i++)
            {
                int indexOriginal = indexDivListOriginal[i];
                int indexNow = indexDivListNow[i];
                string addedString = string.Format(@" stl-index=""{0}""", indexOriginal);
                contentBuilder.Insert(indexNow + 4 + addedLength, addedString);
                addedLength += addedString.Length;
            }

            if (!string.IsNullOrEmpty(includeUrl))
            {
                string html = CreateCacheManager.FileContent.GetTemplateContent(publishmentSystemInfo, templateInfo);
                ArrayList cssArrayList = RegexUtils.GetOriginalCssHrefs(html);
                StringBuilder cssBuilder = new StringBuilder();
                foreach (string css in cssArrayList)
                {
                    cssBuilder.AppendFormat(@"<link rel=""stylesheet"" type=""text/css"" href=""{0}"" />", css);
                }

                string content = string.Format(@"<html><head><title>包含文件</title><meta charset=""{0}"">{1}</head><body>{2}</body></html>", ECharsetUtils.GetValue(templateInfo.Charset), cssBuilder, contentBuilder);

                contentBuilder.Length = 0;
                contentBuilder.Append(content);
            }
        }

        public static void ChangeResponseHtml(StringBuilder contentBuilder, PublishmentSystemInfo publishmentSystemInfo)
        {
            string topHtml = CreateCacheManager.FileContent.GetContentByFilePath(PathUtils.GetSiteFilesPath(PageUtils.Combine(SiteFiles.DaD.Templates.Directory, "include/top.html")), ECharset.utf_8);
            contentBuilder.Replace("<!-- siteserver top placeholder -->", topHtml);

            string leftHtml = CreateCacheManager.FileContent.GetContentByFilePath(PathUtils.GetSiteFilesPath(PageUtils.Combine(SiteFiles.DaD.Templates.Directory, "include/left.html")), ECharset.utf_8);
            contentBuilder.Replace("<!-- siteserver left placeholder -->", leftHtml);

            string jsHtml = CreateCacheManager.FileContent.GetContentByFilePath(PathUtils.GetSiteFilesPath(PageUtils.Combine(SiteFiles.DaD.Templates.Directory, "include/js.html")), ECharset.utf_8);
            contentBuilder.Replace("<!-- siteserver js placeholder -->", jsHtml);
        }

        public static void RegistDesignScript(PageInfo pageInfo, string includeUrl)
        {
            string pageJsName = "SiteServer.CMS.Core.TemplateDesignManager";
            if (pageInfo.IsPageScriptsExists(pageJsName)) return;

            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);

            StringBuilder builder = new StringBuilder();

            if (pageInfo.PublishmentSystemInfo.Additional.DesignMode == EDesignMode.Edit)
            {
                string templatUrl = string.Format("{0}&publishmentSystemID={1}&templateID={2}&includeUrl={3}", BackgroundServiceSTL.GetRedirectUrl(BackgroundServiceSTL.TYPE_StlTemplate), pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl);
                string absoluteUrl = PageUtils.AddProtocolToUrl(PageUtils.AddQueryString(pageInfo.PublishmentSystemInfo.PublishmentSystemUrl.TrimEnd('/'), "_r", StringUtils.GetRandomInt(100, 10000).ToString()));

                builder.Append(@"<script type=""text/javascript"">");

                builder.AppendFormat(@"
$pageInfo.stlTemplateUrl = '{0}';
$pageInfo.stlAbsoluteUrl = '{1}';
$pageInfo.stlPublishmentSystemType = '{2}';
", templatUrl, absoluteUrl, EPublishmentSystemTypeUtils.GetValue(pageInfo.PublishmentSystemInfo.PublishmentSystemType)).AppendLine();

                //string openWindowHTML = CreateCacheManager.FileContent.GetContentByFilePath(PathUtils.GetSiteFilesPath(PageUtils.Combine(SiteFiles.DaD.Templates.Directory, "openWindow.html")), ECharset.utf_8);
                //builder.Append(openWindowHTML).AppendLine();

                string dynamicPageUrl = PageUtility.DynamicPage.GetRedirectUrl(pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.TemplateInfo.TemplateType == ETemplateType.FileTemplate ? pageInfo.TemplateInfo.TemplateID : 0, 0);

                //top parameters
                string topUrlPreview = PageUtility.DynamicPage.GetDesignUrl(dynamicPageUrl, includeUrl, EDesignMode.Preview);
                string topModalEdit = StlTemplateEdit.GetOpenLayerStringToTemplate(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl);
                string topModalRestore = SiteServer.STL.BackgroundPages.Modal.TemplateRestore.GetOpenLayerString(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl, true);
                string topModalCreate = SiteServer.CMS.BackgroundPages.Modal.ProgressBar.GetOpenWindowStringWithCreateByTemplate(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID);
                string topModalSelect = StlTemplateSelect.GetOpenLayerString(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateType, pageInfo.TemplateInfo.TemplateID, true);
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, pageInfo.PageNodeID);
                string topTextSelect = pageInfo.TemplateInfo.TemplateName;
                if (!string.IsNullOrEmpty(includeUrl))
                {
                    topTextSelect = RuntimeUtils.DecryptStringByTranslate(includeUrl);
                }
                TemplateDesignUndoRedo undoRedo = TemplateDesignUndoRedo.GetInstance(pageInfo.PublishmentSystemInfo, pageInfo.TemplateInfo, includeUrl);
                string topCssUndo = undoRedo.IsUndo ? "spec_top_cutnbtn" : "disabled";
                string topCssRedo = undoRedo.IsRedo ? "spec_top_cutnbtn" : "disabled";
                string topCssSave = undoRedo.IsDirty ? "" : "spec_disabled";
                string topCssGuideLine = pageInfo.PublishmentSystemInfo.Additional.IsDesignGuideLine && pageInfo.PublishmentSystemInfo.Additional.DesignMode == EDesignMode.Edit ? "spec_top_cutnbtn" : string.Empty;

                string textAlert = TemplateDesignManager.GetAlertText(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID);
                string alertScript = string.Empty;
                if (!string.IsNullOrEmpty(textAlert))
                {
                    alertScript = string.Format(@"toastr.success('{0}');", textAlert);
                }
                bool isAdvanced = !FileConfigManager.Instance.IsSaas;

                builder.AppendFormat(@"
$pageInfo.top = {{}};
$pageInfo.top.urlPreview = ""{0}"";
$pageInfo.top.modalEdit = function(){{
    {1}
}};
$pageInfo.top.modalRestore = function(){{
    {2}
}};
$pageInfo.top.modalCreate = function(){{
    {3}
}};
$pageInfo.top.modalSelect = function(){{
    {4}
}};
$pageInfo.top.textSelect = ""{5}"";
$pageInfo.top.cssUndo = ""{6}"";
$pageInfo.top.cssRedo = ""{7}"";
$pageInfo.top.cssSave = ""{8}"";
$pageInfo.top.cssGuideline = ""{9}"";
$pageInfo.top.isAdvanced = {10};
", topUrlPreview, topModalEdit, topModalRestore, topModalCreate, topModalSelect, topTextSelect, topCssUndo, topCssRedo, topCssSave, topCssGuideLine, isAdvanced.ToString().ToLower());

                //left parameters
                builder.Append("$pageInfo.left = {};").AppendLine();
                TemplateDesignManager.GetOpenWindowStringToAdd(builder, pageInfo, includeUrl);

                builder.Append(alertScript);

                builder.Append("</script>");

                pageInfo.AddPageScriptsIfNotExists(pageJsName, builder.ToString());

                string templateHtml = CreateCacheManager.FileContent.GetContentByFilePath(PathUtils.GetSiteFilesPath(PageUtils.Combine(SiteFiles.DaD.Templates.Directory, "template.html")), ECharset.utf_8);

                string[] templates = StringUtils.SplitStringIgnoreCase(templateHtml, "<!-- siteserver loading placeholder -->");
                pageInfo.AddPageScriptsIfNotExists(pageJsName + "_prev", templates[0]);
                pageInfo.AddPageScriptsIfNotExists(pageJsName + "_next", templates[1], false);
            }
            else
            {
                builder.AppendFormat(@"<link href=""{0}/styles/preview.css"" rel=""stylesheet"" type=""text/css"" />", PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.DaD.Directory));

                string previewHtml = CreateCacheManager.FileContent.GetContentByFilePath(PathUtils.GetSiteFilesPath(PageUtils.Combine(SiteFiles.DaD.Templates.Directory, "preview.html")), ECharset.utf_8);
                string dynamicPageUrl = PageUtility.DynamicPage.GetRedirectUrl(pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.TemplateInfo.TemplateType == ETemplateType.FileTemplate ? pageInfo.TemplateInfo.TemplateID : 0, 0);
                string urlEdit = PageUtility.DynamicPage.GetDesignUrl(dynamicPageUrl, includeUrl, EDesignMode.Edit);
                previewHtml = previewHtml.Replace("__URL_EDIT__", urlEdit);

                builder.Append(previewHtml);

                pageInfo.AddPageScriptsIfNotExists(pageJsName, builder.ToString());
            }
        }

        private static void GetOpenWindowStringToAdd(StringBuilder builder, PageInfo pageInfo, string includeUrl)
        {
            builder.AppendFormat(@"$pageInfo.left.modalAudio = ""{0}"";", StlElementAudio.GetOpenLayerStringToAdd(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl)).AppendLine();
            builder.AppendFormat(@"$pageInfo.left.modalChannels = ""{0}"";", StlElementChannels.GetOpenLayerStringToAdd(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl)).AppendLine();
            builder.AppendFormat(@"$pageInfo.left.modalContent = ""{0}"";", StlElementContent.GetOpenLayerStringToAdd(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, pageInfo.PageNodeID, includeUrl)).AppendLine();
            builder.AppendFormat(@"$pageInfo.left.modalContents = ""{0}"";", StlElementContents.GetOpenLayerStringToAdd(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl)).AppendLine();
            builder.AppendFormat(@"$pageInfo.left.modalFile = ""{0}"";", StlElementFile.GetOpenLayerStringToAdd(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, pageInfo.PageNodeID, includeUrl)).AppendLine();
            builder.AppendFormat(@"$pageInfo.left.modalFocusViewer = ""{0}"";", StlElementFocusViewer.GetOpenLayerStringToAdd(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl)).AppendLine();
            builder.AppendFormat(@"$pageInfo.left.modalImage = ""{0}"";", StlElementImage.GetOpenLayerStringToAdd(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl)).AppendLine();
            builder.AppendFormat(@"$pageInfo.left.modalInclude = ""{0}"";", StlElementInclude.GetOpenLayerStringToAdd(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl)).AppendLine();
            builder.AppendFormat(@"$pageInfo.left.modalLayout = ""{0}"";", StlElementLayout.GetOpenLayerStringToAdd(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl)).AppendLine();
            builder.AppendFormat(@"$pageInfo.left.modalPlayer = ""{0}"";", StlElementPlayer.GetOpenLayerStringToAdd(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl)).AppendLine();
            builder.AppendFormat(@"$pageInfo.left.modalStlInsert = ""{0}"";", StlTemplateEdit.GetOpenLayerStringToAdd(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl)).AppendLine();
        }

        private static Dictionary<string, string> GetOpenWindowStringDictionary(string stlElementName, string stlElement, PageInfo pageInfo, string includeUrl, int stlIndex, string stlEncryptElement)
        {
            Dictionary<string, string> dictionary = new Dictionary<string,string>();

            if (StringUtils.EqualsIgnoreCase(stlElementName, StlAudio.ElementName))
            {
                string settingUrl = StlElementAudio.GetOpenLayerStringToEdit(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl, stlIndex, stlEncryptElement);
                dictionary.Add(settingUrl, "设置");
            }
            else if (StringUtils.EqualsIgnoreCase(stlElementName, StlChannels.ElementName))
            {
                string editUrl = StlEasyData.GetOpenWindowStringByStlChannels(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl, stlIndex, stlEncryptElement);
                dictionary.Add(editUrl, "编辑");

                string settingUrl = StlElementChannels.GetOpenLayerStringToEdit(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl, stlIndex, stlEncryptElement);
                dictionary.Add(settingUrl, "设置");
            }
            else if (StringUtils.EqualsIgnoreCase(stlElementName, StlContent.ElementName))
            {
                string settingUrl = StlElementContent.GetOpenLayerStringToEdit(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, pageInfo.PageNodeID, includeUrl, stlIndex, stlEncryptElement);
                dictionary.Add(settingUrl, "设置");
            }
            else if (StringUtils.EqualsIgnoreCase(stlElementName, StlContents.ElementName))
            {
                string editUrl = StlEasyData.GetOpenWindowStringByStlContents(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl, stlIndex, stlEncryptElement);
                dictionary.Add(editUrl, "编辑");

                string settingUrl = StlElementContents.GetOpenLayerStringToEdit(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl, stlIndex, stlEncryptElement);
                dictionary.Add(settingUrl, "设置");
            }
            else if (StringUtils.EqualsIgnoreCase(stlElementName, StlFile.ElementName))
            {
                string settingUrl = StlElementFile.GetOpenLayerStringToEdit(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, pageInfo.PageNodeID, includeUrl, stlIndex, stlEncryptElement);
                dictionary.Add(settingUrl, "设置");
            }
            else if (StringUtils.EqualsIgnoreCase(stlElementName, StlFocusViewer.ElementName))
            {
                string settingUrl = StlElementFocusViewer.GetOpenLayerStringToEdit(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl, stlIndex, stlEncryptElement);
                dictionary.Add(settingUrl, "设置");
            }
            else if (StringUtils.EqualsIgnoreCase(stlElementName, StlImage.ElementName))
            {
                string src = StlParserUtility.GetAttribute(stlElement, StlImage.Attribute_Src);
                if (!string.IsNullOrEmpty(src))
                {
                    string cuttingUrl = StlCuttingImage.GetOpenWindowStringToImageUrl(pageInfo.PublishmentSystemID, src);
                    dictionary.Add(cuttingUrl, "裁切/旋转");
                }

                string settingUrl = StlElementImage.GetOpenLayerStringToEdit(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl, stlIndex, stlEncryptElement);
                dictionary.Add(settingUrl, "设置");
            }
            else if (StringUtils.EqualsIgnoreCase(stlElementName, StlInclude.ElementName))
            {
                string file = StlParserUtility.GetAttribute(stlElement, StlInclude.Attribute_File);
                if (!string.IsNullOrEmpty(file))
                {
                    string designUrl = PageUtility.DynamicPage.GetDesignUrl(PageUtility.DynamicPage.GetRedirectUrl(pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.PageContentID, pageInfo.TemplateInfo.TemplateType == ETemplateType.FileTemplate ? pageInfo.TemplateInfo.TemplateID : 0, 0), file, EDesignMode.Edit);
                    dictionary.Add(designUrl, "可视化");
                }

                string settingUrl = StlElementInclude.GetOpenLayerStringToEdit(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl, stlIndex, stlEncryptElement);
                dictionary.Add(settingUrl, "设置");
            }
            else if (StringUtils.EqualsIgnoreCase(stlElementName, StlLayout.ElementName))
            {
                string settingUrl = StlElementLayout.GetOpenLayerStringToEdit(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl, stlIndex, stlEncryptElement);
                dictionary.Add(settingUrl, "设置");
            }
            else if (StringUtils.EqualsIgnoreCase(stlElementName, StlPlayer.ElementName))
            {
                string settingUrl = StlElementPlayer.GetOpenLayerStringToEdit(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl, stlIndex, stlEncryptElement);
                dictionary.Add(settingUrl, "设置");
            }

            return dictionary;
        }

        public static string GetDesignHtml(PageInfo pageInfo, string includeUrl, int stlSequence, int stlIndex, string stlElement, string parsedContent, bool isInnerElement)
        {
            if (pageInfo.PublishmentSystemInfo.Additional.DesignMode == EDesignMode.Edit)
            {
                string stlEncryptElement = RuntimeUtils.EncryptStringByTranslate(stlElement);
                string stlElementName = RegexUtils.GetTagName(stlElement);

                StringBuilder commandBuilder = new StringBuilder();

                Dictionary<string, string> openWindows = TemplateDesignManager.GetOpenWindowStringDictionary(stlElementName, stlElement, pageInfo, includeUrl, stlIndex, stlEncryptElement);
                if (openWindows.Count > 0)
                {
                    foreach (var val in openWindows)
                    {
                        if (val.Key.Contains("/modal_"))
                        {
                            commandBuilder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", val.Key, val.Value);
                        }
                        else
                        {
                            commandBuilder.AppendFormat(@"<a href=""{0}"" target=""_blank"">{1}</a>", val.Key, val.Value);
                        }
                    }
                }

                string clickStringOfTemplateEdit = StlTemplateEdit.GetOpenLayerStringToEdit(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl, stlSequence);
                commandBuilder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">代码</a>", clickStringOfTemplateEdit);

                string clickStringOfDelete = StlTemplateMessage.GetOpenLayerStringToDeleteStlElement(pageInfo.PublishmentSystemID, pageInfo.TemplateInfo.TemplateID, includeUrl, stlSequence);
                commandBuilder.AppendFormat(@"<a href=""javascript:;"" onclick=""{0}"">删除</a>", clickStringOfDelete);

//                return string.Format(@"
//<div stl-sequence=""{0}"" stl-index=""{1}"" class=""stl-wrapper stl-element stl-element-{2}"">
//
//<div class=""btn-group stl-command"">
//    <a class=""btn btn-mini btn-inverse stl-drag"">拖动</a>
//    <button class=""btn btn-mini btn-inverse dropdown-toggle"" data-toggle=""dropdown""><span class=""caret""></span></button>
//    <ul class=""dropdown-menu pull-right"">{3}</ul>
//</div>
//
//{4}
//</div>
//", stlSequence, stlIndex, stlElementName.Replace("stl:", string.Empty).ToLower(), commandBuilder, parsedContent);

                string clazz = string.Format("stl-wrapper stl-element stl-element-tips stl-element-{0}", stlElementName.Replace("stl:", string.Empty).ToLower());
                return TemplateDesignUtility.AddClassAndAttribute(parsedContent, clazz, "tipHtml", HtmlAttributesEncode(commandBuilder.ToString()), false);
            }
            else
            {
                return parsedContent;
            }
        }

        private static string HtmlAttributesEncode(string attributes)
        {
            if (!string.IsNullOrEmpty(attributes))
            {
                attributes = attributes.Replace("<", "_lt_");
                attributes = attributes.Replace(">", "_gt_");
                attributes = attributes.Replace("&", "_amp_");
                attributes = attributes.Replace("\"", "_dq_");
                attributes = attributes.Replace("'", "_sq_");
                attributes = attributes.Replace("=", "_eq_");
                attributes = attributes.Replace("\n", "_n_");
                attributes = attributes.Replace("\r", "_r_");
            }
            return attributes;
        }

        public static string GetAlertText(int publishmentSystemID, int templateID)
        {
            string cacheKey = string.Format("SiteServer.CMS.Core.TemplateDesignManager.AlertText.{0}.{1}", publishmentSystemID, templateID);
            return DbCacheManager.GetAndRemove(cacheKey);
        }

        public static void SetAlertText(int publishmentSystemID, int templateID, string alertText)
        {
            string cacheKey = string.Format("SiteServer.CMS.Core.TemplateDesignManager.AlertText.{0}.{1}", publishmentSystemID, templateID);
            DbCacheManager.Insert(cacheKey, alertText);
        }
    }
}
