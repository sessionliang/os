using System.Collections.Specialized;
using System.Web.UI;
using BaiRong.Core;
using System.Web;

namespace BaiRong.Core
{
    public class JsUtils
    {
        private JsUtils()
        {
        }

        public static void ResponseScripts(Page page, string scripts)
        {
            page.Response.Clear();
            page.Response.Write(string.Format("<script language=\"javascript\">{0}</script>", scripts));
            //page.Response.End();
        }


        public static string GetRedirectStringWithCheckBoxValue(string redirectUrl, string checkBoxServerID, string checkBoxClientID, string emptyAlertText)
        {
            return string.Format(@"if (!_alertCheckBoxCollection(document.getElementsByName('{0}'), '{1}')){{_goto('{2}' + '&{3}=' + _getCheckBoxCollectionValue(document.getElementsByName('{0}')));}};return false;", checkBoxClientID, emptyAlertText, redirectUrl, checkBoxServerID);
        }

        public static string GetRedirectStringWithCheckBoxValueAndAlert(string redirectUrl, string checkBoxServerID, string checkBoxClientID, string emptyAlertText, string alertText)
        {
            return string.Format(@"if (!_alertCheckBoxCollection(document.getElementsByName('{0}'), '{1}')){{if (confirm('{2}'))_goto('{3}' + '&{4}=' + _getCheckBoxCollectionValue(document.getElementsByName('{0}')));}};return false;", checkBoxClientID, emptyAlertText, alertText, redirectUrl, checkBoxServerID);

        }

        public static string GetRedirectStringWithConfirm(string redirectUrl, string confirmString)
        {
            return string.Format(@"if (window.confirm('{0}')){{window.location.href='{1}';}};return false;", confirmString, redirectUrl);
        }

        public static string GetRedirectString(string redirectUrl)
        {
            return string.Format(@"window.location.href='{0}';return false;", redirectUrl);
        }

        #region OpenWindow

        public class OpenWindow
        {
            private OpenWindow() { }

            public const string HIDE_POP_WIN = "window.parent.closeWindow();";

            public const string TIPS_SUCCESS = "success";
            public const string TIPS_ERROR = "error";
            public const string TIPS_INFO = "info";
            public const string TIPS_WARN = "warn";

            public static string GetOpenTipsString(string message, string tipsType)
            {
                return string.Format(@"openTips('{0}', '{1}');", message, tipsType);
            }

            /// <summary>
            /// 提示框，带有确认按钮
            /// </summary>
            /// <param name="message"></param>
            /// <param name="tipsType"></param>
            /// <returns></returns>
            public static string GetOpenTipsString(string message, string tipsType, bool isCloseOnly, string btnValue, string btnClick)
            {
                return string.Format(@"openTips('{0}', '{1}', '{2}', '{3}', '{4}');", message, tipsType, isCloseOnly.ToString().ToLower(), btnValue, btnClick);
            }

            public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments, int width, int height)
            {
                return GetOpenWindowString(title, pageUrl, arguments, width, height, false);
            }

            public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments)
            {
                return GetOpenWindowString(title, pageUrl, arguments, 0, 0, false);
            }

