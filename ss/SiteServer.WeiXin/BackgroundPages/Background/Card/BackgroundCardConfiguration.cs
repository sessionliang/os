using System;

using System.Drawing;
using System.Drawing.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;
using System.Collections;
using System.Text;



namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundCardConfiguration : BackgroundBasePageWX
	{
        public RadioButtonList IsClaimCardCredits;
	    public Control ClaimCardCreditsRow;
        public TextBox tbClaimCardCredits;

        public RadioButtonList IsGiveConsumeCredits;
        public Control GiveConsumeCreditsRow;
        public TextBox tbConsumeAmount;
        public TextBox tbGivCredits;
        public RadioButtonList IsBinding;
        public RadioButtonList IsExchange;
        public Control ExchangeProportionRow;
        public TextBox tbExchangeProportion;

        public RadioButtonList IsSign;
        public Control SignCreditsRow;
        public Literal ltlScript;
        private ArrayList configureInfoArrayList = new ArrayList();

        public string GetSignDayFrom(int itemIndex)
        {
            if (this.configureInfoArrayList == null || this.configureInfoArrayList.Count <= itemIndex) return string.Empty;
            string configureInfo = configureInfoArrayList[itemIndex] as string;
            if (!configureInfo.Contains("&")) return string.Empty;
            string signDayFrom = configureInfo.Split('&')[0];
            return signDayFrom;
        }
        public string GetSignDayTo(int itemIndex)
        {
            if (this.configureInfoArrayList == null || this.configureInfoArrayList.Count <= itemIndex) return string.Empty;
            string configureInfo = configureInfoArrayList[itemIndex] as string;
            if (!configureInfo.Contains("&")) return string.Empty;
            string signDayTo = configureInfo.Split('&')[1];
            return signDayTo;
        }

        public string GetSignCredits(int itemIndex)
        {
            if (this.configureInfoArrayList == null || this.configureInfoArrayList.Count <= itemIndex) return string.Empty;
            string configureInfo = configureInfoArrayList[itemIndex] as string;
            if (!configureInfo.Contains("&")) return string.Empty;
            string signCredits = configureInfo.Split('&')[2];
            return signCredits;
        }
		 
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.GetQueryString("successMessage") != null)
            {
                base.SuccessMessage("会员卡设置修改成功！");
            }
         
			if (!IsPostBack)
            {
               
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Card, "会员卡设置", AppManager.WeiXin.Permission.WebSite.Card);
                EBooleanUtils.AddListItems(this.IsClaimCardCredits, "是", "否");
                ControlUtils.SelectListItemsIgnoreCase(this.IsClaimCardCredits, base.PublishmentSystemInfo.Additional.Card_IsClaimCardCredits.ToString());
                this.tbClaimCardCredits.Text = base.PublishmentSystemInfo.Additional.Card_ClaimCardCredits.ToString();

                EBooleanUtils.AddListItems(this.IsGiveConsumeCredits, "是", "否");
                ControlUtils.SelectListItemsIgnoreCase(this.IsGiveConsumeCredits, base.PublishmentSystemInfo.Additional.Card_IsGiveConsumeCredits.ToString());

                this.tbConsumeAmount.Text = base.PublishmentSystemInfo.Additional.Card_ConsumeAmount.ToString();
                this.tbGivCredits.Text = base.PublishmentSystemInfo.Additional.Card_GiveCredits.ToString();
                EBooleanUtils.AddListItems(this.IsBinding, "是", "否");
                ControlUtils.SelectListItemsIgnoreCase(this.IsBinding, base.PublishmentSystemInfo.Additional.Card_IsBinding.ToString());
                EBooleanUtils.AddListItems(this.IsExchange, "是", "否");
                ControlUtils.SelectListItemsIgnoreCase(this.IsExchange, base.PublishmentSystemInfo.Additional.Card_IsExchange.ToString());
                this.tbExchangeProportion.Text = base.PublishmentSystemInfo.Additional.Card_ExchangeProportion.ToString();

                EBooleanUtils.AddListItems(this.IsSign, "是", "否");
                ControlUtils.SelectListItemsIgnoreCase(this.IsSign, base.PublishmentSystemInfo.Additional.Card_IsSign.ToString());
                 
                if (TranslateUtils.ToBool(this.IsClaimCardCredits.SelectedValue))
                {
                    this.ClaimCardCreditsRow.Visible = true;
                }
                else
                {
                    this.ClaimCardCreditsRow.Visible = false;
                }

                if (TranslateUtils.ToBool(this.IsGiveConsumeCredits.SelectedValue))
                {
                    this.GiveConsumeCreditsRow.Visible = true;
                }
                else
                {
                    this.GiveConsumeCreditsRow.Visible = false;
                }

                if (TranslateUtils.ToBool(this.IsExchange.SelectedValue))
                {
                    this.ExchangeProportionRow.Visible = true;
                }
                else
                {
                    this.ExchangeProportionRow.Visible = false;
                }
                if (TranslateUtils.ToBool(this.IsSign.SelectedValue))
                {
                    this.SignCreditsRow.Visible = true;
                }
                else
                {
                    this.SignCreditsRow.Visible = false;
                }
                 
			}

            string signCreditsConfigure = base.PublishmentSystemInfo.Additional.Card_SignCreditsConfigure;
            if (!string.IsNullOrEmpty(signCreditsConfigure))
            {
                configureInfoArrayList = TranslateUtils.StringCollectionToArrayList(signCreditsConfigure);
            }

            string script = string.Empty;

            for (int i = 2; i < this.configureInfoArrayList.Count; i++)
            {
                string configureInfo = this.configureInfoArrayList[i] as string;
                if (string.IsNullOrEmpty(configureInfo)) continue;
                script += string.Format("addItem('{0}','{1}','{2}');", configureInfo.Split('&')[0], configureInfo.Split('&')[1], configureInfo.Split('&')[2]);
            }
            if (!string.IsNullOrEmpty(script))
            {
                this.ltlScript.Text = string.Format(@"<script>{0}</script>", script);
            }
		}
         
        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.Card_IsClaimCardCredits = TranslateUtils.ToBool(this.IsClaimCardCredits.SelectedValue);
                base.PublishmentSystemInfo.Additional.Card_ClaimCardCredits = TranslateUtils.ToInt(this.tbClaimCardCredits.Text);

                base.PublishmentSystemInfo.Additional.Card_IsGiveConsumeCredits = TranslateUtils.ToBool(this.IsGiveConsumeCredits.SelectedValue);
                base.PublishmentSystemInfo.Additional.Card_ConsumeAmount = TranslateUtils.ToDecimal(this.tbConsumeAmount.Text);
                base.PublishmentSystemInfo.Additional.Card_GiveCredits = TranslateUtils.ToInt(this.tbGivCredits.Text);
                base.PublishmentSystemInfo.Additional.Card_IsBinding = TranslateUtils.ToBool(this.IsBinding.SelectedValue);
                base.PublishmentSystemInfo.Additional.Card_IsExchange = TranslateUtils.ToBool(this.IsExchange.SelectedValue);
                base.PublishmentSystemInfo.Additional.Card_ExchangeProportion = TranslateUtils.ToDecimal(this.tbExchangeProportion.Text);

                base.PublishmentSystemInfo.Additional.Card_IsSign = TranslateUtils.ToBool(this.IsSign.SelectedValue);
                int itemCount = TranslateUtils.ToInt(base.Request.Form["itemCount"]);
                StringBuilder signCreditsConfigure = new StringBuilder();
                for (int i = 0; i < itemCount; i++)
                {
                    string optionConfigure = string.Empty;
                    string dayFrom = base.Request.Form["optionsDayFrom[" + i + "]"];
                    string dayTo = base.Request.Form["optionsDayTo[" + i + "]"];
                    string credits = base.Request.Form["optionsSignCredits[" + i + "]"];
                    if (!string.IsNullOrEmpty(dayFrom) && !string.IsNullOrEmpty(dayTo) && !string.IsNullOrEmpty(credits))
                    {
                        optionConfigure = string.Format("{0}&{1}&{2}", dayFrom, dayTo, credits);
                    }
                    signCreditsConfigure.AppendFormat("{0},", optionConfigure);
                }

                base.PublishmentSystemInfo.Additional.Card_SignCreditsConfigure = signCreditsConfigure.ToString(); ;
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "修改会员卡设置");
                    base.SuccessMessage("会员卡设置修改成功！");

                    //PageUtils.Redirect(string.Format("background_cardConfiguration.aspx?PublishmentSystemID={0}&successMessage={1}", base.PublishmentSystemID, "会员卡设置修改成功"));
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "会员卡设置修改失败！");
				}
			}
		}

		public void  Refrush(object sender, EventArgs e)
		{
            if (TranslateUtils.ToBool(this.IsClaimCardCredits.SelectedValue))
            {
                this.ClaimCardCreditsRow.Visible = true;
            }
            else
            {
                this.ClaimCardCreditsRow.Visible = false;
            }

            if (TranslateUtils.ToBool(this.IsGiveConsumeCredits.SelectedValue))
            {
                this.GiveConsumeCreditsRow.Visible = true;
            }
            else
            {
                this.GiveConsumeCreditsRow.Visible = false;
            }
            if (TranslateUtils.ToBool(this.IsExchange.SelectedValue))
            {
                this.ExchangeProportionRow.Visible = true;
            }
            else
            {
                this.ExchangeProportionRow.Visible = false;
            }
            if (TranslateUtils.ToBool(this.IsSign.SelectedValue))
            {
                this.SignCreditsRow.Visible = true;
            }
            else
            {
                this.SignCreditsRow.Visible = false;
            }
		}
	}
}
