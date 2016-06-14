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
using BaiRong.Model;
using BaiRong.Controls;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class InvoiceSetting : BackgroundBasePage
	{
        public DropDownList ddlIsInvoice;
        public DateTimeTextBox tbInvoiceDate;
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
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("设置属性", "modal_invoiceSetting.aspx", arguments, "IDCollection", "请选择需要设置的发票！", 500, 350);
        }

        public static string GetShowPopWinString(int orderID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("IDCollection", orderID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("设置属性", "modal_invoiceSetting.aspx", arguments, 500, 350);
        }
        
		public void Page_Load(object sender, EventArgs E)
		{
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                EBooleanUtils.AddListItems(this.ddlIsInvoice);
                this.ddlIsInvoice.Items.Insert(0, new ListItem("<保持不变>", string.Empty));

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
                    InvoiceInfo invoiceInfo = DataProvider.InvoiceDAO.GetInvoiceInfo(id);
                    if (!string.IsNullOrEmpty(this.ddlIsInvoice.SelectedValue))
                    {
                        invoiceInfo.IsInvoice = TranslateUtils.ToBool(this.ddlIsInvoice.SelectedValue);
                        invoiceInfo.InvoiceDate = this.tbInvoiceDate.DateTime;
                    }
                    if (!string.IsNullOrEmpty(this.ddlIsConfirm.SelectedValue))
                    {
                        invoiceInfo.IsConfirm = TranslateUtils.ToBool(this.ddlIsConfirm.SelectedValue);
                        invoiceInfo.ConfirmDate = this.tbConfirmDate.DateTime;
                    }

                    DataProvider.InvoiceDAO.Update(invoiceInfo);
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
