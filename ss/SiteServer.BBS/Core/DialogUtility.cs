using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core;
using SiteServer.BBS.Model;

using System.Collections.Specialized;
using System.Web.UI;

namespace SiteServer.BBS.Core
{
    public class DialogUtility
    {
        private DialogUtility() { }

        public static string GetOpenWindowString(string pageUrl, NameValueCollection arguments, int width, int height, string title, string tips)
        {
            if (arguments == null) arguments = new NameValueCollection();
            string url = PageUtils.AddQueryString(pageUrl, arguments);
            return string.Format(@"showDialog('{0}', {1}, {2}, '{3}', '{4}');return false;", url, height, width, title, tips);
        }

        public static string GetOpenWindowStringWithTextBoxValue(string pageUrl, NameValueCollection arguments, string textBoxID, int width, int height, string title, string tips)
        {
            if (arguments == null) arguments = new NameValueCollection();
            string url = PageUtils.AddQueryString(pageUrl, arguments);
            return string.Format(@"showDialog('{0}' + '&{1}=' + $('#{1}').val(),{2}, {3}, '{4}', '{5}');return false;", url, textBoxID, height, width, title, tips);
        }

        public static string GetOpenWindowStringWithCheckBoxValue(string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, int width, int height, string title, string tips)
        {
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
                return string.Format(@"showDialog('{0}' + '&{1}=' + getCheckBoxCollectionValue(document.getElementsByName('{1}')),{2}, {3}, '{4}', '{5}');return false;", url, checkBoxID, height, width, title, tips);
            }
            else
            {
                return string.Format(@"if (!alertCheckBoxCollection(document.getElementsByName('{0}'), '{1}')){{showDialog('{2}' + '&{0}=' + getCheckBoxCollectionValue(document.getElementsByName('{0}')),{3}, {4}, '{5}', '{6}');}}return false;", checkBoxID, alertText, url, height, width, title, tips);
            }
        }

        public static string FailureMessage(string alertString)
        {
            return string.Format(@"failureMessage('{0}');", alertString);
        }

        public static void CloseDialogPage(Page page)
        {
            page.Response.Clear();
            page.Response.Write("<script language=\"javascript\">window.parent.hideDialog();window.parent._refresh();</script>");
            page.Response.End();
        }

        public static void CloseDialogPageAndRedirect(Page page, string redirectUrl)
        {
            page.Response.Clear();
            page.Response.Write(string.Format("<script language=\"javascript\">window.parent.hideDialog();window.parent._goto('{0}');</script>", redirectUrl));
            page.Response.End();
        }

        public static void CloseDialogPageWithoutRefresh(Page page)
        {
            page.Response.Clear();
            page.Response.Write("<script language=\"javascript\">window.parent.hideDialog();</script>");
            page.Response.End();
        }

        public static void CloseDialogPageWithoutRefresh(Page page, string scripts)
        {
            page.Response.Clear();
            page.Response.Write(string.Format("<script language=\"javascript\">{0}</script>", scripts));
            page.Response.Write("<script language=\"javascript\">window.parent.hideDialog();</script>");
            page.Response.End();
        }
    }
}
