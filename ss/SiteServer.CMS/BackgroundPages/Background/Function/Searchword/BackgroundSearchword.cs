using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundSearchword : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;

        #region 操作按钮
        public Button btnAdd;
        public Button btnDelete;
        public Button btnUpdateBatch;
        public Button btnUpdateAll;
        public Button btnImport;
        public Button btnExport;
        #endregion

        #region 搜索条件
        public TextBox tbSearchResultCountFrom;
        public TextBox tbSearchResultCountTo;
        public TextBox tbSearchCountFrom;
        public TextBox tbSearchCountTo;
        public TextBox tbKeyword;
        #endregion


        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!string.IsNullOrEmpty(base.GetQueryString("Delete")))
            {
                int id = TranslateUtils.ToInt(base.GetQueryString("ID"));
                DataProvider.SearchwordDAO.Delete(id);
                base.SuccessMessage("删除成功！");
                PageUtils.Redirect(this.PageUrl);
            }
            else if (!string.IsNullOrEmpty(base.GetQueryString("UpdateResultCount")))
            {
                int id = TranslateUtils.ToInt(base.GetQueryString("ID"));
                DataProvider.SearchwordDAO.UpdateSearchResultCount(id);
                base.SuccessMessage("更新成功！");
                PageUtils.Redirect(this.PageUrl);
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            StringBuilder where = new StringBuilder(string.Format(" SearchResultCount >= {0} AND SearchCount <= {1} AND SearchResultCount >= {2} AND SearchCount <= {3} ",
                TranslateUtils.ToInt(base.GetQueryString("SearchResultCountFrom"), 0), TranslateUtils.ToInt(base.GetQueryString("SearchResultCountTo"), Int32.MaxValue),
                TranslateUtils.ToInt(base.GetQueryString("SearchCountFrom"), 0), TranslateUtils.ToInt(base.GetQueryString("SearchCountTo"), Int32.MaxValue)));
            where.AppendFormat(" AND searchword like '%{0}%' ", PageUtils.FilterSql(base.GetQueryString("Keyword")));

            this.spContents.SelectCommand = DataProvider.SearchwordDAO.GetSelectString(base.PublishmentSystemID, where.ToString());
            this.spContents.SortField = DataProvider.SearchwordDAO.GetSortFieldName(base.PublishmentSystemID);
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Searchword, "搜索关键词管理", AppManager.CMS.Permission.WebSite.Searchword);

                this.tbKeyword.Text = base.GetQueryString("Keyword");
                this.tbSearchCountFrom.Text = base.GetQueryString("SearchCountFrom");
                this.tbSearchCountTo.Text = base.GetQueryString("SearchCountTo");
                this.tbSearchResultCountFrom.Text = base.GetQueryString("SearchResultCountFrom");
                this.tbSearchResultCountTo.Text = base.GetQueryString("SearchResultCountTo");


                this.spContents.DataBind();
                string showPopWinString = string.Empty;

                showPopWinString = Modal.SearchwordAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.PageUrl);
                this.btnAdd.Attributes.Add("onclick", showPopWinString);

                this.btnDelete.Attributes.Add("onclick", "return confirm(\"此操作将删除所选内容，确定吗？\");");
                this.btnUpdateBatch.Attributes.Add("onclick", "return confirm(\"此操作将更新所选内容的搜索结果数，确定吗？\");");
                this.btnUpdateAll.Attributes.Add("onclick", "return confirm(\"此操作将更新全部内容的搜索结果数，将会处理一段时间，确定吗？\");");
                this.btnImport.Attributes.Add("onclick", Modal.SearchwordImport.GetOpenWindowString(base.PublishmentSystemID));
                this.btnExport.Attributes.Add("onclick", Modal.SearchwordExport.GetOpenWindowString(base.PublishmentSystemID));

            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int searchwordID = TranslateUtils.EvalInt(e.Item.DataItem, "ID");
                SearchwordInfo contentInfo = DataProvider.SearchwordDAO.GetSearchwordInfo(searchwordID);
                Literal ltlSearchword = e.Item.FindControl("ltlSearchword") as Literal;
                Literal ltlSearchResultCount = e.Item.FindControl("ltlSearchResultCount") as Literal;
                Literal ltlSearchCount = e.Item.FindControl("ltlSearchCount") as Literal;
                Literal ltlEdit = e.Item.FindControl("ltlEdit") as Literal;
                Literal ltlDelete = e.Item.FindControl("ltlDelete") as Literal;

                ltlSearchword.Text = contentInfo.Searchword;

                NameValueCollection nvcUpdateResultCount = new NameValueCollection();
                nvcUpdateResultCount.Add("ID", searchwordID.ToString());
                nvcUpdateResultCount.Add("UpdateResultCount", true.ToString());
                ltlSearchResultCount.Text = string.Format(@"{0} <a href=""javascript:;"" onclick=""{1}"">更新</a>", contentInfo.SearchResultCount.ToString(), JsUtils.GetRedirectStringWithConfirm(PageUtils.AddQueryString(this.PageUrl, nvcUpdateResultCount), "是否要更新该搜索关键词的搜索结果数？"));

                ltlSearchCount.Text = contentInfo.SearchCount.ToString();
                ltlEdit.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">修改</a>", Modal.SearchwordAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, searchwordID, this.PageUrl));

                NameValueCollection nvcDelete = new NameValueCollection();
                nvcDelete.Add("ID", searchwordID.ToString());
                nvcDelete.Add("Delete", true.ToString());
                ltlDelete.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">删除</a>", JsUtils.GetRedirectStringWithConfirm(PageUtils.AddQueryString(this.PageUrl, nvcDelete), "是否要删除该搜索关键词？"));
            }
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
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
                        DataProvider.SearchwordDAO.Delete(arraylist);
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

        public void btnUpdateBatch_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (Request.Form["ContentIDCollection"] != null)
                {
                    ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.Form["ContentIDCollection"]);
                    try
                    {
                        DataProvider.SearchwordDAO.UpdateSearchResultCount(arraylist);
                        base.SuccessMessage("更新成功！");
                        PageUtils.Redirect(this.PageUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "更新失败！");
                    }
                }
            }
        }

        public void btnUpdateAll_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    DataProvider.SearchwordDAO.UpdateSearchResultCountAll(base.PublishmentSystemID);
                    base.SuccessMessage("已经启动更新，您可以离开继续其他擦做！");
                    PageUtils.Redirect(this.PageUrl);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "更新失败！");
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
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_searchword.aspx?PublishmentSystemID={0}&SearchResultCountFrom={1}&SearchResultCountTo={2}&SearchCountFrom={3}&SearchCountTo={4}&Keyword={5}", base.PublishmentSystemID, this.tbSearchResultCountFrom.Text, this.tbSearchResultCountTo.Text, this.tbSearchCountFrom.Text, this.tbSearchCountTo.Text, this.tbKeyword.Text));
                }
                return _pageUrl;
            }
        }
    }
}
