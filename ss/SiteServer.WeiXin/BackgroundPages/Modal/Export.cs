using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using System.Data.OleDb;
using SiteServer.CMS.BackgroundPages;
using System.Web.UI;
using System.Collections.Generic;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.IO;
using System.Web;
using SiteServer.CMS.Core.Office;
using BaiRong.Core.IO;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class Export : BackgroundBasePage
    {
        public static string GetOpenWindowStringByLottery(int publishmentSystemID, ELotteryType lotteryType, int lotteryID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("lotteryType", ELotteryTypeUtils.GetValue(lotteryType));
            arguments.Add("lotteryID", lotteryID.ToString());

            return JsUtils.OpenWindow.GetOpenWindowString("导出CSV", "modal_export.aspx", arguments, 400, 240, true);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                ELotteryType lotteryType = ELotteryTypeUtils.GetEnumType(base.GetQueryString("lotteryType"));
                int lotteryID = TranslateUtils.ToInt(base.GetQueryString("lotteryID"));

                List<LotteryWinnerInfo> winnerInfoList = DataProviderWX.LotteryWinnerDAO.GetWinnerInfoList(base.PublishmentSystemID, lotteryType, lotteryID);

                if (winnerInfoList.Count == 0)
                {
                    base.FailMessage("暂无数据导出！");
                    return;
                }

                string docFileName = "获奖名单" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                string filePath = PathUtils.GetTemporaryFilesPath(docFileName);

                this.ExportLotteryCSV(filePath, winnerInfoList);

                string fileUrl = PageUtils.GetTemporaryFilesUrl(docFileName);
                base.SuccessMessage(string.Format(@"成功导出文件，请点击 <a href=""{0}"">这里</a> 进行下载！", fileUrl));
            }
        }


        private Dictionary<int, LotteryAwardInfo> awardInfoMap = new Dictionary<int, LotteryAwardInfo>();

        public void ExportLotteryCSV(string filePath, List<LotteryWinnerInfo> winnerInfoList)
        {
            List<string> nameList = new List<string>();
            nameList.Add("序号");
            nameList.Add("奖项");
            nameList.Add("姓名");
            nameList.Add("手机");
            nameList.Add("邮箱");
            nameList.Add("地址");
            nameList.Add("状态");
            nameList.Add("中奖时间");
            nameList.Add("兑奖码");
            nameList.Add("兑奖时间");

            List<List<string>> valueListOfList = new List<List<string>>();

            int index = 1;
            foreach (LotteryWinnerInfo winnerInfo in winnerInfoList)
            {
                LotteryAwardInfo awardInfo = null;
                if (this.awardInfoMap.ContainsKey(winnerInfo.AwardID))
                {
                    awardInfo = this.awardInfoMap[winnerInfo.AwardID];
                }
                else
                {
                    awardInfo = DataProviderWX.LotteryAwardDAO.GetAwardInfo(winnerInfo.AwardID);
                    this.awardInfoMap.Add(winnerInfo.AwardID, awardInfo);
                }

                string award = string.Empty;
                if (awardInfo != null)
                {
                    award = awardInfo.AwardName + "：" + awardInfo.Title;
                }

                List<string> valueList = new List<string>();

                valueList.Add((index++).ToString());
                valueList.Add(award);
                valueList.Add(winnerInfo.RealName);
                valueList.Add(winnerInfo.Mobile);
                valueList.Add(winnerInfo.Email);
                valueList.Add(winnerInfo.Address);
                valueList.Add(EWinStatusUtils.GetText(EWinStatusUtils.GetEnumType(winnerInfo.Status)));
                valueList.Add(DateUtils.GetDateAndTimeString(winnerInfo.AddDate));
                valueList.Add(winnerInfo.CashSN);
                valueList.Add(DateUtils.GetDateAndTimeString(winnerInfo.CashDate));

                valueListOfList.Add(valueList);
            }

            CSVUtils.ExportCSVFile(filePath, nameList, valueListOfList);
            
            //StringWriter sw = new StringWriter(sb);
            //sw.Close();
            //context.Response.Clear();
            ///* 
            // * Acme 2012-07-04 edit 
            // *  
            //context.Response.Charset = "gb2312"; 
            //context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312"); 
            //context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; // "application/octet-stream"; //"application/vnd.ms-excel"; //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet                   
            ////context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName)); 
            //*/
            ////below is new writing  
            //context.Response.Charset = "UTF-8";
            //context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //context.Response.HeaderEncoding = System.Text.Encoding.UTF8;
            //context.Response.ContentType = "text/csv";
            ////主要是下面这一句
            //context.Response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });
            //context.Response.Write(sw);
            //context.Response.AppendHeader("content-disposition", "attachment; filename=" + HttpUtility.UrlEncode(exportFileName + ".csv", System.Text.Encoding.UTF8).Replace("+", "%20"));
            ////context.Response.OutputStream.Write(fileData, 0, fileData.Length);  
            //context.Response.Flush();
            //context.Response.End();
   
        }
    }
}