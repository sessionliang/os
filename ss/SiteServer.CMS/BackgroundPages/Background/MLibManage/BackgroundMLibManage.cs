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
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Controls;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundMLibManage : BackgroundBasePage
    {
        public DropDownList ddlPublishmentSystem;
        public DropDownList NodeIDDropDownList;
        public DropDownList State;
        public CheckBox IsDuplicate;
        public DropDownList SearchType;
        public TextBox Keyword;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;
        public TextBox MemberName;

        public Repeater rptContents;
        public SqlPagerByDataReader spContents;
        public Literal ltlColumnHeadRows;
        public Literal ltlCommandHeadRows;

        public Button AddContent;
        public Button AddToGroup;
        public Button Delete;
        public Button Translate;
        public Button SelectButton;
        public PlaceHolder CheckPlaceHolder;
        public Button Check;
        public Button Generate;

        int nodeID = 0;
        NodeInfo nodeInfo;
        private ETableStyle tableStyle;
        //private StringCollection attributesOfDisplay;
        //ArrayList relatedIdentities;
        //ArrayList tableStyleInfoArrayList;
        private int publishmentSystemID;
        private PublishmentSystemInfo publishmentSystemInfo;
        public ArrayList nodeList = new ArrayList();

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;


            if (ConfigManager.Additional.IsUseMLib == false)
            {
                PageUtils.RedirectToErrorPage("投稿中心尚未开启.请在投稿基本设置中启用投稿");
                return;
            }
            //if (ConfigManager.Additional.MLibPublishmentSystemIDs == "")
            //{
            //    PageUtils.RedirectToErrorPage("未加载到投稿范围，请检查是否设置投稿范围");
            //    return;
            //}


            #region 获取稿件发布都有权限的站点和栏目
            if (this.ddlPublishmentSystem.Items.Count == 0)
            {
                this.ddlPublishmentSystem.Items.Clear();
                foreach (PublishmentSystemInfo info in PublishmentSystemManager.GetPublishmentSystem(UserManager.CurrentNewGroupMLibAddUser))
                {
                    ListItem item = new ListItem(info.PublishmentSystemName, info.PublishmentSystemID.ToString());
                    this.ddlPublishmentSystem.Items.Add(item);
                }
            }
            #endregion


            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_MLibManage, "稿件管理", AppManager.User.Permission.Usercenter_MLibManageSetting);



                ETriStateUtils.AddListItems(this.State, "全部", "已审核", "待审核");

                //添加隐藏属性
                //this.SearchType.Items.Add(new ListItem("内容ID", ContentAttribute.ID));
                //this.SearchType.Items.Add(new ListItem("添加者", ContentAttribute.AddUserName));
                //this.SearchType.Items.Add(new ListItem("最后修改者", ContentAttribute.LastEditUserName));

                ddlPublishmentSystem_OnSelectedIndexChanged(sender, E);
            }
        }

        public void ContentBind()
        {
            this.publishmentSystemID = TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue);
            this.publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            this.nodeID = TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue);
            if (this.nodeID == 0)
            {
                this.nodeID = publishmentSystemID;
                if (nodeList.Count == 0)
                {
                    ArrayList mLibNodeInfoArrayList = PublishmentSystemManager.GetNode(UserManager.CurrentNewGroupMLibAddUser, this.publishmentSystemInfo.PublishmentSystemID);
                    foreach (NodeInfo info in mLibNodeInfoArrayList)
                    {
                        nodeList.Add(info.NodeID);
                    }
                }
            }

            this.nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            this.tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
            string tableName = publishmentSystemInfo.AuxiliaryTableForContent; //NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
            //this.attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(NodeManager.GetContentAttributesOfDisplay(publishmentSystemID, nodeID));
            //this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemID, this.nodeID);
            //this.tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, this.relatedIdentities);

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            ETriState stateType = ETriStateUtils.GetEnumType(this.State.SelectedValue);
             
            if (string.IsNullOrEmpty(this.DateFrom.Text) && string.IsNullOrEmpty(this.DateTo.Text))
            {
                this.DateFrom.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                this.DateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            this.spContents.SelectCommand = DataProvider.ContentDAO.GetSelectCommend(tableStyle, tableName, publishmentSystemID, this.nodeID, PermissionsManager.Current.IsSystemAdministrator, this.nodeList, "Title", this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text, true, stateType, !this.IsDuplicate.Checked, false, AdminUtility.IsViewContentOnlySelf(publishmentSystemID, nodeID), this.MemberName.Text, true);

            this.spContents.ItemsPerPage = publishmentSystemInfo.Additional.PageSize;
            this.spContents.SortField = ContentAttribute.ID;
            this.spContents.SortMode = SortMode.DESC;
            this.spContents.OrderByString = ETaxisTypeUtils.GetOrderByString(this.tableStyle, ETaxisType.OrderByIDDesc);
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            this.spContents.DataBind();


            //if (this.tableStyleInfoArrayList != null)
            //{
            //    foreach (TableStyleInfo styleInfo in this.tableStyleInfoArrayList)
            //    { 
            //        if (styleInfo.IsVisible)
            //        {
            //            ListItem listitem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);
            //            this.SearchType.Items.Add(listitem);
            //        }
            //    }
            //}


            string showPopWinString = Modal.AddToGroup.GetOpenWindowStringToContentForMultiChannels(this.publishmentSystemID);
            this.AddToGroup.Attributes.Add("onclick", showPopWinString);

            showPopWinString = Modal.SelectColumns.GetOpenWindowStringToContent(this.publishmentSystemID, this.nodeID, true);
            this.SelectButton.Attributes.Add("onclick", showPopWinString);

            showPopWinString = Modal.ProgressBar.GetOpenWindowStringWithCreateContentsByMlib(this.publishmentSystemID);
            this.Generate.Attributes.Add("onclick", showPopWinString);

            if (AdminUtility.HasChannelPermissions(publishmentSystemID, publishmentSystemID, AppManager.CMS.Permission.Channel.ContentCheck))
            {
                showPopWinString = Modal.ContentCheck.GetOpenWindowStringForMultiChannels(publishmentSystemID, this.PageUrl);
                this.Check.Attributes.Add("onclick", showPopWinString);
            }
            else
            {
                this.CheckPlaceHolder.Visible = false;
            }

            //this.ltlColumnHeadRows.Text = ContentUtility.GetColumnHeadRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.tableStyle, publishmentSystemInfo);
            //this.ltlCommandHeadRows.Text = ContentUtility.GetCommandHeadRowsHtml(this.tableStyle, publishmentSystemInfo, this.nodeInfo);



            if (!base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.ContentAdd)) this.AddContent.Visible = false;
            if (!base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.ContentTranslate))
            {
                this.Translate.Visible = false;
            }
            else
            {
                this.Translate.Attributes.Add("onclick", BackgroundContentTranslate.GetRedirectClickStringForMultiChannels(this.publishmentSystemID, this.PageUrl));
            }

            if (!base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.ContentDelete))
            {
                this.Delete.Visible = false;
            }
            else
            {
                this.Delete.Attributes.Add("onclick", BackgroundContentDelete.GetRedirectClickStringForMultiChannels(this
                    .publishmentSystemID, false, this.PageUrl));
            }

        }

        private readonly Hashtable valueHashtable = new Hashtable();

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlItemTitle = e.Item.FindControl("ltlItemTitle") as Literal;
                Literal ltlChannel = e.Item.FindControl("ltlChannel") as Literal;
                Literal ltlColumnItemRows = e.Item.FindControl("ltlColumnItemRows") as Literal;
                Literal ltlItemStatus = e.Item.FindControl("ltlItemStatus") as Literal;
                Literal ltlItemEditUrl = e.Item.FindControl("ltlItemEditUrl") as Literal;
                Literal ltlCommandItemRows = e.Item.FindControl("ltlCommandItemRows") as Literal;
                Literal ltlSelect = e.Item.FindControl("ltlSelect") as Literal;
                Literal ltlAddTime = e.Item.FindControl("ltlAddTime") as Literal;
                Literal ltlMemberName = e.Item.FindControl("ltlMemberName") as Literal;

                ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(this.publishmentSystemID, contentInfo.NodeID);

                ltlItemTitle.Text = WebUtils.GetContentTitle(this.publishmentSystemInfo, contentInfo, this.PageUrl);
                string nodeName = valueHashtable[contentInfo.NodeID] as string;
                if (nodeName == null)
                {
                    nodeName = NodeManager.GetNodeNameNavigation(this.publishmentSystemID, contentInfo.NodeID);
                    valueHashtable[contentInfo.NodeID] = nodeName;
                }
                ltlChannel.Text = nodeName;

                string showPopWinString = Modal.CheckState.GetOpenWindowString(contentInfo.PublishmentSystemID, contentInfo, this.PageUrl);
                ltlItemStatus.Text = string.Format(@"<a href=""javascript:;"" title=""设置内容状态"" onclick=""{0}"">{1}</a>", showPopWinString, LevelManager.GetCheckState(this.publishmentSystemInfo, contentInfo.IsChecked, contentInfo.CheckedLevel));

                if (base.HasChannelPermissions(contentInfo.NodeID, AppManager.CMS.Permission.Channel.ContentEdit) || AdminManager.Current.UserName == contentInfo.AddUserName)
                {
                    ltlItemEditUrl.Text = string.Format("<a href=\"{0}\">编辑</a>", WebUtils.GetContentAddEditUrl(this.publishmentSystemID, nodeInfo, contentInfo.ID, this.PageUrl));
                }

                //ltlColumnItemRows.Text = ContentUtility.GetColumnItemRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.valueHashtable, this.tableStyle, this.publishmentSystemInfo, contentInfo);

                //ltlCommandItemRows.Text = ContentUtility.GetCommandItemRowsHtml(this.tableStyle, this.publishmentSystemInfo, nodeInfo, contentInfo, this.PageUrl);

                ltlSelect.Text = string.Format(@"<input type=""checkbox"" name=""IDsCollection"" value=""{0}_{1}"" />", contentInfo.NodeID, contentInfo.ID);

                ltlAddTime.Text = contentInfo.AddDate.ToString("yyyy-MM-dd HH:mm:ss");
                ltlMemberName.Text = contentInfo.MemberName;
            }
        }

        public void AddContent_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(WebUtils.GetContentAddAddUrl(base.PublishmentSystemID, this.nodeInfo, this.PageUrl));
        }
        public void ddlPublishmentSystem_OnSelectedIndexChanged(object sender, EventArgs E)
        {
            int publishmentSystemId = TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue);
            #region 获取稿件发布都有权限的站点和栏目
            if (this.ddlPublishmentSystem.Items.Count > 0)
            {
                this.NodeIDDropDownList.Items.Clear();
                this.nodeList.Clear();
                ListItem itemd = new ListItem("全部", "0");
                this.NodeIDDropDownList.Items.Add(itemd);
                PublishmentSystemInfo pinfo = PublishmentSystemManager.GetPublishmentSystemInfo(TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue));
                ArrayList mLibNodeInfoArrayList = PublishmentSystemManager.GetNode(ConfigManager.Additional.UnifiedMLibAddUser, pinfo.PublishmentSystemID);
                foreach (NodeInfo nodeInfo in mLibNodeInfoArrayList)
                {
                    ListItem item = new ListItem(nodeInfo.NodeName, nodeInfo.NodeID.ToString());
                    this.NodeIDDropDownList.Items.Add(item);
                    this.nodeList.Add(nodeInfo.NodeID);
                }
                nodeID = TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue);
            }
            #endregion
            ContentBind();
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            // PageUtils.Redirect(this.PageUrl);
            ContentBind();
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = PageUtils.GetPlatformUrl(string.Format("background_mlibManage.aspx?PublishmentSystemID={0}&NodeID={1}&State={2}&IsDuplicate={3}&SearchType={4}&Keyword={5}&DateFrom={6}&DateTo={7}", this.ddlPublishmentSystem.SelectedValue, this.NodeIDDropDownList.SelectedValue, this.State.SelectedValue, this.IsDuplicate.Checked, this.SearchType.SelectedValue, this.Keyword.Text, this.DateFrom.Text, this.DateTo.Text));
                }
                return this._pageUrl;
            }
        }
    }
}
