using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.ListTemplate;
using SiteServer.CMS.Model;
using System.Text;
using System;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;
using System.Collections;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlComments
    {
        public const string ElementName = "stl:comments";//评论列表

        public const string Attribute_IsPage = "ispage";					    //是否需要翻页
        public const string Attribute_PageNum = "pagenum";					    //每页显示的项数

        public const string Attribute_IsRecommend = "isrecommend";				//是否显示推荐内容

        public const string Attribute_TotalNum = "totalnum";					//显示内容数目
        public const string Attribute_StartNum = "startnum";					//从第几条信息开始显示
        public const string Attribute_Order = "order";						//排序
        public const string Attribute_Where = "where";                      //获取内容列表的条件判断

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
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();

                attributes.Add(Attribute_IsPage, "是否需要翻页");
                attributes.Add(Attribute_PageNum, "每页显示的评论数目");

                attributes.Add(Attribute_IsRecommend, "是否显示推荐内容");

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
                attributes.Add(Attribute_Where, "获取内容列表的条件判断");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                ContentsDisplayInfo displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.Comment);

                if (string.IsNullOrEmpty(node.InnerXml))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC);
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.B_CMS_COMMENT);

                    string filePath = PathUtils.GetSiteFilesPath("services/cms/components/comment/commentsTemplate.html");
                    string innerHtml = FileUtils.ReadText(filePath, ECharset.utf_8);
                    innerHtml = StlParserUtility.HtmlToXml(innerHtml);

                    StringBuilder builder = new StringBuilder(innerHtml);
                    StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                    parsedContent = string.Format(@"
<script type=""text/html"" class=""commentController"">
    {0}
</script>
<script>
    var commentController = commentController || {{}};
    commentController.showNum = {1};
</script>
", builder, displayInfo.TotalNum);
                }
                else
                {
                    if (displayInfo.IsDynamic)
                    {
                        parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                    }
                    else
                    {
                        parsedContent = ParseImpl(pageInfo, contextInfo, displayInfo);
                    }
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

            if (displayInfo.Layout == ELayout.None)
            {
                Repeater rptContents = new Repeater();

                rptContents.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, null, null, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Comment, contextInfo);

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
                    rptContents.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, null, null, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Comment, contextInfo);
                }

                rptContents.DataSource = StlDataUtility.GetCommentsDataSource(pageInfo.PublishmentSystemID, contextInfo.ChannelID, contextInfo.ContentID, contextInfo.ItemContainer, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.IsRecommend, displayInfo.OrderByString, displayInfo.Where);

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

                pdlContents.ItemTemplate = new DataListTemplate(displayInfo.ItemTemplate, null, null, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Comment, contextInfo);
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
                    pdlContents.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, null, null, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Comment, contextInfo);
                }

                pdlContents.DataSource = StlDataUtility.GetCommentsDataSource(pageInfo.PublishmentSystemID, contextInfo.ChannelID, contextInfo.ContentID, contextInfo.ItemContainer, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.IsRecommend, displayInfo.OrderByString, displayInfo.Where);
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
