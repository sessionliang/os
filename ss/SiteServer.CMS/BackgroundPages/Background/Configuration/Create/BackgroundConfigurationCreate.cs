using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System.Collections;
using BaiRong.Controls;
using SiteServer.CMS.Model;
using System.Web.UI.HtmlControls;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundConfigurationCreate : BackgroundBasePage
	{
        public RadioButtonList IsCreateRedirectPage;

        public RadioButtonList IsCreateContentIfContentChanged;
        public RadioButtonList IsCreateChannelIfChannelChanged;

        public RadioButtonList IsCreateShowPageInfo;

        public RadioButtonList IsCreateIE8Compatible;
        public RadioButtonList IsCreateBrowserNoCache;
        public RadioButtonList IsCreateJsIgnoreError;
        public RadioButtonList IsCreateSearchDuplicate;

        public RadioButtonList IsCreateIncludeToSSI;
        public RadioButtonList IsCreateWithJQuery;

        public TextBox tbIISDefaultPage;

        public RadioButtonList IsCreateDoubleClick;
        public TextBox tbCreateStaticMaxPage;


        public RadioButtonList IsCreateChannelsByChildNodeID;
        public TextBox tbScope;
        public PlaceHolder phScoap;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, AppManager.CMS.LeftMenu.Configuration.ID_ConfigurationCreate, "页面生成设置", AppManager.CMS.Permission.WebSite.Configration);

                this.tbScope.Text = base.PublishmentSystemInfo.Additional.CreateScopeByChildNodeID.ToString();

				EBooleanUtils.AddListItems(this.IsCreateRedirectPage, "仅浏览时生成页面", "生成页面");
				ControlUtils.SelectListItemsIgnoreCase(this.IsCreateRedirectPage, base.PublishmentSystemInfo.Additional.IsCreateRedirectPage.ToString());

                EBooleanUtils.AddListItems(this.IsCreateContentIfContentChanged, "生成", "不生成");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateContentIfContentChanged, base.PublishmentSystemInfo.Additional.IsCreateContentIfContentChanged.ToString());

                EBooleanUtils.AddListItems(this.IsCreateChannelIfChannelChanged, "生成", "不生成");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateChannelIfChannelChanged, base.PublishmentSystemInfo.Additional.IsCreateChannelIfChannelChanged.ToString());

                EBooleanUtils.AddListItems(this.IsCreateChannelsByChildNodeID, "生成", "不生成");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateChannelsByChildNodeID, base.PublishmentSystemInfo.Additional.IsCreateChannelsByChildNodeID.ToString());

                EBooleanUtils.AddListItems(this.IsCreateShowPageInfo, "显示", "不显示");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateShowPageInfo, base.PublishmentSystemInfo.Additional.IsCreateShowPageInfo.ToString());

                EBooleanUtils.AddListItems(this.IsCreateIE8Compatible, "强制兼容", "不设置");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateIE8Compatible, base.PublishmentSystemInfo.Additional.IsCreateIE8Compatible.ToString());

                EBooleanUtils.AddListItems(this.IsCreateBrowserNoCache, "强制清除缓存", "不设置");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateBrowserNoCache, base.PublishmentSystemInfo.Additional.IsCreateBrowserNoCache.ToString());

                EBooleanUtils.AddListItems(this.IsCreateJsIgnoreError, "包含JS容错代码", "不设置");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateJsIgnoreError, base.PublishmentSystemInfo.Additional.IsCreateJsIgnoreError.ToString());

                EBooleanUtils.AddListItems(this.IsCreateSearchDuplicate, "包含", "不包含");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateSearchDuplicate, base.PublishmentSystemInfo.Additional.IsCreateSearchDuplicate.ToString());

                EBooleanUtils.AddListItems(this.IsCreateWithJQuery, "是", "否");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateWithJQuery, base.PublishmentSystemInfo.Additional.IsCreateWithJQuery.ToString());

                EBooleanUtils.AddListItems(this.IsCreateIncludeToSSI, "是", "否");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateIncludeToSSI, base.PublishmentSystemInfo.Additional.IsCreateIncludeToSSI.ToString());

                this.tbIISDefaultPage.Text = base.PublishmentSystemInfo.Additional.IISDefaultPage;

                EBooleanUtils.AddListItems(this.IsCreateDoubleClick, "启用双击生成", "不启用");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateDoubleClick, base.PublishmentSystemInfo.Additional.IsCreateDoubleClick.ToString());

                this.tbCreateStaticMaxPage.Text = base.PublishmentSystemInfo.Additional.CreateStaticMaxPage.ToString();

                if (TranslateUtils.ToBool(this.IsCreateChannelsByChildNodeID.SelectedValue))
                {
                    this.phScoap.Visible = true;
                }
                else
                {
                    this.phScoap.Visible = false;
                }
			}
		}

        public void IsCreateChannelsByChildNodeID_OnSelectedIndexChanged(object sender, EventArgs E)
        {
            if (TranslateUtils.ToBool(this.IsCreateChannelsByChildNodeID.SelectedValue))
            {
                this.phScoap.Visible = true;
            }
            else
            {
                this.phScoap.Visible = false;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				base.PublishmentSystemInfo.Additional.IsCreateRedirectPage = TranslateUtils.ToBool(this.IsCreateRedirectPage.SelectedValue);

                base.PublishmentSystemInfo.Additional.IsCreateContentIfContentChanged = TranslateUtils.ToBool(this.IsCreateContentIfContentChanged.SelectedValue);
                base.PublishmentSystemInfo.Additional.IsCreateChannelIfChannelChanged = TranslateUtils.ToBool(this.IsCreateChannelIfChannelChanged.SelectedValue);
                base.PublishmentSystemInfo.Additional.IsCreateChannelsByChildNodeID = TranslateUtils.ToBool(this.IsCreateChannelsByChildNodeID.SelectedValue);
                base.PublishmentSystemInfo.Additional.CreateScopeByChildNodeID = TranslateUtils.ToInt(this.tbScope.Text);

                base.PublishmentSystemInfo.Additional.IsCreateShowPageInfo = TranslateUtils.ToBool(this.IsCreateShowPageInfo.SelectedValue);
                base.PublishmentSystemInfo.Additional.IsCreateIE8Compatible = TranslateUtils.ToBool(this.IsCreateIE8Compatible.SelectedValue);
                base.PublishmentSystemInfo.Additional.IsCreateBrowserNoCache = TranslateUtils.ToBool(this.IsCreateBrowserNoCache.SelectedValue);
                base.PublishmentSystemInfo.Additional.IsCreateJsIgnoreError = TranslateUtils.ToBool(this.IsCreateJsIgnoreError.SelectedValue);
                base.PublishmentSystemInfo.Additional.IsCreateSearchDuplicate = TranslateUtils.ToBool(this.IsCreateSearchDuplicate.SelectedValue);
                base.PublishmentSystemInfo.Additional.IsCreateIncludeToSSI = TranslateUtils.ToBool(this.IsCreateIncludeToSSI.SelectedValue);
                base.PublishmentSystemInfo.Additional.IsCreateWithJQuery = TranslateUtils.ToBool(this.IsCreateWithJQuery.SelectedValue);

                if (this.tbIISDefaultPage.Text.IndexOf(".") != -1)
                {
                    base.PublishmentSystemInfo.Additional.IISDefaultPage = this.tbIISDefaultPage.Text;
                }

                base.PublishmentSystemInfo.Additional.IsCreateDoubleClick = TranslateUtils.ToBool(this.IsCreateDoubleClick.SelectedValue);
                base.PublishmentSystemInfo.Additional.CreateStaticMaxPage = TranslateUtils.ToInt(this.tbCreateStaticMaxPage.Text);
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改页面生成设置");

                    base.SuccessMessage("页面生成设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "页面生成设置修改失败！");
				}
			}
		}
	}
}
