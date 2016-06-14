using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundTagStyleTemplate : BackgroundBasePage
	{
        public Literal ltlStyleName;
        public Literal ltlElement;
        public RadioButtonList rblIsTemplate;

        public PlaceHolder phTemplate;
        public CheckBox cbIsCreateTemplate;
		public TextBox tbContent;
        public PlaceHolder phSuccess;
        public TextBox tbSuccess;
        public PlaceHolder phFailure;
        public TextBox tbFailure;
        public PlaceHolder phStyle;
        public TextBox tbStyle;
        public PlaceHolder phScript;
        public TextBox tbScript;

        public Button Preview;
        public PlaceHolder phReturn;

        private TagStyleInfo styleInfo;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("StyleID");

            int styleID = base.GetIntQueryString("StyleID");
            this.styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);

			if (!IsPostBack)
			{
                this.ltlStyleName.Text = this.styleInfo.StyleName;
                this.ltlElement.Text = string.Format(@"
&lt;{0} styleName=&quot;{1}&quot;&gt;&lt;/{0}&gt;", this.styleInfo.ElementName, this.styleInfo.StyleName);
                if (StringUtils.EqualsIgnoreCase(this.styleInfo.ElementName, StlGovInteractApply.ElementName))
                {
                    int nodeID = DataProvider.GovInteractChannelDAO.GetNodeIDByApplyStyleID(this.styleInfo.StyleID);
                    string nodeName = NodeManager.GetNodeName(base.PublishmentSystemID, nodeID);
                    this.ltlStyleName.Text = nodeName;
                    this.ltlElement.Text = string.Format(@"&lt;{0} interactName=&quot;{1}&quot;&gt;&lt;/{0}&gt;", this.styleInfo.ElementName, nodeName);
                }
                else if (StringUtils.EqualsIgnoreCase(this.styleInfo.ElementName, StlGovInteractQuery.ElementName))
                {
                    int nodeID = DataProvider.GovInteractChannelDAO.GetNodeIDByQueryStyleID(this.styleInfo.StyleID);
                    string nodeName = NodeManager.GetNodeName(base.PublishmentSystemID, nodeID);
                    this.ltlStyleName.Text = nodeName;
                    this.ltlElement.Text = string.Format(@"&lt;{0} interactName=&quot;{1}&quot;&gt;&lt;/{0}&gt;", this.styleInfo.ElementName, nodeName);
                }

                EBooleanUtils.AddListItems(this.rblIsTemplate, "自定义模板", "默认模板");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsTemplate, this.styleInfo.IsTemplate.ToString());
                this.phTemplate.Visible = this.styleInfo.IsTemplate;

                string previewUrl = PageUtils.GetSTLUrl(string.Format(@"background_tagStylePreview.aspx?PublishmentSystemID={0}&StyleID={1}&ReturnUrl={2}", base.PublishmentSystemID, this.styleInfo.StyleID, PageUtils.UrlEncode(base.GetQueryString("ReturnUrl"))));
                this.Preview.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", previewUrl));

                this.phSuccess.Visible = TagStyleUtility.IsSuccessVisible(this.styleInfo.ElementName);
                this.phFailure.Visible = TagStyleUtility.IsFailureVisible(this.styleInfo.ElementName);
                this.phStyle.Visible = TagStyleUtility.IsStyleVisible(this.styleInfo.ElementName);
                this.phScript.Visible = TagStyleUtility.IsScriptVisible(this.styleInfo.ElementName);

                if (this.styleInfo.IsTemplate)
                {
                    this.tbContent.Text = this.styleInfo.ContentTemplate;
                    if (this.phSuccess.Visible)
                    {
                        this.tbSuccess.Text = this.styleInfo.SuccessTemplate;
                    }
                    if (this.phFailure.Visible)
                    {
                        this.tbFailure.Text = this.styleInfo.FailureTemplate;
                    }
                    if (this.phStyle.Visible)
                    {
                        this.tbStyle.Text = this.styleInfo.StyleTemplate;
                    }
                    if (this.phScript.Visible)
                    {
                        this.tbScript.Text = this.styleInfo.ScriptTemplate;
                    }
                }

                if (string.IsNullOrEmpty(base.GetQueryString("ReturnUrl")))
                {
                    this.phReturn.Visible = false;
                }
			}
		}

        public void rblIsTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phTemplate.Visible = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
            if (this.phTemplate.Visible && string.IsNullOrEmpty(this.tbContent.Text))
            {
                this.cbIsCreateTemplate_CheckedChanged(sender, e);
            }
        }

        public void cbIsCreateTemplate_CheckedChanged(object sender, EventArgs e)
        {
            TagStyleUtility.IsCreateTemplate_CheckedChanged(this.styleInfo, base.PublishmentSystemInfo, this.tbContent, this.tbSuccess, this.tbFailure, this.tbStyle, this.tbScript);
            this.cbIsCreateTemplate.Checked = false;
        }


        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                this.styleInfo.IsTemplate = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
                this.styleInfo.StyleTemplate = this.tbStyle.Text;
                this.styleInfo.ScriptTemplate = this.tbScript.Text;
                this.styleInfo.ContentTemplate = this.tbContent.Text;
                this.styleInfo.SuccessTemplate = this.tbSuccess.Text;
                this.styleInfo.FailureTemplate = this.tbFailure.Text;
                
                try
                {
                    DataProvider.TagStyleDAO.Update(this.styleInfo);
                    base.SuccessMessage("模板修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "模板修改失败," + ex.Message);
                }
			}
		}
	}
}
