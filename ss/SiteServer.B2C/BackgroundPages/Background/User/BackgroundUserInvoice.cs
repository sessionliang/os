using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Collections;
using System.Collections.Generic;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;
using System.Text;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundUserInvoice : BackgroundBasePage
    {
        public Repeater rptContents;
        public Button btnAdd;

        private string userName;
        private string returnUrl;
        private List<InvoiceInfo> invoiceInfoList;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.userName = base.GetQueryString("userName");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("returnUrl"));

            if (base.GetQueryString("isDelete") != null && base.GetQueryString("invoiceID") != null)
            {
                int invoiceID = base.GetIntQueryString("invoiceID");
                if (invoiceID > 0)
                {
                    DataProviderB2C.InvoiceDAO.Delete(invoiceID);
                    base.SuccessMessage("��Ʊ��Ϣɾ���ɹ�");
                }
            }
            else if (base.GetQueryString("isDefault") != null && base.GetQueryString("invoiceID") != null)
            {
                int invoiceID = base.GetIntQueryString("invoiceID");
                if (invoiceID > 0)
                {
                    DataProviderB2C.InvoiceDAO.SetDefault(invoiceID);
                    base.SuccessMessage("Ĭ�Ϸ�Ʊ��Ϣ���óɹ�");
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.B2C.LeftMenu.ID_User, "��Ʊ��Ϣ", string.Empty);

                this.invoiceInfoList = DataProviderB2C.InvoiceDAO.GetInvoiceInfoList(base.PublishmentSystemInfo.GroupSN, this.userName);

                this.rptContents.DataSource = this.invoiceInfoList;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();

                //this.btnAdd.Attributes.Add("onclick", Modal.InvoiceAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.userName));
            }
        }

        private void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                InvoiceInfo invoiceInfo = e.Item.DataItem as InvoiceInfo;

                Literal ltlIsVat = e.Item.FindControl("ltlIsVat") as Literal;
                Literal ltlCompany = e.Item.FindControl("ltlCompany") as Literal;
                Literal ltlVat = e.Item.FindControl("ltlVat") as Literal;
                Literal ltlConsignee = e.Item.FindControl("ltlConsignee") as Literal;
                Literal ltlIsDefault = e.Item.FindControl("ltlIsDefault") as Literal;
                //Literal ltlIsDefaultUrl = e.Item.FindControl("ltlIsDefaultUrl") as Literal;
                //Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlIsVat.Text = invoiceInfo.IsVat ? "��ֵ˰��Ʊ" : "��ͨ��Ʊ";
                
                if (!invoiceInfo.IsVat)
                {
                    ltlCompany.Text = invoiceInfo.IsCompany ? invoiceInfo.CompanyName : "����";
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat("��λ���ƣ�{0}<br />", invoiceInfo.VatCompanyName);
                    builder.AppendFormat("��˰��ʶ��ţ�{0}<br />", invoiceInfo.VatCode);
                    builder.AppendFormat("ע���ַ��{0}<br />", invoiceInfo.VatAddress);
                    builder.AppendFormat("ע��绰��{0}<br />", invoiceInfo.VatPhone);
                    builder.AppendFormat("�������У�{0}<br />", invoiceInfo.VatBankName);
                    builder.AppendFormat("�����ʻ���{0}", invoiceInfo.VatBankAccount);
                    ltlVat.Text = builder.ToString();
                }

                if (invoiceInfo.IsDefault)
                {
                    ltlIsDefault.Text = StringUtils.GetTrueImageHtml(true);
                }
                //else
                //{
                //    string urlDefault = PageUtils.GetB2CUrl(string.Format("background_userInvoice.aspx?publishmentSystemID={0}&userName={1}&isDefault=True&invoiceID={2}&returnUrl={3}", base.PublishmentSystemID, this.userName, invoiceInfo.ID, StringUtils.ValueToUrl(this.returnUrl)));
                //    ltlIsDefaultUrl.Text = string.Format(@"<a href=""{0}"">��ΪĬ��</a>", urlDefault);
                //}

                ltlConsignee.Text += string.Format("��Ʊ��������{0}<br />", invoiceInfo.ConsigneeName);
                ltlConsignee.Text += string.Format("��Ʊ���ֻ��ţ�{0}<br />", invoiceInfo.ConsigneeMobile);
                ltlConsignee.Text += string.Format("��Ʊ��ַ��{0}", invoiceInfo.ConsigneeAddress);
                
                //ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">�޸�</a>", Modal.InvoiceAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, this.userName, invoiceInfo.ID));

                string urlDelete = PageUtils.GetB2CUrl(string.Format("background_userInvoice.aspx?publishmentSystemID={0}&userName={1}&isDelete=True&invoiceID={2}&returnUrl={3}", base.PublishmentSystemID, this.userName, invoiceInfo.ID, StringUtils.ValueToUrl(this.returnUrl)));
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onclick=""javascript:return confirm('�˲�����ɾ��ѡ����Ʊ��Ϣ��ȷ����');"">ɾ��</a>", urlDelete);
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.returnUrl);
        }
    }
}
