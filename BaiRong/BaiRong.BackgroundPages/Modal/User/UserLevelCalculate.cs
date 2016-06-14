using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;

namespace BaiRong.BackgroundPages.Modal
{
    public class UserLevelCalculate : BackgroundBasePage
    {
        //protected TextBox txtPostCount;
        //protected TextBox txtPostDigestCount;

        protected Literal ltlNameCreditNum;
        protected TextBox txtCreditNum;
        protected Literal ltlNameCashNum;
        protected TextBox txtCashNum;


        public static string GetOpenWindowString()
        {
            NameValueCollection arguments = new NameValueCollection();
            return PageUtilityPF.GetOpenWindowString("设置总积分计算公式", "modal_userLevelCalculate.aspx", arguments, 330, 380);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                //this.txtPostCount.Text = base.Additional.CreditMultiplierPostCount.ToString();
                //this.txtPostDigestCount.Text = base.Additional.CreditMultiplierPostDigestCount.ToString();

                this.ltlNameCreditNum.Text = UserConfigManager.Instance.Additional.CreditNumName;
                this.txtCreditNum.Text = UserConfigManager.Instance.Additional.CreditMultiplier.ToString();

                this.ltlNameCashNum.Text = UserConfigManager.Instance.Additional.CashNumName;
                this.txtCashNum.Text = UserConfigManager.Instance.Additional.CashMultiplier.ToString();

            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            try
            {
                //base.Additional.CreditNumName = this.ltlNameCreditNum.Text;
                UserConfigManager.Instance.Additional.CreditMultiplier = TranslateUtils.ToInt(this.txtCreditNum.Text);
                //base.Additional.CashNumName = this.ltlNameCashNum.Text;
                UserConfigManager.Instance.Additional.CashMultiplier = TranslateUtils.ToInt(this.txtCashNum.Text);

                BaiRongDataProvider.UserConfigDAO.Update(UserConfigManager.Instance);

                UserConfigManager.IsChanged = isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "编辑积分属性出错！");
            }
            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
