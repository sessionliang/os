using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using SiteServer.B2C.Model;
using System;
using System.Text;
using System.Collections.Generic;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser.ListTemplate;
using SiteServer.B2C.Core;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlSpecs
    {
        public const string ElementName = "stl:specs";//商品规格列表

        public const string Attribute_DefaultClass = "defaultclass";
        public const string Attribute_SelectedClass = "selectedclass";
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

                attributes.Add(Attribute_DefaultClass, "默认Css类");
                attributes.Add(Attribute_SelectedClass, "选中Css类");
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
                ContentsDisplayInfo displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.Content);

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

            pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_SPEC);

            int specID = 0;
            if (contextInfo.ItemContainer != null && contextInfo.ItemContainer.SpecItem != null)
            {
                specID = TranslateUtils.EvalInt(contextInfo.ItemContainer.SpecItem.DataItem, "SpecID");
            }

            if (displayInfo.Layout == ELayout.None)
            {
                Repeater rptContents = new Repeater();

                rptContents.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Spec, contextInfo);
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
                    rptContents.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Spec, contextInfo);
                }

                rptContents.DataSource = StlDataUtility.GetSpecsDataSource(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID, contextInfo.ContentID, specID, displayInfo.StartNum, displayInfo.TotalNum);
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

                pdlContents.ItemTemplate = new DataListTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Spec, contextInfo);
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
                    pdlContents.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Spec, contextInfo);
                }

                pdlContents.DataSource = StlDataUtility.GetSpecsDataSource(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID, contextInfo.ContentID, specID, displayInfo.StartNum, displayInfo.TotalNum);
                pdlContents.DataBind();

                if (pdlContents.Items.Count > 0)
                {
                    parsedContent = ControlUtils.GetControlRenderHtml(pdlContents);
                }
            }

            string defaultClass = displayInfo.OtherAttributes[StlSpecs.Attribute_DefaultClass];
            string selectedClass = displayInfo.OtherAttributes[StlSpecs.Attribute_SelectedClass];
            if (!string.IsNullOrEmpty(defaultClass) && !string.IsNullOrEmpty(selectedClass))
            {
                string scripts = string.Format(@"
<script language=""javascript"">
$(document).ready(function() {{
    $("".{0}"").click(function(){{
	    $(this).siblings().removeClass(""{1}"");
	    $(this).addClass(""{1}"");
    }});
}});
</script>
", defaultClass, selectedClass);
                pageInfo.AddPageEndScriptsIfNotExists(StlSpecs.ElementName, scripts);
            }

            return parsedContent;
        }
    }
}
