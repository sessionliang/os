using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class ContentDiggSet : BackgroundBasePage
	{
		protected TextBox GoodNum;
        protected TextBox BadNum;

        private int channelID;
        private int contentID;

        public static string GetOpenWindowString(int publishmentSystemID, int channelID, int contentID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ChannelID", channelID.ToString());
            arguments.Add("ContentID", contentID.ToString());

            return PageUtility.GetOpenWindowString("内容Digg设置", "modal_contentDiggSet.aspx", arguments, 350, 280);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.channelID = TranslateUtils.ToInt(base.GetQueryString("ChannelID"));
            this.contentID = TranslateUtils.ToInt(base.GetQueryString("ContentID"));

			if (!IsPostBack)
			{
                int[] nums = BaiRongDataProvider.DiggDAO.GetCount(base.PublishmentSystemID, this.contentID);

                this.GoodNum.Text = Convert.ToString(nums[0]);
                this.BadNum.Text = Convert.ToString(nums[1]);
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

            try
            {
                int goodNum = TranslateUtils.ToInt(this.GoodNum.Text);
                int badNum = TranslateUtils.ToInt(this.BadNum.Text);

                BaiRongDataProvider.DiggDAO.SetCount(base.PublishmentSystemID, this.contentID, goodNum, badNum);
                StringUtility.AddLog(base.PublishmentSystemID, this.channelID, this.contentID, "设置内容Digg值", string.Empty);
                isChanged = true;
            }
            catch(Exception ex)
            {
                base.FailMessage(ex, "Digg设置失败！");
            }

			if (isChanged)
			{
				JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
