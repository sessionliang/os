using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class ReferenceLogs : MLibBackgroundBasePage
    {

        public Repeater rptContents;
        public SqlPager spContents;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;

        public void Page_Load(object sender, EventArgs E)
        {
            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;


            this.spContents.SelectCommand = "SELECT [RLID],[ml_ReferenceLogs].[RTID],[PublishmentSystemID],[NodeID],[ToContentID],[Operator],[OperateDate],[ml_ReferenceLogs].[SubmissionID], ";
            this.spContents.SelectCommand += " (select title from ml_Submission where ml_Submission.SubmissionID=[ml_ReferenceLogs].SubmissionID) as title, ";
            this.spContents.SelectCommand += "(select RTName from ml_ReferenceType where ml_ReferenceType.RTID = ml_ReferenceLogs.RTID) as RTName FROM [ml_ReferenceLogs]  ";
            
            if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
            {
                this.spContents.SelectCommand += " where [ml_ReferenceLogs].SubmissionID=" + Request.QueryString["sid"];
            }
            if (!string.IsNullOrEmpty(Request["DateFrom"]))
            {
                DateFrom.Text = Request["DateFrom"];
                this.spContents.SelectCommand += " and OperateDate>'" + Request["DateFrom"] + " 00:00:00'";           
            }

            if (!string.IsNullOrEmpty(Request["DateTo"]))
            {
                DateTo.Text = Request["DateTo"];
                this.spContents.SelectCommand += " and OperateDate<'" + Request["DateTo"] + " 23:59:59'";
            }

            this.spContents.SortField = "OperateDate";
            this.spContents.SortMode = SortMode.DESC;
            this.spContents.OrderByString = "ORDER BY OperateDate DESC";
            if (!IsPostBack)
            {
                this.spContents.DataBind();
            }
        }


        int i = 0;
        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                i++;
                Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlItemTitle = e.Item.FindControl("ltlItemTitle") as Literal;
                Literal ltlRType = e.Item.FindControl("ltlRType") as Literal;
                Literal ltlOpreateDate = e.Item.FindControl("ltlOpreateDate") as Literal;
                Literal ltlOpreator = e.Item.FindControl("ltlOpreator") as Literal;


                ltlID.Text = i.ToString();
                //ltlItemTitle.Text = TranslateUtils.Eval(e.Item.DataItem, "Title").ToString();
                if (TranslateUtils.Eval(e.Item.DataItem, "RTID").ToString() == "0")
                {
                    int pid = TranslateUtils.ToInt(TranslateUtils.Eval(e.Item.DataItem, "PublishmentSystemID").ToString());
                    int nodeid = TranslateUtils.ToInt(TranslateUtils.Eval(e.Item.DataItem, "NodeID").ToString());
                    var pInfo = PublishmentSystemManager.GetPublishmentSystemInfo(pid);
                    var nodeName = NodeManager.GetNodeName(pid, nodeid);
                    ltlRType.Text = "站点引用:" + pInfo.PublishmentSystemName + " - " + nodeName;
                }
                else
                {
                    ltlRType.Text = TranslateUtils.Eval(e.Item.DataItem, "RTName").ToString();
                }

                ltlOpreateDate.Text = TranslateUtils.ToDateTime(TranslateUtils.Eval(e.Item.DataItem, "OperateDate").ToString()).ToString("yyyy-MM-dd");

                ltlOpreator.Text = TranslateUtils.Eval(e.Item.DataItem, "Operator").ToString();
            }


        }
        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = string.Format("ReferrenceLogs.aspx?sid={0}&DateFrom={1}&DateTo={2}", Request.QueryString["sid"], DateFrom.Text, DateTo.Text);
                }
                return _pageUrl;
            }
        }

    }
}
