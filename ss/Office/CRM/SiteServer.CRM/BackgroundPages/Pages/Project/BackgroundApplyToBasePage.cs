using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CRM.Core;
using SiteServer.CRM.Model;
using System.Web.UI;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages
{
    public class BackgroundApplyToBasePage : BackgroundBasePage
    {
        public HyperLink hlAccept;
        public HyperLink hlDeny;
        public HyperLink hlCheck;
        public HyperLink hlRedo;
        public HyperLink hlSetting;
        public HyperLink hlSwitchTo;
        public HyperLink hlTranslate;
        public HyperLink hlComment;
        public PlaceHolder phDelete;
        public HyperLink hlDelete;

        public Repeater rptContents;
        public SqlPager spContents;

        protected int projectID;
        private Hashtable typeHashtable = new Hashtable();

        protected override void OnInit(EventArgs e)
        {
            this.projectID = TranslateUtils.ToInt(base.Request.QueryString["ProjectID"]);

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.QueryString["IDCollection"]);
                if (arraylist.Count > 0)
                {
                    try
                    {
                        DataProvider.ApplyDAO.Delete(arraylist);
                        base.SuccessMessage("删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["Deny"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToIntArrayList(Request.QueryString["IDCollection"]);
                foreach (int applyID in arraylist)
                {
                    EApplyState state = DataProvider.ApplyDAO.GetState(applyID);
                    if (state == EApplyState.New || state == EApplyState.Accepted)
                    {
                        ApplyManager.Log(applyID, EProjectLogType.Deny);
                        DataProvider.ApplyDAO.UpdateState(applyID, EApplyState.Denied);
                    }
                }
                base.SuccessMessage("拒绝受理办件成功！");
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["Check"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToIntArrayList(Request.QueryString["IDCollection"]);
                foreach (int applyID in arraylist)
                {
                    EApplyState state = DataProvider.ApplyDAO.GetState(applyID);
                    if (state == EApplyState.Replied)
                    {
                        ApplyManager.Log(applyID, EProjectLogType.Check);
                        ApplyInfo applyInfo = DataProvider.ApplyDAO.GetApplyInfo(applyID);
                        applyInfo.CheckUserName = AdminManager.Current.UserName;
                        applyInfo.CheckDate = DateTime.Now;
                        applyInfo.State = EApplyState.Checked;
                        applyInfo.EndDate = DateTime.Now;
                        DataProvider.ApplyDAO.Update(applyInfo);
                    }
                }
                base.SuccessMessage("审核办件成功！");
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = this.GetSelectString();
            this.spContents.SortField = DataProvider.ApplyDAO.GetSortFieldName();
            this.spContents.SortMode = this.GetSortMode();
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                this.spContents.DataBind();

                if (this.hlAccept != null)
                {
                    this.hlAccept.Attributes.Add("onclick", Modal.ApplyAccept.GetShowPopWinString(base.PublishmentSystemID));
                }
                if (this.hlDeny != null)
                {
                    this.hlDeny.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Deny=True", "IDCollection", "IDCollection", "请选择需要拒绝的办件！", "此操作将拒绝受理所选办件，确定吗？"));
                }
                if (this.hlCheck != null)
                {
                    this.hlCheck.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Check=True", "IDCollection", "IDCollection", "请选择需要审核的办件！", "此操作将审核所选办件，确定吗？"));
                }
                if (this.hlRedo != null)
                {
                    this.hlRedo.Attributes.Add("onclick", Modal.ApplyRedo.GetShowPopWinString(base.PublishmentSystemID));
                }
                if (this.hlSetting != null)
                {
                    this.hlSetting.Attributes.Add("onclick", Modal.ApplySetting.GetShowPopWinString(base.PublishmentSystemID));
                }
                if (this.hlSwitchTo != null)
                {
                    this.hlSwitchTo.Attributes.Add("onclick", Modal.ApplySwitchTo.GetShowPopWinString(base.PublishmentSystemID));
                }
                if (this.hlTranslate != null)
                {
                    this.hlTranslate.Attributes.Add("onclick", Modal.ApplyTranslate.GetShowPopWinString(base.PublishmentSystemID));
                }
                if (this.hlComment != null)
                {
                    this.hlComment.Attributes.Add("onclick", Modal.ApplyComment.GetShowPopWinString(base.PublishmentSystemID));
                }
                if (this.phDelete != null)
                {
                    this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的办件！", "此操作将删除所选办件，确定吗？"));
                }
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ApplyInfo applyInfo = new ApplyInfo(e.Item.DataItem);
                //if (base.ProjectID <= 0)
                //{
                //    applyInfo.Title = string.Format("【{0}】{1}", DataProvider.ProjectDAO.GetProjectName(applyInfo.ProjectID), applyInfo.Title);
                //}

                Literal ltlTr = e.Item.FindControl("ltlTr") as Literal;
                Literal ltlApplyID = e.Item.FindControl("ltlApplyID") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlType = e.Item.FindControl("ltlType") as Literal;
                Literal ltlDepartment = e.Item.FindControl("ltlDepartment") as Literal;
                Literal ltlAddUserName = e.Item.FindControl("ltlAddUserName") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlLimit = e.Item.FindControl("ltlLimit") as Literal;
                Literal ltlState = e.Item.FindControl("ltlState") as Literal;
                Literal ltlFlowUrl = e.Item.FindControl("ltlFlowUrl") as Literal;
                Literal ltlViewUrl = e.Item.FindControl("ltlViewUrl") as Literal;

                ELimitType limitType = ELimitType.Normal;
                ltlTr.Text = @"<tr class=""tdbg"" style=""height:25px"">";
                if (applyInfo.State == EApplyState.Denied || applyInfo.State == EApplyState.Checked)
                {
                    ltlTr.Text = @"<tr class=""success"">";
                }
                else
                {
                    limitType = ApplyManager.GetLimitType(applyInfo);
                    if (limitType == ELimitType.Alert)
                    {
                        ltlTr.Text = @"<tr class=""tdbg"" style=""height:25px;background-color:#FF9"">";
                    }
                    else if (limitType == ELimitType.Yellow)
                    {
                        ltlTr.Text = @"<tr class=""tdbg"" style=""height:25px;background-color:#FE8D96"">";
                    }
                    else if (limitType == ELimitType.Red)
                    {
                        ltlTr.Text = @"<tr class=""tdbg"" style=""height:25px;background-color:#FE5461"">";
                    }
                    else if (limitType == ELimitType.Green)
                    {
                        ltlTr.Text = @"<tr class=""tdbg"" style=""height:25px;background-color:#A4FFA4"">";
                    }
                }

                ltlApplyID.Text = applyInfo.ID.ToString();

                if (applyInfo.State == EApplyState.Accepted || applyInfo.State == EApplyState.Redo)
                {
                    ltlTitle.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", BackgroundApplyToReplyDetail.GetRedirectUrl(applyInfo.ID), applyInfo.Title);
                }
                else if (applyInfo.State == EApplyState.Replied)
                {
                    ltlTitle.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", BackgroundApplyToCheckDetail.GetRedirectUrl(applyInfo.ID), applyInfo.Title);
                }
                else if (applyInfo.State == EApplyState.Denied || applyInfo.State == EApplyState.New)
                {
                    ltlTitle.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", BackgroundApplyToAcceptDetail.GetRedirectUrl(applyInfo.ID), applyInfo.Title);
                }
                else if (applyInfo.State == EApplyState.Checked)
                {
                    ltlTitle.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", BackgroundApplyToDoneDetail.GetRedirectUrl(applyInfo.ID), applyInfo.Title);
                }

                ltlAddDate.Text = DateUtils.GetDateAndTimeString(applyInfo.AddDate);
                ltlType.Text = ProjectManager.GetTypeName(applyInfo.TypeID, this.typeHashtable);
                
                if (ltlDepartment != null)
                {
                    ltlDepartment.Text = DepartmentManager.GetDepartmentName(applyInfo.DepartmentID);
                }
                ltlAddUserName.Text = AdminManager.GetDisplayName(applyInfo.AddUserName, true);
                if (ltlUserName != null)
                {
                    ltlUserName.Text = AdminManager.GetDisplayName(applyInfo.UserName, true);
                }
                ltlLimit.Text = ApplyManager.GetDateLimit(base.PublishmentSystemInfo, applyInfo);
                ltlState.Text = EApplyStateUtils.GetText(applyInfo.State);
                if (applyInfo.State == EApplyState.New)
                {
                    ltlState.Text = string.Format("<span style='color:red'>{0}</span>", ltlState.Text);
                }
                else if (applyInfo.State == EApplyState.Redo)
                {
                    ltlState.Text = string.Format("<span style='color:red'>{0}</span>", ltlState.Text);
                }
                ltlFlowUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">轨迹</a>", Modal.ApplyFlow.GetShowPopWinString(base.PublishmentSystemID, applyInfo.ID));
                ltlViewUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">查看</a>", Modal.ApplyView.GetShowPopWinString(base.PublishmentSystemID, applyInfo.ID));
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
