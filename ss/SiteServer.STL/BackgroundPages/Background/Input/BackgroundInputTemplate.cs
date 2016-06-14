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

using SiteServer.STL.StlTemplate;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundInputTemplate : BackgroundBasePage
	{
        public Literal ltlInputName;
        public Literal ltlElement;
        public RadioButtonList rblIsTemplate;

        public PlaceHolder phTemplate;
        public CheckBox IsCreateTemplate;
        public TextBox Content;
        public TextBox Style;
        public TextBox Script;

        public Button Preview;

        private InputInfo inputInfo;
        public int GetItemID()
        {
            return this.inputInfo.ClassifyID;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("InputID");

            int inputID = base.GetIntQueryString("InputID");
            this.inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Input, "自定义模板", AppManager.CMS.Permission.WebSite.Input);

                this.ltlInputName.Text = this.inputInfo.InputName;
                this.ltlElement.Text = string.Format(@"
&lt;stl:input inputName=&quot;{0}&quot;&gt;&lt;/stl:input&gt;", this.inputInfo.InputName);

                EBooleanUtils.AddListItems(this.rblIsTemplate, "自定义模板", "默认模板");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsTemplate, this.inputInfo.IsTemplate.ToString());
                this.phTemplate.Visible = this.inputInfo.IsTemplate;

                string previewUrl = PageUtils.GetSTLUrl(string.Format("background_inputPreview.aspx?PublishmentSystemID={0}&InputID={1}&ReturnUrl={2}", base.PublishmentSystemID, this.inputInfo.InputID, PageUtils.UrlEncode(base.GetQueryString("ReturnUrl"))));
                this.Preview.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", previewUrl));

                if (this.inputInfo.IsTemplate)
                {
                    this.Style.Text = this.inputInfo.StyleTemplate;
                    this.Script.Text = this.inputInfo.ScriptTemplate;
                    this.Content.Text = this.inputInfo.ContentTemplate;
                }
			}
		}

        public void rblIsTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phTemplate.Visible = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
            if (this.phTemplate.Visible && string.IsNullOrEmpty(this.Content.Text))
            {
                this.IsCreateTemplate_CheckedChanged(sender, e);
            }
        }

        public void IsCreateTemplate_CheckedChanged(object sender, EventArgs e)
        {
            InputTemplate inputTemplate = new InputTemplate(base.PublishmentSystemInfo, this.inputInfo);
            this.Content.Text = inputTemplate.GetContent();
            this.Style.Text = inputTemplate.GetStyle(ETableStyle.InputContent);
            this.Script.Text = inputTemplate.GetScript();

            this.IsCreateTemplate.Checked = false;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                this.inputInfo.IsTemplate = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
                this.inputInfo.StyleTemplate = this.Style.Text;
                this.inputInfo.ScriptTemplate = this.Script.Text;
                this.inputInfo.ContentTemplate = this.Content.Text;

                try
                {
                    DataProvider.InputDAO.Update(this.inputInfo);
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
