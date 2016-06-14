using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using SiteServer.Project.Core;
using BaiRong.Controls;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundDownload : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public void Page_Load(object sender, EventArgs E)
        {
            int hotfixID = TranslateUtils.ToInt(base.Request.QueryString["hotfixID"]);

            this.spContents.ControlToPaginate = rptContents;
            this.spContents.ItemsPerPage = 50;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.HotfixDownloadDAO.GetSelectSqlString(hotfixID);
            this.spContents.SortField = DataProvider.HotfixDownloadDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                this.spContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int id = (int)DataBinder.Eval(e.Item.DataItem, "ID");
                string version = (string)DataBinder.Eval(e.Item.DataItem, "Version");
                bool isBeta = TranslateUtils.ToBool((string)DataBinder.Eval(e.Item.DataItem, "IsBeta"));
                string domain = (string)DataBinder.Eval(e.Item.DataItem, "Domain");
                DateTime downloadDate = (DateTime)DataBinder.Eval(e.Item.DataItem, "DownloadDate");

                Literal ltlDomain = e.Item.FindControl("ltlDomain") as Literal;
                Literal ltlVersion = e.Item.FindControl("ltlVersion") as Literal;
                Literal ltlDownloadDate = e.Item.FindControl("ltlDownloadDate") as Literal;

                if (!string.IsNullOrEmpty(domain))
                {
                    ltlDomain.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", domain);
                }
                ltlVersion.Text = ProductManager.GetFullVersion(version, isBeta);
                ltlDownloadDate.Text = DateUtils.GetDateAndTimeString(downloadDate);
            }
        }
    }
}
