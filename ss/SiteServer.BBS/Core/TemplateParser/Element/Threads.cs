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
    public class Threads
    {
        public const string ElementName = "bbs:threads";//内容列表

        public const string Attribute_ForumIndex = "forumindex";			//板块索引
        public const string Attribute_ForumName = "forumname";				//板块名称
        public const string Attribute_UpLevel = "uplevel";						//上级栏目的级别
        public const string Attribute_TopLevel = "toplevel";					//从首页向下的栏目级别
        public const string Attribute_Scope = "scope";							//内容范围
        public const string Attribute_Group = "group";		                    //指定显示的内容组
        public const string Attribute_GroupNot = "groupnot";	                //指定不显示的内容组
        public const string Attribute_GroupChannel = "groupchannel";		    //指定显示的栏目组
        public const string Attribute_GroupChannelNot = "groupchannelnot";	    //指定不显示的栏目组
        public const string Attribute_GroupContent = "groupcontent";		    //指定显示的内容组
        public const string Attribute_GroupContentNot = "groupcontentnot";	    //指定不显示的内容组
        public const string Attribute_Tags = "tags";	                        //指定标签

        public const string Attribute_IsTop = "istop";                       //仅显示置顶内容
        public const string Attribute_IsRecommend = "isrecommend";           //仅显示推荐内容
        public const string Attribute_IsHot = "ishot";                       //仅显示热点内容
        public const string Attribute_IsColor = "iscolor";                   //仅显示醒目内容

        public const string Attribute_TotalNum = "totalnum";					//显示内容数目
        public const string Attribute_StartNum = "startnum";					//从第几条信息开始显示
        public const string Attribute_Order = "order";						//排序
        public const string Attribute_IsImage = "isimage";					//仅显示图片内容
        public const string Attribute_IsFile = "isfile";                    //仅显示附件内容
        public const string Attribute_IsDisplayIfEmpty = "isdisplayifempty";    //当项为零是是否显示
        public const string Attribute_IsRelatedContents = "isrelatedcontents";    //显示相关内容列表
        public const string Attribute_Where = "where";                      //获取内容列表的条件判断
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        const string Attribute_Columns = "columns";
        const string Attribute_Direction = "direction";
        const string Attribute_Height = "height";
        const string Attribute_Width = "width";
        const string Attribute_Align = "align";
        const string Attribute_ItemHeight = "itemheight";
        const string Attribute_ItemWidth = "itemwidth";
        const string Attribute_ItemAlign = "itemalign";
        const string Attribute_ItemVerticalAlign = "itemverticalalign";
        const string Attribute_ItemClass = "itemclass";
        const string Attribute_Layout = "layout";

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
                attributes.Add(Attribute_IsImage, "仅显示图片内容");
                attributes.Add(Attribute_IsFile, "仅显示附件内容");
                attributes.Add(Attribute_IsDisplayIfEmpty, "当项为零是是否显示");
                attributes.Add(Attribute_IsRelatedContents, "显示相关内容列表");
                attributes.Add(Attribute_Where, "获取内容列表的条件判断");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");

                attributes.Add(Attribute_ForumIndex, "板块索引");
                attributes.Add(Attribute_ForumName, "板块名称");
                attributes.Add(Attribute_UpLevel, "上级栏目的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的栏目级别");
                attributes.Add(Attribute_Scope, "内容范围");
                attributes.Add(Attribute_GroupChannel, "指定显示的栏目组");
                attributes.Add(Attribute_GroupChannelNot, "指定不显示的栏目组");
                attributes.Add(Attribute_GroupContent, "指定显示的内容组");
                attributes.Add(Attribute_GroupContentNot, "指定不显示的内容组");
                attributes.Add(Attribute_Tags, "指定标签");
                attributes.Add(Attribute_IsTop, "仅显示置顶内容");
                attributes.Add(Attribute_IsRecommend, "仅显示推荐内容");
                attributes.Add(Attribute_IsHot, "仅显示热点内容");
                attributes.Add(Attribute_IsColor, "仅显示醒目内容");

                return attributes;
            }
        }

        public static string Parse(string element, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                ContentsDisplayInfo displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.Thread);

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

            int forumID = DataUtility.GetForumIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ForumID, displayInfo.UpLevel, displayInfo.TopLevel);
            forumID = DataUtility.GetForumIDByForumIndexOrForumName(pageInfo.PublishmentSystemID, forumID, displayInfo.ForumIndex, displayInfo.ForumName);

            if (displayInfo.TotalNum == 0)
            {
                ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(pageInfo.PublishmentSystemID);

                displayInfo.TotalNum = additional.ThreadPageNum;
            }

            if (displayInfo.Layout == ELayout.None)
            {
                Repeater MyRepeater = new Repeater();

                MyRepeater.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Thread, contextInfo);
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
                    MyRepeater.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Thread, contextInfo);
                }

                MyRepeater.DataSource = DataUtility.GetThreadsDataSource(pageInfo.PublishmentSystemID, forumID, 0, string.Empty, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.IsTopExists, displayInfo.IsTop, displayInfo.Where, displayInfo.OrderByString, displayInfo.IsAllChildren);
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

                MyDataList.ItemTemplate = new DataListTemplate(displayInfo.ItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Thread, contextInfo);
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
                    MyDataList.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Thread, contextInfo);
                }

                MyDataList.DataSource = DataUtility.GetThreadsDataSource(pageInfo.PublishmentSystemID, forumID, 0, string.Empty, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.IsTopExists, displayInfo.IsTop, displayInfo.Where, displayInfo.OrderByString, displayInfo.IsAllChildren);
                MyDataList.DataKeyField = ThreadAttribute.ID;
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
