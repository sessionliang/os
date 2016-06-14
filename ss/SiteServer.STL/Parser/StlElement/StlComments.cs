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
        public const string ElementName = "stl:comments";//�����б�

        public const string Attribute_IsPage = "ispage";					    //�Ƿ���Ҫ��ҳ
        public const string Attribute_PageNum = "pagenum";					    //ÿҳ��ʾ������

        public const string Attribute_IsRecommend = "isrecommend";				//�Ƿ���ʾ�Ƽ�����

        public const string Attribute_TotalNum = "totalnum";					//��ʾ������Ŀ
        public const string Attribute_StartNum = "startnum";					//�ӵڼ�����Ϣ��ʼ��ʾ
        public const string Attribute_Order = "order";						//����
        public const string Attribute_Where = "where";                      //��ȡ�����б�������ж�

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
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();

                attributes.Add(Attribute_IsPage, "�Ƿ���Ҫ��ҳ");
                attributes.Add(Attribute_PageNum, "ÿҳ��ʾ��������Ŀ");

                attributes.Add(Attribute_IsRecommend, "�Ƿ���ʾ�Ƽ�����");

                attributes.Add("cellpadding", "���");
                attributes.Add("cellspacing", "���");
                attributes.Add("class", "Css��");
                attributes.Add(Attribute_Columns, "����");
                attributes.Add(Attribute_Direction, "����");
                attributes.Add(Attribute_Layout, "ָ���б��ַ�ʽ");

                attributes.Add(Attribute_Height, "����߶�");
                attributes.Add(Attribute_Width, "������");
                attributes.Add(Attribute_Align, "�������");
                attributes.Add(Attribute_ItemHeight, "��߶�");
                attributes.Add(Attribute_ItemWidth, "����");
                attributes.Add(Attribute_ItemAlign, "��ˮƽ����");
                attributes.Add(Attribute_ItemVerticalAlign, "�ֱ����");
                attributes.Add(Attribute_ItemClass, "��Css��");

                attributes.Add(Attribute_TotalNum, "��ʾ������Ŀ");
                attributes.Add(Attribute_StartNum, "�ӵڼ�����Ϣ��ʼ��ʾ");
                attributes.Add(Attribute_Order, "����");
                attributes.Add(Attribute_Where, "��ȡ�����б�������ж�");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
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
