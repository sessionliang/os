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
    public class Forums
    {
        public const string ElementName = "bbs:forums";//板块列表

        public const string Attribute_ForumIndex = "forumindex";			//板块索引
        public const string Attribute_ForumName = "forumname";				//板块名称
        public const string Attribute_UpLevel = "uplevel";						//上级板块的级别
        public const string Attribute_TopLevel = "toplevel";					//从首页向下的板块级别
        public const string Attribute_IsTotal = "istotal";						//是否从所有板块中选择
        public const string Attribute_IsAllChildren = "isallchildren";			//是否显示所有级别的子板块

        public const string Attribute_Group = "group";		                    //指定显示的板块组
        public const string Attribute_GroupNot = "groupnot";	                //指定不显示的板块组
        public const string Attribute_TotalNum = "totalnum";					//显示板块数目
        public const string Attribute_StartNum = "startnum";					//从第几条信息开始显示
        public const string Attribute_Order = "order";						    //排序
        public const string Attribute_IsTop = "istop";					    //仅显示图片板块
        public const string Attribute_IsDisplayIfEmpty = "isdisplayifempty";    //当项为零是是否显示
        public const string Attribute_Where = "where";                          //获取板块列表的条件判断
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

                attributes.Add(Attribute_TotalNum, "显示板块数目");
                attributes.Add(Attribute_StartNum, "从第几条信息开始显示");
                attributes.Add(Attribute_Order, "排序");
                attributes.Add(Attribute_IsDisplayIfEmpty, "当项为零是是否显示");
                attributes.Add(Attribute_Where, "获取板块列表的条件判断");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");

                attributes.Add(Attribute_ForumIndex, "板块索引");
                attributes.Add(Attribute_ForumName, "板块名称");
                attributes.Add(Attribute_UpLevel, "上级板块的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的板块级别");
                attributes.Add(Attribute_IsTotal, "是否从所有板块中选择");
                attributes.Add(Attribute_IsAllChildren, "是否显示所有级别的子板块");
                return attributes;
            }
        }

        const string Selected_Current = "current";                   //当前板块为选中板块
        const string Selected_Up = "up";                             //当前板块的上级板块为选中板块
        const string Selected_Top = "top";                           //当前板块从首页向下的板块为选中板块

        public static string Parse(string element, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                ContentsDisplayInfo displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.Forum);

                if (displayInfo.IsDynamic)
                {
                    parsedContent = Dynamic.ParseDynamicElement(ElementName, element, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, displayInfo);
                }
            }
            catch (Exception ex)
            {
                parsedContent = ParserUtility.GetErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }


        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, ContentsDisplayInfo displayInfo)
        {
            string parsedContent = string.Empty;

            int forumID = DataUtility.GetForumIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ForumID, displayInfo.UpLevel, displayInfo.TopLevel);
            forumID = DataUtility.GetForumIDByForumIndexOrForumName(pageInfo.PublishmentSystemID, forumID, displayInfo.ForumIndex, displayInfo.ForumName);

            displayInfo.Columns = DataUtility.GetForumColumns(pageInfo.PublishmentSystemID, displayInfo.Columns, forumID);

            if (displayInfo.Columns == 2)
            {
                displayInfo.ItemWidth = Unit.Percentage(50);
            }
            else if (displayInfo.Columns == 3)
            {
                displayInfo.ItemWidth = Unit.Percentage(33.3);
            }
            else if (displayInfo.Columns == 4)
            {
                displayInfo.ItemWidth = Unit.Percentage(25);
            }
            else if (displayInfo.Columns == 5)
            {
                displayInfo.ItemWidth = Unit.Percentage(20);
            }

            //bool isTotal = TranslateUtils.ToBool(displayInfo.OtherAttributes[StlChannels.Attribute_IsTotal]);

            //if (TranslateUtils.ToBool(displayInfo.OtherAttributes[StlChannels.Attribute_IsAllChildren]))
            //{
            //    displayInfo.Scope = EScopeType.Descendant;
            //}

            if (displayInfo.Layout == ELayout.None)
            {
                Repeater MyRepeater = new Repeater();

                MyRepeater.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Forum, contextInfo);
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
                    MyRepeater.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Forum, contextInfo);
                }

                MyRepeater.DataSource = DataUtility.GetForumsDataSource(pageInfo.PublishmentSystemID, forumID, displayInfo.Group, displayInfo.GroupNot, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.OrderByString, displayInfo.Where);
                MyRepeater.DataBind();

                if (displayInfo.IsDisplayIfEmpty || MyRepeater.Items.Count > 0)
                {
                    parsedContent = ControlUtils.GetControlRenderHtml(MyRepeater);
                }
            }
            else
            {
                ParsedDataList MyDataList = new ParsedDataList();

                //设置显示属性
                TemplateUtility.PutContentsDisplayInfoToMyDataList(MyDataList, displayInfo);
                MyDataList.RepeatDirection = RepeatDirection.Vertical;

                //设置列表模板
                MyDataList.ItemTemplate = new DataListTemplate(displayInfo.ItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Forum, contextInfo);
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
                    MyDataList.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Forum, contextInfo);
                }

                MyDataList.DataSource = DataUtility.GetForumsDataSource(pageInfo.PublishmentSystemID, forumID, displayInfo.Group, displayInfo.GroupNot, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.OrderByString, displayInfo.Where);
                MyDataList.DataKeyField = ForumAttribute.ForumID;
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
