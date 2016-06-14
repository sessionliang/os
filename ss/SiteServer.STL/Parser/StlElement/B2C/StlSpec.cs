using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.B2C.Model;
using System;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Core;
using System.Text;
using SiteServer.CMS.Model;
using SiteServer.B2C.Core;
using System.Collections.Generic;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlSpec
    {
        private StlSpec() { }
        public const string ElementName = "stl:spec";//��Ʒ���

        public const string Attribute_Type = "type";						//��ʾ������
        public const string Attribute_LeftText = "lefttext";                    //��ʾ����Ϣǰ������
        public const string Attribute_RightText = "righttext";                  //��ʾ����Ϣ�������
        public const string Attribute_FormatString = "formatstring";        //��ʾ�ĸ�ʽ
        public const string Attribute_Separator = "separator";              //��ʾ����ʱ�ķָ��ַ���
        public const string Attribute_StartIndex = "startindex";			//�ַ���ʼλ��
        public const string Attribute_Length = "length";			        //ָ���ַ�����
        public const string Attribute_WordNum = "wordnum";					//��ʾ�ַ�����Ŀ
        public const string Attribute_Ellipsis = "ellipsis";                //���ֳ���������ʾ������
        public const string Attribute_Replace = "replace";                      //��Ҫ�滻�����֣�������������ʽ
        public const string Attribute_To = "to";                                //�滻replace��������Ϣ
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

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();

                    if (attributeName.Equals(StlSpec.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_LeftText))
                    {
                        leftText = attr.Value;
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_RightText))
                    {
                        rightText = attr.Value;
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_FormatString))
                    {
                        formatString = attr.Value;
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_Separator))
                    {
                        separator = attr.Value;
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_StartIndex))
                    {
                        startIndex = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_Length))
                    {
                        length = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_Ellipsis))
                    {
                        ellipsis = attr.Value;
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_Replace))
                    {
                        replace = attr.Value;
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_To))
                    {
                        to = attr.Value;
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_IsClearTags))
                    {
                        isClearTags = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_IsReturnToBR))
                    {
                        isReturnToBR = TranslateUtils.ToBool(attr.Value, false);
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_IsLower))
                    {
                        isLower = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_IsUpper))
                    {
                        isUpper = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlSpec.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
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
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, attributes, leftText, rightText, formatString, separator, startIndex, length, wordNum, ellipsis, replace, to, isClearTags, isReturnToBR, isLower, isUpper, type);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, StringDictionary attributes, string leftText, string rightText, string formatString, string separator, int startIndex, int length, int wordNum, string ellipsis, string replace, string to, bool isClearTags, bool isReturnToBR, bool isLower, bool isUpper, string type)
        {
            string parsedContent = string.Empty;

            pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_SPEC);
            if (!pageInfo.IsPageEndScriptsExists(PageInfo.JsServiceComponents.C_B2C_SPEC + "_CURRENT"))
            {
                StringBuilder itemBuilder = new StringBuilder();

                GoodsContentInfo contentInfo = contextInfo.ContentInfo as GoodsContentInfo;
                if (contentInfo == null)
                {
                    contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(pageInfo.PublishmentSystemInfo.AuxiliaryTableForGoods, contextInfo.ContentID);
                }

                List<GoodsInfo> goodsInfoList = DataProviderB2C.GoodsDAO.GetGoodsInfoList(pageInfo.PublishmentSystemID, contextInfo.ContentID);

                foreach (GoodsInfo goodsInfo in goodsInfoList)
                {
                    if (goodsInfo.IsOnSale)
                    {
                        decimal priceMarket = goodsInfo.PriceMarket;
                        if (priceMarket < 0)
                        {
                            priceMarket = contentInfo.PriceMarket;
                        }
                        decimal priceSale = goodsInfo.PriceSale;
                        if (priceSale < 0)
                        {
                            priceSale = contentInfo.PriceSale;
                        }
                        decimal priceSaved = priceMarket - priceSale;
                        if (priceSaved < 0)
                        {
                            priceSaved = 0;
                        }

                        itemBuilder.AppendFormat(@"{{goodsID:{0}, itemIDCollection:'{1}', priceMarket:{2}, priceSale:{3}, priceSaved:{4}, stock:{5}}},", goodsInfo.GoodsID, goodsInfo.SpecItemIDCollection, priceMarket, priceSale, priceSaved, goodsInfo.Stock);
                    }
                }
                if (itemBuilder.Length > 0) itemBuilder.Length -= 1;

                string scripts = string.Format(@"
<script language=""javascript"">
  specController.goodsList = [{0}];
</script>
", itemBuilder.ToString());
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_SPEC + "_CURRENT", scripts);
                parsedContent = scripts;
            }

            if (!string.IsNullOrEmpty(type) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.SpecItem != null)
            {
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
                else
                {
                    formatString = "{0}";
                }

                if (string.IsNullOrEmpty(type))
                {
                    type = "imageUrl";
                }

                if (StringUtils.StartsWithIgnoreCase(type, StlParserUtility.ItemIndex))
                {
                    int itemIndex = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.SpecItem.ItemIndex, type, contextInfo);

                    if (!string.IsNullOrEmpty(formatString))
                    {
                        parsedContent = string.Format(formatString, itemIndex);
                    }
                    else
                    {
                        parsedContent = itemIndex.ToString();
                    }
                }
                else if (StringUtils.StartsWithIgnoreCase(type, "previewUrl") || StringUtils.StartsWithIgnoreCase(type, "imageUrl"))
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.SpecItem.DataItem, "IconUrl", formatString);

                    parsedContent = InputParserUtility.GetImageOrFlashHtml(pageInfo.PublishmentSystemInfo, parsedContent, attributes, false);
                }
                else if (StringUtils.StartsWithIgnoreCase(type, "description") || StringUtils.StartsWithIgnoreCase(type, "content"))
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.SpecItem.DataItem, "description", formatString);

                    parsedContent = StringUtils.ReplaceNewlineToBR(parsedContent);
                }
                else if (StringUtils.StartsWithIgnoreCase(type, "title"))
                {
                    int specID = TranslateUtils.EvalInt(contextInfo.ItemContainer.SpecItem.DataItem, "specID");
                    int itemID = TranslateUtils.EvalInt(contextInfo.ItemContainer.SpecItem.DataItem, "itemID");
                    string clazz = contextInfo.ItemContainer.SpecItem.ItemIndex == 0 ? "firstSpecItem" : string.Empty;
                    string title = DataBinder.Eval(contextInfo.ItemContainer.SpecItem.DataItem, "title", formatString);

                    parsedContent = string.Format(@"<span specID=""{0}"" itemID=""{1}"" class=""{2}"">{3}</span>", specID, itemID, clazz, title);
                }
                else
                {
                    parsedContent = DataBinder.Eval(contextInfo.ItemContainer.SpecItem.DataItem, type, formatString);
                }
            }

            if (!string.IsNullOrEmpty(parsedContent))
            {
                parsedContent = StringUtils.ParseString(parsedContent, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBR, isLower, isUpper, formatString);

                if (!string.IsNullOrEmpty(parsedContent))
                {
                    parsedContent = leftText + parsedContent + rightText;
                }
            }

            return parsedContent;
        }
    }
}
