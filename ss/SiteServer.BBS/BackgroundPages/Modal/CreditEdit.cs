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
    public class CreditEdit : BackgroundBasePage
    {
        protected Literal ltlCreditID;
        protected TextBox txtCreditName;
        protected TextBox txtCreditUnit;
        protected TextBox txtInitial;
        protected RadioButtonList rblIsUsing;

        private ECreditType creditType;

        public static string GetOpenWindowString(int publishmentSystemID, ECreditType creditType)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("CreditType", ECreditTypeUtils.GetCreditID(creditType));
            return JsUtils.OpenWindow.GetOpenWindowString("编辑积分属性", PageUtils.GetBBSUrl("modal_creditEdit.aspx"), arguments, 330, 400);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.creditType = ECreditTypeUtils.GetEnumType(base.GetQueryString("CreditType"));

            if (!IsPostBack)
            {
                this.ltlCreditID.Text = ECreditTypeUtils.GetCreditID(this.creditType);
                this.txtCreditName.Text = ECreditTypeUtils.GetCreditName(base.PublishmentSystemID, this.creditType);
                this.txtCreditUnit.Text = ECreditTypeUtils.GetCreditUnit(base.PublishmentSystemID, this.creditType);
                this.txtInitial.Text = ECreditTypeUtils.GetCreditInitial(base.PublishmentSystemID, this.creditType).ToString();
                EBooleanUtils.AddListItems(this.rblIsUsing, "启用", "禁用");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsUsing, ECreditTypeUtils.GetIsUsing(base.PublishmentSystemID, this.creditType).ToString());

                this.rblIsUsing.Enabled = !ECreditTypeUtils.IsDefaultCredit(this.creditType);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            try
            {
                string creditName = this.txtCreditName.Text;
                string creditUnit = this.txtCreditUnit.Text;
                int initial = TranslateUtils.ToInt(this.txtInitial.Text);
                bool isUsing = TranslateUtils.ToBool(this.rblIsUsing.SelectedValue);

                if (this.creditType == ECreditType.Prestige)
                {
                    base.Additional.CreditNamePrestige = creditName;
                    base.Additional.CreditUnitPrestige = creditUnit;
                    base.Additional.CreditInitialPrestige = initial;
                }
                else if (this.creditType == ECreditType.Contribution)
                {
                    base.Additional.CreditNameContribution = creditName;
                    base.Additional.CreditUnitContribution = creditUnit;
                    base.Additional.CreditInitialContribution = initial;
                }
                else if (this.creditType == ECreditType.Currency)
                {
                    base.Additional.CreditNameCurrency = creditName;
                    base.Additional.CreditUnitCurrency = creditUnit;
                    base.Additional.CreditInitialCurrency = initial;
                }
                else if (this.creditType == ECreditType.ExtCredit1)
                {
                    base.Additional.CreditNameExtCredit1 = creditName;
                    base.Additional.CreditUnitExtCredit1 = creditUnit;
                    base.Additional.CreditInitialExtCredit1 = initial;
                    base.Additional.CreditUsingExtCredit1 = isUsing;
                }
                else if (this.creditType == ECreditType.ExtCredit2)
                {
                    base.Additional.CreditNameExtCredit2 = creditName;
                    base.Additional.CreditUnitExtCredit2 = creditUnit;
                    base.Additional.CreditInitialExtCredit2 = initial;
                    base.Additional.CreditUsingExtCredit2 = isUsing;
                }
                else if (this.creditType == ECreditType.ExtCredit3)
                {
                    base.Additional.CreditNameExtCredit3 = creditName;
                    base.Additional.CreditUnitExtCredit3 = creditUnit;
                    base.Additional.CreditInitialExtCredit3 = initial;
                    base.Additional.CreditUsingExtCredit3 = isUsing;
                }

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
