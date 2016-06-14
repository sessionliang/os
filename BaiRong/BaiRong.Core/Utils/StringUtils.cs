using System;
using System.Text;
using System.Web;
using BaiRong.Model;
using System.Text.RegularExpressions;
using BaiRong.Core.Data.Provider;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace BaiRong.Core
{
    public class StringUtils
    {
        public sealed class Constants
        {
            public const string ReturnAndNewline = "\r\n";//回车换行
            public const string Html5Empty = @"<html><head><meta charset=""utf-8""></head><body></body></html>";

            public const string Ellipsis = "...";

            //分钟基数
            public static readonly int MinuteFactorWeek = 5040;
            public static readonly int MinuteFactorDay = 720;
            public static readonly int MinuteFactorHour = 60;

            public const int PageSize = 25;//后台分页数
            public const string HIDE_ELEMENT_STYLE = "display:none";
            public const string SHOW_ELEMENT_STYLE = "display:";

            public const string CompanyName = "北京百容千域软件技术开发有限责任公司";
            public const string CompanyUrl = "http://www.siteserver.cn";
            public const string ProductName = "SITESERVER";
            public const string ProductUrl = "http://www.siteserver.cn";

            public const string Url_Validate = "http://brs.siteserver.cn/yun/validate.aspx";
            public const string Url_HelpSetup = "http://help.siteserver.cn/setup/2/9.html";
            public const string Url_Hotfix = "http://brs.siteserver.cn/yun/hotfix.aspx";
            public const string Url_Request = "http://brs.siteserver.cn/yun/request.html";
            public const string Url_Moban = "http://moban.siteserver.cn/templates.xml";
            public const string Hotfix_FileName = "hotfix.zip";
            public const string Hotfix_DirectoryName = "hotfix";

            public static string GetStlUrl(bool isEntity, string label)
            {
                if (isEntity)
                {
                    return string.Format("http://stl.siteserver.cn/e-{0}", label);
                }
                else
                {
                    return string.Format("http://stl.siteserver.cn/{0}", label.Substring("stl:".Length));
                }
            }

            public static string GetRequestUrl()
            {
                return string.Format("{0}?domain={1}&orderID={2}", Url_Request, PageUtils.GetHost(), ConfigManager.Additional.SiteYun_OrderID);
            }

            public static string GetValidateUrl(bool isAjax, bool isSiteCreated)
            {
                if (isAjax)
                {
                    return string.Format("{0}?domain={1}&isSiteCreated={2}&callback=?", Url_Validate, PageUtils.GetHost(), isSiteCreated);
                }
                else
                {
                    return string.Format("{0}?domain={1}&isSiteCreated={2}", Url_Validate, PageUtils.GetHost(), isSiteCreated);
                }
            }

            public static string GetOpenWindowStringWithHotfix(string parameters)
            {
                string url = string.Format("modal_hotfixConfirm.aspx?{0}", parameters);
                return PageUtilityPF.GetOpenWindowString("产品在线升级", url, null, 450, 300, true);
            }

            public static string GetHotfixUrl(string hotfixID)
            {
                string hotfixUrl = string.Empty;
                if (!string.IsNullOrEmpty(hotfixID))
                {
                    hotfixUrl = string.Format("{0}?hotfixID={1}&version={2}&isBeta={3}&domain={4}&r={5}", StringUtils.Constants.Url_Hotfix, hotfixID, ProductManager.Version, ProductManager.IsBeta, PageUtils.GetHost(), StringUtils.GetRandomInt(1, 100000));
                }
                else
                {
                    //string guid = BaiRongDataProvider.ConfigDAO.GetGUID(appID);
                    //int site = BaiRongDataProvider.ConfigDAO.GetSiteCount(appID);
                    //string domain = PageUtils.GetHost();

                    //hotfixUrl = string.Format("{0}?productID={1}&guid={2}&site={3}&version={4}&hotfix={5}&databaseType={6}&dotnet={7}&domain={8}&r={9}&callback=?", StringUtils.Constants.Url_Hotfix, appID, guid, site, ProductManager.Version, ProductManager.Hotfix, EDatabaseTypeUtils.GetValue(BaiRongDataProvider.DatabaseType), StringUtils.GetDotNetVersion(), domain, StringUtils.GetRandomInt(1, 100000));
                }
                return hotfixUrl;
            }

            public static string GetHotfixHTML(bool isHotfixPage)
            {
                string html = string.Empty;

                LicenseManager licenseManager = LicenseManager.Instance;

                if (licenseManager != null)
                {
                    if (isHotfixPage || FileConfigManager.Instance.IsCollection)
                    {
                        html += @"
  <div id=""hotfix-tips1"" class=""alert alert-block alert-info fade in"" style=""display:none"">
    <button type=""button"" class=""close"" data-dismiss=""alert"">&times;</button>
    <h4 class=""alert-heading"">新版本升级提示! </h4>
    <p style=""margin:10px 0"">系统检测到新版本，版本号：SITESERVER <code id=""hotfix-version""></code>，发布日期：<code id=""hotfix-date""></code>。<a id=""hotfix-url"" href=""javascript:;"" target=""_blank"">新版本介绍</a></p>
    <p id=""hotfix-message1"" style=""margin:10px 0""></p>
    <p>
    <p>
      <a id=""hotfix-open"" class=""btn btn-success"" href=""javascript:;"">在线升级</a> <a id=""hotfix-close"" class=""btn"" href=""javascript:;"">暂不升级</a>
    </p>
  </div>

  <div id=""hotfix-tips2"" class=""alert alert-block alert-info fade in"" style=""display:none"">
    <button type=""button"" class=""close"" data-dismiss=""alert"">&times;</button>
    <h4 id=""hotfix-title2"" class=""alert-heading"">新版本升级提示! </h4>
    <p id=""hotfix-message2"" style=""margin:10px 0""></p>
    <p>
    </p>
  </div>

  <script type=""text/javascript"">
  $(document).ready(function() {
    $('#hotfix-close').click(function(){
        $('#hotfix-tips1').alert('close');
        _setCookie('hotfix-tips-close', 'true', 24);
    });
    $.getJSON('[hotfixUrl]', function(data) {
        if(data.isForbidden && data.isForbidden == 'true'){
            showTips('产品许可已过期，系统将自动退出，请与管理员联系更换许可证。', 'error');
            window.setTimeout(""top.location.href='../logout.aspx'"", 5000);
            return;
        }
        if(data.isExpire && data.isExpire == 'true'){
            $('#hotfix-title2').html('产品许可到期提示! ');
            $('#hotfix-message2').html('产品将在 ' + data.expireDate + '到期，请及时更换许可证');
            $('#hotfix-tips2').fadeIn(500);
            return;
        }
      });
    });
  </script>";
                        string hotfixUrl = StringUtils.Constants.GetHotfixUrl(string.Empty);
                        string openWindow = GetOpenWindowStringWithHotfix("hotfixID=' + data.hotfixID");
                        openWindow = openWindow.Replace("data.hotfixID'", "data.hotfixID");

                        html = html.Replace("[isHotfixPage]", isHotfixPage.ToString().ToLower());
                        html = html.Replace("[openWindow]", openWindow);
                        html = html.Replace("[hotfixUrl]", hotfixUrl);
                    }
                }

                return html;
            }

            public static string GetUpdateSiteYunStateScript()
            {
                string html = string.Empty;

                html = @"
  <script type=""text/javascript"">
  $(document).ready(function() {
    $.getJSON('[validateUrl]', function(data) {
        if (data.success == 'true'){
            $.post('[updateSiteYunStateUrl]', { orderID: data.orderID, redirectUrl: data.redirectUrl });
        }
      });
    });
  </script>";
                bool isSiteCreated = BaiRongDataProvider.ConfigDAO.GetSiteCount() > 0;
                string validateUrl = StringUtils.Constants.GetValidateUrl(true, isSiteCreated);
                string updateSiteYunStateUrl = PageUtils.GetPlatformSystemServiceUrl("UpdateSiteYunState");

                html = html.Replace("[validateUrl]", validateUrl);
                html = html.Replace("[updateSiteYunStateUrl]", updateSiteYunStateUrl);

                return html;
            }
        }

        public static bool IsMobile(string val)
        {
            return Regex.IsMatch(val, @"^1[358]\d{9}$", RegexOptions.IgnoreCase);
        }

        public static bool IsEmail(string val)
        {
            return Regex.IsMatch(val, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.IgnoreCase);
        }

        public static bool IsIPAddress(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static bool IsNumber(string val)
        {
            string formatNumber = "^[0-9]+$";
            return Regex.IsMatch(val, formatNumber);
        }

        public static bool IsDateTime(string val)
        {
            string formatDate = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
            string formatDateTime = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";

            return Regex.IsMatch(val, formatDate) || Regex.IsMatch(val, formatDateTime);
        }

        public static bool In(string text, string inText)
        {
            if (text == null) return false;
            if (text == inText || text.StartsWith(inText + ",") || text.EndsWith("," + inText) || text.IndexOf("," + inText + ",") != -1)
            {
                return true;
            }
            return false;
        }

        public static bool Contains(string text, string inner)
        {
            if (text == null) return false;
            return (text.IndexOf(inner) >= 0);
        }

        public static bool ContainsIgnoreCase(string text, string inner)
        {
            if (text == null || inner == null) return false;
            return (text.ToLower().IndexOf(inner.ToLower()) >= 0);
        }

        public static string Trim(string text)
        {
            if (text == null) return string.Empty;
            return text.Trim();
        }

        public static string TrimAndToLower(string text)
        {
            if (text == null) return string.Empty;
            return text.ToLower().Trim();
        }

        public static string Remove(string text, int startIndex)
        {
            if (text == null) return string.Empty;
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }
            if (startIndex >= text.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }
            return text.Substring(0, startIndex);
        }

        public static string RemoveAttribute(string content, string attributeName)
        {
            string regex = string.Format(@"\s{0}=\""[^\""]*\""", attributeName);
            return RegexUtils.Replace(regex, content, string.Empty);
        }

        public static string RemoveNewline(string inputString)
        {
            StringBuilder retVal = new StringBuilder();
            if (!string.IsNullOrEmpty(inputString))
            {
                inputString = inputString.Trim();
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '\n':
                            break;
                        case '\r':
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }
            }
            return retVal.ToString();
        }

        public static string GUID()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GetShortGUID()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        public static string GetShortGUID(bool isUppercase)
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            string retval = string.Format("{0:x}", i - DateTime.Now.Ticks);
            if (isUppercase)
            {
                retval = retval.ToUpper();
            }
            else
            {
                retval = retval.ToLower();
            }

            return retval;
        }

        /// <summary>
        /// 获取json对象值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="json">属性值</param>
        /// <returns></returns>
        public static string GetJsonValue(string attributeName, string jsonContent)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(jsonContent);
            object value;
            if (json.TryGetValue(attributeName, out value))
            {
                return Convert.ToString(value);
            }
            return string.Empty;
        }

        public static string GetBoolText(bool type)
        {
            if (type)
            {
                return "是";
            }
            return "否";
        }

        public static bool EqualsIgnoreCase(string a, string b)
        {
            if (a == b) return true;
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;
            return string.Equals(a.Trim().ToLower(), b.Trim().ToLower());
        }

        public static bool EqualsIgnoreOrder(List<int> idList, string idCollection)
        {
            if ((idList == null || idList.Count == 0) && string.IsNullOrEmpty(idCollection)) return true;
            if ((idList == null || idList.Count == 0) && !string.IsNullOrEmpty(idCollection)) return false;
            if ((idList != null && idList.Count > 0) && string.IsNullOrEmpty(idCollection)) return false;

            List<int> idList2 = TranslateUtils.StringCollectionToIntList(idCollection);

            if (idList.Count != idList2.Count) return false;

            idList.Sort();
            idList2.Sort();

            for (int i = 0; i < idList.Count; i++)
            {
                if (idList[i] != idList2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool StartsWithIgnoreCase(string text, string startString)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(startString)) return false;
            return text.Trim().ToLower().StartsWith(startString.Trim().ToLower()) || (text.Trim().ToLower() == startString.Trim().ToLower());
        }

        public static bool EndsWithIgnoreCase(string text, string endString)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(endString)) return false;
            return text.Trim().ToLower().EndsWith(endString.Trim().ToLower());
        }

        public static bool StartsWith(string text, string startString)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(startString)) return false;
            return text.StartsWith(startString);
        }

        public static bool EndsWith(string text, string endString)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(endString)) return false;
            return text.EndsWith(endString);
        }

        public static bool StringEndsWith(string s, char c)
        {
            int num1 = s.Length;
            if (num1 != 0)
            {
                return (s[num1 - 1] == c);
            }
            return false;
        }

        public static bool StringStartsWith(string s, char c)
        {
            if (s.Length != 0)
            {
                return (s[0] == c);
            }
            return false;
        }

        public static string InsertBefore(string[] insertBeforeArray, string content, string insertContent)
        {
            if (content != null)
            {
                foreach (string insertBefore in insertBeforeArray)
                {
                    if (content.IndexOf(insertBefore) != -1)
                    {
                        return InsertBefore(insertBefore, content, insertContent);
                    }
                }
            }
            return content;
        }

        public static string InsertBefore(string insertBefore, string content, string insertContent)
        {
            string retval = content;
            if (insertBefore != null && content != null)
            {
                int startIndex = content.IndexOf(insertBefore);
                if (startIndex != -1)
                {
                    retval = content.Substring(0, startIndex) + insertContent + insertBefore + content.Substring(startIndex + insertBefore.Length);
                }
            }
            return retval;
        }

        public static bool InsertBefore(string[] insertBeforeArray, StringBuilder contentBuilder, string insertContent)
        {
            if (contentBuilder != null)
            {
                foreach (string insertBefore in insertBeforeArray)
                {
                    if (contentBuilder.ToString().IndexOf(insertBefore) != -1)
                    {
                        InsertBefore(insertBefore, contentBuilder, insertContent);
                        return true;
                    }
                }
            }
            return false;
        }

        public static void InsertBefore(string insertBefore, StringBuilder contentBuilder, string insertContent)
        {
            if (insertBefore != null && contentBuilder != null)
            {
                int startIndex = contentBuilder.ToString().IndexOf(insertBefore);
                if (startIndex != -1)
                {
                    contentBuilder.Insert(startIndex, insertContent);
                }
            }
        }

        public static void InsertBeforeOrAppend(string[] insertBeforeArray, StringBuilder contentBuilder, string insertContent)
        {
            if (!InsertBefore(insertBeforeArray, contentBuilder, insertContent))
            {
                contentBuilder.Append(insertContent);
            }
        }

        public static void InsertAfterOrAppend(string[] insertAfterArray, StringBuilder contentBuilder, string insertContent)
        {
            if (!InsertAfter(insertAfterArray, contentBuilder, insertContent))
            {
                contentBuilder.Append(insertContent);
            }
        }

        public static bool InsertAfter(string[] insertAfterArray, StringBuilder contentBuilder, string insertContent)
        {
            if (contentBuilder != null)
            {
                foreach (string insertAfter in insertAfterArray)
                {
                    if (contentBuilder.ToString().IndexOf(insertAfter) != -1)
                    {
                        InsertAfter(insertAfter, contentBuilder, insertContent);
                        return true;
                    }
                }
            }
            return false;
        }

        public static void InsertAfter(string insertAfter, StringBuilder contentBuilder, string insertContent)
        {
            if (insertAfter != null && contentBuilder != null)
            {
                int startIndex = contentBuilder.ToString().IndexOf(insertAfter);
                if (startIndex != -1)
                {
                    if (startIndex != -1)
                    {
                        contentBuilder.Insert(startIndex + insertAfter.Length, insertContent);
                    }
                }
            }
        }

        public static string HtmlDecode(string inputString)
        {
            return HttpUtility.HtmlDecode(inputString);
        }

        public static string HtmlEncode(string inputString)
        {
            return HttpUtility.HtmlEncode(inputString);
        }

        public static string ToXmlContent(string inputString)
        {
            StringBuilder contentBuilder = new StringBuilder(inputString);
            contentBuilder.Replace("<![CDATA[", string.Empty);
            contentBuilder.Replace("]]>", string.Empty);
            contentBuilder.Insert(0, "<![CDATA[");
            contentBuilder.Append("]]>");
            return contentBuilder.ToString();
            //inputString = inputString.Replace("<![CDATA[", string.Empty);
            //inputString = inputString.Replace("]]>", string.Empty);
            //return string.Format("<![CDATA[{0}]]>", inputString);
        }

        public static string StripTags(string inputString)
        {
            string retval = RegexUtils.Replace("<script[^>]*>.*?<\\/script>", inputString, string.Empty);
            retval = RegexUtils.Replace("<[\\/]?[^>]*>|<[\\S]+", retval, string.Empty);
            return retval;
        }

        public static string StripTagsExcludeBR(string inputString)
        {
            string content = RegexUtils.Replace("<[\\/]?br[^>]*>", inputString, "[_LineBreak_]");
            content = StringUtils.StripTags(content);
            content = content.Replace("[_LineBreak_]", "<br />");
            return content;
        }

        public static string StripTags(string inputString, params string[] tagNames)
        {
            string retval = inputString;
            foreach (string tagName in tagNames)
            {
                retval = RegexUtils.Replace(string.Format("<[\\/]?{0}[^>]*>|<{0}", tagName), retval, string.Empty);
            }
            return retval;
        }

        public static string StripEntities(string inputString)
        {
            string retval = RegexUtils.Replace("&[^;]*;", inputString, string.Empty);
            return retval;
        }

        public static string CleanText(string text)
        {
            return StringUtils.StripTags(text);
        }

        public static string CleanTextArea(string text)
        {
            text = StringUtils.StripTags(text);
            text = StringUtils.ReplaceNewlineToBR(text);
            return text;
        }

        public static string ParseWordString(string wordString)
        {
            string parsedContent = RegexUtils.GetInnerContent("body", wordString);
            parsedContent = parsedContent.Replace(@"
<p class=MsoNormal><span lang=EN-US><o:p>&nbsp;</o:p></span></p>
", string.Empty);
            return StringUtils.CleanTextArea(parsedContent.Trim());
        }

        public static string ReplaceIgnoreCase(string original, string pattern, string replacement)
        {
            if (original == null) return string.Empty;
            if (replacement == null) replacement = string.Empty;
            int count, position0, position1;
            count = position0 = position1 = 0;
            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern, position0)) != -1)
            {
                for (int i = position0; i < position1; ++i) chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i) chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i) chars[count++] = original[i];
            return new string(chars, 0, count);
        }

        public static string Replace(string replace, string input, string to)
        {
            string retval = RegexUtils.Replace(replace, input, to);
            if (!string.IsNullOrEmpty(replace))
            {
                if (replace.StartsWith("/") && replace.EndsWith("/"))
                {
                    retval = RegexUtils.Replace(replace.Trim(new char[] { '/' }), input, to);
                }
                else
                {
                    retval = input.Replace(replace, to);
                }
            }
            return retval;
        }

        public static string ReplaceFirst(string replace, string input, string to)
        {
            int pos = input.IndexOf(replace);
            if (pos > 0)
            {
                //取位置前部分+替换字符串+位置（加上查找字符长度）后部分
                return input.Substring(0, pos) + to + input.Substring(pos + replace.Length);
            }
            else if (pos == 0)
            {
                return to + input.Substring(replace.Length);
            }
            return input;
        }

        public static string ReplaceSpecified(string replace, string input, string to, int specified)
        {
            if (specified <= 1)
            {
                return ReplaceFirst(replace, input, to);
            }
            int pos = 0;
            for (int i = 1; i <= specified; i++)
            {
                pos = input.IndexOf(replace, pos + 1);
            }

            if (pos > 0)
            {
                //取位置前部分+替换字符串+位置（加上查找字符长度）后部分
                return input.Substring(0, pos) + to + input.Substring(pos + replace.Length);
            }
            else if (pos == 0)
            {
                return to + input.Substring(replace.Length);
            }
            return input;
        }

        public static string ReplaceAfterIndex(string replace, string input, string to, int index)
        {
            index = input.IndexOf(replace, index + 1);
            if (index > 0)
            {
                //取位置前部分+替换字符串+位置（加上查找字符长度）后部分
                return input.Substring(0, index) + to + input.Substring(index + replace.Length);
            }
            else if (index == 0)
            {
                return to + input.Substring(replace.Length);
            }
            return input;
        }

        public static string ReplaceStartsWith(string input, string replace, string to)
        {
            string retval = input;
            if (!string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(replace) && input.StartsWith(replace))
            {
                retval = to + input.Substring(replace.Length);
            }
            return retval;
        }

        public static string ReplaceEndsWith(string input, string replace, string to)
        {
            string retval = input;
            if (!string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(replace) && input.EndsWith(replace))
            {
                retval = input.Substring(0, input.LastIndexOf(replace)) + to;
            }
            return retval;
        }

        /// <summary>
        /// 将回车换行符替换为"<br>"
        /// </summary>
        public static string ReplaceNewlineToBR(string inputString)
        {
            StringBuilder retVal = new StringBuilder();
            if (!string.IsNullOrEmpty(inputString))
            {
                inputString = inputString.Trim();
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '\n':
                            retVal.Append("<br />");
                            break;
                        case '\r':
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }
            }
            return retVal.ToString();
        }

        public static string ReplaceBRToNewline(string inputString)
        {
            return RegexUtils.Replace("<br[^>]*>", inputString, "\n");
        }

        /// <summary>
        /// 将回车换行符替换为Tab符
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string ReplaceNewlineToTab(string inputString)
        {
            StringBuilder retVal = new StringBuilder();
            if (!string.IsNullOrEmpty(inputString))
            {
                inputString = inputString.Trim();
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '\r':
                            retVal.Append("\t");
                            break;
                        case '\n':
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }
            }
            return retVal.ToString();
        }

        public static string ReplaceNewline(string inputString, string replacement)
        {
            StringBuilder retVal = new StringBuilder();
            if (!string.IsNullOrEmpty(inputString))
            {
                inputString = inputString.Trim();
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '\n':
                            retVal.Append(replacement);
                            break;
                        case '\r':
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }
            }
            return retVal.ToString();
        }

        public static string CutLengthText(string inputString, int length)
        {
            string retval = inputString;
            if (!string.IsNullOrEmpty(retval))
            {
                retval = retval.Substring(0, length);
                if (retval.Length != inputString.Length)
                {
                    retval += StringUtils.Constants.Ellipsis;
                }
            }
            return retval;
        }

        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }

        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                        startIndex = startIndex - length;
                }

                if (startIndex > str.Length)
                    return "";
            }
            else
            {
                if (length < 0)
                    return "";
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                        return "";
                }
            }

            if (str.Length - startIndex < length)
                length = str.Length - startIndex;

            return str.Substring(startIndex, length);
        }

        public static string MaxLengthText(string inputString, int maxLength)
        {
            return MaxLengthText(inputString, maxLength, StringUtils.Constants.Ellipsis);
        }


        /// <summary>
        /// 将字符串截断，被截断的字符串后面带...符号
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="maxLength">值为零是代表不限制字数</param>
        /// <returns></returns>
        public static string MaxLengthText(string inputString, int maxLength, string endString)
        {
            string retval = inputString;
            try
            {
                if (maxLength > 0)
                {
                    int totalLength = maxLength * 2;
                    int length = 0;
                    StringBuilder builder = new StringBuilder();

                    bool isOneBytesChar = false;
                    char lastChar = ' ';

                    foreach (char singleChar in inputString.ToCharArray())
                    {
                        builder.Append(singleChar);

                        if (IsTwoBytesChar(singleChar))
                        {
                            length += 2;
                            if (length >= totalLength)
                            {
                                lastChar = singleChar;
                                break;
                            }
                        }
                        else
                        {
                            length += 1;
                            if (length == totalLength)
                            {
                                isOneBytesChar = true;//已经截取到需要的字数，再多截取一位
                            }
                            else if (length > totalLength)
                            {
                                lastChar = singleChar;
                                break;
                            }
                            else
                            {
                                isOneBytesChar = !isOneBytesChar;
                            }
                        }
                    }
                    if (isOneBytesChar && length > totalLength)
                    {
                        builder.Length--;
                        string theStr = builder.ToString();
                        retval = builder.ToString();
                        if (Char.IsLetter(lastChar))
                        {
                            for (int i = theStr.Length - 1; i > 0; i--)
                            {
                                char theChar = theStr[i];
                                if (!IsTwoBytesChar(theChar) && Char.IsLetter(theChar))
                                {
                                    retval = retval.Substring(0, i - 1);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            //int index = retval.LastIndexOfAny(new char[] { ' ', '\t', '\n', '\v', '\f', '\r', '\x0085' });
                            //if (index != -1)
                            //{
                            //    retval = retval.Substring(0, index);
                            //}
                        }
                    }
                    else
                    {
                        retval = builder.ToString();
                    }
                    if (inputString != retval && endString != null)
                    {
                        retval += endString;
                    }
                }
            }
            catch { }

            return retval;
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitStringIgnoreCase(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.ToLower().IndexOf(strSplit.ToLower()) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitStringIgnoreCase(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitStringIgnoreCase(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }

        public static bool IsTwoBytesChar(char chr)
        {
            // 使用中文支持编码
            if (ECharsetUtils.GB2312.GetByteCount(new char[] { chr }) == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsContainTwoBytesChar(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                foreach (char c in str)
                {
                    if (StringUtils.IsTwoBytesChar(c))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static int GetByteCount(string content)
        {
            if (string.IsNullOrEmpty(content)) return 0;
            return System.Text.Encoding.GetEncoding("gb2312").GetByteCount(content);
        }


        /// <summary>
        /// 得到innerText在content中的数目
        /// </summary>
        /// <param name="innerText"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int GetCount(string innerText, string content)
        {
            if (innerText == null || content == null)
            {
                return 0;
            }
            int count = 0;
            for (int index = content.IndexOf(innerText); index != -1; index = content.IndexOf(innerText, index + innerText.Length))
            {
                count++;
            }
            return count;
        }

        public static int GetStartCount(char startChar, string content)
        {
            if (content == null)
            {
                return 0;
            }
            int count = 0;

            foreach (char theChar in content)
            {
                if (theChar == startChar)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        public static int GetStartCount(string startString, string content)
        {
            if (content == null)
            {
                return 0;
            }
            int count = 0;

            while (true)
            {
                if (content.StartsWith(startString))
                {
                    count++;
                    content = content.Remove(0, startString.Length);
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        public static string GetFirstOfStringCollection(string collection)
        {
            return GetFirstOfStringCollection(collection, ',');
        }

        public static string GetFirstOfStringCollection(string collection, char separator)
        {
            if (!string.IsNullOrEmpty(collection))
            {
                int index = collection.IndexOf(separator);
                if (index == -1)
                {
                    return collection;
                }
                else
                {
                    return collection.Substring(0, index);
                }
            }
            return string.Empty;
        }


        private static int randomSeq = 0;
        public static int GetRandomInt(int minValue, int maxValue)
        {
            Random ro = new Random(unchecked((int)DateTime.Now.Ticks));
            int retval = ro.Next(minValue, maxValue);
            retval += StringUtils.randomSeq++;
            if (retval >= maxValue)
            {
                StringUtils.randomSeq = 0;
                retval = minValue;
            }
            return retval;
        }

        public static string ValueToUrl(string value)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                //替换url中的换行符，update by sessionliang at 20151211
                retval = value.Replace("=", "_equals_").Replace("&", "_and_").Replace("?", "_question_").Replace("'", "_quote_").Replace("+", "_add_").Replace("\r", "").Replace("\n", "");
            }
            return retval;
        }

        public static string ValueToUrl(string value, bool replaceSlash)
        {

            string retval = string.Empty;
            if (replaceSlash)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    retval = value.Replace("=", "_equals_").Replace("&", "_and_").Replace("?", "_question_").Replace("'", "_quote_").Replace("+", "_add_").Replace("/", "_slash_");
                }
            }
            else
            {
                retval = ValueToUrl(value);
            }
            return retval;
        }

        public static string ValueFromUrl(string value)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                retval = value.Replace("_equals_", "=").Replace("_and_", "&").Replace("_question_", "?").Replace("_quote_", "'").Replace("_add_", "+");
            }
            return retval;
        }

        public static string ValueFromUrl(string value, bool replaceSlash)
        {
            string retval = string.Empty;
            if (replaceSlash)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    retval = value.Replace("_equals_", "=").Replace("_and_", "&").Replace("_question_", "?").Replace("_quote_", "'").Replace("_add_", "+").Replace("_slash_", "/");
                }
            }
            else
            {
                retval = ValueFromUrl(value);
            }
            return retval;
        }

        public static string ToJsString(string value)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                retval = value.Replace("'", @"\'").Replace("\r", "\\r").Replace("\n", "\\n");
            }
            return retval;
        }

        public static int GetDotNetVersion()
        {
            return System.Environment.Version.Major;
        }

        public static string ParseString(EInputType inputType, string content, string replace, string to, int startIndex, int length, int wordNum, string ellipsis, bool isClearTags, bool isReturnToBr, bool isLower, bool isUpper, string formatString)
        {
            if (EInputTypeUtils.IsPureString(inputType))
            {
                return ParseString(content, replace, to, startIndex, length, wordNum, ellipsis, isClearTags, isReturnToBr, isLower, isUpper, formatString);
            }
            else
            {
                return content;
            }
        }

        public static string ParseString(string content, string replace, string to, int startIndex, int length, int wordNum, string ellipsis, bool isClearTags, bool isReturnToBR, bool isLower, bool isUpper, string formatString)
        {
            string parsedContent = content;

            if (!string.IsNullOrEmpty(replace))
            {
                parsedContent = StringUtils.Replace(replace, parsedContent, to);
            }

            if (isClearTags)
            {
                parsedContent = StringUtils.StripTags(parsedContent);
            }

            if (!string.IsNullOrEmpty(parsedContent))
            {
                if (startIndex > 0 || length > 0)
                {
                    try
                    {
                        if (length > 0)
                        {
                            parsedContent = parsedContent.Substring(startIndex, length);
                        }
                        else
                        {
                            parsedContent = parsedContent.Substring(startIndex);
                        }
                    }
                    catch { }
                }

                if (wordNum > 0)
                {
                    parsedContent = StringUtils.MaxLengthText(parsedContent, wordNum, ellipsis);
                }

                if (isReturnToBR)
                {
                    parsedContent = StringUtils.ReplaceNewlineToBR(parsedContent);
                }

                if (!string.IsNullOrEmpty(formatString))
                {
                    parsedContent = string.Format(formatString, parsedContent);
                }

                if (isLower)
                {
                    parsedContent = parsedContent.ToLower();
                }
                if (isUpper)
                {
                    parsedContent = parsedContent.ToUpper();
                }
            }

            return parsedContent;
        }

        public static string IntToSignString(int i)
        {
            string retval = "0";
            if (i != 0)
            {
                retval = i > 0 ? "+" + i.ToString() : i.ToString();
            }
            return retval;
        }

        public static string GetPercentage(int num, int totalNum)
        {
            return Convert.ToDouble((double)num / (double)totalNum).ToString("0.00%");
        }

        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="Htmlstring">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Htmlstring.Replace("\r\n", "");
            Htmlstring = Regex.Replace(Htmlstring, @"<script.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<style.*?</style>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<.*?>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Htmlstring.Replace("&ldquo;", "\"");
            Htmlstring = Htmlstring.Replace("&rdquo;", "\"");
            Htmlstring = Htmlstring.Replace("<", "");
            Htmlstring = Htmlstring.Replace(">", "");
            Htmlstring = Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }

        public static string GetTrueImageHtml(string isDefaultStr)
        {
            return GetTrueImageHtml(TranslateUtils.ToBool(isDefaultStr));
        }

        public static string GetTrueImageHtml(bool isDefault)
        {
            string retval = string.Empty;
            if (isDefault)
            {
                retval = "<img src='../pic/icon/right.gif' border='0'/>";
            }
            return retval;
        }

        public static string GetFalseImageHtml(string isDefaultStr)
        {
            return GetFalseImageHtml(TranslateUtils.ToBool(isDefaultStr));
        }

        public static string GetFalseImageHtml(bool isDefault)
        {
            string retval = string.Empty;
            if (isDefault == false)
            {
                retval = "<img src='../pic/icon/wrong.gif' border='0'/>";
            }
            return retval;
        }

        public static string GetTrueOrFalseImageHtml(string isDefaultStr)
        {
            return GetTrueOrFalseImageHtml(TranslateUtils.ToBool(isDefaultStr));
        }

        public static string GetTrueOrFalseImageHtml(bool isDefault)
        {
            string retval;
            if (isDefault)
            {
                retval = "<img src='../pic/icon/right.gif' border='0'/>";
            }
            else
                retval = "<img src='../pic/icon/wrong.gif' border='0'/>";
            return retval;
        }

        public static string GetBreadCrumbHtml(string moduleID, string topMenuID, string topTitle, string leftMenuID, string leftTitle, string pageUrl, string pageTitle, string itemTitle, bool isOEM)
        {
            return GetBreadCrumbHtml(moduleID, topMenuID, topTitle, leftMenuID, leftTitle, string.Empty, string.Empty, pageUrl, pageTitle, itemTitle, isOEM);
        }

        public static string GetBreadCrumbHtml(string moduleID, string topMenuID, string topTitle, string leftMenuID, string leftTitle, string leftSubMenuID, string leftSubTitle, string pageUrl, string pageTitle, string itemTitle, bool isOEM)
        {
            isOEM = true;
            StringBuilder builder = new StringBuilder();
            builder.Append(@"<ul class=""breadcrumb"">");

            if (FileConfigManager.Instance.IsSaasQCloud)
            {
                builder.Append(@"<li><a href=""http://www.gexia.com/wxy"" target=""_blank"" style=""color:#CA1A1A"">我们将为您提供免费的一对一服务，帮您在最短的时间内配置好一个功能齐全的微信公众号，请加企业QQ：4008770550</a></li><li class=""pull-right""><script charset=""utf-8"" type=""text/javascript"" src=""http://wpa.b.qq.com/cgi/wpa.php?key=XzkzODAxMTMwMF8xMzc3MjZfNDAwODc3MDU1MF8""></script></li>");
            }
            else
            {
                string topHtml = isOEM ? topTitle : string.Format(@"<a href=""http://help.siteserver.cn/{0}/top/{1}.html"" target=""_blank"">{2}</a>", moduleID, topMenuID, topTitle);
                builder.AppendFormat(@"<li>{0} <span class=""divider"">/</span></li>", topHtml);

                string leftHtml = isOEM ? leftTitle : string.Format(@"<a href=""http://help.siteserver.cn/{0}/left/{1}.html"" target=""_blank"">{2}</a>", moduleID, leftMenuID, leftTitle);

                if (pageTitle != "")
                {
                    builder.AppendFormat(@"<li>{0} <span class=""divider"">/</span></li>", leftHtml);
                }
                else
                {
                    builder.AppendFormat(@"<li class=""active"">{0}</li>", leftHtml);
                }

                if (!string.IsNullOrEmpty(leftSubMenuID))
                {
                    string leftSubHtml = isOEM ? leftSubTitle : string.Format(@"<a href=""http://help.siteserver.cn/{0}/left/{1}.html"" target=""_blank"">{2}</a>", moduleID, leftSubMenuID, leftSubTitle);
                    builder.AppendFormat(@"<li>{0} <span class=""divider"">/</span></li>", leftSubHtml);
                }

                string pageHtml = isOEM ? pageTitle : string.Format(@"<a href=""http://help.siteserver.cn/{0}/{1}"" target=""_blank"">{2}</a>", moduleID, pageUrl.Replace(".aspx", ".html"), pageTitle);

                if (!string.IsNullOrEmpty(itemTitle))
                {

                    builder.AppendFormat(@"<li>{0} <span class=""divider"">/</span></li>", pageHtml);
                    builder.AppendFormat(@"<li class=""active"">{0}</li>", itemTitle.Replace(">", "/"));

                }
                else
                {
                    builder.AppendFormat(@"<li class=""active"">{0}</li>", pageHtml);
                }
            }

            builder.Append("</ul>");

            return builder.ToString();
        }

        public static bool IsNullorEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string ReplaceInvalidChar(string input)
        {
            List<char> invalidChars = new List<char>();
            invalidChars.AddRange(PathUtils.InvalidPathChars);
            invalidChars.Add(' ');
            invalidChars.Add('　');
            invalidChars.Add('\'');
            invalidChars.Add(':');
            invalidChars.Add('.');
            foreach (char item in input)
            {
                if (invalidChars.IndexOf(item) != -1)
                    input = input.Remove(input.IndexOf(item), 1);
            }

            return input;
        }

        //        public static string GetBreadCrumbHtml(string moduleID, string topMenuID, string topTitle, string leftMenuID, string leftTitle, string leftSubMenuID, string leftSubTitle, string pageUrl, string pageTitle, bool isOEM)
        //        {
        //            if (isOEM)
        //            {
        //                return string.Format(@"
        //  <ul class=""breadcrumb"">
        //    <li>{0} <span class=""divider"">/</span></li>
        //    <li>{1} <span class=""divider"">/</span></li>
        //    <li>{2} <span class=""divider"">/</span></li>
        //    <li class=""active"">{3}</li>
        //  </ul>
        //", topTitle, leftTitle, leftSubTitle, pageTitle);
        //            }
        //            else if (string.IsNullOrEmpty(pageUrl))
        //            {
        //                return string.Format(@"
        //  <ul class=""breadcrumb"">
        //    <li>
        //      <a href=""http://help.siteserver.cn/{0}/top/{1}.html"" target=""_blank"">{2}</a>
        //      <span class=""divider"">/</span>
        //    </li>
        //    <li>
        //      <a href=""http://help.siteserver.cn/{0}/left/{3}.html"" target=""_blank"">{4}</a>
        //      <span class=""divider"">/</span>
        //    </li>
        //    <li>
        //      <a href=""http://help.siteserver.cn/{0}/left/{5}.html"" target=""_blank"">{6}</a>
        //      <span class=""divider"">/</span>
        //    </li>
        //    <li class=""active"">
        //      {7}
        //    </li>
        //  </ul>
        //", moduleID, topMenuID, topTitle, leftMenuID, leftTitle, leftSubMenuID, leftSubTitle, pageTitle);
        //            }
        //            else
        //            {
        //                return string.Format(@"
        //  <ul class=""breadcrumb"">
        //    <li>
        //      <a href=""http://help.siteserver.cn/{0}/top/{1}.html"" target=""_blank"">{2}</a>
        //      <span class=""divider"">/</span>
        //    </li>
        //    <li>
        //      <a href=""http://help.siteserver.cn/{0}/left/{3}.html"" target=""_blank"">{4}</a>
        //      <span class=""divider"">/</span>
        //    </li>
        //    <li>
        //      <a href=""http://help.siteserver.cn/{0}/left/{5}.html"" target=""_blank"">{6}</a>
        //      <span class=""divider"">/</span>
        //    </li>
        //    <li class=""active"">
        //      <a href=""http://help.siteserver.cn/{0}/{7}"" target=""_blank"">{8}</a>
        //    </li>
        //  </ul>
        //", moduleID, topMenuID, topTitle, leftMenuID, leftTitle, leftSubMenuID, leftSubTitle, pageUrl.Replace(".aspx", ".html"), pageTitle);
        //            }
        //        }
    }
}
