using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.ListTemplate;
using SiteServer.CMS.Model;
using System;
using SiteServer.CMS.Core;
using System.Text;
using SiteServer.STL.StlTemplate;
using System.Collections;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlWebsiteMessageContents
    {
        public const string ElementName = "stl:websitemessagecontents";                  //提交内容列表
        public const string SimpleElementName = "stl:wsmcontents";

        public const string Attribute_WebsiteMessageName = "websitemessagename";                  //提交表单名称
        public const string Attribute_TotalNum = "totalnum";					//显示内容数目
        public const string Attribute_StartNum = "startnum";					//从第几条信息开始显示
        public const string Attribute_Order = "order";						    //排序
        public const string Attribute_IsReply = "isreply";                      //是否仅显示已回复内容
        public const string Attribute_Where = "where";                          //获取内容列表的条件判断
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public const string Attribute_ClassifyID = "classifyid";              //分类ID

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

                attributes.Add(Attribute_WebsiteMessageName, "提交表单名称");
                attributes.Add(Attribute_IsReply, "是否仅显示已回复内容");
                attributes.Add(Attribute_TotalNum, "显示内容数目");
                attributes.Add(Attribute_StartNum, "从第几条信息开始显示");
                attributes.Add(Attribute_Order, "排序");
                attributes.Add(Attribute_Where, "获取内容列表的条件判断");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");

                attributes.Add(Attribute_ClassifyID, "分类ID");

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

                return attributes;
            }
        }

        //public sealed class WebsiteMessageContentsItem
        //{
        //    public const string ElementName = "stl:websiteMessagecontentsitem";               //提交内容列表项

        //    public const string Attribute_Type = "type";                        //提交内容列表项类型
        //    public const string Attribute_Selected = "selected";                //列表当前选定项类型
        //    public const string Attribute_SelectedValue = "selectedvalue";      //列表当前选定项的值

        //    public const string Type_Header = "header";
        //    public const string Type_Footer = "footer";
        //    public const string Type_Item = "item";
        //    public const string Type_AlternatingItem = "alternatingitem";
        //    public const string Type_SelectedItem = "selecteditem";
        //    public const string Type_Separator = "separator";

        //    public static ListDictionary AttributeList
        //    {
        //        get
        //        {
        //            ListDictionary attributes = new ListDictionary();

        //            attributes.Add(Attribute_Type, "提交内容列表项类型");
        //            attributes.Add(Attribute_Selected, "列表当前选定项类型");
        //            attributes.Add(Attribute_SelectedValue, "列表当前选定项值");
        //            return attributes;
        //        }
        //    }
        //}


        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                ContentsDisplayInfo displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.WebsiteMessageContent);

                if (!string.IsNullOrEmpty(displayInfo.OtherAttributes["classifyid"]))
                {
                    int classifyID = TranslateUtils.ToInt(displayInfo.OtherAttributes["classifyid"]);
                    if (classifyID > 0)
                    {
                        if (displayInfo.Where.Length > 0)
                        {
                            displayInfo.Where += string.Format(" and {0}={1} ", WebsiteMessageContentAttribute.ClassifyID, classifyID);
                        }
                        else
                        {
                            displayInfo.Where += string.Format(" {0}={1} ", WebsiteMessageContentAttribute.ClassifyID, classifyID);
                        }
                    }
                }

                if (displayInfo.IsDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, node, displayInfo);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, XmlNode node, ContentsDisplayInfo displayInfo)
        {
            string parsedContent = string.Empty;
            contextInfo.TitleWordNum = 0;

            displayInfo.OtherAttributes[StlWebsiteMessageContents.Attribute_WebsiteMessageName] = "Default";

            int websiteMessageID = DataProvider.WebsiteMessageDAO.GetWebsiteMessageIDAsPossible(displayInfo.OtherAttributes[StlWebsiteMessageContents.Attribute_WebsiteMessageName], pageInfo.PublishmentSystemID);
            WebsiteMessageInfo websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(websiteMessageID);

            string innerHtml = node.InnerXml;
            if (string.IsNullOrEmpty(innerHtml))
            {
                WebsiteMessageTemplate websiteMessageTemplate = new WebsiteMessageTemplate(pageInfo.PublishmentSystemInfo, websiteMessageInfo);
                ArrayList relatedIndentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, pageInfo.PublishmentSystemID, websiteMessageInfo.WebsiteMessageID);
                innerHtml = websiteMessageTemplate.GetListContentTemplate(websiteMessageInfo.IsTemplateList, websiteMessageInfo, relatedIndentities);
                StringBuilder builder = new StringBuilder(innerHtml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);
                parsedContent = builder.ToString();
                return parsedContent;
            }



            if (displayInfo.Layout == ELayout.None)
            {
                Repeater rptContents = new Repeater();

                rptContents.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, null, null, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.WebsiteMessageContent, contextInfo);
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
                    rptContents.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, null, null, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.WebsiteMessageContent, contextInfo);
                }

                rptContents.DataSource = StlDataUtility.GetWebsiteMessageContentsDataSource(pageInfo.PublishmentSystemID, websiteMessageID, displayInfo);
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

                pdlContents.ItemTemplate = new DataListTemplate(displayInfo.ItemTemplate, null, null, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.WebsiteMessageContent, contextInfo);
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
                    pdlContents.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, null, null, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.WebsiteMessageContent, contextInfo);
                }

                pdlContents.DataSource = StlDataUtility.GetWebsiteMessageContentsDataSource(pageInfo.PublishmentSystemID, websiteMessageID, displayInfo);
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
