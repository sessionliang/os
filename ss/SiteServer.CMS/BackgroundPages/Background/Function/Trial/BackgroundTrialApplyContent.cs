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
    public class BackgroundTrialApplyContent : BackgroundBasePage
    {
        public DropDownList PageNum;
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
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Trial, "试用申请记录管理", AppManager.CMS.Permission.WebSite.Trial);

                if (base.GetQueryString("Delete") != null)
                {
                    ArrayList deleteIDList = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("ContentIDCollection"));
                    try
                    {
                        DataProvider.TrialApplyDAO.Delete(base.PublishmentSystemID, this.nodeID, 0, deleteIDList);
                        StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "删除试用报告", string.Empty);
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
                this.spContents.SelectCommand = DataProvider.TrialApplyDAO.GetSelectSqlString(base.PublishmentSystemID, channelIDList, TranslateUtils.ToInt(this.SearchDate.SelectedValue), checkedState, ETriState.False);

                this.spContents.SortField = DataProvider.TrialApplyDAO.GetSortFieldName();
                this.spContents.SortMode = SortMode.DESC;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.spContents.DataBind();

                this.phCheck.Visible = true;
                string showPopWinString = Modal.TrialApplyChecked.GetOpenWindowString(base.PublishmentSystemID, this.nodeID, 0, this.RedirectStr);
                this.btnCheck.Attributes.Add("onclick", showPopWinString);


                if (nodeInfo.Additional.IsUseTrial)
                {
                    this.phDelete.Visible = true;
                    this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(this.redirectStr + "&Delete=True"), "ContentIDCollection", "ContentIDCollection", "请选择需要删除的试用申请！", "此操作将删除所选试用申请，确认吗？"));
                }
                else
                {
                    this.phDelete.Visible = false;
                    this.alertsID.Text = "当前栏目未启用试用功能，试用申请功能不可用！";
                }
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int id = TranslateUtils.EvalInt(e.Item.DataItem, TrialApplyAttribute.TAID);
                int nodeID = TranslateUtils.EvalInt(e.Item.DataItem, TrialApplyAttribute.NodeID);
                int contentID = TranslateUtils.EvalInt(e.Item.DataItem, TrialApplyAttribute.ContentID);
                string userName = TranslateUtils.EvalString(e.Item.DataItem, TrialApplyAttribute.UserName);
                string applystatus = TranslateUtils.EvalString(e.Item.DataItem, TrialApplyAttribute.ApplyStatus);
                bool isChecked = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, TrialApplyAttribute.IsChecked));
                bool isReport = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, TrialApplyAttribute.IsReport));
                string ipAddress = TranslateUtils.EvalString(e.Item.DataItem, TrialApplyAttribute.IPAddress);
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, TrialApplyAttribute.AddDate);
                string name = TranslateUtils.EvalString(e.Item.DataItem, TrialApplyAttribute.Name);
                string phone = TranslateUtils.EvalString(e.Item.DataItem, TrialApplyAttribute.Phone);

                Literal ltlPageUrl = (Literal)e.Item.FindControl("ltlPageUrl");
                Literal ltlAddDate = (Literal)e.Item.FindControl("ltlAddDate");
                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
                Literal ltlIsChecked = (Literal)e.Item.FindControl("ltlIsChecked");

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
                    string showchekPopWinString = Modal.TrialApplyChecked.GetOneOpenWindowString(base.PublishmentSystemID, this.nodeID, contentID, id, this.RedirectStr);
                    ltlIsChecked.Text = string.Format("&nbsp;<a href=\"javascript:;\" onclick=\"{0}\"><span style='color:#ff0000;text-decoration:none'>未审核({1})</span></a>", showchekPopWinString, EFunctionStatusTypeUtils.GetText(EFunctionStatusTypeUtils.GetEnumType(applystatus)));
                }
                else
                    ltlIsChecked.Text = string.Format("<span style='color:green;text-decoration:none'>{0}</span>", EFunctionStatusTypeUtils.GetText(EFunctionStatusTypeUtils.GetEnumType(applystatus)));

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
                string showPopWinString = Modal.FunctionStyleValueShow.GetOpenWindowStringToTrialApply(base.PublishmentSystemID, this.nodeID, contentID, id);
                ltlEditUrl.Text = string.Format("<a href=\"javascript:;\" onclick=\"{0}\">查看</a>&nbsp;&nbsp;", showPopWinString);
                if (isReport)
                {
                    showPopWinString = Modal.FunctionStyleValueShow.GetOpenWindowStringToTrialReport(base.PublishmentSystemID, this.nodeID, contentID, id);
                    ltlEditUrl.Text += string.Format("&nbsp;<a href=\"javascript:;\" onclick=\"{0}\">试用报告</a>", showPopWinString);

                }
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
                    redirectStr = string.Format("background_trialApplyMainContent.aspx?PublishmentSystemID={0}&NodeID={1}&PageNum={2}&SearchDate={3}&IsChecked={4}&IsSearchChildren={5}", base.PublishmentSystemID, this.nodeID, this.PageNum.SelectedValue, this.SearchDate.SelectedValue, this.IsChecked.SelectedValue, this.IsSearchChildren.Checked);
                }
                return this.redirectStr;
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_trialApplyMainContent.aspx?PublishmentSystemID={0}&NodeID={1}", publishmentSystemID, nodeID));
        }
    }
}
