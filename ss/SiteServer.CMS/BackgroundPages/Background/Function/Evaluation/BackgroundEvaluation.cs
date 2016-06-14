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
    public class BackgroundEvaluation : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlTarget;

        private string returnUrl = string.Empty;

        public Button btnAdd;
        public PlaceHolder phCheck;
        public Button btnCheck;
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
                if (nodeInfo.Additional.IsUseEvaluation == false)
                {
                    PageUtils.RedirectToErrorPage("此栏目下内容不允许评价！");
                    return;
                }
            }

            this.spContents.ControlToPaginate = rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.EvaluationContentDAO.GetSelectSqlString(base.PublishmentSystemID, this.nodeID, this.contentID);
            this.spContents.SortField = DataProvider.EvaluationContentDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "评价管理", string.Empty);

                if (base.GetQueryString("Delete") != null)
                {
                    ArrayList deleteIDList = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("ContentIDCollection"));
                    try
                    {
                        DataProvider.EvaluationContentDAO.Delete(base.PublishmentSystemID, this.nodeID, 0, deleteIDList);
                        StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "删除评价", string.Empty);
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
                    this.ltlTarget.Text += string.Format(@"<span style=""color:red"">(未审核数目：{0})</span>", this.uncheckedCommentNum);
                }

                if (base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.CommentCheck) && this.uncheckedCommentNum > 0)
                {
                    this.phCheck.Visible = true;
                    this.btnCheck.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_evaluation.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&Check=True&ReturnUrl={3}", base.PublishmentSystemID, this.nodeID, this.contentID, StringUtils.ValueToUrl(this.returnUrl))), "ContentIDCollection", "ContentIDCollection", "请选择需要通过审核的评价！", "此操作将把所选评价设置为审核通过，确认吗？"));
                }
                else
                {
                    this.phCheck.Visible = false;
                } 
                //if (base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.CommentDelete))
                if (nodeInfo.Additional.IsUseEvaluation)
                {
                    this.phDelete.Visible = true;
                    this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_evaluation.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&Delete=True&ReturnUrl={3}", base.PublishmentSystemID, this.nodeID, this.contentID, StringUtils.ValueToUrl(this.returnUrl))), "ContentIDCollection", "ContentIDCollection", "请选择需要删除的评价！", "此操作将删除所选评价，确认吗？"));
                }
                else
                {
                    this.phDelete.Visible = false;
                }

                string showPopWinString = Modal.CommentAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.nodeID, this.contentID);
                this.btnAdd.Attributes.Add("onclick", showPopWinString);

                showPopWinString = PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToComment(base.PublishmentSystemID, this.nodeID, this.contentID);
                this.btnExport.Attributes.Add("onclick", showPopWinString);

                string openWindow = Modal.FunctionStyleAnalysisShow.GetOpenWindowString(base.PublishmentSystemID, this.nodeInfo.NodeID, contentID, ETableStyle.EvaluationContent, EvaluationContentInfo.TableName, string.Empty, string.Empty);
                this.btnAnalysis.Attributes.Add("onclick", openWindow);
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int commentID = TranslateUtils.EvalInt(e.Item.DataItem, EvaluationContentAttribute.ECID);
                int nodeID = TranslateUtils.EvalInt(e.Item.DataItem, EvaluationContentAttribute.NodeID);
                int contentID = TranslateUtils.EvalInt(e.Item.DataItem, EvaluationContentAttribute.ContentID);
                string userName = TranslateUtils.EvalString(e.Item.DataItem, EvaluationContentAttribute.UserName);
                bool isChecked = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, EvaluationContentAttribute.IsChecked));
                string ipAddress = TranslateUtils.EvalString(e.Item.DataItem, EvaluationContentAttribute.IPAddress);
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, EvaluationContentAttribute.AddDate);
                string content = TranslateUtils.EvalString(e.Item.DataItem, EvaluationContentAttribute.Description);
                string compositeScore = TranslateUtils.EvalString(e.Item.DataItem, EvaluationContentAttribute.CompositeScore);

                Literal ltlIndex = (Literal)e.Item.FindControl("ltlIndex");
                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlContent = (Literal)e.Item.FindControl("ltlContent");
                Literal ltlIPAddress = (Literal)e.Item.FindControl("ltlIPAddress");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
                Literal ltlCompositeScore = (Literal)e.Item.FindControl("ltlCompositeScore");

                ltlIndex.Text = Convert.ToString(e.Item.ItemIndex + 1);

                if (string.IsNullOrEmpty(userName))
                {
                    ltlUserName.Text = "匿名用户";
                }
                else
                {
                    ltlUserName.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", Modal.UserView.GetOpenWindowString(base.PublishmentSystemID, userName), userName);
                }

                ltlContent.Text = TranslateUtils.ParseCommentContent(content);

                ltlIPAddress.Text = ipAddress;

                ltlAddDate.Text = DateUtils.GetDateAndTimeString(addDate);

                string showPopWinString = Modal.FunctionStyleValueShow.GetOpenWindowStringToEvaluation(base.PublishmentSystemID, this.nodeID, this.contentID, commentID);
                ltlEditUrl.Text = string.Format("<a href=\"javascript:;\" onclick=\"{0}\">查看</a>", showPopWinString);
                ltlCompositeScore.Text = compositeScore;
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.returnUrl);
        }
    }
}
