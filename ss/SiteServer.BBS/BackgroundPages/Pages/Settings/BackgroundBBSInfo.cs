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
    public class BackgroundBBSInfo : BackgroundBasePage
    {
        public PlaceHolder phCheck;
        public TextBox txtBBSName;
        public TextBox txtSiteName;
        public TextBox txtSiteUrl;
        public TextBox txtAdminEmail;
        public TextBox txtCountCode;
        public TextBox txtCloseBBSReason;
        public RadioButtonList IsCloseBBS;
        public RadioButtonList IsLogBBS;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_bbsInfo.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, "论坛信息", AppManager.BBS.Permission.BBS_Settings);

                ControlUtils.SelectListItemsIgnoreCase(this.IsCloseBBS, base.Additional.IsCloseBBS.ToString());
                ControlUtils.SelectListItemsIgnoreCase(this.IsLogBBS, base.Additional.IsLogBBS.ToString());
                this.phCheck.Visible = base.Additional.IsCloseBBS;
                this.txtBBSName.Text = base.Additional.BBSName.ToString();
                this.txtSiteName.Text = base.Additional.SiteName.ToString();
                this.txtSiteUrl.Text = base.Additional.SiteUrl.ToString();
                this.txtAdminEmail.Text = base.Additional.AdminEmail.ToString();
                this.txtCountCode.Text = base.Additional.CountCode.ToString();
                this.txtCloseBBSReason.Text = base.Additional.CloseBBSReason.ToString();
            }
        }

        public void IsCloseBBS_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phCheck.Visible = !this.phCheck.Visible;
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                base.Additional.BBSName = this.txtBBSName.Text.Trim();
                base.Additional.SiteName = this.txtSiteName.Text.Trim();
                base.Additional.SiteUrl = this.txtSiteUrl.Text.Trim();
                base.Additional.AdminEmail = this.txtAdminEmail.Text.Trim();
                base.Additional.CountCode = this.txtCountCode.Text.Trim();
                base.Additional.IsCloseBBS = TranslateUtils.ToBool(this.IsCloseBBS.SelectedValue);
                base.Additional.CloseBBSReason = this.txtCloseBBSReason.Text.Trim();
                base.Additional.IsLogBBS = TranslateUtils.ToBool(this.IsLogBBS.SelectedValue);

                try
                {
                    ConfigurationManager.Update(base.PublishmentSystemID);

                    base.SuccessMessage("论坛信息修改成功！");
                    base.AddWaitAndRedirectScript(BackgroundBBSInfo.GetRedirectUrl(base.PublishmentSystemID));
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "论坛信息修改失败！");
                }
            }
        }
    }
}