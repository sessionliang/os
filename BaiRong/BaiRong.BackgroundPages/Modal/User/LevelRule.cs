using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BaiRong.BackgroundPages.Modal
{
    public class LevelRule : BackgroundBasePage
    {
        protected Literal ltlRuleName;
        protected RadioButtonList PeriodType;
        protected PlaceHolder phPeriodCount;
        protected TextBox PeriodCount;
        protected TextBox MaxNum;
        protected Literal ltlNameCreditNum;
        protected TextBox CreditNum;
        protected Literal ltlNameCashNum;
        protected TextBox CashNum;

        private ELevelRule levelRule;

        public static string GetOpenWindowString(ELevelRule levelRule)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("LevelRule", ELevelRuleUtils.GetValue(levelRule));

            return PageUtilityPF.GetOpenWindowString("规则策略设置", "modal_levelRule.aspx", arguments, 400, 500);

        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.levelRule = ELevelRuleUtils.GetEnumType(base.GetQueryString("LevelRule"));
            LevelRuleInfo crInfo = LevelRuleManager.GetLevelRuleInfo(this.levelRule);

            if (!IsPostBack)
            {
                this.ltlRuleName.Text = ELevelRuleUtils.GetText(this.levelRule);
                ELevelPeriodTypeUtils.AddListItems(this.PeriodType);
                ControlUtils.SelectListItemsIgnoreCase(this.PeriodType, ELevelPeriodTypeUtils.GetValue(crInfo.PeriodType).ToString());
                if (crInfo.PeriodType == ELevelPeriodType.Hour || crInfo.PeriodType == ELevelPeriodType.Minute)
                {
                    this.phPeriodCount.Visible = true;
                    this.PeriodCount.Text = crInfo.PeriodCount.ToString();
                }
                else
                {
                    this.phPeriodCount.Visible = false;
                }
                this.MaxNum.Text = crInfo.MaxNum.ToString();
                this.ltlNameCreditNum.Text = base.Additional.CreditNumName;
                this.CreditNum.Text = crInfo.CreditNum.ToString();
                this.ltlNameCashNum.Text = base.Additional.CashNumName;
                this.CashNum.Text = crInfo.CashNum.ToString();
            }
        }

        public void PeriodType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ELevelPeriodType type = ELevelPeriodTypeUtils.GetEnumType(this.PeriodType.SelectedValue);
            if (type == ELevelPeriodType.Hour || type == ELevelPeriodType.Minute)
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
                LevelRuleInfo crInfo = LevelRuleManager.GetLevelRuleInfo(this.levelRule);
                crInfo.PeriodType = ELevelPeriodTypeUtils.GetEnumType(this.PeriodType.SelectedValue);
                crInfo.PeriodCount = TranslateUtils.ToInt(this.PeriodCount.Text);
                crInfo.MaxNum = TranslateUtils.ToInt(this.MaxNum.Text);
                crInfo.CreditNum = TranslateUtils.ToInt(this.CreditNum.Text);
                crInfo.CashNum = TranslateUtils.ToInt(this.CashNum.Text);

                if (crInfo.ID > 0)
                {
                    BaiRongDataProvider.LevelRuleDAO.Update(crInfo);
                }
                else
                {
                    BaiRongDataProvider.LevelRuleDAO.Insert(crInfo);
                }

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "规则策略设置出错！");
            }
            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundLevelRule.GetRedirectUrl());
            }
        }
    }
}
