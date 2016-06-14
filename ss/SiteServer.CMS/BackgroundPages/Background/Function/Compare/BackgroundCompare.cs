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
    public class BackgroundCompare : BackgroundBasePage
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
                if (nodeInfo.Additional.IsUseCompare == false)
                {
                    PageUtils.RedirectToErrorPage("此内容所属栏目未开启比较功能！");
                    return;
                }
            }

            this.spContents.ControlToPaginate = rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.CompareContentDAO.GetSelectSqlString(base.PublishmentSystemID, this.nodeID, this.contentID);
            this.spContents.SortField = DataProvider.CompareContentDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "比较反馈管理", string.Empty);

                if (base.GetQueryString("Delete") != null)
                {
                    ArrayList deleteIDList = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("ContentIDCollection"));
                    try
                    {
                        DataProvider.CompareContentDAO.Delete(base.PublishmentSystemID, this.nodeID, 0, deleteIDList);
                        StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "删除反馈", string.Empty);
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
                }

                this.phCheck.Visible = false;
                if (nodeInfo.Additional.IsUseCompare)
                {
                    this.phDelete.Visible = true;
                    this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_evaluation.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&Delete=True&ReturnUrl={3}", base.PublishmentSystemID, this.nodeID, this.contentID, StringUtils.ValueToUrl(this.returnUrl))), "ContentIDCollection", "ContentIDCollection", "请选择需要删除的反馈内容！", "此操作将删除所选反馈内容，确认吗？"));
                }
                else
                {
                    this.phDelete.Visible = false;
                }

                string showPopWinString = Modal.CommentAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.nodeID, this.contentID);
                this.btnAdd.Attributes.Add("onclick", showPopWinString);

                showPopWinString = PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToComment(base.PublishmentSystemID, this.nodeID, this.contentID);
                this.btnExport.Attributes.Add("onclick", showPopWinString);

                string openWindow = Modal.FunctionStyleAnalysisShow.GetOpenWindowString(base.PublishmentSystemID, this.nodeInfo.NodeID, contentID, ETableStyle.CompareContent, EvaluationContentInfo.TableName, string.Empty, string.Empty);
                this.btnAnalysis.Attributes.Add("onclick", openWindow);
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int commentID = TranslateUtils.EvalInt(e.Item.DataItem, CompareContentAttribute.CCID);
                int nodeID = TranslateUtils.EvalInt(e.Item.DataItem, CompareContentAttribute.NodeID);
                int contentID = TranslateUtils.EvalInt(e.Item.DataItem, CompareContentAttribute.ContentID);
                string userName = TranslateUtils.EvalString(e.Item.DataItem, CompareContentAttribute.UserName);
                bool isChecked = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, CompareContentAttribute.CompareStatus));
                string ipAddress = TranslateUtils.EvalString(e.Item.DataItem, CompareContentAttribute.IPAddress);
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, CompareContentAttribute.AddDate);
                string content = TranslateUtils.EvalString(e.Item.DataItem, CompareContentAttribute.Description);
                string compositeScore1 = TranslateUtils.EvalString(e.Item.DataItem, CompareContentAttribute.CompositeScore1);
                string compositeScore2 = TranslateUtils.EvalString(e.Item.DataItem, CompareContentAttribute.CompositeScore2);

                Literal ltlIndex = (Literal)e.Item.FindControl("ltlIndex");
                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlContent = (Literal)e.Item.FindControl("ltlContent");
                Literal ltlIPAddress = (Literal)e.Item.FindControl("ltlIPAddress");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
                Literal ltlCompositeScore1 = (Literal)e.Item.FindControl("ltlCompositeScore1");
                Literal ltlCompositeScore2 = (Literal)e.Item.FindControl("ltlCompositeScore2");

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
                    ltlUserName.Text += "&nbsp;<span style='color:#ff0000;text-decoration:none'>[未查看]</span>";
                }
                ltlContent.Text = TranslateUtils.ParseCommentContent(content);

                ltlIPAddress.Text = ipAddress;

                ltlAddDate.Text = DateUtils.GetDateAndTimeString(addDate);

                string showPopWinString = Modal.FunctionStyleValueShow.GetOpenWindowStringToCompare(base.PublishmentSystemID, this.nodeID, this.contentID, commentID);
                ltlEditUrl.Text = string.Format("<a href=\"javascript:;\" onclick=\"{0}\">查看</a>", showPopWinString);
                ltlCompositeScore1.Text = compositeScore1;
                ltlCompositeScore2.Text = compositeScore2;
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.returnUrl);
        }
    }
}
