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

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovInteractListBasePage : BackgroundGovInteractBasePage
    {
        public PlaceHolder phAccept;
        public HyperLink hlAccept;
        public HyperLink hlDeny;
        public PlaceHolder phCheck;
        public HyperLink hlCheck;
        public HyperLink hlRedo;
        public PlaceHolder phSwitchToTranslate;
        public HyperLink hlSwitchTo;
        public HyperLink hlTranslate;
        public PlaceHolder phComment;
        public HyperLink hlComment;
        public PlaceHolder phDelete;
        public HyperLink hlDelete;
        public HyperLink hlExport;

        public Literal ltlTotalCount;

        public Repeater rptContents;
        public SqlPager spContents;

        protected int nodeID;
        private bool isPermissionReply = false;
        private bool isPermissionEdit = false;

        protected override void OnInit(EventArgs e)
        {
            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID");
            this.nodeID = TranslateUtils.ToInt(base.Request.QueryString["NodeID"]);

            this.isPermissionReply = GovInteractManager.IsPermission(base.PublishmentSystemID, this.nodeID, AppManager.CMS.Permission.GovInteract.GovInteractReply);
            this.isPermissionEdit = GovInteractManager.IsPermission(base.PublishmentSystemID, this.nodeID, AppManager.CMS.Permission.GovInteract.GovInteractEdit);

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToIntArrayList(Request.QueryString["IDCollection"]);
                if (arraylist.Count > 0)
                {
                    try
                    {
                        DataProvider.ContentDAO.DeleteContents(base.PublishmentSystemID, base.PublishmentSystemInfo.AuxiliaryTableForGovInteract, arraylist, this.nodeID);
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
                foreach (int contentID in arraylist)
                {
                    GovInteractContentInfo contentInfo = DataProvider.GovInteractContentDAO.GetContentInfo(base.PublishmentSystemInfo, contentID);
                    if (contentInfo.State == EGovInteractState.New || contentInfo.State == EGovInteractState.Denied)
                    {
                        GovInteractApplyManager.Log(base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, EGovInteractLogType.Accept);
                        DataProvider.GovInteractContentDAO.UpdateState(base.PublishmentSystemInfo, contentInfo.ID, EGovInteractState.Accepted);
                    }
                }
                base.SuccessMessage("受理申请成功！");
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["Deny"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToIntArrayList(Request.QueryString["IDCollection"]);
                foreach (int contentID in arraylist)
                {
                    GovInteractContentInfo contentInfo = DataProvider.GovInteractContentDAO.GetContentInfo(base.PublishmentSystemInfo, contentID);
                    if (contentInfo.State == EGovInteractState.New || contentInfo.State == EGovInteractState.Accepted)
                    {
                        GovInteractApplyManager.Log(base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, EGovInteractLogType.Deny);
                        DataProvider.GovInteractContentDAO.UpdateState(base.PublishmentSystemInfo, contentID, EGovInteractState.Denied);
                    }
                }
                base.SuccessMessage("拒绝受理申请成功！");
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["Check"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToIntArrayList(Request.QueryString["IDCollection"]);
                foreach (int contentID in arraylist)
                {
                    GovInteractContentInfo contentInfo = DataProvider.GovInteractContentDAO.GetContentInfo(base.PublishmentSystemInfo, contentID);
                    if (contentInfo.State == EGovInteractState.Replied)
                    {
                        GovInteractApplyManager.Log(base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, EGovInteractLogType.Check);
                        DataProvider.GovInteractContentDAO.UpdateState(base.PublishmentSystemInfo, contentID, EGovInteractState.Checked);
                    }
                }
                base.SuccessMessage("审核申请成功！");
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = this.GetSelectString();
            this.spContents.SortField = ContentAttribute.Taxis;
            this.spContents.SortMode = this.GetSortMode();
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                this.spContents.DataBind();
                this.ltlTotalCount.Text = this.spContents.TotalCount.ToString();

                if (this.phAccept != null)
                {
                    this.phAccept.Visible = GovInteractManager.IsPermission(base.PublishmentSystemID, this.nodeID, AppManager.CMS.Permission.GovInteract.GovInteractAccept);
                    if (this.hlAccept != null)
                    {
                        this.hlAccept.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Accept=True", "IDCollection", "IDCollection", "请选择需要受理的申请！", "此操作将受理所选申请，确定吗？"));
                    }
                    if (this.hlDeny != null)
                    {
                        this.hlDeny.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Deny=True", "IDCollection", "IDCollection", "请选择需要拒绝的申请！", "此操作将拒绝受理所选申请，确定吗？"));
                    }
                }
                if (this.phCheck != null)
                {
                    this.phCheck.Visible = GovInteractManager.IsPermission(base.PublishmentSystemID, this.nodeID, AppManager.CMS.Permission.GovInteract.GovInteractCheck);
                    if (this.hlCheck != null)
                    {
                        this.hlCheck.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Check=True", "IDCollection", "IDCollection", "请选择需要审核的申请！", "此操作将审核所选申请，确定吗？"));
                    }
                    if (this.hlRedo != null)
                    {
                        this.hlRedo.Attributes.Add("onclick", Modal.GovInteractApplyRedo.GetOpenWindowString(base.PublishmentSystemID));
                    }
                }
                if (this.phSwitchToTranslate != null)
                {
                    this.phSwitchToTranslate.Visible = GovInteractManager.IsPermission(base.PublishmentSystemID, this.nodeID, AppManager.CMS.Permission.GovInteract.GovInteractSwitchToTranslate);
                    if (this.hlSwitchTo != null)
                    {
                        this.hlSwitchTo.Attributes.Add("onclick", Modal.GovInteractApplySwitchTo.GetOpenWindowString(base.PublishmentSystemID, this.nodeID));
                    }
                    if (this.hlTranslate != null)
                    {
                        this.hlTranslate.Attributes.Add("onclick", Modal.GovInteractApplyTranslate.GetOpenWindowString(base.PublishmentSystemID, this.nodeID));
                    }
                }
                if (this.phComment != null)
                {
                    this.phComment.Visible = GovInteractManager.IsPermission(base.PublishmentSystemID, this.nodeID, AppManager.CMS.Permission.GovInteract.GovInteractComment);
                    this.hlComment.Attributes.Add("onclick", Modal.GovInteractApplyComment.GetOpenWindowString(base.PublishmentSystemID));
                }
                if (this.phDelete != null)
                {
                    this.phDelete.Visible = GovInteractManager.IsPermission(base.PublishmentSystemID, this.nodeID, AppManager.CMS.Permission.GovInteract.GovInteractDelete) && base.PublishmentSystemInfo.Additional.GovInteractApplyIsDeleteAllowed;
                    this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的申请！", "此操作将删除所选申请，确定吗？"));
                }
                if (this.hlExport != null)
                {
                    this.hlExport.Attributes.Add("onclick", PageUtility.ModalSTL.ContentExport_GetOpenWindowString(base.PublishmentSystemID, this.nodeID));
                }
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GovInteractContentInfo contentInfo = new GovInteractContentInfo(e.Item.DataItem);

                Literal ltlTr = e.Item.FindControl("ltlTr") as Literal;
                Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlRemark = e.Item.FindControl("ltlRemark") as Literal;
                Literal ltlDepartment = e.Item.FindControl("ltlDepartment") as Literal;
                Literal ltlLimit = e.Item.FindControl("ltlLimit") as Literal;
                Literal ltlState = e.Item.FindControl("ltlState") as Literal;
                Literal ltlFlowUrl = e.Item.FindControl("ltlFlowUrl") as Literal;
                Literal ltlViewUrl = e.Item.FindControl("ltlViewUrl") as Literal;
                Literal ltlReplyUrl = e.Item.FindControl("ltlReplyUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                EGovInteractLimitType limitType = EGovInteractLimitType.Normal;
                ltlTr.Text = @"<tr>";
                if (contentInfo.State == EGovInteractState.Denied || contentInfo.State == EGovInteractState.Checked)
                {
                    ltlTr.Text = @"<tr class=""success"">";
                }
                else
                {
                    limitType = GovInteractApplyManager.GetLimitType(base.PublishmentSystemInfo, contentInfo);
                    if (limitType == EGovInteractLimitType.Alert)
                    {
                        ltlTr.Text = @"<tr class=""info"">";
                    }
                    else if (limitType == EGovInteractLimitType.Yellow)
                    {
                        ltlTr.Text = @"<tr class=""warning"">";
                    }
                    else if (limitType == EGovInteractLimitType.Red)
                    {
                        ltlTr.Text = @"<tr class=""error"">";
                    }
                }

                ltlID.Text = contentInfo.ID.ToString();

                string title = contentInfo.Title;
                if (string.IsNullOrEmpty(title))
                {
                    title = StringUtils.MaxLengthText(contentInfo.Content, 30);
                }
                if (string.IsNullOrEmpty(title))
                {
                    title = contentInfo.QueryCode;
                }

                string target = base.PublishmentSystemInfo.Additional.GovInteractApplyIsOpenWindow ? @"target=""_blank""" : string.Empty;
                if (contentInfo.State == EGovInteractState.Accepted || contentInfo.State == EGovInteractState.Redo)
                {
                    ltlTitle.Text = string.Format(@"<a href=""{0}"" {1}>{2}</a>", BackgroundGovInteractPageReply.GetRedirectUrl(base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, this.PageUrl), target, title);
                }
                else if (contentInfo.State == EGovInteractState.Checked || contentInfo.State == EGovInteractState.Replied)
                {
                    ltlTitle.Text = string.Format(@"<a href=""{0}"" {1}>{2}</a>", BackgroundGovInteractPageCheck.GetRedirectUrl(base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, this.PageUrl), target, title);
                }
                else if (contentInfo.State == EGovInteractState.Denied || contentInfo.State == EGovInteractState.New)
                {
                    ltlTitle.Text = string.Format(@"<a href=""{0}"" {1}>{2}</a>", BackgroundGovInteractPageAccept.GetRedirectUrl(base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, this.PageUrl), target, title);
                }
                string departmentName = DepartmentManager.GetDepartmentName(contentInfo.DepartmentID);
                if (contentInfo.DepartmentID > 0 && departmentName != contentInfo.DepartmentName)
                {
                    ltlTitle.Text += "<span style='color:red'>【转办】</span>";
                }
                else if (TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(GovInteractContentAttribute.TranslateFromNodeID)) > 0)
                {
                    ltlTitle.Text += "<span style='color:red'>【转移】</span>";
                }
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(contentInfo.AddDate);
                ltlRemark.Text = GovInteractApplyManager.GetApplyRemark(base.PublishmentSystemID, contentInfo.ID);
                ltlDepartment.Text = departmentName;
                ltlLimit.Text = EGovInteractLimitTypeUtils.GetText(limitType);
                ltlState.Text = EGovInteractStateUtils.GetText(contentInfo.State);
                if (contentInfo.State == EGovInteractState.New)
                {
                    ltlState.Text = string.Format("<span style='color:red'>{0}</span>", ltlState.Text);
                }
                else if (contentInfo.State == EGovInteractState.Redo)
                {
                    ltlState.Text = string.Format("<span style='color:red'>{0}</span>", ltlState.Text);
                }
                ltlFlowUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">轨迹</a>", Modal.GovInteractApplyFlow.GetOpenWindowString(base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID));
                ltlViewUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">查看</a>", Modal.GovInteractApplyView.GetOpenWindowString(base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID));
                if (this.isPermissionReply)
                {
                    ltlReplyUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">办理</a>", Modal.GovInteractApplyReply.GetOpenWindowString(base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID));
                }
                if (this.isPermissionEdit)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, contentInfo.NodeID);
                    ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", WebUtils.GetContentAddEditUrl(base.PublishmentSystemID, nodeInfo, contentInfo.ID, this.PageUrl));
                }
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
