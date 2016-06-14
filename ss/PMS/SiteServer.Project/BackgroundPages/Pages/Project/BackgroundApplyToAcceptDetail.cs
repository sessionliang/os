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
    public class BackgroundApplyToAcceptDetail : BackgroundApplyToDetailBasePage
	{
        public TextBox tbAcceptRemark;
        public HtmlControl divSelectToUserName;
        public DateTimeTextBox tbAcceptEndDate;
        public TextBox tbDenyReply;

        public static string GetRedirectUrl(int applyID)
        {
            return string.Format(@"background_ApplyToAcceptDetail.aspx?ApplyID={0}", applyID);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                if (base.applyInfo.EndDate != DateUtils.SqlMinValue)
                {
                    this.tbAcceptEndDate.DateTime = base.applyInfo.EndDate;
                }

                if (this.divSelectToUserName != null)
                {
                    string scriptName = "selectToUserName";
                    this.divSelectToUserName.Attributes.Add("onclick", Modal.UserNameSelect.GetShowPopWinString(base.applyInfo.DepartmentID, scriptName));
                }
            }
        }

        public void Accept_OnClick(object sender, System.EventArgs e)
        {
            string selectToUserName = base.Request.Form["selectToUserName"];
            if (string.IsNullOrEmpty(selectToUserName))
            {
                base.FailMessage("受理失败，必须选择负责人");
                return;
            }
            try
            {
                RemarkInfo remarkInfo = new RemarkInfo(0, base.applyInfo.ID, ERemarkType.Accept, this.tbAcceptRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.RemarkDAO.Insert(remarkInfo);

                ApplyManager.Log(this.applyInfo.ID, EProjectLogType.Accept);
                base.applyInfo.AcceptUserName = AdminManager.Current.UserName;
                base.applyInfo.AcceptDate = DateTime.Now;
                base.applyInfo.DepartmentID = AdminManager.GetDepartmentID(selectToUserName);
                base.applyInfo.UserName = selectToUserName;
                base.applyInfo.EndDate = this.tbAcceptEndDate.DateTime;
                base.applyInfo.State = EApplyState.Accepted;
                base.applyInfo.StartDate = DateTime.Now;
                DataProvider.ApplyDAO.Update(base.applyInfo);
                base.SuccessMessage("办件受理成功");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        public void Deny_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                DataProvider.ReplyDAO.DeleteByApplyID(this.applyInfo.ID);
                ReplyInfo replyInfo = new ReplyInfo(0, this.applyInfo.ID, this.tbDenyReply.Text, string.Empty, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.ReplyDAO.Insert(replyInfo);

                ApplyManager.Log(this.applyInfo.ID, EProjectLogType.Deny);
                DataProvider.ApplyDAO.UpdateState(this.applyInfo.ID, EApplyState.Denied);

                base.SuccessMessage("拒绝办件成功");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
	}
}
