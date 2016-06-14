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

using System.Web.UI.HtmlControls;
using System.Text;

namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicApplyToDetailBasePage : BackgroundGovPublicBasePage
    {
        public Literal ltlName;
        public Literal ltlState;
        public Literal ltlType;

        public PlaceHolder phCivic;
        public Literal ltlCivicName;
        public Literal ltlCivicOrganization;
        public Literal ltlCivicCardType;
        public Literal ltlCivicCardNo;
        public Literal ltlCivicPhone;
        public Literal ltlCivicPostCode;
        public Literal ltlCivicAddress;
        public Literal ltlCivicEmail;
        public Literal ltlCivicFax;

        public PlaceHolder phOrg;
        public Literal ltlOrgName;
        public Literal ltlOrgUnitCode;
        public Literal ltlOrgLegalPerson;
        public Literal ltlOrgLinkName;
        public Literal ltlOrgPhone;
        public Literal ltlOrgPostCode;
        public Literal ltlOrgAddress;
        public Literal ltlOrgEmail;
        public Literal ltlOrgFax;

        public Literal ltlQueryCode;
        public Literal ltlTitle;
        public Literal ltlContent;
        public Literal ltlPurpose;

        public Literal ltlIsApplyFree;
        public Literal ltlProvideType;
        public Literal ltlObtainType;

        public PlaceHolder phReply;
        public Literal ltlDepartmentAndUserName;
        public Literal ltlAddDate;
        public Literal ltlReply;
        public Literal ltlFileUrl;

        public TextBox tbReply;
        public HtmlInputFile htmlFileUrl;
        public TextBox tbSwitchToRemark;
        public HtmlControl divAddDepartment;
        public Literal ltlScript;
        public TextBox tbCommentRemark;

        public PlaceHolder phRemarks;
        public Repeater rptRemarks;
        public Repeater rptLogs;

        protected GovPublicApplyInfo applyInfo;
        private string returnUrl;

        public string MyDepartment
        {
            get { return AdminManager.CurrrentDepartmentName; }
        }

        public string MyDisplayName
        {
            get { return AdminManager.DisplayName; }
        }

        public string ApplyDepartment
        {
            get
            {
                if (!string.IsNullOrEmpty(this.applyInfo.DepartmentName))
                {
                    return this.applyInfo.DepartmentName;
                }
                return "<<未指定>>";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            PageUtils.CheckRequestParameter("PublishmentSystemID", "ApplyID", "ReturnUrl");

            this.applyInfo = DataProvider.GovPublicApplyDAO.GetApplyInfo(TranslateUtils.ToInt(base.Request.QueryString["ApplyID"]));
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);

            if (!IsPostBack)
            {
                if (!applyInfo.IsOrganization)
                {
                    this.ltlType.Text = "公民";
                    this.phCivic.Visible = true;
                    this.phOrg.Visible = false;
                    this.ltlCivicName.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.CivicName);
                    this.ltlCivicOrganization.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.CivicOrganization);
                    this.ltlCivicCardType.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.CivicCardType);
                    this.ltlCivicCardNo.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.CivicCardNo);
                    this.ltlCivicPhone.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.CivicPhone);
                    this.ltlCivicPostCode.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.CivicPostCode);
                    this.ltlCivicAddress.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.CivicAddress);
                    this.ltlCivicEmail.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.CivicEmail);
                    this.ltlCivicFax.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.CivicFax);

                    this.ltlName.Text = this.ltlCivicName.Text;
                }
                else
                {
                    this.ltlType.Text = "法人/其他组织";
                    this.phCivic.Visible = false;
                    this.phOrg.Visible = true;
                    this.ltlOrgName.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.OrgName);
                    this.ltlOrgUnitCode.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.OrgUnitCode);
                    this.ltlOrgLegalPerson.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.OrgLegalPerson);
                    this.ltlOrgLinkName.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.OrgLinkName);
                    this.ltlOrgPhone.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.OrgPhone);
                    this.ltlOrgPostCode.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.OrgPostCode);
                    this.ltlOrgAddress.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.OrgAddress);
                    this.ltlOrgEmail.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.OrgEmail);
                    this.ltlOrgFax.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.OrgFax);

                    this.ltlName.Text = this.ltlOrgName.Text;
                }

                this.ltlState.Text = EGovPublicApplyStateUtils.GetText(applyInfo.State);
                this.ltlQueryCode.Text = applyInfo.QueryCode;
                this.ltlTitle.Text = applyInfo.Title;
                this.ltlContent.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.Content);
                this.ltlPurpose.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.Purpose);
                this.ltlIsApplyFree.Text = TranslateUtils.ToBool(applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.Content)) ? "申请" : "不申请";
                this.ltlProvideType.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.ProvideType);
                this.ltlObtainType.Text = applyInfo.GetExtendedAttribute(GovPublicApplyAttribute.ObtainType);

                if (this.phReply != null)
                {
                    if (this.applyInfo.State == EGovPublicApplyState.Denied || this.applyInfo.State == EGovPublicApplyState.Replied || this.applyInfo.State == EGovPublicApplyState.Redo || this.applyInfo.State == EGovPublicApplyState.Checked)
                    {
                        GovPublicApplyReplyInfo replyInfo = DataProvider.GovPublicApplyReplyDAO.GetReplyInfoByApplyID(this.applyInfo.ID);
                        if (replyInfo != null)
                        {
                            this.phReply.Visible = true;
                            this.ltlDepartmentAndUserName.Text = string.Format("{0}({1})", DepartmentManager.GetDepartmentName(replyInfo.DepartmentID), replyInfo.UserName);
                            this.ltlAddDate.Text = DateUtils.GetDateAndTimeString(replyInfo.AddDate);
                            this.ltlReply.Text = replyInfo.Reply;
                            this.ltlFileUrl.Text = replyInfo.FileUrl;
                        }
                    }
                }

                if (this.divAddDepartment != null)
                {
                    this.divAddDepartment.Attributes.Add("onclick", Modal.GovPublicCategoryDepartmentSelect.GetOpenWindowString(base.PublishmentSystemID));
                    StringBuilder scriptBuilder = new StringBuilder();
                    if (applyInfo.DepartmentID > 0)
                    {
                        string departmentName = DepartmentManager.GetDepartmentName(applyInfo.DepartmentID);
                        scriptBuilder.AppendFormat(@"<script>showCategoryDepartment('{0}', '{1}');</script>", departmentName, applyInfo.DepartmentID);
                    }
                    this.ltlScript.Text = scriptBuilder.ToString();
                }

                this.rptRemarks.DataSource = DataProvider.GovPublicApplyRemarkDAO.GetDataSourceByApplyID(this.applyInfo.ID);
                this.rptRemarks.ItemDataBound += new RepeaterItemEventHandler(rptRemarks_ItemDataBound);
                this.rptRemarks.DataBind();

                if (this.rptLogs != null)
                {
                    this.rptLogs.DataSource = DataProvider.GovPublicApplyLogDAO.GetDataSourceByApplyID(this.applyInfo.ID);
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
                base.FailMessage("回复失败，必须填写答复内容");
                return;
            }
            try
            {
                DataProvider.GovPublicApplyReplyDAO.DeleteByApplyID(this.applyInfo.ID);
                string fileUrl = string.Empty;
                GovPublicApplyReplyInfo replyInfo = new GovPublicApplyReplyInfo(0, base.PublishmentSystemID, this.applyInfo.ID, this.tbReply.Text, fileUrl, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.GovPublicApplyReplyDAO.Insert(replyInfo);

                GovPublicApplyManager.Log(base.PublishmentSystemID, this.applyInfo.ID, EGovPublicApplyLogType.Reply);
                DataProvider.GovPublicApplyDAO.UpdateState(this.applyInfo.ID, EGovPublicApplyState.Replied);

                base.SuccessMessage("申请回复成功");

                base.AddWaitAndRedirectScript(this.ListPageUrl);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        public void SwitchTo_OnClick(object sender, System.EventArgs e)
        {
            int switchToDepartmentID = TranslateUtils.ToInt(base.Request.Form["switchToDepartmentID"]);
            if (switchToDepartmentID == 0)
            {
                base.FailMessage("转办失败，必须选择转办部门");
                return;
            }
            string switchToDepartmentName = DepartmentManager.GetDepartmentName(switchToDepartmentID);
            try
            {
                DataProvider.GovPublicApplyDAO.UpdateDepartmentID(this.applyInfo.ID, switchToDepartmentID);

                GovPublicApplyRemarkInfo remarkInfo = new GovPublicApplyRemarkInfo(0, base.PublishmentSystemID, this.applyInfo.ID, EGovPublicApplyRemarkType.SwitchTo, this.tbSwitchToRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.GovPublicApplyRemarkDAO.Insert(remarkInfo);

                GovPublicApplyManager.LogSwitchTo(base.PublishmentSystemID, this.applyInfo.ID, switchToDepartmentName);

                base.SuccessMessage("申请转办成功");

                base.AddWaitAndRedirectScript(this.ListPageUrl);
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
                    base.FailMessage("批示失败，必须填写意见");
                    return;
                }

                GovPublicApplyRemarkInfo remarkInfo = new GovPublicApplyRemarkInfo(0, base.PublishmentSystemID, this.applyInfo.ID, EGovPublicApplyRemarkType.Comment, this.tbCommentRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.GovPublicApplyRemarkDAO.Insert(remarkInfo);

                GovPublicApplyManager.Log(base.PublishmentSystemID, this.applyInfo.ID, EGovPublicApplyLogType.Comment);

                base.SuccessMessage("申请批示成功");

                base.AddWaitAndRedirectScript(this.ListPageUrl);
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
                EGovPublicApplyRemarkType remarkType = EGovPublicApplyRemarkTypeUtils.GetEnumType(TranslateUtils.EvalString(e.Item.DataItem, "RemarkType"));
                string remark = TranslateUtils.EvalString(e.Item.DataItem, "Remark");

                if (string.IsNullOrEmpty(remark))
                {
                    e.Item.Visible = false;
                }
                else
                {
                    this.phRemarks.Visible = true;
                    ltlRemarkType.Text = EGovPublicApplyRemarkTypeUtils.GetText(remarkType);
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
                ltlUserName.Text = userName;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(addDate);
                ltlSummary.Text = summary;
            }
        }

        public string ListPageUrl
        {
            get { return this.returnUrl; }
        }
    }
}
