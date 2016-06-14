using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace BaiRong.BackgroundPages
{
    public class BackgroundSMSServerConfiguration : BackgroundBasePage
    {
        public TextBox tbSMSServerName;
        public Literal ltlSMSServerType;

        public PlaceHolder phYunPian;
        public TextBox tbYunPianKey;

        public PlaceHolder phWeiMi;
        public TextBox tbUID;
        public TextBox tbUPASS;

        private int SMSServerID;

        public static string GetRedirectUrl(int SMSServerID)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_SMSServerConfiguration.aspx?SMSServerID={0}", SMSServerID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.SMSServerID = base.GetIntQueryString("SMSServerID");

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.Platform.LeftMenu.ID_SMS, "短信服务商管理", AppManager.Platform.Permission.platform_SMS);

                if (this.SMSServerID > 0)
                {
                    SMSServerInfo smsServerInfo = BaiRongDataProvider.SMSServerDAO.GetSMSServerInfo(this.SMSServerID);
                    if (smsServerInfo != null)
                    {

                        this.tbSMSServerName.Text = smsServerInfo.SMSServerName;
                        this.ltlSMSServerType.Text = ESMSServerTypeUtils.GetText(ESMSServerTypeUtils.GetEnumType(smsServerInfo.SMSServerEName));

                        if (smsServerInfo.SMSServerEName == ESMSServerType.YunPian.ToString())
                        {
                            this.phYunPian.Visible = true;
                            this.tbYunPianKey.Text = smsServerInfo.ParamCollection["apikey"];
                        }
                        else if (smsServerInfo.SMSServerEName == ESMSServerType.WeiMi.ToString())
                        {
                            this.phWeiMi.Visible = true;
                            this.tbUID.Text = smsServerInfo.ParamCollection["uid"];
                            this.tbUPASS.Text = smsServerInfo.ParamCollection["pas"];
                        }
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                SMSServerInfo smsServerInfo = new SMSServerInfo();
                if (this.SMSServerID > 0)
                {
                    smsServerInfo = BaiRongDataProvider.SMSServerDAO.GetSMSServerInfo(this.SMSServerID);
                }

                smsServerInfo.SMSServerName = this.tbSMSServerName.Text;

                if (smsServerInfo.SMSServerEName == ESMSServerType.YunPian.ToString())
                {
                    smsServerInfo.ParamCollection.Remove("apikey");
                    smsServerInfo.ParamCollection.Add("apikey", this.tbYunPianKey.Text);
                }
                else if (smsServerInfo.SMSServerEName == ESMSServerType.WeiMi.ToString())
                {
                    smsServerInfo.ParamCollection.Remove("uid");
                    smsServerInfo.ParamCollection.Add("uid", this.tbUID.Text);
                    smsServerInfo.ParamCollection.Remove("pas");
                    smsServerInfo.ParamCollection.Add("pas", this.tbUPASS.Text);
                }


                if (this.SMSServerID > 0)
                {
                    BaiRongDataProvider.SMSServerDAO.Update(smsServerInfo);
                }
                else
                {
                    BaiRongDataProvider.SMSServerDAO.Insert(smsServerInfo);
                }
                SMSServerManager.IsChange = true;
                base.SuccessMessage("配置短信服务商成功！");

                base.AddWaitAndRedirectScript(BackgroundSMSServerInit.GetRedirectUrl());
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
