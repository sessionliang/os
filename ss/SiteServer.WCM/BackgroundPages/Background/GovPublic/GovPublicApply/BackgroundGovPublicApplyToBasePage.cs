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
using System.Web.UI;


namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicApplyToBasePage : BackgroundGovPublicBasePage
    {
        public HyperLink hlAccept;
        public HyperLink hlDeny;
        public HyperLink hlCheck;
        public HyperLink hlRedo;
        public HyperLink hlSwitchTo;
        public HyperLink hlComment;
        public PlaceHolder phDelete;
        public HyperLink hlDelete;
        public Literal ltlTotalCount;

        public Repeater rptContents;
        public SqlPager spContents;

        protected override void OnInit(EventArgs e)
        {
            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.QueryString["IDCollection"]);
                if (arraylist.Count > 0)
                {
                    try
                    {
                        DataProvider.GovPublicApplyDAO.Delete(arraylist);
                        StringUtility.AddLog(base.PublishmentSystemID, "删除申请");
                        base.SuccessMessage("删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["Accept"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToIntArrayList(Request.QueryString["IDCollection"]);
                foreach (int applyID in arraylist)
                {
                    EGovPublicApplyState state = DataProvider.GovPublicApplyDAO.GetState(applyID);
                    if (state == EGovPublicApplyState.New || state == EGovPublicApplyState.Denied)
                    {
                        GovPublicApplyManager.Log(base.PublishmentSystemID, applyID, EGovPublicApplyLogType.Accept);
                        DataProvider.GovPublicApplyDAO.UpdateState(applyID, EGovPublicApplyState.Accepted);
                    }
                }
                base.SuccessMessage("受理申请成功！");
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["Deny"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToIntArrayList(Request.QueryString["IDCollection"]);
                foreach (int applyID in arraylist)
                {
                    EGovPublicApplyState state = DataProvider.GovPublicApplyDAO.GetState(applyID);
                    if (state == EGovPublicApplyState.New || state == EGovPublicApplyState.Accepted)
                    {
                        GovPublicApplyManager.Log(base.PublishmentSystemID, applyID, EGovPublicApplyLogType.Deny);
                        DataProvider.GovPublicApplyDAO.UpdateState(applyID, EGovPublicApplyState.Denied);
                    }
                }
                base.SuccessMessage("拒绝受理申请成功！");
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["Check"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToIntArrayList(Request.QueryString["IDCollection"]);
                foreach (int applyID in arraylist)
                {
                    EGovPublicApplyState state = DataProvider.GovPublicApplyDAO.GetState(applyID);
                    if (state == EGovPublicApplyState.Replied)
                    {
                        GovPublicApplyManager.Log(base.PublishmentSystemID, applyID, EGovPublicApplyLogType.Check);
                        DataProvider.GovPublicApplyDAO.UpdateState(applyID, EGovPublicApplyState.Checked);
                    }
                }
                base.SuccessMessage("审核申请成功！");
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = this.GetSelectString();
            this.spContents.SortField = DataProvider.GovPublicApplyDAO.GetSortFieldName();
            this.spContents.SortMode = this.GetSortMode();
            this.rptContents.ItemDataBound +=new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                this.spContents.DataBind();
                this.ltlTotalCount.Text = this.spContents.TotalCount.ToString();

                if (this.hlAccept != null)
                {
                    this.hlAccept.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Accept=True", "IDCollection", "IDCollection", "请选择需要受理的申请！", "此操作将受理所选申请，确定吗？"));
                }
                if (this.hlDeny != null)
                {
                    this.hlDeny.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Deny=True", "IDCollection", "IDCollection", "请选择需要拒绝的申请！", "此操作将拒绝受理所选申请，确定吗？"));
                }
                if (this.hlCheck != null)
                {
                    this.hlCheck.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Check=True", "IDCollection", "IDCollection", "请选择需要审核的申请！", "此操作将审核所选申请，确定吗？"));
                }
                if (this.hlRedo != null)
                {
                    this.hlRedo.Attributes.Add("onclick", Modal.GovPublicApplyRedo.GetOpenWindowString(base.PublishmentSystemID));
                }
                if (this.hlSwitchTo != null)
                {
                    this.hlSwitchTo.Attributes.Add("onclick", Modal.GovPublicApplySwitchTo.GetOpenWindowString(base.PublishmentSystemID));
                }
                if (this.hlComment != null)
                {
                    this.hlComment.Attributes.Add("onclick", Modal.GovPublicApplyComment.GetOpenWindowString(base.PublishmentSystemID));
                }
                if (this.phDelete != null)
                {
                    this.phDelete.Visible = base.PublishmentSystemInfo.Additional.GovPublicApplyIsDeleteAllowed;
                    this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的申请！", "此操作将删除所选申请，确定吗？"));
                }
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GovPublicApplyInfo applyInfo = new GovPublicApplyInfo(e.Item.DataItem);

                Literal ltlTr = e.Item.FindControl("ltlTr") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlRemark = e.Item.FindControl("ltlRemark") as Literal;
                Literal ltlDepartment = e.Item.FindControl("ltlDepartment") as Literal;
                Literal ltlLimit = e.Item.FindControl("ltlLimit") as Literal;
                Literal ltlState = e.Item.FindControl("ltlState") as Literal;
                Literal ltlFlowUrl = e.Item.FindControl("ltlFlowUrl") as Literal;
                Literal ltlViewUrl = e.Item.FindControl("ltlViewUrl") as Literal;

                ltlTr.Text = @"<tr class=""tdbg"" style=""height:25px"">";
                EGovPublicApplyLimitType limitType = GovPublicApplyManager.GetLimitType(base.PublishmentSystemInfo, applyInfo);
                if (limitType == EGovPublicApplyLimitType.Alert)
                {
                    ltlTr.Text = @"<tr class=""tdbg"" style=""height:25px;background-color:#AAAAD5"">";
                }
                else if (limitType == EGovPublicApplyLimitType.Yellow)
                {
                    ltlTr.Text = @"<tr class=""tdbg"" style=""height:25px;background-color:#FF9"">";
                }
                else if (limitType == EGovPublicApplyLimitType.Red)
                {
                    ltlTr.Text = @"<tr class=""tdbg"" style=""height:25px;background-color:#FE5461"">";
                }

                if (applyInfo.State == EGovPublicApplyState.Accepted || applyInfo.State == EGovPublicApplyState.Redo)
                {
                    ltlTitle.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundGovPublicApplyToReplyDetail.GetRedirectUrl(base.PublishmentSystemID, applyInfo.ID, this.PageUrl), applyInfo.Title);
                }
                else if (applyInfo.State == EGovPublicApplyState.Checked || applyInfo.State == EGovPublicApplyState.Replied)
                {
                    ltlTitle.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundGovPublicApplyToCheckDetail.GetRedirectUrl(base.PublishmentSystemID, applyInfo.ID, this.PageUrl), applyInfo.Title);
                }
                else if (applyInfo.State == EGovPublicApplyState.Denied || applyInfo.State == EGovPublicApplyState.New)
                {
                    ltlTitle.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundGovPublicApplyToAcceptDetail.GetRedirectUrl(base.PublishmentSystemID, applyInfo.ID, this.PageUrl), applyInfo.Title);
                }
                string departmentName = DepartmentManager.GetDepartmentName(applyInfo.DepartmentID);
                if (applyInfo.DepartmentID > 0 && departmentName != applyInfo.DepartmentName)
                {
                    ltlTitle.Text += "<span style='color:red'>【转】</span>";
                }
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(applyInfo.AddDate);
                ltlRemark.Text = GovPublicApplyManager.GetApplyRemark(applyInfo.ID);
                ltlDepartment.Text = departmentName;
                ltlLimit.Text = EGovPublicApplyLimitTypeUtils.GetText(limitType);
                ltlState.Text = EGovPublicApplyStateUtils.GetText(applyInfo.State);
                if (applyInfo.State == EGovPublicApplyState.New)
                {
                    ltlState.Text = string.Format("<span style='color:red'>{0}</span>", ltlState.Text);
                }
                else if (applyInfo.State == EGovPublicApplyState.Redo)
                {
                    ltlState.Text = string.Format("<span style='color:red'>{0}</span>", ltlState.Text);
                }
                ltlFlowUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">轨迹</a>", Modal.GovPublicApplyFlow.GetOpenWindowString(base.PublishmentSystemID, applyInfo.ID));
                ltlViewUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">查看</a>", Modal.GovPublicApplyView.GetOpenWindowString(base.PublishmentSystemID, applyInfo.ID));
            }
        }

        protected virtual string GetSelectString() { return string.Empty;}

        protected virtual string PageUrl
        {
            get { return string.Empty; }
        }

        protected virtual SortMode GetSortMode()
        {
            return SortMode.DESC;
        }
    }
}
