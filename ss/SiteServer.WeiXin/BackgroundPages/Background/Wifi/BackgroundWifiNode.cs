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
    public class BackgroundWifiNode : BackgroundBasePageWX
    {
        public Repeater rptContents;

        public Button btnAdd;
        public Button btnDelete;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_wifiNode.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);


            if (!IsPostBack)
            { 
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Wifi, "路由器管理", AppManager.WeiXin.Permission.WebSite.Wifi);
                if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]) && !string.IsNullOrEmpty(base.Request.QueryString["WifiNodeIDCollection"]))
                {

                    deleteWechatWifi(base.Request.QueryString["WifiNodeIDCollection"]);

                    base.SuccessMessage("成功删除路由器信息.");
                }

                WifiInfo wifiInfo = DataProviderWX.WifiDAO.GetWifiInfoByPublishmentSystemID(base.PublishmentSystemID);

                if (wifiInfo == null)
                {
                    base.InfoMessage("请先配置商家信息,再添加路由！");
                    return;
                }

                string jsonString = returnJsonstring(wifiInfo.BusinessID);

                JavaScriptSerializer js = new JavaScriptSerializer();

                WifiNodeListInfo wifiNodeInfo = js.Deserialize<WifiNodeListInfo>(jsonString);

                this.rptContents.DataSource = wifiNodeInfo.data.list;

                this.rptContents.DataBind();

                string urlAdd = BackgroundWifiNodeAdd.GetRedirectUrl(base.PublishmentSystemID, "0");
                this.btnAdd.Attributes.Add("onclick", string.Format("location.href='{0}';return false", urlAdd));

                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetWXUrl("background_wifiNode.aspx?Delete=True&publishmentSystemID=" + base.PublishmentSystemID + ""), "WifiNodeIDCollection", "WifiNodeIDCollection", "请选择需要删除的路由器！", "此操作将删除所选路由器，确认删除吗？"));

            }
        }

        public string returnJsonstring(string businessID)
        {
            string postUrl = "http://wifi.wechatwifi.com/api/node/list";

            NameValueCollection list = new NameValueCollection();

            list.Add("page", "1");
            list.Add("a", "0pqqYvq7j0xqqmq5");
            list.Add("b", businessID.Substring(0, 16));

            string retval = "";

            WebClientUtils.Post(postUrl, list, out retval);

            string jsonString = retval.Substring(9, retval.Length - 10);

            return jsonString;
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                WifiInfo wifiInfo = DataProviderWX.WifiDAO.GetWifiInfoByPublishmentSystemID(base.PublishmentSystemID);

                string jsonString = returnJsonstring(wifiInfo.BusinessID);

                JavaScriptSerializer js = new JavaScriptSerializer();

                WifiNodeListInfo wifiNodeInfo = js.Deserialize<WifiNodeListInfo>(jsonString);

                Literal ltlNodeID = e.Item.FindControl("ltlNodeID") as Literal;
                Literal ltlNodeName = e.Item.FindControl("ltlNodeName") as Literal;
                Literal ltlWxBind = e.Item.FindControl("ltlWxBind") as Literal;
                Literal ltlWechatID = e.Item.FindControl("ltlWechatID") as Literal;
                Literal ltlOnline = e.Item.FindControl("ltlOnline") as Literal;
                Literal ltlNewFans = e.Item.FindControl("ltlNewFans") as Literal;
                Literal ltlFans = e.Item.FindControl("ltlFans") as Literal;
                Literal ltlVisiter = e.Item.FindControl("ltlVisiter") as Literal;
                Literal ltlSelectClient = e.Item.FindControl("ltlSelectClient") as Literal;
                Literal ltlbtnEdite = e.Item.FindControl("ltlbtnEdite") as Literal;

                ltlNodeID.Text = wifiNodeInfo.data.list[e.Item.ItemIndex].id.ToString();
                ltlNodeName.Text = wifiNodeInfo.data.list[e.Item.ItemIndex].name.ToString();
                ltlWxBind.Text = "未绑定";
                if (wifiNodeInfo.data.list[e.Item.ItemIndex].wx_bind.ToString() == "1")
                {
                    ltlWxBind.Text = "绑定";
                }
                ltlWechatID.Text = wifiNodeInfo.data.list[e.Item.ItemIndex].wechat_id.ToString();
                ltlOnline.Text = wifiNodeInfo.data.list[e.Item.ItemIndex].online.ToString();
                ltlNewFans.Text = wifiNodeInfo.data.list[e.Item.ItemIndex].today_newfans.ToString();
                ltlFans.Text = wifiNodeInfo.data.list[e.Item.ItemIndex].today_fans.ToString();
                ltlVisiter.Text = wifiNodeInfo.data.list[e.Item.ItemIndex].today_visiter.ToString();

                string urlEdite = BackgroundWifiNodeAdd.GetRedirectUrl(base.PublishmentSystemID, wifiNodeInfo.data.list[e.Item.ItemIndex].id.ToString());
                ltlbtnEdite.Text = string.Format(@"<a href=""{0}"">编辑</a>", urlEdite);

                string urlSelectClient = BackgroundWifiNodeClient.GetRedirectUrl(base.PublishmentSystemID, wifiNodeInfo.data.list[e.Item.ItemIndex].id.ToString());
                ltlSelectClient.Text = string.Format(@"<a href=""{0}"">连接名单</a>", urlSelectClient);

            }
        }

        public string deleteWechatWifi(string NodeID)
        {
            WifiInfo wifiInfo = DataProviderWX.WifiDAO.GetWifiInfoByPublishmentSystemID(base.PublishmentSystemID);

            string postUrl = "http://wifi.wechatwifi.com/api/node/delete";

            NameValueCollection list = new NameValueCollection();

            list.Add("a", "0pqqYvq7j0xqqmq5");
            list.Add("b", wifiInfo.BusinessID.Substring(0, 16));
            list.Add("id", NodeID);

            string retval = "";

            WebClientUtils.Post(postUrl, list, out retval);

            string jsonString = retval.Substring(9, retval.Length - 10);

            return GetRedirectUrl(base.PublishmentSystemID);
        }
    }
}
