using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.ListTemplate;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using BaiRong.Model;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlEach
    {
        public const string ElementName = "stl:each";                       //ѭ��

        public const string Attribute_Type = "type";
        public const string Attribute_Value = "value";
        public const string Attribute_TotalNum = "totalnum";				//��ʾ������Ŀ
        public const string Attribute_StartNum = "startnum";				//�ӵڼ�����Ϣ��ʼ��ʾ
        public const string Attribute_Order = "order";						//����
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

        public const string Attribute_OnPreLoad = "onpreload";              //��������ǰ��ִ�к���
        public const string Attribute_OnLoaded = "onloaded";                //�������ݺ�ִ�к���

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

        public const string Type_Photo = "photo";
        public const string Type_Cart = "cart";
        public const string Type_Guess = "guess";                            //����ϲ��
        public const string Type_Filter = "filter";
        public const string Type_FilterItem = "filterItem";
        public const string Type_FilterContents = "filterContents";
        public const string Type_FilterNavigation = "filterPageNavigation";
        public const string Type_Order = "order";
        public const string Type_Template = "template";
        public const string Type_Teleplay = "teleplay";
        public const string Type_Consultation = "consultation";
        public const string Type_OrderComment = "orderComment";

        public const string Type_Property = "property";                     //���ԣ����ݣ���Ŀ��

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();

                attributes.Add(Attribute_Type, "ѭ������");
                attributes.Add(Attribute_Value, "ѭ��ֵ");
                attributes.Add(Attribute_TotalNum, "��ʾ������Ŀ");
                attributes.Add(Attribute_StartNum, "�ӵڼ�����Ϣ��ʼ��ʾ");
                attributes.Add(Attribute_Order, "����");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
                attributes.Add(Attribute_OnPreLoad, "��������ǰ��ִ�к���");
                attributes.Add(Attribute_OnLoaded, "�������ݺ�ִ�к���");

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
                    parsedContent = ParseImpl(pageInfo, node, contextInfo, displayInfo);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, XmlNode node, ContextInfo contextInfo, ContentsDisplayInfo displayInfo)
        {
            string parsedContent = string.Empty;

            string type = displayInfo.OtherAttributes[Attribute_Type];
            if (string.IsNullOrEmpty(type))
            {
                type = BackgroundContentAttribute.ImageUrl;
            }
            string value = displayInfo.OtherAttributes[Attribute_Value];
            string onPreLoad = displayInfo.OtherAttributes[Attribute_OnPreLoad];
            string onLoaded = displayInfo.OtherAttributes[Attribute_OnLoaded];

            EContextType contextType = EContextType.Each;
            IEnumerable dataSource = null;
            if (StringUtils.EqualsIgnoreCase(type, StlEach.Type_Photo))
            {
                contextType = EContextType.Photo;
                dataSource = StlDataUtility.GetPhotosDataSource(pageInfo.PublishmentSystemInfo, contextInfo.ContentID, displayInfo.StartNum, displayInfo.TotalNum);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlEach.Type_Teleplay))
            {
                contextType = EContextType.Teleplay;
                dataSource = StlDataUtility.GetTeleplaysDataSource(pageInfo.PublishmentSystemInfo, contextInfo.ContentID, displayInfo.StartNum, displayInfo.TotalNum);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlEach.Type_Property))
            {
                if (contextInfo.ContextType == EContextType.Undefined)
                    contextInfo.ContextType = EContextType.Content;
                contextType = contextInfo.ContextType;
                dataSource = StlDataUtility.GetPropertysDataSource(pageInfo.PublishmentSystemInfo, contextInfo.ContentInfo, contextType, value, displayInfo.StartNum, displayInfo.TotalNum);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlEach.Type_Cart))
            {
                return string.Format(@"
<%for(i = 0; i < carts.length; i ++) {{
    cart = carts[i]; %>
    {0}
<%}}%>
", node.InnerXml);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlEach.Type_Guess))
            {
                pageInfo.AddPageEndScriptsIfNotExists("extControllerOn", string.Format(@"<script>function extControllerOn(){{ if(typeof({0})=='function')extController.onPreLoad = {0};if(typeof({1})=='function')extController.onLoaded = {1};}}</script>", string.IsNullOrEmpty(onPreLoad) ? "null" : onPreLoad, string.IsNullOrEmpty(onLoaded) ? "null" : onLoaded));
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.D_Ext_GUESSES);

                return string.Format(@"
<script class=""extController"" type=""text/html"">
<%for(i = 0; i < guesses.length; i ++) {{
        guess = guesses[i]; 
        %>{0}<%
}}%>
</script>
", node.InnerXml);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlEach.Type_Consultation))
            {
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.D_CONSULTATION);
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.D_PAGE_DATA_UTILS);

                return string.Format(@"
<script class=""consultationController"" type=""text/html"">
<%for(i = 0; i < consultations.length; i ++) {{
        consultation = consultations[i]; %>
       {0}
<%}}%>
</script>
", node.InnerXml);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlEach.Type_OrderComment))
            {
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.D_PAGE_DATA_UTILS);
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.D_ORDER_COMMENT);

                return string.Format(@"
<script class=""orderCommentController"" type=""text/html"">
<%for(i = 0; i < orderComments.length; i ++) {{
        comment = orderComments[i]; %>
       {0}
<%}}%>
</script>
", node.InnerXml);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlEach.Type_Filter))
            {
                StringBuilder builder = new StringBuilder(node.InnerXml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                return string.Format(@"
<%for(i = 0; i < data.filters.length; i ++) {{
    filter = data.filters[i]; %>
    {0}
<%}}%>
", builder);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlEach.Type_FilterItem))
            {
                StringBuilder builder = new StringBuilder(node.InnerXml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                return string.Format(@"
<%for(j = 0; j < filter.items.length; j ++) {{
    item = filter.items[j]; %>
    {0}
<%}}%>
", builder);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlEach.Type_FilterContents))
            {
                StringBuilder builder = new StringBuilder(node.InnerXml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                return string.Format(@"
<%for(i = 0; i < data.contents.length; i ++) {{
    content = data.contents[i]; %>
    {0}
<%}}%>
", builder);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlEach.Type_FilterNavigation))//˧ѡ���˷�ҳҳ��
            {
                StringBuilder builder = new StringBuilder(node.InnerXml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                return string.Format(@"
<%for(i = 0; i < data.pageItem.pageNavigation.length; i ++) {{
    pageNum = data.pageItem.pageNavigation[i]; %>
    {0}
<%}}%>
", builder);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlEach.Type_Template))
            {
                StringBuilder builder = new StringBuilder(node.InnerXml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                return string.Format(@"
<%for(i = 0; i < {0}.length; i ++) {{
    item = {0}[i]; %>
    {1}
<%}}%>
", value, builder);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlEach.Type_Order))
            {
                StringBuilder builder = new StringBuilder(node.InnerXml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                if (StringUtils.EqualsIgnoreCase(value, "Consignees"))
                {
                    return string.Format(@"
<%for(i = 0; i < consignees.length; i ++) {{
    item = consignees[i]; %>
    {0}
<%}}%>
", builder);
                }
                else if (StringUtils.EqualsIgnoreCase(value, "Payments"))
                {
                    return string.Format(@"
<%for(i = 0; i < payments.length; i ++) {{
    item = payments[i]; %>
    {0}
<%}}%>
", builder);
                }
                else if (StringUtils.EqualsIgnoreCase(value, "Shipments"))
                {
                    return string.Format(@"
<%for(i = 0; i < shipments.length; i ++) {{
    item = shipments[i]; %>
    {0}
<%}}%>
", builder);
                }
                else if (StringUtils.EqualsIgnoreCase(value, "Invoices"))
                {
                    return string.Format(@"
<%for(i = 0; i < invoices.length; i ++) {{
    item = invoices[i]; %>
    {0}
<%}}%>
", builder);
                }
                else if (StringUtils.EqualsIgnoreCase(value, "OrderList"))
                {
                    return string.Format(@"
<%for(i = 0; i < orderList.length; i ++) {{
    order = orderList[i].orderInfo;
    items = orderList[i].items;
    isPaymentClick = orderList[i].isPaymentClick;
%>
    {0}
<%}}%>
", builder);
                }
                else if (StringUtils.EqualsIgnoreCase(value, "OrderItemList"))
                {
                    return string.Format(@"
<%for(j = 0; j < items.length; j ++) {{
    item = items[j]; %>
    {0}
<%}}%>
", builder);
                }
            }
            else
            {
                ContentInfo contentInfo = contextInfo.ContentInfo;
                if (contentInfo != null)
                {
                    List<string> eachList = new List<string>();

                    if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(type)))
                    {
                        eachList.Add(contentInfo.GetExtendedAttribute(type));
                    }

                    string extendAttributeName = ContentAttribute.GetExtendAttributeName(type);
                    string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                    if (!string.IsNullOrEmpty(extendValues))
                    {
                        foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                        {
                            eachList.Add(extendValue);
                        }
                    }

                    if (displayInfo.StartNum > 1 || displayInfo.TotalNum > 0)
                    {
                        if (displayInfo.StartNum > 1)
                        {
                            int count = displayInfo.StartNum - 1;
                            if (count > eachList.Count)
                            {
                                count = eachList.Count;
                            }
                            eachList.RemoveRange(0, count);
                        }

                        if (displayInfo.TotalNum > 0)
                        {
                            if (displayInfo.TotalNum < eachList.Count)
                            {
                                eachList.RemoveRange(displayInfo.TotalNum, eachList.Count - displayInfo.TotalNum);
                            }
                        }
                    }

                    dataSource = eachList;
                }
            }

            if (displayInfo.Layout == ELayout.None)
            {
                Repeater rptContents = new Repeater();

                rptContents.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, contextType, contextInfo);
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
                    rptContents.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, contextType, contextInfo);
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

                TemplateUtility.PutContentsDisplayInfoToMyDataList(pdlContents, displayInfo);

                pdlContents.ItemTemplate = new DataListTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, contextType, contextInfo);
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
                    pdlContents.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, contextType, contextInfo);
                }

                pdlContents.DataSource = dataSource;

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
