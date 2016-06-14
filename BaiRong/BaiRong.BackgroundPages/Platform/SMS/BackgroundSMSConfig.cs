using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Cryptography;
using BaiRong.Core;

namespace BaiRong.BackgroundPages
{
    public class BackgroundSMSConfig : BackgroundBasePage
    {
        public TextBox tbSMSAccount;
        public TextBox tbSMSPassword;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_SMS, "手机短信设置", AppManager.Platform.Permission.platform_SMS);

                this.tbSMSAccount.Text = ConfigManager.Additional.SMSAccount;
                this.tbSMSPassword.Text = ConfigManager.Additional.SMSPassword;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            try
            {
                ConfigManager.Additional.SMSAccount = this.tbSMSAccount.Text;
                ConfigManager.Additional.SMSPassword = this.tbSMSPassword.Text;
                ConfigManager.Additional.SMSMD5String = EncryptUtils.Md5(ConfigManager.Additional.SMSPassword);

                BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);

                int totalCount = 0;
                string errorMessage = string.Empty;
                bool isSuccess = SMSManager.GetTotalCount(out totalCount, out errorMessage);

                if (isSuccess)
                {
                    base.SuccessMessage(string.Format("手机短信设置成功，当前剩余{0}条短信", totalCount));
                }
                else
                {
                    base.FailMessage(string.Format("手机短信设置失败,{0}!", errorMessage));
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, string.Format("手机短信设置失败,{0}", ex.Message));
            }
        }
    }
}
