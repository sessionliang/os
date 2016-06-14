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
using System.Collections.Generic;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundTrialReport : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlTarget;

        private string returnUrl = string.Empty;

        public Button btnAdd;
        public PlaceHolder phCheck;
        public Button Check;
        public Button btnRecommend;
        public Button btnRecommendFalse;
        public PlaceHolder phDelete;
        public Button btnDelete;
        public Button btnExport;
        public Button btnAnalysis;

        private int nodeID = 0;
        private int contentID = 0;
        private int uncheckedCommentNum = 0;
        private NodeInfo nodeInfo;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ReturnUrl");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));

            this.nodeID = base.GetIntQueryString("NodeID");
            this.contentID = base.GetIntQueryString("ContentID");

            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
            if (this.contentID > 0)
            {
                if (nodeInfo.Additional.IsUseTrial == false)
                {
                    PageUtils.RedirectToErrorPage("此栏目下内容未开启试用功能！");
                    return;
                }
            }

            this.spContents.ControlToPaginate = rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.TrialReportDAO.GetSelectSqlString(base.PublishmentSystemID, this.nodeID, this.contentID);
            this.spContents.SortField = DataProvider.TrialReportDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "试用报告", string.Empty);

                if (base.GetQueryString("Delete") != null)
                {
                    ArrayList deleteIDList = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("ContentIDCollection"));
                    try
                    {
                        DataProvider.TrialReportDAO.Delete(base.PublishmentSystemID, this.nodeID, 0, deleteIDList);
                        StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "删除申请报告", string.Empty);
                        base.SuccessDeleteMessage();
                    }
                    catch (Exception ex)
                    {
                        base.FailDeleteMessage(ex);
                    }
                }

                this.spContents.DataBind();

                if (this.contentID != 0)
                {
                    ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeInfo);
                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeInfo);
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, this.contentID);
                    this.ltlTarget.Text = string.Format("<a href='{0}' target='blank'>{1}</a>", PageUtility.GetContentUrl(base.PublishmentSystemInfo, contentInfo, base.PublishmentSystemInfo.Additional.VisualType), contentInfo.Title);
                }
                else
                {
                    this.ltlTarget.Text = string.Format("<a href='{0}' target='blank'>{1}</a>", PageUtility.GetChannelUrl(base.PublishmentSystemInfo, nodeInfo, base.PublishmentSystemInfo.Additional.VisualType), nodeInfo.NodeName);
                }

                if (this.uncheckedCommentNum > 0)
                {
                    this.ltlTarget.Text += string.Format(@"<span style=""color:red"">(未查看数目：{0})</span>", this.uncheckedCommentNum);

                    this.phCheck.Visible = true;
                    string showPopWinString = Modal.TrialApplyChecked.GetOpenWindowString(base.PublishmentSystemID, this.nodeID, this.contentID, this.PageUrl);
                    this.Check.Attributes.Add("onclick", showPopWinString);
                }
                else
                {
                    this.phCheck.Visible = false;
                }


                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_trialApply.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&Delete=True&ReturnUrl={3}", base.PublishmentSystemID, this.nodeID, this.contentID, StringUtils.ValueToUrl(this.returnUrl))), "ContentIDCollection", "ContentIDCollection", "请选择需要删除的试用报告！", "此操作将删除所选试用报告，确认吗？"));

                string openWindow = Modal.FunctionStyleAnalysisShow.GetOpenWindowString(base.PublishmentSystemID, this.nodeInfo.NodeID, contentID, ETableStyle.TrialReportContent, TrialReportInfo.TableName, string.Empty, string.Empty);
                this.btnAnalysis.Attributes.Add("onclick", openWindow);

            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int taid = TranslateUtils.EvalInt(e.Item.DataItem, TrialReportAttribute.TRID);
                int nodeID = TranslateUtils.EvalInt(e.Item.DataItem, TrialReportAttribute.NodeID);
                int contentID = TranslateUtils.EvalInt(e.Item.DataItem, TrialReportAttribute.ContentID);
                string userName = TranslateUtils.EvalString(e.Item.DataItem, TrialReportAttribute.UserName);
                bool isChecked = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, TrialReportAttribute.ReportStatus)); 
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, TrialReportAttribute.AddDate);

                Literal ltlIndex = (Literal)e.Item.FindControl("ltlIndex");
                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
                Literal ltlIsChecked = (Literal)e.Item.FindControl("ltlIsChecked");

                ltlIndex.Text = Convert.ToString(e.Item.ItemIndex + 1);

                if (string.IsNullOrEmpty(userName))
                {
                    ltlUserName.Text = "匿名用户";
                }
                else
                {
                    ltlUserName.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", Modal.UserView.GetOpenWindowString(base.PublishmentSystemID, userName), userName);
                }

                if (isChecked == false)
                {
                    this.uncheckedCommentNum++;
                    string showchekPopWinString = Modal.TrialApplyChecked.GetOneOpenWindowString(base.PublishmentSystemID, this.nodeID, contentID, taid, this.PageUrl);
                    ltlIsChecked.Text = string.Format("&nbsp;<a href=\"javascript:;\" onclick=\"{0}\"><span style='color:#ff0000;text-decoration:none'>未查看</span></a>", showchekPopWinString);
                }
                else
                    ltlIsChecked.Text = "<span style='color:green;text-decoration:none'>已查看</span>";

                ltlAddDate.Text = DateUtils.GetDateAndTimeString(addDate);

                string showPopWinString = Modal.FunctionStyleValueShow.GetOpenWindowStringToTrialReport(base.PublishmentSystemID, this.nodeID, this.contentID, taid);
                ltlEditUrl.Text = string.Format("<a href=\"javascript:;\" onclick=\"{0}\">查看</a>&nbsp;&nbsp;", showPopWinString);
                 
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.returnUrl);
        }


        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = PageUtils.GetCMSUrl(string.Format("background_trialApply.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&ReturnUrl={3}", base.PublishmentSystemID, this.nodeID, this.contentID, this.returnUrl));
                }
                return this._pageUrl;
            }
        }
    }
}
