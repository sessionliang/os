using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class ContentStarSet : BackgroundBasePage
	{
		protected TextBox TotalCount;
        protected TextBox PointAverage;

        private int channelID;
        private int contentID;

        public static string GetOpenWindowString(int publishmentSystemID, int channelID, int contentID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ChannelID", channelID.ToString());
            arguments.Add("ContentID", contentID.ToString());

            return PageUtility.GetOpenWindowString("内容评分设置", "modal_contentStarSet.aspx", arguments, 350, 300);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.channelID = TranslateUtils.ToInt(base.GetQueryString("ChannelID"));
            this.contentID = TranslateUtils.ToInt(base.GetQueryString("ContentID"));

			if (!IsPostBack)
			{
                object[] totalCountAndPointAverage = DataProvider.StarSettingDAO.GetTotalCountAndPointAverage(base.PublishmentSystemID, contentID);
                int settingTotalCount = (int)totalCountAndPointAverage[0];
                decimal settingPointAverage = (decimal)totalCountAndPointAverage[1];

                if (settingTotalCount > 0 || settingPointAverage > 0)
                {
                    this.TotalCount.Text = settingTotalCount.ToString();
                    this.PointAverage.Text = settingPointAverage.ToString();
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

            try
            {
                int totalCount = TranslateUtils.ToInt(this.TotalCount.Text);
                decimal pointAverage = TranslateUtils.ToDecimal(this.PointAverage.Text);
                if (totalCount == 0)
                {
                    pointAverage = 0;
                }
                else if (pointAverage == 0)
                {
                    totalCount = 0;
                }

                StarsManager.SetStarSetting(base.PublishmentSystemID, this.channelID, this.contentID, totalCount, pointAverage);

                StringUtility.AddLog(base.PublishmentSystemID, this.channelID, this.contentID, "设置内容评分值", string.Empty);

                isChanged = true;
            }
            catch(Exception ex)
            {
                base.FailMessage(ex, "评分设置失败！");
            }

			if (isChanged)
			{
				JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
