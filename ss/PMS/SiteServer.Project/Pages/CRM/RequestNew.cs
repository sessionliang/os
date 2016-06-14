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
    public class RequestNew : BackgroundBasePage
    {
        public Literal ltlJsonOrder;

        private string domain = string.Empty;
        private int orderID = 0;

        protected override bool IsAccessable
        {
            get
            {
                return true;
            }
        }

        public static string GetRedirectUrl(string domain)
        {
            return string.Format("http://brs.siteserver.cn/siteserver/project/request_new.aspx?domain={0}", domain);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.domain = base.GetQueryString("domain");
            this.domain = ProjectManager.GetCleanDomain(this.domain);
            this.orderID = DataProvider.OrderDAO.GetOrderID(this.domain);

            if (!IsPostBack)
            {
                if (this.orderID > 0)
                {
                    OrderInfo orderInfo = DataProvider.OrderDAO.GetOrderInfo(this.orderID);

                    base.AddAttributes(orderInfo.Attributes);
                    base.RemoveValue(OrderAttribute.Summary);
                    base.SetValue("orderID", orderInfo.ID.ToString());

                    this.ltlJsonOrder.Text = TranslateUtils.NameValueCollectionToJsonString(base.GetAttributes());
                }
            }
        }
    }
}
