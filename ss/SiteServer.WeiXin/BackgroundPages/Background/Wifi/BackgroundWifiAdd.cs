using BaiRong.Core;
using BaiRong.Core.Net;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;
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
    public class BackgroundWifiAdd : BackgroundBasePageWX
    {
        public Literal ltlPageTitle;

        public PlaceHolder phStep1;

        public TextBox tbKeywords;
        public TextBox tbTitle;
        public TextBox tbSummary;
        public CheckBox cbIsEnabled;
        public TextBox tbIntroduction;
        public TextBox tbContactAddress;
        public TextBox tbContactQQ;
        public TextBox tbPassword;

        public Literal ltlImageUrl;

        public Button btnSubmit;

        public HtmlInputHidden imageUrl;

        private int wifiID;

        public static string GetRedirectUrl(int publishmentSystemID, int wifiID)
        {
            return PageUtils.GetWXUrl(string.Format("background_wifiAdd.aspx?publishmentSystemID={0}&wifiID={1}", publishmentSystemID, wifiID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }


        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.wifiID = TranslateUtils.ToInt(base.GetQueryString("wifiID"));

            WifiInfo newWifiInfo = DataProviderWX.WifiDAO.GetWifiInfoByPublishmentSystemID(base.PublishmentSystemID);

            int returnWifiID = 0;

            if (newWifiInfo != null)
            {
                returnWifiID = newWifiInfo.ID;
            }
            this.wifiID = returnWifiID;

            if (!IsPostBack)
            {

                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Wifi, "商家设置", AppManager.WeiXin.Permission.WebSite.Wifi);
                this.ltlPageTitle.Text = "商家设置";
                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", WifiManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));

                if (this.wifiID > 0)
                {
                    WifiInfo wifiInfo = DataProviderWX.WifiDAO.GetWifiInfo(this.wifiID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(wifiInfo.KeywordID);
                    this.cbIsEnabled.Checked = !wifiInfo.IsDisabled;
                    this.tbTitle.Text = wifiInfo.Title;
                    if (!string.IsNullOrEmpty(wifiInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, wifiInfo.ImageUrl));
                    }

                    this.tbSummary.Text = wifiInfo.Summary;
                    this.imageUrl.Value = wifiInfo.ImageUrl;

                    string businessInfoJson = GetWifiBusinessInfo(wifiInfo.BusinessID);

                    string jsonString = businessInfoJson.Substring(9, businessInfoJson.Length - 10);

                    JavaScriptSerializer js = new JavaScriptSerializer();

                    WifiBusinessInfo wifiBusinessInfo = js.Deserialize<WifiBusinessInfo>(jsonString);

                    this.tbContactQQ.Text = wifiBusinessInfo.data.contact_qq;
                    this.tbContactAddress.Text = wifiBusinessInfo.data.contact_address;
                    this.tbIntroduction.Text = wifiBusinessInfo.data.introduction;
                }
            }
        }

        public string GetWifiBusinessInfo(string businessID)
        {

            string postUrl = " http://wifi.wechatwifi.com/api/business/info";

            NameValueCollection list = new NameValueCollection();

            list.Add("page", "1");
            list.Add("a", "0pqqYvq7j0xqqmq5OkTfOlzc");
            list.Add("b", businessID.Substring(0, 16).ToString());

            string retval = "";

            WebClientUtils.Post(postUrl, list, out retval);

            return retval;

        }


        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {

                bool isConflict = false;
                string conflictKeywords = string.Empty;
                if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                {
                    if (this.wifiID > 0)
                    {
                        WifiInfo wifiInfo = DataProviderWX.WifiDAO.GetWifiInfo(this.wifiID);
                        isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, wifiInfo.KeywordID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                    }
                    else
                    {
                        isConflict = KeywordManager.IsKeywordInsertConflict(base.PublishmentSystemID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                    }
                }

                if (isConflict)
                {
                    base.FailMessage(string.Format("触发关键词“{0}”已存在，请设置其他关键词", conflictKeywords));
                    this.phStep1.Visible = true;
                }
                else
                {
                    WifiInfo wifiInfo = new WifiInfo();

                    if (this.wifiID > 0)
                    {
                        wifiInfo = DataProviderWX.WifiDAO.GetWifiInfo(this.wifiID);
                    }
                    wifiInfo.PublishmentSystemID = base.PublishmentSystemID;

                    wifiInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordID(base.PublishmentSystemID, this.wifiID > 0, this.tbKeywords.Text, EKeywordType.Wifi, wifiInfo.KeywordID);
                    wifiInfo.IsDisabled = !this.cbIsEnabled.Checked;

                    wifiInfo.Title = PageUtils.FilterXSS(this.tbTitle.Text);
                    wifiInfo.ImageUrl = this.imageUrl.Value;
                    wifiInfo.Summary = this.tbSummary.Text;

                    try
                    {
                        string welcome_img = "http://bbs.gexia.com/upload/2014/8/19154613962.jpg"; ;  //欢迎图片
                        string other_img = "http://bbs.gexia.com/upload/2014/8/19154613962.jpg"; ;    //成功图片

                        NameValueCollection list = new NameValueCollection();

                        list.Add("reply_content", this.tbKeywords.Text);
                        list.Add("name", "");
                        list.Add("password", this.tbPassword.Text);
                        list.Add("contact_qq", this.tbContactQQ.Text);
                        list.Add("contact_email", "");
                        list.Add("contact_phone", "");
                        list.Add("contact_address", this.tbContactAddress.Text);
                        list.Add("introduction", this.tbIntroduction.Text);
                        list.Add("welcome_img", welcome_img);
                        list.Add("other_img", other_img);
                        list.Add("a", "0pqqYvq7j0xqqmq5");

                        if (this.wifiID > 0)
                        {

                            string postUrl = "http://wifi.wechatwifi.com/api/business/edit";

                            string retval = "";

                            list.Add("b", wifiInfo.BusinessID.Substring(0, 16).ToString());
                            //list.Add("b", "jtyqSNqhTjIyqIqF");
                            WebClientUtils.Post(postUrl, list, out retval);

                            DataProviderWX.WifiDAO.Update(wifiInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "商家配置", string.Format("微Wifi:{0}", this.tbTitle.Text));
                            base.SuccessMessage("商家配置成功！");
                        }
                        else
                        {

                            string postUrl = "http://wifi.wechatwifi.com/api/business/create";

                            string retval = "";

                            WebClientUtils.Post(postUrl, list, out retval);

                            string jsonString = retval.Substring(9, retval.Length - 10);

                            JavaScriptSerializer js = new JavaScriptSerializer();

                            WifiBusinessInfo wifiBusinessInfo = js.Deserialize<WifiBusinessInfo>(jsonString);

                            wifiInfo.BusinessID = wifiBusinessInfo.data.business_id;
                            wifiInfo.CallBackString = retval;

                            this.wifiID = DataProviderWX.WifiDAO.Insert(wifiInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "商家配置", string.Format("微WIfi:{0}", this.tbTitle.Text));
                            base.SuccessMessage("商家配置成功！");
                        }

                        string redirectUrl = PageUtils.GetWXUrl(string.Format("background_WifiAdd.aspx?publishmentSystemID={0}&wifiID={1}", base.PublishmentSystemID, this.wifiID));
                        base.AddWaitAndRedirectScript(redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "商家配置失败！");
                    }
                }

            }
        }
    }
}
