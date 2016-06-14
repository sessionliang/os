using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WCM.Core;

namespace SiteServer.WCM.BackgroundPages.Modal
{
	public class GovPublicIdentifierRuleAdd : BackgroundBasePage
	{
        public TextBox tbRuleName;
        public DropDownList ddlIdentifierType;
        public PlaceHolder phAttributeName;
        public DropDownList ddlAttributeName;
        public PlaceHolder phMinLength;
        public TextBox tbMinLength;
        public PlaceHolder phFormatString;
        public TextBox tbFormatString;
        public TextBox tbSuffix;
        public PlaceHolder phSequence;
        public TextBox tbSequence;
        public RadioButtonList rblIsSequenceChannelZero;
        public RadioButtonList rblIsSequenceDepartmentZero;
        public RadioButtonList rblIsSequenceYearZero;

        private int ruleID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityWCM.GetOpenWindowString("添加规则", "modal_govPublicIdentifierRuleAdd.aspx", arguments, 520, 460);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int ruleID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RuleID", ruleID.ToString());
            return PageUtilityWCM.GetOpenWindowString("修改规则", "modal_govPublicIdentifierRuleAdd.aspx", arguments, 520, 460);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.ruleID = TranslateUtils.ToInt(base.Request.QueryString["RuleID"]);

			if (!IsPostBack)
			{
                EGovPublicIdentifierTypeUtils.AddListItems(this.ddlIdentifierType);

                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.GovPublicContent, base.PublishmentSystemInfo.AuxiliaryTableForGovPublic, null);
                foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
                {
                    if (tableStyleInfo.AttributeName == ContentAttribute.Title || tableStyleInfo.AttributeName == GovPublicContentAttribute.Content || tableStyleInfo.AttributeName == GovPublicContentAttribute.DepartmentID || tableStyleInfo.AttributeName == GovPublicContentAttribute.Description || tableStyleInfo.AttributeName == GovPublicContentAttribute.ImageUrl || tableStyleInfo.AttributeName == GovPublicContentAttribute.FileUrl || tableStyleInfo.AttributeName == GovPublicContentAttribute.Identifier || tableStyleInfo.AttributeName == GovPublicContentAttribute.Keywords || tableStyleInfo.AttributeName == GovPublicContentAttribute.DocumentNo || tableStyleInfo.AttributeName == GovPublicContentAttribute.Publisher) continue;
                    this.ddlAttributeName.Items.Add(new ListItem(tableStyleInfo.DisplayName + "(" + tableStyleInfo.AttributeName + ")", tableStyleInfo.AttributeName));
                }
                EBooleanUtils.AddListItems(this.rblIsSequenceChannelZero);
                EBooleanUtils.AddListItems(this.rblIsSequenceDepartmentZero);
                EBooleanUtils.AddListItems(this.rblIsSequenceYearZero);

