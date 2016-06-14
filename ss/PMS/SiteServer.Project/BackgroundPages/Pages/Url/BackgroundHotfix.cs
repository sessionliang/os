using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Controls;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundHotfix : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Button AddButton;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.Request.QueryString["Delete"] != null)
            {
                int id = int.Parse(base.Request.QueryString["ID"]);
                DataProvider.HotfixDAO.Delete(id);
                base.SuccessMessage("成功删除升级包");
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 50;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.HotfixDAO.GetSelectSqlString();
            this.spContents.SortField = DataProvider.HotfixDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                this.spContents.DataBind();

                this.AddButton.Attributes.Add("onclick", Modal.HotfixAdd.GetShowPopWinStringToAdd());
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int id = (int)DataBinder.Eval(e.Item.DataItem, "ID");
                string version = (string)DataBinder.Eval(e.Item.DataItem, "Version");
                int hotfix = (int)DataBinder.Eval(e.Item.DataItem, "Hotfix");
                string fileUrl = (string)DataBinder.Eval(e.Item.DataItem, "FileUrl");
                string pageUrl = (string)DataBinder.Eval(e.Item.DataItem, "PageUrl");
                DateTime pubDate = (DateTime)DataBinder.Eval(e.Item.DataItem, "PubDate");
                string message = (string)DataBinder.Eval(e.Item.DataItem, "Message");
                bool isEnabled = TranslateUtils.ToBool((string)DataBinder.Eval(e.Item.DataItem, "IsEnabled"));
                bool isRestrict = TranslateUtils.ToBool((string)DataBinder.Eval(e.Item.DataItem, "IsRestrict"));
                int downloadCount = (int)DataBinder.Eval(e.Item.DataItem, "DownloadCount");

                Literal ltlVersion = e.Item.FindControl("ltlVersion") as Literal;
                Literal ltlHotfix = e.Item.FindControl("ltlHotfix") as Literal;
                Literal ltlPubDate = e.Item.FindControl("ltlPubDate") as Literal;
                Literal ltlMessage = e.Item.FindControl("ltlMessage") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlIsRestrict = e.Item.FindControl("ltlIsRestrict") as Literal;
                Literal ltlDownloadCount = e.Item.FindControl("ltlDownloadCount") as Literal;
                Literal ltlDownloadUrl = e.Item.FindControl("ltlDownloadUrl") as Literal;
                Literal ltlFileUrl = e.Item.FindControl("ltlFileUrl") as Literal;
                Literal ltlPageUrl = e.Item.FindControl("ltlPageUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlVersion.Text = version;
                ltlHotfix.Text = hotfix == 0 ? string.Empty : string.Format("SP {0}", hotfix);
                ltlPubDate.Text = DateUtils.GetDateString(pubDate, EDateFormatType.Chinese);
                ltlMessage.Text = message;
                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(isEnabled);
                ltlIsRestrict.Text = StringUtils.GetTrueImageHtml(isRestrict);
                ltlDownloadCount.Text = downloadCount.ToString();
                if (downloadCount > 0)
                {
                    ltlDownloadUrl.Text = string.Format(@"<a href=""product_download.aspx?hotfixID={0}"">查看</a>", id);
                }
                if (!string.IsNullOrEmpty(fileUrl))
                {
                    ltlFileUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">下载地址</a>", fileUrl);
                }
                if (!string.IsNullOrEmpty(pageUrl))
                {
                    ltlPageUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">升级说明</a>", pageUrl);
                }
                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.HotfixAdd.GetShowPopWinStringToEdit(id));
            }
        }
    }
}
