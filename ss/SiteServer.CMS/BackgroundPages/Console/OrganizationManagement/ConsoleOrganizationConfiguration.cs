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

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundConfiguration : BackgroundBasePage
    {
        protected TextBox OrganizationBaiduAK;
        public RadioButtonList OrganizationIsCrossDomain;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Organization, "分支机构设置", AppManager.Platform.Permission.Platform_Organization);

                EBooleanUtils.AddListItems(this.OrganizationIsCrossDomain, "启用", "不启用");
                ControlUtils.SelectListItemsIgnoreCase(this.OrganizationIsCrossDomain, ConfigManager.Instance.Additional.OrganizationIsCrossDomain.ToString());

                this.OrganizationBaiduAK.Text = ConfigManager.Instance.Additional.OrganizationBaiduAK;

            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            try
            {
                ConfigManager.Instance.Additional.OrganizationBaiduAK = PageUtils.FilterXSS(this.OrganizationBaiduAK.Text.Trim());
                ConfigManager.Instance.Additional.OrganizationIsCrossDomain = TranslateUtils.ToBool(this.OrganizationIsCrossDomain.SelectedValue);
                BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "分支机构设置");
                base.SuccessMessage("分支机构设置成功");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
