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
    public class BackgroundSurveyContent : BackgroundBasePage
    {
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
        public Alerts alertsID;

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

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            if (!base.HasChannelPermissions(nodeInfo.NodeID, AppManager.CMS.Permission.Channel.ContentView, AppManager.CMS.Permission.Channel.ContentDelete))
            {
                this.alertsID.Text = "您无当前栏目的查看和删除权限！";
                return;
            }
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Survey, "调查问卷管理", AppManager.CMS.Permission.WebSite.Survey);

                if (base.GetQueryString("Delete") != null)
                {
                    ArrayList deleteIDList = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("ContentIDCollection"));
                    try
                    {
                        DataProvider.SurveyQuestionnaireDAO.Delete(base.PublishmentSystemID, this.nodeID, 0, deleteIDList);
                        StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "删除调查问卷", string.Empty);
                        base.SuccessDeleteMessage();
                    }
                    catch (Exception ex)
                    {
                        base.FailDeleteMessage(ex);
                    }
                }

                if (!string.IsNullOrEmpty(base.GetQueryString("NodeID")))
                {
                    ControlUtils.SelectListItems(this.PageNum, base.GetQueryString("PageNum"));
                    this.Keyword.Text = base.GetQueryString("Keyword");
                    ControlUtils.SelectListItems(this.SearchDate, base.GetQueryString("SearchDate"));
                    this.IsChecked.SelectedValue = base.GetQueryString("IsChecked");
                    this.IsSearchChildren.Checked = TranslateUtils.ToBool(base.GetQueryString("IsSearchChildren"));
                }

                List<int> channelIDList = new List<int>();
                if (this.IsSearchChildren.Checked)
                {
                    channelIDList = DataProvider.NodeDAO.GetNodeIDListForDescendantUseEvaluation(this.PublishmentSystemID, this.nodeID);
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
                this.spContents.SelectCommand = DataProvider.SurveyQuestionnaireDAO.GetSelectSqlString(base.PublishmentSystemID, channelIDList, this.Keyword.Text, TranslateUtils.ToInt(this.SearchDate.SelectedValue), checkedState, ETriState.False);

                this.spContents.SortField = DataProvider.SurveyQuestionnaireDAO.GetSortFieldName();
                this.spContents.SortMode = SortMode.DESC;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.spContents.DataBind();


                this.phCheck.Visible = false;


                if (nodeInfo.Additional.IsUseSurvey)
                {
                    this.phDelete.Visible = true;
                    this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(this.redirectStr + "&Delete=True"), "ContentIDCollection", "ContentIDCollection", "请选择需要删除的调查问卷！", "此操作将删除所选调查问卷，确认吗？"));
                }
                else
                {
                    this.phDelete.Visible = false;
                    this.alertsID.Text = "当前栏目未启用调查问卷功能，调查问卷功能不可用！";
                }
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int id = TranslateUtils.EvalInt(e.Item.DataItem, SurveyQuestionnaireAttribute.SQID);
                int nodeID = TranslateUtils.EvalInt(e.Item.DataItem, SurveyQuestionnaireAttribute.NodeID);
                int contentID = TranslateUtils.EvalInt(e.Item.DataItem, SurveyQuestionnaireAttribute.ContentID);
                string userName = TranslateUtils.EvalString(e.Item.DataItem, SurveyQuestionnaireAttribute.UserName);
                bool isChecked = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, SurveyQuestionnaireAttribute.SurveyStatus));
                string ipAddress = TranslateUtils.EvalString(e.Item.DataItem, SurveyQuestionnaireAttribute.IPAddress);
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, SurveyQuestionnaireAttribute.AddDate);
                string content = TranslateUtils.EvalString(e.Item.DataItem, SurveyQuestionnaireAttribute.Description);
                string compositeScore = TranslateUtils.EvalString(e.Item.DataItem, SurveyQuestionnaireAttribute.CompositeScore);

                Literal ltlContent = (Literal)e.Item.FindControl("ltlContent");
                Literal ltlPageUrl = (Literal)e.Item.FindControl("ltlPageUrl");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
                Literal ltlCompositeScore = (Literal)e.Item.FindControl("ltlCompositeScore"); 

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
                    ltlUserName.Text += "&nbsp;<span style='color:#ff0000;text-decoration:none'>[未查看]</span>";
                }

                ltlAddDate.Text = DateUtils.GetDateAndTimeString(addDate);

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
                ltlCompositeScore.Text = compositeScore;

                string showPopWinString = Modal.FunctionStyleValueShow.GetOpenWindowStringToSurvey(base.PublishmentSystemID, this.nodeID, contentID, id);
                ltlEditUrl.Text += string.Format("&nbsp;<a href=\"javascript:;\" onclick=\"{0}\">查看</a>", showPopWinString);
            }
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(PageUtils.GetCMSUrl(this.RedirectStr));
        }

        private string redirectStr;
        public string RedirectStr
        {
            get
            {
                if (string.IsNullOrEmpty(this.redirectStr))
                {
                    redirectStr = string.Format("background_surveyMainContent.aspx?PublishmentSystemID={0}&NodeID={1}&PageNum={2}&Keyword={3}&SearchDate={4}&IsChecked={5}&IsSearchChildren={6}", base.PublishmentSystemID, this.nodeID, this.PageNum.SelectedValue, this.Keyword.Text, this.SearchDate.SelectedValue, this.IsChecked.SelectedValue, this.IsSearchChildren.Checked);
                }
                return this.redirectStr;
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_surveyMainContent.aspx?PublishmentSystemID={0}&NodeID={1}", publishmentSystemID, nodeID));
        }
    }
}
