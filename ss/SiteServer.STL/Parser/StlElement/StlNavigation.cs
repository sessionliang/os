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
        public const string ElementName = "stl:navigation";//显示导航

        public const string Attribute_Type = "type";							//导航的类型
        public const string Attribute_EmptyText = "emptytext";					//当无内容时显示的信息
        public const string Attribute_TipText = "tiptext";					    //导航提示信息
        public const string Attribute_WordNum = "wordnum";					    //显示字数
        public const string Attribute_IsDisplayIfEmpty = "isdisplayifempty";    //当没链接时是否显示
        public const string Attribute_IsDynamic = "isdynamic";                  //是否动态显示
        public const string Attribute_IsKeyboard = "iskeyboard";                //是否开启键盘，←→↑↓键分别为上（下）一内容（栏目）链接

        public const string Type_PreviousChannel = "PreviousChannel";			//上一栏目链接
        public const string Type_NextChannel = "NextChannel";					//下一栏目链接
        public const string Type_PreviousContent = "PreviousContent";			//上一内容链接
        public const string Type_NextContent = "NextContent";					//下一内容链接

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Type, "显示的类型");
                attributes.Add(Attribute_EmptyText, "当无内容时显示的信息");
                attributes.Add(Attribute_TipText, "导航提示信息");
                attributes.Add(Attribute_WordNum, "显示字数");
                attributes.Add(Attribute_IsDisplayIfEmpty, "当没链接时是否显示");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
                attributes.Add(Attribute_IsKeyboard, "是否开启键盘");
                return attributes;
            }
        }

        //对“导航”（stl:navigation）元素进行解析
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
