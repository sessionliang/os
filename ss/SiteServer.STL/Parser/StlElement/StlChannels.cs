using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.ListTemplate;
using SiteServer.CMS.Model;
using System;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System.Collections;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlChannels
    {
        public const string ElementName = "stl:channels";//栏目列表

        public const string Attribute_ChannelIndex = "channelindex";			//栏目索引
        public const string Attribute_ChannelName = "channelname";				//栏目名称
        public const string Attribute_UpLevel = "uplevel";						//上级栏目的级别
        public const string Attribute_TopLevel = "toplevel";					//从首页向下的栏目级别
        public const string Attribute_IsTotal = "istotal";						//是否从所有栏目中选择（包括首页）
        public const string Attribute_IsAllChildren = "isallchildren";			//是否显示所有级别的子栏目

        public const string Attribute_GroupChannel = "groupchannel";		    //指定显示的栏目组
        public const string Attribute_GroupChannelNot = "groupchannelnot";	    //指定不显示的栏目组
        public const string Attribute_Group = "group";		                    //指定显示的栏目组
        public const string Attribute_GroupNot = "groupnot";	                //指定不显示的栏目组
        public const string Attribute_TotalNum = "totalnum";					//显示栏目数目
        public const string Attribute_StartNum = "startnum";					//从第几条信息开始显示
        public const string Attribute_TitleWordNum = "titlewordnum";			//栏目名称文字数量
        public const string Attribute_Order = "order";						    //排序
        public const string Attribute_IsImage = "isimage";					    //仅显示图片栏目
        public const string Attribute_Where = "where";                          //获取栏目列表的条件判断
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

                attributes.Add(Attribute_GroupChannel, "指定显示的栏目组");
                attributes.Add(Attribute_GroupChannelNot, "指定不显示的栏目组");
                attributes.Add(Attribute_TotalNum, "显示栏目数目");
                attributes.Add(Attribute_StartNum, "从第几条信息开始显示");
                attributes.Add(Attribute_Order, "排序");
                attributes.Add(Attribute_IsImage, "仅显示图片栏目");
                attributes.Add(Attribute_Where, "获取栏目列表的条件判断");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");

                attributes.Add(Attribute_ChannelIndex, "栏目索引");
                attributes.Add(Attribute_ChannelName, "栏目名称");
                attributes.Add(Attribute_UpLevel, "上级栏目的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的栏目级别");
                attributes.Add(Attribute_IsTotal, "是否从所有栏目中选择");
                attributes.Add(Attribute_IsAllChildren, "是否显示所有级别的子栏目");
                return attributes;
            }
        }

        //public sealed class ChannelsItem
        //{
        //    public const string ElementName = "stl:channelsitem";               //栏目列表项

        //    public const string Attribute_Type = "type";                        //列表项类型
        //    public const string Attribute_Selected = "selected";                //列表当前选定项类型
        //    public const string Attribute_SelectedValue = "selectedvalue";      //列表当前选定项的值

        //    public const string Selected_Current = "current";                   //当前栏目为选中栏目
        //    public const string Selected_Image = "image";                       //带图片栏目为选中栏目
        //    public const string Selected_Up = "up";                             //当前栏目的上级栏目为选中栏目
        //    public const string Selected_Top = "top";                           //当前栏目从首页向下的栏目为选中栏目

        //    public const string Type_Header = "header";                 //为 stl:channels 中的项提供头部内容
        //    public const string Type_Footer = "footer";                 //为 stl:channels 中的项提供底部内容
        //    public const string Type_Item = "item";                             //为 stl:channels 中的项提供内容和布局
        //    public const string Type_AlternatingItem = "alternatingitem";       //为 stl:channels 中的交替项提供内容和布局
        //    public const string Type_SelectedItem = "selecteditem";             //为 stl:channels 中当前选定项提供内容和布局
        //    public const string Type_Separator = "separator";                   //为 stl:channels 中各项之间的分隔符提供内容和布局

        //    public static ListDictionary AttributeList
        //    {
        //        get
        //        {
        //            ListDictionary attributes = new ListDictionary();

        //            attributes.Add(Attribute_Type, "栏目列表项类型");
        //            attributes.Add(Attribute_Selected, "栏目列表当前选定项类型");
        //            attributes.Add(Attribute_SelectedValue, "栏目列表当前选定项值");
        //            return attributes;
        //        }
        //    }
        //}


        //对“栏目列表”（stl:channels）元素进行解析
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                ContentsDisplayInfo displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.Channel);

                if (displayInfo.IsDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, displayInfo);
                }

            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        public static IEnumerable GetDataSource(PageInfo pageInfo, ContextInfo contextInfo, ContentsDisplayInfo displayInfo)
        {
            int channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, displayInfo.UpLevel, displayInfo.TopLevel);

            channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, displayInfo.ChannelIndex, displayInfo.ChannelName);

            bool isTotal = TranslateUtils.ToBool(displayInfo.OtherAttributes[StlChannels.Attribute_IsTotal]);

            if (TranslateUtils.ToBool(displayInfo.OtherAttributes[StlChannels.Attribute_IsAllChildren]))
            {
                displayInfo.Scope = EScopeType.Descendant;
            }

            return StlDataUtility.GetChannelsDataSource(pageInfo.PublishmentSystemID, channelID, displayInfo.GroupChannel, displayInfo.GroupChannelNot, displayInfo.IsImageExists, displayInfo.IsImage, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.OrderByString, displayInfo.Scope, isTotal, displayInfo.Where);
        }


        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, ContentsDisplayInfo displayInfo)
        {
            string parsedContent = string.Empty;

            contextInfo.TitleWordNum = displayInfo.TitleWordNum;

            IEnumerable dataSource = StlChannels.GetDataSource(pageInfo, contextInfo, displayInfo);

            if (displayInfo.Layout == ELayout.None)
            {
                Repeater rptContents = new Repeater();

                rptContents.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Channel, contextInfo);
                if (!string.IsNullOrEmpty(displayInfo.HeaderTemplate))
                {
                    rptContents.HeaderTemplate = new SeparatorTemplate(displayInfo.HeaderTemplate);
                }
                if (!string.IsNullOrEmpty(displayInfo.FooterTemplate))
                {
                    rptContents.FooterTemplate = new SeparatorTemplate(displayInfo.FooterTemplate);
                }
                if (!string.IsNullOrEmpty(displayInfo.SeparatorTemplate))
                {
                    rptContents.SeparatorTemplate = new SeparatorTemplate(displayInfo.SeparatorTemplate);
                }
                if (!string.IsNullOrEmpty(displayInfo.AlternatingItemTemplate))
                {
                    rptContents.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Channel, contextInfo);
                }

                rptContents.DataSource = dataSource;
                rptContents.DataBind();

                if (rptContents.Items.Count > 0)
                {
                    parsedContent = ControlUtils.GetControlRenderHtml(rptContents);
                }
            }
            else
            {
                ParsedDataList pdlContents = new ParsedDataList();

                //设置显示属性
                TemplateUtility.PutContentsDisplayInfoToMyDataList(pdlContents, displayInfo);

                //设置列表模板
                pdlContents.ItemTemplate = new DataListTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Channel, contextInfo);
                if (!string.IsNullOrEmpty(displayInfo.HeaderTemplate))
                {
                    pdlContents.HeaderTemplate = new SeparatorTemplate(displayInfo.HeaderTemplate);
                }
                if (!string.IsNullOrEmpty(displayInfo.FooterTemplate))
                {
                    pdlContents.FooterTemplate = new SeparatorTemplate(displayInfo.FooterTemplate);
                }
                if (!string.IsNullOrEmpty(displayInfo.SeparatorTemplate))
                {
                    pdlContents.SeparatorTemplate = new SeparatorTemplate(displayInfo.SeparatorTemplate);
                }
                if (!string.IsNullOrEmpty(displayInfo.AlternatingItemTemplate))
                {
                    pdlContents.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Channel, contextInfo);
                }

                pdlContents.DataSource = dataSource;
                pdlContents.DataKeyField = NodeAttribute.NodeID;
                pdlContents.DataBind();

                if (pdlContents.Items.Count > 0)
                {
                    parsedContent = ControlUtils.GetControlRenderHtml(pdlContents);
                }
            }

            return parsedContent;
        }
    }
}
