using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System.Collections;
using BaiRong.Controls;
using SiteServer.CMS.Model;

using SiteServer.CMS.BackgroundPages;
using BaiRong.Core.Integration;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundConfigurationZDH : BackgroundBasePage
    {
        public RadioButtonList rblIsEnabled;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, "百度直达号设置", AppManager.CMS.Permission.WebSite.Configration);

                EBooleanUtils.AddListItems(this.rblIsEnabled, "启用直达号", "禁用直达号");
                ControlUtils.SelectListItems(this.rblIsEnabled, base.PublishmentSystemInfo.Additional.ZDH_IsEnabled.ToString());

            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                base.PublishmentSystemInfo.Additional.ZDH_IsEnabled = TranslateUtils.ToBool(this.rblIsEnabled.SelectedValue);

                try
                {
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                    base.SuccessMessage("百度直达号设置修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "百度直达号设置修改失败！");
                }
            }
        }
    }
}
