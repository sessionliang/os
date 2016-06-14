using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.Project.Controls;
using System.Collections.Specialized;


using System.Web;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundOrderRefundAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        public Literal ltlOrderSN;
        public Literal ltlLoginName;

        private OrderInfo orderInfo;
        private OrderRefundInfo refundInfo;
        private int orderID;
        private string returnUrl;

        public static string GetRedirectUrl(int orderID, string returnUrl)
        {
            return string.Format("background_orderRefundAdd.aspx?OrderID={0}&ReturnUrl={1}", orderID, StringUtils.ValueToUrl(returnUrl));
        }

        public override string GetValue(string attributeName)
        {
            if (attributeName == OrderRefundAttribute.AliyunFileUrl)
            {
                if (!string.IsNullOrEmpty(base.GetValue(attributeName)))
                {
                    return string.Format(@"<a href=""{0}"" target=""_blank"">下载</a>", PageUtils.ParseNavigationUrl(base.GetValue(attributeName)));
                }
            }
            return base.GetValue(attributeName);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.orderID = TranslateUtils.ToInt(Request.QueryString["OrderID"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundOrderRefund.GetRedirectUrl(true, string.Empty, 1);
            }

            this.orderInfo = DataProvider.OrderDAO.GetOrderInfo(this.orderID);
            this.refundInfo = DataProvider.OrderRefundDAO.GetOrderRefundInfoByOrderID(this.orderID);

            if (this.refundInfo != null)
            {
                base.AddAttributes(this.refundInfo);
                this.ltlPageTitle.Text = "修改退款";
            }
            else
            {
                this.ltlPageTitle.Text = "新增退款";
            }

            if (!IsPostBack)
            {
                this.ltlOrderSN.Text = this.orderInfo.SN;
                this.ltlLoginName.Text = this.orderInfo.LoginName;
            }
        }

        public void Return_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.returnUrl);
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                if (this.refundInfo != null)
                {
                    OrderRefundInfo refundInfoToEdit = DataProvider.OrderRefundDAO.GetOrderRefundInfo(this.orderID, base.Request.Form);
                    foreach (string attributeName in OrderRefundAttribute.BasicAttributes)
                    {
                        if (base.Request.Form[attributeName] != null)
                        {
                            this.refundInfo.SetExtendedAttribute(attributeName, refundInfoToEdit.GetExtendedAttribute(attributeName));
                        }
                    }

                    if (base.Request.Files != null && base.Request.Files.Count > 0)
                    {
                        foreach (string attributeName in base.Request.Files.AllKeys)
                        {
                            HttpPostedFile myFile = base.Request.Files[attributeName];
                            if (myFile != null && "" != myFile.FileName)
                            {
                                string fileUrl = this.UploadFile(myFile);
                                this.refundInfo.SetExtendedAttribute(attributeName, fileUrl);
                            }
                        }
                    }

                    this.refundInfo.OrderSN = this.orderInfo.SN;
                    this.refundInfo.LoginName = this.orderInfo.LoginName;

                    DataProvider.OrderRefundDAO.Update(this.refundInfo);
                }
                else
                {
                    OrderRefundInfo refundInfoToAdd = DataProvider.OrderRefundDAO.GetOrderRefundInfo(this.orderID, base.Request.Form);

                    if (base.Request.Files != null && base.Request.Files.Count > 0)
                    {
                        foreach (string attributeName in base.Request.Files.AllKeys)
                        {
                            HttpPostedFile myFile = base.Request.Files[attributeName];
                            if (myFile != null && "" != myFile.FileName)
                            {
                                string fileUrl = this.UploadFile(myFile);
                                refundInfoToAdd.SetExtendedAttribute(attributeName, fileUrl);
                            }
                        }
                    }

                    refundInfoToAdd.OrderSN = this.orderInfo.SN;
                    refundInfoToAdd.LoginName = this.orderInfo.LoginName;

                    DataProvider.OrderRefundDAO.Insert(refundInfoToAdd);

                    base.SuccessMessage("退款添加成功");
                }

                DataProvider.OrderDAO.UpdateIsRefund(this.orderID, true);
                
                base.AddWaitAndRedirectScript(this.returnUrl);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        private string UploadFile(HttpPostedFile myFile)
        {
            string fileUrl = string.Empty;

            string filePath = myFile.FileName;
            try
            {
                string fileExtName = PathUtils.GetExtension(filePath);
                string localDirectoryPath = PathUtils.GetUserUploadDirectoryPath(this.orderInfo.DomainTemp);
                string localFileName = PathUtils.GetUserUploadFileName(filePath);

                string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                myFile.SaveAs(localFilePath);

                fileUrl = PageUtils.GetRootUrlByPhysicalPath(localFilePath);
            }
            catch { }

            return fileUrl;
        }
    }
}