                ControlUtils.SelectListItemsIgnoreCase(this.rblIsSequenceChannelZero, true.ToString());
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsSequenceDepartmentZero, false.ToString());
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsSequenceYearZero, true.ToString());

                if (this.ruleID > 0)
                {
                    GovPublicIdentifierRuleInfo ruleInfo = DataProvider.GovPublicIdentifierRuleDAO.GetIdentifierRuleInfo(this.ruleID);
                    if (ruleInfo != null)
                    {
                        this.tbRuleName.Text = ruleInfo.RuleName;
                        ControlUtils.SelectListItems(this.ddlIdentifierType, EGovPublicIdentifierTypeUtils.GetValue(ruleInfo.IdentifierType));
                        ControlUtils.SelectListItems(this.ddlAttributeName, ruleInfo.AttributeName);
                        this.tbMinLength.Text = ruleInfo.MinLength.ToString();
                        this.tbFormatString.Text = ruleInfo.FormatString;
                        this.tbSuffix.Text = ruleInfo.Suffix;
                        this.tbSequence.Text = ruleInfo.Sequence.ToString();

                        ControlUtils.SelectListItemsIgnoreCase(this.rblIsSequenceChannelZero, ruleInfo.Additional.IsSequenceChannelZero.ToString());
                        ControlUtils.SelectListItemsIgnoreCase(this.rblIsSequenceDepartmentZero, ruleInfo.Additional.IsSequenceDepartmentZero.ToString());
                        ControlUtils.SelectListItemsIgnoreCase(this.rblIsSequenceYearZero, ruleInfo.Additional.IsSequenceYearZero.ToString());
                    }
                }

                this.ddlIdentifierType.SelectedIndexChanged += new EventHandler(ddlIdentifierType_SelectedIndexChanged);
                this.ddlIdentifierType_SelectedIndexChanged(null, EventArgs.Empty);
			}
		}

        public void ddlIdentifierType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EGovPublicIdentifierType identifierType = EGovPublicIdentifierTypeUtils.GetEnumType(this.ddlIdentifierType.SelectedValue);
            if (identifierType == EGovPublicIdentifierType.Department || identifierType == EGovPublicIdentifierType.Channel)
            {
                this.phAttributeName.Visible = false;
                this.phFormatString.Visible = false;
                this.phMinLength.Visible = true;
                this.phSequence.Visible = false;
            }
            else if (identifierType == EGovPublicIdentifierType.Sequence)
            {
                this.phAttributeName.Visible = false;
                this.phFormatString.Visible = false;
                this.phMinLength.Visible = true;
                this.phSequence.Visible = true;
            }
            else if (identifierType == EGovPublicIdentifierType.Attribute)
            {
                this.phAttributeName.Visible = true;
                this.phFormatString.Visible = true;
                this.phMinLength.Visible = true;
                this.phSequence.Visible = false;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            ArrayList ruleInfoArrayList = DataProvider.GovPublicIdentifierRuleDAO.GetRuleInfoArrayList(base.PublishmentSystemID);
				
			if (this.ruleID > 0)
			{
				try
				{
                    GovPublicIdentifierRuleInfo ruleInfo = DataProvider.GovPublicIdentifierRuleDAO.GetIdentifierRuleInfo(this.ruleID);
                    ruleInfo.RuleName = this.tbRuleName.Text;
                    ruleInfo.IdentifierType = EGovPublicIdentifierTypeUtils.GetEnumType(this.ddlIdentifierType.SelectedValue);
                    ruleInfo.MinLength = TranslateUtils.ToInt(this.tbMinLength.Text);
                    ruleInfo.Suffix = this.tbSuffix.Text;
                    ruleInfo.FormatString = this.tbFormatString.Text;
                    ruleInfo.AttributeName = this.ddlAttributeName.SelectedValue;
                    ruleInfo.Sequence = TranslateUtils.ToInt(this.tbSequence.Text);

                    if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Sequence)
                    {
                        ruleInfo.Additional.IsSequenceChannelZero = TranslateUtils.ToBool(this.rblIsSequenceChannelZero.SelectedValue);
                        ruleInfo.Additional.IsSequenceDepartmentZero = TranslateUtils.ToBool(this.rblIsSequenceDepartmentZero.SelectedValue);
                        ruleInfo.Additional.IsSequenceYearZero = TranslateUtils.ToBool(this.rblIsSequenceYearZero.SelectedValue);
                    }

                    foreach (GovPublicIdentifierRuleInfo identifierRuleInfo in ruleInfoArrayList)
                    {
                        if (identifierRuleInfo.RuleID == ruleInfo.RuleID) continue;
                        if (identifierRuleInfo.IdentifierType != EGovPublicIdentifierType.Attribute && identifierRuleInfo.IdentifierType == ruleInfo.IdentifierType)
                        {
                            base.FailMessage("规则修改失败，本类型规则只能添加一次！");
                            return;
                        }
                        if (identifierRuleInfo.RuleName == this.tbRuleName.Text)
                        {
                            base.FailMessage("规则修改失败，规则名称已存在！");
                            return;
                        }
                    }

                    DataProvider.GovPublicIdentifierRuleDAO.Update(ruleInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改规则", string.Format("规则:{0}", ruleInfo.RuleName));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "规则修改失败！");
				}
			}
			else
			{
                EGovPublicIdentifierType identifierType = EGovPublicIdentifierTypeUtils.GetEnumType(this.ddlIdentifierType.SelectedValue);

                foreach (GovPublicIdentifierRuleInfo ruleInfo in ruleInfoArrayList)
                {
                    if (ruleInfo.IdentifierType != EGovPublicIdentifierType.Attribute && ruleInfo.IdentifierType == identifierType)
                    {
                        base.FailMessage("规则添加失败，本类型规则只能添加一次！");
                        return;
                    }
                    if (ruleInfo.RuleName == this.tbRuleName.Text)
                    {
                        base.FailMessage("规则添加失败，规则名称已存在！");
                        return;
                    }
                }

                try
                {
                    GovPublicIdentifierRuleInfo ruleInfo = new GovPublicIdentifierRuleInfo(0, this.tbRuleName.Text, base.PublishmentSystemID, identifierType, TranslateUtils.ToInt(this.tbMinLength.Text), this.tbSuffix.Text, this.tbFormatString.Text, this.ddlAttributeName.SelectedValue, TranslateUtils.ToInt(this.tbSequence.Text), 0, string.Empty);

                    if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Sequence)
                    {
                        ruleInfo.Additional.IsSequenceChannelZero = TranslateUtils.ToBool(this.rblIsSequenceChannelZero.SelectedValue);
                        ruleInfo.Additional.IsSequenceDepartmentZero = TranslateUtils.ToBool(this.rblIsSequenceDepartmentZero.SelectedValue);
                        ruleInfo.Additional.IsSequenceYearZero = TranslateUtils.ToBool(this.rblIsSequenceYearZero.SelectedValue);
                    }

                    DataProvider.GovPublicIdentifierRuleDAO.Insert(ruleInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "添加规则", string.Format("规则:{0}", ruleInfo.RuleName));

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "规则添加失败！");
                }
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetCMSUrl(PageUtils.GetWCMUrl(string.Format("background_govPublicIdentifierRule.aspx?PublishmentSystemID={0}", base.PublishmentSystemID))));
			}
		}
	}
}
