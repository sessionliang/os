using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core;
using BaiRong.Text.LitJson;
using BaiRong.Model;
using BaiRong.Core.Data;
using SiteServer.Project.BackgroundPages;

namespace SiteServer.Project.Yun.Pages
{
    public class Validate : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                string callback = PageUtils.FilterSql(base.Request.QueryString["callback"]);
                string domain = PageUtils.FilterSql(base.Request.QueryString["domain"]);

                if (!string.IsNullOrEmpty(domain))
                {
                    domain = domain.ToLower().Trim();
                    domain = StringUtils.ReplaceStartsWith(domain, "http://", string.Empty);
                    domain = domain.Replace("/siteserver", string.Empty);

                    if (string.IsNullOrEmpty(callback))
                    {
                        Hashtable attributes = this.GetHashtable(domain);
                        string jsonString = JsonMapper.ToJson(attributes);
                        base.Response.ContentType = "application/json";
                        base.Response.Write(jsonString);
                        base.Response.End();
                    }
                    else
                    {
                        Hashtable attributes = this.GetHashtable(domain);
                        string jsonString = JsonMapper.ToJson(attributes);
                        base.Response.ContentType = "text/javascript";
                        base.Response.Write(callback + "(" + jsonString + ")");
                        base.Response.End();
                    }
                }
            }
        }

        //{success:'true',orderID:'{0}',redirectUrl:'{1}'}
        public Hashtable GetHashtable(string domain)
        {
            Hashtable attributes = new Hashtable();

            OrderInfo orderInfo = DataProvider.OrderDAO.GetOrderInfo(domain);

            if (orderInfo != null)
            {
                attributes.Add("success", "true");
                attributes.Add("orderID", orderInfo.ID.ToString());
                if (orderInfo.Status == EOrderStatus.New)
                {
                    string redirectUrl = RequestNew.GetRedirectUrl(orderInfo.DomainTemp);
                    attributes.Add("redirectUrl", redirectUrl);
                }
                else if (orderInfo.Status == EOrderStatus.Messaged || orderInfo.Status == EOrderStatus.Formed)
                {
                    bool isInitializationForm = DataProvider.MobanDAO.IsInitializationForm(orderInfo.MobanID);
                    if (isInitializationForm)
                    {
                        string redirectUrl = RequestForm.GetRedirectUrl(orderInfo.DomainTemp);
                        attributes.Add("redirectUrl", redirectUrl);
                        //int orderFormID = 0;
                        //bool isCompleted = DataProvider.OrderFormDAO.IsCompleted(orderInfo.ID, out orderFormID);
                        //if (isCompleted == false)
                        //{
                        //    string redirectUrl = RequestForm.GetRedirectUrl(orderInfo.DomainTemp);
                        //    attributes.Add("redirectUrl", redirectUrl);
                        //}
                    }
                }
            }
            else
            {
                attributes.Add("success", "false");
            }

            return attributes;
        }
    }
}
