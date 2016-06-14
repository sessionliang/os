using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.Project.Controls;
using System.Collections.Specialized;


using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Web;

namespace SiteServer.Project.BackgroundPages
{
    public class RequestSEM : BackgroundBasePage
    {
        public Literal ltlJsonOrder;

        private string orderSN = string.Empty;
        private OrderInfo orderInfo = null;

        protected override bool IsAccessable
        {
            get
            {
                return true;
            }
        }

        public static string GetRedirectUrl(string orderSN)
        {
            return string.Format("http://brs.siteserver.cn/siteserver/project/request_sem.aspx?sn={0}", orderSN);
            //return string.Format("http://www.yun.com/siteserver/project/request_sem.aspx?sn={0}", orderSN);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.orderSN = base.GetQueryString("sn");

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.orderSN))
                {
                    this.orderInfo = DataProvider.OrderDAO.GetOrderInfoBySN(this.orderSN);

                    if (this.orderInfo != null)
                    {
                        OrderFormSEMInfo formInfo = DataProvider.OrderFormSEMDAO.GetOrderFormSEMInfoByOrderID(orderInfo.ID);

                        base.AddAttributes(orderInfo.Attributes);
                        base.RemoveValue(OrderAttribute.Summary);
                        base.SetValue("orderFormID", formInfo == null ? "0" : formInfo.ID.ToString());
                        base.SetValue("orderID", orderInfo.ID.ToString());

                        if (formInfo == null)
                        {
                            base.SetValue("state", "undo");
                        }
                        else
                        {
                            base.SetValue("state", "done");

                            base.SetValue("submitDate", DateUtils.GetDateString(formInfo.AddDate, EDateFormatType.Chinese));
                            base.SetValue("doneDate", DateUtils.GetDateString(formInfo.AddDate.AddDays(2), EDateFormatType.Chinese));
                        }
                        this.ltlJsonOrder.Text = TranslateUtils.NameValueCollectionToJsonString(base.GetAttributes());
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                int orderID = TranslateUtils.ToInt(base.Request.Form["orderID"]);

                OrderFormSEMInfo formInfo = DataProvider.OrderFormSEMDAO.GetOrderFormSEMInfo(base.Request.Form);
                formInfo.Attributes.Remove("__viewstate");

                formInfo.OrderID = orderID;
                formInfo.AddDate = DateTime.Now;

                DataProvider.OrderFormSEMDAO.Insert(formInfo);

                PageUtils.Redirect(string.Format("request_sem.aspx?sn={0}", base.Request.QueryString["sn"]));
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
