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
    public class BackgroundMobanAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        private MobanInfo mobanInfo;
        private string returnUrl;

        public static string GetAddUrl(string returnUrl)
        {
            return string.Format("background_mobanAdd.aspx?ReturnUrl={0}", StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetEditUrl(int id, string returnUrl)
        {
            return string.Format("background_mobanAdd.aspx?ID={0}&ReturnUrl={1}", id, StringUtils.ValueToUrl(returnUrl));
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
                this.returnUrl = BackgroundMoban.GetRedirectUrl(true, string.Empty, string.Empty, 1);
            }

            if (id > 0)
            {
                this.mobanInfo = DataProvider.MobanDAO.GetMobanInfo(id);
                base.AddAttributes(this.mobanInfo);

                this.ltlPageTitle.Text = "修改模板";
            }
            else
            {
                this.ltlPageTitle.Text = "新增模板";
            }

            if (!IsPostBack)
            {
                
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
                if (this.mobanInfo != null)
                {
                    MobanInfo mobanInfoToEdit = DataProvider.MobanDAO.GetMobanInfo(base.Request.Form);
                    foreach (string attributeName in MobanAttribute.BasicAttributes)
                    {
                        if (base.Request.Form[attributeName] != null)
                        {
                            this.mobanInfo.SetExtendedAttribute(attributeName, mobanInfoToEdit.GetExtendedAttribute(attributeName));
                        }
                    }

                    DataProvider.MobanDAO.Update(this.mobanInfo);
                }
                else
                {
                    MobanInfo mobanInfoToAdd = DataProvider.MobanDAO.GetMobanInfo(base.Request.Form);

                    DataProvider.MobanDAO.Insert(mobanInfoToAdd);

                    base.SuccessMessage("模板添加成功");
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
