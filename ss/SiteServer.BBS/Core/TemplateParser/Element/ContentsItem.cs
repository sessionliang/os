using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using System;

namespace SiteServer.BBS.Core.TemplateParser.Element
{
    public class ContentsItem
    {
        public const string ElementName = "bbs:contentsitem";               //内容列表项

        public const string Attribute_Type = "type";                        //内容项类型
        public const string Attribute_SelectedValue = "selectedvalue";      //内容当前选定项的值

        public const string Type_Header = "header";                 //为 stl:contents 中的项提供头部内容
        public const string Type_Footer = "footer";                 //为 stl:contents 中的项提供底部内容
        public const string Type_Item = "item";                             //为 stl:contents 中的项提供内容和布局
        public const string Type_AlternatingItem = "alternatingitem";       //为 stl:contents 中的交替项提供内容和布局
        public const string Type_Separator = "separator";                   //为 stl:contents 中各项之间的分隔符提供内容和布局

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();

                attributes.Add(Attribute_Type, "内容列表项类型");
                attributes.Add(Attribute_SelectedValue, "内容当前选定项的值");
                return attributes;
            }
        }

    }
}
