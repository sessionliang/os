using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Controls;



namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundContentArchive : BackgroundBasePage
	{
        public DropDownList NodeIDDropDownList;
        public DropDownList PageNum;
        public DropDownList SearchType;
        public TextBox Keyword;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;

		public Repeater rptContents;
		public SqlPager spContents;

        public Button Restore;
        public Button RestoreAll;
		public Button Delete;
        public Button DeleteAll;

		int nodeID = 0;
        ArrayList relatedIdentities;
        ArrayList tableStyleInfoArrayList;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            if (!string.IsNullOrEmpty(base.GetQueryString("NodeID")))
            {
                this.nodeID = base.GetIntQueryString("NodeID");
            }
            else
            {
                this.nodeID = base.PublishmentSystemID;
            }
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            ArchiveManager.CreateArchiveTableIfNotExists(base.PublishmentSystemInfo, tableName);
            string tableNameOfArchive = TableManager.GetTableNameOfArchive(tableName);

            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeID);
            this.tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, this.relatedIdentities);

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            if (string.IsNullOrEmpty(base.GetQueryString("NodeID")))
            {
                if (TranslateUtils.ToInt(this.PageNum.SelectedValue) == 0)
                {
                    this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
                }
                else
                {
                    this.spContents.ItemsPerPage = TranslateUtils.ToInt(this.PageNum.SelectedValue);
                }
                this.spContents.SelectCommand = DataProvider.ContentDAO.GetSelectCommend(tableStyle, tableNameOfArchive, base.PublishmentSystemID, this.nodeID, PermissionsManager.Current.IsSystemAdministrator, ProductPermissionsManager.Current.OwningNodeIDArrayList, this.SearchType.SelectedValue, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text, true, ETriState.All);
            }
            else
            {
                if (TranslateUtils.ToInt(base.GetQueryString("PageNum")) == 0)
                {
                    this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
                }
                else
                {
                    this.spContents.ItemsPerPage = TranslateUtils.ToInt(base.GetQueryString("PageNum"));
                }
                this.spContents.SelectCommand = DataProvider.ContentDAO.GetSelectCommend(tableStyle, tableNameOfArchive, base.PublishmentSystemID, this.nodeID, PermissionsManager.Current.IsSystemAdministrator, ProductPermissionsManager.Current.OwningNodeIDArrayList, base.GetQueryString("SearchType"), base.GetQueryString("Keyword"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"), true, ETriState.All);
            }
            this.spContents.SortField = ContentAttribute.LastEditDate;
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, "内容归档管理", AppManager.CMS.Permission.WebSite.Archive);

                if (!string.IsNullOrEmpty(base.GetQueryString("IsDelete")))
                {
                    ArrayList contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ContentIDCollection"));
                    BaiRongDataProvider.ContentDAO.DeleteContentsArchive(base.PublishmentSystemID, tableNameOfArchive, contentIDArrayList);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除内容归档");
                    base.SuccessMessage("成功删除内容归档!");
                    base.AddWaitAndRedirectScript(this.PageUrl);
                    return;
                }
                else if (!string.IsNullOrEmpty(base.GetQueryString("IsDeleteAll")))
                {
                    ArrayList contentIDArrayList = BaiRongDataProvider.ContentDAO.GetContentIDArrayListByPublishmentSystemID(tableNameOfArchive, base.PublishmentSystemID);
                    BaiRongDataProvider.ContentDAO.DeleteContentsArchive(base.PublishmentSystemID, tableNameOfArchive, contentIDArrayList);
                    base.SuccessMessage("成功清空内容归档!");
                    StringUtility.AddLog(base.PublishmentSystemID, "清空内容归档");
                    base.AddWaitAndRedirectScript(this.PageUrl);
                    return;
                }
                else if (!string.IsNullOrEmpty(base.GetQueryString("IsRestore")))
                {
                    ArrayList contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ContentIDCollection"));
                    foreach (int contentID in contentIDArrayList)
                    {
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableNameOfArchive, contentID);
                        DataProvider.ContentDAO.Insert(tableName, base.PublishmentSystemInfo, contentInfo);
                    }
                    BaiRongDataProvider.ContentDAO.DeleteContentsArchive(base.PublishmentSystemID, tableNameOfArchive, contentIDArrayList);
                    base.SuccessMessage("成功取消归档内容!");
                    StringUtility.AddLog(base.PublishmentSystemID, "取消内容归档");
                    base.AddWaitAndRedirectScript(this.PageUrl);
                    return;
                }
                else if (!string.IsNullOrEmpty(base.GetQueryString("IsRestoreAll")))
                {
                    ArrayList contentIDArrayList = BaiRongDataProvider.ContentDAO.GetContentIDArrayListByPublishmentSystemID(tableNameOfArchive, base.PublishmentSystemID);
                    foreach (int contentID in contentIDArrayList)
                    {
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableNameOfArchive, contentID);

                        DataProvider.ContentDAO.Insert(tableName, base.PublishmentSystemInfo, contentInfo);
                    }
                    BaiRongDataProvider.ContentDAO.DeleteContentsArchive(base.PublishmentSystemID, tableNameOfArchive, contentIDArrayList);

                    base.SuccessMessage("成功取消归档所有内容!");
                    StringUtility.AddLog(base.PublishmentSystemID, "取消归档所有内容");
                    base.AddWaitAndRedirectScript(this.PageUrl);
                    return;
                }
                NodeManager.AddListItems(this.NodeIDDropDownList.Items, base.PublishmentSystemInfo, true, false);

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
                //添加隐藏属性
                this.SearchType.Items.Add(new ListItem("内容ID", ContentAttribute.ID));
                this.SearchType.Items.Add(new ListItem("添加者", ContentAttribute.AddUserName));
                this.SearchType.Items.Add(new ListItem("最后修改者", ContentAttribute.LastEditUserName));

                if (!string.IsNullOrEmpty(base.GetQueryString("NodeID")))
                {
                    if (base.PublishmentSystemID != this.nodeID)
                    {
                        ControlUtils.SelectListItems(this.NodeIDDropDownList, this.nodeID.ToString());
                    }
                    ControlUtils.SelectListItems(this.PageNum, base.GetQueryString("PageNum"));
                    ControlUtils.SelectListItems(this.SearchType, base.GetQueryString("SearchType"));
                    this.Keyword.Text = base.GetQueryString("Keyword");
                    this.DateFrom.Text = base.GetQueryString("DateFrom");
                    this.DateTo.Text = base.GetQueryString("DateTo");
                }

                this.spContents.DataBind();
            }

            if (!base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.ContentDelete))
            {
                this.Delete.Visible = false;
                this.DeleteAll.Visible = false;
            }
            else
            {
                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.AddQueryString(this.PageUrl, "IsDelete", "True"), "ContentIDCollection", "ContentIDCollection", "请选择需要删除的内容！", "确实要删除所选内容吗?"));
                this.DeleteAll.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.AddQueryString(this.PageUrl, "IsDeleteAll", "True"), "确实要清空内容归档吗?"));
            }
            this.Restore.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.AddQueryString(this.PageUrl, "IsRestore", "True"), "ContentIDCollection", "ContentIDCollection", "请选择需要取消归档的内容！"));
            this.RestoreAll.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.AddQueryString(this.PageUrl, "IsRestoreAll", "True"), "确实要取消归档所有内容吗?"));
        }

        private readonly Hashtable nodeNameNavigations = new Hashtable();
        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal itemTitle = (Literal)e.Item.FindControl("ItemTitle");
                Literal itemChannelName = (Literal)e.Item.FindControl("ItemChannelName");
                Literal itemArchiveDate = (Literal)e.Item.FindControl("ItemArchiveDate");

                BackgroundContentInfo contentInfo = new BackgroundContentInfo(e.Item.DataItem);

                itemTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.PageUrl);
                string nodeNameNavigation;
                if (!nodeNameNavigations.ContainsKey(contentInfo.NodeID))
                {
                    nodeNameNavigation = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID);
                    nodeNameNavigations.Add(contentInfo.NodeID, nodeNameNavigation);
                }
                else
                {
                    nodeNameNavigation = nodeNameNavigations[contentInfo.NodeID] as string;
                }
                itemChannelName.Text = nodeNameNavigation;
                itemArchiveDate.Text = DateUtils.GetDateAndTimeString(contentInfo.LastEditDate);
            }
        }

        //private string GetTitle(BackgroundContentInfo contentInfo)
        //{
        //    string url = string.Empty;

        //    string displayString = string.Empty;

        //    if (contentInfo.IsColor)
        //    {
        //        displayString = string.Format("<span style='color:#ff0000;text-decoration:none' title='醒目'>{0}</span>", contentInfo.Title);
        //    }
        //    else
        //    {
        //        displayString = contentInfo.Title;
        //    }

        //    if (contentInfo.IsChecked)
        //    {
        //        //url = string.Format("<a href='{0}' target='blank'>{1}</a>", PageUtility.GetContentUrl(base.PublishmentSystemInfo, contentInfo, base.PublishmentSystemInfo.VisualType), displayString);
        //        url = displayString;
        //    }
        //    else
        //    {
        //        url = displayString + "&nbsp;<span style='color:#ff0000;text-decoration:none'>[未审核]</span>";
        //    }

        //    string image = string.Empty;
        //    if (!string.IsNullOrEmpty(contentInfo.ImageUrl))
        //    {
        //        image += "&nbsp;<img src='../../sitefiles/bairong/icons/img.gif' alt='图片' align='absmiddle' border=0 />";
        //    }
        //    if (contentInfo.IsRecommend)
        //    {
        //        image += "&nbsp;<img src='../pic/icon/recommend.gif' alt='推荐' align='absmiddle' border=0 />";
        //    }
        //    if (contentInfo.IsHot)
        //    {
        //        image += "&nbsp;<img src='../pic/icon/hot.gif' alt='热点' align='absmiddle' border=0 />";
        //    }
        //    if (contentInfo.IsTop)
        //    {
        //        image += "&nbsp;<img src='../pic/icon/top.gif' alt='置顶' align='absmiddle' border=0 />";
        //    }
        //    if (!string.IsNullOrEmpty(contentInfo.FileUrl))
        //    {
        //        image += "&nbsp;<img src='../pic/icon/attachment.gif' alt='附件' align='absmiddle' border=0 />";
        //        if (base.PublishmentSystemInfo.Additional.IsCountDownload)
        //        {
        //            int count = CountManager.GetCount(AppManager.CMS.AppID, base.PublishmentSystemInfo.AuxiliaryTableForContent, contentInfo.ID.ToString(), ECountType.Download);
        //            image += string.Format("下载次数:<strong>{0}</strong>", count);
        //        }
        //    }
        //    return url + image;
        //}

        public void Search_OnClick(object sender, EventArgs E)
        {
            base.Response.Redirect(this.PageUrl, true);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = PageUtils.GetCMSUrl(string.Format("background_contentArchive.aspx?PublishmentSystemID={0}&NodeID={1}&PageNum={2}&SearchType={3}&Keyword={4}&DateFrom={5}&DateTo={6}", base.PublishmentSystemID, this.NodeIDDropDownList.SelectedValue, this.PageNum.SelectedValue, this.SearchType.SelectedValue, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text));
                }
                return this._pageUrl;
            }
        }
	}
}
