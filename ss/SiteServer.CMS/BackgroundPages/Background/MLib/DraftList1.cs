using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class DraftList1 : MLibBackgroundBasePage
    {

        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlColumnHeadRows;
        public Literal ltlCommandHeadRows;

        public Literal ltlContentButtons;

        public Literal ltlContentCount1;
        public Literal ltlContentCount2;


        public DateTimeTextBox DateFrom;
        public DropDownList SearchType;
        public DropDownList ddlStatus;
        public TextBox Keyword;

        private NodeInfo nodeInfo;
        private ETableStyle tableStyle;
        private string tableName;
        private StringCollection attributesOfDisplay;
        private ArrayList relatedIdentities;
        private ArrayList tableStyleInfoArrayList;
        private readonly Hashtable valueHashtable = new Hashtable();
        private Hashtable gatherHashtable = new Hashtable();
        public string IsShowTab;
        public int nodeID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!this.HasCheckLevelPermissions("1"))
            {
                IsShowTab = " style=\"display:none;\"";
            }

            nodeID = base.PublishmentSystemID;
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeInfo);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeInfo);
            this.tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.tableName, this.relatedIdentities);

            VisualInfo.RemovePreviewContent(base.PublishmentSystemInfo, this.nodeInfo);

            //this.gatherHashtable = DataProvider.GatherContentDAO.GetGatherContentIDHashtable(base.PublishmentSystemID, nodeID);



            this.attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(NodeManager.GetContentAttributesOfDisplay(base.PublishmentSystemID, nodeID));

            //this.attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(this.nodeInfo.Additional.ContentAttributesOfDisplay);

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;


            string cmd = "";

            if (!string.IsNullOrEmpty(Request["DateFrom"]))
            {
                cmd += " and ml_Content.AddDate>'" + Request["DateFrom"] + " 00:00:00'";
            }

            if (!string.IsNullOrEmpty(base.Request.QueryString["SearchType"]))
            {
                string schType = base.Request.QueryString["SearchType"];
                string keyword = base.Request.QueryString["Keyword"];

                cmd += " and ml_Content.[" + schType + "] like '%" + keyword + "%'";
            }

            if (!string.IsNullOrEmpty(base.Request.QueryString["Status"]))
            {
                var arr = Request.QueryString["Status"].Split('_');
                string checkedLevel = arr[0];
                string isChecked = arr[1];

                cmd += " and ml_Submission.CheckedLevel=" + checkedLevel + " and ml_Submission.IsChecked='" + isChecked + "'";
            }


            this.spContents.SelectCommand = "select ml_Content.*,ml_Submission.CheckedLevel as curCheckedLevel,ml_Submission.IsChecked as curIsChecked ";
            this.spContents.SelectCommand += " from ml_Content left join ml_Submission";
            this.spContents.SelectCommand += " on ml_Submission.SubmissionID = ml_Content.ReferenceID ";
            this.spContents.SelectCommand += " where NodeID=" + nodeID + " AND ml_Content.checkedlevel=0 and ml_Content.IsChecked='True' " + cmd;


            this.spContents.SortField = "LastEditDate";
            this.spContents.SortMode = SortMode.DESC;
            this.spContents.OrderByString = "ORDER BY LastEditDate DESC";

            if (!IsPostBack)
            {
                string nodeName = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, nodeID);
                //base.BreadCrumbWithItemTitle(ProductManager.WCM.LeftMenu.ID_Content, "投稿箱", nodeName, string.Empty);

                this.spContents.DataBind();


                ltlContentCount1.Text = DataProvider.MlibDAO.GetSubmissionCount(" NodeID=" + nodeID + " AND  ml_Content.checkedlevel=0 and ml_Content.IsChecked='True' ").ToString();

                ltlContentCount2.Text = this.spContents.TotalCount.ToString();

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
                this.SearchType.Items.Add(new ListItem("内容组", ContentAttribute.ContentGroupNameCollection));

                if (!string.IsNullOrEmpty(base.Request.QueryString["SearchType"]))
                {
                    this.DateFrom.Text = base.Request.QueryString["DateFrom"];
                    ControlUtils.SelectListItems(this.SearchType, base.Request.QueryString["SearchType"]);
                    this.Keyword.Text = base.Request.QueryString["Keyword"];

                }
                ddlStatus.Items.Add(new ListItem("全部", ""));
                ddlStatus.Items.Add(new ListItem("初审通过", "1_True"));

                int MaxCheckLevel = TranslateUtils.ToInt(DataProvider.MlibDAO.GetConfigAttr("CheckLevel"));
                for (int i = 2; i <= MaxCheckLevel; i++)
                {
                    ddlStatus.Items.Add(new ListItem(Number2Chinese(i) + "审通过", i + "_True"));
                    ddlStatus.Items.Add(new ListItem(Number2Chinese(i) + "审退稿", i + "_False"));
                }

                if (!string.IsNullOrEmpty(base.Request.QueryString["Status"]))
                {
                    ddlStatus.SelectedValue = Request.QueryString["Status"];
                }
                this.ltlColumnHeadRows.Text = ContentUtility.GetColumnHeadRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.tableStyle, base.PublishmentSystemInfo);
                this.ltlCommandHeadRows.Text = ContentUtility.GetCommandHeadRowsHtml(this.tableStyle, base.PublishmentSystemInfo, this.nodeInfo);
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
                Literal ltlColumnItemRows = e.Item.FindControl("ltlColumnItemRows") as Literal;
                Literal ltlItemStatus = e.Item.FindControl("ltlItemStatus") as Literal;
                Literal ltlIsTask = e.Item.FindControl("ltlIsTask") as Literal;
                Literal ltlItemEditUrl = e.Item.FindControl("ltlItemEditUrl") as Literal;
                Literal ltlCommandItemRows = e.Item.FindControl("ltlCommandItemRows") as Literal;
                Literal ltlGatherIamgeUrl = e.Item.FindControl("ltlGatherIamgeUrl") as Literal;

                ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);

                ltlID.Text = i.ToString();
                ltlItemTitle.Text = contentInfo.Title;
                //if (this.gatherHashtable.Count > 0)
                //{
                //    if (this.gatherHashtable[contentInfo.ID] != null)
                //    {
                //        ltlGatherIamgeUrl.Text = StringUtils.GetGatherImageHtml();
                //    }
                //}

                DataRowView drv = (DataRowView)e.Item.DataItem;
                bool curIsChecked = drv["curIsChecked"].ToString() == "True";
                int curCheckedLevel = TranslateUtils.ToInt(drv["curCheckedLevel"].ToString());
                ltlItemStatus.Text = GetStatusText(curIsChecked, curCheckedLevel);


                ltlItemEditUrl.Text = string.Format("<a href=\"ReviewAdd.aspx?PublishmentSystemID={0}&nodeid={1}&id={2}\">使用草稿</a>", base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID);


                ltlColumnItemRows.Text = ContentUtility.GetColumnItemRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.valueHashtable, this.tableStyle, base.PublishmentSystemInfo, contentInfo);

                ltlCommandItemRows.Text = ContentUtility.GetCommandItemRowsHtml(this.tableStyle, base.PublishmentSystemInfo, this.nodeInfo, contentInfo, PageUtils.GetMLibUrl(this.PageUrl));

            }
        }

        private string GetStatusText(bool isChecked, int CheckedLevel)
        {
            string returnVal;
            if (CheckedLevel == 0)
            {
                returnVal = "未审核";
            }
            else
            {
                string StageText = Number2Chinese(CheckedLevel) + "审";
                string PassText = isChecked ? "通过" : "<span style=\"color:#f00;\">退稿</span>";
                string nextStage = isChecked ? ",等待" + Number2Chinese(CheckedLevel + 1) + "审" : "";

                returnVal = StageText + "" + PassText + nextStage;
            }
            return returnVal;
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
                    //_pageUrl = string.Format("background_content.aspx?PublishmentSystemID={0}&NodeID={1}&ContentType={2}&page={3}", base.PublishmentSystemID, this.nodeInfo.NodeID, EContentTypeUtils.GetValue(this.contentType), TranslateUtils.ToInt(Request.QueryString["page"], 1));
                    _pageUrl = string.Format("DraftList1.aspx?PublishmentSystemID={0}&NodeID={1}&DateFrom={2}&SearchType={3}&Keyword={4}&Status={6}&page={5}", base.PublishmentSystemID, this.nodeInfo.NodeID, this.DateFrom.Text, this.SearchType.SelectedValue, this.Keyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1), ddlStatus.SelectedValue);
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID)
        {
            return string.Format("DraftList1.aspx?PublishmentSystemID={0}&NodeID={1}", publishmentSystemID, nodeID);
        }


        public string Number2Chinese(int n)
        {

            int MaxCheckLevel = TranslateUtils.ToInt(DataProvider.MlibDAO.GetConfigAttr("CheckLevel"));
            if (n == MaxCheckLevel)
            {
                return "终";
            }
            var chinese = new string[] { "", "初", "二", "三", "四", "五", "六", "七", "八", "九" };
            return chinese[n];
        }
    }
}
