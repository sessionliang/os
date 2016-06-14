using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Controls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.BBS.Model;
using System.Web.UI;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundSusceptivityPostPassed : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = DataProvider.ConnectionString;

            this.spContents.SelectCommand = GetSelectCommend();
            this.spContents.SortField = "CheckDate";
            this.spContents.SortMode = SortMode.DESC;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Content, "查看已审核记录", AppManager.BBS.Permission.BBS_Content);

                spContents.DataBind();
            }
        }

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlNum = e.Item.FindControl("ltlNum") as Literal;
                Literal ltlThread = e.Item.FindControl("ltlThread") as Literal;
                Literal ltlUser = e.Item.FindControl("ltlUser") as Literal;
                Literal ltlAssessor = e.Item.FindControl("ltlAssessor") as Literal;
                Literal ltlTime = e.Item.FindControl("ltlTime") as Literal;
                Literal ltlLookUp = e.Item.FindControl("ltlLookUp") as Literal;

                int id = (int)DataBinder.Eval(e.Item.DataItem, "ID");

                ltlNum.Text = ConvertHelper.GetString(e.Item.ItemIndex + 1);
                ltlThread.Text = GetThreadTitleByID((int)DataBinder.Eval(e.Item.DataItem, "ThreadID"));
                ltlUser.Text = (string)DataBinder.Eval(e.Item.DataItem, "UserName");
                ltlAssessor.Text = (string)DataBinder.Eval(e.Item.DataItem, "Assessor");
                ltlTime.Text = ConvertHelper.GetString(DataBinder.Eval(e.Item.DataItem, "CheckDate"));
                ltlLookUp.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">查看</a>", Modal.LookupTheKeywordsPost.GetOpenWindowString(base.PublishmentSystemID, id, "Look"));
            }
        }

        public string GetSelectCommend()
        {
            string isChecked = "True";
            string Assessor = "";
            string sqlString =string.Format("SELECT ID,ThreadID,UserName,Assessor,CheckDate FROM bbs_Post where IsChecked='{0}' and Assessor!='{1}'",isChecked,Assessor);
            return sqlString;
        }

        public string GetThreadTitleByID(int ID)
        {
            ThreadInfo info = DataProvider.ThreadDAO.GetThreadInfo(base.PublishmentSystemID, ID);
            return info.Title;
        }
    }
}
