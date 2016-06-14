using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System.Web.UI;
using System;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.STL.Parser.StlEntity;

namespace SiteServer.STL.Parser.StlElement
{
    /*
<stl:if 
	testType="[Attribute]|TemplateName|ChannelName|TopLevel|UpLevel|UpChannel|SelfOrUpChannel"
	testOperate="Equals|NotEquals|Empty|NotEmpty|GreatThan|LessThan|In"
	testValue=""
	context=""
>
</stl:if>
     * */
    public class StlIf
    {
        private StlIf() { }
        public const string ElementName = "stl:if";//条件判断

        public const string Attribute_Type = "type";			        //测试类型
        public const string Attribute_Operate = "operate";				//测试操作
        public const string Attribute_Value = "value";				    //测试值
        public const string Attribute_Context = "context";                      //所处上下文
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

        public const string TestOperate_Empty = "Empty";			                    //值为空
        public const string TestOperate_NotEmpty = "NotEmpty";			                //值不为空
        public const string TestOperate_Equals = "Equals";			                    //值等于
        public const string TestOperate_NotEquals = "NotEquals";			            //值不等于
        public const string TestOperate_GreatThan = "GreatThan";			            //值大于
        public const string TestOperate_LessThan = "LessThan";			                //值小于
        public const string TestOperate_In = "In";			                            //值属于
        public const string TestOperate_NotIn = "NotIn";			                    //值不属于

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();

                attributes.Add(Attribute_Type, "测试类型");
                attributes.Add(Attribute_Operate, "测试操作");
                attributes.Add(Attribute_Value, "测试值");
                attributes.Add(Attribute_Context, "所处上下文");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");

