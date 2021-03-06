using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using SiteServer.STL.Parser.ListTemplate;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlSites
    {
        public const string ElementName = "stl:sites";//获取应用列表

        public const string Attribute_SiteName = "sitename";				//应用名称
        public const string Attribute_Directory = "directory";				//应用文件夹

        public const string Attribute_TotalNum = "totalnum";				//显示内容数目
        public const string Attribute_StartNum = "startnum";				//从第几条信息开始显示
        public const string Attribute_Where = "where";                      //获取应用列表的条件判断
        public const string Attribute_Scope = "scope";						//范围
        public const string Attribute_Order = "order";						//排序
        public const string Attribute_Since = "since";				        //时间段
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
                attributes.Add(Attribute_SiteName, "应用名称");
                attributes.Add(Attribute_Directory, "应用文件夹");

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
                attributes.Add(Attribute_Where, "获取应用列表的条件判断");
                attributes.Add(Attribute_Scope, "范围");
                attributes.Add(Attribute_Order, "排序");
                attributes.Add(Attribute_Since, "时间段");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                return attributes;
            }
        }

        //public sealed class SitesItem
        //{
        //    public const string ElementName = "stl:sitesitem";               //列表项

        //    public const string Attribute_Type = "type";                        //列表项类型
        //    public const string Attribute_Selected = "selected";                //当前选定项类型
        //    public const string Attribute_SelectedValue = "selectedvalue";      //当前选定项的值

        //    public const string Type_Header = "header";                 //为项提供头部内容
        //    public const string Type_Footer = "footer";                 //为项提供底部内容
        //    public const string Type_Item = "item";                             //为项提供内容和布局
        //    public const string Type_AlternatingItem = "alternatingitem";       //为交替项提供内容和布局
        //    public const string Type_SelectedItem = "selecteditem";             //为当前选定项提供内容和布局
        //    public const string Type_Separator = "separator";                   //为各项之间的分隔符提供内容和布局

        //    public static ListDictionary AttributeList
        //    {
        //        get
        //        {
        //            ListDictionary attributes = new ListDictionary();

        //            attributes.Add(Attribute_Type, "列表项类型");
        //            attributes.Add(Attribute_Selected, "列表当前选定项类型");
        //            attributes.Add(Attribute_SelectedValue, "列表当前选定项的值");
        //            return attributes;
        //        }
        //    }
        //}

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                ContentsDisplayInfo displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.Site);

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

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, ContentsDisplayInfo displayInfo)
        {
            string parsedContent = string.Empty;

            contextInfo.TitleWordNum = 0;

            string siteName = displayInfo.OtherAttributes[StlSites.Attribute_SiteName];
            string directory = displayInfo.OtherAttributes[StlSites.Attribute_Directory];
            string since = displayInfo.OtherAttributes[StlSites.Attribute_Since];

            if (displayInfo.Layout == ELayout.None)
            {
                Repeater rptContents = new Repeater();

                rptContents.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Site, contextInfo);
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
                    rptContents.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Site, contextInfo);
                }

                rptContents.DataSource = StlDataUtility.GetSitesDataSource(siteName, directory, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.Where, displayInfo.Scope, displayInfo.OrderByString, since);
                rptContents.DataBind();

                if (rptContents.Items.Count > 0)
                {
                    parsedContent = ControlUtils.GetControlRenderHtml(rptContents);
                }
            }
            else
            {
                ParsedDataList pdlContents = new ParsedDataList();

                TemplateUtility.PutContentsDisplayInfoToMyDataList(pdlContents, displayInfo);

                pdlContents.ItemTemplate = new DataListTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Site, contextInfo);
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
                    pdlContents.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Site, contextInfo);
                }

                pdlContents.DataSource = StlDataUtility.GetSitesDataSource(siteName, directory, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.Where, displayInfo.Scope, displayInfo.OrderByString, since);
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
