using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
//using Xym.Core;
//using Xym.Core.Data.Provider;
using System.Data.SqlClient;
using System.Web;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;

namespace SiteServer.BBS.Controls
{
    #region 类 页面改变事件参数
    public class PageChangedEventArgs : EventArgs
    {
        public int OldPageIndex;
        public int NewPageIndex;
    }
    #endregion 


    [DefaultProperty("SelectCommand")]
    [DefaultEvent("PageIndexChanged")]
    [ToolboxData("<{0}:SqlPager runat=\"server\" />")]
    public class SqlPager : Table, INamingContainer
    {
        #region  PRIVATE DATA MEMBERS
        private PagedDataSource _dataSource;
        private Control _controlToPaginate;
        private string CacheKeyName
        {
            get { return Page.Request.FilePath + "_" + UniqueID + "_Data"; }
        }
        private string QueryPageCommandText = "SELECT * FROM " +
            "(SELECT TOP {0} * FROM " +
            "(SELECT TOP {1} * FROM ({2}) AS t0 ORDER BY {3} {4}) AS t1 " +
            "ORDER BY {3} {5}) AS t2 " +
            "ORDER BY {3} {4}";
        private string QueryCountCommandText = "SELECT COUNT(*) FROM ({0}) AS t0";
        const string PARM_PAGE = "page";
        #endregion

        #region CTOR(s)
        public SqlPager()
            : base()
        {
            _dataSource = null;
            _controlToPaginate = null;

            PagingMode = PagingMode.NonCached;
            PagerStyle = PagerStyle.NextPrev;
            CurrentPageIndex = 0;
            SelectCommand = "";
            ConnectionString = "";
            ItemsPerPage = 10;
            TotalPages = -1;
            CacheDuration = 60;
            SortMode = SortMode.DESC;
        }
        #endregion
        // ***********************************************************************
		/**//// <summary>
		/// 事件 页面索引改变
		/// 当跳转到新页面时发生
		/// </summary>
		delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);
		private event PageChangedEventHandler PageIndexChanged;
		protected virtual void OnPageIndexChanged(PageChangedEventArgs e)
		{
			if (PageIndexChanged != null)
				PageIndexChanged(this, e);
		}
		// ***********************************************************************
	

        #region PUBLIC PROGRAMMING INTERFACE
        /// <summary>
        /// Removes any data cached for paging
        /// </summary>
        public void ClearCache()
        {
            if (PagingMode == PagingMode.Cached)
                Page.Cache.Remove(CacheKeyName);
        }

        [Description("Gets and sets for how many seconds the data should stay in the cache")]
        public int CacheDuration
        {
            get { return Convert.ToInt32(ViewState["CacheDuration"]); }
            set { ViewState["CacheDuration"] = value; }
        }

        [Description("Indicates whether the data are retrieved page by page or can be cached")]
        public PagingMode PagingMode
        {
            get { return (PagingMode)ViewState["PagingMode"]; }
            set { ViewState["PagingMode"] = value; }
        }

        [Description("Indicates the style of the pager's navigation bar")]
        public PagerStyle PagerStyle
        {
            get { return (PagerStyle)ViewState["PagerStyle"]; }
            set { ViewState["PagerStyle"] = value; }
        }

        [Description("Gets and sets the name of the control to paginate")]
        public Control ControlToPaginate
        {
            get { return this._controlToPaginate; }
            set { this._controlToPaginate = value; }
        }

        [Description("Gets and sets the number of items to display per page")]
        public int ItemsPerPage
        {
            get { return Convert.ToInt32(ViewState["ItemsPerPage"]); }
            set { ViewState["ItemsPerPage"] = value; }
        }

        [Description("Gets and sets the index of the currently displayed page")]
        public int CurrentPageIndex
        {
            get { return Convert.ToInt32(ViewState["CurrentPageIndex"]); }
            set { ViewState["CurrentPageIndex"] = value; }
        }

        [Description("Gets and sets the connection string to access the database")]
        public string ConnectionString
        {
            get
            {
                string connectionString = ViewState["ConnectionString"] as string;
                if (string.IsNullOrEmpty(connectionString))
                {
                    return DataProvider.ConnectionString;
                }
                return connectionString;
            }
            set { ViewState["ConnectionString"] = value; }
        }