            public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments, bool isCloseOnly)
            {
                return GetOpenWindowString(title, pageUrl, arguments, 0, 0, isCloseOnly);
            }

            public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments, int width, int height, bool isCloseOnly)
            {
                if (height > 590) height = 0;
                if (arguments == null) arguments = new NameValueCollection();
                string url = PageUtils.AddQueryString(pageUrl, arguments);
                return string.Format(@"openWindow('{0}','{1}',{2},{3},'{4}');return false;", title, url, width, height, isCloseOnly.ToString().ToLower());
            }

            public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID)
            {
                return GetOpenWindowStringWithTextBoxValue(title, pageUrl, arguments, textBoxID, 0, 0, false);
            }

            public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID, int width, int height)
            {
                return GetOpenWindowStringWithTextBoxValue(title, pageUrl, arguments, textBoxID, width, height, false);
            }

            public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID, bool isCloseOnly)
            {
                return GetOpenWindowStringWithTextBoxValue(title, pageUrl, arguments, textBoxID, 0, 0, isCloseOnly);
            }

            public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID, int width, int height, bool isCloseOnly)
            {
                if (height > 590) height = 0;
                if (arguments == null) arguments = new NameValueCollection();
                string url = PageUtils.AddQueryString(pageUrl, arguments);
                return string.Format(@"openWindow('{0}','{1}' + '&{2}=' + $('#{2}').val(),{3}, {4}, '{5}');return false;", title, url, textBoxID, width, height, isCloseOnly.ToString().ToLower());
            }

            public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, int width, int height)
            {
                return GetOpenWindowStringWithCheckBoxValue(title, pageUrl, arguments, checkBoxID, alertText, width, height, false);
            }

            public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText)
            {
                return GetOpenWindowStringWithCheckBoxValue(title, pageUrl, arguments, checkBoxID, alertText, 0, 0, false);
            }

            public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, bool isCloseOnly)
            {
                return GetOpenWindowStringWithCheckBoxValue(title, pageUrl, arguments, checkBoxID, alertText, 0, 0, isCloseOnly);
            }

            public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, int width, int height, bool isCloseOnly)
            {
                if (height > 590) height = 0;
                if (arguments == null) arguments = new NameValueCollection();
                string url;
                if (arguments.Count > 0)
                {
                    url = PageUtils.AddQueryString(pageUrl, arguments);
                }
                else
                {
                    url = pageUrl + "?";
                }
                if (string.IsNullOrEmpty(alertText))
                {
                    return string.Format(@"openWindow('{0}', '{1}' + '&{2}=' + _getCheckBoxCollectionValue(document.getElementsByName('{2}')),{3}, {4}, '{5}');return false;", title, url, checkBoxID, width, height, isCloseOnly.ToString().ToLower());
                }
                else
                {
                    return string.Format(@"if (!_alertCheckBoxCollection(document.getElementsByName('{0}'), '{1}')){{openWindow('{2}', '{3}' + '&{0}=' + _getCheckBoxCollectionValue(document.getElementsByName('{0}')),{4}, {5}, '{6}');}};return false;", checkBoxID, alertText, title, url, width, height, isCloseOnly.ToString().ToLower());
                }
            }

            public static string GetOpenWindowStringWithTwoCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID1, string checkBoxID2, string alertText, int width, int height)
            {
                return GetOpenWindowStringWithTwoCheckBoxValue(title, pageUrl, arguments, checkBoxID1, checkBoxID2, alertText, width, height, false);
            }

            public static string GetOpenWindowStringWithTwoCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID1, string checkBoxID2, string alertText, int width, int height, bool isCloseOnly)
            {
                if (height > 590) height = 0;
                if (arguments == null) arguments = new NameValueCollection();
                string url = PageUtils.AddQueryString(pageUrl, arguments);
                return string.Format(@"var collectionValue1 = _getCheckBoxCollectionValue(document.getElementsByName('{0}'));var collectionValue2 = _getCheckBoxCollectionValue(document.getElementsByName('{1}'));if (collectionValue1.length == 0 && collectionValue2.length == 0){{alert('{2}');}}else{{openWindow('{3}', '{4}' + '&{0}=' + _getCheckBoxCollectionValue(document.getElementsByName('{0}')) + '&{1}=' + _getCheckBoxCollectionValue(document.getElementsByName('{1}')),{5}, {6}, '{7}');}};return false;", checkBoxID1, checkBoxID2, alertText, title, url, width, height, isCloseOnly.ToString().ToLower());
            }

            public static void SetCancelAttribute(IAttributeAccessor accessor)
            {
                accessor.SetAttribute("onclick", "window.parent.closeWindow();");
            }

            //todo:火狐不兼容
            public static void CloseModalPage(Page page)
            {
                page.Response.Clear();
                page.Response.Write("<script language=\"javascript\">window.parent.closeWindow();window.parent.location.reload(false);</script>");
                //page.Response.End();
            }

            public static void CloseModalPage(Page page, string scripts)
            {
                page.Response.Clear();
                page.Response.Write(string.Format("<script language=\"javascript\">{0}</script>", scripts));
                page.Response.Write("<script language=\"javascript\">window.parent.closeWindow();window.parent.location.reload(false);</script>");
                //page.Response.End();
            }

            public static void CloseModalPageAndRedirect(Page page, string redirectUrl)
            {
                page.Response.Clear();
                page.Response.Write(string.Format("<script language=\"javascript\">window.parent.closeWindow();window.parent._goto('{0}');</script>", redirectUrl));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            public static void CloseModalPageAndRedirect(Page page, string redirectUrl, string scripts)
            {
                page.Response.Clear();
                page.Response.Write(string.Format("<script language=\"javascript\">{0}</script>", scripts));
                page.Response.Write(string.Format("<script language=\"javascript\">window.parent.closeWindow();window.parent._goto('{0}');</script>", redirectUrl));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            public static void CloseModalPageWithoutRefresh(Page page)
            {
                page.Response.Clear();
                page.Response.Write("<script language=\"javascript\">window.parent.closeWindow();</script>");
                //page.Response.End();
            }

            public static void CloseModalPageWithoutRefresh(Page page, string scripts)
            {
                page.Response.Clear();
                page.Response.Write(string.Format("<script language=\"javascript\">{0}</script>", scripts));
                page.Response.Write("<script language=\"javascript\">window.parent.closeWindow();</script>");
                //page.Response.End();
            }
        }

        #endregion

        #region Layer

        public class Layer
        {
            private Layer() { }

            public const string HIDE_POP_WIN = "window.parent.layer.close();";

            public static string GetOpenLayerString(string title, string pageUrl, NameValueCollection arguments)
            {
                return GetOpenLayerString(title, pageUrl, arguments, 0, 0);
            }

            public static string GetOpenLayerString(string title, string pageUrl, NameValueCollection arguments, int width, int height)
            {
                string areaWidth = string.Format("'{0}px'", width);
                string areaHeight = string.Format("'{0}px'", height);
                string offsetLeft = "''";
                string offsetRight = "''";
                if (width == 0)
                {
                    areaWidth = "($(window).width() - 10) +'px'";
                    offsetRight = "'0px'";
                }
                if (height == 0)
                {
                    areaHeight = "($(window).height() - 10) +'px'";
                    offsetLeft = "'0px'";
                }
                if (arguments == null) arguments = new NameValueCollection();
                string url = PageUtils.AddQueryString(pageUrl, arguments);
                return string.Format(@"$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{0}', shade: [0.1,'#fff'], iframe: {{src: '{1}'}}, area: [{2}, {3}], offset: [{4}, {5}]}});return false;", title, url, areaWidth, areaHeight, offsetLeft, offsetRight);
            }

            public static string GetOpenLayerStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID)
            {
                return GetOpenLayerStringWithTextBoxValue(title, pageUrl, arguments, textBoxID, 0, 0);
            }

            public static string GetOpenLayerStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID, int width, int height)
            {
                string areaWidth = string.Format("'{0}px'", width);
                string areaHeight = string.Format("'{0}px'", height);
                string offset = string.Empty;
                if (width == 0)
                {
                    areaWidth = "($(window).width() - 10) +'px'";
                    offset = "offset: ['0px','0px'],";
                }
                if (height == 0)
                {
                    areaHeight = "($(window).height() - 10) +'px'";
                    offset = "offset: ['0px','0px'],";
                }
                if (arguments == null) arguments = new NameValueCollection();
                string url = PageUtils.AddQueryString(pageUrl, arguments);
                return string.Format(@"$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{0}', shade: [0.1,'#fff'], iframe: {{src: '{1}' + '&{2}=' + $('#{2}').val()}}, area: [{3}, {4}], {5}}});return false;", title, url, textBoxID, areaWidth, areaHeight, offset);
            }

            public static string GetOpenLayerStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText)
            {
                return GetOpenLayerStringWithCheckBoxValue(title, pageUrl, arguments, checkBoxID, alertText, 0, 0);
            }

            public static string GetOpenLayerStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, int width, int height)
            {
                string areaWidth = string.Format("'{0}px'", width);
                string areaHeight = string.Format("'{0}px'", height);
                string offset = string.Empty;
                if (width == 0)
                {
                    areaWidth = "($(window).width() - 10) +'px'";
                    offset = "offset: ['0px','0px'],";
                }
                if (height == 0)
                {
                    areaHeight = "($(window).height() - 10) +'px'";
                    offset = "offset: ['0px','0px'],";
                }
                if (arguments == null) arguments = new NameValueCollection();
                string url;
                if (arguments.Count > 0)
                {
                    url = PageUtils.AddQueryString(pageUrl, arguments);
                }
                else
                {
                    url = pageUrl + "?";
                }

                if (string.IsNullOrEmpty(alertText))
                {
                    return string.Format(@"$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{0}', shade: [0.1,'#fff'], iframe: {{src: '{1}' + '&{2}=' + _getCheckBoxCollectionValue(document.getElementsByName('{2}'))}}, area: [{3}, {4}], {5}}});return false;", title, url, checkBoxID, areaWidth, areaHeight, offset);
                }
                else
                {
                    return string.Format(@"if (!_alertCheckBoxCollection(document.getElementsByName('{0}'), '{1}')){{$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{2}', shade: [0.1,'#fff'], iframe: {{src: '{3}' + '&{0}=' + _getCheckBoxCollectionValue(document.getElementsByName('{0}'))}}, area: [{4}, {5}], {6}}});}};return false;", checkBoxID, alertText, title, url, areaWidth, areaHeight, offset);
                }
            }

            public static string GetOpenLayerStringWithTwoCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID1, string checkBoxID2, string alertText, int width, int height)
            {
                string areaWidth = string.Format("'{0}px'", width);
                string areaHeight = string.Format("'{0}px'", height);
                string offset = string.Empty;
                if (width == 0)
                {
                    areaWidth = "($(window).width() - 10) +'px'";
                    offset = "offset: ['0px','0px'],";
                }
                if (height == 0)
                {
                    areaHeight = "($(window).height() - 10) +'px'";
                    offset = "offset: ['0px','0px'],";
                }

                if (arguments == null) arguments = new NameValueCollection();
                string url = PageUtils.AddQueryString(pageUrl, arguments);
                return string.Format(@"var collectionValue1 = _getCheckBoxCollectionValue(document.getElementsByName('{0}'));var collectionValue2 = _getCheckBoxCollectionValue(document.getElementsByName('{1}'));if (collectionValue1.length == 0 && collectionValue2.length == 0){{alert('{2}');}}else{{$.layer({{type: 2, maxmin: true, shadeClose: true, title: '{3}', shade: [0.1,'#fff'], iframe: {{src: '{4}' + '&{0}=' + _getCheckBoxCollectionValue(document.getElementsByName('{0}')) + '&{1}=' + _getCheckBoxCollectionValue(document.getElementsByName('{1}'))}}, area: [{5}, {6}], {7}}});}};return false;", checkBoxID1, checkBoxID2, alertText, title, url, width, height, offset);
            }

            public static void SetCancelAttribute(IAttributeAccessor accessor)
            {
                accessor.SetAttribute("onclick", "window.parent.layer.close();");
            }

            //todo:火狐不兼容
            public static void CloseModalLayer(Page page)
            {
                page.Response.Clear();
                page.Response.Write("<script language=\"javascript\">window.parent.layer.close();window.parent.location.reload();window.parent.location.reload();</script>");
                //page.Response.End();
            }

            public static void CloseModalLayer(Page page, string scripts)
            {
                page.Response.Clear();
                page.Response.Write(string.Format("<script language=\"javascript\">{0}</script>", scripts));
                page.Response.Write("<script language=\"javascript\">window.parent.layer.close();window.parent.location.reload();window.parent.location.reload();</script>");
                //page.Response.End();
            }

            public static void CloseModalLayerAndRedirect(Page page, string redirectUrl)
            {
                page.Response.Clear();
                page.Response.Write(string.Format("<script language=\"javascript\">window.parent.layer.close();window.parent.location.href = '{0}';</script>", redirectUrl));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            public static void CloseModalLayerAndRedirect(Page page, string redirectUrl, string scripts)
            {
                page.Response.Clear();
                page.Response.Write(string.Format("<script language=\"javascript\">{0}</script>", scripts));
                page.Response.Write(string.Format("<script language=\"javascript\">window.parent.layer.close();window.parent.location.href = '{0}';</script>", redirectUrl));
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            public static void CloseModalLayerWithoutRefresh(Page page)
            {
                page.Response.Clear();
                page.Response.Write("<script language=\"javascript\">window.parent.layer.close();</script>");
                //page.Response.End();
            }

            public static void CloseModalLayerWithoutRefresh(Page page, string scripts)
            {
                page.Response.Clear();
                page.Response.Write(string.Format("<script language=\"javascript\">{0}</script>", scripts));
                page.Response.Write("<script language=\"javascript\">window.parent.layer.close();</script>");
                //page.Response.End();
            }
        }

        #endregion
    }
}
