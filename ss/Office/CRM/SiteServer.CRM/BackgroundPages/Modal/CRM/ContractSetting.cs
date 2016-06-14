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
using BaiRong.Model;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages.Modal
{
	public class ContractSetting : BackgroundBasePage
	{
        public DropDownList ddlIsContract;
        public DateTimeTextBox tbContractDate;
        public DropDownList ddlIsConfirm;
        public DateTimeTextBox tbConfirmDate;

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
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("设置属性", "modal_contractSetting.aspx", arguments, "IDCollection", "请选择需要设置的合同！", 500, 350);
        }

        public static string GetShowPopWinString(int orderID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("IDCollection", orderID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("设置属性", "modal_contractSetting.aspx", arguments, 500, 350);
        }
        
		public void Page_Load(object sender, EventArgs E)
		{
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                EBooleanUtils.AddListItems(this.ddlIsContract);
                this.ddlIsContract.Items.Insert(0, new ListItem("<保持不变>", string.Empty));

                EBooleanUtils.AddListItems(this.ddlIsConfirm);
                this.ddlIsConfirm.Items.Insert(0, new ListItem("<保持不变>", string.Empty));
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
            try
            {
                foreach (int id in this.idArrayList)
                {
                    ContractInfo contractInfo = DataProvider.ContractDAO.GetContractInfo(id);
                    if (!string.IsNullOrEmpty(this.ddlIsContract.SelectedValue))
                    {
                        contractInfo.IsContract = TranslateUtils.ToBool(this.ddlIsContract.SelectedValue);
                        contractInfo.ContractDate = this.tbContractDate.DateTime;
                    }
                    if (!string.IsNullOrEmpty(this.ddlIsConfirm.SelectedValue))
                    {
                        contractInfo.IsConfirm = TranslateUtils.ToBool(this.ddlIsConfirm.SelectedValue);
                        contractInfo.ConfirmDate = this.tbConfirmDate.DateTime;
                    }

                    DataProvider.ContractDAO.Update(contractInfo);
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
