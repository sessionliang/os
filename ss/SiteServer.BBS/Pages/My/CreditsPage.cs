using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using SiteServer.BBS.Core.TemplateParser;
using System.Collections.Specialized;
using SiteServer.BBS.Core.TemplateParser.Model;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.IO;
using BaiRong.Model;
using BaiRong.Core.Drawing;
using System.Text;

namespace SiteServer.BBS.Pages
{
    public class CreditsPage : BasePage
    {
        public Literal ltlCredit;
        public Literal ltlCalculate;
        public Literal ltlCreditRows;
        public Repeater rptRuleLog;
        public Repeater rptRule;

        protected override bool IsAccessable
        {
            get
            {
                return false;
            }
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BBSUserInfo userInfo = BBSUserManager.GetCurrentUserInfo();

                this.ltlCredit.Text = UserManager.Current.Credits.ToString();
                this.ltlCalculate.Text = CreditRuleManager.GetCreditCalculate(base.PublishmentSystemID);

                StringBuilder builder = new StringBuilder();

                builder.AppendFormat(@"
<tr>
<td>{0}£º{1}</td>
<td>{2}£º{3}</td>
<td>{4}£º{5}</td>
</tr>
", base.Additional.CreditNamePrestige, userInfo.Prestige, base.Additional.CreditNameContribution, userInfo.Contribution, base.Additional.CreditNameCurrency, userInfo.Currency);
                if (base.Additional.CreditUsingExtCredit1)
                {
                    builder.AppendFormat(@"
<tr>
<td>{0}£º{1}</td>
", base.Additional.CreditNameExtCredit1, userInfo.ExtCredit1);
                    if (base.Additional.CreditUsingExtCredit2)
                    {
                        builder.AppendFormat(@"
<td>{0}£º{1}</td>
", base.Additional.CreditNameExtCredit2, userInfo.ExtCredit2);
                    }
                    if (base.Additional.CreditUsingExtCredit3)
                    {
                        builder.AppendFormat(@"
<td>{0}£º{1}</td>
", base.Additional.CreditNameExtCredit3, userInfo.ExtCredit3);
                    }

                    builder.Append("</tr>");
                }

                this.ltlCreditRows.Text = builder.ToString();

                this.rptRuleLog.DataSource = DataProvider.CreditRuleLogDAO.GetCreditRuleLogInfoArrayList(base.PublishmentSystemID, BaiRongDataProvider.UserDAO.CurrentUserName);
                this.rptRuleLog.ItemDataBound += new RepeaterItemEventHandler(rptRuleLog_ItemDataBound);
                this.rptRuleLog.DataBind();

                this.rptRule.DataSource = ECreditRuleUtils.GetValueArrayList();
                this.rptRule.ItemDataBound += new RepeaterItemEventHandler(rptRule_ItemDataBound);
                this.rptRule.DataBind();
            }
        }

        void rptRuleLog_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CreditRuleLogInfo ruleLogInfo = (CreditRuleLogInfo)e.Item.DataItem;

                Literal ltlRuleName = e.Item.FindControl("ltlRuleName") as Literal;
                Literal ltlTotalCount = e.Item.FindControl("ltlTotalCount") as Literal;
                Literal ltlPrestige = e.Item.FindControl("ltlPrestige") as Literal;
                Literal ltlContribution = e.Item.FindControl("ltlContribution") as Literal;
                Literal ltlCurrency = e.Item.FindControl("ltlCurrency") as Literal;
                Literal ltlLastDate = e.Item.FindControl("ltlLastDate") as Literal;

                ltlRuleName.Text = ECreditRuleUtils.GetText(ruleLogInfo.RuleType);
                ltlTotalCount.Text = ruleLogInfo.TotalCount.ToString();
                ltlPrestige.Text = ruleLogInfo.Prestige.ToString();
                ltlContribution.Text = ruleLogInfo.Contribution.ToString();
                ltlCurrency.Text = ruleLogInfo.Currency.ToString();
                ltlLastDate.Text = DateUtils.GetDateAndTimeString(ruleLogInfo.LastDate);
            }
        }

        void rptRule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ECreditRule creditRule = ECreditRuleUtils.GetEnumType((string)e.Item.DataItem);
                CreditRuleInfo ruleInfo = CreditRuleManager.GetCreditRuleInfo(base.PublishmentSystemID, creditRule, 0);

                Literal ltlRuleName = e.Item.FindControl("ltlRuleName") as Literal;
                Literal ltlPeriodType = e.Item.FindControl("ltlPeriodType") as Literal;
                Literal ltlMaxNum = e.Item.FindControl("ltlMaxNum") as Literal;
                Literal ltlPrestige = e.Item.FindControl("ltlPrestige") as Literal;
                Literal ltlContribution = e.Item.FindControl("ltlContribution") as Literal;
                Literal ltlCurrency = e.Item.FindControl("ltlCurrency") as Literal;

                ltlRuleName.Text = ECreditRuleUtils.GetText(creditRule);
                ltlPeriodType.Text = EPeriodTypeUtils.GetText(ruleInfo.PeriodType);
                ltlMaxNum.Text = (ruleInfo.MaxNum > 0) ? ruleInfo.MaxNum + "´Î" : "²»ÏÞ";
                ltlPrestige.Text = GetCreditValue("Prestige", creditRule).ToString();
                ltlContribution.Text = GetCreditValue("Contribution", creditRule).ToString();
                ltlCurrency.Text = GetCreditValue("Currency", creditRule).ToString();
            }
        }

        private int GetCreditValue(string typeName, ECreditRule creditRule)
        {
            CreditRuleInfo ruleInfo = CreditRuleManager.GetCreditRuleInfo(base.PublishmentSystemID, creditRule, 0);
            ECreditType creditType = ECreditTypeUtils.GetEnumType(typeName);
            if (creditType == ECreditType.Prestige)
            {
                return ruleInfo.Prestige;
            }
            else if (creditType == ECreditType.Contribution)
            {
                return ruleInfo.Contribution;
            }
            else if (creditType == ECreditType.Currency)
            {
                return ruleInfo.Currency;
            }
            return 0;
        }
    }
}
