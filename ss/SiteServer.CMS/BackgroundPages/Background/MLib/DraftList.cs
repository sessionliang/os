using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class DraftList : MLibBackgroundBasePage
    {

        public PlaceHolder phStatus;
        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlColumnHeadRows;
        public Literal ltlCommandHeadRows;

        public Literal ltlContentCount1;
        public Literal ltlContentCount2;

        public DateTimeTextBox DateFrom;
        public DropDownList SearchType;
        public TextBox Keyword;

        private NodeInfo nodeInfo;
        private ETableStyle tableStyle;
        private string tableName;
        private StringCollection attributesOfDisplay;
        private ArrayList relatedIdentities;
        private ArrayList tableStyleInfoArrayList;
        private readonly Hashtable valueHashtable = new Hashtable();
        private Hashtable gatherHashtable = new Hashtable();
        public int nodeID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!this.HasCheckLevelPermissions("1"))
            {
                Response.Redirect(Request.Url.ToString().Replace("DraftList", "DraftList1"));
            }
            //批量审核
            string action = Request.QueryString["action"];
            string ContentIDCollection = Request.QueryString["ContentIDCollection"];
            if (action == "pass")
            {
                //ContentBatchPass(ContentIDCollection);

                Response.Write("<script>alert('批量审核完成!');location.href='" + PageUrl + "';</script>");
                Response.End();
            }
            else if (action == "del")
            {
                ContentBatchDel(ContentIDCollection);

                Response.Write("<script>alert('批量删除完成!');location.href='" + PageUrl + "';</script>");
                Response.End();

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



            this.spContents.SelectCommand = "select ml_Content.* from ml_Submission left join ml_Content";
            this.spContents.SelectCommand += " on ml_Submission.SubmissionID = ml_Content.ReferenceID and ml_Submission.CheckedLevel=ml_Content.CheckedLevel";
            this.spContents.SelectCommand += " where NodeID=" + nodeID + " AND ml_Submission.CheckedLevel=0 and ml_Submission.IsChecked='True' " + cmd;


            this.spContents.SortField = "AddDate";
            this.spContents.SortMode = SortMode.DESC;
            this.spContents.OrderByString = "ORDER BY AddDate DESC";

            if (!IsPostBack)
            {
                string nodeName = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, nodeID);
                //base.BreadCrumbWithItemTitle(ProductManager.WCM.LeftMenu.ID_Content, "投稿箱", nodeName, string.Empty);

                this.spContents.DataBind();


                ltlContentCount1.Text = this.spContents.TotalCount.ToString();
                ltlContentCount2.Text = DataProvider.MlibDAO.GetDraftCount(base.PublishmentSystemID).ToString();



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
                    this.phStatus.Visible = true;

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
                PlaceHolder phItemStatus = e.Item.FindControl("phItemStatus") as PlaceHolder;

                phItemStatus.Visible = true;
                ltlItemStatus.Visible = true;

                ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);

                ltlID.Text = i.ToString();
                ltlItemTitle.Text = contentInfo.Title;
                //if (this.gatherHashtable.Count > 0)
                //{
                //    if (this.gatherHashtable[contentInfo.ID] != null)
                //    {
                //        ltlGatherIamgeUrl.Text = StringUtils.g();
                //    }
                //}


                ltlItemStatus.Text = GetStatusText(contentInfo.IsChecked, contentInfo.CheckedLevel);


                ltlItemEditUrl.Text = string.Format("<a href=\"DraftEdit.aspx?PublishmentSystemID={0}&nodeid={1}&id={2}\">审核</a>", base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID);


                ltlColumnItemRows.Text = ContentUtility.GetColumnItemRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.valueHashtable, this.tableStyle, base.PublishmentSystemInfo, contentInfo);

                ltlCommandItemRows.Text = ContentUtility.GetCommandItemRowsHtml(this.tableStyle, base.PublishmentSystemInfo, this.nodeInfo, contentInfo, this.PageUrl);

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
                string PassText = isChecked ? "已通过" : "<span style=\"color:#f00;\">未通过</span>";
                returnVal = StageText + "" + PassText;
            }
            return returnVal;
        }


        
        private void ContentBatchDel(string ids)
        {
            var idArr = ids.Split(',');
            for (int i = 0; i < idArr.Length; i++)
            {
                int nodeID = TranslateUtils.ToInt(Request.QueryString["nodeid"]);
                int contentID = TranslateUtils.ToInt(idArr[i]);

                this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeInfo);
                this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
                this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);
                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, contentID);

                DataProvider.MlibDAO.DeleteSubmission(contentInfo.ReferenceID);
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
                    //_pageUrl = string.Format("background_content.aspx?PublishmentSystemID={0}&NodeID={1}&ContentType={2}&page={3}", base.PublishmentSystemID, this.nodeInfo.NodeID, EContentTypeUtils.GetValue(this.contentType), TranslateUtils.ToInt(Request.QueryString["page"], 1));
                    _pageUrl = string.Format("DraftList.aspx?PublishmentSystemID={0}&NodeID={1}&DateFrom={2}&SearchType={3}&Keyword={4}&page={5}", base.PublishmentSystemID, this.nodeInfo.NodeID, this.DateFrom.Text, this.SearchType.SelectedValue, this.Keyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID)
        {
            return string.Format("DraftList.aspx?PublishmentSystemID={0}&NodeID={1}", publishmentSystemID, nodeID);
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
