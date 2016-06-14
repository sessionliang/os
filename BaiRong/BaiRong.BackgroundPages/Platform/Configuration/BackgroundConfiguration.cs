using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;

using BaiRong.Core.Data.Provider;

using BaiRong.Core.Configuration;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.OracleClient;
using BaiRong.Core.Data;
using BaiRong.Core.Service;

namespace BaiRong.BackgroundPages
{
    public class BackgroundConfiguration : BackgroundBasePage
    {
        protected RadioButtonList rblIsRelatedUrl;
        public PlaceHolder phRootUrl;
        protected TextBox tbRootUrl;
        public RadioButtonList IsUseAjaxCreatePage;
        public RadioButtonList IsFilterXss;
        public RadioButtonList isSiteServerServiceCreate;
        public RadioButtonList rblIsViewContentOnlySelf;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Configuration, "平台参数设置", AppManager.Platform.Permission.Platform_Configuration);

                EBooleanUtils.AddListItems(this.rblIsRelatedUrl, "相对路径", "绝对路径");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsRelatedUrl, ConfigManager.Instance.IsRelatedUrl.ToString());
                this.tbRootUrl.Text = ConfigManager.Instance.RootUrl;

                EBooleanUtils.AddListItems(this.IsUseAjaxCreatePage, "启用异步生成", "不启用");
                ControlUtils.SelectListItemsIgnoreCase(this.IsUseAjaxCreatePage, ConfigManager.Instance.Additional.IsUseAjaxCreatePage.ToString());

                EBooleanUtils.AddListItems(this.IsFilterXss, "启用Xss过滤", "不启用");
                ControlUtils.SelectListItemsIgnoreCase(this.IsFilterXss, ConfigManager.Instance.Additional.IsFilterXss.ToString());

                EBooleanUtils.AddListItems(this.isSiteServerServiceCreate, "启用SiteServer服务组件生成", "不启用");
                ControlUtils.SelectListItemsIgnoreCase(this.isSiteServerServiceCreate, ConfigManager.Instance.Additional.IsSiteServerServiceCreate.ToString());

                EBooleanUtils.AddListItems(this.rblIsViewContentOnlySelf, "不可以", "可以");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsViewContentOnlySelf, ConfigManager.Instance.Additional.IsViewContentOnlySelf.ToString());

                this.rblIsRelatedUrl_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        public void rblIsRelatedUrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phRootUrl.Visible = !TranslateUtils.ToBool(this.rblIsRelatedUrl.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            try
            {
                ConfigManager.Instance.IsRelatedUrl = TranslateUtils.ToBool(this.rblIsRelatedUrl.SelectedValue);
                if (ConfigManager.Instance.IsRelatedUrl == false)
                {
                    this.tbRootUrl.Text = PageUtils.AddProtocolToUrl(this.tbRootUrl.Text);
                    this.tbRootUrl.Text = this.tbRootUrl.Text.TrimEnd(new char[] { '/', '\\', ' ' });
                    ConfigManager.Instance.RootUrl = this.tbRootUrl.Text;
                }

                ConfigManager.Instance.Additional.IsUseAjaxCreatePage = TranslateUtils.ToBool(this.IsUseAjaxCreatePage.SelectedValue);
                ConfigManager.Instance.Additional.IsFilterXss = TranslateUtils.ToBool(this.IsFilterXss.SelectedValue);
                ConfigManager.Instance.Additional.IsSiteServerServiceCreate = TranslateUtils.ToBool(this.isSiteServerServiceCreate.SelectedValue);
                ConfigManager.Instance.Additional.IsViewContentOnlySelf = TranslateUtils.ToBool(this.rblIsViewContentOnlySelf.SelectedValue);
                BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "设置平台参数");
                base.SuccessMessage("平台参数设置成功");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        protected void isSiteServerServiceCreate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TranslateUtils.ToBool(this.isSiteServerServiceCreate.SelectedValue))
            {
                //开启的时候，检测服务组件是否启用
                DateTime dateTime = DateTime.MinValue;
                bool isOk = CacheManager.IsServiceOnline(out dateTime);
                if (isOk)
                {
                    TimeSpan ts = DateTime.Now - dateTime;
                    if (ts.TotalMinutes > CacheManager.Slide_Minutes_Status * 2)
                    {
                        isOk = false;
                    }
                }
                if (!isOk)
                {
                    base.FailMessage("SiteServer Service 服务组件未安装或未启动");
                    this.isSiteServerServiceCreate.SelectedValue = false.ToString();
                }
            }
        }
    }
}
