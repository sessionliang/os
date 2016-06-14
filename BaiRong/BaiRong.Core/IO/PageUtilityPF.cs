using System;
using System.IO;
using System.Collections;
using System.Web;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;

namespace BaiRong.Core
{
    public class PageUtilityPF
    {
        private PageUtilityPF()
        {
        }

        public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments, int width, int height)
        {
            return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetPlatformUrl(pageUrl), arguments, width, height);
        }

        public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments)
        {
            return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetPlatformUrl(pageUrl), arguments);
        }

        public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetPlatformUrl(pageUrl), arguments, isCloseOnly);
        }

        public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments, int width, int height, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetPlatformUrl(pageUrl), arguments, width, height, isCloseOnly);
        }

        public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTextBoxValue(title, PageUtils.GetPlatformUrl(pageUrl), arguments, textBoxID);
        }

        public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID, int width, int height)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTextBoxValue(title, PageUtils.GetPlatformUrl(pageUrl), arguments, textBoxID, width, height);
        }

        public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTextBoxValue(title, PageUtils.GetPlatformUrl(pageUrl), arguments, textBoxID, isCloseOnly);
        }

        public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID, int width, int height, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTextBoxValue(title, PageUtils.GetPlatformUrl(pageUrl), arguments, textBoxID, width, height, isCloseOnly);
        }

        public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, int width, int height)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue(title, PageUtils.GetPlatformUrl(pageUrl), arguments, checkBoxID, alertText, width, height);
        }

        public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue(title, PageUtils.GetPlatformUrl(pageUrl), arguments, checkBoxID, alertText);
        }

        public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue(title, PageUtils.GetPlatformUrl(pageUrl), arguments, checkBoxID, alertText, isCloseOnly);
        }

        public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, int width, int height, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue(title, PageUtils.GetPlatformUrl(pageUrl), arguments, checkBoxID, alertText, width, height, isCloseOnly);
        }

        public static string GetOpenWindowStringWithTwoCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID1, string checkBoxID2, string alertText, int width, int height)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTwoCheckBoxValue(title, PageUtils.GetPlatformUrl(pageUrl), arguments, checkBoxID1, checkBoxID2, alertText, width, height);
        }

        public static string GetOpenWindowStringWithTwoCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID1, string checkBoxID2, string alertText, int width, int height, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTwoCheckBoxValue(title, PageUtils.GetPlatformUrl(pageUrl), arguments, checkBoxID1, checkBoxID2, alertText, width, height, isCloseOnly);
        }

        public static string GetPermissionsSetOpenWindowString(string userName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("UserName", userName);
            return JsUtils.OpenWindow.GetOpenWindowString("»®œﬁ…Ë÷√", PageUtils.GetCMSUrl("modal_permissionsSet.aspx"), arguments);
        }
    }
}
