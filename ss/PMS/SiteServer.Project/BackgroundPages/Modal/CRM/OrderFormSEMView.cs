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
    public class OrderFormSEMView : BackgroundBasePage
    {
        public static string GetShowPopWinString(int orderFormID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("orderFormID", orderFormID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("表单查看", "modal_orderFormSEMView.aspx", arguments, true);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int orderFormID = int.Parse(base.Request.QueryString["orderFormID"]);
            OrderFormSEMInfo formInfo = DataProvider.OrderFormSEMDAO.GetOrderFormSEMInfo(orderFormID);
            base.AddAttributes(formInfo);
        }
    }
}
