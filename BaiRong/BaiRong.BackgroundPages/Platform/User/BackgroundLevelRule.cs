using System;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core.Configuration;
using BaiRong.Model;
using BaiRong.Core;
using System.Web.UI;
using System.Collections;

namespace BaiRong.BackgroundPages
{
    public class BackgroundLevelRule : BackgroundBasePage
    {
        public DataGrid MyDataGrid;
        public Button SetButton;

        public static string GetRedirectUrl()
        {
            return PageUtils.GetPlatformUrl(string.Format("background_levelRule.aspx"));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserLevelCredit, "用户等级规则", AppManager.User.Permission.Usercenter_Setting);

                BindGrid();
            }
        }

        public void SetButton_Click(object sender, EventArgs e)
        {
            ArrayList values = ELevelRuleUtils.GetValueArrayList();
            foreach (string ruleName in values)
            {
                string creditNumName = ruleName + "_Credit";
                string cashNumName = ruleName + "_Cash";


                if (base.Request.Form[creditNumName] != null)
                {
                    ELevelRule LevelRule = ELevelRuleUtils.GetEnumType(ruleName);
                    LevelRuleInfo ruleInfo = LevelRuleManager.GetLevelRuleInfo(LevelRule);
                    int creditNum = TranslateUtils.ToInt(base.Request.Form[creditNumName]);
                    int cashNum = TranslateUtils.ToInt(base.Request.Form[cashNumName]);


                    if (ruleInfo.CreditNum != creditNum || ruleInfo.CashNum != cashNum)
                    {
                        ruleInfo.CreditNum = creditNum;
                        ruleInfo.CashNum = cashNum; 

                        if (ruleInfo.ID > 0)
                        {
                            BaiRongDataProvider.LevelRuleDAO.Update(ruleInfo);
                        }
                        else
                        {
                            BaiRongDataProvider.LevelRuleDAO.Insert(ruleInfo);
                        }
                    }
                }
            }
            this.BindGrid();
        }

        public void BindGrid()
        {
            MyDataGrid.DataSource = ELevelRuleUtils.GetValueArrayList();
            MyDataGrid.ItemDataBound += new DataGridItemEventHandler(MyDataGrid_ItemDataBound);
            MyDataGrid.DataBind();
        }

        public int GetLevelValue(string typeName, string ruleName)
        {
            ELevelRule LevelRule = ELevelRuleUtils.GetEnumType(ruleName);
            LevelRuleInfo ruleInfo = LevelRuleManager.GetLevelRuleInfo(LevelRule);
            ELevelRuleType LevelType = ELevelRuleTypeUtils.GetEnumType(typeName);
            if (LevelType == ELevelRuleType.CreditNum)
            {
                return ruleInfo.CreditNum;
            }
            else if (LevelType == ELevelRuleType.CashNum)
            {
                return ruleInfo.CashNum;
            }
            return 0;
        }

        public void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ELevelRule LevelRule = ELevelRuleUtils.GetEnumType((string)e.Item.DataItem);
                LevelRuleInfo ruleInfo = LevelRuleManager.GetLevelRuleInfo(LevelRule);

                Literal ltlRuleName = e.Item.FindControl("ltlRuleName") as Literal;
                Literal ltlPeriodType = e.Item.FindControl("ltlPeriodType") as Literal;
                Literal ltlMaxNum = e.Item.FindControl("ltlMaxNum") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlRuleName.Text = ELevelRuleUtils.GetText(LevelRule);
                ltlPeriodType.Text = ELevelPeriodTypeUtils.GetText(ruleInfo.PeriodType);
                ltlMaxNum.Text = (ruleInfo.MaxNum > 0) ? ruleInfo.MaxNum + "次" : "不限";

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.LevelRule.GetOpenWindowString(LevelRule));
            }
        }
    }
}
