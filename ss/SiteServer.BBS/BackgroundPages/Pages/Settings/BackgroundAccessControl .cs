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
    public class BackgroundAccessControl : BackgroundBasePage
    {
        public TextBox txtNovitiateByMinute;
        public TextBox txtForbiddenAccessTime;
        public TextBox txtForbiddenPostTime;

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, "访问控制", AppManager.BBS.Permission.BBS_Settings);

                this.txtNovitiateByMinute.Text = base.Additional.NovitiateByMinute.ToString();
                this.txtForbiddenAccessTime.Text = base.Additional.ForbiddenAccessTime.ToString().Replace("|", "\r\n");
                this.txtForbiddenPostTime.Text = base.Additional.ForbiddenPostTime.ToString().Replace("|", "\r\n");
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                string inputText = "";
                base.Additional.NovitiateByMinute = TranslateUtils.ToInt(this.txtNovitiateByMinute.Text.Trim());
                if (!string.IsNullOrEmpty(this.txtForbiddenAccessTime.Text))
                {
                    inputText = this.txtForbiddenAccessTime.Text.Trim().Replace("\r\n", "|");
                    base.Additional.ForbiddenAccessTime = inputText;
                }
                if (!string.IsNullOrEmpty(this.txtForbiddenPostTime.Text))
                {
                    inputText = this.txtForbiddenPostTime.Text.Trim().Replace("\r\n", "|");
                    base.Additional.ForbiddenPostTime = inputText;
                }
                try
                {
                    ConfigurationManager.Update(base.PublishmentSystemID);

                    base.SuccessMessage("访问控制修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "访问控制修改失败！");
                }
            }
        }
    }
}
