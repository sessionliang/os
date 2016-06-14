using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Collections;
using System.Web.UI.HtmlControls;

using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.BackgroundPages
{
	public class BackgroundConfigurationOrder : BackgroundBasePage
	{
        public DropDownList ddlIsLocationAll;
        public TextBox tbOrderInvoiceContentCollection;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            base.BreadCrumb(AppManager.B2C.LeftMenu.ID_ConfigrationB2C, "��������", AppManager.B2C.Permission.WebSite.Configration);

			if (!IsPostBack)
			{
                EBooleanUtils.AddListItems(this.ddlIsLocationAll, "����ȫ����Χ����", "����������������õķ�Χ����");
                ControlUtils.SelectListItems(this.ddlIsLocationAll, base.PublishmentSystemInfo.Additional.OrderIsLocationAll.ToString());
                this.tbOrderInvoiceContentCollection.Text = base.PublishmentSystemInfo.Additional.OrderInvoiceContentCollection.ToString();
			}
		}

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.OrderIsLocationAll = TranslateUtils.ToBool(this.ddlIsLocationAll.SelectedValue);
                base.PublishmentSystemInfo.Additional.OrderInvoiceContentCollection = this.tbOrderInvoiceContentCollection.Text;
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "�޸Ķ�������");

                    base.SuccessMessage("���������޸ĳɹ���");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "���������޸�ʧ�ܣ�");
				}
			}
		}
	}
}
