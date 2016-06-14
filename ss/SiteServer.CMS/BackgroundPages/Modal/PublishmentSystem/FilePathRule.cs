using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class FilePathRule : BackgroundBasePage
	{
        protected Literal ltlRules;
        protected TextBox tbRule;
        protected Literal ltlTips;

        private int nodeID = 0;
        private bool isChannel = false;
        private string textBoxClientID = string.Empty;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, bool isChannel, string textBoxclientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("IsChannel", isChannel.ToString());
            arguments.Add("TextBoxClientID", textBoxclientID);
            return PageUtility.GetOpenWindowStringWithTextBoxValue(isChannel ? "栏目页文件名规则" : "内容页文件名规则", "modal_filePathRule.aspx", arguments, textBoxclientID);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"));
            this.isChannel = base.GetBoolQueryString("IsChannel");
            this.textBoxClientID = base.GetQueryString("TextBoxClientID");
			if (!IsPostBack)
			{
                this.ltlRules.Text = this.GetRulesString();
                if (!string.IsNullOrEmpty(textBoxClientID))
                {
                    this.tbRule.Text = base.GetQueryString(textBoxClientID);
                }
                if (this.isChannel)
                {
                    this.ltlTips.Text = "系统生成栏目页时采取的文件名规则，建议保留{@ChannelID}栏目ID项，否则可能出现重复的文件名称";
                }
                else
                {
                    this.ltlTips.Text = "系统生成内容页时采取的文件名规则，建议保留{@ContentID}内容ID项，否则可能出现重复的文件名称";
                }
			}
		}

        private string GetRulesString()
        {
            string retval = string.Empty;

            StringBuilder builder = new StringBuilder();
            int mod = 0;
            int count = 0;
            IDictionary entitiesDictionary = null;
            if (this.isChannel)
            {
                entitiesDictionary = PathUtility.ChannelFilePathRules.GetDictionary(base.PublishmentSystemInfo, this.nodeID);
            }
            else
            {
                entitiesDictionary = PathUtility.ContentFilePathRules.GetDictionary(base.PublishmentSystemInfo, this.nodeID);
            }
            
            foreach (string label in entitiesDictionary.Keys)
            {
                count++;
                string labelName = (string)entitiesDictionary[label];
                string td = string.Format(@"<td><a href=""javascript:;"" onclick=""AddOnPos('{0}');return false;"">{0}</a></td><td>{1}</td>", label, labelName);
                if (count == entitiesDictionary.Count)
                {
                    td = string.Format(@"<td><a href=""javascript:;"" onclick=""AddOnPos('{0}');return false;"">{0}</a></td><td colspan=""5"">{1}</td></tr>", label, labelName);
                }
                if (mod++ % 3 == 0)
                {
                    builder.Append("<tr>" + td);
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
            string scripts = string.Format("window.parent.document.all.{0}.value = '{1}';", this.textBoxClientID, this.tbRule.Text);
            JsUtils.OpenWindow.CloseModalPageWithoutRefresh(Page, scripts);
		}
	}
}
