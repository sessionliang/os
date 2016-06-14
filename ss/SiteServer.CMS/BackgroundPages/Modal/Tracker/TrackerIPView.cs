using System;
using System.Collections.Specialized;
using System.IO;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System.Collections;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class TrackerIPView : BackgroundBasePage
	{
        public DataList MyDataList;
        protected Button ExportAndDelete;
        protected Button Export;

        private string startDateString;
        private string endDateString;
        private int nodeID;
        private int contentID;
        private int totalNum;
        private int totalAccessNum = 0;

        public static string GetOpenWindowString(string startDateString, string endDateString, int publishmentSystemID, int nodeID, int contentID, int totalNum)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("StartDateString", startDateString);
            arguments.Add("EndDateString", endDateString);
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("TotalNum", totalNum.ToString());
            return PageUtility.GetOpenWindowString("内容点击详细记录", "modal_trackerIPView.aspx", arguments, 580, 520, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.startDateString = base.GetQueryString("StartDateString");
            this.endDateString = base.GetQueryString("EndDateString");
            this.nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"));
            this.contentID = TranslateUtils.ToInt(base.GetQueryString("ContentID"));
            this.totalNum = TranslateUtils.ToInt(base.GetQueryString("TotalNum"));

            if (this.nodeID == this.PublishmentSystemID)
            {
                this.ExportAndDelete.Visible = true;
            }

			if (!IsPostBack)
			{
                DateTime begin = DateUtils.SqlMinValue;
                if (!string.IsNullOrEmpty(this.startDateString))
                {
                    begin = TranslateUtils.ToDateTime(this.startDateString);
                }
                DateTime end = TranslateUtils.ToDateTime(this.endDateString);

                if (!string.IsNullOrEmpty(this.startDateString))
                {
                    base.InfoMessage(string.Format("开始时间：{0}&nbsp;&nbsp;结束时间：{1}&nbsp;&nbsp;总访问量：{2}", this.startDateString, this.endDateString, this.totalNum));
                }
                else
                {
                    base.InfoMessage(string.Format("结束时间：{1}&nbsp;&nbsp;总访问量：{2}", this.startDateString, this.endDateString, this.totalNum));
                }

                ArrayList ipAddresses = DataProvider.TrackingDAO.GetContentIPAddressArrayList(base.PublishmentSystemID, this.nodeID, this.contentID, begin, end);
                SortedList cityWithNumSortedList = BaiRongDataProvider.IP2CityDAO.GetCityWithNumSortedList(ipAddresses);
                foreach (string city in cityWithNumSortedList.Keys)
                {
                    int accessNum = 0;
                    if (cityWithNumSortedList[city] != null)
                    {
                        accessNum = Convert.ToInt32(cityWithNumSortedList[city]);
                    }
                    this.totalAccessNum += accessNum;
                }

                this.MyDataList.DataSource = cityWithNumSortedList;
                this.MyDataList.ItemDataBound += new DataListItemEventHandler(MyDataList_ItemDataBound);
                this.MyDataList.DataBind();

				
			}
		}

        public void Export_Click(object sender, EventArgs e)
        {
            string redirectUrl = PageUtility.ModalSTL.ExportMessage.GetRedirectUrlStringToExportTracker(this.startDateString, this.endDateString, base.PublishmentSystemID, this.nodeID, this.contentID, this.totalNum, false);
            PageUtils.Redirect(redirectUrl);
        }

        public void ExportAndDelete_Click(object sender, EventArgs e)
        {
            string redirectUrl = PageUtility.ModalSTL.ExportMessage.GetRedirectUrlStringToExportTracker(this.startDateString, this.endDateString, base.PublishmentSystemID, this.nodeID, this.contentID, this.totalNum, true);
            PageUtils.Redirect(redirectUrl);
        }

        void MyDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DictionaryEntry entry = (DictionaryEntry)e.Item.DataItem;

                Literal ltlItemTitle = (Literal)e.Item.FindControl("ltlItemTitle");
                Literal ltlAccessNumBar = (Literal)e.Item.FindControl("ltlAccessNumBar");
                Literal ltlItemCount = (Literal)e.Item.FindControl("ltlItemCount");

                int accessNum = Convert.ToInt32(entry.Value);

                ltlItemTitle.Text = entry.Key.ToString();
                ltlAccessNumBar.Text = string.Format(@"<div class=""progress progress-success progress-striped"">
            <div class=""bar"" style=""width: {0}%""></div>
          </div>", GetAccessNumBarWidth(accessNum));
                ltlItemCount.Text = accessNum.ToString();
            }
        }

        private double GetAccessNumBarWidth(int accessNum)
        {
            double width = 0;
            if (this.totalAccessNum > 0)
            {
                width = Convert.ToDouble(accessNum) / Convert.ToDouble(this.totalAccessNum);
                width = Math.Round(width, 2) * 200;
            }
            return width;
        }
	}
}
