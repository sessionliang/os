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

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundPayment : BackgroundBasePage
    {
        public Repeater rptInstalled;
        public Repeater rptUnInstalled;

        private List<PaymentInfo> paymentInfoList;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetB2CUrl(string.Format("background_payment.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("isInstall") != null && base.GetQueryString("paymentType") != null)
            {
                EPaymentType paymentType = EPaymentTypeUtils.GetEnumType(base.GetQueryString("paymentType"));

                if (!DataProviderB2C.PaymentDAO.IsExists(base.PublishmentSystemID, paymentType))
                {
                    PaymentInfo paymentInfo = new PaymentInfo(0, base.PublishmentSystemID, paymentType, EPaymentTypeUtils.GetText(paymentType), true, true, 0, EPaymentTypeUtils.GetDescription(paymentType), string.Empty);

                    DataProviderB2C.PaymentDAO.Insert(paymentInfo);
                    base.SuccessMessage("֧����ʽ��װ�ɹ�");
                }
            }
            else if (base.GetQueryString("isDelete") != null && base.GetQueryString("paymentID") != null)
            {
                int paymentID = base.GetIntQueryString("paymentID");
                if (paymentID > 0)
                {
                    DataProviderB2C.PaymentDAO.Delete(paymentID);
                    base.SuccessMessage("֧����ʽɾ���ɹ�");
                }
            }
            else if (base.GetQueryString("isEnable") != null && base.GetQueryString("paymentID") != null)
            {
                int paymentID = base.GetIntQueryString("paymentID");
                if (paymentID > 0)
                {
                    PaymentInfo paymentInfo = DataProviderB2C.PaymentDAO.GetPaymentInfo(paymentID);
                    if (paymentInfo != null)
                    {
                        string action = paymentInfo.IsEnabled ? "����" : "����";
                        paymentInfo.IsEnabled = !paymentInfo.IsEnabled;
                        DataProviderB2C.PaymentDAO.Update(paymentInfo);
                        base.SuccessMessage(string.Format("�ɹ�{0}֧����ʽ", action));
                    }
                }
            }
            else if (base.GetQueryString("setTaxis") != null)
            {
                int paymentID = base.GetIntQueryString("paymentID");
                string direction = base.GetQueryString("direction");
                if (paymentID > 0)
                {

                    switch (direction.ToUpper())
                    {
                        case "UP":
                            DataProviderB2C.PaymentDAO.UpdateTaxisToUp(base.PublishmentSystemID, paymentID);
                            break;
                        case "DOWN":
                            DataProviderB2C.PaymentDAO.UpdateTaxisToDown(base.PublishmentSystemID, paymentID);
                            break;
                        default:
                            break;
                    }
                    base.SuccessMessage("����ɹ���");
                    base.AddWaitAndRedirectScript(BackgroundPayment.GetRedirectUrl(base.PublishmentSystemID));
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.B2C.LeftMenu.ID_PaymentShipment, "֧����ʽ", string.Empty);

                this.paymentInfoList = DataProviderB2C.PaymentDAO.GetPaymentInfoList(base.PublishmentSystemID);

                this.rptInstalled.DataSource = this.paymentInfoList;
                this.rptInstalled.ItemDataBound += new RepeaterItemEventHandler(rptInstalled_ItemDataBound);
                this.rptInstalled.DataBind();

                this.rptUnInstalled.DataSource = EPaymentTypeUtils.GetEPaymentList();
                this.rptUnInstalled.ItemDataBound += new RepeaterItemEventHandler(rptUnInstalled_ItemDataBound);
                this.rptUnInstalled.DataBind();
            }
        }

        private void rptInstalled_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PaymentInfo paymentInfo = e.Item.DataItem as PaymentInfo;

                Literal ltlPaymentName = e.Item.FindControl("ltlPaymentName") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                HyperLink hlUpLink = e.Item.FindControl("hlUpLink") as HyperLink;
                HyperLink hlDownLink = e.Item.FindControl("hlDownLink") as HyperLink;
                Literal ltlConfigUrl = e.Item.FindControl("ltlConfigUrl") as Literal;
                Literal ltlIsEnabledUrl = e.Item.FindControl("ltlIsEnabledUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlPaymentName.Text = paymentInfo.PaymentName;
                ltlDescription.Text = StringUtils.MaxLengthText(paymentInfo.Description, 200);
                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(paymentInfo.IsEnabled);
                hlUpLink.NavigateUrl = BackgroundPayment.GetRedirectUrl(base.PublishmentSystemID) + string.Format("&setTaxis=True&paymentID={0}&direction=UP", paymentInfo.ID);
                hlDownLink.NavigateUrl = BackgroundPayment.GetRedirectUrl(base.PublishmentSystemID) + string.Format("&setTaxis=True&paymentID={0}&direction=DOWN", paymentInfo.ID);

                string urlConfig = BackgroundPaymentConfiguration.GetRedirectUrl(base.PublishmentSystemID, paymentInfo.ID);
                ltlConfigUrl.Text = string.Format(@"<a href=""{0}"">����</a>", urlConfig);

                string action = paymentInfo.IsEnabled ? "����" : "����";
                string urlIsEnabled = BackgroundPayment.GetRedirectUrl(base.PublishmentSystemID) + string.Format("&isEnable=True&paymentID={0}", paymentInfo.ID);
                ltlIsEnabledUrl.Text = string.Format(@"<a href=""{0}"">{1}</a>", urlIsEnabled, action);

                string urlDelete = BackgroundPayment.GetRedirectUrl(base.PublishmentSystemID) + string.Format("&isDelete=True&paymentID={0}", paymentInfo.ID);
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onclick=""javascript:return confirm('�˲�����ɾ��ѡ��֧����ʽ��ȷ����');"">ɾ��</a>", urlDelete);
            }
        }

        private void rptUnInstalled_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                EPaymentType paymentType = (EPaymentType)e.Item.DataItem;

                foreach (PaymentInfo paymentInfo in this.paymentInfoList)
                {
                    if (paymentInfo.PaymentType == paymentType)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }

                Literal ltlPaymentName = e.Item.FindControl("ltlPaymentName") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlInstallUrl = e.Item.FindControl("ltlInstallUrl") as Literal;

                ltlPaymentName.Text = EPaymentTypeUtils.GetText(paymentType);
                ltlDescription.Text = EPaymentTypeUtils.GetDescription(paymentType);

                string urlInstall = BackgroundPayment.GetRedirectUrl(base.PublishmentSystemID) + string.Format("&isInstall=True&paymentType={0}", EPaymentTypeUtils.GetValue(paymentType));
                ltlInstallUrl.Text = string.Format(@"<a href=""{0}"">��װ</a>", urlInstall);
            }
        }
    }
}
