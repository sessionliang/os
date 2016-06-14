using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlContent
    {
        private StlContent() { }
        public const string ElementName = "stl:content";//内容值

        public const string Attribute_Type = "type";						//显示的类型
        public const string Attribute_LeftText = "lefttext";                //显示在信息前的文字
        public const string Attribute_RightText = "righttext";              //显示在信息后的文字
        public const string Attribute_FormatString = "formatstring";        //显示的格式
        public const string Attribute_NO = "no";                            //显示第几项
        public const string Attribute_Separator = "separator";              //显示多项时的分割字符串
        public const string Attribute_StartIndex = "startindex";			//字符开始位置
        public const string Attribute_Length = "length";			        //指定字符长度
        public const string Attribute_WordNum = "wordnum";					//显示字符的数目
        public const string Attribute_Ellipsis = "ellipsis";                //文字超出部分显示的文字
        public const string Attribute_Replace = "replace";                  //需要替换的文字，可以是正则表达式
        public const string Attribute_To = "to";                            //替换replace的文字信息
        public const string Attribute_IsClearTags = "iscleartags";          //是否清除标签信息
        public const string Attribute_IsReturnToBR = "isreturntobr";        //是否将回车替换为HTML换行标签
        public const string Attribute_IsLower = "islower";			        //转换为小写
        public const string Attribute_IsUpper = "isupper";			        //转换为大写
        public const string Attribute_IsOriginal = "isoriginal";            //如果是引用地址，是否获取所引用内容的值
        //public const string Attribute_IsEditable = "iseditable";            //是否可在前台编辑
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示
        public const string Attribute_ImageType = "imagetype";                        //图片类型

        public const string Attribute_ImageType_Original = "original";        //原始图片
        public const string Attribute_ImageType_Compression = "compression";  //压缩图片
        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Type, "显示的类型");
                attributes.Add(Attribute_LeftText, "显示在信息前的文字");
                attributes.Add(Attribute_RightText, "显示在信息后的文字");
                attributes.Add(Attribute_FormatString, "显示的格式");
                attributes.Add(Attribute_NO, "显示第几项");
                attributes.Add(Attribute_Separator, "显示多项时的分割字符串");
                attributes.Add(Attribute_StartIndex, "字符开始位置");
                attributes.Add(Attribute_Length, "指定字符长度");
                attributes.Add(Attribute_WordNum, "显示字符的数目");
                attributes.Add(Attribute_Ellipsis, "文字超出部分显示的文字");
                attributes.Add(Attribute_Replace, "需要替换的文字，可以是正则表达式");
                attributes.Add(Attribute_To, "替换replace的文字信息");
                attributes.Add(Attribute_IsClearTags, "是否清除标签信息");
                attributes.Add(Attribute_IsReturnToBR, "是否将回车替换为HTML换行标签");
                attributes.Add(Attribute_IsLower, "转换为小写");
                attributes.Add(Attribute_IsUpper, "转换为大写");
                attributes.Add(Attribute_IsOriginal, "如果是引用内容，是否获取所引用内容的值");
                //attributes.Add(Attribute_IsEditable, "是否可在前台编辑");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                attributes.Add(Attribute_ImageType, "图片类型：缩略图/原图");
                return attributes;
            }
        }

        //对“内容属性”（stl:content）元素进行解析
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();
                StringDictionary attributes = new StringDictionary();
                string leftText = string.Empty;
                string rightText = string.Empty;
                string formatString = string.Empty;
                string no = "0";
                string separator = null;
                int startIndex = 0;
                int length = 0;
                int wordNum = 0;
                string ellipsis = StringUtils.Constants.Ellipsis;
                string replace = string.Empty;
                string to = string.Empty;
                bool isClearTags = false;
                string isReturnToBRStr = string.Empty;
                bool isLower = false;
                bool isUpper = false;
                bool isOriginal = true;//引用的时候，默认使用原来的数据
                //bool isEditable = true;
                bool isDynamic = false;
                string imageType = string.Empty;
                string type = ContentAttribute.Title.ToLower();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlContent.Attribute_Type))
                    {
                        type = attr.Value.ToLower();
                    }
                    else if (attributeName.Equals(StlContent.Attribute_LeftText))
                    {
                        leftText = attr.Value;
                    }
                    else if (attributeName.Equals(StlContent.Attribute_RightText))
                    {
                        rightText = attr.Value;
                    }
                    else if (attributeName.Equals(StlContent.Attribute_FormatString))
                    {
                        formatString = attr.Value;
                    }
                    else if (attributeName.Equals(StlContent.Attribute_NO))
                    {
                        no = attr.Value;
                    }
                    else if (attributeName.Equals(StlContent.Attribute_Separator))
                    {
                        separator = attr.Value;
                    }
                    else if (attributeName.Equals(StlContent.Attribute_StartIndex))
                    {
                        startIndex = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlContent.Attribute_Length))
                    {
                        length = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlContent.Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlContent.Attribute_Ellipsis))
                    {
                        ellipsis = attr.Value;
                    }
                    else if (attributeName.Equals(StlContent.Attribute_Replace))
                    {
                        replace = attr.Value;
                    }
                    else if (attributeName.Equals(StlContent.Attribute_To))
                    {
                        to = attr.Value;
                    }
                    else if (attributeName.Equals(StlContent.Attribute_IsClearTags))
                    {
                        isClearTags = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlContent.Attribute_IsReturnToBR))
                    {
                        isReturnToBRStr = attr.Value;
                    }
                    else if (attributeName.Equals(StlContent.Attribute_IsLower))
                    {
                        isLower = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlContent.Attribute_IsUpper))
                    {
                        isUpper = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlContent.Attribute_IsOriginal))
                    {
                        isOriginal = TranslateUtils.ToBool(attr.Value, true);
                    }
                    //else if (attributeName.Equals(StlContent.Attribute_IsEditable))
                    //{
                    //    isEditable = TranslateUtils.ToBool(attr.Value, true);
                    //}
                    else if (attributeName.Equals(StlContent.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlContent.Attribute_ImageType))
                    {
                        imageType = attr.Value;
                    }
                    else
                    {
                        attributes.Add(attributeName, attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(stlElement, node, pageInfo, contextInfo, leftText, rightText, formatString, no, separator, startIndex, length, wordNum, ellipsis, replace, to, isClearTags, isReturnToBRStr, isLower, isUpper, isOriginal, type, attributes, imageType);

                    StringBuilder innerBuilder = new StringBuilder(parsedContent);
                    StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                    parsedContent = innerBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, string leftText, string rightText, string formatString, string no, string separator, int startIndex, int length, int wordNum, string ellipsis, string replace, string to, bool isClearTags, string isReturnToBRStr, bool isLower, bool isUpper, bool isOriginal, string type, StringDictionary attributes, string imageType)
        {
            string parsedContent = string.Empty;

            bool isReturnToBR = false;
            if (string.IsNullOrEmpty(isReturnToBRStr))
            {
                if (BackgroundContentAttribute.Summary.ToLower().Equals(type))
                {
                    isReturnToBR = true;
                }
            }
            else
            {
                isReturnToBR = TranslateUtils.ToBool(isReturnToBRStr, true);
            }

            int contentID = contextInfo.ContentID;
            ContentInfo contentInfo = contextInfo.ContentInfo;

            if (isOriginal)
            {
                if (contentInfo != null && contentInfo.ReferenceID > 0 && contentInfo.SourceID > 0 && contentInfo.GetExtendedAttribute(ContentAttribute.TranslateContentType) == ETranslateContentType.Reference.ToString())
                {
                    int targetNodeID = contentInfo.SourceID;
                    int targetPublishmentSystemID = DataProvider.NodeDAO.GetPublishmentSystemID(targetNodeID);
                    PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);
                    NodeInfo targetNodeInfo = NodeManager.GetNodeInfo(targetPublishmentSystemID, targetNodeID);

                    ETableStyle tableStyle = NodeManager.GetTableStyle(targetPublishmentSystemInfo, targetNodeInfo);
                    string tableName = NodeManager.GetTableName(targetPublishmentSystemInfo, targetNodeInfo);
                    ContentInfo targetContentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentInfo.ReferenceID);
                    if (targetContentInfo != null || targetContentInfo.NodeID > 0)
                    {
                        //标题可以使用自己的
                        targetContentInfo.Title = contentInfo.Title;

                        contentInfo = targetContentInfo;
                    }
                }
            }

            if (!string.IsNullOrEmpty(formatString))
            {
                formatString = formatString.Trim();
                if (!formatString.StartsWith("{0"))
                {
                    formatString = "{0:" + formatString;
                }
                if (!formatString.EndsWith("}"))
                {
                    formatString = formatString + "}";
                }
            }

            if (contentID != 0)
            {
                if (ContentAttribute.Title.ToLower().Equals(type))
                {
                    ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(pageInfo.PublishmentSystemID, contentInfo.NodeID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contentInfo.NodeID);
                    ETableStyle tableStyle = NodeManager.GetTableStyle(pageInfo.PublishmentSystemInfo, nodeInfo);
                    string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, nodeInfo);

                    TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(tableStyle, tableName, type, relatedIdentities);
                    if (wordNum == 0)
                    {
                        wordNum = contextInfo.TitleWordNum;
                    }
                    parsedContent = InputTypeParser.GetContentByTableStyle(contentInfo.Title, separator, pageInfo.PublishmentSystemInfo, tableStyle, styleInfo, formatString, attributes, node.InnerXml, false);
                    parsedContent = StringUtils.ParseString(styleInfo.InputType, parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);

                    if (!isClearTags && !string.IsNullOrEmpty(contentInfo.Attributes[BackgroundContentAttribute.TitleFormatString]))
                    {
                        parsedContent = ContentUtility.FormatTitle(contentInfo.Attributes[BackgroundContentAttribute.TitleFormatString], parsedContent);
                    }

                    if (!contextInfo.IsInnerElement)
                    {
                        parsedContent = parsedContent.Replace("&", "&amp;");
                    }

                    if (pageInfo.PublishmentSystemInfo.Additional.IsContentTitleBreakLine)
                    {
                        if (!contextInfo.IsInnerElement)
                        {
                            parsedContent = parsedContent.Replace("  ", "<br />");
                        }
                        else
                        {
                            parsedContent = parsedContent.Replace("  ", string.Empty);
                        }
                    }
                }
                else if (BackgroundContentAttribute.Summary.ToLower().Equals(type))
                {
                    parsedContent = StringUtils.ParseString(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Summary), replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                    if (!contextInfo.IsInnerElement)
                    {
                        parsedContent = parsedContent.Replace("&", "&amp;");
                    }
                }
                else if (BackgroundContentAttribute.Content.ToLower().Equals(type))
                {
                    parsedContent = StringUtility.TextEditorContentDecode(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content), pageInfo.PublishmentSystemInfo);

                    if (pageInfo.PublishmentSystemInfo.Additional.IsInnerLink)
                    {
                        ArrayList innerLinkInfoArrayList = pageInfo.CacheOfInnerLinkInfoArrayList;
                        if (innerLinkInfoArrayList != null && innerLinkInfoArrayList.Count > 0)
                        {
                            InnerLinkInfo newInnerLinkInfo;
                            for (int i = 0; i < innerLinkInfoArrayList.Count - 1; i++)
                            {
                                for (int j = i + 1; j < innerLinkInfoArrayList.Count; j++)
                                {

                                    if (((SiteServer.CMS.Model.InnerLinkInfo)(innerLinkInfoArrayList[i])).InnerLinkName.Length < ((SiteServer.CMS.Model.InnerLinkInfo)(innerLinkInfoArrayList[j])).InnerLinkName.Length)
                                    {
                                        newInnerLinkInfo = ((SiteServer.CMS.Model.InnerLinkInfo)(innerLinkInfoArrayList[i]));
                                        innerLinkInfoArrayList[i] = innerLinkInfoArrayList[j];
                                        innerLinkInfoArrayList[j] = newInnerLinkInfo;
                                    }
                                }
                            }

                            ArrayList arrayLinkName = new ArrayList();
                            ArrayList arrayInnerLink = new ArrayList();
                            for (int i = 0; i < innerLinkInfoArrayList.Count; i++)
                            {
                                newInnerLinkInfo = ((SiteServer.CMS.Model.InnerLinkInfo)(innerLinkInfoArrayList[i]));
                                arrayLinkName.Add(newInnerLinkInfo.InnerLinkName);
                                arrayInnerLink.Add(newInnerLinkInfo.InnerString);

                            }
                            for (int m = 0; m < arrayLinkName.Count; m++)
                            {
                                string innerLinkName = arrayLinkName[m].ToString();
                                arrayLinkName[m] = Guid.NewGuid().ToString();
                                parsedContent = RegexUtils.Replace(string.Format("({0})(?!</a>)(?![^><]*>)", innerLinkName.Replace(" ", "\\s")), parsedContent, arrayLinkName[m].ToString(), pageInfo.PublishmentSystemInfo.Additional.InnerLinkMaxNum);

                            }
                            for (int n = 0; n < arrayLinkName.Count; n++)
                            {
                                parsedContent = RegexUtils.Replace(string.Format("({0})(?!</a>)(?![^><]*>)", arrayLinkName[n].ToString().Replace(" ", "\\s")), parsedContent, arrayInnerLink[n].ToString(), pageInfo.PublishmentSystemInfo.Additional.InnerLinkMaxNum);
                            }
                        }
                    }

                    if (isClearTags)
                    {
                        parsedContent = StringUtils.StripTags(parsedContent);
                    }

                    if (!string.IsNullOrEmpty(replace))
                    {
                        parsedContent = StringUtils.Replace(replace, parsedContent, to);
                    }

                    if (wordNum > 0 && !string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = StringUtils.MaxLengthText(parsedContent, wordNum, ellipsis);
                    }

                    if (!string.IsNullOrEmpty(formatString))
                    {
                        parsedContent = string.Format(formatString, parsedContent);
                    }

                    if (!contextInfo.IsInnerElement)
                    {
                        parsedContent = parsedContent.Replace("&", "&amp;");
                    }
                }
                else if (BackgroundContentAttribute.PageContent.ToLower().Equals(type))
                {
                    //if (contextInfo.IsInnerElement)
                    // {
                    parsedContent = StringUtility.TextEditorContentDecode(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content), pageInfo.PublishmentSystemInfo);


                    if (pageInfo.PublishmentSystemInfo.Additional.IsInnerLink)
                    {
                        ArrayList innerLinkInfoArrayList = pageInfo.CacheOfInnerLinkInfoArrayList;
                        if (innerLinkInfoArrayList != null && innerLinkInfoArrayList.Count > 0)
                        {
                            InnerLinkInfo newInnerLinkInfo;
                            for (int i = 0; i < innerLinkInfoArrayList.Count - 1; i++)
                            {
                                for (int j = i + 1; j < innerLinkInfoArrayList.Count; j++)
                                {

                                    if (((SiteServer.CMS.Model.InnerLinkInfo)(innerLinkInfoArrayList[i])).InnerLinkName.Length < ((SiteServer.CMS.Model.InnerLinkInfo)(innerLinkInfoArrayList[j])).InnerLinkName.Length)
                                    {
                                        newInnerLinkInfo = ((SiteServer.CMS.Model.InnerLinkInfo)(innerLinkInfoArrayList[i]));
                                        innerLinkInfoArrayList[i] = innerLinkInfoArrayList[j];
                                        innerLinkInfoArrayList[j] = newInnerLinkInfo;
                                    }
                                }
                            }
                            //foreach (InnerLinkInfo innerLinkInfo in innerLinkInfoArrayList)
                            //{
                            //    parsedContent = RegexUtils.Replace(string.Format("({0})(?!</a>)(?![^><]*>)", innerLinkInfo.InnerLinkName.Replace(" ", "\\s")), parsedContent, innerLinkInfo.InnerString, pageInfo.PublishmentSystemInfo.Additional.InnerLinkMaxNum);

                            //}
                            for (int i = 0; i < innerLinkInfoArrayList.Count; i++)
                            {
                                newInnerLinkInfo = ((SiteServer.CMS.Model.InnerLinkInfo)(innerLinkInfoArrayList[i]));
                                for (int j = i + 1; j < innerLinkInfoArrayList.Count; j++)
                                {
                                    InnerLinkInfo lastInnerLinkInfo = ((SiteServer.CMS.Model.InnerLinkInfo)(innerLinkInfoArrayList[j]));
                                    if (newInnerLinkInfo.InnerLinkName.Contains(lastInnerLinkInfo.InnerLinkName))
                                    {
                                        innerLinkInfoArrayList.Remove(lastInnerLinkInfo);
                                    }
                                }
                                parsedContent = RegexUtils.Replace(string.Format("({0})(?!</a>)(?![^><]*>)", newInnerLinkInfo.InnerLinkName.Replace(" ", "\\s")), parsedContent, newInnerLinkInfo.InnerString, pageInfo.PublishmentSystemInfo.Additional.InnerLinkMaxNum);
                            }
                        }
                    }

                    if (isClearTags)
                    {
                        parsedContent = StringUtils.StripTags(parsedContent);
                    }

                    if (!string.IsNullOrEmpty(replace))
                    {
                        parsedContent = StringUtils.Replace(replace, parsedContent, to);
                    }

                    if (wordNum > 0 && !string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = StringUtils.MaxLengthText(parsedContent, wordNum, ellipsis);
                    }

                    if (!string.IsNullOrEmpty(formatString))
                    {
                        parsedContent = string.Format(formatString, parsedContent);
                    }
                    //}
                    // else
                    //{
                    //    return stlElement;
                    // }
                }
                else if (ContentAttribute.AddDate.ToLower().Equals(type))
                {
                    parsedContent = DateUtils.Format(contentInfo.AddDate, formatString);
                }
                else if (ContentAttribute.LastEditDate.ToLower().Equals(type))
                {
                    parsedContent = DateUtils.Format(contentInfo.LastEditDate, formatString);
                }
                else if (BackgroundContentAttribute.ImageUrl.ToLower().Equals(type))
                {
                    if (no == "all")
                    {
                        StringBuilder sbParsedContent = new StringBuilder();
                        //第一条
                        if (contextInfo.IsCurlyBrace)
                        {
                            sbParsedContent.Append(PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, GetImageUrlByType(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl), imageType)));
                        }
                        else
                        {
                            sbParsedContent.Append(InputParserUtility.GetImageOrFlashHtml(pageInfo.PublishmentSystemInfo, GetImageUrlByType(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl), imageType), attributes, false));
                        }
                        //第n条
                        string extendAttributeName = ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.ImageUrl);
                        string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                        if (!string.IsNullOrEmpty(extendValues))
                        {
                            foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                            {
                                string newExtendValue = GetImageUrlByType(extendValue, imageType);
                                if (contextInfo.IsCurlyBrace)
                                {
                                    sbParsedContent.Append(PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, newExtendValue));
                                }
                                else
                                {
                                    sbParsedContent.Append(InputParserUtility.GetImageOrFlashHtml(pageInfo.PublishmentSystemInfo, newExtendValue, attributes, false));
                                }
                            }
                        }

                        parsedContent = sbParsedContent.ToString();
                    }
                    else
                    {
                        int num = TranslateUtils.ToInt(no, 0);
                        if (num <= 1)
                        {
                            if (contextInfo.IsCurlyBrace)
                            {
                                parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, GetImageUrlByType(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl), imageType));
                            }
                            else
                            {
                                parsedContent = InputParserUtility.GetImageOrFlashHtml(pageInfo.PublishmentSystemInfo, GetImageUrlByType(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl), imageType), attributes, false);
                            }
                        }
                        else
                        {
                            string extendAttributeName = ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.ImageUrl);
                            string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                            if (!string.IsNullOrEmpty(extendValues))
                            {
                                int index = 2;
                                foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                                {
                                    string newExtendValue = GetImageUrlByType(extendValue, imageType);
                                    if (index == num)
                                    {
                                        if (contextInfo.IsCurlyBrace)
                                        {
                                            parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, newExtendValue);
                                        }
                                        else
                                        {
                                            parsedContent = InputParserUtility.GetImageOrFlashHtml(pageInfo.PublishmentSystemInfo, newExtendValue, attributes, false);
                                        }
                                        break;
                                    }
                                    index++;
                                }
                            }
                        }
                    }
                }
                else if (BackgroundContentAttribute.VideoUrl.ToLower().Equals(type))
                {
                    if (no == "all")
                    {
                        StringBuilder sbParsedContent = new StringBuilder();
                        //第一条
                        sbParsedContent.Append(InputParserUtility.GetVideoHtml(pageInfo.PublishmentSystemInfo, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl), attributes, false));

                        //第n条
                        string extendAttributeName = ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.VideoUrl);
                        string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                        if (!string.IsNullOrEmpty(extendValues))
                        {
                            foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                            {

                                sbParsedContent.Append(InputParserUtility.GetVideoHtml(pageInfo.PublishmentSystemInfo, extendValue, attributes, false));

                            }
                        }

                        parsedContent = sbParsedContent.ToString();
                    }
                    else
                    {
                        int num = TranslateUtils.ToInt(no, 0);
                        if (num <= 1)
                        {
                            parsedContent = InputParserUtility.GetVideoHtml(pageInfo.PublishmentSystemInfo, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl), attributes, false);
                        }
                        else
                        {
                            string extendAttributeName = ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.VideoUrl);
                            string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                            if (!string.IsNullOrEmpty(extendValues))
                            {
                                int index = 2;
                                foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                                {
                                    if (index == num)
                                    {
                                        parsedContent = InputParserUtility.GetVideoHtml(pageInfo.PublishmentSystemInfo, extendValue, attributes, false);
                                        break;
                                    }
                                    index++;
                                }
                            }
                        }
                    }

                }
                else if (BackgroundContentAttribute.FileUrl.ToLower().Equals(type))
                {

                    if (no == "all")
                    {
                        StringBuilder sbParsedContent = new StringBuilder();
                        if (contextInfo.IsCurlyBrace)
                        {
                            //第一条
                            sbParsedContent.Append(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl));
                            //第n条
                            string extendAttributeName = ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.FileUrl);
                            string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                            if (!string.IsNullOrEmpty(extendValues))
                            {
                                foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                                {
                                    sbParsedContent.Append(extendValue);
                                }
                            }
                        }
                        else
                        {
                            //第一条
                            sbParsedContent.Append(InputParserUtility.GetFileHtmlWithCount(pageInfo.PublishmentSystemInfo, contentInfo.NodeID, contentInfo.ID, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl), attributes, node.InnerXml, false));
                            //第n条
                            string extendAttributeName = ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.FileUrl);
                            string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                            if (!string.IsNullOrEmpty(extendValues))
                            {
                                foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                                {
                                    sbParsedContent.Append(InputParserUtility.GetFileHtmlWithCount(pageInfo.PublishmentSystemInfo, contentInfo.NodeID, contentInfo.ID, extendValue, attributes, node.InnerXml, false));
                                }
                            }

                        }

                        parsedContent = sbParsedContent.ToString();

                    }
                    else
                    {
                        int num = TranslateUtils.ToInt(no, 0);
                        if (contextInfo.IsCurlyBrace)
                        {
                            if (num <= 1)
                            {
                                parsedContent = contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl);
                            }
                            else
                            {
                                string extendAttributeName = ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.FileUrl);
                                string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                                if (!string.IsNullOrEmpty(extendValues))
                                {
                                    int index = 2;
                                    foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                                    {
                                        if (index == num)
                                        {
                                            parsedContent = extendValue;
                                            break;
                                        }
                                        index++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (num <= 1)
                            {
                                parsedContent = InputParserUtility.GetFileHtmlWithCount(pageInfo.PublishmentSystemInfo, contentInfo.NodeID, contentInfo.ID, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl), attributes, node.InnerXml, false);
                            }
                            else
                            {
                                string extendAttributeName = ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.FileUrl);
                                string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                                if (!string.IsNullOrEmpty(extendValues))
                                {
                                    int index = 2;
                                    foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                                    {
                                        if (index == num)
                                        {
                                            parsedContent = InputParserUtility.GetFileHtmlWithCount(pageInfo.PublishmentSystemInfo, contentInfo.NodeID, contentInfo.ID, extendValue, attributes, node.InnerXml, false);
                                            break;
                                        }
                                        index++;
                                    }
                                }
                            }
                        }
                    }


                }
                else if (BackgroundContentAttribute.NavigationUrl.ToLower().Equals(type))
                {
                    parsedContent = PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, contentInfo, pageInfo.PublishmentSystemInfo.Additional.VisualType);
                }
                else if (ContentAttribute.Tags.ToLower().Equals(type))
                {
                    parsedContent = contentInfo.Tags;
                }
                else if (StringUtils.StartsWithIgnoreCase(type, StlParserUtility.ItemIndex) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.ContentItem != null)
                {
                    int itemIndex = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.ContentItem.ItemIndex, type, contextInfo);
                    if (!string.IsNullOrEmpty(formatString))
                    {
                        parsedContent = string.Format(formatString, itemIndex);
                    }
                    else
                    {
                        parsedContent = itemIndex.ToString();
                    }
                }
                else if (ContentAttribute.AddUserName.ToLower().Equals(type))
                {
                    if (!string.IsNullOrEmpty(contentInfo.AddUserName))
                    {
                        string displayName = BaiRongDataProvider.AdministratorDAO.GetDisplayName(contentInfo.AddUserName);
                        if (string.IsNullOrEmpty(displayName))
                        {
                            parsedContent = contentInfo.AddUserName;
                        }
                        else
                        {
                            parsedContent = displayName;
                        }
                    }
                }
                else
                {
                    bool isSelected = false;
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contentInfo.NodeID);
                    ETableStyle tableStyle = NodeManager.GetTableStyle(pageInfo.PublishmentSystemInfo, nodeInfo);

                    #region WCM SPECIFIC

                    if (tableStyle == ETableStyle.GovInteractContent)
                    {
                        isSelected = true;
                        if (GovInteractContentAttribute.State.ToLower().Equals(type))
                        {
                            parsedContent = EGovInteractStateUtils.GetText(EGovInteractStateUtils.GetEnumType(contentInfo.GetExtendedAttribute(GovInteractContentAttribute.State)));
                        }
                        else if (StringUtils.EqualsIgnoreCase(type, GovInteractContentAttribute.Reply))
                        {
                            GovInteractReplyInfo replyInfo = DataProvider.GovInteractReplyDAO.GetReplyInfoByContentID(pageInfo.PublishmentSystemID, contentInfo.ID);
                            if (replyInfo != null)
                            {
                                parsedContent = replyInfo.Reply;
                                if (!string.IsNullOrEmpty(parsedContent))
                                {
                                    parsedContent = StringUtils.ParseString(EInputType.TextEditor, parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                                }
                            }
                        }
                        else if (StringUtils.EqualsIgnoreCase(type, GovInteractContentAttribute.ReplyDepartment))
                        {
                            GovInteractReplyInfo replyInfo = DataProvider.GovInteractReplyDAO.GetReplyInfoByContentID(pageInfo.PublishmentSystemID, contentInfo.ID);
                            if (replyInfo != null)
                            {
                                parsedContent = DepartmentManager.GetDepartmentName(replyInfo.DepartmentID);
                            }
                        }
                        else if (StringUtils.EqualsIgnoreCase(type, GovInteractContentAttribute.ReplyUserName))
                        {
                            GovInteractReplyInfo replyInfo = DataProvider.GovInteractReplyDAO.GetReplyInfoByContentID(pageInfo.PublishmentSystemID, contentInfo.ID);
                            if (replyInfo != null)
                            {
                                parsedContent = replyInfo.UserName;
                            }
                        }
                        else if (StringUtils.EqualsIgnoreCase(type, GovInteractContentAttribute.ReplyDate))
                        {
                            GovInteractReplyInfo replyInfo = DataProvider.GovInteractReplyDAO.GetReplyInfoByContentID(pageInfo.PublishmentSystemID, contentInfo.ID);
                            if (replyInfo != null)
                            {
                                DateTime addDate = replyInfo.AddDate;
                                parsedContent = DateUtils.Format(addDate, formatString);
                            }
                        }
                        else if (StringUtils.EqualsIgnoreCase(type, GovInteractContentAttribute.ReplyFileUrl))
                        {
                            GovInteractReplyInfo replyInfo = DataProvider.GovInteractReplyDAO.GetReplyInfoByContentID(pageInfo.PublishmentSystemID, contentInfo.ID);
                            if (replyInfo != null)
                            {
                                parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, replyInfo.FileUrl);
                            }
                        }
                        else
                        {
                            isSelected = false;
                        }
                    }

                    #endregion

                    #region B2C SPECIFIC

                    else if (tableStyle == ETableStyle.GoodsContent)
                    {
                        isSelected = true;
                        if (GoodsContentAttribute.PriceSaved.ToLower().Equals(type))
                        {
                            parsedContent = Convert.ToString(TranslateUtils.ToDecimal(contentInfo.GetExtendedAttribute(GoodsContentAttribute.PriceMarket)) - TranslateUtils.ToDecimal(contentInfo.GetExtendedAttribute(GoodsContentAttribute.PriceSale)));
                        }
                        else
                        {
                            isSelected = false;
                        }
                    }

                    #endregion

                    if (!isSelected && contentInfo.ContainsKey(type))
                    {
                        if (!ContentAttribute.HiddenAttributes.Contains(type.ToLower()))
                        {
                            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(pageInfo.PublishmentSystemID, contentInfo.NodeID);
                            string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, nodeInfo);
                            TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(tableStyle, tableName, type, relatedIdentities);
                            int num = TranslateUtils.ToInt(no);
                            parsedContent = InputTypeParser.GetContentByTableStyle(contentInfo, separator, pageInfo.PublishmentSystemInfo, tableStyle, styleInfo, formatString, num, attributes, node.InnerXml, false);
                            parsedContent = StringUtils.ParseString(styleInfo.InputType, parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                        }
                        else
                        {
                            parsedContent = contentInfo.GetExtendedAttribute(type);
                            parsedContent = StringUtils.ParseString(parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                        }
                    }

                    if (!contextInfo.IsInnerElement)
                    {
                        parsedContent = parsedContent.Replace("&", "&amp;");
                    }
                }

                if (!string.IsNullOrEmpty(parsedContent))
                {
                    parsedContent = leftText + parsedContent + rightText;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(type) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.ContentItem != null)
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.ContentItem.DataItem, type, "{0}");

                    parsedContent = StringUtils.ParseString(parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);

                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = leftText + parsedContent + rightText;
                    }
                }
            }

            return parsedContent;
        }

        public static string GetImageUrlByType(string imageUrl, string imageType)
        {
            string fileName = PathUtils.GetFileName(imageUrl);
            string filePath = DirectoryUtils.GetDirectoryPath(imageUrl);
            string origFile = fileName;//源文件名
            if (fileName.StartsWith(SiteServer.CMS.Core.Constants.TitleImageAppendix))
            {
                //去除前缀
                origFile = fileName.Substring(SiteServer.CMS.Core.Constants.TitleImageAppendix.Length);
            }
            switch (imageType)
            {
                case Attribute_ImageType_Original:
                    return PathUtils.Combine(filePath, origFile);

                case Attribute_ImageType_Compression:
                    return PathUtils.Combine(filePath, SiteServer.CMS.Core.Constants.TitleImageAppendix + origFile);

                default:
                    return imageUrl;
            }
        }
    }
}
