using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Xml;
using System.Collections;
using SiteServer.BBS.Core.TemplateParser.Element;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Core.TemplateParser.Model
{
    public class ContentsDisplayInfo
    {
        private EContextType contextType = EContextType.Forum;
        private string headerTemplate = string.Empty;
        private string footerTemplate = string.Empty;
        private string itemTemplate = string.Empty;
        private string separatorTemplate = string.Empty;
        private string alternatingItemTemplate = string.Empty;
        private string separatorRepeatTemplate = string.Empty;
        private int separatorRepeat = 0;

        //sqlContents only
        private string connectionString = string.Empty;
        private string queryString = string.Empty;

        private string forumIndex = string.Empty;
        private string forumName = string.Empty;
        private int upLevel = 0;
        private int topLevel = -1;
        private string group = string.Empty;
        private string groupNot = string.Empty;
        private string tags = string.Empty;

        private bool isTop = false;
        private bool isTopExists = false;

        private string where = string.Empty;
        private bool isAllChildren = false;
        private bool isDynamic = false;
        private int totalNum = 0;
        private int pageNum = 0;
        private int startNum = 1;
        private string orderByString = string.Empty;
        private int columns = 0;
        private RepeatDirection direction = RepeatDirection.Vertical;
        private Unit height = Unit.Empty;
        private Unit width = Unit.Percentage(100);
        private string align = string.Empty;
        private Unit itemHeight = Unit.Empty;
        private Unit itemWidth = Unit.Empty;
        private string itemAlign = string.Empty;
        private string itemVerticalAlign = string.Empty;
        private string itemClass = string.Empty;
        private bool isDisplayIfEmpty = false;

        private ELayout layout = ELayout.None;

        private NameValueCollection otherAttributes = new NameValueCollection();

        public static ContentsDisplayInfo GetContentsDisplayInfoByXmlNode(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, EContextType contextType)
        {
            ContentsDisplayInfo displayInfo = new ContentsDisplayInfo();
            displayInfo.contextType = contextType;

            string innerXml = node.InnerXml;
            string itemTemplate = string.Empty;

            if (!string.IsNullOrEmpty(innerXml))
            {
                ArrayList elementArrayList = ParserUtility.GetElementArrayList(innerXml);
                if (elementArrayList.Count > 0)
                {
                    foreach (string theElement in elementArrayList)
                    {
                        if (ParserUtility.IsSpecifiedStlElement(theElement, ContentsItem.ElementName))
                        {
                            LowerNameValueCollection attributes = new LowerNameValueCollection();
                            string templateString = ParserUtility.GetInnerXml(theElement, true, attributes);
                            if (!string.IsNullOrEmpty(templateString))
                            {
                                foreach (string key in attributes.Keys)
                                {
                                    if (key == ContentsItem.Attribute_Type)
                                    {
                                        string type = attributes[key];
                                        if (StringUtils.EqualsIgnoreCase(type, ContentsItem.Type_Item))
                                        {
                                            itemTemplate = templateString;
                                        }
                                        else if (StringUtils.EqualsIgnoreCase(type, ContentsItem.Type_Header))
                                        {
                                            displayInfo.headerTemplate = templateString;
                                        }
                                        else if (StringUtils.EqualsIgnoreCase(type, ContentsItem.Type_Footer))
                                        {
                                            displayInfo.footerTemplate = templateString;
                                        }
                                        else if (StringUtils.EqualsIgnoreCase(type, ContentsItem.Type_AlternatingItem))
                                        {
                                            displayInfo.alternatingItemTemplate = templateString;
                                        }
                                        else if (StringUtils.EqualsIgnoreCase(type, ContentsItem.Type_Separator))
                                        {
                                            int selectedValue = TranslateUtils.ToInt(attributes[ContentsItem.Attribute_SelectedValue], 1);
                                            if (selectedValue <= 1)
                                            {
                                                displayInfo.separatorTemplate = templateString;
                                            }
                                            else
                                            {
                                                displayInfo.separatorRepeatTemplate = templateString;
                                                displayInfo.separatorRepeat = selectedValue;
                                            }
                                        }
                                    }
                                }
                            }
                            innerXml = innerXml.Replace(theElement, string.Empty);
                        }
                        else if (contextType == EContextType.SqlContent && ParserUtility.IsSpecifiedStlElement(theElement, SiteServer.BBS.Core.TemplateParser.Element.QueryString.ElementName))
                        {
                            StringBuilder innerBuilder = new StringBuilder(ParserUtility.GetInnerXml(theElement, true));
                            ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            ParserUtility.XmlToHtml(innerBuilder);
                            displayInfo.QueryString = innerBuilder.ToString();
                            innerXml = innerXml.Replace(theElement, string.Empty);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(itemTemplate))
            {
                if (!string.IsNullOrEmpty(innerXml))
                {
                    displayInfo.ItemTemplate = innerXml;
                }
                else
                {
                    displayInfo.ItemTemplate = "<stl:a target=\"_blank\"></stl:a>";
                }
            }
            else
            {
                displayInfo.itemTemplate = itemTemplate;
            }

            IEnumerator ie = node.Attributes.GetEnumerator();
            bool isSetDirection = false;//是否设置了direction属性

            while (ie.MoveNext())
            {
                XmlAttribute attr = (XmlAttribute)ie.Current;
                string attributeName = attr.Name.ToLower();

                if (attributeName.Equals(Forums.Attribute_ForumIndex))
                {
                    displayInfo.forumIndex = attr.Value;
                }
                else if (attributeName.Equals(Forums.Attribute_ForumName))
                {
                    displayInfo.forumName = attr.Value;
                }
                else if (attributeName.Equals(Forums.Attribute_UpLevel))
                {
                    displayInfo.upLevel = TranslateUtils.ToInt(attr.Value);
                }
                else if (attributeName.Equals(Forums.Attribute_TopLevel))
                {
                    displayInfo.topLevel = TranslateUtils.ToInt(attr.Value);
                }
                //else if (attributeName.Equals(Forums.Attribute_IsTop))
                //{
                //    displayInfo.IsTop = TranslateUtils.ToBool(attr.Value);
                //}
                //else if (attributeName.Equals(StlContents.Attribute_IsRecommend))
                //{
                //    displayInfo.IsRecommend = TranslateUtils.ToBool(attr.Value);
                //}
                //else if (attributeName.Equals(StlContents.Attribute_IsHot))
                //{
                //    displayInfo.IsHot = TranslateUtils.ToBool(attr.Value);
                //}
                //else if (attributeName.Equals(StlContents.Attribute_IsColor))
                //{
                //    displayInfo.IsColor = TranslateUtils.ToBool(attr.Value);
                //}
                else if (attributeName.Equals(Forums.Attribute_Where))
                {
                    displayInfo.where = attr.Value;
                }
                else if (attributeName.Equals(Forums.Attribute_IsAllChildren))
                {
                    displayInfo.isAllChildren = TranslateUtils.ToBool(attr.Value);
                }
                else if (attributeName.Equals(Forums.Attribute_IsDynamic))
                {
                    displayInfo.IsDynamic = TranslateUtils.ToBool(attr.Value);
                }
                else if (attributeName.Equals(Forums.Attribute_TotalNum))
                {
                    displayInfo.totalNum = TranslateUtils.ToInt(attr.Value);
                }
                //else if (attributeName.Equals(StlPageContents.Attribute_PageNum))
                //{
                //    displayInfo.PageNum = TranslateUtils.ToInt(attr.Value);
                //}
                else if (attributeName.Equals(Forums.Attribute_StartNum))
                {
                    displayInfo.StartNum = TranslateUtils.ToInt(attr.Value);
                }
                //else if (attributeName.Equals(Forums.Attribute_Order))
                //{
                //    if (contextType == EContextType.Content)
                //    {
                //        displayInfo.OrderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, attr.Value, EAuxiliaryTableType.BackgroundContent, EBBSTaxisType.OrderByTaxisDesc);
                //    }
                //    else if (contextType == EContextType.Channel)
                //    {
                //        displayInfo.OrderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, attr.Value, EAuxiliaryTableType.Channel, EBBSTaxisType.OrderByTaxis);
                //    }
                //    else if (contextType == EContextType.Comment)
                //    {
                //        displayInfo.OrderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, attr.Value, EAuxiliaryTableType.SiteComment, EBBSTaxisType.OrderByTaxisDesc);
                //    }
                //    else if (contextType == EContextType.InputContent)
                //    {
                //        displayInfo.OrderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, attr.Value, EAuxiliaryTableType.InputContent, EBBSTaxisType.OrderByTaxisDesc);
                //    }
                //    else
                //    {
                //        displayInfo.OrderByString = attr.Value;
                //    }
                //}
                //else if (attributeName.Equals(StlContents.Attribute_GroupChannel))
                //{
                //    displayInfo.GroupChannel = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                //    if (string.IsNullOrEmpty(displayInfo.GroupChannel))
                //    {
                //        displayInfo.GroupChannel = "__Empty__";
                //    }
                //}
                //else if (attributeName.Equals(StlContents.Attribute_GroupChannelNot))
                //{
                //    displayInfo.GroupChannelNot = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                //    if (string.IsNullOrEmpty(displayInfo.GroupChannelNot))
                //    {
                //        displayInfo.GroupChannelNot = "__Empty__";
                //    }
                //}
                //else if (attributeName.Equals(StlContents.Attribute_GroupContent) || attributeName.Equals(StlContents.Attribute_Group))
                //{
                //    displayInfo.GroupContent = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                //    if (string.IsNullOrEmpty(displayInfo.GroupContent))
                //    {
                //        displayInfo.GroupContent = "__Empty__";
                //    }
                //}
                //else if (attributeName.Equals(StlContents.Attribute_GroupContentNot) || attributeName.Equals(StlContents.Attribute_GroupNot))
                //{
                //    displayInfo.GroupContentNot = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                //    if (string.IsNullOrEmpty(displayInfo.GroupContentNot))
                //    {
                //        displayInfo.GroupContentNot = "__Empty__";
                //    }
                //}
                //else if (attributeName.Equals(StlContents.Attribute_Tags))
                //{
                //    displayInfo.Tags = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                //}
                else if (attributeName.Equals(Forums.Attribute_Columns))
                {
                    displayInfo.Columns = TranslateUtils.ToInt(attr.Value);
                    displayInfo.layout = ELayout.Table;
                    if (displayInfo.Columns > 1 && isSetDirection == false)
                    {
                        displayInfo.Direction = RepeatDirection.Horizontal;
                    }
                }
                else if (attributeName.Equals(Forums.Attribute_Direction))
                {
                    displayInfo.layout = ELayout.Table;
                    displayInfo.Direction = (RepeatDirection)TranslateUtils.ToEnum(typeof(RepeatDirection), attr.Value, RepeatDirection.Vertical);
                    isSetDirection = true;
                }
                else if (attributeName.Equals(Forums.Attribute_Height))
                {
                    try
                    {
                        displayInfo.Height = Unit.Parse(attr.Value);
                    }
                    catch { }
                }
                else if (attributeName.Equals(Forums.Attribute_Width))
                {
                    try
                    {
                        displayInfo.Width = Unit.Parse(attr.Value);
                    }
                    catch { }
                }
                else if (attributeName.Equals(Forums.Attribute_Align))
                {
                    displayInfo.Align = attr.Value;
                }
                else if (attributeName.Equals(Forums.Attribute_ItemHeight))
                {
                    try
                    {
                        displayInfo.itemHeight = Unit.Parse(attr.Value);
                    }
                    catch { }
                }
                else if (attributeName.Equals(Forums.Attribute_ItemWidth))
                {
                    try
                    {
                        displayInfo.ItemWidth = Unit.Parse(attr.Value);
                    }
                    catch { }
                }
                else if (attributeName.Equals(Forums.Attribute_ItemAlign))
                {
                    displayInfo.ItemAlign = attr.Value;
                }
                else if (attributeName.Equals(Forums.Attribute_ItemVerticalAlign))
                {
                    displayInfo.ItemVerticalAlign = attr.Value;
                }
                else if (attributeName.Equals(Forums.Attribute_ItemClass))
                {
                    displayInfo.ItemClass = attr.Value;
                }
                else if (attributeName.Equals(Forums.Attribute_IsTop))
                {
                    displayInfo.IsTop = TranslateUtils.ToBool(attr.Value);
                }
                //else if (attributeName.Equals(Forums.Attribute_IsFile))
                //{
                //    displayInfo.IsFile = TranslateUtils.ToBool(attr.Value);
                //}
                else if (attributeName.Equals(Forums.Attribute_IsDisplayIfEmpty))
                {
                    displayInfo.IsDisplayIfEmpty = TranslateUtils.ToBool(attr.Value);
                }
                else if (attributeName.Equals(Forums.Attribute_Layout))
                {
                    displayInfo.Layout = ELayoutUtils.GetEnumType(attr.Value);
                }
                else if (contextType == EContextType.SqlContent && attributeName.Equals(SqlContents.Attribute_ConnectionString))
                {
                    displayInfo.ConnectionString = attr.Value;
                }
                else if (contextType == EContextType.SqlContent && attributeName.Equals(SqlContents.Attribute_ConnectionStringName))
                {
                    if (string.IsNullOrEmpty(displayInfo.ConnectionString))
                    {
                        displayInfo.ConnectionString = ConfigUtils.Instance.GetAppSettings(attr.Value);
                    }
                }
                else
                {
                    displayInfo.OtherAttributes.Add(attributeName, attr.Value);
                }
            }

            return displayInfo;

        }

        public string ItemTemplate
        {
            get { return itemTemplate; }
            set { itemTemplate = value; }
        }

        public string HeaderTemplate
        {
            get { return headerTemplate; }
            set { headerTemplate = value; }
        }

        public string FooterTemplate
        {
            get { return footerTemplate; }
            set { footerTemplate = value; }
        }

        public string SeparatorTemplate
        {
            get { return separatorTemplate; }
            set { separatorTemplate = value; }
        }

        public string AlternatingItemTemplate
        {
            get { return alternatingItemTemplate; }
            set { alternatingItemTemplate = value; }
        }

        public string SeparatorRepeatTemplate
        {
            get { return separatorRepeatTemplate; }
            set { separatorRepeatTemplate = value; }
        }

        public int SeparatorRepeat
        {
            get { return separatorRepeat; }
            set { separatorRepeat = value; }
        }

        public int TotalNum
        {
            get { return totalNum; }
            set { totalNum = value; }
        }

        public int PageNum
        {
            get { return pageNum; }
            set { pageNum = value; }
        }

        public int StartNum
        {
            get { return startNum; }
            set { startNum = value; }
        }

        public string OrderByString
        {
            get
            {
                if (string.IsNullOrEmpty(this.orderByString))
                {
                    if (this.contextType == EContextType.Forum)
                    {
                        return EBBSTaxisTypeUtils.GetOrderByString(EContextType.Forum, EBBSTaxisType.OrderByTaxis);
                    }
                    else if (this.contextType == EContextType.Thread)
                    {
                        return EBBSTaxisTypeUtils.GetOrderByString(EContextType.Thread, EBBSTaxisType.OrderByTaxisDesc);
                    }
                    else if (this.contextType == EContextType.Post)
                    {
                        return EBBSTaxisTypeUtils.GetOrderByString(EContextType.Post, EBBSTaxisType.OrderByTaxis);
                    }
                }
                return this.orderByString;
            }
            set { orderByString = value; }
        }

        public string Group
        {
            get { return group; }
            set { group = value; }
        }

        public string GroupNot
        {
            get { return groupNot; }
            set { groupNot = value; }
        }

        public string Tags
        {
            get { return tags; }
            set { tags = value; }
        }

        public int Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        public RepeatDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Unit Height
        {
            get { return height; }
            set { height = value; }
        }

        public Unit Width
        {
            get { return width; }
            set { width = value; }
        }

        public string Align
        {
            get { return align; }
            set { align = value; }
        }

        public Unit ItemHeight
        {
            get { return itemHeight; }
            set { itemHeight = value; }
        }

        public Unit ItemWidth
        {
            get { return itemWidth; }
            set { itemWidth = value; }
        }

        public string ItemAlign
        {
            get { return itemAlign; }
            set { itemAlign = value; }
        }

        public string ItemVerticalAlign
        {
            get { return itemVerticalAlign; }
            set { itemVerticalAlign = value; }
        }

        public string ItemClass
        {
            get { return itemClass; }
            set { itemClass = value; }
        }

        public bool IsDisplayIfEmpty
        {
            get { return isDisplayIfEmpty; }
            set { isDisplayIfEmpty = value; }
        }

        public ELayout Layout
        {
            get { return layout; }
            set { layout = value; }
        }

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public string QueryString
        {
            get { return queryString; }
            set { queryString = value; }
        }

        public string ForumName
        {
            get { return forumName; }
            set { forumName = value; }
        }

        public string ForumIndex
        {
            get { return forumIndex; }
            set { forumIndex = value; }
        }

        public int UpLevel
        {
            get { return upLevel; }
            set { upLevel = value; }
        }

        public int TopLevel
        {
            get { return topLevel; }
            set { topLevel = value; }
        }

        public bool IsTop
        {
            get { return isTop; }
            set
            {
                isTopExists = true;
                isTop = value;
            }
        }

        public bool IsTopExists
        {
            get { return isTopExists; }
        }

        public string Where
        {
            get { return this.where; }
            set { this.where = value; }
        }

        public bool IsAllChildren
        {
            get { return isAllChildren; }
            set { isAllChildren = value; }
        }

        public bool IsDynamic
        {
            get { return isDynamic; }
            set { isDynamic = value; }
        }

        public NameValueCollection OtherAttributes
        {
            get { return this.otherAttributes; }
            set { this.otherAttributes = value; }
        }

    }
}
