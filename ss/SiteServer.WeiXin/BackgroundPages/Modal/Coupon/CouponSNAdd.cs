using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Net;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using SiteServer.CMS.Core;
using System.Web.UI.HtmlControls;
using SiteServer.CMS.Core.Office;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Collections.Generic;
using BaiRong.Core.IO;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class CouponSNAdd : BackgroundBasePage
    {
        public TextBox tbTotalNum;
        public HtmlInputFile hifUpload;

        private int couponID;
        private int flg;

        public static string GetOpenWindowString(int publishmentSystemID, int couponID, int flg)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("couponID", couponID.ToString());
            arguments.Add("flg", flg.ToString());
            return PageUtilityWX.GetOpenWindowString("新增优惠劵数量", "modal_couponSNAdd.aspx", arguments, 400, 300);
        }

        public static string GetOpenUploadWindowString(int publishmentSystemID, int couponID, int flg)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("couponID", couponID.ToString());
            arguments.Add("flg", flg.ToString());
            return PageUtilityWX.GetOpenWindowString("上传优惠劵", "modal_couponSNUpload.aspx", arguments, 400, 300);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.couponID = TranslateUtils.ToInt(base.GetQueryString("couponID"));
            this.flg = TranslateUtils.ToInt(base.GetQueryString("flg"));
            if (!IsPostBack)
            {
                if (this.flg == 0)
                {
                    this.tbTotalNum.ReadOnly = false;
                   
                }
                if (this.flg == 1)
                {
                    this.tbTotalNum.ReadOnly = true;
                   
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {
                if (this.flg == 0)
                {
                    int totalNum = TranslateUtils.ToInt(this.tbTotalNum.Text);
                    if (totalNum > 1000)
                    {
                        base.FailMessage("新增失败，一次最多只能新增1000张优惠劵");
                    }
                    else
                    {
                        DataProviderWX.CouponSNDAO.Insert(base.PublishmentSystemID, this.couponID, totalNum);

                        StringUtility.AddLog(base.PublishmentSystemID, "新增优惠劵数量", string.Format("数量:{0}", totalNum));
                        isChanged = true;
                    }
                }
                if (this.flg == 1)
                {
                    //string filehou = this.hifUpload.PostedFile.FileName.Split('.')[1].ToString();

                    //if (filehou != "xls" || filehou != "xlsx")
                    //{
                    //    base.FailMessage("请检查上传文件的类型.必须为EXCEL文件.");
                    //}

                    string filePath = PathUtils.GetTemporaryFilesPath("coupon_sn_upload.csv");
                    FileUtils.DeleteFileIfExists(filePath);

                    this.hifUpload.PostedFile.SaveAs(filePath);

                    try
                    {
                        List<string> snList = CSVUtils.ReadCSVFile(filePath);

                        if (snList.Count > 0)
                        {
                            DataProviderWX.CouponSNDAO.Insert(base.PublishmentSystemID, this.couponID, snList);
                            StringUtility.AddLog(base.PublishmentSystemID, "新增优惠劵数量", string.Format("数量:{0}", snList.Count));
                            isChanged = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "失败：" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "失败：" + ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
