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
    public class CreditRule : BackgroundBasePage
    {
        protected Literal ltlRuleName;
        protected RadioButtonList PeriodType;
        protected PlaceHolder phPeriodCount;
        protected TextBox PeriodCount;
        protected TextBox MaxNum;
        protected Literal ltlNamePrestige;
        protected TextBox Prestige;
        protected Literal ltlNameContribution;
        protected TextBox Contribution;
        protected Literal ltlNameCurrency;
        protected TextBox Currency;
        protected PlaceHolder phExtCredit1;
        protected Literal ltlNameExtCredit1;
        protected TextBox ExtCredit1;
        protected PlaceHolder phExtCredit2;
        protected Literal ltlNameExtCredit2;
        protected TextBox ExtCredit2;
        protected PlaceHolder phExtCredit3;
        protected Literal ltlNameExtCredit3;
        protected TextBox ExtCredit3;

        private ECreditRule creditRule;
        private int forumID;

        public static string GetOpenWindowString(int publishmentSystemID, ECreditRule creditRule, int forumID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("CreditRule", ECreditRuleUtils.GetValue(creditRule));
            arguments.Add("ForumID", forumID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("积分策略设置", PageUtils.GetBBSUrl("modal_creditRule.aspx"), arguments, 400, 500);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.creditRule = ECreditRuleUtils.GetEnumType(base.GetQueryString("CreditRule"));
            this.forumID = base.GetIntQueryString("ForumID");
            CreditRuleInfo crInfo = CreditRuleManager.GetCreditRuleInfo(base.PublishmentSystemID, this.creditRule, this.forumID);

            if (!IsPostBack)
            {
                this.ltlRuleName.Text = ECreditRuleUtils.GetText(this.creditRule);
                EPeriodTypeUtils.AddListItems(this.PeriodType);
                ControlUtils.SelectListItemsIgnoreCase(this.PeriodType, EPeriodTypeUtils.GetValue(crInfo.PeriodType).ToString());
                if (crInfo.PeriodType == EPeriodType.Hour || crInfo.PeriodType == EPeriodType.Minute)
                {
                    this.phPeriodCount.Visible = true;
                    this.PeriodCount.Text = crInfo.PeriodCount.ToString();
                }
                else
                {
                    this.phPeriodCount.Visible = false;
                }
                this.MaxNum.Text = crInfo.MaxNum.ToString();
                this.ltlNamePrestige.Text = base.Additional.CreditNamePrestige;
                this.Prestige.Text = crInfo.Prestige.ToString();
                this.ltlNameContribution.Text = base.Additional.CreditNameContribution;
                this.Contribution.Text = crInfo.Contribution.ToString();
                this.ltlNameCurrency.Text = base.Additional.CreditNameCurrency;
                this.Currency.Text = crInfo.Currency.ToString();
                if (base.Additional.CreditUsingExtCredit1)
                {
                    this.phExtCredit1.Visible = true;
                    this.ltlNameExtCredit1.Text = base.Additional.CreditNameExtCredit1;
                    this.ExtCredit1.Text = crInfo.ExtCredit1.ToString();
                }
                else
                {
                    this.phExtCredit1.Visible = false;
                }
                if (base.Additional.CreditUsingExtCredit2)
                {
                    this.phExtCredit2.Visible = true;
                    this.ltlNameExtCredit2.Text = base.Additional.CreditNameExtCredit2;
                    this.ExtCredit2.Text = crInfo.ExtCredit2.ToString();
                }
                else
                {
                    this.phExtCredit2.Visible = false;
                }
                if (base.Additional.CreditUsingExtCredit3)
                {
                    this.phExtCredit3.Visible = true;
                    this.ltlNameExtCredit3.Text = base.Additional.CreditNameExtCredit3;
                    this.ExtCredit3.Text = crInfo.ExtCredit3.ToString();
                }
                else
                {
                    this.phExtCredit3.Visible = false;
                }
            }
        }

        public void PeriodType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EPeriodType type = EPeriodTypeUtils.GetEnumType(this.PeriodType.SelectedValue);
            if (type == EPeriodType.Hour || type == EPeriodType.Minute)
            {
                this.phPeriodCount.Visible = true;
            }
            else
            {
                this.phPeriodCount.Visible = false;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            try
            {
                CreditRuleInfo crInfo = CreditRuleManager.GetCreditRuleInfo(base.PublishmentSystemID, this.creditRule, this.forumID);
                crInfo.PeriodType = EPeriodTypeUtils.GetEnumType(this.PeriodType.SelectedValue);
                crInfo.PeriodCount = TranslateUtils.ToInt(this.PeriodCount.Text);
                crInfo.MaxNum = TranslateUtils.ToInt(this.MaxNum.Text);
                crInfo.Prestige = TranslateUtils.ToInt(this.Prestige.Text);
                crInfo.Contribution = TranslateUtils.ToInt(this.Contribution.Text);
                crInfo.Currency = TranslateUtils.ToInt(this.Currency.Text);
                crInfo.ExtCredit1 = TranslateUtils.ToInt(this.ExtCredit1.Text);
                crInfo.ExtCredit2 = TranslateUtils.ToInt(this.ExtCredit2.Text);
                crInfo.ExtCredit3 = TranslateUtils.ToInt(this.ExtCredit3.Text);

                if (crInfo.ID > 0)
                {
                    DataProvider.CreditRuleDAO.Update(base.PublishmentSystemID, crInfo);
                }
                else
                {
                    DataProvider.CreditRuleDAO.Insert(base.PublishmentSystemID, crInfo);
                }

                isChanged = true;
            }
            catch(Exception ex)
            {
                base.FailMessage(ex, "积分策略设置出错！");
            }
            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundConfigurationCredit2.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}
