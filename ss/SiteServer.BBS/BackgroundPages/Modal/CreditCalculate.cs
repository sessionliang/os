using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using SiteServer.BBS.Core;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class CreditCalculate : BackgroundBasePage
    {
        protected TextBox txtPostCount;
        protected TextBox txtPostDigestCount;
        protected Literal ltlNamePrestige;
        protected TextBox txtPrestige;
        protected Literal ltlNameContribution;
        protected TextBox txtContribution;
        protected Literal ltlNameCurrency;
        protected TextBox txtCurrency;
        protected PlaceHolder phExtCredit1;
        protected Literal ltlNameExtCredit1;
        protected TextBox txtExtCredit1;
        protected PlaceHolder phExtCredit2;
        protected Literal ltlNameExtCredit2;
        protected TextBox txtExtCredit2;
        protected PlaceHolder phExtCredit3;
        protected Literal ltlNameExtCredit3;
        protected TextBox txtExtCredit3;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("设置总积分计算公式", PageUtils.GetBBSUrl("modal_creditCalculate.aspx"), arguments, 330, 380);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                this.txtPostCount.Text = base.Additional.CreditMultiplierPostCount.ToString();
                this.txtPostDigestCount.Text = base.Additional.CreditMultiplierPostDigestCount.ToString();

                this.ltlNamePrestige.Text = base.Additional.CreditNamePrestige;
                this.txtPrestige.Text = base.Additional.CreditMultiplierPrestige.ToString();
                this.ltlNameContribution.Text = base.Additional.CreditNameContribution;
                this.txtContribution.Text = base.Additional.CreditMultiplierContribution.ToString();
                this.ltlNameCurrency.Text = base.Additional.CreditNameCurrency;
                this.txtCurrency.Text = base.Additional.CreditMultiplierCurrency.ToString();

                this.phExtCredit1.Visible = base.Additional.CreditUsingExtCredit1;
                this.ltlNameExtCredit1.Text = base.Additional.CreditNameExtCredit1;
                this.txtExtCredit1.Text = base.Additional.CreditMultiplierExtCredit1.ToString();

                this.phExtCredit2.Visible = base.Additional.CreditUsingExtCredit2;
                this.ltlNameExtCredit2.Text = base.Additional.CreditNameExtCredit2;
                this.txtExtCredit2.Text = base.Additional.CreditMultiplierExtCredit2.ToString();

                this.phExtCredit3.Visible = base.Additional.CreditUsingExtCredit3;
                this.ltlNameExtCredit3.Text = base.Additional.CreditNameExtCredit3;
                this.txtExtCredit3.Text = base.Additional.CreditMultiplierExtCredit3.ToString();
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            try
            {
                base.Additional.CreditMultiplierPostCount = TranslateUtils.ToInt(this.txtPostCount.Text);

                ConfigurationManager.Update(base.PublishmentSystemID);

                isChanged = true;
            }
            catch(Exception ex)
            {
                base.FailMessage(ex, "编辑积分属性出错！");
            }
            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundConfigurationCredit1.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}
