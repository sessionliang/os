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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundWifiNodeAdd : BackgroundBasePage
    {

        public Literal ltlPageTitle;

        public PlaceHolder phStep1;

        public TextBox tbNodeID;
        public TextBox tbNodeName;
        public TextBox tbWechatID;
        public CheckBox cbIsWxBindType;

        public Literal ltlImageUrl;

        public Button btnSubmit;
        public Button btnReturn;


        public HtmlInputHidden imageUrl;

        private string nodeID;

        public static string GetRedirectUrl(int publishmentSystemID, string nodeID)
        {
            return PageUtils.GetWXUrl(string.Format("background_wifiNodeAdd.aspx?publishmentSystemID={0}&nodeID={1}", publishmentSystemID, nodeID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.nodeID = base.GetQueryString("nodeID");

            if (!IsPostBack)
            {
                string pageTitle = this.nodeID != "" ? "编辑路由器" : "添加路由器";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Wifi, pageTitle, AppManager.WeiXin.Permission.WebSite.Wifi);
                this.ltlPageTitle.Text = pageTitle;

                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", WifiManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));

                AccountInfo accountInfo = WeiXinManager.GetAccountInfo(base.PublishmentSystemID);

                this.tbWechatID.Text = accountInfo.WeChatID;

                WifiInfo wifiInfo = DataProviderWX.WifiDAO.GetWifiInfoByPublishmentSystemID(base.PublishmentSystemID);

                if (this.nodeID != "0")
                {

                    string businessInfoJson = GetWifiNodeDetailInfo(wifiInfo.BusinessID, this.nodeID);

                    string jsonString = businessInfoJson.Substring(9, businessInfoJson.Length - 10);

                    JavaScriptSerializer js = new JavaScriptSerializer();

                    WifiNodeDetailInfo wifiNodeDetailInfo = js.Deserialize<WifiNodeDetailInfo>(jsonString);

                    this.tbNodeID.Text = wifiNodeDetailInfo.data.id;
                    this.tbNodeName.Text = wifiNodeDetailInfo.data.name;
                    this.tbWechatID.Text = wifiNodeDetailInfo.data.wechat_id;
                    this.cbIsWxBindType.Checked = false;
                    if (wifiNodeDetailInfo.data.wx_bind == "1")
                    {
                        this.cbIsWxBindType.Checked = true;
                    }
                }
            }

            this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundWifiNode.GetRedirectUrl(base.PublishmentSystemID)));
        }


        public string GetWifiNodeDetailInfo(string businessID, string nodeID)
        {
            string postUrl = "http://wifi.wechatwifi.com/api/node/info";

            NameValueCollection list = new NameValueCollection();

            list.Add("page", "1");
            list.Add("a", "0pqqYvq7j0xqqmq5");
            list.Add("b", businessID.Substring(0, 16).ToString());
            list.Add("id", nodeID);

            string retval = "";

            WebClientUtils.Post(postUrl, list, out retval);

            return retval;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    WifiInfo newWifiInfo = DataProviderWX.WifiDAO.GetWifiInfoByPublishmentSystemID(base.PublishmentSystemID);

                    string postUrl = "http://wifi.wechatwifi.com/api/node/create";
                    if (this.nodeID != "0")
                    {
                        postUrl = "http://wifi.wechatwifi.com/api/node/edit";
                    }
                    string welcome_img = "http://bbs.gexia.com/upload/2014/8/19154613962.jpg"; ;

                    NameValueCollection list = new NameValueCollection();
                    int wx_bind_type = 2;
                    list.Add("gw_id", this.tbNodeID.Text);
                    list.Add("gw_name", this.tbNodeName.Text);
                    list.Add("public_wechat_id", this.tbWechatID.Text);
                    list.Add("qrcode_url", welcome_img);
                    if (this.cbIsWxBindType.Checked)
                    {
                        wx_bind_type = 1;
                    }

                    list.Add("wx_bind_type", wx_bind_type.ToString());
                    list.Add("a", "0pqqYvq7j0xqqmq5");
                    list.Add("b", newWifiInfo.BusinessID.Substring(0, 16));
                    string retval = "";
                    WebClientUtils.Post(postUrl, list, out retval);
                    string jsonString = retval.Substring(9, retval.Length - 10);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    WifiNodeResult wifiNodeResultInfo = js.Deserialize<WifiNodeResult>(jsonString);
                    int result = wifiNodeResultInfo.ret;
                    if (result < 0)
                    {
                        base.FailMessage("" + wifiNodeResultInfo.msg + "");
                        return;
                    }
                    WifiNodeInfo wifiNodeInfo = new WifiNodeInfo();
                    wifiNodeInfo.BusinessID = newWifiInfo.BusinessID;
                    wifiNodeInfo.NodeID = this.tbNodeID.Text;
                    wifiNodeInfo.CallBackString = retval;
                    wifiNodeInfo.PublishmentSystemID = base.PublishmentSystemID;
                    if (this.nodeID != "0")
                    {
                        DataProviderWX.WifiNodeDAO.Update(wifiNodeInfo);
                    }
                    else
                    {
                        DataProviderWX.WifiNodeDAO.Insert(wifiNodeInfo);
                    }

                    base.SuccessMessage("添加路由成功！");
                    string redirectUrl = PageUtils.GetWXUrl(string.Format("background_wifiNode.aspx?publishmentSystemID={0}", base.PublishmentSystemID));
                    base.AddWaitAndRedirectScript(redirectUrl);

                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "路由设置失败！");
                }

            }
        }
    }
}
