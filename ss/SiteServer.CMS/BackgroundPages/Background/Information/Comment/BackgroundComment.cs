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
	public class BackgroundComment : BackgroundBasePage
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
                if (base.PublishmentSystemInfo.Additional.IsContentCommentable == false)
                {
                    PageUtils.RedirectToErrorPage("此栏目下内容不允许评论！");
                    return;
                }
            }

            this.spContents.ControlToPaginate = rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.CommentDAO.GetSelectSqlString(base.PublishmentSystemID, this.nodeID, this.contentID);
            this.spContents.SortField = DataProvider.CommentDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if(!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "评论管理", string.Empty);

				if (base.GetQueryString("Delete") != null)
				{
                    List<int> deleteIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("ContentIDCollection"));
					try
					{
                        DataProvider.CommentDAO.Delete(base.PublishmentSystemID, this.nodeID, this.contentID, deleteIDList);

                        StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, this.contentID, "删除评论", string.Empty);

						base.SuccessDeleteMessage();
					}
					catch(Exception ex)
					{
                        base.FailDeleteMessage(ex);
					}
                }
                else if (base.GetQueryString("Check") != null)
                {
                    List<int> checkIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("ContentIDCollection"));
                    try
                    {
                        DataProvider.CommentDAO.Check(checkIDList, base.PublishmentSystemID);

                        StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, this.contentID, "审核评论", string.Empty);

                        base.SuccessCheckMessage();
                    }
                    catch (Exception ex)
                    {
                        base.FailCheckMessage(ex);
                    }
                }
                else if (base.GetQueryString("Recommend") != null)
                {
                    bool isRecommend = base.GetBoolQueryString("Recommend");
                    List<int> recommendIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("ContentIDCollection"));
                    try
                    {
                        DataProvider.CommentDAO.Recommend(recommendIDList, isRecommend, base.PublishmentSystemID);
                        base.SuccessMessage("设置成功");
                    }
                    catch (Exception ex)
                    {
                        base.FailCheckMessage(ex);
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
                    this.btnCheck.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_comment.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&Check=True&ReturnUrl={3}", base.PublishmentSystemID, this.nodeID, this.contentID, StringUtils.ValueToUrl(this.returnUrl))), "ContentIDCollection", "ContentIDCollection", "请选择需要通过审核的评论！", "此操作将把所选评论设置为审核通过，确认吗？"));
                }
                else
                {
                    this.phCheck.Visible = false;
                }

                this.btnRecommend.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_comment.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&Recommend=True&ReturnUrl={3}", base.PublishmentSystemID, this.nodeID, this.contentID, StringUtils.ValueToUrl(this.returnUrl))), "ContentIDCollection", "ContentIDCollection", "请选择需要设置的评论！", "此操作将把所选评论设置为精彩评论，确认吗？"));

                this.btnRecommendFalse.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_comment.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&Recommend=False&ReturnUrl={3}", base.PublishmentSystemID, this.nodeID, this.contentID, StringUtils.ValueToUrl(this.returnUrl))), "ContentIDCollection", "ContentIDCollection", "请选择需要设置的评论！", "此操作将把所选评论设置为非精彩评论，确认吗？"));

                if (base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.CommentDelete))
                {
                    this.phDelete.Visible = true;
                    this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_comment.aspx?PublishmentSystemID={0}&NodeID={1}&ContentID={2}&Delete=True&ReturnUrl={3}", base.PublishmentSystemID, this.nodeID, this.contentID, StringUtils.ValueToUrl(this.returnUrl))), "ContentIDCollection", "ContentIDCollection", "请选择需要删除的评论！", "此操作将删除所选评论，确认吗？"));
                }
                else
                {
                    this.phDelete.Visible = false;
                }

                string showPopWinString = Modal.CommentAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.nodeID, this.contentID);
                this.btnAdd.Attributes.Add("onclick", showPopWinString);

                showPopWinString = PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToComment(base.PublishmentSystemID, this.nodeID, this.contentID);
                this.btnExport.Attributes.Add("onclick", showPopWinString);
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
                int commentID = TranslateUtils.EvalInt(e.Item.DataItem, CommentAttribute.CommentID);
                int nodeID = TranslateUtils.EvalInt(e.Item.DataItem, CommentAttribute.NodeID);
                int contentID = TranslateUtils.EvalInt(e.Item.DataItem, CommentAttribute.ContentID);
                string userName = TranslateUtils.EvalString(e.Item.DataItem, CommentAttribute.UserName);
                bool isChecked = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, CommentAttribute.IsChecked));
                bool isRecommend = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, CommentAttribute.IsRecommend));
                string ipAddress = TranslateUtils.EvalString(e.Item.DataItem, CommentAttribute.IPAddress);
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, CommentAttribute.AddDate);
                string content = TranslateUtils.EvalString(e.Item.DataItem, CommentAttribute.Content);

                Literal ltlIndex = (Literal)e.Item.FindControl("ltlIndex");
                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlContent = (Literal)e.Item.FindControl("ltlContent");
                Literal ltlIPAddress = (Literal)e.Item.FindControl("ltlIPAddress");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");

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

                if (isChecked == false)
                {
                    this.uncheckedCommentNum++;
                    ltlContent.Text += "&nbsp;<span style='color:#ff0000;text-decoration:none'>[未审核]</span>";
                }
                if (isRecommend)
                {
                    ltlContent.Text += "&nbsp;<span style='color:#ff0000;text-decoration:none'>[精彩评论]</span>";
                }

                ltlIPAddress.Text = ipAddress;

                ltlAddDate.Text = DateUtils.GetDateAndTimeString(addDate);

                string showPopWinString = Modal.CommentAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, this.nodeID, this.contentID, commentID);
                ltlEditUrl.Text = string.Format("<a href=\"javascript:;\" onclick=\"{0}\">修改</a>", showPopWinString);

//                string linkUrl = string.Empty;
//                string linkText = string.Empty;
//                if (nodeID != 0 && contentID != 0)
//                {
//                    linkUrl = PageUtility.GetContentUrl(base.PublishmentSystemInfo, this.nodeInfo, contentID, base.PublishmentSystemInfo.Additional.VisualType);
//                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeInfo);
//                    linkText = BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.Title);
//                }
//                else if (nodeID != 0)
//                {
//                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
//                    if (nodeInfo != null)
//                    {
//                        linkUrl = PageUtility.GetChannelUrl(base.PublishmentSystemInfo, nodeInfo, base.PublishmentSystemInfo.Additional.VisualType);
//                        linkText = nodeInfo.NodeName;
//                    }
//                }
//                tableColumns.Text += string.Format(@"
//		<tr>
//            <td width='50' align='left' valign='top' nowrap><nobr>评论页面：</nobr></span></td>
//            <td align='left'><a href=""{0}"" target=""_blank"">{1}</a></td>
//        </tr>", linkUrl, linkText);
			}
		}

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.returnUrl);
        }
	}
}
