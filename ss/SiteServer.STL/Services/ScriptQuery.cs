using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using SiteServer.CMS.Model;
using System.Collections.Specialized;


namespace SiteServer.CMS.Services
{
    public class ScriptQuery : BasePage
    {
        public Literal ltlScript;

        public void Page_Load(object sender, System.EventArgs e)
        {
            string action = base.Request.QueryString["action"];
            string callback_success = base.Request.QueryString["callback_success"];
            string callback_failure = base.Request.QueryString["callback_failure"];

            if (StringUtils.EqualsIgnoreCase(action, "isLogin"))
            {
                if (BaiRongDataProvider.UserDAO.IsAnonymous)
                {
                    this.ltlScript.Text = callback_failure;
                }
                else
                {
                    this.ltlScript.Text = callback_success;
                }
            }

            
        }
    }
}
