using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using System.Collections;
using BaiRong.Core;
using SiteServer.BBS.Core.TemplateParser.Model;
using System.Web.UI.WebControls;
using SiteServer.BBS.Core.TemplateParser.ListTemplate;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Core.TemplateParser.Element
{
    public class Posts
    {
        public const string ElementName = "bbs:posts";//回复列表

        public const string Attribute_TotalNum = "totalnum";					//显示内容数目
        public const string Attribute_StartNum = "startnum";					//从第几条信息开始显示
        public const string Attribute_Order = "order";						//排序
        public const string Attribute_IsThread = "isthread";
        public const string Attribute_IsDisplayIfEmpty = "isdisplayifempty";    //当项为零是是否显示
        public const string Attribute_Where = "where";                      //获取内容列表的条件判断
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public const string Attribute_Columns = "columns";
        public const string Attribute_Direction = "direction";
        public const string Attribute_Height = "height";
        public const string Attribute_Width = "width";
        public const string Attribute_Align = "align";
        public const string Attribute_ItemHeight = "itemheight";
        public const string Attribute_ItemWidth = "itemwidth";
        public const string Attribute_ItemAlign = "itemalign";
        public const string Attribute_ItemVerticalAlign = "itemverticalalign";
        public const string Attribute_ItemClass = "itemclass";
        public const string Attribute_Layout = "layout";

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();

                attributes.Add("cellpadding", "填充");
                attributes.Add("cellspacing", "间距");
                attributes.Add("class", "Css类");
                attributes.Add(Attribute_Columns, "列数");
                attributes.Add(Attribute_Direction, "方向");
                attributes.Add(Attribute_Layout, "指定列表布局方式");

                attributes.Add(Attribute_Height, "整体高度");
                attributes.Add(Attribute_Width, "整体宽度");
                attributes.Add(Attribute_Align, "整体对齐");
                attributes.Add(Attribute_ItemHeight, "项高度");
                attributes.Add(Attribute_ItemWidth, "项宽度");
                attributes.Add(Attribute_ItemAlign, "项水平对齐");
                attributes.Add(Attribute_ItemVerticalAlign, "项垂直对齐");
                attributes.Add(Attribute_ItemClass, "项Css类");

                attributes.Add(Attribute_TotalNum, "显示内容数目");
                attributes.Add(Attribute_StartNum, "从第几条信息开始显示");
                attributes.Add(Attribute_Order, "排序");
                attributes.Add(Attribute_IsThread, "仅显示主题帖");
                attributes.Add(Attribute_IsDisplayIfEmpty, "当项为零是是否显示");
                attributes.Add(Attribute_Where, "获取内容列表的条件判断");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");

                return attributes;
            }
        }

        public static string Parse(string element, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                ContentsDisplayInfo displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.Post);

                if (displayInfo.IsDynamic)
                {
                    parsedContent = Dynamic.ParseDynamicElement(ElementName, element, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, displayInfo);
                }
            }
            catch (Exception ex)
            {
                parsedContent = ParserUtility.GetErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, ContentsDisplayInfo displayInfo)
        {
            string parsedContent = string.Empty;

            bool isThreadExists = !string.IsNullOrEmpty(displayInfo.OtherAttributes[Attribute_IsThread]);
            bool isThread = TranslateUtils.ToBool(displayInfo.OtherAttributes[Attribute_IsThread]);

            if (displayInfo.TotalNum == 0)
            {
                ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(pageInfo.PublishmentSystemID);
                displayInfo.TotalNum = additional.PostPageNum;
            }

            if (contextInfo.ThreadID > 0)
            {
                if (displayInfo.Layout == ELayout.None)
                {
                    Repeater MyRepeater = new Repeater();

                    MyRepeater.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Post, contextInfo);
                    if (!string.IsNullOrEmpty(displayInfo.HeaderTemplate))
                    {
                        MyRepeater.HeaderTemplate = new SeparatorTemplate(displayInfo.HeaderTemplate);
                    }
                    if (!string.IsNullOrEmpty(displayInfo.FooterTemplate))
                    {
                        MyRepeater.FooterTemplate = new SeparatorTemplate(displayInfo.FooterTemplate);
                    }
                    if (!string.IsNullOrEmpty(displayInfo.SeparatorTemplate))
                    {
                        MyRepeater.SeparatorTemplate = new SeparatorTemplate(displayInfo.SeparatorTemplate);
                    }
                    if (!string.IsNullOrEmpty(displayInfo.AlternatingItemTemplate))
                    {
                        MyRepeater.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Post, contextInfo);
                    }

                    MyRepeater.DataSource = DataUtility.GetPostsDataSource(pageInfo.PublishmentSystemID, contextInfo.ThreadID, displayInfo.StartNum, displayInfo.TotalNum, isThreadExists, isThread, displayInfo.OrderByString, displayInfo.Where);
                    MyRepeater.DataBind();

                    if (displayInfo.IsDisplayIfEmpty || MyRepeater.Items.Count > 0)
                    {
                        parsedContent = ControlUtils.GetControlRenderHtml(MyRepeater);
                    }
                }
                else
                {
                    ParsedDataList MyDataList = new ParsedDataList();

                    TemplateUtility.PutContentsDisplayInfoToMyDataList(MyDataList, displayInfo);

                    MyDataList.ItemTemplate = new DataListTemplate(displayInfo.ItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Post, contextInfo);
                    if (!string.IsNullOrEmpty(displayInfo.HeaderTemplate))
                    {
                        MyDataList.HeaderTemplate = new SeparatorTemplate(displayInfo.HeaderTemplate);
                    }
                    if (!string.IsNullOrEmpty(displayInfo.FooterTemplate))
                    {
                        MyDataList.FooterTemplate = new SeparatorTemplate(displayInfo.FooterTemplate);
                    }
                    if (!string.IsNullOrEmpty(displayInfo.SeparatorTemplate))
                    {
                        MyDataList.SeparatorTemplate = new SeparatorTemplate(displayInfo.SeparatorTemplate);
                    }
                    if (!string.IsNullOrEmpty(displayInfo.AlternatingItemTemplate))
                    {
                        MyDataList.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Post, contextInfo);
                    }

                    MyDataList.DataSource = DataUtility.GetPostsDataSource(pageInfo.PublishmentSystemID, contextInfo.ThreadID, displayInfo.StartNum, displayInfo.TotalNum, isThreadExists, isThread, displayInfo.OrderByString, displayInfo.Where);
                    MyDataList.DataKeyField = PostAttribute.ID;
                    MyDataList.DataBind();

                    if (displayInfo.IsDisplayIfEmpty || MyDataList.Items.Count > 0)
                    {
                        parsedContent = ControlUtils.GetControlRenderHtml(MyDataList);
                    }
                }
            }

            return parsedContent;
        }

    }
}
