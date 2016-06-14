using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Web.UI;
using SiteServer.CMS.Model;
using System.Text;
using System.Collections;

using BaiRong.Model;

namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicIdentifierRule : BackgroundGovPublicBasePage
    {
        public Literal ltlPreview;
        public DataGrid dgContents;
        public Button AddButton;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.Request.QueryString["Delete"] != null)
            {
                int ruleID = TranslateUtils.ToInt(base.Request.QueryString["RuleID"]);
                try
                {
                    DataProvider.GovPublicIdentifierRuleDAO.Delete(ruleID);
                    base.SuccessMessage("成功删除规则");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除规则失败，{0}", ex.Message));
                }
            }
            else if ((base.Request.QueryString["Up"] != null || base.Request.QueryString["Down"] != null) && base.Request.QueryString["RuleID"] != null)
            {
                int ruleID = TranslateUtils.ToInt(base.Request.QueryString["RuleID"]);
                bool isDown = (base.Request.QueryString["Down"] != null) ? true : false;
                if (isDown)
                {
                    DataProvider.GovPublicIdentifierRuleDAO.UpdateTaxisToUp(ruleID, base.PublishmentSystemID);
                }
                else
                {
                    DataProvider.GovPublicIdentifierRuleDAO.UpdateTaxisToDown(ruleID, base.PublishmentSystemID);
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicContentConfiguration, "索引号生成规则", AppManager.CMS.Permission.WebSite.GovPublicContentConfiguration);

                this.ltlPreview.Text = GovPublicManager.GetPreviewIdentifier(base.PublishmentSystemID);

                this.dgContents.DataSource = DataProvider.GovPublicIdentifierRuleDAO.GetRuleInfoArrayList(base.PublishmentSystemID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.AddButton.Attributes.Add("onclick", Modal.GovPublicIdentifierRuleAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GovPublicIdentifierRuleInfo ruleInfo = e.Item.DataItem as GovPublicIdentifierRuleInfo;

                Literal ltlIndex = e.Item.FindControl("ltlIndex") as Literal;
                Literal ltlRuleName = e.Item.FindControl("ltlRuleName") as Literal;
                Literal ltlIdentifierType = e.Item.FindControl("ltlIdentifierType") as Literal;
                Literal ltlMinLength = e.Item.FindControl("ltlMinLength") as Literal;
                Literal ltlSuffix = e.Item.FindControl("ltlSuffix") as Literal;
                HyperLink hlUpLinkButton = e.Item.FindControl("hlUpLinkButton") as HyperLink;
                HyperLink hlDownLinkButton = e.Item.FindControl("hlDownLinkButton") as HyperLink;
                Literal ltlSettingUrl = e.Item.FindControl("ltlSettingUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlRuleName.Text = ruleInfo.RuleName;
                ltlIdentifierType.Text = EGovPublicIdentifierTypeUtils.GetText(ruleInfo.IdentifierType);
                ltlMinLength.Text = ruleInfo.MinLength.ToString();
                ltlSuffix.Text = ruleInfo.Suffix;

                hlUpLinkButton.NavigateUrl = PageUtils.GetWCMUrl(string.Format("background_govPublicIdentifierRule.aspx?PublishmentSystemID={0}&RuleID={1}&Up=True", base.PublishmentSystemID, ruleInfo.RuleID));

                hlDownLinkButton.NavigateUrl = PageUtils.GetWCMUrl(string.Format("background_govPublicIdentifierRule.aspx?PublishmentSystemID={0}&RuleID={1}&Down=True", base.PublishmentSystemID, ruleInfo.RuleID));

                if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Department)
                {
                    string urlSetting = PageUtils.GetWCMUrl(string.Format("background_govPublicDepartment.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
                    ltlSettingUrl.Text = string.Format(@"<a href=""{0}"">机构分类设置</a>", urlSetting);
                }
                else if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Channel)
                {
                    string urlSetting = PageUtils.GetWCMUrl(string.Format("background_govPublicChannel.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
                    ltlSettingUrl.Text = string.Format(@"<a href=""{0}"">主题分类设置</a>", urlSetting);
                }

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.GovPublicIdentifierRuleAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, ruleInfo.RuleID));

                string urlDelete = PageUtils.GetWCMUrl(string.Format("background_govPublicIdentifierRule.aspx?PublishmentSystemID={0}&Delete=True&RuleID={1}", base.PublishmentSystemID, ruleInfo.RuleID));
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除规则“{1}”，确认吗？');"">删除</a>", urlDelete, ruleInfo.RuleName);
            }
        }
    }
}
