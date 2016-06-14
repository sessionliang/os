using System;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core.Configuration;
using BaiRong.Model;
using BaiRong.Core;
using System.Web.UI;
using System.Collections;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundConfigurationCredit2 : BackgroundBasePage
    {
        public DataGrid MyDataGrid;
        public Button SetButton;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_configurationCredit2.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, "积分奖赏设置", AppManager.BBS.Permission.BBS_Settings);

                BindGrid();
            }
        }

        public void SetButton_Click(object sender, EventArgs e)
        {
            ArrayList values = ECreditRuleUtils.GetValueArrayList();
            foreach (string ruleName in values)
            {
                string prestigeName = ruleName + "_Prestige";
                string contributionName = ruleName + "_Contribution";
                string currencyName = ruleName + "_Currency";

                if (base.Request.Form[prestigeName] != null)
                {
                    ECreditRule creditRule = ECreditRuleUtils.GetEnumType(ruleName);
                    CreditRuleInfo ruleInfo = CreditRuleManager.GetCreditRuleInfo(base.PublishmentSystemID, creditRule, 0);
                    int prestige = TranslateUtils.ToInt(base.Request.Form[prestigeName]);
                    int contribution = TranslateUtils.ToInt(base.Request.Form[contributionName]);
                    int currency = TranslateUtils.ToInt(base.Request.Form[currencyName]);

                    if (ruleInfo.Prestige != prestige || ruleInfo.Contribution != contribution || ruleInfo.Currency != currency)
                    {
                        ruleInfo.Prestige = prestige;
                        ruleInfo.Contribution = contribution;
                        ruleInfo.Currency = currency;
                        if (ruleInfo.ID > 0)
                        {
                            DataProvider.CreditRuleDAO.Update(base.PublishmentSystemID, ruleInfo);
                        }
                        else
                        {
                            DataProvider.CreditRuleDAO.Insert(base.PublishmentSystemID, ruleInfo);
                        }
                    }
                }
            }
            this.BindGrid();
        }

        public void BindGrid()
        {
            MyDataGrid.DataSource = ECreditRuleUtils.GetValueArrayList();
            MyDataGrid.ItemDataBound += new DataGridItemEventHandler(MyDataGrid_ItemDataBound);
            MyDataGrid.DataBind();
        }

        public int GetCreditValue(string typeName, string ruleName)
        {
            ECreditRule creditRule = ECreditRuleUtils.GetEnumType(ruleName);
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

        public void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ECreditRule creditRule = ECreditRuleUtils.GetEnumType((string)e.Item.DataItem);
                CreditRuleInfo ruleInfo = CreditRuleManager.GetCreditRuleInfo(base.PublishmentSystemID, creditRule, 0);

                Literal ltlRuleName = e.Item.FindControl("ltlRuleName") as Literal;
                Literal ltlPeriodType = e.Item.FindControl("ltlPeriodType") as Literal;
                Literal ltlMaxNum = e.Item.FindControl("ltlMaxNum") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlRuleName.Text = ECreditRuleUtils.GetText(creditRule);
                ltlPeriodType.Text = EPeriodTypeUtils.GetText(ruleInfo.PeriodType);
                ltlMaxNum.Text = (ruleInfo.MaxNum > 0) ? ruleInfo.MaxNum + "次" : "不限";

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.CreditRule.GetOpenWindowString(base.PublishmentSystemID, creditRule, 0));
            }
        }
    }
}
