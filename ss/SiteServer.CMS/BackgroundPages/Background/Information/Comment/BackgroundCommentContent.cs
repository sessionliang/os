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
using System.Collections.Generic;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundCommentContent : BackgroundBasePage
	{
        public DropDownList NodeIDDropDownList;
        public DropDownList TypeDropDownList;
        public DropDownList PageNum;
        public TextBox Keyword;
        public DropDownList SearchDate;
        public DropDownList IsChecked;
        public CheckBox IsSearchChildren;

		public Repeater rptContents;
		public SqlPager spContents;

        public PlaceHolder phCheck;
        public Button btnCheck;
        public Button btnRecommend;
        public Button btnRecommendFalse;
        public PlaceHolder phDelete;
        public Button btnDelete;

        int nodeID = 0;

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

			if(!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "评论管理", string.Empty);

                if (base.GetQueryString("Delete") != null)
				{
                    List<int> deleteIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("ContentIDCollection"));
					try
					{
                        DataProvider.CommentDAO.Delete(deleteIDList);
                        StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "删除评论", string.Empty);
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
                        StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "审核评论", string.Empty);
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

                NodeManager.AddListItems(this.NodeIDDropDownList.Items, base.PublishmentSystemInfo, true, true);

                ListItem listItem = new ListItem("全部", ETriStateUtils.GetValue(ETriState.All));
                this.TypeDropDownList.Items.Add(listItem);
                listItem = new ListItem("栏目评论", ETriStateUtils.GetValue(ETriState.True));
                this.TypeDropDownList.Items.Add(listItem);
                listItem = new ListItem("内容评论", ETriStateUtils.GetValue(ETriState.False));
                this.TypeDropDownList.Items.Add(listItem);

                if (!string.IsNullOrEmpty(base.GetQueryString("NodeID")))
                {
                    ControlUtils.SelectListItems(this.NodeIDDropDownList, this.nodeID.ToString());
                    ControlUtils.SelectListItems(this.TypeDropDownList, base.GetQueryString("Type"));
                    ControlUtils.SelectListItems(this.PageNum, base.GetQueryString("PageNum"));
                    this.Keyword.Text = base.GetQueryString("Keyword");
                    ControlUtils.SelectListItems(this.SearchDate, base.GetQueryString("SearchDate"));
                    this.IsChecked.SelectedValue = base.GetQueryString("IsChecked");
                    this.IsSearchChildren.Checked = TranslateUtils.ToBool(base.GetQueryString("IsSearchChildren"));
                }

                List<int> channelIDList = new List<int>();
                if (this.IsSearchChildren.Checked)
                {
                    channelIDList = DataProvider.NodeDAO.GetNodeIDListForDescendant(this.nodeID);
                    channelIDList.Add(this.nodeID);
                }
                else
                {
                    channelIDList.Add(this.nodeID);
                }

                this.spContents.ControlToPaginate = this.rptContents;
                if (TranslateUtils.ToInt(this.PageNum.SelectedValue) == 0)
                {
                    this.spContents.ItemsPerPage = PublishmentSystemManager.GetPublishmentSystemInfo(base.PublishmentSystemID).Additional.PageSize;
                }
                else
                {
                    this.spContents.ItemsPerPage = TranslateUtils.ToInt(this.PageNum.SelectedValue);
                }
                this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
                ETriState checkedState = ETriStateUtils.GetEnumType(this.IsChecked.SelectedValue);
                ETriState channelState = ETriStateUtils.GetEnumType(this.TypeDropDownList.SelectedValue);
                this.spContents.SelectCommand = DataProvider.CommentDAO.GetSelectSqlString(base.PublishmentSystemID, channelIDList, this.Keyword.Text, TranslateUtils.ToInt(this.SearchDate.SelectedValue), checkedState, channelState);

                this.spContents.SortField = DataProvider.CommentDAO.GetSortFieldName();
                this.spContents.SortMode = SortMode.DESC;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.spContents.DataBind();

                if (base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.CommentCheck))
                {
                    this.phCheck.Visible = true;
                    this.btnCheck.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_commentContent.aspx?PublishmentSystemID={0}&NodeID={1}&PageNum={2}&Keyword={3}&SearchDate={4}&IsChecked={5}&IsSearchChildren={6}&Type={7}&Check=True", base.PublishmentSystemID, this.NodeIDDropDownList.SelectedValue, this.PageNum.SelectedValue, this.Keyword.Text, this.SearchDate.SelectedValue, this.IsChecked.SelectedValue, this.IsSearchChildren.Checked, this.TypeDropDownList.SelectedValue)), "ContentIDCollection", "ContentIDCollection", "请选择需要通过审核的评论！", "此操作将把所选评论设置为审核通过，确认吗？"));
                }
                else
                {
                    this.phCheck.Visible = false;
                }

                this.btnRecommend.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_commentContent.aspx?PublishmentSystemID={0}&NodeID={1}&PageNum={2}&Keyword={3}&SearchDate={4}&IsChecked={5}&IsSearchChildren={6}&Type={7}&Recommend=True", base.PublishmentSystemID, this.NodeIDDropDownList.SelectedValue, this.PageNum.SelectedValue, this.Keyword.Text, this.SearchDate.SelectedValue, this.IsChecked.SelectedValue, this.IsSearchChildren.Checked, this.TypeDropDownList.SelectedValue)), "ContentIDCollection", "ContentIDCollection", "请选择需要设置的评论！", "此操作将把所选评论设置为精彩评论，确认吗？"));

                this.btnRecommendFalse.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_commentContent.aspx?PublishmentSystemID={0}&NodeID={1}&PageNum={2}&Keyword={3}&SearchDate={4}&IsChecked={5}&IsSearchChildren={6}&Type={7}&Recommend=False", base.PublishmentSystemID, this.NodeIDDropDownList.SelectedValue, this.PageNum.SelectedValue, this.Keyword.Text, this.SearchDate.SelectedValue, this.IsChecked.SelectedValue, this.IsSearchChildren.Checked, this.TypeDropDownList.SelectedValue)), "ContentIDCollection", "ContentIDCollection", "请选择需要设置的评论！", "此操作将把所选评论设置为非精彩评论，确认吗？"));

                if (base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.CommentDelete))
                {
                    this.phDelete.Visible = true;
                    this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_commentContent.aspx?PublishmentSystemID={0}&NodeID={1}&PageNum={2}&Keyword={3}&SearchDate={4}&IsChecked={5}&IsSearchChildren={6}&Type={7}&Delete=True", base.PublishmentSystemID, this.NodeIDDropDownList.SelectedValue, this.PageNum.SelectedValue, this.Keyword.Text, this.SearchDate.SelectedValue, this.IsChecked.SelectedValue, this.IsSearchChildren.Checked, this.TypeDropDownList.SelectedValue)), "ContentIDCollection", "ContentIDCollection", "请选择需要删除的评论！", "此操作将删除所选评论，确认吗？"));
                }
                else
                {
                    this.phDelete.Visible = false;
                }
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

                Literal ltlContent = (Literal)e.Item.FindControl("ltlContent");
                Literal ltlPageUrl = (Literal)e.Item.FindControl("ltlPageUrl");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");

                if (string.IsNullOrEmpty(userName))
                {
                    ltlUserName.Text = "匿名用户";
                }
                else
                {
                    ltlUserName.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", Modal.UserView.GetOpenWindowString(base.PublishmentSystemID, userName), userName);
                }

                if (!string.IsNullOrEmpty(content))
                {
                    content = StringUtils.MaxLengthText(StringUtils.StripTags(content), 100);
                    ltlContent.Text = TranslateUtils.ParseEmotionHtml(content, false);
                }
                else
                {
                    e.Item.Visible = false;
                }

                if (isChecked == false)
                {
                    ltlContent.Text += "&nbsp;<span style='color:#ff0000;text-decoration:none'>[未审核]</span>";
                }
                if (isRecommend)
                {
                    ltlContent.Text += "&nbsp;<span style='color:#ff0000;text-decoration:none'>[精彩评论]</span>";
                }

                ltlAddDate.Text = DateUtils.GetDateAndTimeString(addDate);

                string showPopWinString = Modal.CommentAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, nodeID, contentID, commentID);
                ltlEditUrl.Text = string.Format("<a href=\"javascript:;\" onclick=\"{0}\">修改</a>", showPopWinString);

                string linkUrl = string.Empty;
                string linkText = string.Empty;

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                if (nodeInfo != null)
                {
                    if (nodeID != 0 && contentID != 0)
                    {
                        linkUrl = PageUtility.ServiceSTL.Utils.GetRedirectUrl(base.PublishmentSystemID, nodeID, contentID, true);
                        string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID));
                        linkText = BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.Title);
                    }
                    else if (nodeID != 0)
                    {
                        linkUrl = PageUtility.GetChannelUrl(base.PublishmentSystemInfo, nodeInfo, base.PublishmentSystemInfo.Additional.VisualType);
                        linkText = nodeInfo.NodeName;
                    }
                }

                if (string.IsNullOrEmpty(linkText))
                {
                    e.Item.Visible = false;
                }
                ltlPageUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", linkUrl, linkText);
			}
		}

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_commentContent.aspx?PublishmentSystemID={0}&NodeID={1}&PageNum={2}&Keyword={3}&SearchDate={4}&IsChecked={5}&IsSearchChildren={6}&Type={7}", base.PublishmentSystemID, this.NodeIDDropDownList.SelectedValue, this.PageNum.SelectedValue, this.Keyword.Text, this.SearchDate.SelectedValue, this.IsChecked.SelectedValue, this.IsSearchChildren.Checked, this.TypeDropDownList.SelectedValue)));
        }
	}
}
