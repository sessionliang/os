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

namespace BaiRong.BackgroundPages
{
    public class BackgroundConfigurationLog : BackgroundBasePage
    {
        protected RadioButtonList rblIsTimeThreshold;
        public PlaceHolder phTimeThreshold;
        protected TextBox tbTime;
        public RadioButtonList rblIsCounterThreshold;
        public PlaceHolder phCounterThreshold;
        protected TextBox tbCounter;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Configuration, "平台参数设置", AppManager.Platform.Permission.Platform_Configuration);

                EBooleanUtils.AddListItems(this.rblIsTimeThreshold, "启用", "不启用");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsTimeThreshold, ConfigManager.Additional.IsTimeThreshold.ToString());
                this.tbTime.Text = ConfigManager.Additional.TimeThreshold.ToString();

                EBooleanUtils.AddListItems(this.rblIsCounterThreshold, "启用", "不启用");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsCounterThreshold, ConfigManager.Additional.IsCounterThreshold.ToString());
                this.tbCounter.Text = ConfigManager.Additional.CounterThreshold.ToString();

                this.rblIsTimeThreshold_SelectedIndexChanged(null, EventArgs.Empty);
                this.rblIsCounterThreshold_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        public void rblIsTimeThreshold_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phTimeThreshold.Visible = TranslateUtils.ToBool(this.rblIsTimeThreshold.SelectedValue);
        }

        public void rblIsCounterThreshold_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phCounterThreshold.Visible = TranslateUtils.ToBool(this.rblIsCounterThreshold.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            try
            {
                ConfigManager.Additional.IsTimeThreshold = TranslateUtils.ToBool(this.rblIsTimeThreshold.SelectedValue);
                if (ConfigManager.Additional.IsTimeThreshold)
                {
                    ConfigManager.Additional.TimeThreshold = TranslateUtils.ToInt(this.tbTime.Text);
                }

                ConfigManager.Additional.IsCounterThreshold = TranslateUtils.ToBool(this.rblIsCounterThreshold.SelectedValue);
                if (ConfigManager.Additional.IsCounterThreshold)
                {
                    ConfigManager.Additional.CounterThreshold = TranslateUtils.ToInt(this.tbCounter.Text);
                }

                BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "设置日志阈值参数");
                base.SuccessMessage("日志阈值参数设置成功");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
