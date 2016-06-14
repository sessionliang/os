using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Services;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlVisible
    {
        private StlVisible() { }
        public const string ElementName = "stl:visible";                    //区域是否可视

        public const string Attribute_Test = "test";                        //判断条件
        public const string Attribute_UserGroup = "usergroup";			    //用户组
        public const string Attribute_UserRank = "userrank";			    //权限级别
        public const string Attribute_UserCredits = "usercredits";			//用户积分
        public const string Attribute_RedirectUrl = "redirecturl";			//链接地址

        public const string Test_IsUser = "IsUser";			                    //仅用户可见
        public const string Test_IsAdmin = "IsAdmin";			                //仅管理员可见
        public const string Test_IsUserOrAdmin = "IsUserOrAdmin";			    //仅用户与管理员可见

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Test, "判断条件");
                attributes.Add(Attribute_UserGroup, "用户组");
                attributes.Add(Attribute_UserRank, "权限级别");
                attributes.Add(Attribute_UserCredits, "用户积分");
                attributes.Add(Attribute_RedirectUrl, "链接地址");
                return attributes;
            }
        }

        internal static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
        {
            string parsedContent = string.Empty;
            ContextInfo contextInfo = contextInfoRef.Clone();
            try
            {
                string test = StlVisible.Test_IsUser;
                string userGroup = string.Empty;
                int userRank = 0;
                int userCredits = 0;
                string redirectUrl = string.Empty;
                
                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlVisible.Attribute_Test))
                    {
                        test = attr.Value;
                    }
                    else if (attributeName.Equals(StlVisible.Attribute_UserGroup))
                    {
                        userGroup = attr.Value;
                    }
                    else if (attributeName.Equals(StlVisible.Attribute_UserRank))
                    {
                        userRank = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlVisible.Attribute_UserCredits))
                    {
                        userCredits = TranslateUtils.ToInt(attr.Value);
                    }
                    else if (attributeName.Equals(StlVisible.Attribute_RedirectUrl))
                    {
                        redirectUrl = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                    }
                }

                string successTemplateString = string.Empty;
                string failureTemplateString = string.Empty;
                StlParserUtility.GetInnerTemplateString(node, out successTemplateString, out failureTemplateString, pageInfo, contextInfo);

                pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);

                string ajaxDivID = StlParserUtility.GetAjaxDivID(pageInfo.UniqueID);

                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(@"<span id=""{0}""></span>", ajaxDivID);
                builder.Append(PageService.RegisterIsVisible(test, pageInfo.PublishmentSystemInfo, userGroup, userRank, userCredits, ajaxDivID, redirectUrl, successTemplateString, failureTemplateString));

                parsedContent = builder.ToString();
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }
    }
}
