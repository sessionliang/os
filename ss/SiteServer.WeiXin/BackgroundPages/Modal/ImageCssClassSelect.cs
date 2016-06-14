using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Net;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;

using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using SiteServer.CMS.Core;
using System.Web.UI.HtmlControls;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class ImageCssClassSelect : BackgroundBasePage
    {
        public Literal ltlScript;

        private string jsMethod;
        private int itemIndex;
        
        public static string GetOpenWindowString(int publishmentSystemID, string jsMethod)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("jsMethod", jsMethod);
            return PageUtilityWX.GetOpenWindowString("选择导航图标", "modal_imageCssClassSelect.aspx", arguments, true);
        }

        public static string GetOpenWindowStringByItemIndex(int publishmentSystemID, string jsMethod, string itemIndex)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("jsMethod", jsMethod);
            arguments.Add("itemIndex", itemIndex);
            return PageUtilityWX.GetOpenWindowString("选择导航图标", "modal_imageCssClassSelect.aspx", arguments, true);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.jsMethod = base.Request.QueryString["jsMethod"];
            this.itemIndex = TranslateUtils.ToInt(base.Request.QueryString["itemIndex"]);

            this.ltlScript.Text = string.Format(@"
            $(""a"").click(function () {{
               var cssClass = $(this).children().first().attr('class');
               window.parent.{0}({1}, cssClass);
               {2}
           }});", this.jsMethod, this.itemIndex, JsUtils.OpenWindow.HIDE_POP_WIN);
        }         
    }
}
