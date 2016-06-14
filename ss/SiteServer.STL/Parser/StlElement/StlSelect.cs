using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlSelect
    {
        private StlSelect() { }
        public const string ElementName = "stl:select";//��Ŀ�����������б�

        public const string Attribute_IsChannel = "ischannel";                  //�Ƿ���ʾ��Ŀ�����б�

        public const string Attribute_ChannelIndex = "channelindex";			//��Ŀ����
        public const string Attribute_ChannelName = "channelname";				//��Ŀ����
        public const string Attribute_UpLevel = "uplevel";					    //�ϼ���Ŀ�ļ���
        public const string Attribute_TopLevel = "toplevel";				    //����ҳ���µ���Ŀ����

        public const string Attribute_Scope = "scope";							//��Χ
        public const string Attribute_GroupChannel = "groupchannel";		    //ָ����ʾ����Ŀ��
        public const string Attribute_GroupChannelNot = "groupchannelnot";	    //ָ������ʾ����Ŀ��
        public const string Attribute_GroupContent = "groupcontent";		    //ָ����ʾ��������
        public const string Attribute_GroupContentNot = "groupcontentnot";	    //ָ������ʾ��������
        public const string Attribute_Tags = "tags";	                        //ָ����ǩ
        public const string Attribute_Order = "order";							//����
        public const string Attribute_TotalNum = "totalnum";					//��ʾ��Ŀ
        public const string Attribute_TitleWordNum = "titlewordnum";			//������������
        public const string Attribute_Where = "where";                          //��ȡ�����б�������ж�
        public const string Attribute_QueryString = "querystring";              //���Ӳ���

        public const string Attribute_IsTop = "istop";                       //����ʾ�ö�����
        public const string Attribute_IsRecommend = "isrecommend";           //����ʾ�Ƽ�����
        public const string Attribute_IsHot = "ishot";                       //����ʾ�ȵ�����
        public const string Attribute_IsColor = "iscolor";                   //����ʾ��Ŀ����

        public const string Attribute_Title = "title";							//�����б���ʾ����
        public const string Attribute_OpenWin = "openwin";						//ѡ���Ƿ��´��ڴ�����
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_IsChannel, "�Ƿ���ʾ��Ŀ�����б�");

                attributes.Add(Attribute_ChannelIndex, "��Ŀ����");
                attributes.Add(Attribute_ChannelName, "��Ŀ����");
                attributes.Add(Attribute_UpLevel, "�ϼ���Ŀ�ļ���");
                attributes.Add(Attribute_TopLevel, "����ҳ���µ���Ŀ����");
                attributes.Add(Attribute_Scope, "ѡ��ķ�Χ");
                attributes.Add(Attribute_GroupChannel, "ָ����ʾ����Ŀ��");
                attributes.Add(Attribute_GroupChannelNot, "ָ������ʾ����Ŀ��");
                attributes.Add(Attribute_GroupContent, "ָ����ʾ��������");
                attributes.Add(Attribute_GroupContentNot, "ָ������ʾ��������");
                attributes.Add(Attribute_Tags, "ָ����ǩ");
                attributes.Add(Attribute_Order, "����");
                attributes.Add(Attribute_TotalNum, "��ʾ��Ŀ");
                attributes.Add(Attribute_TitleWordNum, "������������");
                attributes.Add(Attribute_Where, "��ȡ�����б�������ж�");
                attributes.Add(Attribute_QueryString, "���Ӳ���");

                attributes.Add(Attribute_IsTop, "����ʾ�ö�����");
                attributes.Add(Attribute_IsRecommend, "����ʾ�Ƽ�����");
                attributes.Add(Attribute_IsHot, "����ʾ�ȵ�����");
                attributes.Add(Attribute_IsColor, "����ʾ��Ŀ����");

                attributes.Add(Attribute_Title, "�����б���ʾ����");
                attributes.Add(Attribute_OpenWin, "ѡ���Ƿ��´��ڴ�����");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
                return attributes;
            }
        }


        //�ԡ������б���stl:select��Ԫ�ؽ��н���
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                System.Web.UI.HtmlControls.HtmlSelect selectControl = new HtmlSelect();
                IEnumerator ie = node.Attributes.GetEnumerator();

                bool isChannel = true;
                string channelIndex = string.Empty;
                string channelName = string.Empty;
                int upLevel = 0;
                int topLevel = -1;
                string scopeTypeString = string.Empty;
                string groupChannel = string.Empty;
                string groupChannelNot = string.Empty;
                string groupContent = string.Empty;
                string groupContentNot = string.Empty;
                string tags = string.Empty;
                string order = string.Empty;
                int totalNum = 0;
                int titleWordNum = 0;
                string where = string.Empty;
                string queryString = string.Empty;

                bool isTop = false;
                bool isTopExists = false;
                bool isRecommend = false;
                bool isRecommendExists = false;
                bool isHot = false;
                bool isHotExists = false;
                bool isColor = false;
                bool isColorExists = false;

                string displayTitle = string.Empty;
                bool openWin = true;
                bool isDynamic = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(Attribute_IsChannel))
                    {
                        isChannel = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_ChannelIndex))
                    {
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_UpLevel))
                    {
                        upLevel = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_Scope))
                    {
                        scopeTypeString = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_GroupChannel))
                    {
                        groupChannel = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_GroupChannelNot))
                    {
                        groupChannelNot = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_GroupContent))
                    {
                        groupContent = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_GroupContentNot))
                    {
                        groupContentNot = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_Tags))
                    {
                        tags = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_Order))
                    {
                        order = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_TotalNum))
                    {
                        try
                        {
                            totalNum = int.Parse(attr.Value);
                        }
                        catch { }
                    }
                    else if (attributeName.Equals(Attribute_Where))
                    {
                        where = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_QueryString))
                    {
                        queryString = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_IsTop))
                    {
                        isTopExists = true;
                        isTop = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsRecommend))
                    {
                        isRecommendExists = true;
                        isRecommend = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsHot))
                    {
                        isHotExists = true;
                        isHot = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsColor))
                    {
                        isColorExists = true;
                        isColor = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_TitleWordNum))
                    {
                        try
                        {
                            titleWordNum = int.Parse(attr.Value);
                        }
                        catch { }
                    }
                    else if (attributeName.Equals(Attribute_Title))
                    {
                        displayTitle = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                    else if (attributeName.Equals(Attribute_OpenWin))
                    {
                        try
                        {
                            openWin = bool.Parse(attr.Value);
                        }
                        catch { }
                    }
                    else if (attributeName.Equals(Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                    else
                    {
                        selectControl.Attributes.Remove(attributeName);
                        selectControl.Attributes.Add(attributeName, attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, selectControl, isChannel, channelIndex, channelName, upLevel, topLevel, scopeTypeString, groupChannel, groupChannelNot, groupContent, groupContentNot, tags, order, totalNum, titleWordNum, where, queryString, isTop, isTopExists, isRecommend, isRecommendExists, isHot, isHotExists, isColor, isColorExists, displayTitle, openWin);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, System.Web.UI.HtmlControls.HtmlSelect selectControl, bool isChannel, string channelIndex, string channelName, int upLevel, int topLevel, string scopeTypeString, string groupChannel, string groupChannelNot, string groupContent, string groupContentNot, string tags, string order, int totalNum, int titleWordNum, string where, string queryString, bool isTop, bool isTopExists, bool isRecommend, bool isRecommendExists, bool isHot, bool isHotExists, bool isColor, bool isColorExists, string displayTitle, bool openWin)
        {
            string parsedContent = string.Empty;

            EScopeType scopeType;
            if (!string.IsNullOrEmpty(scopeTypeString))
            {
                scopeType = EScopeTypeUtils.GetEnumType(scopeTypeString);
            }
            else
            {
                if (isChannel)
                {
                    scopeType = EScopeType.Children;
                }
                else
                {
                    scopeType = EScopeType.Self;
                }
            }

            string orderByString;
            if (isChannel)
            {
                orderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, order, ETableStyle.Channel, ETaxisType.OrderByTaxis);
            }
            else
            {
                orderByString = StlDataUtility.GetOrderByString(pageInfo.PublishmentSystemID, order, ETableStyle.BackgroundContent, ETaxisType.OrderByTaxisDesc);
            }

            int channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);

            channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);

            NodeInfo channel = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, channelID);

            string uniqueID = "Select_" + pageInfo.UniqueID;
            selectControl.ID = uniqueID;

            string scriptHtml;
            if (openWin)
            {
                scriptHtml = string.Format(@"
<script language=""JavaScript"" type=""text/JavaScript"">
<!--
function {0}_jumpMenu(targ,selObj)
{1} //v3.0
window.open(selObj.options[selObj.selectedIndex].value);
selObj.selectedIndex=0;
{2}
//-->
</script>", uniqueID, "{", "}");
                selectControl.Attributes.Add("onChange", string.Format("{0}_jumpMenu('parent',this)", uniqueID));
            }
            else
            {
                scriptHtml = string.Format("<script language=\"JavaScript\">function {0}_jumpMenu(targ,selObj,restore){1}eval(targ+\".location='\"+selObj.options[selObj.selectedIndex].value+\"'\");if (restore) selObj.selectedIndex=0;{2}</script>", uniqueID, "{", "}");
                selectControl.Attributes.Add("onChange", string.Format("{0}_jumpMenu('self',this,0)", uniqueID));
            }
            if (!string.IsNullOrEmpty(displayTitle))
            {
                ListItem listitem = new ListItem(displayTitle, PageUtils.UNCLICKED_URL);
                listitem.Selected = true;
                selectControl.Items.Add(listitem);
            }

            if (isChannel)
            {
                ArrayList nodeIDArrayList = StlDataUtility.GetNodeIDArrayList(pageInfo.PublishmentSystemID, channel.NodeID, groupContent, groupContentNot, orderByString, scopeType, groupChannel, groupChannelNot, false, false, totalNum, where);

                if (nodeIDArrayList != null && nodeIDArrayList.Count > 0)
                {
                    foreach (int nodeIDInSelect in nodeIDArrayList)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, nodeIDInSelect);

                        if (nodeInfo != null)
                        {
                            string title = StringUtils.MaxLengthText(nodeInfo.NodeName, titleWordNum);
                            string url = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, nodeInfo, pageInfo.VisualType);
                            if (!string.IsNullOrEmpty(queryString))
                            {
                                url = PageUtils.AddQueryString(url, queryString);
                            }
                            ListItem listitem = new ListItem(title, url);
                            selectControl.Items.Add(listitem);
                        }
                    }
                }
            }
            else
            {
                IEnumerable dataSource = StlDataUtility.GetContentsDataSource(pageInfo.PublishmentSystemInfo, channelID, contextInfo.ContentID, groupContent, groupContentNot, tags, false, false, false, false, false, false, false, false, 1, totalNum, orderByString, isTopExists, isTop, isRecommendExists, isRecommend, isHotExists, isHot, isColorExists, isColor, where, scopeType, groupChannel, groupChannelNot, null);

                if (dataSource != null)
                {
                    foreach (object dataItem in dataSource)
                    {
                        BackgroundContentInfo contentInfo = new BackgroundContentInfo(dataItem);
                        if (contentInfo != null)
                        {
                            string title = StringUtils.MaxLengthText(contentInfo.Title, titleWordNum);
                            string url = PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, contentInfo, pageInfo.VisualType);
                            if (!string.IsNullOrEmpty(queryString))
                            {
                                url = PageUtils.AddQueryString(url, queryString);
                            }
                            ListItem listitem = new ListItem(title, url);
                            selectControl.Items.Add(listitem);
                        }
                    }
                }
            }

            parsedContent = scriptHtml + ControlUtils.GetControlRenderHtml(selectControl);

            return parsedContent;
        }
    }
}
