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

namespace SiteServer.STL.Parser.StlElement
{
    public class StlInputContents
    {
        public const string ElementName = "stl:inputcontents";                  //�ύ�����б�

        public const string Attribute_InputName = "inputname";                  //�ύ������
        public const string Attribute_TotalNum = "totalnum";					//��ʾ������Ŀ
        public const string Attribute_StartNum = "startnum";					//�ӵڼ�����Ϣ��ʼ��ʾ
        public const string Attribute_Order = "order";						    //����
        public const string Attribute_IsReply = "isreply";                      //�Ƿ����ʾ�ѻظ�����
        public const string Attribute_Where = "where";                          //��ȡ�����б�������ж�
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

                attributes.Add(Attribute_InputName, "�ύ������");
                attributes.Add(Attribute_IsReply, "�Ƿ����ʾ�ѻظ�����");
                attributes.Add(Attribute_TotalNum, "��ʾ������Ŀ");
                attributes.Add(Attribute_StartNum, "�ӵڼ�����Ϣ��ʼ��ʾ");
                attributes.Add(Attribute_Order, "����");
                attributes.Add(Attribute_Where, "��ȡ�����б�������ж�");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");

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

        //public sealed class InputContentsItem
        //{
        //    public const string ElementName = "stl:inputcontentsitem";               //�ύ�����б���

        //    public const string Attribute_Type = "type";                        //�ύ�����б�������
        //    public const string Attribute_Selected = "selected";                //�б�ǰѡ��������
        //    public const string Attribute_SelectedValue = "selectedvalue";      //�б�ǰѡ�����ֵ

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

        //            attributes.Add(Attribute_Type, "�ύ�����б�������");
        //            attributes.Add(Attribute_Selected, "�б�ǰѡ��������");
        //            attributes.Add(Attribute_SelectedValue, "�б�ǰѡ����ֵ");
        //            return attributes;
        //        }
        //    }
        //}


        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                ContentsDisplayInfo displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.InputContent);

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
            int inputID = DataProvider.InputDAO.GetInputIDAsPossible(displayInfo.OtherAttributes[StlInputContents.Attribute_InputName], pageInfo.PublishmentSystemID);

            if (displayInfo.Layout == ELayout.None)
            {
                Repeater rptContents = new Repeater();

                rptContents.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, null, null, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.InputContent, contextInfo);
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
                    rptContents.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, null, null, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.InputContent, contextInfo);
                }

                rptContents.DataSource = StlDataUtility.GetInputContentsDataSource(pageInfo.PublishmentSystemID, inputID, displayInfo);
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

                pdlContents.ItemTemplate = new DataListTemplate(displayInfo.ItemTemplate, null, null, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.InputContent, contextInfo);
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
                    pdlContents.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, null, null, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.InputContent, contextInfo);
                }

                pdlContents.DataSource = StlDataUtility.GetInputContentsDataSource(pageInfo.PublishmentSystemID, inputID, displayInfo);
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
