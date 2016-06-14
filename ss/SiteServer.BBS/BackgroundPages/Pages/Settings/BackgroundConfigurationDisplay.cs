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
    public class BackgroundConfigurationDisplay : BackgroundBasePage
    {

        public RadioButtonList rblDisplayFullScreen;
        public DropDownList ddlDisplayColumns;
        public TextBox tbThreadPageNum;
        public TextBox tbPostPageNum;
        public TextBox tbOnlineTimeout;
        public RadioButtonList rblIsOnlineInIndexPage;
        public RadioButtonList rblIsOnlineUserOnly;
        public TextBox tbOnlineMaxInIndexPage;

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, "界面设置", AppManager.BBS.Permission.BBS_Settings);

                ControlUtils.SelectListItemsIgnoreCase(this.rblDisplayFullScreen, base.Additional.IsFullScreen.ToString());
                ControlUtils.SelectListItemsIgnoreCase(this.ddlDisplayColumns, base.Additional.DisplayColumns.ToString());
                this.tbThreadPageNum.Text = base.Additional.ThreadPageNum.ToString();
                this.tbPostPageNum.Text = base.Additional.PostPageNum.ToString();
                this.tbOnlineTimeout.Text = base.Additional.OnlineTimeout.ToString();
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsOnlineInIndexPage, base.Additional.IsOnlineInIndexPage.ToString());
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsOnlineUserOnly, base.Additional.IsOnlineUserOnly.ToString());
                this.tbOnlineMaxInIndexPage.Text = base.Additional.OnlineMaxInIndexPage.ToString();
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                base.Additional.IsFullScreen = TranslateUtils.ToBool(this.rblDisplayFullScreen.SelectedValue, base.Additional.IsFullScreen);
                base.Additional.DisplayColumns = TranslateUtils.ToInt(this.ddlDisplayColumns.SelectedValue, base.Additional.DisplayColumns);
                base.Additional.ThreadPageNum = TranslateUtils.ToInt(this.tbThreadPageNum.Text, base.Additional.ThreadPageNum);
                base.Additional.PostPageNum = TranslateUtils.ToInt(this.tbPostPageNum.Text, base.Additional.PostPageNum);
                base.Additional.OnlineTimeout = TranslateUtils.ToInt(this.tbOnlineTimeout.Text, base.Additional.OnlineTimeout);

                base.Additional.IsOnlineInIndexPage = TranslateUtils.ToBool(this.rblIsOnlineInIndexPage.SelectedValue, base.Additional.IsOnlineInIndexPage);
                base.Additional.IsOnlineUserOnly = TranslateUtils.ToBool(this.rblIsOnlineUserOnly.SelectedValue, base.Additional.IsOnlineUserOnly);
                base.Additional.OnlineMaxInIndexPage = TranslateUtils.ToInt(this.tbOnlineMaxInIndexPage.Text, base.Additional.OnlineMaxInIndexPage);

                try
                {
                    ConfigurationManager.Update(base.PublishmentSystemID);

                    base.SuccessMessage("界面设置修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "界面设置修改失败！");
                }
            }
        }
    }
}
