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
using System.Web;

namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovInteractPageBasePage : BackgroundGovInteractBasePage
    {
        public Literal ltlTitle;
        public Literal ltlApplyAttributes;
        public Literal ltlAddDate;
        public Literal ltlQueryCode;
        public Literal ltlState;
        public Literal ltlDepartmentName;

        public PlaceHolder phReply;
        public Literal ltlDepartmentAndUserName;
        public Literal ltlReplyAddDate;
        public Literal ltlReply;
        public Literal ltlReplyFileUrl;

        public PlaceHolder phBtnAccept;
        public PlaceHolder phBtnSwitchToTranslate;
        public PlaceHolder phBtnReply;
        public PlaceHolder phBtnCheck;
        public PlaceHolder phBtnComment;
        public PlaceHolder phBtnReturn;

        public TextBox tbReply;
        public HtmlInputFile htmlFileUrl;
        public TextBox tbSwitchToRemark;
        public HtmlControl divAddDepartment;
        public Literal ltlScript;
        public DropDownList ddlTranslateNodeID;
        public TextBox tbTranslateRemark;
        public TextBox tbCommentRemark;

        public PlaceHolder phRemarks;
        public Repeater rptRemarks;
        public Repeater rptLogs;

        protected GovInteractContentInfo contentInfo;
        private string returnUrl;

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
            PageUtils.CheckRequestParameter("PublishmentSystemID", "ContentID", "ReturnUrl");

            this.contentInfo = DataProvider.GovInteractContentDAO.GetContentInfo(base.PublishmentSystemInfo, TranslateUtils.ToInt(base.Request.QueryString["ContentID"]));
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);

            if (!IsPostBack)
            {
                if (this.phBtnAccept != null)
                {
                    this.phBtnAccept.Visible = GovInteractManager.IsPermission(base.PublishmentSystemID, this.contentInfo.NodeID, AppManager.CMS.Permission.GovInteract.GovInteractAccept);
                }
                if (this.phBtnSwitchToTranslate != null)
                {
                    this.phBtnSwitchToTranslate.Visible = GovInteractManager.IsPermission(base.PublishmentSystemID, this.contentInfo.NodeID, AppManager.CMS.Permission.GovInteract.GovInteractSwitchToTranslate);
                }
                if (this.phBtnReply != null)
                {
                    this.phBtnReply.Visible = GovInteractManager.IsPermission(base.PublishmentSystemID, this.contentInfo.NodeID, AppManager.CMS.Permission.GovInteract.GovInteractReply);
                }
                if (this.phBtnCheck != null)
                {
                    if (this.contentInfo.State == EGovInteractState.Checked)
                    {
                        this.phBtnCheck.Visible = false;
                    }
                    else
                    {
                        this.phBtnCheck.Visible = GovInteractManager.IsPermission(base.PublishmentSystemID, this.contentInfo.NodeID, AppManager.CMS.Permission.GovInteract.GovInteractCheck);
                    }
                }
                if (this.phBtnComment != null)
                {
                    if (this.contentInfo.State == EGovInteractState.Checked)
                    {
                        this.phBtnComment.Visible = false;
                    }
                    else
                    {
                        this.phBtnComment.Visible = GovInteractManager.IsPermission(base.PublishmentSystemID, this.contentInfo.NodeID, AppManager.CMS.Permission.GovInteract.GovInteractComment);
                    }
                }
                if (this.phBtnReturn != null)
                {
                    this.phBtnReturn.Visible = !base.PublishmentSystemInfo.Additional.GovInteractApplyIsOpenWindow;
                }

                StringBuilder builder = new StringBuilder();
                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.GovInteractContent, base.PublishmentSystemInfo.AuxiliaryTableForGovInteract, RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.contentInfo.NodeID));

                bool isPreviousSingleLine = true;
                bool isPreviousLeftColumn = false;
                foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                {
                    if (styleInfo.IsVisible && !StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, GovInteractContentAttribute.DepartmentID))
                    {
                        string value = this.contentInfo.GetExtendedAttribute(styleInfo.AttributeName);
                        if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, GovInteractContentAttribute.TypeID))
                        {
                            value = GovInteractManager.GetTypeName(TranslateUtils.ToInt(value));
                        }
                        else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, GovInteractContentAttribute.IsPublic))
                        {
                            value = TranslateUtils.ToBool(value) ? "公开" : "不公开";
                        }
                        else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, GovInteractContentAttribute.FileUrl))
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                value = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, value), value);
                            }
                        }

                        if (builder.Length > 0)
                        {
                            if (isPreviousSingleLine)
                            {
                                builder.Append("</tr>");
                            }
                            else
                            {
                                if (!isPreviousLeftColumn)
                                {
                                    builder.Append("</tr>");
                                }
                                else if (styleInfo.IsSingleLine)
                                {
                                    builder.Append(@"<td bgcolor=""#f0f6fc"" class=""attribute""></td><td></td></tr>");
                                }
                            }
                        }

                        //this line

                        if (styleInfo.IsSingleLine || isPreviousSingleLine || !isPreviousLeftColumn)
                        {
                            builder.Append("<tr>");
                        }

                        builder.AppendFormat(@"<td bgcolor=""#f0f6fc"" class=""attribute"">{0}</td><td {1} class=""tdBorder"">{2}</td>", styleInfo.DisplayName, styleInfo.IsSingleLine ? @"colspan=""3""" : string.Empty, value);


                        if (styleInfo.IsSingleLine)
                        {
                            isPreviousSingleLine = true;
                            isPreviousLeftColumn = false;
                        }
                        else
                        {
                            isPreviousSingleLine = false;
                            isPreviousLeftColumn = !isPreviousLeftColumn;
                        }
                    }
                }

                if (builder.Length > 0)
                {
                    if (isPreviousSingleLine || !isPreviousLeftColumn)
                    {
                        builder.Append("</tr>");
                    }
                    else
                    {
                        builder.Append(@"<td bgcolor=""#f0f6fc"" class=""attribute""></td><td></td></tr>");
                    }
                }
                this.ltlTitle.Text = this.contentInfo.Title;
                this.ltlApplyAttributes.Text = builder.ToString();
                this.ltlAddDate.Text = DateUtils.GetDateAndTimeString(this.contentInfo.AddDate);
                this.ltlQueryCode.Text = this.contentInfo.QueryCode;
                this.ltlState.Text = EGovInteractStateUtils.GetText(this.contentInfo.State);
                this.ltlDepartmentName.Text = this.contentInfo.DepartmentName;

                if (this.phReply != null)
                {
                    if (this.contentInfo.State == EGovInteractState.Denied || this.contentInfo.State == EGovInteractState.Replied || this.contentInfo.State == EGovInteractState.Redo || this.contentInfo.State == EGovInteractState.Checked)
                    {
                        GovInteractReplyInfo replyInfo = DataProvider.GovInteractReplyDAO.GetReplyInfoByContentID(base.PublishmentSystemID, this.contentInfo.ID);
                        if (replyInfo != null)
                        {
                            this.phReply.Visible = true;
                            this.ltlDepartmentAndUserName.Text = string.Format("{0}({1})", DepartmentManager.GetDepartmentName(replyInfo.DepartmentID), replyInfo.UserName);
                            this.ltlReplyAddDate.Text = DateUtils.GetDateAndTimeString(replyInfo.AddDate);
                            this.ltlReply.Text = replyInfo.Reply;
                            if (!string.IsNullOrEmpty(replyInfo.FileUrl))
                            {
                                this.ltlReplyFileUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, replyInfo.FileUrl), replyInfo.FileUrl);
                            }
                        }
                    }
                }

                if (this.divAddDepartment != null)
                {
                    this.divAddDepartment.Attributes.Add("onclick", Modal.GovInteractDepartmentSelect.GetOpenWindowString(base.PublishmentSystemID, this.contentInfo.NodeID));
                    StringBuilder scriptBuilder = new StringBuilder();
                    if (this.contentInfo.DepartmentID > 0)
                    {
                        string departmentName = DepartmentManager.GetDepartmentName(this.contentInfo.DepartmentID);
                        scriptBuilder.AppendFormat(@"<script>showCategoryDepartment('{0}', '{1}');</script>", departmentName, this.contentInfo.DepartmentID);
                    }
                    this.ltlScript.Text = scriptBuilder.ToString();
                }

                if (this.ddlTranslateNodeID != null)
                {
                    ArrayList nodeInfoArrayList = GovInteractManager.GetNodeInfoArrayList(base.PublishmentSystemInfo);
                    foreach (NodeInfo nodeInfo in nodeInfoArrayList)
                    {
                        if (nodeInfo.NodeID != this.contentInfo.NodeID)
                        {
                            ListItem listItem = new ListItem(nodeInfo.NodeName, nodeInfo.NodeID.ToString());
                            this.ddlTranslateNodeID.Items.Add(listItem);
                        }
                    }
                }

                this.rptRemarks.DataSource = DataProvider.GovInteractRemarkDAO.GetDataSourceByContentID(base.PublishmentSystemID, this.contentInfo.ID);
                this.rptRemarks.ItemDataBound += new RepeaterItemEventHandler(rptRemarks_ItemDataBound);
                this.rptRemarks.DataBind();

                if (this.rptLogs != null)
                {
                    this.rptLogs.DataSource = DataProvider.GovInteractLogDAO.GetDataSourceByContentID(base.PublishmentSystemID, this.contentInfo.ID);
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
                DataProvider.GovInteractReplyDAO.DeleteByContentID(base.PublishmentSystemID, this.contentInfo.ID);
                string fileUrl = this.UploadFile(this.htmlFileUrl.PostedFile);
                GovInteractReplyInfo replyInfo = new GovInteractReplyInfo(0, base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, this.tbReply.Text, fileUrl, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.GovInteractReplyDAO.Insert(replyInfo);

                GovInteractApplyManager.Log(base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, EGovInteractLogType.Reply);
                if (AdminManager.Current.DepartmentID > 0)
                {
                    DataProvider.GovInteractContentDAO.UpdateDepartmentID(base.PublishmentSystemInfo, this.contentInfo.ID, AdminManager.Current.DepartmentID);
                }
                DataProvider.GovInteractContentDAO.UpdateState(base.PublishmentSystemInfo, this.contentInfo.ID, EGovInteractState.Replied);

                base.SuccessMessage("办件回复成功");
                if (!base.PublishmentSystemInfo.Additional.GovInteractApplyIsOpenWindow)
                {
                    base.AddWaitAndRedirectScript(this.ListPageUrl);
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        private string UploadFile(HttpPostedFile myFile)
        {
            string fileUrl = string.Empty;

            if (myFile != null && !string.IsNullOrEmpty(myFile.FileName))
            {
                string filePath = myFile.FileName;
                try
                {
                    string fileExtName = PathUtils.GetExtension(filePath);
                    string localDirectoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                    string localFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath);

                    string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                    if (!PathUtility.IsFileExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                    {
                        return string.Empty;
                    }
                    if (!PathUtility.IsFileSizeAllowed(base.PublishmentSystemInfo, myFile.ContentLength))
                    {
                        return string.Empty;
                    }

                    myFile.SaveAs(localFilePath);
                    FileUtility.AddWaterMark(base.PublishmentSystemInfo, localFilePath);

                    fileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                    fileUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, fileUrl);
                }
                catch { }
            }

            return fileUrl;
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
                DataProvider.GovInteractContentDAO.UpdateDepartmentID(base.PublishmentSystemInfo, this.contentInfo.ID, switchToDepartmentID);

                GovInteractRemarkInfo remarkInfo = new GovInteractRemarkInfo(0, base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, EGovInteractRemarkType.SwitchTo, this.tbSwitchToRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.GovInteractRemarkDAO.Insert(remarkInfo);

                GovInteractApplyManager.LogSwitchTo(base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, switchToDepartmentName);

                base.SuccessMessage("办件转办成功");
                if (!base.PublishmentSystemInfo.Additional.GovInteractApplyIsOpenWindow)
                {
                    base.AddWaitAndRedirectScript(this.ListPageUrl);
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        public void Translate_OnClick(object sender, System.EventArgs e)
        {
            int translateNodeID = TranslateUtils.ToInt(this.ddlTranslateNodeID.SelectedValue);
            if (translateNodeID == 0)
            {
                base.FailMessage("转移失败，必须选择转移目标");
                return;
            }
            try
            {
                this.contentInfo.SetExtendedAttribute(GovInteractContentAttribute.TranslateFromNodeID, this.contentInfo.NodeID.ToString());
                this.contentInfo.NodeID = translateNodeID;
                DataProvider.ContentDAO.Update(base.PublishmentSystemInfo.AuxiliaryTableForGovInteract, base.PublishmentSystemInfo, this.contentInfo);

                if (!string.IsNullOrEmpty(this.tbTranslateRemark.Text))
                {
                    GovInteractRemarkInfo remarkInfo = new GovInteractRemarkInfo(0, base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, EGovInteractRemarkType.Translate, this.tbTranslateRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                    DataProvider.GovInteractRemarkDAO.Insert(remarkInfo);
                }

                GovInteractApplyManager.LogTranslate(base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, NodeManager.GetNodeName(base.PublishmentSystemID, this.contentInfo.NodeID));

                base.SuccessMessage("办件转移成功");
                if (!base.PublishmentSystemInfo.Additional.GovInteractApplyIsOpenWindow)
                {
                    base.AddWaitAndRedirectScript(this.ListPageUrl);
                }
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

                GovInteractRemarkInfo remarkInfo = new GovInteractRemarkInfo(0, base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, EGovInteractRemarkType.Comment, this.tbCommentRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.GovInteractRemarkDAO.Insert(remarkInfo);

                GovInteractApplyManager.Log(base.PublishmentSystemID, this.contentInfo.NodeID, this.contentInfo.ID, EGovInteractLogType.Comment);

                base.SuccessMessage("办件批示成功");
                if (!base.PublishmentSystemInfo.Additional.GovInteractApplyIsOpenWindow)
                {
                    base.AddWaitAndRedirectScript(this.ListPageUrl);
                }
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
                EGovInteractRemarkType remarkType = EGovInteractRemarkTypeUtils.GetEnumType(TranslateUtils.EvalString(e.Item.DataItem, "RemarkType"));
                string remark = TranslateUtils.EvalString(e.Item.DataItem, "Remark");

                if (string.IsNullOrEmpty(remark))
                {
                    e.Item.Visible = false;
                }
                else
                {
                    this.phRemarks.Visible = true;
                    ltlRemarkType.Text = EGovInteractRemarkTypeUtils.GetText(remarkType);
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
