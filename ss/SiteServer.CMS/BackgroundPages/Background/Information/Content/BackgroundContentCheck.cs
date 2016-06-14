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
    public class BackgroundContentCheck : BackgroundBasePage
    {
        public RadioButtonList State;
        public PlaceHolder phContentModel;
        public DropDownList ContentModelID;

        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlColumnHeadRows;
        public Literal ltlCommandHeadRows;

        public Button Check;
        public Button Delete;

        private readonly Hashtable displayNameHashtable = new Hashtable();
        private StringCollection attributesOfDisplay;
        private ArrayList tableStyleInfoArrayList;
        private ArrayList relatedIdentities;
        private ETableStyle tableStyle;
        private NodeInfo nodeInfo;
        private string tableName;
        private bool isGovPublic;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            int nodeID = base.PublishmentSystemID;
            this.isGovPublic = TranslateUtils.ToBool(base.GetQueryString("IsGovPublic"));
            if (this.isGovPublic)
            {
                nodeID = base.PublishmentSystemInfo.Additional.GovPublicNodeID;
                if (nodeID == 0)
                {
                    nodeID = base.PublishmentSystemID;
                }
            }
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeInfo);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeInfo);
            this.tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.tableName, this.relatedIdentities);
            this.attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(NodeManager.GetContentAttributesOfDisplay(base.PublishmentSystemID, nodeID));

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "内容审核", string.Empty);

                int checkedLevel = 5;
                bool isChecked = true;
                foreach (int owningNodeID in ProductPermissionsManager.Current.OwningNodeIDArrayList)
                {
                    int checkedLevelByNodeID = 0;
                    bool isCheckedByNodeID = CheckManager.GetUserCheckLevel(base.PublishmentSystemInfo, owningNodeID, out checkedLevelByNodeID);
                    if (checkedLevel > checkedLevelByNodeID)
                    {
                        checkedLevel = checkedLevelByNodeID;
                    }
                    if (!isCheckedByNodeID)
                    {
                        isChecked = isCheckedByNodeID;
                    }
                }

                LevelManager.LoadContentLevelToList(this.State, base.PublishmentSystemInfo, base.PublishmentSystemID, isChecked, checkedLevel);

                if (this.isGovPublic)
                {
                    this.phContentModel.Visible = false;
                }
                else
                {
                    this.phContentModel.Visible = true;
                    ArrayList contentModelArrayList = ContentModelManager.GetContentModelArrayList(base.PublishmentSystemInfo);
                    foreach (ContentModelInfo modelInfo in contentModelArrayList)
                    {
                        this.ContentModelID.Items.Add(new ListItem(modelInfo.ModelName, modelInfo.ModelID));
                    }
                    ControlUtils.SelectListItems(this.ContentModelID, nodeInfo.ContentModelID);
                    //EContentModelTypeUtils.AddListItemsForContentCheck(this.ContentModelID);
                }

                if (base.GetQueryString("State") != null)
                {
                    ControlUtils.SelectListItems(this.State, base.GetQueryString("State"));
                    ControlUtils.SelectListItems(this.ContentModelID, base.GetQueryString("ModelID"));
                }

                this.spContents.ControlToPaginate = this.rptContents;
                this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
                this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;

                ArrayList checkLevelArrayList = new ArrayList();

                if (!string.IsNullOrEmpty(base.GetQueryString("State")))
                {
                    checkLevelArrayList.Add(base.GetQueryString("State"));
                }
                else
                {
                    checkLevelArrayList = LevelManager.LevelInt.GetCheckLevelArrayList(base.PublishmentSystemInfo, isChecked, checkedLevel);
                }
                string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.ContentModelID.SelectedValue);
                if (this.isGovPublic)
                {
                    tableName = base.PublishmentSystemInfo.AuxiliaryTableForGovPublic;
                }
                this.spContents.SelectCommand = BaiRongDataProvider.ContentDAO.GetSelectedCommendByCheck(tableName, base.PublishmentSystemID, PermissionsManager.Current.IsSystemAdministrator, ProductPermissionsManager.Current.OwningNodeIDArrayList, checkLevelArrayList);

                this.spContents.SortField = ContentAttribute.LastEditDate;
                this.spContents.SortMode = SortMode.DESC;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

                this.spContents.DataBind();

                string showPopWinString = Modal.ContentCheck.GetOpenWindowStringForMultiChannels(base.PublishmentSystemID, this.PageUrl);
                this.Check.Attributes.Add("onclick", showPopWinString);

                this.ltlColumnHeadRows.Text = ContentUtility.GetColumnHeadRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.tableStyle, base.PublishmentSystemInfo);
                this.ltlCommandHeadRows.Text = ContentUtility.GetCommandHeadRowsHtml(this.tableStyle, base.PublishmentSystemInfo, this.nodeInfo);
            }

            if (!base.HasChannelPermissions(base.PublishmentSystemID, AppManager.CMS.Permission.Channel.ContentDelete))
            {
                this.Delete.Visible = false;
            }
            else
            {
                this.Delete.Attributes.Add("onclick", BackgroundContentDelete.GetRedirectClickStringForMultiChannels(base.PublishmentSystemID, false, this.PageUrl));
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
                Literal ltlItemSelect = e.Item.FindControl("ltlItemSelect") as Literal;

                ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, contentInfo.NodeID);

                ltlItemTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.PageUrl);
                string nodeName = valueHashtable[contentInfo.NodeID] as string;
                if (nodeName == null)
                {
                    nodeName = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID);
                    valueHashtable[contentInfo.NodeID] = nodeName;
                }
                ltlChannel.Text = nodeName;

                string showPopWinString = Modal.CheckState.GetOpenWindowString(base.PublishmentSystemID, contentInfo, this.PageUrl);
                ltlItemStatus.Text = string.Format(@"<a href=""javascript:;"" title=""设置内容状态"" onclick=""{0}"">{1}</a>", showPopWinString, LevelManager.GetCheckState(base.PublishmentSystemInfo, contentInfo.IsChecked, contentInfo.CheckedLevel));

                if (base.HasChannelPermissions(contentInfo.NodeID, AppManager.CMS.Permission.Channel.ContentEdit) || AdminManager.Current.UserName == contentInfo.AddUserName)
                {
                    ltlItemEditUrl.Text = string.Format("<a href=\"{0}\">编辑</a>", WebUtils.GetContentAddEditUrl(base.PublishmentSystemID, nodeInfo, contentInfo.ID, this.PageUrl));
                }

                ltlItemSelect.Text = string.Format(@"<input type=""checkbox"" name=""IDsCollection"" value=""{0}_{1}"" />", contentInfo.NodeID, contentInfo.ID);

                ltlColumnItemRows.Text = ContentUtility.GetColumnItemRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.valueHashtable, this.tableStyle, base.PublishmentSystemInfo, contentInfo);

                ltlCommandItemRows.Text = ContentUtility.GetCommandItemRowsHtml(this.tableStyle, base.PublishmentSystemInfo, nodeInfo, contentInfo, this.PageUrl);
            }
        }

        public void State_SelectedIndexChanged(object sender, EventArgs E)
        {
            base.Response.Redirect(this.PageUrl, true);
        }

        public void ContentModelID_SelectedIndexChanged(object sender, EventArgs E)
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
                    this._pageUrl = PageUtils.GetCMSUrl(string.Format("background_contentCheck.aspx?PublishmentSystemID={0}&State={1}&ModelID={2}&IsGovPublic={3}", base.PublishmentSystemID, this.State.SelectedValue, this.ContentModelID.SelectedValue, this.isGovPublic));
                }
                return this._pageUrl;
            }
        }
    }
}
