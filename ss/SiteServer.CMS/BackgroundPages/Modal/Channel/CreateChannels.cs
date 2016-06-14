using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class CreateChannels : BackgroundBasePage
	{
		protected RadioButtonList IsIncludeChildren;
        protected RadioButtonList IsCreateContents;

        private string channelIDCollection;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowStringWithCheckBoxValue("生成栏目页", "modal_createChannels.aspx", arguments, "ChannelIDCollection", "请选择需要生成页面的栏目!", 450, 300);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ChannelIDCollection");

            this.channelIDCollection = base.GetQueryString("ChannelIDCollection");
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            PageUtils.Redirect(Modal.ProgressBar.GetRedirectUrlStringWithCreateChannelsOneByOne(base.PublishmentSystemID, this.channelIDCollection, this.IsIncludeChildren.SelectedValue, this.IsCreateContents.SelectedValue));
		}
	}
}
