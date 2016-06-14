using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Web.UI;
using BaiRong.Core;
using System.Collections;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundConfigurationTemplate : BackgroundBasePage
    {
        public RadioButtonList IsCreateDoubleClick;

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, "模板生成设置", AppManager.BBS.Permission.BBS_Settings);

                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateDoubleClick, base.Additional.IsCreateDoubleClick.ToString());
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                base.Additional.IsCreateDoubleClick = TranslateUtils.ToBool(this.IsCreateDoubleClick.SelectedValue);

                try
                {
                    ConfigurationManager.Update(base.PublishmentSystemID);

                    base.SuccessMessage("模板生成设置修改成功！");
                    base.AddWaitAndRedirectScript(BackgroundBBSInfo.GetRedirectUrl(base.PublishmentSystemID));
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "模板生成设置修改失败！");
                }
            }
        }
    }
}