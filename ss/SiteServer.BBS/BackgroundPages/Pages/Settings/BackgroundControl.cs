using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Web.UI;
using BaiRong.Core;
using System.Collections;
using SiteServer.BBS.Core; 

namespace SiteServer.BBS.BackgroundPages {

    public class BackgroundControl : BackgroundBasePage
    {
        public TextBox tbPostInterval;
        public RadioButtonList rblIsVerifyCodeThread;
        public RadioButtonList rblIsVerifyCodePost;
        public PlaceHolder phPostVerifyCode;
        public TextBox tbPostVerifyCodeCount;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_control.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, "防灌水设置", AppManager.BBS.Permission.BBS_Settings);

                this.tbPostInterval.Text = base.Additional.PostInterval.ToString();
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsVerifyCodeThread, base.Additional.IsVerifyCodeThread.ToString());
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsVerifyCodePost, base.Additional.IsVerifyCodePost.ToString());
                this.phPostVerifyCode.Visible = base.Additional.IsVerifyCodeThread || base.Additional.IsVerifyCodePost;
                this.tbPostVerifyCodeCount.Text = base.Additional.PostVerifyCodeCount.ToString();
            }
        }

        public void rblIsVerifyCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phPostVerifyCode.Visible = TranslateUtils.ToBool(this.rblIsVerifyCodeThread.SelectedValue) || TranslateUtils.ToBool(this.rblIsVerifyCodePost.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                base.Additional.PostInterval = TranslateUtils.ToInt(this.tbPostInterval.Text);
                base.Additional.IsVerifyCodeThread = TranslateUtils.ToBool(this.rblIsVerifyCodeThread.SelectedValue);
                base.Additional.IsVerifyCodePost = TranslateUtils.ToBool(this.rblIsVerifyCodePost.SelectedValue);
                base.Additional.PostVerifyCodeCount = TranslateUtils.ToInt(this.tbPostVerifyCodeCount.Text);

                try
                {
                    ConfigurationManager.Update(base.PublishmentSystemID);

                    base.SuccessMessage("防灌水设置修改成功！");
                    base.AddWaitAndRedirectScript(BackgroundControl.GetRedirectUrl(base.PublishmentSystemID));
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "防灌水设置修改失败！");
                }
            }
        }
    }
}
