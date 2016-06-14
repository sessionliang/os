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
    public class BackgroundOrderMessageAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        private OrderMessageInfo messageInfo;
        private string returnUrl;

        public static string GetAddUrl(string returnUrl)
        {
            return string.Format("background_orderMessageAdd.aspx?ReturnUrl={0}", StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetEditUrl(int id, string returnUrl)
        {
            return string.Format("background_orderMessageAdd.aspx?ID={0}&ReturnUrl={1}", id, StringUtils.ValueToUrl(returnUrl));
        }

        public string GetOptions(string attributeName)
        {
            StringBuilder builder = new StringBuilder();
            return builder.ToString();
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int id = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = "background_orderMessage.aspx";
            }

            if (id > 0)
            {
                this.messageInfo = DataProvider.OrderMessageDAO.GetOrderMessageInfo(id);

                this.ltlPageTitle.Text = "修改消息";
            }
            else
            {
                this.ltlPageTitle.Text = "新增消息";
            }

            if (!IsPostBack)
            {
                
            }
        }

        public void Return_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.returnUrl);
        }

        public override string GetValue(string attributeName)
        {
            if (this.messageInfo != null)
            {
                object obj = this.messageInfo.GetValue(attributeName);
                if (obj != null)
                {
                    return obj.ToString();
                }
            }
            return string.Empty;
        }

        public string GetSelected(string attributeName, string value)
        {
            if (this.messageInfo != null)
            {
                if (this.messageInfo.GetValue(attributeName).ToString() == value)
                {
                    return @"selected=""selected""";
                }
            }
            return string.Empty;
        }

        public string GetSelected(string attributeName, string value, bool isDefault)
        {
            if (this.messageInfo != null)
            {
                if (this.messageInfo.GetValue(attributeName).ToString() == value)
                {
                    return @"selected=""selected""";
                }
            }
            else
            {
                if (isDefault)
                {
                    return @"selected=""selected""";
                }
            }
            return string.Empty;
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                if (this.messageInfo != null)
                {
                    OrderMessageInfo messageInfoToEdit = new OrderMessageInfo(base.Request.Form);
                    foreach (string attributeName in OrderMessageAttribute.AllAttributes)
                    {
                        if (base.Request.Form[attributeName] != null)
                        {
                            this.messageInfo.SetValue(attributeName, messageInfoToEdit.GetValue(attributeName));
                        }
                    }

                    DataProvider.OrderMessageDAO.Update(this.messageInfo);
                }
                else
                {
                    OrderMessageInfo messageInfoToAdd = new OrderMessageInfo(base.Request.Form);

                    DataProvider.OrderMessageDAO.Insert(messageInfoToAdd);

                    base.SuccessMessage("消息添加成功");
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
