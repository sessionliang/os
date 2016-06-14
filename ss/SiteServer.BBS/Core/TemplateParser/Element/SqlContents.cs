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
    public class SqlContents
    {
        public const string ElementName = "bbs:sqlcontents";//获取数据库数据列表

        public const string Attribute_ConnectionStringName = "connectionstringname";        //数据库链接字符串名称
        public const string Attribute_ConnectionString = "connectionstring";	            //数据库链接字符串

        public const string Attribute_Predefined = "predefined";                    //预定义
        public const string Attribute_TotalNum = "totalnum";					            //显示内容数目
        public const string Attribute_StartNum = "startnum";					            //从第几条信息开始显示
        public const string Attribute_Order = "order";						                //排序
        public const string Attribute_IsDisplayIfEmpty = "isdisplayifempty";    //当项为零是是否显示
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

        public const string Predefined_NavHeader = "NavHeader";
        public const string Predefined_NavSecondary = "NavSecondary";
        public const string Predefined_NavFooter = "NavFooter";
        public const string Predefined_Announcement = "Announcement";
        public const string Predefined_LinkIcon = "LinkIcon";
        public const string Predefined_LinkText = "LinkText";

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_ConnectionStringName, "数据库链接字符串名称");
                attributes.Add(Attribute_ConnectionString, "数据库链接字符串");

                attributes.Add(Attribute_Predefined, "预定义");
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
                attributes.Add(Attribute_IsDisplayIfEmpty, "当项为零是是否显示");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                ContentsDisplayInfo displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.SqlContent);

                if (displayInfo.IsDynamic)
                {
                    parsedContent = Dynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
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

            if (displayInfo.Layout == ELayout.None)
            {
                Repeater MyRepeater = new Repeater();

                MyRepeater.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.SqlContent, contextInfo);
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
                    MyRepeater.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.SqlContent, contextInfo);
                }

                MyRepeater.DataSource = DataUtility.GetSqlContentsDataSource(pageInfo.PublishmentSystemID, displayInfo.ConnectionString, displayInfo.QueryString, displayInfo.OtherAttributes[SqlContents.Attribute_Predefined], displayInfo.StartNum, displayInfo.TotalNum, displayInfo.OrderByString);
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

                MyDataList.ItemTemplate = new DataListTemplate(displayInfo.ItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.SqlContent, contextInfo);
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
                    MyDataList.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.SqlContent, contextInfo);
                }

                MyDataList.DataSource = DataUtility.GetSqlContentsDataSource(pageInfo.PublishmentSystemID, displayInfo.ConnectionString, displayInfo.QueryString, displayInfo.OtherAttributes[SqlContents.Attribute_Predefined], displayInfo.StartNum, displayInfo.TotalNum, displayInfo.OrderByString);
                MyDataList.DataBind();

                if (displayInfo.IsDisplayIfEmpty || MyDataList.Items.Count > 0)
                {
                    parsedContent = ControlUtils.GetControlRenderHtml(MyDataList);
                }
            }

            return parsedContent;
        }
    }
}
