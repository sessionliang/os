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
    public class BackgroundWifiNodeReport : BackgroundBasePageWX
    {

        public DropDownList ddlWifiNode;
        public TextBox tbBeginTime;
        public TextBox tbEndTime;
        public TextBox tbOptionJson;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_wifiNodeReport.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Wifi, "路由器报表", AppManager.WeiXin.Permission.WebSite.Wifi);

                WifiNodeListInfo wifiNodeList = getNodeList();
                if (wifiNodeList == null)
                {
                    base.InfoMessage("请先配置商家信息,再查看路由器报表！");
                    return;
                }
                foreach (List wifiNode in wifiNodeList.data.list)
                {
                    ListItem listitem = new ListItem(wifiNode.name, wifiNode.id);
                    this.ddlWifiNode.Items.Add(listitem);
                }
                string beginTime = "";
                string endTime = "";
                if (base.Request.QueryString["beginTime"] != null && base.Request.QueryString["endTime"] != null)
                {
                    beginTime = base.Request.QueryString["beginTime"];
                    endTime = base.Request.QueryString["endTime"];
                    this.tbBeginTime.Text = beginTime;
                    this.tbEndTime.Text = endTime;
                }
                else
                {
                    this.tbBeginTime.Text = DateTime.Now.AddDays(-7).ToShortDateString();
                    this.tbEndTime.Text = DateTime.Now.ToShortDateString();
                    beginTime = this.tbBeginTime.Text;
                    endTime = this.tbEndTime.Text;
                }
                string wifiNodeID = "";
                if (base.Request.QueryString["wifiNode"] != null)
                {
                    wifiNodeID = base.Request.QueryString["wifiNode"];
                }
                else
                {
                    wifiNodeID = this.ddlWifiNode.SelectedValue;
                }


                if (wifiNodeID == "")
                {
                    base.InfoMessage("请先配置路由器信息,再查看路由器报表！");
                    return;
                }
                string jsonString = getNodeReportInfo(beginTime, endTime, wifiNodeID);
                JavaScriptSerializer js = new JavaScriptSerializer();
                WifiNodeReportInfo wifiNodeReportInfo = js.Deserialize<WifiNodeReportInfo>(jsonString);
                List<ReportList> reportList = wifiNodeReportInfo.data.list;
                if (reportList.Count == 0)
                {
                    base.FailMessage("该路由暂不存在数据报表！");
                    return;
                }
                string fansNum = "";
                string visiterNum = "";
                string onlineNum = "";
                string totalNum = "";
                string newNum = "";
                string newDate = "";
                for (DateTime dt = DateTime.Parse(beginTime); dt <= DateTime.Parse(endTime); dt = dt.AddDays(1))
                {
                    newDate += "'" + dt.ToShortDateString() + "'" + ",";
                    for (int i = 0; i < reportList.Count; i++)
                    {
                        if (DateTime.Parse(DateTime.Now.Year + "-" + reportList[i].created_at).ToShortDateString() == dt.ToShortDateString())
                        {
                            fansNum += reportList[i].fans_cnt + ",";
                            visiterNum += reportList[i].visiter_cnt + ",";
                            onlineNum += reportList[i].online_cnt + ",";
                            totalNum += reportList[i].total_cnt + ",";
                            newNum += reportList[i].new_cnt + ",";
                            break;
                        }
                        else
                        {
                            fansNum += 0 + ",";
                            visiterNum += 0 + ",";
                            onlineNum += 0 + ",";
                            totalNum += 0 + ",";
                            newNum += 0 + ",";
                            break;
                        }
                    }
                }

                string reportHtml = "";
                reportHtml += "{";
                reportHtml += "tooltip: { trigger: 'axis'},";
                reportHtml += "legend: { data: ['粉丝数', '游客数', '在线数', '总量', '新增用户'] },";
                reportHtml += "toolbox: {";
                reportHtml += "show: true,";
                reportHtml += "feature: {";
                reportHtml += "mark: { show: true },";
                reportHtml += "dataView: { show: true, readOnly: false },";
                reportHtml += "magicType: { show: true, type: ['line', 'bar', 'stack', 'tiled'] },";
                reportHtml += "restore: { show: true },";
                reportHtml += "saveAsImage: { show: true }";
                reportHtml += "}";
                reportHtml += "},";
                reportHtml += "calculable: true,";
                reportHtml += "xAxis: [";
                reportHtml += "{";
                reportHtml += "type: 'category',";
                reportHtml += "boundaryGap: false,";
                reportHtml += "data:";
                string aa = "[" + newDate.Substring(0, newDate.Length - 1) + "]";
                reportHtml += "" + aa + "";
                reportHtml += " }";
                reportHtml += "],";
                reportHtml += "yAxis: [{type: 'value'}],";
                reportHtml += "series: [";
                reportHtml += "{";
                reportHtml += "name: '粉丝数',";
                reportHtml += " type: 'line',";
                reportHtml += "stack: '总量',";
                reportHtml += " data: [" + fansNum.Substring(0, fansNum.Length - 1) + "]";
                reportHtml += " },";
                reportHtml += "{";
                reportHtml += "name: '游客数',";
                reportHtml += "type: 'line',";
                reportHtml += "stack: '总量',";
                reportHtml += "data: [" + visiterNum.Substring(0, visiterNum.Length - 1) + "]";
                reportHtml += "},";
                reportHtml += "{";
                reportHtml += "name: '在线数',";
                reportHtml += "type: 'line',";
                reportHtml += "stack: '总量',";
                reportHtml += "data: [" + onlineNum.Substring(0, onlineNum.Length - 1) + "]";
                reportHtml += "},";
                reportHtml += "{";
                reportHtml += "name: '总量',";
                reportHtml += "type: 'line',";
                reportHtml += "stack: '总量',";
                reportHtml += "data: [" + totalNum.Substring(0, totalNum.Length - 1) + "]";
                reportHtml += "},";
                reportHtml += "{";
                reportHtml += "name: '新增用户',";
                reportHtml += "type: 'line',";
                reportHtml += "stack: '总量',";
                reportHtml += "data: [" + newNum.Substring(0, newNum.Length - 1) + "]";
                reportHtml += "}";
                reportHtml += "]";
                reportHtml += "}";

                this.tbOptionJson.Text = reportHtml;
            }
        }

        public string getNodeReportInfo(string beginTime, string endTime, string wifiNodeID)
        {

            WifiInfo wifiInfo = DataProviderWX.WifiDAO.GetWifiInfoByPublishmentSystemID(base.PublishmentSystemID);

            string businessID = wifiInfo.BusinessID;

            string postUrl = "http://wifi.wechatwifi.com/api/node/report";

            NameValueCollection list = new NameValueCollection();

            list.Add("begin_time", beginTime);
            list.Add("end_time", endTime);
            list.Add("id", wifiNodeID.Substring(0, 16));
            list.Add("a", "0pqqYvq7j0xqqmq5");
            list.Add("b", businessID.Substring(0, 16));
            string retval = "";
            WebClientUtils.Post(postUrl, list, out retval);
            string jsonString = retval.Substring(9, retval.Length - 10);
            return jsonString;
        }

        public WifiNodeListInfo getNodeList()
        {
            WifiInfo wifiInfo = DataProviderWX.WifiDAO.GetWifiInfoByPublishmentSystemID(base.PublishmentSystemID);
            if (wifiInfo == null)
            {
                return null;
            }
            string businessID = wifiInfo.BusinessID;

            string postUrl = "http://wifi.wechatwifi.com/api/node/list";

            NameValueCollection list = new NameValueCollection();

            list.Add("page", "1");
            list.Add("a", "0pqqYvq7j0xqqmq5");
            list.Add("b", businessID.Substring(0, 16));

            string retval = "";

            WebClientUtils.Post(postUrl, list, out retval);

            string jsonString = retval.Substring(9, retval.Length - 10);

            JavaScriptSerializer js = new JavaScriptSerializer();

            WifiNodeListInfo wifiNodeInfo = js.Deserialize<WifiNodeListInfo>(jsonString);

            return wifiNodeInfo;
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            base.Response.Redirect(this.PageUrl, true);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = string.Format("background_wifiNodeReport.aspx?publishmentSystemID={0}&wifiNode={1}&beginTime={2}&endTime={3}", base.PublishmentSystemID, this.ddlWifiNode.SelectedValue, this.tbBeginTime.Text, this.tbEndTime.Text);
                }
                return this._pageUrl;
            }
        }
    }
}
