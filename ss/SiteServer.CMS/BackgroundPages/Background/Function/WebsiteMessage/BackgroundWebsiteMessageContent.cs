using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;
using System.Text;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundWebsiteMessageContent : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlColumnHeadRows;
        public Literal ltlHeadRowReply;

        public Button btnAdd;
        public Button btnCheck;
        public Button btnTranslate;
        public Button btnDelete;
        public Button btnExportExcel;
        public Button btnTaxis;
        public Button btnSelectColumns;

        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;
        public DropDownList SearchType;
        public TextBox Keyword;

        private ArrayList relatedIdentities;
        private WebsiteMessageInfo websiteMessageInfo = null;
        private ArrayList tableStyleInfoArrayList;
        private int classifyID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "WebsiteMessageName");

            string theWebsiteMessageName = base.GetQueryString("WebsiteMessageName");
            this.classifyID = TranslateUtils.ToInt(base.GetQueryStringNoSqlAndXss("itemID"));//分类
            WebsiteMessageClassifyInfo classifyInfo = DataProvider.WebsiteMessageClassifyDAO.GetWebsiteMessageClassifyInfo(this.classifyID);

            this.websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(theWebsiteMessageName, base.PublishmentSystemID);
            if (this.websiteMessageInfo == null) return;

            this.relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID);

            this.tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, this.relatedIdentities);

            bool isAnythingVisible = false;
            foreach (TableStyleInfo styleInfo in this.tableStyleInfoArrayList)
            {
                if (styleInfo.IsVisibleInList)
                {
                    isAnythingVisible = true;
                    break;
                }
            }
            if (!isAnythingVisible && this.tableStyleInfoArrayList != null && this.tableStyleInfoArrayList.Count > 0)
            {
                TableStyleInfo tableStyleInfo = (TableStyleInfo)this.tableStyleInfoArrayList[0];
                tableStyleInfo.IsVisibleInList = true;
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;


            string where = GetSelectWhere();

            if (classifyInfo.ParentID == 0)//全部
                this.spContents.SelectCommand = DataProvider.WebsiteMessageContentDAO.GetSelectStringOfContentID(this.websiteMessageInfo.WebsiteMessageID, 0, base.GetQueryString("SearchType"), base.GetQueryString("Keyword"), where);
            else
                this.spContents.SelectCommand = DataProvider.WebsiteMessageContentDAO.GetSelectStringOfContentID(this.websiteMessageInfo.WebsiteMessageID, this.classifyID, base.GetQueryString("SearchType"), base.GetQueryString("Keyword"), where);
            this.spContents.SortField = DataProvider.WebsiteMessageContentDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                #region 默认创建一个网站留言，网站留言分类
                DataProvider.WebsiteMessageClassifyDAO.SetDefaultWebsiteMessageClassifyInfo(base.PublishmentSystemID);
                DataProvider.WebsiteMessageDAO.SetDefaultWebsiteMessageInfo(base.PublishmentSystemID);
                #endregion

                #region 控件赋值
                this.DateFrom.Text = base.GetQueryString("DateFrom");
                this.DateTo.Text = base.GetQueryString("DateTo");
                this.Keyword.Text = base.GetQueryString("Keyword");
                if (this.tableStyleInfoArrayList != null)
                {
                    foreach (TableStyleInfo styleInfo in this.tableStyleInfoArrayList)
                    {
                        if (styleInfo.IsVisible)
                        {
                            ListItem listitem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);
                            this.SearchType.Items.Add(listitem);
                        }
                    }
                }
                //默认选择姓名
                this.SearchType.SelectedValue = WebsiteMessageContentAttribute.Name;
                if (!string.IsNullOrEmpty(base.GetQueryString("SearchType")))
                {
                    ControlUtils.SelectListItems(this.SearchType, base.GetQueryString("SearchType"));
                }
                #endregion

                this.spContents.DataBind();

                base.BreadCrumbWithItemTitle(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_WebsiteMessage, "网站留言内容管理", string.Format("{0}({1})", classifyInfo.ItemName, classifyInfo.ContentNum), AppManager.CMS.Permission.WebSite.WebsiteMessageContentView);

                string showPopWinString = string.Empty;

                if (base.HasWebsitePermissions(AppManager.CMS.Permission.WebSite.WebsiteMessageContentEdit + "_" + this.websiteMessageInfo.WebsiteMessageName))
                {
                    showPopWinString = Modal.WebsiteMessageContentAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID, this.classifyID, this.PageUrl);
                    this.btnAdd.Attributes.Add("onclick", showPopWinString);

                    this.btnDelete.Attributes.Add("onclick", "return confirm(\"此操作将删除所选内容，确定吗？\");");

                    this.btnCheck.Attributes.Add("onclick", "return confirm(\"此操作将把所选内容设为审核通过，确定吗？\");");

                    showPopWinString = Modal.WebsiteMessageContentTaxis.GetOpenWindowString(base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID, this.classifyID, this.PageUrl);
                    this.btnTaxis.Attributes.Add("onclick", showPopWinString);

                    showPopWinString = Modal.SelectColumns.GetOpenWindowStringToWebsiteMessageContent(base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID, true);
                    this.btnSelectColumns.Attributes.Add("onclick", showPopWinString);
                }
                else
                {
                    this.btnAdd.Visible = this.btnDelete.Visible = this.btnCheck.Visible = this.btnTaxis.Visible = this.btnSelectColumns.Visible = this.btnSelectColumns.Visible = this.btnExportExcel.Visible = false;
                }

                showPopWinString = PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToWebsiteMessageContent(base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID, this.classifyID);
                this.btnExportExcel.Attributes.Add("onclick", showPopWinString);

                //转移
                showPopWinString = Modal.WebsiteMessageContentTranslate.GetRedirectString(base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID, 0, this.PageUrl);
                this.btnTranslate.Attributes.Add("onclick", showPopWinString);

                if (this.tableStyleInfoArrayList != null)
                {
                    foreach (TableStyleInfo styleInfo in this.tableStyleInfoArrayList)
                    {
                        if (styleInfo.IsVisibleInList)
                        {
                            this.ltlColumnHeadRows.Text += string.Format(@"<td class=""center"">{0}</td>", styleInfo.DisplayName);
                        }
                    }
                }

                if (this.websiteMessageInfo.IsReply)
                {
                    if (base.HasWebsitePermissions(AppManager.CMS.Permission.WebSite.WebsiteMessageContentEdit + "_" + this.websiteMessageInfo.WebsiteMessageName))
                    {
                        this.ltlHeadRowReply.Text = @"
<td class=""center"" style=""width:60px;"">是否回复</td>
<td class=""center"" style=""width:60px;"">&nbsp;</td>
";
                    }
                }
            }
        }

        private string GetSelectWhere()
        {
            StringBuilder builder = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(base.GetQueryString("DateFrom")))
                builder.AppendFormat(" AND AddDate >= '{0}' ", base.GetQueryString("DateFrom"));
            if (!string.IsNullOrEmpty(base.GetQueryString("DateTo")))
                builder.AppendFormat(" AND AddDate <= '{0}' ", base.GetQueryString("DateTo"));
            return builder.ToString();
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int contentID = TranslateUtils.EvalInt(e.Item.DataItem, "ID");
                WebsiteMessageContentInfo contentInfo = DataProvider.WebsiteMessageContentDAO.GetContentInfo(contentID);
                Literal columnItemRows = (Literal)e.Item.FindControl("ColumnItemRows");
                Literal itemEidtRow = (Literal)e.Item.FindControl("ItemEidtRow");
                Literal itemViewRow = (Literal)e.Item.FindControl("ItemViewRow");
                Literal itemRowReply = e.Item.FindControl("ItemRowReply") as Literal;
                Literal itemDateTime = e.Item.FindControl("ItemDateTime") as Literal;

                if (this.tableStyleInfoArrayList != null)
                {
                    foreach (TableStyleInfo styleInfo in this.tableStyleInfoArrayList)
                    {
                        if (styleInfo.IsVisibleInList)
                        {
                            string value = contentInfo.Attributes.Get(styleInfo.AttributeName);

                            if (!string.IsNullOrEmpty(value))
                            {
                                value = InputTypeParser.GetContentByTableStyle(value, base.PublishmentSystemInfo, ETableStyle.WebsiteMessageContent, styleInfo);
                            }

                            if (contentInfo.IsChecked == false && string.IsNullOrEmpty(columnItemRows.Text))
                            {
                                columnItemRows.Text += string.Format(@"<td>{0}<span style=""color:red"">[未审核]</span></td>", value);
                            }
                            else
                            {
                                columnItemRows.Text += string.Format(@"<td>{0}</td>", value);
                            }
                        }
                    }
                }

                itemViewRow.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">查看</a>", Modal.WebsiteMessageContentView.GetOpenWindowString(base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID, contentInfo.ID));

                if (base.HasWebsitePermissions(AppManager.CMS.Permission.WebSite.WebsiteMessageContentEdit + "_" + this.websiteMessageInfo.WebsiteMessageName))
                {
                    itemEidtRow.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">修改</a>", Modal.WebsiteMessageContentAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID, contentInfo.ID, this.classifyID, this.PageUrl));

                    if (this.websiteMessageInfo.IsReply)
                    {
                        string text = string.IsNullOrEmpty(contentInfo.Reply) ? "提交回复" : "修改回复";
                        itemRowReply.Text = string.Format(@"
<td class=""center"">{0}</a></td>
<td class=""center""><a href=""javascript:;"" onclick=""{1}"">{2}</a></td>", StringUtils.GetTrueImageHtml(!string.IsNullOrEmpty(contentInfo.Reply)), Modal.WebsiteMessageContentReply.GetOpenWindowString(base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID, contentInfo.ID), text);
                    }
                }

                itemDateTime.Text = DateUtils.GetDateString(contentInfo.AddDate);
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
                        DataProvider.WebsiteMessageContentDAO.Delete(this.websiteMessageInfo.WebsiteMessageID, arraylist);
                        StringUtility.AddLog(base.PublishmentSystemID, "删除网站留言内容", string.Format("网站留言:{0}", this.websiteMessageInfo.WebsiteMessageName));
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

        public void btnCheck_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (Request.Form["ContentIDCollection"] != null)
                {
                    ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.Form["ContentIDCollection"]);
                    try
                    {
                        DataProvider.WebsiteMessageContentDAO.Check(this.websiteMessageInfo.WebsiteMessageID, arraylist);
                        StringUtility.AddLog(base.PublishmentSystemID, "审核网站留言内容", string.Format("网站留言:{0}", this.websiteMessageInfo.WebsiteMessageName));
                        base.SuccessMessage("审核成功！");
                        PageUtils.Redirect(this.PageUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "审核失败！");
                    }
                }
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
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_websiteMessageContent.aspx?PublishmentSystemID={0}&WebsiteMessageName={1}&ItemID={2}&DateFrom={3}&DateTo={4}&Keyword={5}&SearchType={6}", base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageName, this.classifyID, this.DateFrom.Text, this.DateTo.Text, this.Keyword.Text, this.SearchType.SelectedValue));
                }
                return _pageUrl;
            }
        }
    }
}
