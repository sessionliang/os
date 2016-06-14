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
        public const string ElementName = "stl:channels";//��Ŀ�б�

        public const string Attribute_ChannelIndex = "channelindex";			//��Ŀ����
        public const string Attribute_ChannelName = "channelname";				//��Ŀ����
        public const string Attribute_UpLevel = "uplevel";						//�ϼ���Ŀ�ļ���
        public const string Attribute_TopLevel = "toplevel";					//����ҳ���µ���Ŀ����
        public const string Attribute_IsTotal = "istotal";						//�Ƿ��������Ŀ��ѡ�񣨰�����ҳ��
        public const string Attribute_IsAllChildren = "isallchildren";			//�Ƿ���ʾ���м��������Ŀ

        public const string Attribute_GroupChannel = "groupchannel";		    //ָ����ʾ����Ŀ��
        public const string Attribute_GroupChannelNot = "groupchannelnot";	    //ָ������ʾ����Ŀ��
        public const string Attribute_Group = "group";		                    //ָ����ʾ����Ŀ��
        public const string Attribute_GroupNot = "groupnot";	                //ָ������ʾ����Ŀ��
        public const string Attribute_TotalNum = "totalnum";					//��ʾ��Ŀ��Ŀ
        public const string Attribute_StartNum = "startnum";					//�ӵڼ�����Ϣ��ʼ��ʾ
        public const string Attribute_TitleWordNum = "titlewordnum";			//��Ŀ������������
        public const string Attribute_Order = "order";						    //����
        public const string Attribute_IsImage = "isimage";					    //����ʾͼƬ��Ŀ
        public const string Attribute_Where = "where";                          //��ȡ��Ŀ�б�������ж�
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

                attributes.Add(Attribute_GroupChannel, "ָ����ʾ����Ŀ��");
                attributes.Add(Attribute_GroupChannelNot, "ָ������ʾ����Ŀ��");
                attributes.Add(Attribute_TotalNum, "��ʾ��Ŀ��Ŀ");
                attributes.Add(Attribute_StartNum, "�ӵڼ�����Ϣ��ʼ��ʾ");
                attributes.Add(Attribute_Order, "����");
                attributes.Add(Attribute_IsImage, "����ʾͼƬ��Ŀ");
                attributes.Add(Attribute_Where, "��ȡ��Ŀ�б�������ж�");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");

                attributes.Add(Attribute_ChannelIndex, "��Ŀ����");
                attributes.Add(Attribute_ChannelName, "��Ŀ����");
                attributes.Add(Attribute_UpLevel, "�ϼ���Ŀ�ļ���");
                attributes.Add(Attribute_TopLevel, "����ҳ���µ���Ŀ����");
                attributes.Add(Attribute_IsTotal, "�Ƿ��������Ŀ��ѡ��");
                attributes.Add(Attribute_IsAllChildren, "�Ƿ���ʾ���м��������Ŀ");
                return attributes;
            }
        }

        //public sealed class ChannelsItem
        //{
        //    public const string ElementName = "stl:channelsitem";               //��Ŀ�б���

        //    public const string Attribute_Type = "type";                        //�б�������
        //    public const string Attribute_Selected = "selected";                //�б�ǰѡ��������
        //    public const string Attribute_SelectedValue = "selectedvalue";      //�б�ǰѡ�����ֵ

        //    public const string Selected_Current = "current";                   //��ǰ��ĿΪѡ����Ŀ
        //    public const string Selected_Image = "image";                       //��ͼƬ��ĿΪѡ����Ŀ
        //    public const string Selected_Up = "up";                             //��ǰ��Ŀ���ϼ���ĿΪѡ����Ŀ
        //    public const string Selected_Top = "top";                           //��ǰ��Ŀ����ҳ���µ���ĿΪѡ����Ŀ

        //    public const string Type_Header = "header";                 //Ϊ stl:channels �е����ṩͷ������
        //    public const string Type_Footer = "footer";                 //Ϊ stl:channels �е����ṩ�ײ�����
        //    public const string Type_Item = "item";                             //Ϊ stl:channels �е����ṩ���ݺͲ���
        //    public const string Type_AlternatingItem = "alternatingitem";       //Ϊ stl:channels �еĽ������ṩ���ݺͲ���
        //    public const string Type_SelectedItem = "selecteditem";             //Ϊ stl:channels �е�ǰѡ�����ṩ���ݺͲ���
        //    public const string Type_Separator = "separator";                   //Ϊ stl:channels �и���֮��ķָ����ṩ���ݺͲ���

        //    public static ListDictionary AttributeList
        //    {
        //        get
        //        {
        //            ListDictionary attributes = new ListDictionary();

        //            attributes.Add(Attribute_Type, "��Ŀ�б�������");
        //            attributes.Add(Attribute_Selected, "��Ŀ�б�ǰѡ��������");
        //            attributes.Add(Attribute_SelectedValue, "��Ŀ�б�ǰѡ����ֵ");
        //            return attributes;
        //        }
        //    }
        //}


        //�ԡ���Ŀ�б���stl:channels��Ԫ�ؽ��н���
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

                //������ʾ����
                TemplateUtility.PutContentsDisplayInfoToMyDataList(pdlContents, displayInfo);

                //�����б�ģ��
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
