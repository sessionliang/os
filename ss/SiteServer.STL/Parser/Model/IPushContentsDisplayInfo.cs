using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Model;
using System.Text;
using SiteServer.CMS.Core;
using System.Collections.Generic;

namespace SiteServer.STL.Parser.Model
{
    /// <summary>
    ///  by 20151125 sofuny
    /// 培生智能推送
    /// 增加智能推送内容列表标签
    /// </summary>
    public class IPushContentsDisplayInfo
    {
        private EContextType contextType = EContextType.Content;
        private string headerTemplate = string.Empty;
        private string footerTemplate = string.Empty;
        private string itemTemplate = string.Empty;
        private string separatorTemplate = string.Empty;
        private string alternatingItemTemplate = string.Empty;
        private LowerNameValueCollection selectedItems = new LowerNameValueCollection();
        private LowerNameValueCollection selectedValues = new LowerNameValueCollection();
        private string separatorRepeatTemplate = string.Empty;
        private int separatorRepeat = 0;

        //sqlContents only
        private string connectionString = string.Empty;
        private string queryString = string.Empty;

        private string channelIndex = string.Empty;
        private string channelName = string.Empty;
        private string channelIndexNot = string.Empty;//指定不显示的栏目ID,索引
        private int upLevel = 0;
        private int topLevel = -1;
        private bool isScopeExists = false;
        private EScopeType scope = EScopeType.Self;
        private string groupContent = string.Empty;
        private string groupContentNot = string.Empty;
        private string groupChannel = string.Empty;
        private string groupChannelNot = string.Empty;
        private string tags = string.Empty;

        private bool isTop = false;
        private bool isTopExists = false;
        private bool isRecommend = false;
        private bool isRecommendExists = false;
        private bool isHot = false;
        private bool isHotExists = false;
        private bool isColor = false;
        private bool isColorExists = false;

        private string where = string.Empty;
        private bool isDynamic = false;
        private int totalNum = 0;
        private int pageNum = 0;
        private int titleWordNum = 0;
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
        private bool isImage = false;
        private bool isVideo = false;
        private bool isFile = false;
        private bool isNoDup = false;
        private bool isRelatedContents = false;

        private ELayout layout = ELayout.None;

        private NameValueCollection otherAttributes = new NameValueCollection();

