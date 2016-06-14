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



namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundOrderServiceAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public BREditor Summary;

        private OrderInfo orderInfo;
        private string returnUrl;

        public static string GetAddUrl(string returnUrl)
        {
            return string.Format("background_orderServiceAdd.aspx?ReturnUrl={0}", StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetEditUrl(int id, string returnUrl)
        {
            return string.Format("background_orderServiceAdd.aspx?ID={0}&ReturnUrl={1}", id, StringUtils.ValueToUrl(returnUrl));
        }

        public override string GetValue(string attributeName)
        {
            string value = base.GetValue(attributeName);
            string retval = value;

            if (attributeName == OrderAttribute.SN)
            {
                if (this.orderInfo == null)
                {
                    return ProjectManager.GetOrderSN(EOrderType.Taobao_Service);
                }
                else
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return ProjectManager.GetOrderSN(EOrderType.Taobao_Service);
                    }
                }
            }
            else if (attributeName == "SelectRelatedOrderID")
            {
                retval = Modal.OrderSelect.GetShowPopWinString("selectRelatedOrderID");
            }
            else if (attributeName == "RelatedOrderSN")
            {
                int relatedOrderID = TranslateUtils.ToInt(base.GetValue(OrderAttribute.RelatedOrderID));
                if (relatedOrderID > 0)
                {
                    retval = DataProvider.OrderDAO.GetOrderSN(relatedOrderID);
                }
                else
                {
                    retval = "选择订单";
                }
            }

            return retval;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int id = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundOrderService.GetRedirectUrl(string.Empty, string.Empty, string.Empty, 1);
            }

            if (id > 0)
            {
                this.orderInfo = DataProvider.OrderDAO.GetOrderInfo(id);
                base.AddAttributes(this.orderInfo);

                this.ltlPageTitle.Text = "修改服务订单";
            }
            else
            {
                this.ltlPageTitle.Text = "新增服务订单";
            }

            string uploadImageUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "Image");
            string uploadScrawlUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "Scrawl");
            string uploadFileUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "File");
            string imageManagerUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "ImageManager");
            string getMovieUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "GetMovie");
            //this.breContent.SetUrl(uploadImageUrl, uploadScrawlUrl, uploadFileUrl, imageManagerUrl, getMovieUrl);

            if (!IsPostBack)
            {
                this.Summary.Text = this.GetValue(OrderAttribute.Summary);
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
                int relatedOrderID = TranslateUtils.ToInt(base.Request.Form[OrderAttribute.RelatedOrderID]);
                OrderInfo relatedOrderInfo = DataProvider.OrderDAO.GetOrderInfo(relatedOrderID);
                if (relatedOrderInfo == null)
                {
                    base.FailMessage("保存失败，请选择关联订单");
                    return;
                }
                if (this.orderInfo != null)
                {
                    OrderInfo orderInfoToEdit = DataProvider.OrderDAO.GetOrderInfo(base.Request.Form);

                    EOrderStatus status = this.orderInfo.Status;

                    foreach (string attributeName in OrderAttribute.BasicAttributes)
                    {
                        if (base.Request.Form[attributeName] == null)
                        {
                            this.orderInfo.SetExtendedAttribute(attributeName, relatedOrderInfo.GetExtendedAttribute(attributeName));
                        }
                        else
                        {
                            this.orderInfo.SetExtendedAttribute(attributeName, orderInfoToEdit.GetExtendedAttribute(attributeName));
                        }
                    }

                    this.orderInfo.OrderType = EOrderType.Taobao_Service;
                    this.orderInfo.Status = status;

                    DataProvider.OrderDAO.Update(this.orderInfo);

                    base.SuccessMessage("订单修改成功");
                }
                else
                {
                    OrderInfo orderInfoToAdd = DataProvider.OrderDAO.GetOrderInfo(base.Request.Form);

                    foreach (string attributeName in OrderAttribute.BasicAttributes)
                    {
                        if (base.Request.Form[attributeName] == null)
                        {
                            orderInfoToAdd.SetExtendedAttribute(attributeName, relatedOrderInfo.GetExtendedAttribute(attributeName));
                        }
                    }

                    orderInfoToAdd.OrderType = EOrderType.Taobao_Service;
                    orderInfoToAdd.Status = EOrderStatus.New;

                    DataProvider.OrderDAO.Insert(orderInfoToAdd);

                    base.SuccessMessage("订单添加成功");
                }
                
                base.AddWaitAndRedirectScript(this.returnUrl);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
