using BaiRong.Controls;
using BaiRong.Core;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class ReferenceSet : MLibBackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;


        public Literal ltlID;
        public Literal ltlReferenceName;
        public Literal ltlAddDate;
        public Literal ltlItemEditUrl;
        public void Page_Load(object sender, EventArgs E)
        {
            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = " SELECT [RTID],[RTName],[AddDate]  FROM [ml_ReferenceType]";
            this.spContents.SortField = "RTName";
            this.spContents.SortMode = SortMode.DESC;
            this.spContents.OrderByString = "ORDER BY RTName DESC";
            this.spContents.DataBind();
        }
        int i = 0;
        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                i++;
                Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlReferenceName = e.Item.FindControl("ltlReferenceName") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlItemEditUrl = e.Item.FindControl("ltlItemEditUrl") as Literal;

                DataRowView rowData = (DataRowView)e.Item.DataItem;

                ltlID.Text = i.ToString();
                ltlReferenceName.Text = rowData["RTName"].ToString();
                ltlAddDate.Text = rowData["AddDate"] is DBNull ? "" : Convert.ToDateTime(rowData["AddDate"]).ToString("yyyy-MM-dd"); 
                ltlItemEditUrl.Text = "<a href=\"javascript:;\">查看引用</a>";
            }
        }
    }
}
