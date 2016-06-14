using System;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SiteServer.Project.Model;
using SiteServer.Project.Core;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Cryptography;


namespace SiteServer.Project.BackgroundPages.Modal
{
    public class OrderView : BackgroundBasePage
    {
        public static string GetShowPopWinString(int orderID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("orderID", orderID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("订单信息", "modal_orderView.aspx", arguments, true);
        }

        public override string GetValue(string attributeName)
        {
            string value = base.GetValue(attributeName);
            string retval = value;

            if (attributeName == OrderAttribute.Status)
            {
                retval = EOrderStatusUtils.GetText(EOrderStatusUtils.GetEnumType(value));
            }
            else if (attributeName == OrderAttribute.IsReNew)
            {
                retval = retval == "True" ? "续费" : "新购";
            }
            
            return retval;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int orderID = int.Parse(base.Request.QueryString["orderID"]);
            OrderInfo orderInfo = DataProvider.OrderDAO.GetOrderInfo(orderID);
            base.AddAttributes(orderInfo);
        }
    }
}
