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
	public class BackgroundApplyToCheckDetail : BackgroundApplyToDetailBasePage
	{
        public TextBox tbRedoRemark;

        public static string GetRedirectUrl(int applyID)
        {
            return string.Format(@"background_ApplyToCheckDetail.aspx?ApplyID={0}", applyID);
        }

        public void Redo_OnClick(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbRedoRemark.Text))
            {
                base.FailMessage("要求返工失败，必须填写意见");
                return;
            }
            try
            {
                RemarkInfo remarkInfo = new RemarkInfo(0, this.applyInfo.ID, ERemarkType.Redo, this.tbRedoRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                DataProvider.RemarkDAO.Insert(remarkInfo);

                ApplyManager.Log(this.applyInfo.ID, EProjectLogType.Redo);
                DataProvider.ApplyDAO.UpdateState(this.applyInfo.ID, EApplyState.Redo);

                base.SuccessMessage("要求返工成功");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        public void Check_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                ApplyManager.Log(this.applyInfo.ID, EProjectLogType.Check);

                base.applyInfo.State = EApplyState.Checked;
                base.applyInfo.CheckUserName = AdminManager.Current.UserName;
                base.applyInfo.CheckDate = DateTime.Now;
                DataProvider.ApplyDAO.Update(this.applyInfo);
                base.SuccessMessage("审核办件成功");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
	}
}
