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
        public const string ElementName = "stl:sites";//��ȡӦ���б�

        public const string Attribute_SiteName = "sitename";				//Ӧ������
        public const string Attribute_Directory = "directory";				//Ӧ���ļ���

        public const string Attribute_TotalNum = "totalnum";				//��ʾ������Ŀ
        public const string Attribute_StartNum = "startnum";				//�ӵڼ�����Ϣ��ʼ��ʾ
        public const string Attribute_Where = "where";                      //��ȡӦ���б�������ж�
        public const string Attribute_Scope = "scope";						//��Χ
        public const string Attribute_Order = "order";						//����
        public const string Attribute_Since = "since";				        //ʱ���
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

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
                attributes.Add(Attribute_SiteName, "Ӧ������");
                attributes.Add(Attribute_Directory, "Ӧ���ļ���");

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
                attributes.Add(Attribute_Where, "��ȡӦ���б�������ж�");
                attributes.Add(Attribute_Scope, "��Χ");
                attributes.Add(Attribute_Order, "����");
                attributes.Add(Attribute_Since, "ʱ���");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
                return attributes;
            }
        }

        //public sealed class SitesItem
        //{
        //    public const string ElementName = "stl:sitesitem";               //�б���

        //    public const string Attribute_Type = "type";                        //�б�������
        //    public const string Attribute_Selected = "selected";                //��ǰѡ��������
        //    public const string Attribute_SelectedValue = "selectedvalue";      //��ǰѡ�����ֵ

        //    public const string Type_Header = "header";                 //Ϊ���ṩͷ������
        //    public const string Type_Footer = "footer";                 //Ϊ���ṩ�ײ�����
        //    public const string Type_Item = "item";                             //Ϊ���ṩ���ݺͲ���
        //    public const string Type_AlternatingItem = "alternatingitem";       //Ϊ�������ṩ���ݺͲ���
        //    public const string Type_SelectedItem = "selecteditem";             //Ϊ��ǰѡ�����ṩ���ݺͲ���
        //    public const string Type_Separator = "separator";                   //Ϊ����֮��ķָ����ṩ���ݺͲ���

        //    public static ListDictionary AttributeList
        //    {
        //        get
        //        {
        //            ListDictionary attributes = new ListDictionary();

        //            attributes.Add(Attribute_Type, "�б�������");
        //            attributes.Add(Attribute_Selected, "�б�ǰѡ��������");
        //            attributes.Add(Attribute_SelectedValue, "�б�ǰѡ�����ֵ");
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
