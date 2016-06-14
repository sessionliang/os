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

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class OrderSetting : BackgroundBasePage
	{
        protected DropDownList ddlStatus;

        private EOrderType orderType;
        private ArrayList idArrayList;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public static string GetShowPopWinString(EOrderType orderType)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("orderType", EOrderTypeUtils.GetValue(orderType));
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("设置属性", "modal_orderSetting.aspx", arguments, "IDCollection", "请选择需要设置的订单！", 500, 350);
        }

        public static string GetShowPopWinString(EOrderType orderType, int orderID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("orderType", EOrderTypeUtils.GetValue(orderType));
            arguments.Add("IDCollection", orderID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("设置属性", "modal_orderSetting.aspx", arguments, 500, 350);
        }
        
		public void Page_Load(object sender, EventArgs E)
		{
            this.orderType = EOrderTypeUtils.GetEnumType(base.Request.QueryString["orderType"]);
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["IDCollection"]);

			if (!IsPostBack)
			{
                EOrderStatusUtils.AddListItems(this.orderType, this.ddlStatus);
                this.ddlStatus.Items.Insert(0, new ListItem("<保持不变>", string.Empty));

                if (this.idArrayList.Count == 1)
                {
                    OrderInfo orderInfo = DataProvider.OrderDAO.GetOrderInfo(Convert.ToInt32(idArrayList[0]));
                    ControlUtils.SelectListItems(this.ddlStatus, EOrderStatusUtils.GetValue(orderInfo.Status));
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
            try
            {
                foreach (int orderID in this.idArrayList)
                {
                    OrderInfo orderInfo = DataProvider.OrderDAO.GetOrderInfo(orderID);
                    if (!string.IsNullOrEmpty(this.ddlStatus.SelectedValue))
                    {
                        orderInfo.Status = EOrderStatusUtils.GetEnumType(this.ddlStatus.SelectedValue);
                    }
                    DataProvider.OrderDAO.Update(orderInfo);
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
