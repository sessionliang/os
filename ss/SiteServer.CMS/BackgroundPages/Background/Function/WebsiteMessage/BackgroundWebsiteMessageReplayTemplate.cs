using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundWebsiteMessageReplayTemplate : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button btnDelete;

        private int templateID;
        private int classifyID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.classifyID = TranslateUtils.ToInt(base.GetQueryStringNoSqlAndXss("itemID"));//分类

            this.templateID = TranslateUtils.ToInt(base.GetQueryStringNoSqlAndXss("templateID"));

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.WebsiteMessageReplayTemplateDAO.GetSelectString(string.Empty);
            this.spContents.SortField = DataProvider.WebsiteMessageReplayTemplateDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_WebsiteMessage, "回复模板管理", AppManager.CMS.Permission.WebSite.WebsiteMessage);
                this.spContents.DataBind();
                string showPopWinString = string.Empty;

                showPopWinString = Modal.WebsiteMessageReplayTemplateAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.classifyID, this.PageUrl);
                this.btnAdd.Attributes.Add("onclick", showPopWinString);

                this.btnDelete.Attributes.Add("onclick", "return confirm(\"此操作将删除所选内容，确定吗？\");");

            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int contentID = TranslateUtils.EvalInt(e.Item.DataItem, "ID");
                WebsiteMessageReplayTemplateInfo contentInfo = DataProvider.WebsiteMessageReplayTemplateDAO.GetWebsiteMessageReplayTemplateInfo(contentID);
                Literal itemEidtRow = (Literal)e.Item.FindControl("ItemEidtRow");
                Literal itemDateTime = e.Item.FindControl("ItemDateTime") as Literal;
                Literal itemTitle = e.Item.FindControl("ItemTitle") as Literal;

                itemDateTime.Text = DateUtils.GetDateString(contentInfo.AddDate);
                itemTitle.Text = contentInfo.TemplateTitle;
                itemEidtRow.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">修改</a>", Modal.WebsiteMessageReplayTemplateAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, contentInfo.ID, this.classifyID, this.PageUrl));
            }
        }

        public void btnDelete_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (Request.Form["ContentIDCollection"] != null)
                {
                    ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.Form["ContentIDCollection"]);
                    try
                    {
                        DataProvider.WebsiteMessageReplayTemplateDAO.Delete(arraylist);
                        base.SuccessMessage("删除成功！");
                        PageUtils.Redirect(this.PageUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }
            }
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_websiteMessageReplayTemplate.aspx?PublishmentSystemID={0}&TemplateID={1}&ItemID={2}", base.PublishmentSystemID, this.templateID, this.classifyID));
                }
                return _pageUrl;
            }
        }
    }
}
