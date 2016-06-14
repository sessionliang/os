using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System.Text.RegularExpressions;
using BaiRong.Model;
using System;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;
using SiteServer.STL.Parser.StlEntity;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlUser
    {
        private StlUser() { }
        public const string ElementName = "stl:user";                       //用户相关

        public const string Attribute_Type = "type";

        public const string TYPE_LOGIN = "login";
        public const string TYPE_LOGOUT = "logout";
        public const string TYPE_REGISTER = "register";
        public const string TYPE_FORGET = "forget";

        public const string TYPE_B2C = "b2c";
        public const string TYPE_B2C_FILTER = "b2cFilter";
        public const string TYPE_B2C_FILTER_PAGE = "b2cFilterPage";
        public const string TYPE_B2C_ORDER = "b2cOrder";
        public const string TYPE_B2C_ORDER_SUCCESS = "b2cOrderSuccess";
        public const string TYPE_B2C_ORDER_RETURN = "b2cOrderReturn";
        public const string TYPE_B2C_ORDER_LIST = "b2cOrderList";
        public const string TYPE_B2C_ORDER_DETAIL = "b2cOrderDetail";

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Type, "类型");

                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                string type = string.Empty;

                IEnumerator ie = node.Attributes.GetEnumerator();

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(StlUser.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                }

                parsedContent = ParseImpl(pageInfo, node, contextInfo, type);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        public static string ParseImpl(PageInfo pageInfo, XmlNode node, ContextInfo contextInfo, string type)
        {
            string parsedContent = string.Empty;

            string innerHtml = node.InnerXml;

            if (StringUtils.EqualsIgnoreCase(type, StlUser.TYPE_LOGIN))
            {
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC);
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_LOGIN);

                innerHtml = RegexUtils.Replace("{user.login.(\\w+)}", innerHtml, "{login.$1}");

                StringBuilder builder = new StringBuilder(innerHtml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                builder.Replace("{login.submit}", "loginController.submit();return false;");
                builder.Replace("{login.redirectToForgetPassword}", "loginController.redirectToForgetPassword();return false;");
                builder.Replace("{login.redirectToRegister}", "loginController.redirectToRegister();return false;");

                string html = RegexUtils.Replace("{login.(\\w+)}", builder.ToString(), "<%=$1%>");

                parsedContent = string.Format(@"
<script type=""text/html"" class=""loginController"">
    {0}
</script>
", html);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlUser.TYPE_LOGOUT))
            {
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC);
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_LOGOUT);

                StringBuilder builder = new StringBuilder(innerHtml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                parsedContent = builder.ToString();
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlUser.TYPE_REGISTER))
            {
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC);
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_REGISTER);

                innerHtml = RegexUtils.Replace("{user.register.(\\w+)}", innerHtml, "{register.$1}");

                StringBuilder builder = new StringBuilder(innerHtml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                builder.Replace("{register.submit}", "registerController.submit();return false;");

                string html = RegexUtils.Replace("{register.(\\w+)}", builder.ToString(), "<%=$1%>");

                parsedContent = string.Format(@"
<script type=""text/html"" class=""registerController"">
    {0}
</script>
", html);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlUser.TYPE_FORGET))
            {
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC);
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_FORGET);

                innerHtml = RegexUtils.Replace("{user.forget.(\\w+)}", innerHtml, "{forget.$1}");

                StringBuilder builder = new StringBuilder(innerHtml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                builder.Replace("{forget.submit}", "forgetController.submit();return false;");

                string html = RegexUtils.Replace("{forget.(\\w+)}", builder.ToString(), "<%=$1%>");

                parsedContent = string.Format(@"
<script type=""text/html"" class=""forgetController"">
    {0}
</script>
", builder);
            }
            //            else if (StringUtils.EqualsIgnoreCase(type, StlUser.TYPE_CMS_COMMENT_INPUT))
            //            {
            //                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC);
            //                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.B_CMS_COMMENT);

            //                if (string.IsNullOrEmpty(innerHtml))
            //                {
            //                    string filePath = PathUtils.GetSiteFilesPath("services/cms/components/comment/commentInputTemplate.html");
            //                    innerHtml = FileUtils.ReadText(filePath, ECharset.utf_8);
            //                    innerHtml = StlParserUtility.HtmlToXml(innerHtml);
            //                }

            //                StringBuilder builder = new StringBuilder(innerHtml);
            //                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

            //                parsedContent = string.Format(@"
            //<script type=""text/html"" class=""commentController"">
            //    {0}
            //</script>
            //", builder);
            //            }
            //            else if (StringUtils.EqualsIgnoreCase(type, StlUser.TYPE_CMS_COMMENTS))
            //            {
            //                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC);
            //                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.B_CMS_COMMENT);

            //                if (string.IsNullOrEmpty(innerHtml))
            //                {
            //                    string filePath = PathUtils.GetSiteFilesPath("services/cms/components/comment/commentsTemplate.html");
            //                    innerHtml = FileUtils.ReadText(filePath, ECharset.utf_8);
            //                    innerHtml = StlParserUtility.HtmlToXml(innerHtml);
            //                }

            //                StringBuilder builder = new StringBuilder(innerHtml);
            //                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

            //                parsedContent = string.Format(@"
            //<script type=""text/html"" class=""commentController"">
            //    {0}
            //</script>
            //", builder);
            //            }
            else if (StringUtils.EqualsIgnoreCase(type, StlUser.TYPE_B2C))
            {
                StringBuilder builder = new StringBuilder(innerHtml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                parsedContent = string.Format(@"
<script type=""text/html"" class=""b2cController"">
    {0}
</script>
", builder);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlUser.TYPE_B2C_FILTER))
            {
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);

                StringBuilder builder = new StringBuilder(innerHtml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                parsedContent = string.Format(@"
<script type=""text/html"" class=""filterController"">
    {0}
</script>
", builder);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlUser.TYPE_B2C_FILTER_PAGE))
            {
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                StringBuilder builder = new StringBuilder(node.InnerXml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                return string.Format(@"
<script type=""text/html"" class=""filterPageController"">
    {0}
</script>
", builder);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlUser.TYPE_B2C_ORDER))
            {
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                if (string.IsNullOrEmpty(innerHtml))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER_CSS);

                    string filePath = PathUtils.GetSiteFilesPath("services/b2c/components/order/template.html");
                    innerHtml = FileUtils.ReadText(filePath, ECharset.utf_8);
                    innerHtml = StlParserUtility.HtmlToXml(innerHtml);
                }

                StringBuilder builder = new StringBuilder(innerHtml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                parsedContent = string.Format(@"
<script type=""text/html"" class=""orderController"">
    {0}
</script>
", builder);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlUser.TYPE_B2C_ORDER_SUCCESS))
            {
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER_SUCCESS);

                StringBuilder builder = new StringBuilder(innerHtml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                parsedContent = string.Format(@"
<script type=""text/html"" class=""orderSuccessController"">
    {0}
</script>
", builder);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlUser.TYPE_B2C_ORDER_RETURN))
            {
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER_RETURN);

                StringBuilder builder = new StringBuilder(innerHtml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                parsedContent = string.Format(@"
<script type=""text/html"" class=""orderReturnController"">
    {0}
</script>
", builder);
            }
            else if (StringUtils.EqualsIgnoreCase(type, StlUser.TYPE_B2C_ORDER_LIST))
            {
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER_LIST);

                StringBuilder builder = new StringBuilder(innerHtml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                parsedContent = string.Format(@"
<script type=""text/html"" class=""orderListController"">
    {0}
</script>
", builder);
            }
            else
            {
                StringBuilder builder = new StringBuilder(innerHtml);
                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                parsedContent = builder.ToString();
            }

            return parsedContent;
        }
    }
}
