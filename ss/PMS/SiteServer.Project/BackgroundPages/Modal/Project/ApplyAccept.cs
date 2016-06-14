using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.Project.Model;
using System.Text;
using BaiRong.Controls;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class ApplyAccept : BackgroundBasePage
	{
        protected TextBox tbAcceptRemark;
        public HtmlControl divSelectToUserName;
        public DateTimeTextBox tbAcceptEndDate;
        public Literal ltlDepartmentName;
        public Literal ltlUserName;

        private ArrayList idArrayList;

        public static string GetShowPopWinString(int projectID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProjectID", projectID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("受理办件", "modal_applyAccept.aspx", arguments, "IDCollection", "请选择需要受理的办件！", 600, 500);
        }
        
		public void Page_Load(object sender, EventArgs E)
		{
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                this.divSelectToUserName.Attributes.Add("onclick", Modal.UserNameSelect.GetShowPopWinString(0, "selectToUserName"));
                this.ltlDepartmentName.Text = AdminManager.CurrrentDepartmentName;
                this.ltlUserName.Text = AdminManager.DisplayName;
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
            try
            {
                string selectToUserName = base.Request.Form["selectToUserName"];
                if (string.IsNullOrEmpty(selectToUserName))
                {
                    base.FailMessage("受理失败，必须选择负责人");
                    return;
                }

                foreach (int applyID in this.idArrayList)
                {
                    EApplyState state = DataProvider.ApplyDAO.GetState(applyID);
                    if (state == EApplyState.New || state == EApplyState.Denied)
                    {
                        ApplyInfo applyInfo = DataProvider.ApplyDAO.GetApplyInfo(applyID);

                        RemarkInfo remarkInfo = new RemarkInfo(0, applyInfo.ID, ERemarkType.Accept, this.tbAcceptRemark.Text, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, DateTime.Now);
                        DataProvider.RemarkDAO.Insert(remarkInfo);

                        ApplyManager.Log(applyInfo.ID, EProjectLogType.Accept);
                        applyInfo.AcceptUserName = AdminManager.Current.UserName;
                        applyInfo.AcceptDate = DateTime.Now;
                        applyInfo.DepartmentID = AdminManager.GetDepartmentID(selectToUserName);
                        applyInfo.UserName = selectToUserName;
                        applyInfo.EndDate = this.tbAcceptEndDate.DateTime;
                        applyInfo.State = EApplyState.Accepted;
                        applyInfo.StartDate = DateTime.Now;
                        DataProvider.ApplyDAO.Update(applyInfo);
                    }
                }

                isChanged = true;
            }
			catch(Exception ex)
			{
                base.FailMessage(ex, ex.Message);
			    isChanged = false;
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPage(Page, string.Format("alert('办件受理成功!');"));
			}
		}

	}
}
