using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.ListTemplate;
using SiteServer.CMS.Model;
using System;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlItemTemplate
	{
        public const string ElementName = "stl:itemtemplate";               //�б���

        public const string Attribute_Type = "type";                        //����������
        public const string Attribute_Selected = "selected";                //���ݵ�ǰѡ��������
        public const string Attribute_SelectedValue = "selectedvalue";      //���ݵ�ǰѡ�����ֵ

        public const string Type_Header = "header";                 //Ϊ stl:contents �е����ṩͷ������
        public const string Type_Footer = "footer";                 //Ϊ stl:contents �е����ṩ�ײ�����
        public const string Type_Item = "item";                             //Ϊ stl:contents �е����ṩ���ݺͲ���
        public const string Type_AlternatingItem = "alternatingitem";       //Ϊ stl:contents �еĽ������ṩ���ݺͲ���
        public const string Type_SelectedItem = "selecteditem";             //Ϊ stl:contents �е�ǰѡ�����ṩ���ݺͲ���
        public const string Type_Separator = "separator";                   //Ϊ stl:contents �и���֮��ķָ����ṩ���ݺͲ���

        public class ContentsItem
        {
            public const string Selected_Current = "current";                   //��ǰ����Ϊѡ������
            public const string Selected_Image = "image";                       //��ͼƬ����Ϊѡ������
            public const string Selected_Video = "video";                       //����Ƶ����Ϊѡ������
            public const string Selected_File = "file";                         //����������Ϊѡ������
            public const string Selected_IsTop = "istop";                       //�ö�����Ϊѡ������
            public const string Selected_IsRecommend = "isrecommend";           //�Ƽ�����Ϊѡ������
            public const string Selected_IsHot = "ishot";                       //�ȵ�����Ϊѡ������
            public const string Selected_IsColor = "iscolor";                   //��Ŀ����Ϊѡ������
            public const string Selected_ChannelName = "channelname";           //ָ����Ŀ���Ƶ�����
        }

        public class ChannelsItem
        {
            public const string Selected_Current = "current";                   //��ǰ��ĿΪѡ����Ŀ
            public const string Selected_Image = "image";                       //��ͼƬ��ĿΪѡ����Ŀ
            public const string Selected_Up = "up";                             //��ǰ��Ŀ���ϼ���ĿΪѡ����Ŀ
            public const string Selected_Top = "top";                           //��ǰ��Ŀ����ҳ���µ���ĿΪѡ����Ŀ
        }

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();

                attributes.Add(Attribute_Type, "�����б�������");
                attributes.Add(Attribute_Selected, "�����б�ǰѡ��������");
                attributes.Add(Attribute_SelectedValue, "���ݵ�ǰѡ�����ֵ");
                return attributes;
            }
        }

	}
}
