using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using System.Web.UI;
using System;
using BaiRong.Model;
using SiteServer.BBS.Core.TemplateParser.Model;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Core.TemplateParser.Element
{
    public class If
    {
        private If() { }
        public const string ElementName = "bbs:if";//条件判断

        public const string Attribute_TestType = "testtype";			        //测试类型
        public const string Attribute_TestOperate = "testoperate";				//测试操作
        public const string Attribute_TestValue = "testvalue";				    //测试值
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

                attributes.Add(Attribute_TestType, "测试类型");
                attributes.Add(Attribute_TestOperate, "测试操作");
                attributes.Add(Attribute_TestValue, "测试值");
                attributes.Add(Attribute_Context, "所处上下文");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");

                return attributes;
            }
        }

        public sealed class SuccessTemplate
        {
            public const string ElementName = "bbs:successtemplate";
        }

        public sealed class FailureTemplate
        {
            public const string ElementName = "bbs:failuretemplate";
        }

        internal static string Parse(string element, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
        {
            string parsedContent = string.Empty;
            ContextInfo contextInfo = contextInfoRef.Clone();
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();

                string testTypeStr = string.Empty;
                string testOperate = If.TestOperate_Equals;
                string testValue = string.Empty;
                bool isDynamic = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(If.Attribute_TestType))
                    {
                        testTypeStr = attr.Value;
                    }
                    else if (attributeName.Equals(If.Attribute_TestOperate))
                    {
                        testOperate = attr.Value;
                    }
                    else if (attributeName.Equals(If.Attribute_TestValue))
                    {
                        testValue = attr.Value;
                    }
                    else if (attributeName.Equals(If.Attribute_Context))
                    {
                        contextInfo.ContextType = EContextTypeUtils.GetEnumType(attr.Value);
                    }
                    else if (attributeName.Equals(If.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = Dynamic.ParseDynamicElement(ElementName, element, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(node, pageInfo, contextInfo, testTypeStr, testOperate, testValue);
                }
            }
            catch (Exception ex)
            {
                parsedContent = ParserUtility.GetErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, string testTypeStr, string testOperate, string testValue)
        {
            string parsedContent = string.Empty;

            string successTemplateString = string.Empty;
            string failureTemplateString = string.Empty;

            ParserUtility.GetInnerTemplateString(node, out successTemplateString, out failureTemplateString);

            //if (!string.IsNullOrEmpty(node.InnerXml))
            //{
            //    ArrayList stlElementArrayList = StlParserUtility.GetStlElementArrayList(node.InnerXml);
            //    if (stlElementArrayList.Count > 0)
            //    {
            //        foreach (string theStlElement in stlElementArrayList)
            //        {
            //            if (StlParserUtility.IsSpecifiedStlElement(theStlElement, If.SuccessTemplate.ElementName))
            //            {
            //                successTemplateString = StlParserUtility.GetInnerXml(theStlElement, true);
            //            }
            //            else if (StlParserUtility.IsSpecifiedStlElement(theStlElement, If.FailureTemplate.ElementName))
            //            {
            //                failureTemplateString = StlParserUtility.GetInnerXml(theStlElement, true);
            //            }
            //        }
            //    }
            //    if (string.IsNullOrEmpty(successTemplateString) && string.IsNullOrEmpty(failureTemplateString))
            //    {
            //        StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
            //        StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
            //        successTemplateString = innerBuilder.ToString();
            //    }
            //}

            bool isSuccess = false;
            ETestType testType = ETestTypeUtils.GetEnumType(testTypeStr);

            if (testType == ETestType.Undefined)
            {
                string theValue = GetAttributeValueByContext(pageInfo, contextInfo, testTypeStr);

                if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_NotEmpty))
                {
                    if (!string.IsNullOrEmpty(theValue))
                    {
                        isSuccess = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_Empty))
                {
                    if (string.IsNullOrEmpty(theValue))
                    {
                        isSuccess = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_Equals))
                {
                    if (StringUtils.EqualsIgnoreCase(theValue, testValue))
                    {
                        isSuccess = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_NotEquals))
                {
                    if (!StringUtils.EqualsIgnoreCase(theValue, testValue))
                    {
                        isSuccess = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_GreatThan))
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
                else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_LessThan))
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
                else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_In))
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
                else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_NotIn))
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
            else if (testType == ETestType.AddDate)
            {
                if (!string.IsNullOrEmpty(testValue))
                {
                    DateTime theAddDate = GetAddDateByContext(pageInfo, contextInfo);

                    if (StringUtils.EqualsIgnoreCase(testValue, "now"))
                    {
                        if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_Equals) || StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_In))
                        {
                            isSuccess = (theAddDate.Date == DateTime.Now.Date);
                        }
                        else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_GreatThan))
                        {
                            isSuccess = (theAddDate > DateTime.Now);
                        }
                        else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_LessThan))
                        {
                            isSuccess = (theAddDate < DateTime.Now);
                        }
                        else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_NotEquals) || StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_NotIn))
                        {
                            isSuccess = (theAddDate.Date != DateTime.Now.Date);
                        }
                    }
                    else
                    {
                        if (DateUtils.IsSince(testValue))
                        {
                            int hours = DateUtils.GetSinceHours(testValue);
                            if (theAddDate.AddHours(hours) >= DateTime.Now)
                            {
                                isSuccess = true;
                            }
                        }
                        else
                        {
                            DateTime dateTime = TranslateUtils.ToDateTime(testValue);
                            isSuccess = (dateTime.Date == theAddDate.Date);
                        }
                    }
                }
            }
            else if (testType == ETestType.ItemIndex)
            {
                int itemIndex = ParserUtility.GetItemIndex(contextInfo);
                isSuccess = IsNumberOperateSuccess(testOperate, itemIndex, testValue);
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
                ParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);

                parsedContent = innerBuilder.ToString();
            }

            return parsedContent;
        }

        private static bool IsNumberOperateSuccess(string testOperate, int number, string testValue)
        {
            bool isSuccess = false;
            if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_Equals))
            {
                if (number == TranslateUtils.ToInt(testValue))
                {
                    isSuccess = true;
                }
            }
            else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_NotEquals))
            {
                if (number != TranslateUtils.ToInt(testValue))
                {
                    isSuccess = true;
                }
            }
            else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_GreatThan))
            {
                if (number > TranslateUtils.ToInt(testValue))
                {
                    isSuccess = true;
                }
            }
            else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_LessThan))
            {
                if (number < TranslateUtils.ToInt(testValue))
                {
                    isSuccess = true;
                }
            }
            else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_In))
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
            else if (StringUtils.EqualsIgnoreCase(testOperate, If.TestOperate_In))
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

        private static string GetAttributeValueByContext(PageInfo pageInfo, ContextInfo contextInfo, string testTypeStr)
        {
            string theValue = null;
            if (contextInfo.ContextType == EContextType.Forum)
            {
                theValue = GetForumValue(pageInfo, contextInfo, testTypeStr, theValue);
            }
            else
            {
                if (contextInfo.ItemContainer != null)
                {
                    if (contextInfo.ItemContainer.ForumItem != null)
                    {
                        theValue = DataBinder.Eval(contextInfo.ItemContainer.ForumItem.DataItem, testTypeStr, "{0}");
                    }
                    else if (contextInfo.ItemContainer.ThreadItem != null)
                    {
                        theValue = DataBinder.Eval(contextInfo.ItemContainer.ThreadItem.DataItem, testTypeStr, "{0}");
                    }
                    else if (contextInfo.ItemContainer.PostItem != null)
                    {
                        theValue = DataBinder.Eval(contextInfo.ItemContainer.PostItem.DataItem, testTypeStr, "{0}");
                    }
                    else if (contextInfo.ItemContainer.SqlItem != null)
                    {
                        theValue = DataBinder.Eval(contextInfo.ItemContainer.SqlItem.DataItem, testTypeStr, "{0}");
                    }
                }
                else if (contextInfo.ForumID != 0)//获取栏目
                {
                    theValue = GetForumValue(pageInfo, contextInfo, testTypeStr, theValue);
                }
            }

            if (theValue == null)
            {
                theValue = string.Empty;
            }
            return theValue;
        }

        private static string GetForumValue(PageInfo pageInfo, ContextInfo contextInfo, string testTypeStr, string theValue)
        {
            ForumInfo forumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, contextInfo.ForumID);

            if (StringUtils.EqualsIgnoreCase(ForumAttribute.AddDate, testTypeStr))
            {
                theValue = forumInfo.AddDate.ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(ForumAttribute.Title, testTypeStr))
            {
                theValue = forumInfo.ForumName;
            }
            else if (StringUtils.EqualsIgnoreCase(ForumAttribute.IconUrl, testTypeStr))
            {
                theValue = forumInfo.IconUrl;
            }
            else if (StringUtils.EqualsIgnoreCase(ForumAttribute.Content, testTypeStr))
            {
                theValue = forumInfo.Content;
            }
            else if (StringUtils.EqualsIgnoreCase(ForumAttribute.Columns, testTypeStr))
            {
                theValue = DataUtility.GetForumColumns(pageInfo.PublishmentSystemID, forumInfo.Columns, contextInfo.ForumID).ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(ForumAttribute.ChildrenCount, testTypeStr))
            {
                theValue = forumInfo.ChildrenCount.ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(ForumAttribute.ThreadCount, testTypeStr))
            {
                theValue = forumInfo.ThreadCount.ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(ForumAttribute.PostCount, testTypeStr))
            {
                theValue = forumInfo.PostCount.ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(ForumAttribute.LinkUrl, testTypeStr))
            {
                theValue = forumInfo.LinkUrl;
            }
            else if (StringUtils.EqualsIgnoreCase(ForumAttribute.ParentsCount, testTypeStr))
            {
                theValue = forumInfo.ParentsCount.ToString();
            }
            return theValue;
        }

        private static DateTime GetAddDateByContext(PageInfo pageInfo, ContextInfo contextInfo)
        {
            DateTime addDate = DateUtils.SqlMinValue;

            if (contextInfo.ContextType == EContextType.Forum)
            {
                ForumInfo forumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, contextInfo.ForumID);

                addDate = forumInfo.AddDate;
            }
            else
            {
                if (contextInfo.ItemContainer != null)
                {
                    if (contextInfo.ItemContainer.ForumItem != null)
                    {
                        addDate = (DateTime)DataBinder.Eval(contextInfo.ItemContainer.ForumItem.DataItem, ForumAttribute.AddDate);
                    }
                    else if (contextInfo.ItemContainer.ThreadItem != null)
                    {
                        addDate = (DateTime)DataBinder.Eval(contextInfo.ItemContainer.ThreadItem.DataItem, ContentAttribute.AddDate);
                    }
                    else if (contextInfo.ItemContainer.PostItem != null)
                    {
                        addDate = (DateTime)DataBinder.Eval(contextInfo.ItemContainer.PostItem.DataItem, "AddDate");
                    }
                }
                else if (contextInfo.ForumID != 0)//获取栏目
                {
                    ForumInfo forumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, contextInfo.ForumID);
                    addDate = forumInfo.AddDate;
                }
            }

            return addDate;
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
    }
}