                return attributes;
            }
        }

        internal static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
        {
            string parsedContent = string.Empty;
            ContextInfo contextInfo = contextInfoRef.Clone();
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();

                string testTypeStr = string.Empty;
                string testOperate = StlIf.TestOperate_Equals;
                string testValue = string.Empty;
                bool isDynamic = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlIf.Attribute_Type) || attributeName == "testtype")
                    {
                        testTypeStr = attr.Value;
                    }
                    else if (attributeName.Equals(StlIf.Attribute_Operate) || attributeName == "testoperate")
                    {
                        testOperate = attr.Value;
                    }
                    else if (attributeName.Equals(StlIf.Attribute_Value) || attributeName == "testvalue")
                    {
                        testValue = attr.Value;
                    }
                    else if (attributeName.Equals(StlIf.Attribute_Context))
                    {
                        contextInfo.ContextType = EContextTypeUtils.GetEnumType(attr.Value);
                    }
                    else if (attributeName.Equals(StlIf.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, testTypeStr, testOperate, testValue);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, string testTypeStr, string testOperate, string testValue)
        {
            string parsedContent = string.Empty;

            string successTemplateString = string.Empty;
            string failureTemplateString = string.Empty;

            StlParserUtility.GetInnerTemplateString(node, pageInfo, out successTemplateString, out failureTemplateString);

            bool isSuccess = false;
            ETestType testType = ETestTypeUtils.GetEnumType(testTypeStr);

            if (testType == ETestType.Undefined)
            {
                string theValue = GetAttributeValueByContext(pageInfo, contextInfo, testTypeStr);

                if (StringUtils.IsDateTime(theValue))
                {
                    DateTime dateTime = TranslateUtils.ToDateTime(theValue);
                    isSuccess = IsDateTime(dateTime, testOperate, testValue);
                }
                else if (StringUtils.IsNumber(theValue))
                {
                    int number = TranslateUtils.ToInt(theValue);
                    isSuccess = IsNumber(number, testOperate, testValue);
                }
                else
                {
                    if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotEmpty))
                    {
                        if (!string.IsNullOrEmpty(theValue))
                        {
                            isSuccess = true;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_Empty))
                    {
                        if (string.IsNullOrEmpty(theValue))
                        {
                            isSuccess = true;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_Equals))
                    {
                        if (StringUtils.EqualsIgnoreCase(theValue, testValue))
                        {
                            isSuccess = true;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotEquals))
                    {
                        if (!StringUtils.EqualsIgnoreCase(theValue, testValue))
                        {
                            isSuccess = true;
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_GreatThan))
                    {
                        if (StringUtils.Contains(theValue, "-"))
                        {
                            if (TranslateUtils.ToDateTime(theValue) > TranslateUtils.ToDateTime(testValue))
                            {
                                isSuccess = true;
                            }
                        }
                        else
                        {
                            if (TranslateUtils.ToInt(theValue) > TranslateUtils.ToInt(testValue))
                            {
                                isSuccess = true;
                            }
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_LessThan))
                    {
                        if (StringUtils.Contains(theValue, "-"))
                        {
                            if (TranslateUtils.ToDateTime(theValue) < TranslateUtils.ToDateTime(testValue))
                            {
                                isSuccess = true;
                            }
                        }
                        else
                        {
                            if (TranslateUtils.ToInt(theValue) < TranslateUtils.ToInt(testValue))
                            {
                                isSuccess = true;
                            }
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_In))
                    {
                        ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);

                        foreach (string str in stringArrayList)
                        {
                            if (StringUtils.EndsWithIgnoreCase(str, "*"))
                            {
                                string theStr = str.Substring(0, str.Length - 1);
                                if (StringUtils.StartsWithIgnoreCase(theValue, theStr))
                                {
                                    isSuccess = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (StringUtils.EqualsIgnoreCase(theValue, str))
                                {
                                    isSuccess = true;
                                    break;
                                }
                            }
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotIn))
                    {
                        ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);

                        bool isIn = false;
                        foreach (string str in stringArrayList)
                        {
                            if (StringUtils.EndsWithIgnoreCase(str, "*"))
                            {
                                string theStr = str.Substring(0, str.Length - 1);
                                if (StringUtils.StartsWithIgnoreCase(theValue, theStr))
                                {
                                    isIn = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (StringUtils.EqualsIgnoreCase(theValue, str))
                                {
                                    isIn = true;
                                    break;
                                }
                            }
                        }
                        if (!isIn)
                        {
                            isSuccess = true;
                        }
                    }
                }
            }
            else if (testType == ETestType.ChannelName)
            {
                string channelName = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID).NodeName;
                if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_Equals))
                {
                    if (StringUtils.EndsWithIgnoreCase(testValue, "*"))
                    {
                        string theStr = testValue.Substring(0, testValue.Length - 1);
                        if (StringUtils.StartsWithIgnoreCase(channelName, theStr))
                        {
                            isSuccess = true;
                        }
                    }
                    else
                    {
                        if (StringUtils.EqualsIgnoreCase(channelName, testValue))
                        {
                            isSuccess = true;
                        }
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotEquals))
                {
                    if (!StringUtils.EqualsIgnoreCase(channelName, testValue))
                    {
                        isSuccess = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_In))
                {
                    ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);

                    foreach (string str in stringArrayList)
                    {
                        if (StringUtils.EndsWithIgnoreCase(str, "*"))
                        {
                            string theStr = str.Substring(0, str.Length - 1);
                            if (StringUtils.StartsWithIgnoreCase(channelName, theStr))
                            {
                                isSuccess = true;
                                break;
                            }
                        }
                        else
                        {
                            if (StringUtils.EqualsIgnoreCase(channelName, str))
                            {
                                isSuccess = true;
                                break;
                            }
                        }
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotIn))
                {
                    ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);

                    bool isIn = false;
                    foreach (string str in stringArrayList)
                    {
                        if (StringUtils.EndsWithIgnoreCase(str, "*"))
                        {
                            string theStr = str.Substring(0, str.Length - 1);
                            if (StringUtils.StartsWithIgnoreCase(channelName, theStr))
                            {
                                isIn = true;
                                break;
                            }
                        }
                        else
                        {
                            if (StringUtils.EqualsIgnoreCase(channelName, str))
                            {
                                isIn = true;
                                break;
                            }
                        }
                    }
                    if (!isIn)
                    {
                        isSuccess = true;
                    }
                }
            }
            else if (testType == ETestType.ChannelIndex)
            {
                string channelIndex = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID).NodeIndexName;
                if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_Equals))
                {
                    if (StringUtils.EndsWithIgnoreCase(testValue, "*"))
                    {
                        string theStr = testValue.Substring(0, testValue.Length - 1);
                        if (StringUtils.StartsWithIgnoreCase(channelIndex, theStr))
                        {
                            isSuccess = true;
                        }
                    }
                    else
                    {
                        if (StringUtils.EqualsIgnoreCase(channelIndex, testValue))
                        {
                            isSuccess = true;
                        }
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotEquals))
                {
                    if (!StringUtils.EqualsIgnoreCase(channelIndex, testValue))
                    {
                        isSuccess = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_In))
                {
                    ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);

                    foreach (string str in stringArrayList)
                    {
                        if (StringUtils.EndsWithIgnoreCase(str, "*"))
                        {
                            string theStr = str.Substring(0, str.Length - 1);
                            if (StringUtils.StartsWithIgnoreCase(channelIndex, theStr))
                            {
                                isSuccess = true;
                                break;
                            }
                        }
                        else
                        {
                            if (StringUtils.EqualsIgnoreCase(channelIndex, str))
                            {
                                isSuccess = true;
                                break;
                            }
                        }
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotIn))
                {
                    ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);

                    bool isIn = false;
                    foreach (string str in stringArrayList)
                    {
                        if (StringUtils.EndsWithIgnoreCase(str, "*"))
                        {
                            string theStr = str.Substring(0, str.Length - 1);
                            if (StringUtils.StartsWithIgnoreCase(channelIndex, theStr))
                            {
                                isIn = true;
                                break;
                            }
                        }
                        else
                        {
                            if (StringUtils.EqualsIgnoreCase(channelIndex, str))
                            {
                                isIn = true;
                                break;
                            }
                        }
                    }
                    if (!isIn)
                    {
                        isSuccess = true;
                    }
                }
            }
            else if (testType == ETestType.TemplateName)
            {
                if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_Equals))
                {
                    if (StringUtils.EndsWithIgnoreCase(testValue, "*"))
                    {
                        string theStr = testValue.Substring(0, testValue.Length - 1);
                        if (StringUtils.StartsWithIgnoreCase(pageInfo.TemplateInfo.TemplateName, theStr))
                        {
                            isSuccess = true;
                        }
                    }
                    else
                    {
                        if (StringUtils.EqualsIgnoreCase(pageInfo.TemplateInfo.TemplateName, testValue))
                        {
                            isSuccess = true;
                        }
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotEquals))
                {
                    if (!StringUtils.EqualsIgnoreCase(pageInfo.TemplateInfo.TemplateName, testValue))
                    {
                        isSuccess = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_In))
                {
                    ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);
                    foreach (string str in stringArrayList)
                    {
                        if (StringUtils.EndsWithIgnoreCase(str, "*"))
                        {
                            string theStr = str.Substring(0, str.Length - 1);
                            if (StringUtils.StartsWithIgnoreCase(pageInfo.TemplateInfo.TemplateName, theStr))
                            {
                                isSuccess = true;
                                break;
                            }
                        }
                        else
                        {
                            if (StringUtils.EqualsIgnoreCase(pageInfo.TemplateInfo.TemplateName, str))
                            {
                                isSuccess = true;
                                break;
                            }
                        }
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotIn))
                {
                    ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);
                    bool isIn = false;
                    foreach (string str in stringArrayList)
                    {
                        if (StringUtils.EndsWithIgnoreCase(str, "*"))
                        {
                            string theStr = str.Substring(0, str.Length - 1);
                            if (StringUtils.StartsWithIgnoreCase(pageInfo.TemplateInfo.TemplateName, theStr))
                            {
                                isIn = true;
                                break;
                            }
                        }
                        else
                        {
                            if (StringUtils.EqualsIgnoreCase(pageInfo.TemplateInfo.TemplateName, str))
                            {
                                isIn = true;
                                break;
                            }
                        }
                    }
                    if (!isIn)
                    {
                        isSuccess = true;
                    }
                }
            }
            else if (testType == ETestType.TemplateType)
            {
                if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_Equals))
                {
                    if (ETemplateTypeUtils.Equals(pageInfo.TemplateInfo.TemplateType, testValue))
                    {
                        isSuccess = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotEquals))
                {
                    if (!ETemplateTypeUtils.Equals(pageInfo.TemplateInfo.TemplateType, testValue))
                    {
                        isSuccess = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_In))
                {
                    ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);
                    foreach (string str in stringArrayList)
                    {
                        if (ETemplateTypeUtils.Equals(pageInfo.TemplateInfo.TemplateType, str))
                        {
                            isSuccess = true;
                            break;
                        }
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotIn))
                {
                    ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);
                    bool isIn = false;
                    foreach (string str in stringArrayList)
                    {
                        if (ETemplateTypeUtils.Equals(pageInfo.TemplateInfo.TemplateType, str))
                        {
                            isIn = true;
                            break;
                        }
                    }
                    if (!isIn)
                    {
                        isSuccess = true;
                    }
                }
            }
            else if (testType == ETestType.TopLevel)
            {
                int topLevel = NodeManager.GetTopLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID);
                isSuccess = IsNumber(topLevel, testOperate, testValue);
            }
            else if (testType == ETestType.UpChannel)
            {
                if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotIn))
                {
                    ArrayList channelIndexes = TranslateUtils.StringCollectionToArrayList(testValue);
                    bool isIn = false;
                    foreach (string channelIndex in channelIndexes)
                    {
                        int parentID = DataProvider.NodeDAO.GetNodeIDByNodeIndexName(pageInfo.PublishmentSystemID, channelIndex);
                        if (parentID != pageInfo.PageNodeID && ChannelUtility.IsAncestorOrSelf(pageInfo.PublishmentSystemID, parentID, pageInfo.PageNodeID))
                        {
                            isIn = true;
                            break;
                        }
                    }
                    if (!isIn)
                    {
                        isSuccess = true;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(testValue))
                    {
                        if (contextInfo.ChannelID != pageInfo.PageNodeID && ChannelUtility.IsAncestorOrSelf(pageInfo.PublishmentSystemID, contextInfo.ChannelID, pageInfo.PageNodeID))
                        {
                            isSuccess = true;
                        }
                    }
                    else
                    {
                        ArrayList channelIndexes = TranslateUtils.StringCollectionToArrayList(testValue);
                        foreach (string channelIndex in channelIndexes)
                        {
                            int parentID = DataProvider.NodeDAO.GetNodeIDByNodeIndexName(pageInfo.PublishmentSystemID, channelIndex);
                            if (parentID != pageInfo.PageNodeID && ChannelUtility.IsAncestorOrSelf(pageInfo.PublishmentSystemID, parentID, pageInfo.PageNodeID))
                            {
                                isSuccess = true;
                                break;
                            }
                        }
                    }
                }
            }
            else if (testType == ETestType.UpChannelOrSelf)
            {
                if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_In))
                {
                    ArrayList channelIndexes = TranslateUtils.StringCollectionToArrayList(testValue);
                    bool isIn = false;
                    foreach (string channelIndex in channelIndexes)
                    {
                        int parentID = DataProvider.NodeDAO.GetNodeIDByNodeIndexName(pageInfo.PublishmentSystemID, channelIndex);
                        if (ChannelUtility.IsAncestorOrSelf(pageInfo.PublishmentSystemID, parentID, pageInfo.PageNodeID))
                        {
                            isIn = true;
                            break;
                        }
                    }
                    if (isIn)
                    {
                        isSuccess = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotIn))
                {
                    ArrayList channelIndexes = TranslateUtils.StringCollectionToArrayList(testValue);
                    bool isIn = false;
                    foreach (string channelIndex in channelIndexes)
                    {
                        int parentID = DataProvider.NodeDAO.GetNodeIDByNodeIndexName(pageInfo.PublishmentSystemID, channelIndex);
                        if (ChannelUtility.IsAncestorOrSelf(pageInfo.PublishmentSystemID, parentID, pageInfo.PageNodeID))
                        {
                            isIn = true;
                            break;
                        }
                    }
                    if (!isIn)
                    {
                        isSuccess = true;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(testValue))
                    {
                        if (ChannelUtility.IsAncestorOrSelf(pageInfo.PublishmentSystemID, contextInfo.ChannelID, pageInfo.PageNodeID))
                        {
                            isSuccess = true;
                        }
                    }
                    else
                    {
                        ArrayList channelIndexes = TranslateUtils.StringCollectionToArrayList(testValue);
                        foreach (string channelIndex in channelIndexes)
                        {
                            int parentID = DataProvider.NodeDAO.GetNodeIDByNodeIndexName(pageInfo.PublishmentSystemID, channelIndex);
                            if (ChannelUtility.IsAncestorOrSelf(pageInfo.PublishmentSystemID, parentID, pageInfo.PageNodeID))
                            {
                                isSuccess = true;
                                break;
                            }
                        }
                    }
                }
            }
            else if (testType == ETestType.GroupChannel)
            {
                ArrayList groupChannels = TranslateUtils.StringCollectionToArrayList(NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID).NodeGroupNameCollection);

                if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_Equals) || StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_In))
                {
                    ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);

                    foreach (string str in stringArrayList)
                    {
                        if (groupChannels.Contains(str))
                        {
                            isSuccess = true;
                            break;
                        }
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotEquals) || StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotIn))
                {
                    ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);

                    bool isIn = false;
                    foreach (string str in stringArrayList)
                    {
                        if (groupChannels.Contains(str))
                        {
                            isIn = true;
                            break;
                        }
                    }
                    if (!isIn)
                    {
                        isSuccess = true;
                    }
                }
            }
            else if (testType == ETestType.GroupContent)
            {
                if (contextInfo.ContextType == EContextType.Content)
                {
                    string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
                    ArrayList groupContents = TranslateUtils.StringCollectionToArrayList(BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, ContentAttribute.ContentGroupNameCollection));

                    if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_Equals) || StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_In))
                    {
                        ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);

                        foreach (string str in stringArrayList)
                        {
                            if (groupContents.Contains(str))
                            {
                                isSuccess = true;
                                break;
                            }
                        }
                    }
                    else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotEquals) || StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotIn))
                    {
                        ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);

                        bool isIn = false;
                        foreach (string str in stringArrayList)
                        {
                            if (groupContents.Contains(str))
                            {
                                isIn = true;
                                break;
                            }
                        }
                        if (!isIn)
                        {
                            isSuccess = true;
                        }
                    }
                }
            }
            else if (testType == ETestType.AddDate)
            {
                DateTime addDate = GetAddDateByContext(pageInfo, contextInfo);
                isSuccess = IsDateTime(addDate, testOperate, testValue);
            }
            else if (testType == ETestType.LastEditDate)
            {
                DateTime lastEditDate = GetLastEditDateByContext(pageInfo, contextInfo);
                isSuccess = IsDateTime(lastEditDate, testOperate, testValue);
            }
            else if (testType == ETestType.ItemIndex)
            {
                int itemIndex = StlParserUtility.GetItemIndex(contextInfo);
                isSuccess = IsNumber(itemIndex, testOperate, testValue);
            }
            else if (testType == ETestType.OddItem)
            {
                int itemIndex = StlParserUtility.GetItemIndex(contextInfo);
                isSuccess = itemIndex % 2 == 1;
            }
            else if (testType == ETestType.OddItem)
            {
                int itemIndex = StlParserUtility.GetItemIndex(contextInfo);
                isSuccess = itemIndex % 2 == 1;
            }
            else if (testType == ETestType.IsAnonymous)
            {
                StringBuilder succssBuilder = new StringBuilder(StlUserEntities.ReplaceEntityToArtTemplate(successTemplateString));
                StringBuilder failureBuilder = new StringBuilder(StlUserEntities.ReplaceEntityToArtTemplate(failureTemplateString));

                StlParserManager.ParseInnerContent(succssBuilder, pageInfo, contextInfo);
                StlParserManager.ParseInnerContent(failureBuilder, pageInfo, contextInfo);

                if (EPublishmentSystemTypeUtils.IsB2C(pageInfo.PublishmentSystemInfo.PublishmentSystemType))
                {
                    if (!string.IsNullOrEmpty(successTemplateString) && !string.IsNullOrEmpty(failureTemplateString))
                    {
                        return string.Format(@"
<script class=""b2cController"" type=""text/html"">
    <% if (isAnonymous) {{ %>
        {0}
    <% }}else{{ %>
        {1}
    <% }} %>
</script>
", succssBuilder, failureBuilder);
                    }
                    else if (!string.IsNullOrEmpty(successTemplateString))
                    {
                        bool isAnonymous = TranslateUtils.ToBool(testValue, true);
                        return string.Format(@"
<script class=""b2cController"" type=""text/html"">
    <% if ({0}isAnonymous) {{ %>
        {1}
    <% }} %>
</script>
", isAnonymous ? string.Empty : "!", succssBuilder);
                    }
                }
                else
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC);
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_USER);

                    if (!string.IsNullOrEmpty(successTemplateString) && !string.IsNullOrEmpty(failureTemplateString))
                    {
                        return string.Format(@"
<script class=""userController"" type=""text/html"">
    <% if (isAnonymous) {{ %>
        {0}
    <% }}else{{ %>
        {1}
    <% }} %>
</script>
", succssBuilder, failureBuilder);
                    }
                    else if (!string.IsNullOrEmpty(successTemplateString))
                    {
                        bool isAnonymous = TranslateUtils.ToBool(testValue, true);
                        return string.Format(@"
<script class=""userController"" type=""text/html"">
    <% if ({0}isAnonymous) {{ %>
        {1}
    <% }} %>
</script>
", isAnonymous ? string.Empty : "!", succssBuilder);
                    }
                }
            }
            else if (testType == ETestType.IsCart)
            {
                if (EPublishmentSystemTypeUtils.IsB2C(pageInfo.PublishmentSystemInfo.PublishmentSystemType))
                {
                    StringBuilder succssBuilder = new StringBuilder(StlUserEntities.ReplaceEntityToArtTemplate(successTemplateString));
                    StringBuilder failureBuilder = new StringBuilder(StlUserEntities.ReplaceEntityToArtTemplate(failureTemplateString));

                    StlParserManager.ParseInnerContent(succssBuilder, pageInfo, contextInfo);
                    StlParserManager.ParseInnerContent(failureBuilder, pageInfo, contextInfo);

                    if (!string.IsNullOrEmpty(successTemplateString) && !string.IsNullOrEmpty(failureTemplateString))
                    {
                        return string.Format(@"
<script class=""b2cController"" type=""text/html"">
    <% if (carts && carts.length > 0) {{ %>
        {0}
    <% }}else{{ %>
        {1}
    <% }} %>
</script>
", succssBuilder, failureBuilder);
                    }
                    else if (!string.IsNullOrEmpty(successTemplateString))
                    {
                        bool isCarts = TranslateUtils.ToBool(testValue, true);
                        if (isCarts)
                        {
                            return string.Format(@"
<script class=""b2cController"" type=""text/html"">
    <% if (carts && carts.length > 0) {{ %>
        {0}
    <% }} %>
</script>
", succssBuilder);
                        }
                        else
                        {
                            return string.Format(@"
<script class=""b2cController"" type=""text/html"">
    <% if (carts && carts.length == 0) {{ %>
        {0}
    <% }} %>
</script>
", succssBuilder);
                        }
                    }
                }
                return string.Empty;
            }
            else if (testType == ETestType.IsCurrent)
            {
                StringBuilder succssBuilder = new StringBuilder(StlUserEntities.ReplaceEntityToArtTemplate(successTemplateString));
                StringBuilder failureBuilder = new StringBuilder(StlUserEntities.ReplaceEntityToArtTemplate(failureTemplateString));

                StlParserManager.ParseInnerContent(succssBuilder, pageInfo, contextInfo);
                StlParserManager.ParseInnerContent(failureBuilder, pageInfo, contextInfo);

                if (StringUtils.EqualsIgnoreCase(testValue, "isSelectMode"))
                {
                    return string.Format(@"
<% if(consigneeSelectMode || paymentShipmentSelectMode || invoiceSelectMode){{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "Consignee"))
                {
                    return string.Format(@"
<% if (item.id == consignee.id) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "ConsigneeView"))
                {
                    return string.Format(@"
<% if (!consigneeSelectMode) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "ConsigneeAdd"))
                {
                    return string.Format(@"
<% if (consignee.id == 0) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "ConsigneeEdit"))
                {
                    return string.Format(@"
<% if (consigneeAddOrEditMode) {{ %>
    {0}
<% }} %>
", succssBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "PaymentShipmentView"))
                {
                    return string.Format(@"
<% if (!paymentShipmentSelectMode) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "InvoiceView"))
                {
                    return string.Format(@"
<% if (!invoiceSelectMode) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "InvoiceAdd"))
                {
                    return string.Format(@"
<% if (invoice.id == 0) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "InvoiceEdit"))
                {
                    return string.Format(@"
<% if (invoiceAddOrEditMode) {{ %>
    {0}
<% }} %>
", succssBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "InvoiceIsVat"))
                {
                    return string.Format(@"
<% if (invoice.isVat) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "InvoiceIsCompany"))
                {
                    return string.Format(@"
<% if (invoice.isCompany) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "Payment"))
                {
                    return string.Format(@"
<% if (item.id == payment.id) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "Shipment"))
                {
                    return string.Format(@"
<% if (item.id == shipment.id) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "Invoice"))
                {
                    return string.Format(@"
<% if (item.id == invoice.id) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "Filter"))
                {
                    string success = successTemplateString.Replace("{b2c.clickFilter}", "<%=getUrl(filter.filterID, 0)%>");
                    string failure = failureTemplateString.Replace("{b2c.clickFilter}", "<%=getUrl(filter.filterID, 0)%>");

                    return string.Format(@"
<% if (isFilter(filter.filterID, 0)) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", success, failure);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "FilterItem"))
                {
                    string success = successTemplateString.Replace("{b2c.clickFilter}", "<%=getUrl(item.filterID, item.itemID)%>");
                    string failure = failureTemplateString.Replace("{b2c.clickFilter}", "<%=getUrl(item.filterID, item.itemID)%>");

                    return string.Format(@"
<% if (isFilter(item.filterID, item.itemID)) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", success, failure);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "OrderDefault"))
                {
                    string success = successTemplateString.Replace("{b2c.clickOrderDefault}", "<%=getUrl(0, 0, 'default')%>");
                    string failure = failureTemplateString.Replace("{b2c.clickOrderDefault}", "<%=getUrl(0, 0, 'default')%>");

                    return string.Format(@"
<% if (isOrder('default')) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", success, failure);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "OrderSales"))
                {
                    string success = successTemplateString.Replace("{b2c.clickOrderSales}", "<%=getUrl(0, 0, 'Sales')%>");
                    string failure = failureTemplateString.Replace("{b2c.clickOrderSales}", "<%=getUrl(0, 0, 'Sales')%>");

                    return string.Format(@"
<% if (isOrder('Sales')) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", success, failure);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "OrderPriceSale"))
                {
                    string success = successTemplateString.Replace("{b2c.clickOrderPriceSale}", "<%=getUrl(0, 0, 'PriceSale')%>");
                    string failure = failureTemplateString.Replace("{b2c.clickOrderPriceSale}", "<%=getUrl(0, 0, 'PriceSale')%>");

                    return string.Format(@"
<% if (isOrder('PriceSale')) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", success, failure);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "OrderComments"))
                {
                    string success = successTemplateString.Replace("{b2c.clickOrderComments}", "<%=getUrl(0, 0, 'Comments')%>");
                    string failure = failureTemplateString.Replace("{b2c.clickOrderComments}", "<%=getUrl(0, 0, 'Comments')%>");

                    return string.Format(@"
<% if (isOrder('Comments')) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", success, failure);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "OrderAddDate"))
                {
                    string success = successTemplateString.Replace("{b2c.clickOrderAddDate}", "<%=getUrl(0, 0, 'AddDate')%>");
                    string failure = failureTemplateString.Replace("{b2c.clickOrderAddDate}", "<%=getUrl(0, 0, 'AddDate')%>");

                    return string.Format(@"
<% if (isOrder('AddDate')) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", success, failure);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "FilterPreviousPage"))
                {
                    string success = successTemplateString.Replace("{b2c.clickFilterPage}", "<%=getUrl(0, 0, '', page - 1)%>");

                    return string.Format(@"
<% if (pageItem.previousPage) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", success, failureTemplateString);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "FilterNextPage"))
                {
                    string success = successTemplateString.Replace("{b2c.clickFilterPage}", "<%=getUrl(0, 0, '', page + 1)%>");

                    return string.Format(@"
<% if (pageItem.nextPage) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", success, failureTemplateString);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "FilterPage"))
                {
                    return string.Format(@"
<% if (data.pageItem.totalPageNum > 1) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "FilterFirstPage"))
                {
                    string success = successTemplateString.Replace("{b2c.clickFilterPage}", "<%=getUrl(0, 0, '', 1)%>");

                    return string.Format(@"
<% if (pageItem.previousPage) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", success, failureTemplateString);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "FilterLastPage"))
                {
                    string success = successTemplateString.Replace("{b2c.clickFilterPage}", "getUrl(0, 0, '', data.pageItem.totalPageNum)");

                    return string.Format(@"
<% if (pageItem.previousPage) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", success, failureTemplateString);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "FilterCurrentPage"))
                {
                    return string.Format(@"
<% if (pageItem.currentPageIndex == pageNum) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "isRecommend"))
                {
                    return string.Format(@"
<% if (content.isrecommend) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "isNew"))
                {
                    return string.Format(@"
<% if (content.isnew) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else if (StringUtils.EqualsIgnoreCase(testValue, "isHot"))
                {
                    return string.Format(@"
<% if (content.ishot) {{ %>
    {0}
<% }}else{{ %>
    {1}
<% }} %>
", succssBuilder, failureBuilder);
                }
                else
                {
                    succssBuilder = new StringBuilder(successTemplateString);
                    failureBuilder = new StringBuilder(failureTemplateString);

                    StlParserManager.ParseInnerContent(succssBuilder, pageInfo, contextInfo);
                    StlParserManager.ParseInnerContent(failureBuilder, pageInfo, contextInfo);

                    return string.Format(@"
<% if ({0}) {{ %>
    {1}
<% }}else{{ %>
    {2}
<% }} %>
", testValue, succssBuilder, failureBuilder);
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
                StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

                parsedContent = innerBuilder.ToString();
            }

            return parsedContent;
        }

        private static string GetAttributeValueByContext(PageInfo pageInfo, ContextInfo contextInfo, string testTypeStr)
        {
            string theValue = null;
            if (contextInfo.ContextType == EContextType.Content)
            {
                theValue = GetValueFromContent(pageInfo, contextInfo, testTypeStr);
            }
            else if (contextInfo.ContextType == EContextType.Channel)
            {
                theValue = GetValueFromChannel(pageInfo, contextInfo, testTypeStr);
            }
            else if (contextInfo.ContextType == EContextType.Comment)
            {
                if (contextInfo.ItemContainer.CommentItem != null)
                {
                    theValue = DataBinder.Eval(contextInfo.ItemContainer.CommentItem.DataItem, testTypeStr, "{0}");
                }
            }
            else if (contextInfo.ContextType == EContextType.InputContent)
            {
                if (contextInfo.ItemContainer.InputItem != null)
                {
                    theValue = DataBinder.Eval(contextInfo.ItemContainer.InputItem.DataItem, testTypeStr, "{0}");
                }
            }
            else if (contextInfo.ContextType == EContextType.SqlContent)
            {
                if (contextInfo.ItemContainer.SqlItem != null)
                {
                    theValue = DataBinder.Eval(contextInfo.ItemContainer.SqlItem.DataItem, testTypeStr, "{0}");
                }
            }
            else if (contextInfo.ContextType == EContextType.Site)
            {
                if (contextInfo.ItemContainer.SiteItem != null)
                {
                    theValue = DataBinder.Eval(contextInfo.ItemContainer.SiteItem.DataItem, testTypeStr, "{0}");
                }
            }
            else
            {
                if (contextInfo.ItemContainer != null)
                {
                    if (contextInfo.ItemContainer.CommentItem != null)
                    {
                        theValue = DataBinder.Eval(contextInfo.ItemContainer.CommentItem.DataItem, testTypeStr, "{0}");
                    }
                    else if (contextInfo.ItemContainer.InputItem != null)
                    {
                        theValue = DataBinder.Eval(contextInfo.ItemContainer.InputItem.DataItem, testTypeStr, "{0}");
                    }
                    else if (contextInfo.ItemContainer.ContentItem != null)
                    {
                        theValue = DataBinder.Eval(contextInfo.ItemContainer.ContentItem.DataItem, testTypeStr, "{0}");
                    }
                    else if (contextInfo.ItemContainer.ChannelItem != null)
                    {
                        theValue = DataBinder.Eval(contextInfo.ItemContainer.ChannelItem.DataItem, testTypeStr, "{0}");
                    }
                    else if (contextInfo.ItemContainer.SqlItem != null)
                    {
                        theValue = DataBinder.Eval(contextInfo.ItemContainer.SqlItem.DataItem, testTypeStr, "{0}");
                    }
                }
                else if (contextInfo.ContentID != 0)//获取内容
                {
                    theValue = GetValueFromContent(pageInfo, contextInfo, testTypeStr);
                }
                else if (contextInfo.ChannelID != 0)//获取栏目
                {
                    theValue = GetValueFromChannel(pageInfo, contextInfo, testTypeStr);
                }
            }

            if (theValue == null)
            {
                theValue = string.Empty;
            }
            return theValue;
        }

        private static string GetValueFromChannel(PageInfo pageInfo, ContextInfo contextInfo, string testTypeStr)
        {
            string theValue = null;

            NodeInfo channel = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID);

            if (StringUtils.EqualsIgnoreCase(NodeAttribute.AddDate, testTypeStr))
            {
                theValue = channel.AddDate.ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(NodeAttribute.Title, testTypeStr))
            {
                theValue = channel.NodeName;
            }
            else if (StringUtils.EqualsIgnoreCase(NodeAttribute.ImageUrl, testTypeStr))
            {
                theValue = channel.ImageUrl;
            }
            else if (StringUtils.EqualsIgnoreCase(NodeAttribute.Content, testTypeStr))
            {
                theValue = channel.Content;
            }
            else if (StringUtils.EqualsIgnoreCase(NodeAttribute.CountOfChannels, testTypeStr))
            {
                theValue = channel.ChildrenCount.ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(NodeAttribute.CountOfContents, testTypeStr))
            {
                theValue = channel.ContentNum.ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(NodeAttribute.CountOfImageContents, testTypeStr))
            {
                int count = DataProvider.BackgroundContentDAO.GetCountCheckedImage(pageInfo.PublishmentSystemID, channel.NodeID);
                theValue = count.ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(NodeAttribute.LinkUrl, testTypeStr))
            {
                theValue = channel.LinkUrl;
            }
            else
            {
                theValue = channel.Additional.Attributes[testTypeStr];
            }
            return theValue;
        }

        private static string GetValueFromContent(PageInfo pageInfo, ContextInfo contextInfo, string testTypeStr)
        {
            string theValue = null;

            if (contextInfo.ItemContainer != null && contextInfo.ItemContainer.ContentItem != null)
            {
                theValue = TranslateUtils.Eval(contextInfo.ItemContainer.ContentItem.DataItem, testTypeStr) as string;
            }

            if (theValue == null)
            {
                if (contextInfo.ContentInfo == null)
                {
                    string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
                    theValue = BaiRongDataProvider.ContentDAO.GetValue(tableName, contextInfo.ContentID, testTypeStr);
                }
                else
                {
                    theValue = contextInfo.ContentInfo.GetExtendedAttribute(testTypeStr);
                }
            }
            return theValue;
        }

        private static DateTime GetAddDateByContext(PageInfo pageInfo, ContextInfo contextInfo)
        {
            DateTime addDate = DateUtils.SqlMinValue;

            if (contextInfo.ContextType == EContextType.Content)
            {
                if (contextInfo.ContentInfo == null)
                {
                    string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
                    addDate = BaiRongDataProvider.ContentDAO.GetAddDate(tableName, contextInfo.ContentID);
                }
                else
                {
                    addDate = contextInfo.ContentInfo.AddDate;
                }
            }
            else if (contextInfo.ContextType == EContextType.Channel)
            {
                NodeInfo channel = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID);

                addDate = channel.AddDate;
            }
            else if (contextInfo.ContextType == EContextType.Comment)
            {
                if (contextInfo.ItemContainer.CommentItem != null)
                {
                    addDate = (DateTime)DataBinder.Eval(contextInfo.ItemContainer.CommentItem.DataItem, CommentAttribute.AddDate);
                }
            }
            else if (contextInfo.ContextType == EContextType.InputContent)
            {
                if (contextInfo.ItemContainer.InputItem != null)
                {
                    addDate = (DateTime)DataBinder.Eval(contextInfo.ItemContainer.InputItem.DataItem, InputContentAttribute.AddDate);
                }
            }
            else
            {
                if (contextInfo.ItemContainer != null)
                {
                    if (contextInfo.ItemContainer.CommentItem != null)
                    {
                        addDate = (DateTime)DataBinder.Eval(contextInfo.ItemContainer.CommentItem.DataItem, CommentAttribute.AddDate);
                    }
                    else if (contextInfo.ItemContainer.InputItem != null)
                    {
                        addDate = (DateTime)DataBinder.Eval(contextInfo.ItemContainer.InputItem.DataItem, InputContentAttribute.AddDate);
                    }
                    else if (contextInfo.ItemContainer.ContentItem != null)
                    {
                        addDate = (DateTime)DataBinder.Eval(contextInfo.ItemContainer.ContentItem.DataItem, ContentAttribute.AddDate);
                    }
                    else if (contextInfo.ItemContainer.ChannelItem != null)
                    {
                        addDate = (DateTime)DataBinder.Eval(contextInfo.ItemContainer.ChannelItem.DataItem, NodeAttribute.AddDate);
                    }
                    else if (contextInfo.ItemContainer.SqlItem != null)
                    {
                        addDate = (DateTime)DataBinder.Eval(contextInfo.ItemContainer.SqlItem.DataItem, "AddDate");
                    }
                }
                else if (contextInfo.ContentID != 0)//获取内容
                {
                    if (contextInfo.ContentInfo == null)
                    {
                        string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
                        addDate = BaiRongDataProvider.ContentDAO.GetAddDate(tableName, contextInfo.ContentID);
                    }
                    else
                    {
                        addDate = contextInfo.ContentInfo.AddDate;
                    }
                }
                else if (contextInfo.ChannelID != 0)//获取栏目
                {
                    NodeInfo channel = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID);
                    addDate = channel.AddDate;
                }
            }

            return addDate;
        }

        private static DateTime GetLastEditDateByContext(PageInfo pageInfo, ContextInfo contextInfo)
        {
            DateTime lastEditDate = DateUtils.SqlMinValue;

            if (contextInfo.ContextType == EContextType.Content)
            {
                if (contextInfo.ContentInfo == null)
                {
                    string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, contextInfo.ChannelID);
                    lastEditDate = BaiRongDataProvider.ContentDAO.GetLastEditDate(tableName, contextInfo.ContentID);
                }
                else
                {
                    lastEditDate = contextInfo.ContentInfo.LastEditDate;
                }
            }

            return lastEditDate;
        }

        private static bool IsIn(string testValue, string theValue)
        {
            bool isSuccess = false;

            ArrayList stringArrayList = TranslateUtils.StringCollectionToArrayList(testValue);

            bool isIn = false;
            foreach (string str in stringArrayList)
            {
                if (StringUtils.EndsWithIgnoreCase(str, "*"))
                {
                    string theStr = str.Substring(0, str.Length - 1);
                    if (StringUtils.StartsWithIgnoreCase(theValue, theStr))
                    {
                        isIn = true;
                        break;
                    }
                }
                else
                {
                    if (StringUtils.EqualsIgnoreCase(theValue, str))
                    {
                        isIn = true;
                        break;
                    }
                }
            }
            if (!isIn)
            {
                isSuccess = true;
            }
            return isSuccess;
        }

        private static bool IsNumber(int number, string testOperate, string testValue)
        {
            bool isSuccess = false;
            if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_Equals))
            {
                if (number == TranslateUtils.ToInt(testValue))
                {
                    isSuccess = true;
                }
            }
            else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotEquals))
            {
                if (number != TranslateUtils.ToInt(testValue))
                {
                    isSuccess = true;
                }
            }
            else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_GreatThan))
            {
                if (number > TranslateUtils.ToInt(testValue))
                {
                    isSuccess = true;
                }
            }
            else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_LessThan))
            {
                if (number < TranslateUtils.ToInt(testValue))
                {
                    isSuccess = true;
                }
            }
            else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_In))
            {
                ArrayList intArrayList = TranslateUtils.StringCollectionToIntArrayList(testValue);
                foreach (int i in intArrayList)
                {
                    if (i == number)
                    {
                        isSuccess = true;
                        break;
                    }
                }
            }
            else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_In))
            {
                ArrayList intArrayList = TranslateUtils.StringCollectionToIntArrayList(testValue);
                bool isIn = false;
                foreach (int i in intArrayList)
                {
                    if (i == number)
                    {
                        isIn = true;
                        break;
                    }
                }
                if (!isIn)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        private static bool IsDateTime(DateTime dateTime, string testOperate, string testValue)
        {
            bool isSuccess = false;

            DateTime dateTimeToTest;

            if (StringUtils.EqualsIgnoreCase("now", testValue))
            {
                dateTimeToTest = DateTime.Now;
            }
            else if (DateUtils.IsSince(testValue))
            {
                int hours = DateUtils.GetSinceHours(testValue);
                dateTimeToTest = DateTime.Now.AddHours(-hours);
            }
            else
            {
                dateTimeToTest = TranslateUtils.ToDateTime(testValue);
            }

            if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_Equals) || StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_In))
            {
                isSuccess = (dateTime.Date == dateTimeToTest.Date);
            }
            else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_GreatThan))
            {
                isSuccess = (dateTime > dateTimeToTest);
            }
            else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_LessThan))
            {
                isSuccess = (dateTime < dateTimeToTest);
            }
            else if (StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotEquals) || StringUtils.EqualsIgnoreCase(testOperate, StlIf.TestOperate_NotIn))
            {
                isSuccess = (dateTime.Date != dateTimeToTest.Date);
            }

            return isSuccess;
        }
    }
}
