using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Core;
using System.Web.UI;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class FilePathRuleMake : BackgroundBasePage
    {
        protected Literal Rules;
        protected TextBox TheRule;

        string textBoxClientID = string.Empty;

        public static string GetOpenWindowString(int publishmentSystemID, string textBoxClientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("TextBoxClientID", textBoxClientID);
            return JsUtils.OpenWindow.GetOpenWindowStringWithTextBoxValue("命名规则构造器", PageUtils.GetBBSUrl("modal_filePathRuleMake.aspx"), arguments, textBoxClientID, 480, 360);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.textBoxClientID = Request.QueryString["TextBoxClientID"];
            if (!IsPostBack)
            {
                this.Rules.Text = this.GetRulesString();
                if (!string.IsNullOrEmpty(textBoxClientID))
                {
                    this.TheRule.Text = Request.QueryString[textBoxClientID];
                }
            }
        }

        private string GetRulesString()
        {
            string retval = string.Empty;

            StringBuilder builder = new StringBuilder();
            string tr = "<tr class=\"tdbg\" onmouseover=\"this.className='tdbg-dark';\" onmouseout=\"this.className='tdbg';\" style=\"height:23px;\">";
            int mod = 0;
            int count = 0;
            IDictionary entitiesDictionary = PathUtilityBBS.FilePathRulesForum.GetDictionary();
            foreach (string label in entitiesDictionary.Keys)
            {
                count++;
                string labelName = (string)entitiesDictionary[label];
                string td = string.Format("<td width=\"0\" align=\"Left\"><a href=\"javascript:;\" onclick=\"AddOnPos(_get('TheRule'),'{0}');return false;\" target=\"_blank\">{0}</a></td><td width=\"0\" align=\"Left\">{1}</td>", label, labelName);
                if (count == entitiesDictionary.Count)
                {
                    td = string.Format("<td width=\"0\" align=\"Left\"><a href=\"javascript:;\" onclick=\"AddOnPos(_get('TheRule'),'{0}');return false;\" target=\"_blank\">{0}</a></td><td width=\"0\" align=\"Left\" colspan=\"5\">{1}</td></tr>", label, labelName);
                }
                if (mod++ % 2 == 0)
                {
                    builder.Append(tr + td);
                }
                else
                {
                    builder.Append(td);
                }
            }
            retval = builder.ToString();

            return retval;
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            string scripts = string.Format("window.parent.document.all.{0}.value = '{1}';", this.textBoxClientID, this.TheRule.Text);
            JsUtils.OpenWindow.CloseModalPageWithoutRefresh(Page, scripts);
        }
    }
}
