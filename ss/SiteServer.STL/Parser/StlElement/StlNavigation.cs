using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlNavigation
    {
        private StlNavigation() { }
        public const string ElementName = "stl:navigation";//��ʾ����

        public const string Attribute_Type = "type";							//����������
        public const string Attribute_EmptyText = "emptytext";					//��������ʱ��ʾ����Ϣ
        public const string Attribute_TipText = "tiptext";					    //������ʾ��Ϣ
        public const string Attribute_WordNum = "wordnum";					    //��ʾ����
        public const string Attribute_IsDisplayIfEmpty = "isdisplayifempty";    //��û����ʱ�Ƿ���ʾ
        public const string Attribute_IsDynamic = "isdynamic";                  //�Ƿ�̬��ʾ
        public const string Attribute_IsKeyboard = "iskeyboard";                //�Ƿ������̣������������ֱ�Ϊ�ϣ��£�һ���ݣ���Ŀ������

        public const string Type_PreviousChannel = "PreviousChannel";			//��һ��Ŀ����
        public const string Type_NextChannel = "NextChannel";					//��һ��Ŀ����
        public const string Type_PreviousContent = "PreviousContent";			//��һ��������
        public const string Type_NextContent = "NextContent";					//��һ��������

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Type, "��ʾ������");
                attributes.Add(Attribute_EmptyText, "��������ʱ��ʾ����Ϣ");
                attributes.Add(Attribute_TipText, "������ʾ��Ϣ");
                attributes.Add(Attribute_WordNum, "��ʾ����");
                attributes.Add(Attribute_IsDisplayIfEmpty, "��û����ʱ�Ƿ���ʾ");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
                attributes.Add(Attribute_IsKeyboard, "�Ƿ�������");
                return attributes;
            }
        }

        //�ԡ���������stl:navigation��Ԫ�ؽ��н���
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
        {
            string parsedContent = string.Empty;
            ContextInfo contextInfo = contextInfoRef.Clone();
            try
            {
                HtmlAnchor stlAnchor = new HtmlAnchor();
                IEnumerator ie = node.Attributes.GetEnumerator();
                string type = Type_NextContent;
                string emptyText = string.Empty;
                string tipText = string.Empty;
                int wordNum = 0;
                bool isDisplayIfEmpty = false;
                bool isDynamic = false;
                bool isKeyboard = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_EmptyText))
                    {
                        emptyText = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_TipText))
                    {
                        tipText = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_WordNum))
                    {
                        wordNum = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsDisplayIfEmpty))
                    {
                        isDisplayIfEmpty = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                    else if (attributeName.Equals(Attribute_IsKeyboard))
                    {
                        isKeyboard = TranslateUtils.ToBool(attr.Value);
                    }
                    else
                    {
                        ControlUtils.AddAttributeIfNotExists(stlAnchor, attributeName, attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, stlAnchor, type, emptyText, tipText, wordNum, isDisplayIfEmpty, isKeyboard);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, HtmlAnchor stlAnchor, string type, string emptyText, string tipText, int wordNum, bool isDisplayIfEmpty, bool isKeyboard)
        {
            string parsedContent = string.Empty;

            string successTemplateString = string.Empty;
            string failureTemplateString = string.Empty;

            StlParserUtility.GetInnerTemplateString(node, pageInfo, out successTemplateString, out failureTemplateString);

            if (string.IsNullOrEmpty(successTemplateString))
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID);

                if (type.ToLower().Equals(Type_PreviousChannel.ToLower()) || type.ToLower().Equals(Type_NextChannel.ToLower()))
                {
                    int taxis = nodeInfo.Taxis;
                    bool isNextChannel = true;
                    if (StringUtils.EqualsIgnoreCase(type, Type_PreviousChannel))
                    {
                        isNextChannel = false;
                    }
                    int siblingNodeID = DataProvider.NodeDAO.GetNodeIDByParentIDAndTaxis(nodeInfo.ParentID, taxis, isNextChannel);
                    if (siblingNodeID != 0)
                    {
                        NodeInfo siblingNodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, siblingNodeID);
                        string url = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, siblingNodeInfo, pageInfo.VisualType);
                        if (url.Equals(PageUtils.UNCLICKED_URL))
                        {
                            stlAnchor.Target = string.Empty;
                        }
                        stlAnchor.HRef = url;

                        if (string.IsNullOrEmpty(node.InnerXml))
                        {
                            stlAnchor.InnerHtml = NodeManager.GetNodeName(pageInfo.PublishmentSystemID, siblingNodeID);
                            if (wordNum > 0)
                            {
                                stlAnchor.InnerHtml = StringUtils.MaxLengthText(stlAnchor.InnerHtml, wordNum);
                            }
                        }
                        else
                        {
                            contextInfo.ChannelID = siblingNodeID;
                            StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            stlAnchor.InnerHtml = innerBuilder.ToString();
                        }
                    }
                }
                else if (type.ToLower().Equals(Type_PreviousContent.ToLower()) || type.ToLower().Equals(Type_NextContent.ToLower()))
                {
                    if (contextInfo.ContentID != 0)
                    {
                        int taxis = contextInfo.ContentInfo.Taxis;
                        bool isNextContent = true;
                        if (StringUtils.EqualsIgnoreCase(type, Type_PreviousContent))
                        {
                            isNextContent = false;
                        }
                        ETableStyle tableStyle = NodeManager.GetTableStyle(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
                        string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
                        int siblingContentID = BaiRongDataProvider.ContentDAO.GetContentID(tableName, contextInfo.ChannelID, taxis, isNextContent);
                        if (siblingContentID != 0)
                        {
                            ContentInfo siblingContentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, siblingContentID);
                            string url = PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, siblingContentInfo, pageInfo.VisualType);
                            if (url.Equals(PageUtils.UNCLICKED_URL))
                            {
                                stlAnchor.Target = string.Empty;
                            }
                            stlAnchor.HRef = url;

                            if (isKeyboard)
                            {
                                int keyCode = isNextContent ? 39 : 37;
                                StringBuilder scriptContent = new StringBuilder();
                                pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);
                                scriptContent.Append(string.Format(
@"<script language=""javascript"" type=""text/javascript""> 
      $(document).keydown(function(event){{
        if(event.keyCode=={0}){{location = '{1}';}}
      }});
</script> 
", keyCode, url));
                                string nextOrPrevious = isNextContent ? "nextContent" : "previousContent";
                                pageInfo.SetPageScripts(nextOrPrevious, scriptContent.ToString(), true);
                            }

                            if (string.IsNullOrEmpty(node.InnerXml))
                            {
                                stlAnchor.InnerHtml = siblingContentInfo.Title;
                                if (wordNum > 0)
                                {
                                    stlAnchor.InnerHtml = StringUtils.MaxLengthText(stlAnchor.InnerHtml, wordNum);
                                }
                            }
                            else
                            {
                                StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                                contextInfo.ContentID = siblingContentID;
                                StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                                stlAnchor.InnerHtml = innerBuilder.ToString();
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(stlAnchor.HRef))
                {
                    if (isDisplayIfEmpty)
                    {
                        if (!string.IsNullOrEmpty(node.InnerXml))
                        {
                            StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            parsedContent = innerBuilder.ToString();
                        }
                        else
                        {
                            parsedContent = emptyText;
                        }
                    }
                    else
                    {
                        parsedContent = emptyText;
                    }
                }
                else
                {
                    parsedContent = ControlUtils.GetControlRenderHtml(stlAnchor);
                }
            }
            else
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID);

                bool isSuccess = false;
                ContextInfo theContextInfo = contextInfo.Clone();

                if (type.ToLower().Equals(Type_PreviousChannel.ToLower()) || type.ToLower().Equals(Type_NextChannel.ToLower()))
                {
                    int taxis = nodeInfo.Taxis;
                    bool isNextChannel = true;
                    if (StringUtils.EqualsIgnoreCase(type, Type_PreviousChannel))
                    {
                        isNextChannel = false;
                    }
                    int siblingNodeID = DataProvider.NodeDAO.GetNodeIDByParentIDAndTaxis(nodeInfo.ParentID, taxis, isNextChannel);
                    if (siblingNodeID != 0)
                    {
                        isSuccess = true;
                        theContextInfo.ContextType = EContextType.Channel;
                        theContextInfo.ChannelID = siblingNodeID;
                    }
                }
                else if (type.ToLower().Equals(Type_PreviousContent.ToLower()) || type.ToLower().Equals(Type_NextContent.ToLower()))
                {
                    if (contextInfo.ContentID != 0)
                    {
                        int taxis = contextInfo.ContentInfo.Taxis;
                        bool isNextContent = true;
                        if (StringUtils.EqualsIgnoreCase(type, Type_PreviousContent))
                        {
                            isNextContent = false;
                        }
                        string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
                        int siblingContentID = BaiRongDataProvider.ContentDAO.GetContentID(tableName, contextInfo.ChannelID, taxis, isNextContent);
                        if (siblingContentID != 0)
                        {
                            isSuccess = true;
                            theContextInfo.ContextType = EContextType.Content;
                            theContextInfo.ContentID = siblingContentID;
                            theContextInfo.ContentInfo = null;
                        }
                    }
                }

                if (isSuccess)
                {
                    parsedContent = successTemplateString;
                }
                else
                {
                    parsedContent = failureTemplateString;
                }

                if (!string.IsNullOrEmpty(parsedContent))
                {
                    StringBuilder innerBuilder = new StringBuilder(parsedContent);
                    StlParserManager.ParseInnerContent(innerBuilder, pageInfo, theContextInfo);

                    parsedContent = innerBuilder.ToString();
                }
            }

            parsedContent = tipText + parsedContent;

            return parsedContent;
        }
    }
}
