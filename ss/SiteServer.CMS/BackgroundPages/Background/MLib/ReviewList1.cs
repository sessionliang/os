using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class ReviewList1 : MLibBackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlColumnHeadRows;
        public Literal ltlCommandHeadRows;

        public Literal ltlContentCount1;
        public Literal ltlContentCount2;

        public Literal ltlContentButtons;

        public DateTimeTextBox DateFrom;
        public DropDownList SearchType;
        public DropDownList ddlStatus;
        public TextBox Keyword;

        private int MaxCheckLevel;
        private ArrayList CheckLevelArr;
        private NodeInfo nodeInfo;
        private ETableStyle tableStyle;
        private string tableName;
        private StringCollection attributesOfDisplay;
        private ArrayList relatedIdentities;
        private ArrayList tableStyleInfoArrayList;
        private readonly Hashtable valueHashtable = new Hashtable();
        private Hashtable gatherHashtable = new Hashtable();
        public string IsShowTab;
        public void Page_Load(object sender, EventArgs E)
        {

            int nodeID = TranslateUtils.ToInt(Request.QueryString["nodeid"]);
            MaxCheckLevel = TranslateUtils.ToInt(DataProvider.MlibDAO.GetConfigAttr("CheckLevel"));
            CheckLevelArr = new ArrayList();


            if (this.HasCheckLevelPermissions("1"))
            {
                CheckLevelArr.Add("1");
            }
            for (int i = 1; i < MaxCheckLevel; i++)
            {
                if (this.HasNodePermissions((i + 1).ToString(), nodeID.ToString()))
                    CheckLevelArr.Add((i + 1).ToString());
            }
            if (CheckLevelArr.Count == 1 && CheckLevelArr[0].ToString() == "1")
            {
                IsShowTab = " style=\"display:none;\"";
            }

            if (CheckLevelArr.Count == 0)
            {
                if (this.HasNodePermissions("0", nodeID.ToString()))
                {
                    IsShowTab = " style=\"display:none;\"";
                }
                else
                {

                    Response.Write("<script>alert('您没有此栏目的审核权限');history.go(-1);</script>");
                    Response.End();
                }
            }



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

                cmd += " and ml_Content.CheckedLevel=" + checkedLevel + " and ml_Content.IsChecked='" + isChecked + "'";
            }


            var idds = DataProvider.MlibDAO.GetContentIDsAll();
            string contentids = "0";
            for (int i = 0; i < idds.Tables[0].Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(idds.Tables[0].Rows[i]["ID"].ToString()))
                {
                    contentids += "," + idds.Tables[0].Rows[i]["ID"].ToString();
                }
            }

            this.spContents.SelectCommand = " select ml_Content.*,ml_Submission.PassDate from ml_Submission left join ml_Content";
            this.spContents.SelectCommand += " on ml_Submission.SubmissionID = ml_Content.ReferenceID and ml_Submission.CheckedLevel=ml_Content.CheckedLevel";
            this.spContents.SelectCommand += " where ml_Content.ID in (" + contentids + ") and ml_Content.NodeID=" + nodeID + cmd;

            this.spContents.SortField = BaiRongDataProvider.ContentDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.spContents.OrderByString = ETaxisTypeUtils.GetOrderByString(this.tableStyle, ETaxisType.OrderByTaxisDesc);//  


            if (!IsPostBack)
            {
                string nodeName = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, nodeID);
                //base.BreadCrumbWithItemTitle(ProductManager.WCM.LeftMenu.ID_Content, "投稿箱", nodeName, string.Empty);

                //this.ltlContentButtons.Text = WebUtils.GetContentCommands(base.PublishmentSystemInfo, this.nodeInfo, this.PageUrl, "background_content.aspx", false);
                this.spContents.DataBind();

                string cmd1 = " and ml_Submission.PassDate is null and  ( 1=0 ";
                foreach (string item in CheckLevelArr)
                {
                    cmd1 += " or (ml_Submission.CheckedLevel>0 and ml_Submission.CheckedLevel=" + (int.Parse(item) - 1) + " and ml_Submission.IsChecked='True')";
                    cmd1 += " or (ml_Submission.CheckedLevel=" + (int.Parse(item) + 1) + " and ml_Submission.IsChecked='False')";

                }
                cmd1 += ") ";


                ltlContentCount1.Text = DataProvider.MlibDAO.GetSubmissionCount(" ml_Content.ID in (" + contentids + ") and ml_Submission.CheckedLevel>0 and ml_Content.NodeID=" + nodeID + cmd1).ToString();

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

                this.ltlColumnHeadRows.Text = ContentUtility.GetColumnHeadRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.tableStyle, base.PublishmentSystemInfo);
                this.ltlCommandHeadRows.Text = ContentUtility.GetCommandHeadRowsHtml(this.tableStyle, base.PublishmentSystemInfo, this.nodeInfo);

                ddlStatus.Items.Add(new ListItem("全部", ""));
                ddlStatus.Items.Add(new ListItem("初审通过", "1_True"));
                for (int i = 2; i <= MaxCheckLevel; i++)
                {
                    ddlStatus.Items.Add(new ListItem(Number2Chinese(i) + "审通过", i + "_True"));
                    ddlStatus.Items.Add(new ListItem(Number2Chinese(i) + "审退稿", i + "_False"));
                }

                if (!string.IsNullOrEmpty(base.Request.QueryString["Status"]))
                {
                    ddlStatus.SelectedValue = Request.QueryString["Status"];
                }

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


                var rowdata = (DataRowView)e.Item.DataItem;
                if (rowdata["PassDate"] is DBNull)
                {
                    ltlItemStatus.Text = GetStatusText(contentInfo.IsChecked, contentInfo.CheckedLevel);

                    ltlItemEditUrl.Text = string.Format("<a href=\"ReviewShow.aspx?PublishmentSystemID={0}&nodeid={1}&id={2}\">查看</a>", base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID);

                }
                else
                {
                    int refCount = DataProvider.MlibDAO.GetReferenceLogCount(contentInfo.ReferenceID);
                    ltlItemStatus.Text = "终审通过";
                    ltlItemEditUrl.Text = string.Format("<a href=\"ReviewShow.aspx?PublishmentSystemID={0}&nodeid={1}&id={2}\">查看</a>", base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID);

                    if (this.HasNodePermissions("0", nodeInfo.NodeID.ToString()))
                        ltlItemEditUrl.Text += string.Format(" | <a href=\"javascript:;\" onclick=\"Reference({0},{1},{2})\">引用</a>", base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID) + string.Format("(<a href=\"javascript: openWindow('引用记录', 'ReferrenceLogs.aspx?sid={0}', 600, 580, 'true');\">" + refCount + "</a>)", contentInfo.ReferenceID);

                }
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
                string StageText, PassText, nextStage;
                if (CheckedLevel > MaxCheckLevel)
                {
                    StageText = Number2Chinese(MaxCheckLevel - 1) + "审";
                    PassText = "通过";
                    nextStage = ",等待" + Number2Chinese(MaxCheckLevel) + "审";

                }
                else if (CheckedLevel == MaxCheckLevel)
                {
                    if (isChecked)
                    {
                        StageText = Number2Chinese(MaxCheckLevel - 1) + "审";
                        PassText = "通过";
                        nextStage = ",等待" + Number2Chinese(MaxCheckLevel) + "审";
                    }
                    else
                    {

                        StageText = Number2Chinese(CheckedLevel) + "审";
                        PassText = "不通过";
                        nextStage = ",等待" + Number2Chinese(CheckedLevel - 1) + "审重审";
                    }
                }
                else
                {
                    StageText = Number2Chinese(CheckedLevel) + "审";
                    if (isChecked)
                    {
                        PassText = "通过";
                        nextStage = ",等待" + Number2Chinese(CheckedLevel + 1) + "审";
                    }
                    else
                    {
                        PassText = "<span style=\"color:#f00;\">退稿</span>";
                        nextStage = "";
                    }
                }

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
                    _pageUrl = string.Format("ReviewList1.aspx?PublishmentSystemID={0}&NodeID={1}&DateFrom={2}&SearchType={3}&Keyword={4}&Status={6}&page={5}", base.PublishmentSystemID, this.nodeInfo.NodeID, this.DateFrom.Text, this.SearchType.SelectedValue, this.Keyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1), this.ddlStatus.SelectedValue);
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID)
        {
            return string.Format("ReviewList1.aspx?PublishmentSystemID={0}&NodeID={1}", publishmentSystemID, nodeID);
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
