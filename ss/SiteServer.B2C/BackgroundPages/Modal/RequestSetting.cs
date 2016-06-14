using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.B2C.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.B2C.Model;
using System.Text;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages.Modal
{
	public class RequestSetting : BackgroundBasePage
	{
        protected DropDownList ddlStatus;
        protected DropDownList ddlChargeUserName;

        private ArrayList idArrayList;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public static string GetShowPopWinString()
        {
            NameValueCollection arguments = new NameValueCollection();
            return PageUtilityB2C.GetOpenWindowStringWithCheckBoxValue("设置属性", "modal_requestSetting.aspx", arguments, "IDCollection", "请选择需要设置的工单！", 500, 350);
        }

        public static string GetShowPopWinString(int requestID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("IDCollection", requestID.ToString());
            return PageUtilityB2C.GetOpenWindowString("设置属性", "modal_requestSetting.aspx", arguments, 500, 350);
        }
        
		public void Page_Load(object sender, EventArgs E)
		{
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("IDCollection"));

			if (!IsPostBack)
			{
                ListItem listItem = new ListItem("<<保持不变>>", "0");
                this.ddlStatus.Items.Add(listItem);
                ERequestStatusUtils.AddListItems(this.ddlStatus);

                listItem = new ListItem("<<保持不变>>", string.Empty);
                this.ddlChargeUserName.Items.Add(listItem);
                ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList();
                foreach (string userName in userNameArrayList)
                {
                    this.ddlChargeUserName.Items.Add(new ListItem(AdminManager.GetDisplayName(userName, true), userName));
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
            try
            {
                string status = this.ddlStatus.SelectedValue;
                string chargeUserName = this.ddlChargeUserName.SelectedValue;
                foreach (int requestID in this.idArrayList)
                {
                    RequestInfo requestInfo = DataProviderB2C.RequestDAO.GetRequestInfo(requestID);
                    if (!string.IsNullOrEmpty(status))
                    {
                        requestInfo.Status = status;
                    }
                    if (!string.IsNullOrEmpty(chargeUserName))
                    {
                        requestInfo.AdminUserName= chargeUserName;
                    }
                    DataProviderB2C.RequestDAO.Update(requestInfo);
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
                JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}

	}
}
