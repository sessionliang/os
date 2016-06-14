using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CRM.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.CRM.Model;
using System.Text;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages.Modal
{
	public class LeadSetting : BackgroundBasePage
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
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("��������", "modal_leadSetting.aspx", arguments, "IDCollection", "��ѡ����Ҫ���õ�������", 500, 350);
        }

        public static string GetShowPopWinString(int leadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("IDCollection", leadID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("��������", "modal_leadSetting.aspx", arguments, 500, 350);
        }
        
		public void Page_Load(object sender, EventArgs E)
		{
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                ListItem listItem = new ListItem("<<���ֲ���>>", "0");
                this.ddlStatus.Items.Add(listItem);
                ELeadStatusUtils.AddListItems(this.ddlStatus);

                listItem = new ListItem("<<���ֲ���>>", string.Empty);
                this.ddlChargeUserName.Items.Add(listItem);
                ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(ConfigurationManager.Additional.CRMDepartmentIDCollection);
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
                foreach (int leadID in this.idArrayList)
                {
                    LeadInfo leadInfo = DataProvider.LeadDAO.GetLeadInfo(leadID);
                    if (!string.IsNullOrEmpty(status))
                    {
                        leadInfo.Status = status;
                    }
                    if (!string.IsNullOrEmpty(chargeUserName))
                    {
                        leadInfo.ChargeUserName = chargeUserName;
                    }
                    DataProvider.LeadDAO.Update(leadInfo);
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
