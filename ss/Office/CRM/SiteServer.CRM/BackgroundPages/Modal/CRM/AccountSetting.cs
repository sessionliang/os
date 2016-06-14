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
	public class AccountSetting : BackgroundBasePage
	{
        protected DropDownList ddlStatus;
        protected DropDownList ddlPriority;
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
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("设置属性", "modal_accountSetting.aspx", arguments, "IDCollection", "请选择需要设置的客户！", 500, 350);
        }

        public static string GetShowPopWinString(int accountID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("IDCollection", accountID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("设置属性", "modal_accountSetting.aspx", arguments, 500, 350);
        }
        
		public void Page_Load(object sender, EventArgs E)
		{
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                ListItem listItem = new ListItem("<<保持不变>>", "0");
                this.ddlStatus.Items.Add(listItem);
                EAccountStatusUtils.AddListItems(this.ddlStatus);

                listItem = new ListItem("<<保持不变>>", string.Empty);
                this.ddlPriority.Items.Add(listItem);
                for (int i = 1; i <= 3; i++ )
                {
                    string priority = string.Empty;
                    if (i == 1)
                    {
                        priority = "普通";
                    }
                    else if (i == 2)
                    {
                        priority = "高";
                    }
                    else if (i == 3)
                    {
                        priority = "重点";
                    }
                    this.ddlPriority.Items.Add(new ListItem(priority, i.ToString()));
                }

                listItem = new ListItem("<<保持不变>>", string.Empty);
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
                int priority = TranslateUtils.ToInt(this.ddlPriority.SelectedValue);
                string chargeUserName = this.ddlChargeUserName.SelectedValue;
                foreach (int accountID in this.idArrayList)
                {
                    AccountInfo accountInfo = DataProvider.AccountDAO.GetAccountInfo(accountID);
                    if (!string.IsNullOrEmpty(status))
                    {
                        accountInfo.Status = status;
                    }
                    if (priority > 0)
                    {
                        accountInfo.Priority = priority;
                    }
                    if (!string.IsNullOrEmpty(chargeUserName))
                    {
                        accountInfo.ChargeUserName = chargeUserName;
                    }
                    DataProvider.AccountDAO.Update(accountInfo);
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
