using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core;
using System.Net;
using System.IO;
using System.Web.UI.WebControls;
using System.Collections.Specialized;


using SiteServer.CMS.Core;
using BaiRong.Controls;
namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundShareStatistics : BackgroundBasePage
    {
        public Literal ltlShareCount;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;
        public TextBox txtAddress;
        public Literal ltlPlatform;
        public Literal ltlShareCounts;
        public Literal ltlClickCoutns;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_BShare, "统计信息", AppManager.CMS.Permission.WebSite.BShare);

                if (string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.BshareUUID))
                {
                    string PublishmentSystemID = Request["PublishmentSystemID"];
                    Response.Redirect(PageUtils.GetCMSUrl("background_shareRegister.aspx?PublishmentSystemID=" + PublishmentSystemID));
                }
                DateTime dt = System.DateTime.Today.AddDays(-7);
                DateFrom.Text = Convert.ToString(dt);
                DateTo.Text = Convert.ToString(System.DateTime.Today);
                string uuid = base.PublishmentSystemInfo.Additional.BshareUUID;
                string platformUrl = @"http://www.bshare.cn/api/analytics/" + uuid + "/platform.json";
                string Shareurl = @"http://www.bshare.cn/api/analytics/" + uuid + "/share.json";
                string Clickurl = @"http://www.bshare.cn/api/analytics/" + uuid + "/click.json";
                string username = base.PublishmentSystemInfo.Additional.BshareUserName;
                string password = base.PublishmentSystemInfo.Additional.BsharePassword;
                string usernamePassword = username + ":" + password;
                GetPlatformApi(platformUrl, username, password, usernamePassword);
                GetShareApi(Shareurl, username, password, usernamePassword);
                GetClickApi(Clickurl, username, password, usernamePassword);
            }
           
        }
        public void btnCheck_Click(object sender, EventArgs E)
        {
       
            string uuid = base.PublishmentSystemInfo.Additional.BshareUUID;
            string username = base.PublishmentSystemInfo.Additional.BshareUserName;
            string password = base.PublishmentSystemInfo.Additional.BsharePassword;
            string usernamePassword = username + ":" + password;
            StringBuilder strUrl = new StringBuilder("http://www.bshare.cn/api/analytics/" + uuid + "/platform.json?");
            StringBuilder shareUrl = new StringBuilder("http://www.bshare.cn/api/analytics/" + uuid + "/share.json?");
            StringBuilder clickUrl = new StringBuilder("http://www.bshare.cn/api/analytics/" + uuid + "/click.json?");
            if (!string.IsNullOrEmpty(DateFrom.Text))
            {
                strUrl.Append("dateStart=" + DateFrom.Text + "");
                shareUrl.Append("dateStart=" + DateFrom.Text + "");
                clickUrl.Append("dateStart=" + DateFrom.Text + "");
            }
            if (!string.IsNullOrEmpty(DateTo.Text))
            {
                strUrl.Append("&dateEnd=" + DateTo.Text + "");
                shareUrl.Append("dateStart=" + DateFrom.Text + "");
                clickUrl.Append("dateStart=" + DateFrom.Text + "");
            }
            if (!string.IsNullOrEmpty(txtAddress.Text))
            {
                strUrl.Append("&url=" + txtAddress.Text + "");
                shareUrl.Append("&url=" + txtAddress.Text + "");
                clickUrl.Append("&url=" + txtAddress.Text + "");
            }
            string strUrlTostring = strUrl.ToString();
            string shareUrlTostring = shareUrl.ToString();
            string clickUrlTostring = clickUrl.ToString();
            GetPlatformApi(strUrlTostring, username, password, usernamePassword);
            GetShareApi(shareUrlTostring, username, password, usernamePassword);
            GetClickApi(clickUrlTostring, username, password, usernamePassword);
        }
        public string GetJson(string url, string username, string password, string usernamePassword)
        {
            try
            {
                CredentialCache mycache = new CredentialCache();
                mycache.Add(new Uri(url), "Basic", new NetworkCredential(username, password));
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));
                req.Credentials = mycache;
                req.Method = "GET";
                WebResponse resp = req.GetResponse();
                StreamReader reader = new StreamReader(resp.GetResponseStream());
                string strjson = reader.ReadToEnd();
                return strjson;
             }
            catch
            {
                throw new Exception(string.Format("页面地址“{0}”无法访问！", url));
            }
        }
        public void GetPlatformApi(string platformUrl, string username, string password, string usernamePassword)//返回前10位的分享平台数据。包括分享总数和平台名称。
        {

            string strjson = GetJson(platformUrl, username, password, usernamePassword);
                List<Analytics> attributes = JsManager.ParseJsonStringToName(strjson);
                if (attributes != null)
                {
                    StringBuilder argumentCount = new StringBuilder();
                    StringBuilder argumentmetric = new StringBuilder();
                    foreach (Analytics attribute in attributes)
                    {
                        argumentCount.Append(attribute.Count + ",");
                        argumentmetric.Append(attribute.Metric + "("+attribute.Count+")" + "|");
                    }
                    string count = argumentCount.ToString().TrimEnd(',');
                    string metric = argumentmetric.ToString().TrimEnd('|');
                    string imgUrl = "<img src=\"https://chart.googleapis.com/chart?";
                    StringBuilder strString = new StringBuilder(imgUrl);
                    strString.AppendFormat("cht={0}", "p3"); //图表类型bvs,bvg,p,p3,v
                    strString.AppendFormat("&chs={0}", "540x200"); //图表大小
                    strString.AppendFormat("&chd={0}", "t:" + count); //图表数据值
                    strString.AppendFormat("&chl={0}", metric); //图表参数
                    strString.Append("\" />");
                    string strToString = strString.ToString();
                    ltlPlatform.Text = strToString;
                }
        }
        public void GetShareApi(string Shareurl, string username, string password, string usernamePassword)//返回分享数和日期。
        {
          string strjson = GetJson(Shareurl, username, password, usernamePassword);
          List<Analytics> attributes = JsManager.ParseJsonStringToName(strjson);
            if (attributes != null)
            {
                StringBuilder argumentCount = new StringBuilder();
                StringBuilder argumentmetric = new StringBuilder();
                foreach (Analytics attribute in attributes)
                {
                    argumentCount.Append(attribute.Count + ",");
                    argumentmetric.Append("|" + Convert.ToDateTime(attribute.Metric).Month.ToString() + "/" + Convert.ToDateTime(attribute.Metric).Day.ToString());
                }
                string count = argumentCount.ToString().TrimEnd(',');//返回点击量
                string ycount ="|"+ argumentCount.ToString().Replace(",", "|");
                
                string metric = argumentmetric.ToString() + "|"; ;  //返回时间
                string imgUrl = "<img src=\"https://chart.googleapis.com/chart?";
                StringBuilder strString = new StringBuilder(imgUrl);
                strString.AppendFormat("cht={0}", "bvs"); //图表类型bvs,bvg,p,p3,v
                strString.AppendFormat("&chs={0}", "540x200"); //图表大小
                //0:|7/28|1:|3|2:|3|
                strString.AppendFormat("&chxl=0:{0}2:{1}", metric, ycount); //x,y坐标值
                strString.AppendFormat("&chd={0}", "t:" + count); //图表参数
                strString.AppendFormat("&chxt={0}", "x,y,t");//显示x,y坐标
                strString.AppendFormat("&chbh={0}", "20,40,20");  //柱间距
                strString.Append("\" />");
                string strToString = strString.ToString();
                ltlShareCounts.Text = strToString;
               
            }
        }
        public void GetClickApi(string Clickurl, string username, string password, string usernamePassword)//返回点击数和日期。
        {
           string strjsonClick = GetJson(Clickurl, username, password, usernamePassword);
           List<Analytics> attributes = JsManager.ParseJsonStringToName(strjsonClick);
            if (attributes != null)
            {
                StringBuilder argumentCount = new StringBuilder();
                StringBuilder argumentmetric = new StringBuilder();
                foreach (Analytics attribute in attributes)
                {
                    argumentCount.Append(attribute.Count + ",");
                    argumentmetric.Append("|" + Convert.ToDateTime(attribute.Metric).Month.ToString() + "/" + Convert.ToDateTime(attribute.Metric).Day.ToString());
                }
                string count = argumentCount.ToString().TrimEnd(',');//返回点击量
                string ycount = "|" + argumentCount.ToString().Replace(",", "|");
                string metric = argumentmetric.ToString() + "|"; ;  //返回时间
                string imgUrl = "<img src=\"https://chart.googleapis.com/chart?";
                StringBuilder strString = new StringBuilder(imgUrl);
                strString.AppendFormat("cht={0}", "bvs"); //图表类型bvs,bvg,p,p3,v
                strString.AppendFormat("&chs={0}", "540x200"); //图表大小
                strString.AppendFormat("&chxl=0:{0}2:{1}", metric, ycount); //x,y坐标值
                strString.AppendFormat("&chd={0}", "t:" + count); //图表参数
                strString.AppendFormat("&chxt={0}", "x,y,t");//显示x,y坐标
                strString.AppendFormat("&chbh={0}", "20,40,20");  //柱间距
                strString.Append("\" />");
                string strToString = strString.ToString();
                ltlClickCoutns.Text = strToString;
            }
        }  
    }
}
