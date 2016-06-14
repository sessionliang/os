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
using BaiRong.Model;
using BaiRong.Core.IO;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class ExportCardSN : BackgroundBasePage
    {
        public static string GetOpenWindowString(int publishmentSystemID, int cardID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("cardID", cardID.ToString());

            return JsUtils.OpenWindow.GetOpenWindowString("导出CSV", "modal_exportCardSN.aspx", arguments, 400, 240, true);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            { 
                int cardID = TranslateUtils.ToInt(base.GetQueryString("cardID"));

                List<CardSNInfo> cardSNInfoList = DataProviderWX.CardSNDAO.GetCardSNInfoList(base.PublishmentSystemID, cardID);

                if (cardSNInfoList.Count == 0)
                {
                    base.FailMessage("暂无数据导出！");
                    return;
                }

                string docFileName = "会员卡名单" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                string filePath = PathUtils.GetTemporaryFilesPath(docFileName);

                this.ExportCardSNCSV(filePath, cardSNInfoList);

                string fileUrl = PageUtils.GetTemporaryFilesUrl(docFileName);
                base.SuccessMessage(string.Format(@"成功导出文件，请点击 <a href=""{0}"">这里</a> 进行下载！", fileUrl));
            }
        }
 
        public void ExportCardSNCSV(string filePath, List<CardSNInfo> cardSNInfoList)
        {
            List<string> nameList = new List<string>();
            nameList.Add("序号");
            nameList.Add("卡号");
            nameList.Add("姓名");
            nameList.Add("手机");
            nameList.Add("邮箱");
            nameList.Add("地址");
            nameList.Add("金额");
            nameList.Add("积分");
            nameList.Add("领卡时间");
            
            List<List<string>> valueListOfList = new List<List<string>>();

            int index = 1;
            foreach (CardSNInfo cardSNInfo in cardSNInfoList)
            {
                UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(base.PublishmentSystemInfo.GroupSN, cardSNInfo.UserName);
                UserContactInfo userContactInfo = BaiRongDataProvider.UserContactDAO.GetContactInfo(cardSNInfo.UserName);

                List<string> valueList = new List<string>();

                valueList.Add((index++).ToString());
                valueList.Add(cardSNInfo.SN);
                valueList.Add(userInfo != null ? userInfo.DisplayName : string.Empty);
                valueList.Add(userInfo != null ? userInfo.Mobile : string.Empty);
                valueList.Add(userInfo != null ? userInfo.Email : string.Empty);
                valueList.Add(userContactInfo != null ? userContactInfo.Address : string.Empty);
                valueList.Add(cardSNInfo.Amount.ToString());
                valueList.Add(userInfo != null ? userInfo.Credits.ToString() : "0");
                valueList.Add(DateUtils.GetDateAndTimeString(cardSNInfo.AddDate));
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