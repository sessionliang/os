using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Web.UI;

using System.Web.UI.HtmlControls;
using System.Text;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundApplyToDetailBasePage : BackgroundBasePage
    {
        public Literal ltlProjectName;
        public Literal ltlTitle;
        public Literal ltlApplyID;
        public Literal ltlState;
        public Literal ltlPriority;
        public Literal ltlTypeID;

        public Literal ltlAddUserName;
        public Literal ltlAddDate;
        public Literal ltlAcceptUserName;
        public Literal ltlAcceptDate;
        public Literal ltlCheckUserName;
        public Literal ltlCheckDate;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        public Literal ltlExpectedDate;
        public Literal ltlStartDate;
        public Literal ltlEndDate;
        public Literal ltlDateLimit;
        public Literal ltlContent;
        public Literal ltlSummary;

        public Button btnSetting;
        public Button btnTranslate;

        public PlaceHolder phReply;
        public Literal ltlDepartmentAndUserName;
        public Literal ltlReplyAddDate;
        public Literal ltlReply;
        public Literal ltlReplyFileUrl;

        public TextBox tbReply;
        public HtmlInputFile htmlFileUrl;
        public TextBox tbSwitchToRemark;
        public HtmlControl divSwitchToUserName;
        public TextBox tbCommentRemark;

        public PlaceHolder phRemarks;
        public Repeater rptRemarks;
        public Repeater rptLogs;

        public Literal ltlScript;

        protected ApplyInfo applyInfo;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public string MyDepartment
        {
            get { return AdminManager.CurrrentDepartmentName; }
        }

        public string MyDisplayName
        {
            get { return AdminManager.DisplayName; }
        }

        protected override void OnInit(EventArgs e)
        {
            PageUtils.CheckRequestParameter("ApplyID");

            this.applyInfo = DataProvider.ApplyDAO.GetApplyInfo(TranslateUtils.ToInt(base.Request.QueryString["ApplyID"]));
            if (this.applyInfo == null)
            {
                base.Response.End();
                return;
            }
            base.ProjectID = this.applyInfo.ProjectID;

            if (!IsPostBack)
            {
                this.ltlProjectName.Text = base.ProjectInfo.ProjectName;
                this.ltlTitle.Text = applyInfo.Title;
                this.ltlApplyID.Text = applyInfo.ID.ToString();
                this.ltlState.Text = EApplyStateUtils.GetText(applyInfo.State);
                this.ltlPriority.Text = ApplyManager.GetPriorityText(applyInfo.Priority);
                this.ltlTypeID.Text = ProjectManager.GetTypeName(applyInfo.TypeID);

                this.ltlAddUserName.Text = AdminManager.GetDisplayName(applyInfo.AddUserName, true);
                this.ltlAddDate.Text = DateUtils.GetDateAndTimeString(applyInfo.AddDate);
                if (this.ltlAcceptUserName != null && !string.IsNullOrEmpty(applyInfo.AcceptUserName))
                {
                    this.ltlAcceptUserName.Text = AdminManager.GetDisplayName(applyInfo.AcceptUserName, true);
                    this.ltlAcceptDate.Text = DateUtils.GetDateAndTimeString(applyInfo.AcceptDate);
                }
                if (this.ltlCheckUserName != null && !string.IsNullOrEmpty(applyInfo.CheckUserName))
                {
                    this.ltlCheckUserName.Text = AdminManager.GetDisplayName(applyInfo.CheckUserName, true);
                    this.ltlCheckDate.Text = DateUtils.GetDateAndTimeString(applyInfo.CheckDate);
                }
                this.ltlDepartmentName.Text = DepartmentManager.GetDepartmentName(applyInfo.DepartmentID);
                if (this.ltlUserName != null)
                {
                    this.ltlUserName.Text = AdminManager.GetDisplayName(applyInfo.UserName, true);
                }
                
                this.ltlExpectedDate.Text = DateUtils.GetDateString(applyInfo.ExpectedDate);
                if (this.ltlStartDate != null)
                {
                    this.ltlStartDate.Text = DateUtils.GetDateAndTimeString(applyInfo.StartDate);
                }
                this.ltlEndDate.Text = DateUtils.GetDateString(applyInfo.EndDate);
                this.ltlDateLimit.Text = ApplyManager.GetDateLimit(base.ProjectInfo, applyInfo);
                
                this.ltlContent.Text = applyInfo.Content;
                this.ltlSummary.Text = applyInfo.Summary;

                if (this.btnSetting != null)
                {
                    this.btnSetting.Attributes.Add("onclick", Modal.ApplySetting.GetShowPopWinString(this.applyInfo.ProjectID, this.applyInfo.ID));
                }
                if (this.btnTranslate != null)
                {
                    this.btnTranslate.Attributes.Add("onclick", Modal.ApplyTranslate.GetShowPopWinString(this.applyInfo.ProjectID, this.applyInfo.ID));
                }

                if (this.phReply != null)
                {
                    if (this.applyInfo.State == EApplyState.Denied || this.applyInfo.State == EApplyState.Replied || this.applyInfo.State == EApplyState.Redo || this.applyInfo.State == EApplyState.Checked)
                    {
                        ReplyInfo replyInfo = DataProvider.ReplyDAO.GetReplyInfoByApplyID(this.applyInfo.ID);
                        if (replyInfo != null)
                        {
                            this.phReply.Visible = true;
                            this.ltlDepartmentAndUserName.Text = AdminManager.GetDisplayName(replyInfo.UserName, true);
                            this.ltlReplyAddDate.Text = DateUtils.GetDateAndTimeString(replyInfo.AddDate);
                            this.ltlReply.Text = replyInfo.Reply;
                            this.ltlReplyFileUrl.Text = replyInfo.FileUrl;
                        }
                    }
                }

                if (this.divSwitchToUserName != null)
                {
                    string scriptName = "switchToUserName";
                    this.divSwitchToUserName.Attributes.Add("onclick", Modal.UserNameSelect.GetShowPopWinString(this.applyInfo.DepartmentID, scriptName));
                    StringBuilder scriptBuilder = new StringBuilder();
                    if (!string.IsNullOrEmpty(applyInfo.UserName))
                    {
                        string displayName = AdminManager.GetDisplayName(applyInfo.UserName, true);
                        scriptBuilder.AppendFormat(@"<script>{0}('{1}', '{2}');</script>", scriptName, displayName, applyInfo.UserName);
                    }
                    this.ltlScript.Text += scriptBuilder.ToString();
                }

                this.rptRemarks.DataSource = DataProvider.RemarkDAO.GetDataSourceByApplyID(this.applyInfo.ID);
                this.rptRemarks.ItemDataBound += new RepeaterItemEventHandler(rptRemarks_ItemDataBound);
                this.rptRemarks.DataBind();

                if (this.rptLogs != null)
                {
                    this.rptLogs.DataSource = DataProvider.ProjectLogDAO.GetDataSourceByApplyID(this.applyInfo.ID);
                    this.rptLogs.ItemDataBound += new RepeaterItemEventHandler(rptLogs_ItemDataBound);
                    this.rptLogs.DataBind();
                }
            }

            base.OnInit(e);
        }

        public void Reply_OnClick(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbReply.Text))
            {
                base.FailMessage("办理失败，必须填写办理结果");
                return;
            }
            try
            {
                DataProvider.ReplyDAO.DeleteByApplyID(this.applyInfo.ID);
                string fileUrl = string.Empty;
                ReplyInfo replyInfo = new ReplyInfo(0, this.applyInfo.ID, this.tbReply.Text, fileUrl, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.ReplyDAO.Insert(replyInfo);

                ApplyManager.Log(this.applyInfo.ID, EProjectLogType.Reply);

                this.applyInfo.DepartmentID = AdminManager.Current.DepartmentID;
                this.applyInfo.UserName = AdminManager.Current.UserName;
                this.applyInfo.EndDate = DateTime.Now;
                this.applyInfo.State = EApplyState.Replied;
                DataProvider.ApplyDAO.Update(this.applyInfo);

                base.SuccessMessage("办理成功");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        public void SwitchTo_OnClick(object sender, System.EventArgs e)
        {
            string switchToUserName = base.Request.Form["switchToUserName"];
            if (string.IsNullOrEmpty(switchToUserName))
            {
                base.FailMessage("转办失败，必须选择转办人员");
                return;
            }
            try
            {
                this.applyInfo.DepartmentID = AdminManager.GetDepartmentID(switchToUserName);
                this.applyInfo.UserName = switchToUserName;
                DataProvider.ApplyDAO.Update(this.applyInfo);

                RemarkInfo remarkInfo = new RemarkInfo(0, this.applyInfo.ID, ERemarkType.SwitchTo, this.tbSwitchToRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.RemarkDAO.Insert(remarkInfo);

                ApplyManager.LogSwitchTo(this.applyInfo.ID, switchToUserName);

                base.SuccessMessage("办件转办成功");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        public void Comment_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.tbCommentRemark.Text))
                {
                    base.FailMessage("批注失败，必须填写意见");
                    return;
                }

                RemarkInfo remarkInfo = new RemarkInfo(0, this.applyInfo.ID, ERemarkType.Comment, this.tbCommentRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.RemarkDAO.Insert(remarkInfo);

                ApplyManager.Log(this.applyInfo.ID, EProjectLogType.Comment);

                base.SuccessMessage("办件批注成功");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        void rptRemarks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlRemarkType = e.Item.FindControl("ltlRemarkType") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlDepartmentAndUserName = e.Item.FindControl("ltlDepartmentAndUserName") as Literal;
                Literal ltlRemark = e.Item.FindControl("ltlRemark") as Literal;

                int departmentID = TranslateUtils.EvalInt(e.Item.DataItem, "DepartmentID");
                string userName = TranslateUtils.EvalString(e.Item.DataItem, "UserName");
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate");
                ERemarkType remarkType = ERemarkTypeUtils.GetEnumType(TranslateUtils.EvalString(e.Item.DataItem, "RemarkType"));
                string remark = TranslateUtils.EvalString(e.Item.DataItem, "Remark");

                if (string.IsNullOrEmpty(remark))
                {
                    e.Item.Visible = false;
                }
                else
                {
                    this.phRemarks.Visible = true;
                    ltlRemarkType.Text = ERemarkTypeUtils.GetText(remarkType);
                    ltlAddDate.Text = DateUtils.GetDateAndTimeString(addDate);
                    ltlDepartmentAndUserName.Text = string.Format("{0}({1})", DepartmentManager.GetDepartmentName(departmentID), userName);
                    ltlRemark.Text = remark;
                }
            }
        }

        void rptLogs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlDepartment = e.Item.FindControl("ltlDepartment") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlSummary = e.Item.FindControl("ltlSummary") as Literal;

                int departmentID = TranslateUtils.EvalInt(e.Item.DataItem, "DepartmentID");
                string userName = TranslateUtils.EvalString(e.Item.DataItem, "UserName");
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate");
                string summary = TranslateUtils.EvalString(e.Item.DataItem, "Summary");

                if (departmentID > 0)
                {
                    ltlDepartment.Text = DepartmentManager.GetDepartmentName(departmentID);
                }
                ltlUserName.Text = AdminManager.GetDisplayName(userName, true);
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(addDate);
                ltlSummary.Text = summary;
            }
        }
    }
}
