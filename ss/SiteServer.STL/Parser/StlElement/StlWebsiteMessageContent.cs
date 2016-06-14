using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Text;
using SiteServer.STL.StlTemplate;
using SiteServer.STL.Parser.ListTemplate;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlWebsiteMessageContent
    {
        private StlWebsiteMessageContent() { }
        public const string ElementName = "stl:websitemessagecontent";               //�ύ����ֵ
        public const string SimpleElementName = "stl:wsmcontent";

        public const string Attribute_ID = "id";						//��ʾ����վ��������

        public const string Attribute_Type = "type";						//��ʾ������
        public const string Attribute_LeftText = "lefttext";                //��ʾ����Ϣǰ������
        public const string Attribute_RightText = "righttext";              //��ʾ����Ϣ�������
        public const string Attribute_FormatString = "formatstring";        //��ʾ�ĸ�ʽ
        public const string Attribute_Separator = "separator";              //��ʾ����ʱ�ķָ��ַ���
        public const string Attribute_StartIndex = "startindex";			//�ַ���ʼλ��
        public const string Attribute_Length = "length";                    //ָ���ַ�����
        public const string Attribute_WordNum = "wordnum";					//��ʾ�ַ�����Ŀ
        public const string Attribute_Ellipsis = "ellipsis";                //���ֳ���������ʾ������
        public const string Attribute_Replace = "replace";                  //��Ҫ�滻�����֣�������������ʽ
        public const string Attribute_To = "to";                            //�滻replace��������Ϣ
        public const string Attribute_IsClearTags = "iscleartags";          //�Ƿ������ǩ��Ϣ
        public const string Attribute_IsReturnToBR = "isreturntobr";        //�Ƿ񽫻س��滻ΪHTML���б�ǩ
        public const string Attribute_IsLower = "islower";			        //ת��ΪСд
        public const string Attribute_IsUpper = "isupper";			        //ת��Ϊ��д
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_ID, "��ʾ����վ��������ID");
                attributes.Add(Attribute_Type, "��ʾ������");
                attributes.Add(Attribute_LeftText, "��ʾ����Ϣǰ������");
                attributes.Add(Attribute_RightText, "��ʾ����Ϣ�������");
                attributes.Add(Attribute_FormatString, "��ʾ�ĸ�ʽ");
                attributes.Add(Attribute_Separator, "��ʾ����ʱ�ķָ��ַ���");
                attributes.Add(Attribute_StartIndex, "�ַ���ʼλ��");
                attributes.Add(Attribute_Length, "ָ���ַ�����");
                attributes.Add(Attribute_WordNum, "��ʾ�ַ�����Ŀ");
                attributes.Add(Attribute_Ellipsis, "���ֳ���������ʾ������");
                attributes.Add(Attribute_Replace, "��Ҫ�滻�����֣�������������ʽ");
                attributes.Add(Attribute_To, "�滻replace��������Ϣ");
                attributes.Add(Attribute_IsClearTags, "�Ƿ������ǩ��Ϣ");
                attributes.Add(Attribute_IsReturnToBR, "�Ƿ񽫻س��滻ΪHTML���б�ǩ");
                attributes.Add(Attribute_IsLower, "ת��ΪСд");
                attributes.Add(Attribute_IsUpper, "ת��Ϊ��д");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
                return attributes;
            }
        }


        //�ԡ��������ԡ���stl:comment��Ԫ�ؽ��н���
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
                string separator = null;
                int startIndex = 0;
                int length = 0;
                int wordNum = 0;
                string ellipsis = StringUtils.Constants.Ellipsis;
                string replace = string.Empty;
                string to = string.Empty;
                bool isClearTags = false;
                bool isReturnToBR = false;
                bool isLower = false;
                bool isUpper = false;
                string type = string.Empty;
                bool isDynamic = false;
                int id = 0;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlWebsiteMessageContent.Attribute_ID))
                    {
                        id = TranslateUtils.ToInt(attr.Value.ToLower());
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_Type))
                    {
                        type = attr.Value.ToLower();
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_LeftText))
                    {
                        leftText = attr.Value;
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_RightText))
                    {
                        rightText = attr.Value;
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_FormatString))
                    {
                        formatString = attr.Value;
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_Separator))
                    {
                        separator = attr.Value;
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_StartIndex))
                    {
                        startIndex = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_Length))
                    {
                        length = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_Ellipsis))
                    {
                        ellipsis = attr.Value;
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_Replace))
                    {
                        replace = attr.Value;
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_To))
                    {
                        to = attr.Value;
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_IsClearTags))
                    {
                        isClearTags = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_IsReturnToBR))
                    {
                        isReturnToBR = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_IsLower))
                    {
                        isLower = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_IsUpper))
                    {
                        isUpper = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlWebsiteMessageContent.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                    else
                    {
                        attributes.Add(attributeName, attr.Value);
                    }
                }

                if (!string.IsNullOrEmpty(type) && id > 0 && (contextInfo.ItemContainer == null || contextInfo.ItemContainer.WebsiteMessageItem == null)) return string.Empty;

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, attributes, leftText, rightText, formatString, separator, startIndex, length, wordNum, ellipsis, replace, to, isClearTags, isReturnToBR, isLower, isUpper, type, id);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, StringDictionary attributes, string leftText, string rightText, string formatString, string separator, int startIndex, int length, int wordNum, string ellipsis, string replace, string to, bool isClearTags, bool isReturnToBR, bool isLower, bool isUpper, string type, int id)
        {
            string parsedContent = string.Empty;

            if (string.IsNullOrEmpty(type))
            {
                WebsiteMessageInfo websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo("Default", pageInfo.PublishmentSystemID);
                WebsiteMessageTemplate websiteMessageTemplate = new WebsiteMessageTemplate(pageInfo.PublishmentSystemInfo, websiteMessageInfo);
                WebsiteMessageContentInfo contentInfo = DataProvider.WebsiteMessageContentDAO.GetContentInfo(id);
                DbItemInfo itemInfo = new DbItemInfo(contentInfo, 0);
                pageInfo.WebsiteMessageItems.Push(itemInfo);
                contextInfo.ItemContainer = DbItemContainer.GetItemContainer(pageInfo);

                ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, pageInfo.PublishmentSystemID, websiteMessageInfo.WebsiteMessageID);

                string innerHtml = websiteMessageTemplate.GetDetailContentTemplate(websiteMessageInfo.IsTemplateDetail, websiteMessageInfo, relatedIdentities);
                StringBuilder builder = new StringBuilder(innerHtml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);
                DbItemContainer.PopWebsiteMessageItem(pageInfo);
                parsedContent = builder.ToString();
                return parsedContent;
            }


            if (WebsiteMessageContentAttribute.ID.ToLower().Equals(type))
            {
                parsedContent = TranslateUtils.EvalString(contextInfo.ItemContainer.WebsiteMessageItem.DataItem, WebsiteMessageContentAttribute.ID);
            }
            else if (WebsiteMessageContentAttribute.AddDate.ToLower().Equals(type))
            {
                DateTime addDate = TranslateUtils.EvalDateTime(contextInfo.ItemContainer.WebsiteMessageItem.DataItem, WebsiteMessageContentAttribute.AddDate);
                parsedContent = DateUtils.Format(addDate, formatString);
            }
            else if (WebsiteMessageContentAttribute.UserName.ToLower().Equals(type))
            {
                parsedContent = TranslateUtils.EvalString(contextInfo.ItemContainer.WebsiteMessageItem.DataItem, WebsiteMessageContentAttribute.UserName);
                if (string.IsNullOrEmpty(parsedContent))
                    parsedContent = "�����û�";
            }
            else if (WebsiteMessageContentAttribute.Reply.ToLower().Equals(type))
            {
                string content = TranslateUtils.EvalString(contextInfo.ItemContainer.WebsiteMessageItem.DataItem, WebsiteMessageContentAttribute.Reply);
                parsedContent = TranslateUtils.ParseEmotionHtml(content, false);
                parsedContent = StringUtils.ParseString(EInputType.TextEditor, parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                if (string.IsNullOrEmpty(parsedContent))
                    parsedContent = "<span style='color:gray;'>��δ�ظ�</span>";
            }
            else if (StringUtils.StartsWithIgnoreCase(type, StlParserUtility.ItemIndex))
            {
                parsedContent = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.WebsiteMessageItem.ItemIndex, type, contextInfo).ToString();
            }
            else
            {
                id = TranslateUtils.EvalInt(contextInfo.ItemContainer.WebsiteMessageItem.DataItem, WebsiteMessageContentAttribute.ID);
                WebsiteMessageContentInfo websiteMessageContentInfo = DataProvider.WebsiteMessageContentDAO.GetContentInfo(id);
                if (websiteMessageContentInfo != null)
                {
                    parsedContent = websiteMessageContentInfo.GetExtendedAttribute(type);
                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        if (!WebsiteMessageContentAttribute.HiddenAttributes.Contains(type.ToLower()))
                        {
                            TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, type, RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, pageInfo.PublishmentSystemID, websiteMessageContentInfo.WebsiteMessageID));
                            parsedContent = InputTypeParser.GetContentByTableStyle(parsedContent, separator, pageInfo.PublishmentSystemInfo, ETableStyle.WebsiteMessageContent, styleInfo, formatString, attributes, node.InnerXml, false);
                            parsedContent = StringUtils.ParseString(styleInfo.InputType, parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(parsedContent))
            {
                parsedContent = leftText + parsedContent + rightText;
            }

            return parsedContent;
        }
    }
}
