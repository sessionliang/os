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
    public class BackgroundOrderAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public BREditor Summary;

        private OrderInfo orderInfo;
        private string returnUrl;

        public static string GetAddUrl(string returnUrl)
        {
            return string.Format("background_orderAdd.aspx?ReturnUrl={0}", StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetEditUrl(int id, string returnUrl)
        {
            return string.Format("background_orderAdd.aspx?ID={0}&ReturnUrl={1}", id, StringUtils.ValueToUrl(returnUrl));
        }

        public override string GetValue(string attributeName)
        {
            string value = base.GetValue(attributeName);
            string retval = value;

            if (attributeName == OrderAttribute.SN)
            {
                if (this.orderInfo == null)
                {
                    return ProjectManager.GetOrderSN(EOrderType.Aliyun_Moban);
                }
                else
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return ProjectManager.GetOrderSN(EOrderType.Aliyun_Moban);
                    }
                }
            }
            else if (attributeName == "SelectMobanID")
            {
                retval = Modal.MobanSelect.GetShowPopWinString("selectMobanID");
            }
            else if (attributeName == "MobanSN")
            {
                retval = base.GetValue(OrderAttribute.MobanID);
                if (string.IsNullOrEmpty(retval))
                {
                    retval = "选择模板";
                }
            }
            else if (attributeName == OrderAttribute.Amount)
            {
                if (this.orderInfo == null)
                {
                    retval = "980.00";
                }
            }
            else if (attributeName == OrderAttribute.Duration)
            {
                if (this.orderInfo == null)
                {
                    retval = "1";
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
                this.returnUrl = BackgroundOrder.GetRedirectUrl(string.Empty, string.Empty, string.Empty, 1);
            }

            if (id > 0)
            {
                this.orderInfo = DataProvider.OrderDAO.GetOrderInfo(id);
                base.AddAttributes(this.orderInfo);

                this.ltlPageTitle.Text = "修改成品网站订单";
            }
            else
            {
                this.ltlPageTitle.Text = "新增成品网站订单";
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
                if (this.orderInfo != null)
                {
                    OrderInfo orderInfoToEdit = DataProvider.OrderDAO.GetOrderInfo(base.Request.Form);
                    foreach (string attributeName in OrderAttribute.BasicAttributes)
                    {
                        if (base.Request.Form[attributeName] != null)
                        {
                            this.orderInfo.SetExtendedAttribute(attributeName, orderInfoToEdit.GetExtendedAttribute(attributeName));
                        }
                    }

                    DataProvider.OrderDAO.Update(this.orderInfo);

                    base.SuccessMessage("订单修改成功");
                }
                else
                {
                    OrderInfo orderInfoToAdd = DataProvider.OrderDAO.GetOrderInfo(base.Request.Form);

                    orderInfoToAdd.OrderType = EOrderType.Aliyun_Moban;

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
