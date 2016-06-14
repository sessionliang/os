using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;



using SiteServer.BBS.Model;
using BaiRong.Core;
using System.Collections.Specialized;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.Pages.Dialog
{
    public class Register : BasePage
    {
        public PlaceHolder phValidateCode;
        public Literal ltlValidateCode;

        private VCManager vcManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.vcManager = VCManager.GetInstanceOfLogin();

            if (!IsPostBack)
            {
                if (FileConfigManager.Instance.IsValidateCode)
                {
                    this.ltlValidateCode.Text = string.Format(@"<img id=""imgVerify"" name=""imgVerify"" src=""{0}"" align=""absmiddle"" />", this.vcManager.GetImageUrl(true));
                }
                else
                {
                    this.phValidateCode.Visible = false;
                }
            }
        }
    }
}
