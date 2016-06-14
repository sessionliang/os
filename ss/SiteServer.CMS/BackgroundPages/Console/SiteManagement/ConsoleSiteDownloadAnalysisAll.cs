using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;


using BaiRong.Controls;
using BaiRong.Model;
using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages
{
    public class ConsoleSiteDownloadAnalysisAll : BackgroundBasePage
    {
        public const string Download_CONTENT = "Download_CONTENT";

        public Repeater rpContents;
        public SqlPager spContents;
        public string returnUrl;

        private readonly Hashtable horizentalHashtable = new Hashtable();
        private readonly Hashtable verticalHashtable = new Hashtable();

        protected override bool IsSaasForbidden { get { return true; } }

        public static string GetRedirectUrl(string returnUrl)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("returnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtils.GetCMSUrl(PageUtils.AddQueryString("console_siteDownloadAnalysisAll.aspx", nvc));
        }


        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.spContents.ControlToPaginate = this.rpContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.PublishmentSystemDAO.GetSelectCommand();
            this.rpContents.ItemDataBound += new RepeaterItemEventHandler(rpContents_ItemDataBound);
            this.spContents.SortMode = SortMode.DESC; //排序

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "应用文件下载量统计", AppManager.Platform.Permission.Platform_Site);

                this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("returnUrl"));

                this.spContents.DataBind();

            }
        }

        private void AddToHorizental(int publishmentSystemID, int num)
        {
            if (horizentalHashtable[publishmentSystemID] == null)
            {
                horizentalHashtable[publishmentSystemID] = num;
            }
            else
            {
                int totalNum = (int)horizentalHashtable[publishmentSystemID];
                horizentalHashtable[publishmentSystemID] = totalNum + num;
            }
        }

        private void AddToVertical(string type, int num)
        {
            if (verticalHashtable[type] == null)
            {
                verticalHashtable[type] = num;
            }
            else
            {
                int totalNum = (int)verticalHashtable[type];
                verticalHashtable[type] = totalNum + num;
            }
        }

        public string GetVerticalNum(string type)
        {
            int totalNum = 0;
            if (verticalHashtable[type] != null)
            {
                totalNum = (int)verticalHashtable[type];
            }
            return (totalNum == 0) ? "0" : string.Format("<strong>{0}</strong>", totalNum);
        }

        public string GetDownloadNum(int publishmentSystemID)
        {
            Model.PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            int num = CountManager.GetCount(AppManager.CMS.AppID, publishmentSystemInfo.AuxiliaryTableForContent, publishmentSystemInfo.PublishmentSystemID, ECountType.Download);
            this.AddToHorizental(publishmentSystemID, num);
            this.AddToVertical(Download_CONTENT, num);
            return (num == 0) ? "0" : string.Format("<strong>{0}</strong>", num);
        }


        public string GetVerticalTotalNum()
        {
            int totalNum = 0;
            foreach (int num in verticalHashtable.Values)
            {
                totalNum += num;
            }
            return (totalNum == 0) ? "0" : string.Format("<strong>{0}</strong>", totalNum);
        }

        void rpContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int publishmentSystemID = TranslateUtils.EvalInt(e.Item.DataItem, "PublishmentSystemID");
                SiteServer.CMS.Model.PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

                Literal ltlPublishmentSystemName = e.Item.FindControl("ltlPublishmentSystemName") as Literal;
                Literal ltlDownloadNum = e.Item.FindControl("ltlDownloadNum") as Literal;

                ltlPublishmentSystemName.Text = publishmentSystemInfo.PublishmentSystemName + "&nbsp;" + EPublishmentSystemTypeUtils.GetIconHtml(publishmentSystemInfo.PublishmentSystemType);
                ltlDownloadNum.Text = this.GetDownloadNum(publishmentSystemID);
            }
        }

        public void Analysis_OnClick(object sender, EventArgs E)
        {
            this.spContents.DataBind();
        }
    }
}