        public static IPushContentsDisplayInfo GetContentsDisplayInfoByXmlNode(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, EContextType contextType)
        {
            IPushContentsDisplayInfo displayInfo = new IPushContentsDisplayInfo();
            displayInfo.contextType = contextType;

            string innerXml = node.InnerXml;
            string itemTemplate = string.Empty;

            if (!string.IsNullOrEmpty(innerXml))
            {
                List<string> stlElementList = StlParserUtility.GetStlElementList(innerXml);
                if (stlElementList.Count > 0)
                {
                    foreach (string theStlElement in stlElementList)
                    {
                        if (StlParserUtility.IsSpecifiedStlElement(theStlElement, StlItemTemplate.ElementName)
                         || StlParserUtility.IsSpecifiedStlElement(theStlElement, "stl:contentsitem")
                         || StlParserUtility.IsSpecifiedStlElement(theStlElement, "stl:inputcontentsitem")
                         || StlParserUtility.IsSpecifiedStlElement(theStlElement, "stl:commentsitem")
                         || StlParserUtility.IsSpecifiedStlElement(theStlElement, "stl:sitesitem")
                         || StlParserUtility.IsSpecifiedStlElement(theStlElement, "stl:channelsitem")
                         || StlParserUtility.IsSpecifiedStlElement(theStlElement, "stl:sqlcontentsitem")
                            )
                        {
                            LowerNameValueCollection attributes = new LowerNameValueCollection();
                            string templateString = StlParserUtility.GetInnerXml(theStlElement, true, attributes);
                            if (!string.IsNullOrEmpty(templateString))
                            {
                                foreach (string key in attributes.Keys)
                                {
                                    if (key == StlItemTemplate.Attribute_Type)
                                    {
                                        string type = attributes[key];
                                        if (StringUtils.EqualsIgnoreCase(type, StlItemTemplate.Type_Item))
                                        {
                                            itemTemplate = templateString;
                                        }
                                        else if (StringUtils.EqualsIgnoreCase(type, StlItemTemplate.Type_Header))
                                        {
                                            displayInfo.headerTemplate = templateString;
                                        }
                                        else if (StringUtils.EqualsIgnoreCase(type, StlItemTemplate.Type_Footer))
                                        {
                                            displayInfo.footerTemplate = templateString;
                                        }
                                        else if (StringUtils.EqualsIgnoreCase(type, StlItemTemplate.Type_AlternatingItem))
                                        {
                                            displayInfo.alternatingItemTemplate = templateString;
                                        }
                                        else if (StringUtils.EqualsIgnoreCase(type, StlItemTemplate.Type_SelectedItem))
                                        {
                                            if (!string.IsNullOrEmpty(attributes[StlItemTemplate.Attribute_Selected]))
                                            {
                                                string selected = attributes[StlItemTemplate.Attribute_Selected];
                                                ArrayList arraylist = new ArrayList();
                                                if (selected.IndexOf(',') != -1)
                                                {
                                                    arraylist.AddRange(selected.Split(','));
                                                }
                                                else
                                                {
                                                    if (selected.IndexOf('-') != -1)
                                                    {
                                                        int first = TranslateUtils.ToInt(selected.Split('-')[0]);
                                                        int second = TranslateUtils.ToInt(selected.Split('-')[1]);
                                                        for (int i = first; i <= second; i++)
                                                        {
                                                            arraylist.Add(i.ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        arraylist.Add(selected);
                                                    }
                                                }
                                                foreach (string val in arraylist)
                                                {
                                                    displayInfo.selectedItems[val] = templateString;
                                                }
                                                if (!string.IsNullOrEmpty(attributes[StlItemTemplate.Attribute_SelectedValue]))
                                                {
                                                    string selectedValue = attributes[StlItemTemplate.Attribute_SelectedValue];
                                                    displayInfo.selectedValues[selectedValue] = templateString;
                                                }
                                            }
                                        }
                                        else if (StringUtils.EqualsIgnoreCase(type, StlItemTemplate.Type_Separator))
                                        {
                                            int selectedValue = TranslateUtils.ToInt(attributes[StlItemTemplate.Attribute_SelectedValue], 1);
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
                            innerXml = innerXml.Replace(theStlElement, string.Empty);
                        }
                        else if (contextType == EContextType.SqlContent && StlParserUtility.IsSpecifiedStlElement(theStlElement, StlQueryString.ElementName))
                        {
                            StringBuilder innerBuilder = new StringBuilder(StlParserUtility.GetInnerXml(theStlElement, true));
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            StlParserUtility.XmlToHtml(innerBuilder);
                            displayInfo.QueryString = innerBuilder.ToString();
                            innerXml = innerXml.Replace(theStlElement, string.Empty);
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

                if (attributeName.Equals(StlIPushContents.Attribute_ChannelIndex))
                {
                    displayInfo.channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_ChannelName))
                {
                    displayInfo.ChannelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_ChannelIndexNot))//增加的排除属性
                {
                    displayInfo.ChannelIndexNot = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_UpLevel))
                {
                    displayInfo.upLevel = TranslateUtils.ToInt(attr.Value);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_TopLevel))
                {
                    displayInfo.topLevel = TranslateUtils.ToInt(attr.Value);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_Scope))
                {
                    displayInfo.Scope = EScopeTypeUtils.GetEnumType(attr.Value);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_IsTop))
                {
                    displayInfo.IsTop = TranslateUtils.ToBool(attr.Value);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_IsRecommend))
                {
                    displayInfo.IsRecommend = TranslateUtils.ToBool(attr.Value);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_IsHot))
                {
                    displayInfo.IsHot = TranslateUtils.ToBool(attr.Value);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_IsColor))
                {
                    displayInfo.IsColor = TranslateUtils.ToBool(attr.Value);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_Where))
                {
                    displayInfo.Where = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_IsDynamic))
                {
                    displayInfo.IsDynamic = TranslateUtils.ToBool(attr.Value);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_TotalNum))
                {
                    displayInfo.TotalNum = TranslateUtils.ToInt(StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo));
                }
                else if (attributeName.Equals(StlPageIPushContents.Attribute_PageNum))
                {
                    displayInfo.PageNum = TranslateUtils.ToInt(StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo));
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_TitleWordNum))
                {
                    displayInfo.TitleWordNum = TranslateUtils.ToInt(StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo));
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_StartNum))
                {
                    displayInfo.StartNum = TranslateUtils.ToInt(StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo));
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_Order))
                {
                    if (contextType == EContextType.Content)
                    {
                        displayInfo.OrderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, attr.Value, ETableStyle.BackgroundContent, ETaxisType.OrderByTaxisDesc);
                    }
                    else if (contextType == EContextType.Channel)
                    {
                        displayInfo.OrderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, attr.Value, ETableStyle.Channel, ETaxisType.OrderByTaxis);
                    }
                    else if (contextType == EContextType.Comment)
                    {
                        displayInfo.OrderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, attr.Value, ETableStyle.Comment, ETaxisType.OrderByTaxisDesc);
                    }
                    else if (contextType == EContextType.InputContent)
                    {
                        displayInfo.OrderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, attr.Value, ETableStyle.InputContent, ETaxisType.OrderByTaxisDesc);
                    }
                    else
                    {
                        displayInfo.OrderByString = attr.Value;
                    }
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_GroupChannel))
                {
                    displayInfo.GroupChannel = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    if (string.IsNullOrEmpty(displayInfo.GroupChannel))
                    {
                        displayInfo.GroupChannel = "__Empty__";
                    }
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_GroupChannelNot))
                {
                    displayInfo.GroupChannelNot = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    if (string.IsNullOrEmpty(displayInfo.GroupChannelNot))
                    {
                        displayInfo.GroupChannelNot = "__Empty__";
                    }
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_GroupContent) || attributeName.Equals(StlIPushContents.Attribute_Group))
                {
                    displayInfo.GroupContent = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    if (string.IsNullOrEmpty(displayInfo.GroupContent))
                    {
                        displayInfo.GroupContent = "__Empty__";
                    }
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_GroupContentNot) || attributeName.Equals(StlIPushContents.Attribute_GroupNot))
                {
                    displayInfo.GroupContentNot = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    if (string.IsNullOrEmpty(displayInfo.GroupContentNot))
                    {
                        displayInfo.GroupContentNot = "__Empty__";
                    }
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_Tags))
                {
                    displayInfo.Tags = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_Columns))
                {
                    displayInfo.Columns = TranslateUtils.ToInt(attr.Value);
                    displayInfo.layout = ELayout.Table;
                    if (displayInfo.Columns > 1 && isSetDirection == false)
                    {
                        displayInfo.Direction = RepeatDirection.Horizontal;
                    }
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_Direction))
                {
                    displayInfo.layout = ELayout.Table;
                    displayInfo.Direction = Converter.ToRepeatDirection(attr.Value);
                    isSetDirection = true;
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_Height))
                {
                    try
                    {
                        displayInfo.Height = Unit.Parse(attr.Value);
                    }
                    catch { }
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_Width))
                {
                    try
                    {
                        displayInfo.Width = Unit.Parse(attr.Value);
                    }
                    catch { }
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_Align))
                {
                    displayInfo.Align = attr.Value;
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_ItemHeight))
                {
                    try
                    {
                        displayInfo.itemHeight = Unit.Parse(attr.Value);
                    }
                    catch { }
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_ItemWidth))
                {
                    try
                    {
                        displayInfo.ItemWidth = Unit.Parse(attr.Value);
                    }
                    catch { }
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_ItemAlign))
                {
                    displayInfo.ItemAlign = attr.Value;
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_ItemVerticalAlign))
                {
                    displayInfo.ItemVerticalAlign = attr.Value;
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_ItemClass))
                {
                    displayInfo.ItemClass = attr.Value;
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_IsImage))
                {
                    displayInfo.IsImage = TranslateUtils.ToBool(attr.Value);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_IsVideo))
                {
                    displayInfo.IsVideo = TranslateUtils.ToBool(attr.Value);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_IsFile))
                {
                    displayInfo.IsFile = TranslateUtils.ToBool(attr.Value);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_IsNoDup))
                {
                    displayInfo.IsNoDup = TranslateUtils.ToBool(attr.Value);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_IsRelatedContents))
                {
                    displayInfo.IsRelatedContents = TranslateUtils.ToBool(attr.Value);
                }
                else if (attributeName.Equals(StlIPushContents.Attribute_Layout))
                {
                    displayInfo.Layout = ELayoutUtils.GetEnumType(attr.Value);
                }
                else if (contextType == EContextType.SqlContent && attributeName.Equals(StlSqlContents.Attribute_ConnectionString))
                {
                    displayInfo.ConnectionString = attr.Value;
                }
                else if (contextType == EContextType.SqlContent && attributeName.Equals(StlSqlContents.Attribute_ConnectionStringName))
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

        public LowerNameValueCollection SelectedItems
        {
            get { return selectedItems; }
            set { selectedItems = value; }
        }

        public LowerNameValueCollection SelectedValues
        {
            get { return selectedValues; }
            set { selectedValues = value; }
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

        public int TitleWordNum
        {
            get { return titleWordNum; }
            set { titleWordNum = value; }
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
                    if (this.contextType == EContextType.Content)
                    {
                        return ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);
                    }
                    else if (this.contextType == EContextType.Channel)
                    {
                        return ETaxisTypeUtils.GetChannelOrderByString(ETaxisType.OrderByTaxis);
                    }
                    else if (this.contextType == EContextType.Comment)
                    {
                        return ETaxisTypeUtils.GetOrderByString(ETableStyle.Comment, ETaxisType.OrderByAddDateDesc, string.Empty, null);
                    }
                    else if (this.contextType == EContextType.InputContent)
                    {
                        return ETaxisTypeUtils.GetOrderByString(ETableStyle.InputContent, ETaxisType.OrderByTaxisDesc, string.Empty, null);
                    }
                }
                return this.orderByString;
            }
            set { orderByString = value; }
        }

        public string GroupChannel
        {
            get { return groupChannel; }
            set { groupChannel = value; }
        }

        public string GroupChannelNot
        {
            get { return groupChannelNot; }
            set { groupChannelNot = value; }
        }

        public string GroupContent
        {
            get { return groupContent; }
            set { groupContent = value; }
        }

        public string GroupContentNot
        {
            get { return groupContentNot; }
            set { groupContentNot = value; }
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

        public bool IsImage
        {
            get
            {
                return isImage;
            }
            set
            {
                isImageExists = true;
                isImage = value;
            }
        }

        private bool isImageExists = false;
        public bool IsImageExists
        {
            get
            {
                return isImageExists;
            }
        }

        public bool IsVideo
        {
            get
            {
                return isVideo;
            }
            set
            {
                isVideoExists = true;
                isVideo = value;
            }
        }

        private bool isVideoExists = false;
        public bool IsVideoExists
        {
            get
            {
                return isVideoExists;
            }
        }

        public bool IsFile
        {
            get
            {
                return isFile;
            }
            set
            {
                isFileExists = true;
                isFile = value;
            }
        }

        private bool isFileExists = false;
        public bool IsFileExists
        {
            get
            {
                return isFileExists;
            }
        }

        public bool IsNoDup
        {
            get { return isNoDup; }
            set { isNoDup = value; }
        }

        public bool IsRelatedContents
        {
            get { return isRelatedContents; }
            set { isRelatedContents = value; }
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

        public string ChannelName
        {
            get { return channelName; }
            set { channelName = value; }
        }

        public string ChannelIndexNot
        {
            get { return channelIndexNot; }
            set { channelIndexNot = value; }
        }
        public string ChannelIndex
        {
            get { return channelIndex; }
            set { channelIndex = value; }
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

        public EScopeType Scope
        {
            get
            {
                if (this.isScopeExists)
                {
                    return this.scope;
                }
                else
                {
                    if (this.contextType == EContextType.Channel || this.contextType == EContextType.Site)
                    {
                        return EScopeType.Children;
                    }
                    else
                    {
                        return EScopeType.Self;
                    }
                }
            }
            set
            {
                this.isScopeExists = true;
                this.scope = value;
            }
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

        public bool IsRecommend
        {
            get { return isRecommend; }
            set
            {
                isRecommendExists = true;
                isRecommend = value;
            }
        }

        public bool IsRecommendExists
        {
            get { return isRecommendExists; }
        }

        public bool IsHot
        {
            get { return isHot; }
            set
            {
                isHotExists = true;
                isHot = value;
            }
        }

        public bool IsHotExists
        {
            get { return isHotExists; }
        }

        public bool IsColor
        {
            get { return isColor; }
            set
            {
                isColorExists = true;
                isColor = value;
            }
        }

        public bool IsColorExists
        {
            get { return isColorExists; }
        }

        public string Where
        {
            get { return this.where; }
            set { this.where = value; }
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
