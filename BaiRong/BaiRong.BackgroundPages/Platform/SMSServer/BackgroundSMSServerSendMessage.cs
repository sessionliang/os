using System;
using System.Net;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections;


using BaiRong.Controls;
using System.Text;
using BaiRong.Core.Data.Provider;

namespace BaiRong.BackgroundPages
{
    public class BackgroundSMSServerSendMessage : BackgroundBasePage
    {
        public TextBox tbMobile;
        public TextBox tbMessage;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                tbMessage.Text = "短信服务商短信发送！";
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_SMS, "发送测试短信", AppManager.Platform.Permission.platform_SMS);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            ArrayList mobileArrayList = new ArrayList();

            ArrayList mobiles = TranslateUtils.StringCollectionToArrayList(this.tbMobile.Text);
            foreach (string mobile in mobiles)
            {
                if (!string.IsNullOrEmpty(mobile) && StringUtils.IsMobile(mobile) && !mobileArrayList.Contains(mobile))
                {
                    mobileArrayList.Add(mobile);
                }
            }

            if (mobileArrayList.Count > 0)
            {
                try
                {
                    string errorMessage = string.Empty;
                    bool isSuccess = SMSServerManager.Send(mobileArrayList, this.tbMessage.Text, out errorMessage);

                    if (isSuccess)
                    {
                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "发送短信", string.Format("接收号码:{0},短信内容：{1}", TranslateUtils.ObjectCollectionToString(mobileArrayList), this.tbMessage.Text));

                        base.SuccessMessage("短信发送成功！");
                    }
                    else
                    {
                        base.FailMessage("短信发送失败：" + errorMessage + "! 请检查短信服务商设置！");
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "短信发送失败：" + ex.Message + " 请与管理员联系！");
                }
            }
        }

        
    }
}
