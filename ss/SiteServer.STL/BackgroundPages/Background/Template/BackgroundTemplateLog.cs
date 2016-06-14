using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using SiteServer.CMS.Services;
using BaiRong.Controls;
using BaiRong.Core.Data.Provider;
using System.Collections.Generic;
using System.Collections;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
    public class BackgroundTemplateLog : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Button btnCompare;
        public Button btnDelete;

        private int templateID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.templateID = base.GetIntQueryString("templateID");

            if (base.GetQueryString("Compare") != null)
            {
                List<int> idList = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (idList.Count != 2)
                {
                    base.FailMessage("请选择2条记录以便进行对比");
                }
            }
            if (base.GetQueryString("Delete") != null)
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.QueryString["IDCollection"]);
                try
                {
                    DataProvider.TemplateLogDAO.Delete(arraylist);
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = StringUtils.Constants.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            this.spContents.SelectCommand = DataProvider.TemplateLogDAO.GetSelectCommend(base.PublishmentSystemID, this.templateID);

            this.spContents.SortField = "ID";
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Template, "修订历史", AppManager.CMS.Permission.WebSite.Template);

                this.btnCompare.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.GetSTLUrl(string.Format("background_templateLog.aspx?PublishmentSystemID={0}&TemplateID={1}&Compare=True", base.PublishmentSystemID, this.templateID)), "IDCollection", "IDCollection", "请选择需要对比的记录！"));

                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetSTLUrl(string.Format("background_templateLog.aspx?PublishmentSystemID={0}&TemplateID={1}&Delete=True", base.PublishmentSystemID, this.templateID)), "IDCollection", "IDCollection", "请选择需要删除的修订历史！", "此操作将删除所选修订历史，确认吗？"));

                this.spContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlIndex = (Literal)e.Item.FindControl("ltlIndex");
                Literal ltlAddUserName = (Literal)e.Item.FindControl("ltlAddUserName");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlContentLength = (Literal)e.Item.FindControl("ltlContentLength");
                Literal ltlView = (Literal)e.Item.FindControl("ltlView");

                int logID = TranslateUtils.EvalInt(e.Item.DataItem, "ID");

                ltlIndex.Text = Convert.ToString(e.Item.ItemIndex + 1);
                ltlAddUserName.Text = TranslateUtils.EvalString(e.Item.DataItem, "AddUserName");
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate"));
                ltlContentLength.Text = TranslateUtils.EvalInt(e.Item.DataItem, "ContentLength").ToString();
                ltlView.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">查看</a>", Modal.TemplateView.GetOpenLayerString(base.PublishmentSystemID, logID));
            }
        }
    }
}