        [Description("Gets and sets the SQL query to get data")]
        public string SelectCommand
        {
            get { return Convert.ToString(ViewState["SelectCommand"]); }
            set { ViewState["SelectCommand"] = value; }
        }

        [Description("Gets and sets the sort-by field. It is mandatory in NonCached mode.)")]
        public string SortField
        {
            get { return Convert.ToString(ViewState["SortKeyField"]); }
            set { ViewState["SortKeyField"] = value; }
        }

        [Description("Gets and sets the Unit.)")]
        public string Unit
        {
            get { return Convert.ToString(ViewState["Unit"]); }
            set { ViewState["Unit"] = value; }
        }

        [Description("取得设置排序模式")]
        public SortMode SortMode
        {
            get { return (SortMode)ViewState["SortMode"]; }
            set { ViewState["SortMode"] = value; }
        }

        public string FirstText
        {
            get
            {
                string text = ViewState["FirstText"] as string;
                if (string.IsNullOrEmpty(text))
                {
                    text = "首页";
                }
                return text;
            }
            set { ViewState["FirstText"] = value; }
        }

        public string LastText
        {
            get
            {
                string text = ViewState["LastText"] as string;
                if (string.IsNullOrEmpty(text))
                {
                    text = "末页";
                }
                return text;
            }
            set { ViewState["LastText"] = value; }
        }

        public string PrevText
        {
            get
            {
                string text = ViewState["PrevText"] as string;
                if (string.IsNullOrEmpty(text))
                {
                    text = "上一页";
                }
                return text;
            }
            set { ViewState["PrevText"] = value; }
        }

        public string NextText
        {
            get
            {
                string text = ViewState["NextText"] as string;
                if (string.IsNullOrEmpty(text))
                {
                    text = "下一页";
                }
                return text;
            }
            set { ViewState["NextText"] = value; }
        }

        public string CurrentPageText
        {
            get
            {
                string text = ViewState["CurrentPageText"] as string;
                if (string.IsNullOrEmpty(text))
                {
                    text = "当前页：";
                }
                return text;
            }
            set { ViewState["CurrentPageText"] = value; }
        }

        public string EnabledCssClass
        {
            get { return ViewState["EnabledCssClass"] as string; }
            set { ViewState["EnabledCssClass"] = value; }
        }

        public string DisabledCssClass
        {
            get { return ViewState["DisabledCssClass"] as string; }
            set { ViewState["DisabledCssClass"] = value; }
        }

        public string TextCssClass
        {
            get { return ViewState["TextCssClass"] as string; }
            set { ViewState["TextCssClass"] = value; }
        }

        /// <summary>
        /// Gets the number of displayable pages 
        /// </summary>
        [Browsable(false)]
        public int PageCount
        {
            get { return TotalPages; }
        }

        /// <summary>
        /// Gets and sets the number of pages to display 
        /// </summary>
        protected int TotalPages
        {
            get { return Convert.ToInt32(ViewState["TotalPages"]); }
            set { ViewState["TotalPages"] = value; }
        }

        /// <summary>
        /// Gets and sets the number of pages to display 
        /// </summary>
        public int TotalCount
        {
            get { return Convert.ToInt32(ViewState["TotalCount"]); }
            set { ViewState["TotalCount"] = value; }
        }

        /// <summary>
        /// Fetches and stores the data
        /// </summary>
        public override void DataBind()
        {
            CurrentPageIndex = TranslateUtils.ToInt(Page.Request.QueryString[SqlPager.PARM_PAGE], 1) - 1;

            base.DataBind();

            // Controls must be recreated after data binding 
            ChildControlsCreated = false;

            // Ensures the control exists and is a list control
            if (_controlToPaginate == null)
                return;
            if (!(_controlToPaginate is BaseDataList || _controlToPaginate is Repeater || _controlToPaginate is ListControl))
                return;

            // Ensures enough info to connect and query is specified
            if (ConnectionString == "" || SelectCommand == "")
                return;

            // Fetch data
            if (PagingMode == PagingMode.Cached)
            {
                FetchAllData();
            }
            else
            {
                FetchPageData();
            }

            // Bind data to the buddy control
            BaseDataList baseDataListControl = null;
            Repeater baseRepeaterControl = null;
            ListControl listControl = null;
            if (_controlToPaginate is BaseDataList)
            {
                baseDataListControl = (BaseDataList)_controlToPaginate;
                baseDataListControl.DataSource = _dataSource;
                baseDataListControl.DataBind();
            }
            else if (_controlToPaginate is Repeater)
            {
                baseRepeaterControl = (Repeater)_controlToPaginate;
                baseRepeaterControl.DataSource = _dataSource;
                baseRepeaterControl.DataBind();
            }
            else if (_controlToPaginate is ListControl)
            {
                listControl = (ListControl)_controlToPaginate;
                listControl.Items.Clear();
                listControl.DataSource = _dataSource;
                listControl.DataBind();
            }
        }

