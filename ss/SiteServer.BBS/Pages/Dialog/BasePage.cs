using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using BaiRong.Core.Diagnostics;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.Pages.Dialog
{
    public class BasePage : Page
    {
        protected Literal ltlScripts;

        protected virtual bool IsAccessable
        {
            get { return true; }
        }

        public BasePage()
        {
            
        }

        private int publishmentSystemID = -1;
        public int PublishmentSystemID
        {
            get
            {
                if (publishmentSystemID == -1)
                {
                    publishmentSystemID = this.GetIntQueryString("PublishmentSystemID");
                }
                return publishmentSystemID;
            }
        }

        private PublishmentSystemInfo publishmentSystemInfo;
        public PublishmentSystemInfo PublishmentSystemInfo
        {
            get
            {
                if (publishmentSystemInfo == null)
                {
                    publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.PublishmentSystemID);
                }
                return publishmentSystemInfo;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (!IsAccessable && BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                if (this.ltlScripts != null)
                {
                    this.ltlScripts.Text = string.Format(@"
<script type=""text/javascript"">
window.top.showDialog('{0}', 350, 450, 'ÓÃ»§µÇÂ¼');
</script>
", PageUtilityBBS.GetDialogPageUrl(this.PublishmentSystemID, "login.aspx"));
                }
            }

            base.OnInit(e);
        }

        private void ShowDig(string functionName, string message, string callback)
        {
            if (this.ltlScripts != null)
            {
                if (string.IsNullOrEmpty(callback))
                {
                    this.ltlScripts.Text += string.Format(@"
<script type=""text/javascript"" language=""javascript"">{0}('{1}');</script>
", functionName, message);
                }
                else
                {
                    this.ltlScripts.Text += string.Format(@"
<script type=""text/javascript"" language=""javascript"">{0}('{1}', {2});</script>
", functionName, message, callback);
                }
            }
        }

        public void SuccessMessage(string message)
        {
            this.ShowDig("successMessage", message, string.Empty);
        }

        public void SuccessMessage(string message, string redirectUrl)
        {
            string callback = string.Format("function() {{location.href='{0}';}}", redirectUrl);
            this.ShowDig("successMessage", message, callback);
        }

        public void FailureMessage(string message)
        {
            this.ShowDig("failureMessage", message, string.Empty);
        }

        public void InfoMessage(string message)
        {
            this.ShowDig("infoMessage", message, string.Empty);
        }

        protected string GetQueryString(string name)
        {
            return GetQueryStringNoSql(name);
        }

        protected int GetIntQueryString(string name)
        {
            return TranslateUtils.ToInt(base.Request.QueryString[name]);
        }

        protected bool GetBoolQueryString(string name)
        {
            return TranslateUtils.ToBool(base.Request.QueryString[name]);
        }

        protected string GetQueryStringNoSql(string name)
        {
            string value = base.Request.QueryString[name];
            if (value == null) return null;
            return PageUtils.FilterSql(value);
        }

        protected string GetQueryStringNoXss(string name)
        {
            string value = base.Request.QueryString[name];
            if (value == null) return null;
            return PageUtils.FilterXSS(value);
        }

        protected string GetQueryStringNoSqlAndXss(string name)
        {
            string value = base.Request.QueryString[name];
            if (value == null) return null;
            return PageUtils.FilterSqlAndXss(value);
        }
    }
}
