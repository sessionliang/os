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

namespace SiteServer.BBS.Pages.Dialog
{
    public class Message : Page
    {
        private string messageType;
        public Literal ltlMessage;

        public string MessageType
        {
            get
            {
                return this.messageType;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.messageType = PageUtils.FilterSqlAndXss(base.Request.QueryString["type"]);
                this.ltlMessage.Text = PageUtils.FilterSqlAndXss(base.Request.QueryString["message"]);
            }
        }
    }
}
