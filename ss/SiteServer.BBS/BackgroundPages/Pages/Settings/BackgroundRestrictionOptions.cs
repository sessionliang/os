using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;


using SiteServer.BBS.Core;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundRestrictionOptions : BackgroundBasePage
	{
        public RadioButtonList RestrictionType;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, "访问限制选项", AppManager.BBS.Permission.BBS_Settings);

                ERestrictionTypeUtils.AddListItems(this.RestrictionType);
                ControlUtils.SelectListItemsIgnoreCase(this.RestrictionType, ERestrictionTypeUtils.GetValue(base.Additional.RestrictionType));
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                base.Additional.RestrictionType = ERestrictionTypeUtils.GetEnumType(this.RestrictionType.SelectedValue);

                try
                {
                    ConfigurationManager.Update(base.PublishmentSystemID);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "设置访问限制选项");

                    base.SuccessMessage("访问限制选项修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "访问限制选项修改失败！");
                }
            }
        }
	}
}
