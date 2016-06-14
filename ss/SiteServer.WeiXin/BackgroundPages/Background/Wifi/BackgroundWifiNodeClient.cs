
using BaiRong.Core;
using BaiRong.Core.Net;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundWifiNodeClient : BackgroundBasePageWX
    {
        public Repeater rptContents;

        public Button btnReturn;

        private string wifiNodeID;

        public static string GetRedirectUrl(int publishmentSystemID, string wifiNodeID)
        {
            return PageUtils.GetWXUrl(string.Format("background_wifiNodeClient.aspx?publishmentSystemID={0}&wifiNodeID={1}", publishmentSystemID, wifiNodeID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            this.wifiNodeID = base.GetQueryString("wifiNodeID").ToString();

            if (!IsPostBack)
            { 
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Wifi, "连接名单管理", AppManager.WeiXin.Permission.WebSite.Wifi);
                WifiInfo wifiInfo = DataProviderWX.WifiDAO.GetWifiInfoByPublishmentSystemID(base.PublishmentSystemID);

                string jsonString = returnJsonstring(wifiInfo.BusinessID, this.wifiNodeID);

                JavaScriptSerializer js = new JavaScriptSerializer();

                WifiNodeClientInfo wifiNodeClientInfo = js.Deserialize<WifiNodeClientInfo>(jsonString);

                this.rptContents.DataSource = wifiNodeClientInfo.data.list;

                this.rptContents.DataBind();

                string urlReturn = BackgroundWifiNode.GetRedirectUrl(base.PublishmentSystemID);
                this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false", urlReturn));
            }
        }

        public string returnJsonstring(string businessID, string wifiNodeID)
        {
            string postUrl = "http://wifi.wechatwifi.com/api/node/client";

            NameValueCollection list = new NameValueCollection();

            list.Add("page", "1");
            list.Add("id", wifiNodeID);
            list.Add("a", "0pqqYvq7j0xqqmq5");
            list.Add("b", businessID.Substring(0, 16));

            string retval = "";

            WebClientUtils.Post(postUrl, list, out retval);

            string jsonString = retval.Substring(9, retval.Length - 10);

            return jsonString;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name=”timeStamp”></param>
        /// <returns></returns>
        private DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime); return dtStart.Add(toNow);
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                WifiInfo wifiInfo = DataProviderWX.WifiDAO.GetWifiInfoByPublishmentSystemID(base.PublishmentSystemID);

                string jsonString = returnJsonstring(wifiInfo.BusinessID, this.wifiNodeID);

                JavaScriptSerializer js = new JavaScriptSerializer();

                WifiNodeClientInfo wifiNodeInfo = js.Deserialize<WifiNodeClientInfo>(jsonString);

                Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlCreatedAt = e.Item.FindControl("ltlCreatedAt") as Literal;
                Literal ltlNodeID = e.Item.FindControl("ltlNodeID") as Literal;
                Literal ltlInCome = e.Item.FindControl("ltlInCome") as Literal;
                Literal ltlOutGo = e.Item.FindControl("ltlOutGo") as Literal;
                Literal ltlIP = e.Item.FindControl("ltlIP") as Literal;
                Literal ltlMac = e.Item.FindControl("ltlMac") as Literal;
                Literal ltlUpdatedAt = e.Item.FindControl("ltlUpdatedAt") as Literal;
                Literal ltlAuthType = e.Item.FindControl("ltlAuthType") as Literal;
                Literal ltlUserType = e.Item.FindControl("ltlUserType") as Literal;

                ltlID.Text = wifiNodeInfo.data.list[e.Item.ItemIndex].id.ToString();
                ltlCreatedAt.Text = GetTime(wifiNodeInfo.data.list[e.Item.ItemIndex].created_at.ToString()).ToString();
                ltlNodeID.Text = wifiNodeInfo.data.list[e.Item.ItemIndex].node_id.ToString();
                ltlInCome.Text = wifiNodeInfo.data.list[e.Item.ItemIndex].in_come.ToString();
                ltlOutGo.Text = wifiNodeInfo.data.list[e.Item.ItemIndex].out_go.ToString();
                ltlIP.Text = wifiNodeInfo.data.list[e.Item.ItemIndex].ip.ToString();
                ltlMac.Text = wifiNodeInfo.data.list[e.Item.ItemIndex].mac.ToString();
                ltlUpdatedAt.Text =  GetTime(wifiNodeInfo.data.list[e.Item.ItemIndex].updated_at.ToString()).ToString();

                ltlAuthType.Text = "微信";
                if (wifiNodeInfo.data.list[e.Item.ItemIndex].auth_type.ToString() == "2")
                {
                    ltlAuthType.Text = "其他";
                } 

                ltlUserType.Text = "新粉丝";
                if (wifiNodeInfo.data.list[e.Item.ItemIndex].user_type.ToString() == "2")
                {
                    ltlUserType.Text = "旧粉丝";
                }
                else if (wifiNodeInfo.data.list[e.Item.ItemIndex].user_type.ToString() == "3")
                {
                    ltlUserType.Text = "游客";
                }

            }
        }
    }
}