        /// <summary>
        /// Writes the content to be rendered on the client
        /// </summary>
        protected override void Render(HtmlTextWriter output)
        {
            // If in design-mode ensure that child controls have been created.
            // Child controls are not created at this time in design-mode because
            // there's no pre-render stage. Do so for composite controls like this 
            if (Site != null && Site.DesignMode)
                CreateChildControls();

            base.Render(output);
        }

        /// <summary>
        /// Outputs the HTML markup for the control
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();
            ClearChildViewState();

            BuildControlHierarchy();
        }

        #endregion

        #region PRIVATE HELPER METHODS

        /// <summary>
        /// Control the building of the control's hierarchy
        /// </summary>
        private void BuildControlHierarchy()
        {
            if (this.TotalPages > 1)
            {
                // Build the surrounding table (one row, two cells)

                // Build the table row
                TableRow row = new TableRow();
                row.Height = 25;
                this.Rows.Add(row);
                //t.Rows.Add(row);

                // Build the cell with navigation bar
                TableCell cellNavBar = new TableCell();
                cellNavBar.VerticalAlign = VerticalAlign.Middle;
                if (PagerStyle == PagerStyle.NextPrev)
                {
                    BuildNextPrevUI(cellNavBar);
                    row.Cells.Add(cellNavBar);
                    // Build the cell with the page index
                    TableCell cellPageDesc = new TableCell();
                    if (!string.IsNullOrEmpty(this.TextCssClass))
                    {
                        cellPageDesc.CssClass = this.TextCssClass;
                    }
                    cellPageDesc.HorizontalAlign = HorizontalAlign.Right;
                    cellPageDesc.VerticalAlign = VerticalAlign.Top;
                    BuildCurrentPage(cellPageDesc);
                    row.Cells.Add(cellPageDesc);
                }
                else if(PagerStyle==PagerStyle.NumericPages)
                {
                    BuildNumericPagesUI(cellNavBar);
                    row.Cells.Add(cellNavBar);
                    // Build the cell with the page index
                    TableCell cellPageDesc = new TableCell();
                    if (!string.IsNullOrEmpty(this.TextCssClass))
                    {
                        cellPageDesc.CssClass = this.TextCssClass;
                    }
                    cellPageDesc.HorizontalAlign = HorizontalAlign.Right;
                    cellPageDesc.VerticalAlign = VerticalAlign.Top;
                    BuildCurrentPage(cellPageDesc);
                    row.Cells.Add(cellPageDesc);
                }
            }
        }

        private string GetNavigationUrl(int page)
        {
            string pageUrl = HttpContext.Current.Request.RawUrl;

            if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[SqlPager.PARM_PAGE]))
            {
                return pageUrl.Replace(".aspx", string.Format("-p{0}.aspx", page));
            }
            else
            {
                return pageUrl.Replace(string.Format("-p{0}.aspx", HttpContext.Current.Request.QueryString[SqlPager.PARM_PAGE]), string.Format("-p{0}.aspx", page));
            }

            //NameValueCollection queryString = new NameValueCollection(Page.Request.QueryString);
            //if (page > 1)
            //{
            //    queryString[SqlPager.PARM_PAGE] = page.ToString();
            //}
            //else
            //{
            //    queryString.Remove(SqlPager.PARM_PAGE);
            //}
            //return PageUtils.AddQueryString(PageUtils.GetUrlWithoutQueryString(Page.Request.RawUrl), queryString);
        }

        /// <summary>
        /// Generates the HTML markup for the Next/Prev navigation bar
        /// </summary>
        /// <param name="cell"></param>
        private void BuildNextPrevUI(TableCell cell)
        {
            bool isValidPage = (CurrentPageIndex >= 0 && CurrentPageIndex <= TotalPages - 1);
            bool canMoveBack = (CurrentPageIndex > 0);
            bool canMoveForward = (CurrentPageIndex < TotalPages - 1);

            // 首页
            bool enabled = isValidPage && canMoveBack;
            Label firstText = new Label();
            firstText.Text = this.FirstText;

            if (enabled)
            {
                HyperLink link = new HyperLink();
                link.Style.Add("text-decoration", "none");
                link.NavigateUrl = this.GetNavigationUrl(1);
                if (!string.IsNullOrEmpty(this.EnabledCssClass))
                {
                    link.CssClass = this.EnabledCssClass;
                }
                link.Controls.Add(new LiteralControl("&nbsp;"));
                link.Controls.Add(firstText);
                cell.Controls.Add(link);
            }
            else
            {
                cell.Controls.Add(new LiteralControl("&nbsp;"));
                if (!string.IsNullOrEmpty(this.DisabledCssClass))
                {
                    firstText.CssClass = this.DisabledCssClass;
                }
                else
                {
                    firstText.Style.Add("color", "gray");
                }
                cell.Controls.Add(firstText);
            }

            cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

            // 上一页
            Label prevText = new Label();
            prevText.Text = this.PrevText;

            if (enabled)
            {
                HyperLink link = new HyperLink();
                link.Style.Add("text-decoration", "none");
                link.NavigateUrl = this.GetNavigationUrl(this.CurrentPageIndex);
                if (!string.IsNullOrEmpty(this.EnabledCssClass))
                {
                    link.CssClass = this.EnabledCssClass;
                }
                link.Controls.Add(new LiteralControl("&nbsp;"));
                link.Controls.Add(prevText);
                cell.Controls.Add(link);
            }
            else
            {
                cell.Controls.Add(new LiteralControl("&nbsp;"));
                if (!string.IsNullOrEmpty(this.DisabledCssClass))
                {
                    prevText.CssClass = this.DisabledCssClass;
                }
                else
                {
                    prevText.Style.Add("color", "gray");
                }
                cell.Controls.Add(prevText);
            }

            cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

            // 下一页
            enabled = isValidPage && canMoveForward;
            Label nextText = new Label();
            nextText.Text = this.NextText;

            if (enabled)
            {
                HyperLink link = new HyperLink();
                link.Style.Add("text-decoration", "none");
                link.NavigateUrl = this.GetNavigationUrl(this.CurrentPageIndex + 2);
                if (!string.IsNullOrEmpty(this.EnabledCssClass))
                {
                    link.CssClass = this.EnabledCssClass;
                }
                link.Controls.Add(new LiteralControl("&nbsp;"));
                link.Controls.Add(nextText);
                cell.Controls.Add(link);
            }
            else
            {
                cell.Controls.Add(new LiteralControl("&nbsp;"));
                if (!string.IsNullOrEmpty(this.DisabledCssClass))
                {
                    nextText.CssClass = this.DisabledCssClass;
                }
                else
                {
                    nextText.Style.Add("color", "gray");
                }
                cell.Controls.Add(nextText);
            }

            cell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

            // 末页
            Label lastText = new Label();
            lastText.Text = this.LastText;

            if (enabled)
            {
                HyperLink link = new HyperLink();
                link.Style.Add("text-decoration", "none");
                link.NavigateUrl = this.GetNavigationUrl(this.TotalPages);
                if (!string.IsNullOrEmpty(this.EnabledCssClass))
                {
                    link.CssClass = this.EnabledCssClass;
                }
                link.Controls.Add(new LiteralControl("&nbsp;"));
                link.Controls.Add(lastText);
                cell.Controls.Add(link);
            }
            else
            {
                cell.Controls.Add(new LiteralControl("&nbsp;"));
                if (!string.IsNullOrEmpty(this.DisabledCssClass))
                {
                    lastText.CssClass = this.DisabledCssClass;
                }
                else
                {
                    lastText.Style.Add("color", "gray");
                }
                cell.Controls.Add(lastText);
            }
        }

        /// <summary>
        /// Generates the HTML markup for the Numeric Pages button bar
        /// </summary>
        private void BuildNumericPagesUI(TableCell cell)
        {
                        if (TotalPages > 0)
                        {
                            cell.Controls.Add(new LiteralControl(@"
            <div class=""Pages"">
            	<div class=""Paginator"">
            "));

                            bool prevEnabled = (CurrentPageIndex > 0);
                            if (prevEnabled)
                            {
                                LinkButton prev = new LinkButton();
                                prev.Text = "< 上一页";
                                prev.CausesValidation = false;
                                prev.CssClass = "Prev";
                                prev.Click += new EventHandler(prev_Click);
                                cell.Controls.Add(prev);
                            }
                            else
                            {
                                cell.Controls.Add(new LiteralControl(@"<span class=""AtStart"">&lt; 上一页</span>"));
                            }

                            int current = CurrentPageIndex + 1;
                            if (TotalPages <= 14)
                            {
                                addLinkButton(cell, 1, current, TotalPages);
                            }
                            else
                            {
                                if (current <= 8)
                                {
                                    addLinkButton(cell, 1, current, 11);
                                    cell.Controls.Add(new LiteralControl(string.Format("<span class=\"break\">...</span>")));
                                    int start = TotalPages - 1;
                                    addLinkButton(cell, start, current, 2);
                                }
                                else if (current > TotalPages - 7)
                                {
                                    addLinkButton(cell, 1, current, 2);
                                    cell.Controls.Add(new LiteralControl(string.Format("<span class=\"break\">...</span>")));
                                    int start = TotalPages - 9;
                                    addLinkButton(cell, start, current, 10);
                                }
                                else
                                {
                                    addLinkButton(cell, 1, current, 2);
                                    cell.Controls.Add(new LiteralControl(string.Format("<span class=\"break\">...</span>")));
                                    int start = current - 3;
                                    addLinkButton(cell, start, current, 7);
                                    cell.Controls.Add(new LiteralControl(string.Format("<span class=\"break\">...</span>")));
                                    start = TotalPages - 1;
                                    addLinkButton(cell, start, current, 2);
                                }
                            }

                            bool nextEnabled = (CurrentPageIndex + 1 < TotalPages);
                            if (nextEnabled)
                            {
                                LinkButton next = new LinkButton();
                                next.Text = "下一页 >";
                                next.CausesValidation = false;
                                next.Click += new EventHandler(next_Click);
                                next.CssClass = "Next";
                                cell.Controls.Add(next);
                            }
                            else
                            {
                                cell.Controls.Add(new LiteralControl(@"<span class=""AtEnd"">下一页 &gt;</span>"));
                            }


                            cell.Controls.Add(new LiteralControl(string.Format(@"<div class=""Results"">({0} {1})</div>", TotalCount, this.Unit)));

                            cell.Controls.Add(new LiteralControl(@"
            </div>
            	</div>
            "));
                        }

                        StringBuilder builder = new StringBuilder();
                        builder.AppendFormat(@"
            <div class=""Pages"">
            	<div class=""Paginator"">
                    <span class=""AtStart"">&lt; Prev</span>
            		<a href=""/photos/chionwolf/page5/"" class=""Prev"">&lt; Prev</a>
            			<a href=""/photos/chionwolf/"">1</a>
            			<a href=""/photos/chionwolf/page2/"">2</a>
            			<a href=""/photos/chionwolf/page3/"">3</a>
                        <a href=""/photos/chionwolf/page4/"">4</a>
                        <a href=""/photos/chionwolf/page5/"">5</a>
                        <span class=""this-page"">6</span>
                        <a href=""/photos/chionwolf/page7/"">7</a>
                        <a href=""/photos/chionwolf/page8/"">8</a>
                        <a href=""/photos/chionwolf/page9/"">9</a>
                        <span class=""break"">...</span>
                        <a href=""/photos/chionwolf/page211/"">211</a>
                        <a href=""/photos/chionwolf/page212/"">212</a>
                    <a href=""/photos/chionwolf/page7/"" class=""Next"">Next &gt;</a>
                    <span class=""AtEnd"">Next &gt;</span>
                </div>
                <div class=""Results"">(3,821 photos)</div>
            </div>	
            ");
        }

        #region 方法 跳转到前一页
        // ***********************************************************************
        /**/
        /// <summary>
        /// 方法 跳转到前一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prev_Click(object sender, EventArgs e)
        {
            GoToPage(CurrentPageIndex - 1);
        }
        // ***********************************************************************
        #endregion

        #region 方法 跳转到后一页
        // ***********************************************************************
        /**/
        /// <summary>
        /// 方法 跳转到后一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void next_Click(object sender, EventArgs e)
        {
            GoToPage(CurrentPageIndex + 1);
        }
        // ***********************************************************************
        #endregion
        private int _GotoPage;
        /// <summary>
        /// 跳转的页面
        /// </summary>
        protected int GotoPage
        {
            get { return this._GotoPage; }
            set { this._GotoPage = value; }
        }

        public void GoToPage(int pageIndex)
        {
            this.GotoPage = pageIndex + 1;
            // 准备事件数据
            PageChangedEventArgs e = new PageChangedEventArgs();
            e.OldPageIndex = CurrentPageIndex;
            e.NewPageIndex = pageIndex;

            // 更新当前页面索引
            CurrentPageIndex = pageIndex;

            // 触发页面改变事件
            OnPageIndexChanged(e);

            // 绑定新的数据
            DataBind();
        }
        
        private int addLinkButton(TableCell cell, int start, int current, int total)
                {
                    for (int count = 0; count < total; start++)
                    {
                        if (start != current)
                        {
                            LinkButton linkButton = new LinkButton();
                            linkButton.Text = Convert.ToString(start);
                            linkButton.CausesValidation = false;
                            linkButton.CommandArgument = Convert.ToString(start - 1);
                            linkButton.Click += new EventHandler(Numeric_Click);
                            linkButton.Enabled = true;
                            cell.Controls.Add(linkButton);
                        }
                        else
                        {
                            cell.Controls.Add(new LiteralControl(string.Format(@"
        <span>{0}</span>
        ", start)));
                        }
                        count++;
                    }
                    return start;
                }

        /// <summary>
        /// Generates the HTML markup to describe the current page (0-based)
        /// </summary>
        private void BuildCurrentPage(TableCell cell)
        {
            Literal text = new Literal();
            text.ID = "Text";
            text.Text = this.CurrentPageText;
            text.Text += string.Format(@"
<script>
function redirect(select){{
    var pageUrl = '{0}';
    var index = parseInt(select.options[select.selectedIndex].value) + 1;
    var isParam = {1};

    if (isParam)
    {{
        pageUrl = pageUrl.replace('.aspx', '-p' + index + '.aspx');
    }}
    else
    {{
        pageUrl = pageUrl.replace('-p' + {2} + '.aspx', '-p' + index + '.aspx');
    }}

    location.href = pageUrl;
}}
</script>
", HttpContext.Current.Request.RawUrl, string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[SqlPager.PARM_PAGE]).ToString().ToLower(), HttpContext.Current.Request.QueryString[SqlPager.PARM_PAGE]);
            cell.Controls.Add(text);
            // Render a drop-down list  
            DropDownList pageList = new DropDownList();
            pageList.ID = "PageList";
            pageList.Attributes.Add("onchange", "redirect(this);");
            pageList.AutoPostBack = false;
            //pageList.SelectedIndexChanged += new EventHandler(PageList_Click);
            pageList.Font.Name = Font.Name;
            pageList.Font.Size = Font.Size;
            pageList.ForeColor = ForeColor;

            // Embellish the list when there are no pages to list 
            if (TotalPages <= 0 || CurrentPageIndex == -1)
            {
                pageList.Items.Add("1 / 1");
                pageList.Enabled = false;
                pageList.SelectedIndex = 0;
            }
            else // Populate the list
            {
                for (int i = 1; i <= TotalPages; i++)
                {
                    ListItem item = new ListItem(string.Format("{0} / {1}", i, TotalPages), (i - 1).ToString());
                    pageList.Items.Add(item);
                }
                pageList.SelectedIndex = CurrentPageIndex;
            }
            cell.Controls.Add(pageList);
        }

        /// <summary>
        /// Ensures the CurrentPageIndex is either valid [0,TotalPages)
        /// </summary>
        private void ValidatePageIndex()
        {
            if (CurrentPageIndex < 0)
            {
                CurrentPageIndex = 0;
            }
            else if (CurrentPageIndex > TotalPages - 1)
            {
                CurrentPageIndex = TotalPages - 1;
            }
            return;
        }

        /// <summary>
        /// Runs the query for all data to be paged and caches the resulting data
        /// </summary>
        private void FetchAllData()
        {
            // Looks for data in the ASP.NET Cache
            DataTable data;
            data = (DataTable)Page.Cache[CacheKeyName];
            if (data == null)
            {
                // Fix SelectCommand with order-by info
                AdjustSelectCommand(true);

                // If data expired or has never been fetched, go to the database
                SqlDataAdapter adapter = new SqlDataAdapter(SelectCommand, ConnectionString);
                //IDbDataAdapter adapter = SqlUtils.GetIDbDataAdapter(BaiRongDataProvider.ADOType, SelectCommand, ConnectionString);
                data = new DataTable();
                adapter.Fill(data);
                //SqlUtils.FillDataAdapterWithDataTable(BaiRongDataProvider.ADOType, adapter, data);
                Page.Cache.Insert(CacheKeyName, data, null,
                    DateTime.Now.AddSeconds(CacheDuration),
                    System.Web.Caching.Cache.NoSlidingExpiration);
            }

            // Configures the paged data source component
            if (_dataSource == null)
                _dataSource = new PagedDataSource();
            _dataSource.DataSource = data.DefaultView; // must be IEnumerable!
            _dataSource.AllowPaging = true;
            _dataSource.PageSize = ItemsPerPage;
            TotalPages = _dataSource.PageCount;

            // Ensures the page index is valid 
            ValidatePageIndex();
            if (CurrentPageIndex == -1)
            {
                _dataSource = null;
                return;
            }

            // Selects the page to view
            _dataSource.CurrentPageIndex = CurrentPageIndex;
        }

        /// <summary>
        /// Runs the query to get only the data that fit into the current page
        /// </summary>
        private void FetchPageData()
        {
            // Need a validated page index to fetch data.
            // Also need the virtual page count to validate the page index
            AdjustSelectCommand(false);
            VirtualRecordCount countInfo = CalculateVirtualRecordCount();
            TotalPages = countInfo.PageCount;
            TotalCount = countInfo.RecordCount;

            // Validate the page number (ensures CurrentPageIndex is valid or -1)
            ValidatePageIndex();
            if (CurrentPageIndex == -1)
                return;

            // Prepare and run the command
            SqlCommand cmd = PrepareCommand(countInfo);
            if (cmd == null)
                return;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //IDbDataAdapter adapter = SqlUtils.GetIDbDataAdapter(BaiRongDataProvider.ADOType);
            adapter.SelectCommand = cmd;
            DataTable data = new DataTable();
            adapter.Fill(data);
            //SqlUtils.FillDataAdapterWithDataTable(BaiRongDataProvider.ADOType, adapter, data);

            // Configures the paged data source component
            if (_dataSource == null)
                _dataSource = new PagedDataSource();
            _dataSource.AllowCustomPaging = true;
            _dataSource.AllowPaging = true;
            _dataSource.CurrentPageIndex = 0;
            _dataSource.PageSize = ItemsPerPage;
            _dataSource.VirtualCount = countInfo.RecordCount;
            _dataSource.DataSource = data.DefaultView;
        }

        /// <summary>
        /// Strips ORDER-BY clauses from SelectCommand and adds a new one based
        /// on SortKeyField
        /// </summary>
        private void AdjustSelectCommand(bool addCustomSortInfo)
        {
            // Truncate where ORDER BY is found
            string temp = SelectCommand.ToLower();
            int pos = temp.IndexOf("order by");
            if (pos > -1)
                SelectCommand = SelectCommand.Substring(0, pos);

            // Add new ORDER BY info if SortKeyField is specified
            if (SortField != "" && addCustomSortInfo)
                SelectCommand += " ORDER BY " + SortField;
        }

        /// <summary>
        /// Calculates record and page count for the specified query
        /// </summary>
        private VirtualRecordCount CalculateVirtualRecordCount()
        {
            VirtualRecordCount count = new VirtualRecordCount();

            // Calculate the virtual number of records from the query
            count.RecordCount = GetQueryVirtualCount();
            count.RecordsInLastPage = ItemsPerPage;

            // Calculate the correspondent number of pages
            int lastPage = count.RecordCount / ItemsPerPage;
            int remainder = count.RecordCount % ItemsPerPage;
            if (remainder > 0)
                lastPage++;
            count.PageCount = lastPage;

            // Calculate the number of items in the last page
            if (remainder > 0)
                count.RecordsInLastPage = remainder;
            return count;
        }

        /// <summary>
        /// Prepares and returns the command object for the reader-based query
        /// </summary>
        private SqlCommand PrepareCommand(VirtualRecordCount countInfo)
        {
            // No sort field specified: figure it out
            if (SortField == "")
            {
                // Get metadata for all columns and choose either the primary key
                // or the 
                string text = "SET FMTONLY ON;" + SelectCommand + ";SET FMTONLY OFF;";
                SqlDataAdapter adapter = new SqlDataAdapter(text, ConnectionString);
                //IDbDataAdapter adapter = SqlUtils.GetIDbDataAdapter(BaiRongDataProvider.ADOType, text, ConnectionString);
                DataTable t = new DataTable();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(t);
                //SqlUtils.FillDataAdapterWithDataTable(BaiRongDataProvider.ADOType, adapter, t);
                DataColumn col = null;
                if (t.PrimaryKey.Length > 0)
                    col = t.PrimaryKey[0];
                else
                    col = t.Columns[0];
                SortField = col.ColumnName;
            }

            // Determines how many records are to be retrieved.
            // The last page could require less than other pages
            int recsToRetrieve = ItemsPerPage;
            if (CurrentPageIndex == countInfo.PageCount - 1)
                recsToRetrieve = countInfo.RecordsInLastPage;

            //			string cmdText = String.Format(QueryPageCommandText, 
            //				recsToRetrieve,						// {0} --> page size
            //				ItemsPerPage*(CurrentPageIndex+1),	// {1} --> size * index
            //				SelectCommand,						// {2} --> base query
            //				SortField,							// {3} --> key field in the query
            //				"ASC",								// Default to ascending order
            //				"DESC");

            string cmdText = String.Format(QueryPageCommandText,
                recsToRetrieve,                        // {0} --> page size
                ItemsPerPage * (CurrentPageIndex + 1),    // {1} --> size * index
                SelectCommand,                        // {2} --> base query
                SortField,                            // {3} --> key field in the query
                SortMode,                           // {4} --> 排序模式
                AlterSortMode(SortMode));

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(cmdText, conn);
            //IDbConnection conn = SqlUtils.GetIDbConnection(BaiRongDataProvider.ADOType, ConnectionString);
            //IDbCommand cmd = SqlUtils.GetIDbCommand(BaiRongDataProvider.ADOType);
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            return cmd;
        }

        #region 方法 反转排序模式

        /// <summary>
        /// 方法 反转排序模式
        /// </summary>
        /// <param name="mode">排序模式</param>
        /// <returns>相反的排序模式</returns>
        private SortMode AlterSortMode(SortMode mode)
        {
            if (mode == SortMode.DESC)
            {
                mode = SortMode.ASC;
            }
            else
            {
                mode = SortMode.DESC;
            }
            return mode;

        }

        #endregion

        /// <summary>
        /// Run a query to get the record count
        /// </summary>
        private int GetQueryVirtualCount()
        {
            string cmdText = String.Format(QueryCountCommandText, SelectCommand);
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(cmdText, conn);
            //IDbConnection conn = SqlUtils.GetIDbConnection(BaiRongDataProvider.ADOType, ConnectionString);
            //IDbCommand cmd = SqlUtils.GetIDbCommand(BaiRongDataProvider.ADOType);
            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            cmd.Connection.Open();
            int recCount = (int)cmd.ExecuteScalar();
            cmd.Connection.Close();

            return recCount;
        }

        ///// <summary>
        ///// Event handler for any page selected from the drop-down page list 
        ///// </summary>
        private void Numeric_Click(object sender, EventArgs e)
        {
            LinkButton linkButton = (LinkButton)sender;
            int pageIndex = Convert.ToInt32(linkButton.CommandArgument);
            GoToPage(pageIndex);
        }

        #endregion
    }
}
